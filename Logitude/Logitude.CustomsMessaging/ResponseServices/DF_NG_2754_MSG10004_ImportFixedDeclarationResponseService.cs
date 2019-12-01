using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Def.Messaging.Customs;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.Common.Gen;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.ResponseServices.DeclarationErrorPointer;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnifreightIIG.Common.ImportDeclarationServiceReference;
using UnifreightIIG.Common.MessageLib.Collateral;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer.DBWCO;
using Logitude.Customs.BL.BL;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class DF_NG_2754_MSG10004_ImportFixedDeclarationResponseService :
        ResponseServiceBase<INF_MSG_GenericResponseData, DF_NG_2754_MSG10004_ImportDeclarationResponse, GenericRequestParams>
    {
        DeclarationPM _MyDeclarationPM;
        private bool _FastDelete;
        //private List<SupplierInvoiceItemsTaxesModPM> _SupplierInvoiceItemsTaxesModificationPMList;
        //public UnifreightIIG.Common.CommonIIGInterface.IResponseHeaderOrFault _ResponseHeaderExeption;
        public bool _IsSubmitDeclarationResponse { get; set; }
        public bool _IsRetrieveDeclarationResponse { get; set; }
        decimal? totGeneralTaxCalc = 0;
        decimal? totPurchaseCalc = 0;
        decimal? totVatCalc = 0;
        decimal? generalTax = 0;
        decimal? purchase = 0;
        decimal? vat = 0;
        private ICustomContext _context;

        DeclarationError _MyDeclarationError;
        decimal? _TotalBtlCoverageNISSum = 0;

        public override void OnRequestFail(DF_NG_2754_MSG10004_ImportDeclarationResponse customResponse, GenericRequestParams requestParams)
        {
            if (!String.IsNullOrWhiteSpace(requestParams.AppicationId))
            {
                CalculateDeclarationCourierStatus.UpdateCourierDeclarationStatusCode(requestParams.Tenant, requestParams.AppicationId);
            }
            base.OnRequestFail(customResponse, requestParams);
        }

        public override INF_MSG_GenericResponseData GetResponse(
            DF_NG_2754_MSG10004_ImportDeclarationResponse customResponse, GenericRequestParams requestParams)
        {

            /// itzik test     TestTrans(requestParams);
            return this.MyResponseData;
        }

        private void TestTrans(GenericRequestParams requestParams) /// itzik test 
        {
            DeclarationStatusRequestParams searchParams = new DeclarationStatusRequestParams()
            {
                LoggingEnabled = true,

                CustomFileNo = _MyDeclarationPM.CustomFileNo,
                DeclarationNumber = _MyDeclarationPM.DeclarationNumber,
                Tenant = requestParams.Tenant,
                RequestName = "Declaration Status Search",
                ResponseName = "Declaration Status Search",
                SuppressSplitWR = true
            };

            searchParams.RequestVIA = SendRequestVIA.WebServiceInteractive;
            var resData = Logitude.CustomsMessaging.MessagingServices.DF_NG_8250_Web01_DeclarationStatus_RequestMessagingService.SendInteractive(searchParams);
            if (!resData.Succeeded)
            {
                LogMessagingUtil.Instance.AppendLine("Sending Declaration Status Request Failed " + resData.CustomsRequestsSheetId + ", Message: " + resData.UserMessage);
                ///return;
            }
            else
            {
                LogMessagingUtil.Instance.AppendLine("Request Succeeded " + resData.CustomsRequestsSheetId);
            }

        }

        private string GetValueIDType<T>(T codeType)  where T : IDType
        {
            if (codeType != null)
                return   codeType.Value;
            return null;
        }

        private string GetValueCodeType<T>(T codeType) where T : CodeType
        {
            if (codeType != null)
                return codeType.Value;
            return null;
        }


        private string GetValueTextType<T>(T codeType) where T : TextType
        {
            if (codeType != null)
                return codeType.Value;
            return null;
        }
        public void MapResponseToDeclaration(UnifreightIIG.Common.ImportDeclarationServiceReference.Declaration declaration, int tenant)
        {
            var context = CustomContext.GetContext(tenant);
 

            DeclarationPM declarationPM = new DeclarationPM()
            {
                ChangeSetOp = ChangeSetOperation.Insert,
                DeclarationNumber = GetValueIDType(declaration.ID),
                DeclarationOfficeCode = GetValueIDType(declaration.DeclarationOfficeID),
      
            
                //AdditionalDocument ********************
                AgentId = GetAgent(declaration),

                Consignments = GetConsignments(declaration),
                //= new List<ConsignmentPM> () {
                //    new ConsignmentPM() {

                //}
                //} *****************
                //   ProcedureCurrentCode = GetProcedureCurrentCode(declaration) ,
                //ImportersCheck ***************
                // ImporterTypeCode =declaration.Importer.where
                //importer************

                SupplierInvoices = GetSupplierInvoices(declaration,tenant,  context)
                //GetDeclarationConsignment
                //declarationGoodsShipment.AdditionalDocument
            };


            if (declaration.DMExtensions != null)
            {
                declarationPM.CustomFileNo = GetValueIDType(declaration.DMExtensions.AgentFileReferenceID);
                declarationPM.ExternalDeclarationNumber = GetValueIDType(declaration.DMExtensions.ExternalDeclarationID);
                declarationPM.TaxationDateTime = Convert.ToDateTime(declaration.DMExtensions.TaxationDateTime);
                declarationPM.AutonomyRegionTypeCode = GetValueIDType(declaration.DMExtensions.AutonomyRegionType);
                declarationPM.DeclarationDocumentId = GetValueIDType(declaration.DMExtensions.PreviousDocument.ID);
                declarationPM.DeclarationDocumentTypeCode = GetValueCodeType( declaration.DMExtensions.PreviousDocument.TypeCode);
                declarationPM.LoadingFactor =  declaration.DMExtensions.ExpenseLoadingFactor.Value ;
            }


            if (declaration.Importer != null)
            {
                SetImporters(ref declarationPM, declaration);
            }




        }

        private void SetImporters(ref DeclarationPM declarationPM, Declaration declaration)
        {
            foreach (var importer in declaration.Importer)
            {
                if (importer.ID != null)
               switch(GetValueCodeType(importer.DMExtensions.RoleCode))
                {
                    case "4":
                        {
                            declarationPM.ImporterTypeCode = importer.ID.schemeID;
                            declarationPM.ImporterAddress = importer.DMExtensions.Address;
                            declarationPM.ImporterName = importer.DMExtensions.Name;
                            declarationPM.MainImporterEntitlemntTypeCode = GetValueCodeType(importer.DMExtensions.EntitlementTypeCode);
                         if (importer.ID.schemeID=="2" || importer.ID.schemeID == "3")   declarationPM.ImporterPassCountryCode = GetValueTextType(importer.DMExtensions.IssueLocation);

                            switch (importer.ID.schemeID)
                            {
                                case "1":
                                    {
                                        declarationPM.ImporterCode = importer.ID.Value;
                                        break;
                                    }

                                case "3":
                                case "2":
                                    {
                                        declarationPM.ImporterPassportNumber = importer.ID.Value;
                                        break;
                                    }

                            }
                            break;
                        }

                    case "5":
                        {
                            switch (importer.ID.schemeID)
                            {
                                case "1":
                                    {
                                        declarationPM.TransferImporterId = importer.ID.Value;
                                        break;
                                    }

                                case "3":
                                case "2":
                                    {
                                        declarationPM.TransferPassportNumber = importer.ID.Value;
                                        break;
                                    }

                            }
                            declarationPM.TransferImporterTypeCode = importer.ID.schemeID;
                            declarationPM.TransferImporterAddress = importer.DMExtensions.Address;
                            declarationPM.TransferImporterName = importer.DMExtensions.Name;
                            declarationPM.TransImporterEntitleTypeCode = GetValueCodeType(importer.DMExtensions.EntitlementTypeCode);
                            if (importer.ID.schemeID == "2" || importer.ID.schemeID == "3") declarationPM.TransferImporterCountryCode = GetValueTextType(importer.DMExtensions.IssueLocation);

                            break;

                        }

                    case "6":
                        {

                            switch (importer.ID.schemeID)
                            {
                                case "1":
                                    {
                                        declarationPM.EntitleImporterId = importer.ID.Value;
                                        break;
                                    }

                                case "3":
                                case "2":
                                    {
                                        declarationPM.EntitlePassportNumber = importer.ID.Value;
                                        break;
                                    }

                            }
                            declarationPM.EntitleImporterTypeCode = importer.ID.schemeID;
                            declarationPM.EntitleImporterAddress = importer.DMExtensions.Address;
                            declarationPM.EntitleImporterName = importer.DMExtensions.Name;
                            declarationPM.ImporterEntitlementTypeCode = GetValueCodeType(importer.DMExtensions.EntitlementTypeCode);
                            if (importer.ID.schemeID == "2" || importer.ID.schemeID == "3") declarationPM.EntitleImporterCountryCode = GetValueTextType(importer.DMExtensions.IssueLocation);


                            break;
                        }

                }

               


            }

        }


    
    
        private List<ConsignmentPM> GetConsignments(Declaration declaration)
        {
            if (declaration.GoodsShipment == null || declaration.GoodsShipment.Count() == 0) return null;
            if (declaration.GoodsShipment[0].Consignment == null || declaration.GoodsShipment[0].Consignment.Count() == 0) return null;

            List<ConsignmentPM> consignmentPMs = new List<ConsignmentPM>();
            foreach (var consignment in declaration.GoodsShipment[0].Consignment)
            {
                ConsignmentPM consignmentPM = new ConsignmentPM();
                consignmentPM.ChangeSetOp = ChangeSetOperation.Insert;

                if (consignment.TransportContractDocument != null)
                { consignmentPM.CargoTypeCode = GetValueCodeType(consignment.TransportContractDocument.TypeCode);
                    consignmentPM.ManifestDate = Convert.ToDateTime(consignment.TransportContractDocument.IssueDateTime);
                    consignmentPM.ManifestNumber = GetValueIDType(consignment.TransportContractDocument.ID);
                   }
                

                if (consignment.TransportContractDocument.DMExtensions != null)
                {
                    consignmentPM.SecondCargoID = GetValueIDType(consignment.TransportContractDocument.DMExtensions.SecondCargoID);
                    consignmentPM.ThirdCargoID = GetValueIDType(consignment.TransportContractDocument.DMExtensions.ThirdCargoID);


                //if (consignmentPM.CargoTypeCode =="17")  ***************
                //{

                //}
                }

                if (consignment.UnloadingLocation != null)
                {
                    consignmentPM.UnloadPortCode = GetValueIDType(consignment.UnloadingLocation.ID);
                    consignmentPM.UnloadDate = Convert.ToDateTime( consignment.UnloadingLocation.ArrivalDateTime);

                }

                if(consignment.LoadingLocation!= null )
                {
                    consignmentPM.LoadingPortCode = GetValueIDType(consignment.LoadingLocation.ID);
                }


                if (consignment.DMExtensions!= null)
                {
                    consignmentPM.CargoDescription = GetValueTextType(consignment.DMExtensions.CargoDescription);
                    if (consignment.DMExtensions.LastReleaseFromWarehousInd != null)
                    {
                        if (consignment.DMExtensions.LastReleaseFromWarehousInd.Value == true)
                            consignmentPM.IsLastReleaseFromWarehous = "T";
                        else
                            consignmentPM.IsLastReleaseFromWarehous = "F";
                    }
                    consignmentPM.OriginCountryCode = GetValueCodeType(consignment.DMExtensions.ExportationCountryCode);

                    if (consignment.DMExtensions.RegisteredFacility!=null && consignment.DMExtensions.RegisteredFacility.Count()>0)
                    {
                        foreach (var registeredFacility in consignment.DMExtensions.RegisteredFacility)
                        {
                            switch(GetValueCodeType(registeredFacility.FacilityType))
                            {
                                case "004":
                                    {
                                        consignmentPM.StorageSiteCode = GetValueIDType(registeredFacility.ID);
                                        break;
                                    }
                                case "003":
                                    {
                                        consignmentPM.ReceiverWarehouseCode = GetValueIDType(registeredFacility.ID);
                                        break;
                                    }
                                case "005":
                                    {

                                        consignmentPM.ConsignmentInternalTransitions = new List<ConsignmentInternalTransitionPM>()
                                        {
                                            new ConsignmentInternalTransitionPM (){
                                               ChangeSetOp = ChangeSetOperation.Insert,
                                                SiteCode=GetValueIDType(registeredFacility.ID)
                                        }
                                        };
                                        break;
                                    }
                            }
                        }
                    }
                }

                if (consignment.DMExtensions.PackagesMeasure!= null)
                {
                    consignmentPM.ConsignmentPackages = GetConsignmentPackages(consignment.DMExtensions.PackagesMeasure);
                }

                consignmentPMs.Add(consignmentPM);

            }

            return consignmentPMs;
        }

        private List<ConsignmentPackagePM> GetConsignmentPackages(DeclarationGoodsShipmentConsignmentDMExtensionsPackagesMeasure[] packagesMeasures)
        {
            List<ConsignmentPackagePM> consignmentPackagePMs = new List<ConsignmentPackagePM>();  
            foreach (var packagesMeasure in packagesMeasures)
            {
                ConsignmentPackagePM consignmentPackagePM = new ConsignmentPackagePM();
                consignmentPackagePM.ChangeSetOp = ChangeSetOperation.Insert;
                consignmentPackagePM.PackageMeasureQualifierCode = GetValueCodeType(packagesMeasure.PackageMeasureQualifier);
                consignmentPackagePM.PackageQuantityTypeCode = packagesMeasure.TotalPackageQuantity.unitCode.ToString();
                consignmentPackagePM.PackageQuantity =Convert.ToInt32(packagesMeasure.TotalPackageQuantity.Value) ;
                consignmentPackagePM.GrossMassMeasureTypeCode = packagesMeasure.GrossMassMeasure.unitCode.ToString();
                consignmentPackagePM.GrossMassMeasure = packagesMeasure.GrossMassMeasure.Value;
                consignmentPackagePM.PackageTypeCode = packagesMeasure.TypeCode.Value;
                consignmentPackagePM.MarksNumbers = packagesMeasure.MarksNumbers.Value;
                consignmentPackagePMs.Add(consignmentPackagePM);
            }

            return consignmentPackagePMs;
        }

      

        private string  GetAgent(Declaration declaration)
        {
            if (declaration.Agent != null && declaration.Agent.Count() > 0)
            {
                var agent = declaration.Agent.FirstOrDefault(x => x.RoleCode.Value == "1");
                if (agent != null)
                    return agent.ID.Value;
            }

            return null;
        }


        private string GetVendorId(string vendorNumber , int tenant, ICustomContext context )
        {      

            var queryService = new CustomsVendorQueryService(context);
           return  queryService.GetIdByVendorNumber(vendorNumber, tenant);
             


        }
        private List<SupplierInvoicePM> GetSupplierInvoices(Declaration declaration , int tenant, ICustomContext context)
        {
            List<SupplierInvoicePM> supplierInvoicePMs = new List<SupplierInvoicePM>();

            foreach (var item in declaration.GoodsShipment)
            {
                SupplierInvoicePM supplierInvoicePM = new SupplierInvoicePM()
                {
                     ChangeSetOp = ChangeSetOperation.Insert,
                    SequenceNumeric = (int)item.SequenceNumeric,
                    InvoiceNumber = item.Invoice.ID.Value,
                    IssueDate = Convert.ToDateTime(item.Invoice.IssueDateTime),
                    AccountTypeCode = item.Invoice.TypeCode.Value,
                  
                    
                };

                if (item.Invoice.DMExtensions != null)
                {
                    supplierInvoicePM.IsPreference = item.Invoice.DMExtensions.IsPrefarenceDocumentInd.Value;
                    supplierInvoicePM.PreferenceDocumentTypeCode = item.Invoice.DMExtensions.PrefarenceDocumentType.Value;
                    supplierInvoicePM.PaymentTypeCode = item.Invoice.DMExtensions.PaymentType.Value;
                    supplierInvoicePM.InvoiceAmount = item.Invoice.DMExtensions.InvoiceAmount.Value;
                    //SetAmountTypeValue *************
                    //InvoiceCurrencyTypeCode
                    supplierInvoicePM.ActualPayedAmount = item.Invoice.DMExtensions.ActualPayedAmount.Value;
                }
                //ActualPayedCurrencyTypeCode
              if (item.Supplier !=null)  supplierInvoicePM.VendorId = GetVendorId( item.Supplier.ID.Value , tenant, context);

              if(item.TradeTerms!= null)
                {
             supplierInvoicePM.IncotermCode = item.TradeTerms.ConditionCode.Value;
                supplierInvoicePM.IssueCountryCode = item.TradeTerms.LocationID.Value;
                }
   
                supplierInvoicePM.SupplierInvoiceModifications = GetSupplierInvoiceModifications(item , ref supplierInvoicePM);


                supplierInvoicePM.SupplierInvoiceItems = GetSupplierInvoiceItems(item);

 
                //InsuranceAmount *****
                //TotalFreightInFreightCurrency
                supplierInvoicePMs.Add(supplierInvoicePM);
            }

            return supplierInvoicePMs;
        }

        private List<SupplierInvoiceItemPM> GetSupplierInvoiceItems(DeclarationGoodsShipment item)
        {
            List<SupplierInvoiceItemPM> supplierInvoiceItemPMs = new List<SupplierInvoiceItemPM>();

            foreach (var governmentAgencyGoodsItem in item.GovernmentAgencyGoodsItem)
            {
                SupplierInvoiceItemPM supplierInvoiceItemPM = new SupplierInvoiceItemPM();
                supplierInvoiceItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                supplierInvoiceItemPM.SequenceNumeric =(int)governmentAgencyGoodsItem.SequenceNumeric;
                supplierInvoiceItemPM.OriginCountryCode = governmentAgencyGoodsItem.Origin.CountryCode.Value;

                if(governmentAgencyGoodsItem.Commodity.Classification!= null || governmentAgencyGoodsItem.Commodity.Classification.Count()>0)
                {
                    var classification = governmentAgencyGoodsItem.Commodity.Classification.FirstOrDefault(x => x.IdentificationTypeCode.Value == "SSO");

                    if (classification != null)
                    {
                        supplierInvoiceItemPM.DangerousClassificationCode = classification.ID.Value;
                        supplierInvoiceItemPM.DangerousPackingGroupTypeCode = classification.DMExtensions.DangerousGoodsPackingRequirementsGroupCode.Value;
                    }


                    var classification2 = governmentAgencyGoodsItem.Commodity.Classification.FirstOrDefault(x => x.IdentificationTypeCode.Value == "HS");

                    if (classification2 != null)
                    {
                        supplierInvoiceItemPM.ClassificationCode = classification2.ID.Value;
                     }
                }


                if (governmentAgencyGoodsItem.Commodity.GovernmentProcedure!= null && governmentAgencyGoodsItem.Commodity.GovernmentProcedure.Count()>0)
                {
                    supplierInvoiceItemPM.SupplierInvoiceItemProcesTypes = new List<SupplierInvoiceItemProcesTypePM>();


                    foreach (var governmentProcedure in governmentAgencyGoodsItem.Commodity.GovernmentProcedure)
                    {
                        SupplierInvoiceItemProcesTypePM supplierInvoiceItemProcesTypePM = new SupplierInvoiceItemProcesTypePM();
                    supplierInvoiceItemProcesTypePM.ChangeSetOp = ChangeSetOperation.Insert;
                        supplierInvoiceItemProcesTypePM.ProcessTypeCode = governmentProcedure.CurrentCode.Value;

                        supplierInvoiceItemPM.SupplierInvoiceItemProcesTypes.Add(supplierInvoiceItemProcesTypePM);
                    }




                }



                foreach (var goodsMeasure in governmentAgencyGoodsItem.GoodsMeasure)
                {
                    if (goodsMeasure.DMExtensions!= null)
                    {
                        switch (goodsMeasure.DMExtensions.MeasureQualifier.Value)
                        {
                            case "1":
                                {
                                    supplierInvoiceItemPM.InvoiceQuantityType = goodsMeasure.TariffQuantity.unitCode.ToString();
                                    supplierInvoiceItemPM.InvoiceQuantity = goodsMeasure.TariffQuantity.Value;
                                      break;
                                }

                            case "2":
                                {
                                    supplierInvoiceItemPM.StatisticQuantityType = goodsMeasure.TariffQuantity.unitCode.ToString();
                                    supplierInvoiceItemPM.StatisticQuantity = goodsMeasure.TariffQuantity.Value;
                                    break;
                                }

                            case "3":
                                {
                                    supplierInvoiceItemPM.AdditionalQuantityType = goodsMeasure.TariffQuantity.unitCode.ToString();
                                    supplierInvoiceItemPM.AdditionalQuantity = goodsMeasure.TariffQuantity.Value;
                                    break;
                                }
                        }



                    }


                   

                }

                     supplierInvoiceItemPM.SupplierInvoiceItemsConDeclars = new List<SupplierInvoiceItemsConDeclarPM>();

                    if (governmentAgencyGoodsItem.PreviousDocument!= null && governmentAgencyGoodsItem.PreviousDocument.Count()>0)
                     {
                    foreach (var previousDocument in governmentAgencyGoodsItem.PreviousDocument)
                    {
                        SupplierInvoiceItemsConDeclarPM supplierInvoiceItemsConDeclarPM = new SupplierInvoiceItemsConDeclarPM();
                    supplierInvoiceItemsConDeclarPM.ChangeSetOp = ChangeSetOperation.Insert;
                        supplierInvoiceItemsConDeclarPM.DeclarationNumber = previousDocument.ID.Value;
                        supplierInvoiceItemsConDeclarPM.ItemSequence =(int) previousDocument.SequenceNumeric;
                        supplierInvoiceItemsConDeclarPM.DeclarationTypeCode = previousDocument.TypeCode.Value;

                        if(previousDocument.DMExtensions!= null)
                        {
                            supplierInvoiceItemsConDeclarPM.QuantityTypeCode = previousDocument.DMExtensions.QuantityQuantity.unitCode.ToString();
                            supplierInvoiceItemsConDeclarPM.Quantity = previousDocument.DMExtensions.QuantityQuantity.Value;
                            supplierInvoiceItemsConDeclarPM.InvoiceNumber = previousDocument.DMExtensions.SequenceNumeric;

                        }

                        supplierInvoiceItemPM.SupplierInvoiceItemsConDeclars.Add(supplierInvoiceItemsConDeclarPM);

                    }
                    if (governmentAgencyGoodsItem.Manufacturer!= null)
                    supplierInvoiceItemPM.ManufactureIdentifier = governmentAgencyGoodsItem.Manufacturer.ID.Value;



                    if (governmentAgencyGoodsItem.DMExtensions!= null)
                    {
                        if (governmentAgencyGoodsItem.DMExtensions.GoodsItemAmount != null && governmentAgencyGoodsItem.DMExtensions.GoodsItemAmount.Count() > 0)
                        {
                            foreach (var goodsItemAmount in governmentAgencyGoodsItem.DMExtensions.GoodsItemAmount)
                            {
                                //    var success = Enum.TryParse<ISO3AlphaCurrencyCodeContentType>(CurrencyCode, out isoCurrency);
 

                                switch (goodsItemAmount.AmountType.Value)
                                {
                                    case "1":
                                        {
                                            supplierInvoiceItemPM.ItemPrice = goodsItemAmount.CustomsValueAmount.Value;
                                            supplierInvoiceItemPM.ItemPriceCurrencyCode= goodsItemAmount.CustomsValueAmount.currencyID.ToString();
                                            break;

                                        }

                                    case "11":
                                        {
                                            supplierInvoiceItemPM.NonCustomsItemPrice = goodsItemAmount.CustomsValueAmount.Value;
                                            supplierInvoiceItemPM.NonCustomsItemPriceCurCode = goodsItemAmount.CustomsValueAmount.currencyID.ToString();
                                            break;

                                        }


                                    case "5":
                                        {
                                            supplierInvoiceItemPM.WholeSaleItemPrice = goodsItemAmount.CustomsValueAmount.Value;
                                            supplierInvoiceItemPM.WholeSaleItemPriceCurrencyCode = goodsItemAmount.CustomsValueAmount.currencyID.ToString();
                                            break;

                                        }

                                }
                            }
                        }
                        supplierInvoiceItemPM.CustomsBookTypeCode = governmentAgencyGoodsItem.DMExtensions.CustomsBookType.Value;
                        supplierInvoiceItemPM.TaxExemptCode = governmentAgencyGoodsItem.DMExtensions.TaxExemptCode.Value;
                        supplierInvoiceItemPM.OptionalTamaPercentage = governmentAgencyGoodsItem.DMExtensions.OptionalTama.Value;


                        supplierInvoiceItemPM.SupplierInvoiceItemVehicles = GetSupplierInvoiceItemVehicles(governmentAgencyGoodsItem);

                        supplierInvoiceItemPM.SalesTaxExemptionTypeCode = governmentAgencyGoodsItem.DMExtensions.SalesTaxExemptionType.Value;
                        supplierInvoiceItemPM.PreferenceDocumentNumber = governmentAgencyGoodsItem.DMExtensions.PreferenceDocumentNumber.Value;
                        supplierInvoiceItemPM.IsUsed = governmentAgencyGoodsItem.DMExtensions.IsUsed.Value;
                        supplierInvoiceItemPM.ActualInvoiceLines = governmentAgencyGoodsItem.DMExtensions.InvoiceLineNumbers;
                        supplierInvoiceItemPM.DeferredCustomsTax = governmentAgencyGoodsItem.DMExtensions.DeferredCustomsTax.Value;
                        supplierInvoiceItemPM.DeferredPurchaseTax = governmentAgencyGoodsItem.DMExtensions.DeferredPurchaseTax.Value;
                        //AdditionalDocument**********
                        supplierInvoiceItemPM.SupplierInvoiceItemsMods = GetSupplierInvoiceItemsMods(governmentAgencyGoodsItem);
                    }

                }
        



                supplierInvoiceItemPMs.Add(supplierInvoiceItemPM);
            }

            return supplierInvoiceItemPMs;
        }


 

        private List<SupplierInvoiceItemsModPM> GetSupplierInvoiceItemsMods(DeclarationGoodsShipmentGovernmentAgencyGoodsItem governmentAgencyGoodsItem)
        {
            List<SupplierInvoiceItemsModPM> supplierInvoiceItemsModPMs = new List<SupplierInvoiceItemsModPM>();


            foreach (var valuationAdjustment in governmentAgencyGoodsItem.ValuationAdjustment)
            {
                SupplierInvoiceItemsModPM supplierInvoiceItemsMod = new SupplierInvoiceItemsModPM();
                supplierInvoiceItemsMod.ChangeSetOp = ChangeSetOperation.Insert;
                supplierInvoiceItemsMod.TypeCode = valuationAdjustment.AdditionCode.Value;
                supplierInvoiceItemsMod.CurrencyTypeCode = valuationAdjustment.AmountAmount.currencyID.ToString();
                supplierInvoiceItemsMod.Amount = valuationAdjustment.AmountAmount.Value;
                supplierInvoiceItemsModPMs.Add(supplierInvoiceItemsMod);
            }

            return supplierInvoiceItemsModPMs;
        }

        private List<SupplierInvoiceItemVehiclePM> GetSupplierInvoiceItemVehicles(DeclarationGoodsShipmentGovernmentAgencyGoodsItem governmentAgencyGoodsItem)
        {
            List<SupplierInvoiceItemVehiclePM> SupplierInvoiceItemVehiclePMs = new List<SupplierInvoiceItemVehiclePM>();

            foreach (var vehicle in governmentAgencyGoodsItem.DMExtensions.Vehicle)
            {
                SupplierInvoiceItemVehiclePM supplierInvoiceItemVehiclePM = new SupplierInvoiceItemVehiclePM();
                supplierInvoiceItemVehiclePM.ChangeSetOp = ChangeSetOperation.Insert;
                supplierInvoiceItemVehiclePM.RichbitFileNumber = vehicle.ID.Value;
                supplierInvoiceItemVehiclePM.VehicleTypeCode = vehicle.IDTypeCode.Value;

                //vehicleDetails.ID.Value = supplierInvoiceItemVehicleItem.VehicleChassisNumber;
                //vehicleDetails.IDTypeCode.Value = supplierInvoiceItemVehicleItem.VehicleTypeCode

                SupplierInvoiceItemVehiclePMs.Add(supplierInvoiceItemVehiclePM);

            }


            return SupplierInvoiceItemVehiclePMs;
        }

        private List<SupplierInvoiceModificationPM> GetSupplierInvoiceModifications(DeclarationGoodsShipment declarationGoodsShipment , ref SupplierInvoicePM supplierInvoicePM)
        {
            List<SupplierInvoiceModificationPM> supplierInvoiceModificationPMs = new List<SupplierInvoiceModificationPM>();

            foreach (var customsValuation in declarationGoodsShipment.CustomsValuation)
            {
                 string[] ChargesTypeCode = new string[] { "67,144,I02" };
                if (!ChargesTypeCode.Contains(customsValuation.ChargesTypeCode.Value)) {
                    SupplierInvoiceModificationPM supplierInvoiceModificationPM = new SupplierInvoiceModificationPM()
                    { ChangeSetOp = ChangeSetOperation.Insert,
                    TypeCode = customsValuation.ChargesTypeCode.Value,
                    //  CurrencyTypeCode = customsValuation.OtherChargeDeductionAmount.currencyID,
                    Amount = customsValuation.OtherChargeDeductionAmount.Value
                };  supplierInvoiceModificationPMs.Add(supplierInvoiceModificationPM);
                  }


                if (customsValuation.ChargesTypeCode.Value=="67")
                {
                    supplierInvoicePM.InsruanceCurrencyTypeCode = customsValuation.ExitToEntryChargeAmount.currencyID.ToString();
                    supplierInvoicePM.InsuranceAmount = customsValuation.ExitToEntryChargeAmount.Value;
                }

                if (customsValuation.ChargesTypeCode.Value == "144")
                {
                    supplierInvoicePM.FreightCurrencyTypeCode = customsValuation.ExitToEntryChargeAmount.currencyID.ToString();
                    supplierInvoicePM.TotalFreightInFreightCurrency = customsValuation.ExitToEntryChargeAmount.Value;
                }

            }

            return supplierInvoiceModificationPMs;
         } 

        public override void Update(DF_NG_2754_MSG10004_ImportDeclarationResponse customResponse, GenericRequestParams requestParams)
        {
            var responseName = requestParams.ResponseName;
            var context = CustomContext.GetContext(requestParams.Tenant);
            var myQueryService = new DeclarationQueryService(context);
            var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            var mySupplierInvoiceItemsTaxUpdateService = new SupplierInvoiceItemsTaxUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            var mySupplierInvoiceItemVehicleModUpdateService = new SupplierInvoiceItemVehicleModUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant); // moran 20.10.15 - Task 17209 
            var mySupplierInvoiceItemModVehicleUpdateService = new SupplierInvoiceItemModVehicleUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant); // moran 24.11.15 - Task 17424 
            //var mySupplierInvoiceItemsTaxesModificationUpdateService = new SupplierInvoiceItemsTaxesModUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            var myDeclarationTaxUpdateService = new DeclarationTaxUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            var myDeclarationConstraintUpdateService = new DeclarationConstraintUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            var myDeclarationPaymentQueryService = new DeclarationPaymentQueryService(context);

            DeclarationErrorPointerService mydDclarationErrorPointerService = new DeclarationErrorPointerService();

            //var mySupplierInvioceItemCertificatUpdateService = new SupplierInvioceItemCertificatUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant); 

            this.MyResponseData = new INF_MSG_GenericResponseData();

            if (string.IsNullOrWhiteSpace(requestParams.AppicationId))
            {
                //In cases the response cannot be connected to a request using the External Id , search by DeclarationNumber
                requestParams.AppicationId = myQueryService.GetIdByDeclarationNumber(customResponse.Response.Declaration.ID.Value, requestParams.Tenant);
                if (string.IsNullOrWhiteSpace(requestParams.AppicationId))
                {
                    //if still not found search by ExternalDeclarationNumber
                    requestParams.AppicationId = myQueryService.GetIdByExternalDeclarationNumber(customResponse.Response.Declaration.DMExtensions.ExternalDeclarationID.Value, requestParams.Tenant);
                }
            }

            if (string.IsNullOrWhiteSpace(requestParams.AppicationId))
            {
                LogMessagingUtil.Instance.AppendLine("Can not find declaration- DeclarationNumber: " + customResponse.Response.Declaration.ID.Value + " ExternalDeclarationNumber: " + customResponse.Response.Declaration.DMExtensions.ExternalDeclarationID.Value);
                this.MyResponseData.ApplicationID = requestParams.AppicationId;
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.UserMessage = "Can not find declaration- DeclarationNumber: " + customResponse.Response.Declaration.ID.Value + " ExternalDeclarationNumber: " + customResponse.Response.Declaration.DMExtensions.ExternalDeclarationID.Value;
                return;
            }


            var swGetSingle = Stopwatch.StartNew();
            myQueryService.LoadSupplierInvoicesItemsParentsOnly = true;
            this._MyDeclarationPM = myQueryService.GetSingle(requestParams.AppicationId, true, false);
            LogMessagingUtil.Instance.AppendLine("myQueryService.GetSingle:Took:" + swGetSingle.ElapsedMilliseconds);
            
            if (this._MyDeclarationPM == null)
            {
                LogMessagingUtil.Instance.AppendLine("Can not find declaration" + requestParams.AppicationId);
                this.MyResponseData.ApplicationID = requestParams.AppicationId;
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.UserMessage = "Can not find declaration" + requestParams.AppicationId;
                return;
            }

            if (_MyDeclarationPM.IsCourierDeclaration && this._MyDeclarationPM.PaymentDate.HasValue)
            {
                if (customResponse.Response != null && customResponse.Response.Status != null && customResponse.Response.Status.NameCode.Value == "13")
                {
                    // Clear Fields
                    _MyDeclarationPM.DeclarationStatusTypeCode = customResponse.Response.Status.NameCode.Value;
                    _MyDeclarationPM.PaymentDate = null;
                    _MyDeclarationPM.PaymentOrderNumber = null;
                    _MyDeclarationPM.PaymentStatusCode = null;
                    _MyDeclarationPM.CourierCustomStatusCode = null;
                    _MyDeclarationPM.CourierSuspentionCode = null;
                    _MyDeclarationPM.CourierSuspentionReasonCode = null;

                    // Delete Payment
                    var mydeclarationPaymentQueryService = new DeclarationPaymentQueryService(context);
                    var declarationPaymentPM = mydeclarationPaymentQueryService.GetSingle(_MyDeclarationPM.Id, true, false);
                    if (declarationPaymentPM != null)
                    {
                        declarationPaymentPM.ChangeSetOp = ChangeSetOperation.Delete;
                        if (declarationPaymentPM.DeclarationPaymentMethods.Any())
                        {
                            foreach (var item in declarationPaymentPM.DeclarationPaymentMethods)
                            {
                                item.ChangeSetOp = ChangeSetOperation.Delete;
                            }
                        }
                        DeclarationPaymentUpdateService declarationPaymentUpdateService = new DeclarationPaymentUpdateService(context, new Dictionary<string, IContext>(), _MyDeclarationPM.Tenant);
                        declarationPaymentUpdateService.Update(declarationPaymentPM, true);
                    }

                    // Delete Status
                    DeclarationUpdateService.DelDeclarationStatus(_MyDeclarationPM, "", "RSH");
                }
            }

            if (requestParams.GetType() == typeof(DeclarationRestoreRequestParams))// moran 10.1.16 Task 19724 // Mirit 24/01/16 19845 
            {
                //if (this._MyDeclarationPM.CorrectionsXml != null)
                //{
                //    string mess = null;

                //    mess = "בוצעו תיקונים בהצהרה (תיקון הצהרה) נתוני ההצהרה לא עודכנו " + " (" + _MyDeclarationPM.DeclarationNumber + ")";
                //    LogMessagingUtil.Instance.AppendLine(mess);
                //    this.MyResponseData.ApplicationID = requestParams.AppicationId;
                //    this.MyResponseData.Succeeded = true;
                //    this.MyResponseData.UserMessage = mess;
                //    this.MyResponseData.HasException = false;

                //    return;
                //}

                if (this._MyDeclarationPM.PaymentDate.HasValue && !_MyDeclarationPM.IsCourierDeclaration) //If declaration was already paid 
                {
                    //Task 44715 allow update of 1.0 if current <1.0 and it's a restore response
                    if (!(requestParams.GetType() == typeof(DeclarationRestoreRequestParams) && System.Convert.ToDouble(_MyDeclarationPM.VersionId) < 1.0 && System.Convert.ToDouble(customResponse.Response.Declaration.DMExtensions.VersionID.Value) == 1.0) //restored version 1.0 and current 0.x
                        && (_MyDeclarationPM.VersionId != customResponse.Response.Declaration.DMExtensions.VersionID.Value)) //Compare Declaration Version

                    //if (_MyDeclarationPM.VersionId != customResponse.Response.Declaration.DMExtensions.VersionID.Value) //Compare Declaration Version
                    {
                        string mess = "נתוני ההצהרה לא עודכנו " + " (" + _MyDeclarationPM.DeclarationNumber + ")" + " הצהרה כבר שולמה ויש שוני בין הגרסאות";
                        LogMessagingUtil.Instance.AppendLine(mess);
                        this.MyResponseData.ApplicationID = requestParams.AppicationId;
                        this.MyResponseData.Succeeded = true;
                        this.MyResponseData.UserMessage = mess;
                        this.MyResponseData.HasException = true;
                        return;
                    }
                    else
                    {
                        _MyDeclarationPM.PaymentDate = null;
                        _MyDeclarationPM.PaymentOrderNumber = "";
                        _MyDeclarationPM.PaymentStatusCode = "";
                    }
                }
            }

            if (this._MyDeclarationPM.IsConvertedDeclaration) // Mirit 24/01/16 19918
            {
                LogMessagingUtil.Instance.AppendLine("נתוני ההצהרה לא עודכנו מכיוון שמדובר בהצהרה מוסבת");
                this.MyResponseData.ApplicationID = requestParams.AppicationId;
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.UserMessage = "נתוני ההצהרה לא עודכנו מכיוון שמדובר בהצהרה מוסבת";
                this.MyResponseData.HasException = false;

                return;
            }

            //<--- Yuval Chalup 28.05.2015 TASK-13252
            if (!string.IsNullOrWhiteSpace(this._MyDeclarationPM.Id))
            {
                if (this.MyRequestSheetParam == null)
                {
                    this.MyRequestSheetParam = new RequestSheetParam();
                }
                this.MyRequestSheetParam.CustomFileNo = this._MyDeclarationPM.CustomFileNo;
                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                this.MyRequestSheetParam.EntityId1 = this._MyDeclarationPM.Id;
                this.MyRequestSheetParam.RequestDescription = "משוב להצהרה  " + this._MyDeclarationPM.DeclarationNumber;
                if (customResponse.Response != null)
                {
                    if (customResponse.Response.Declaration != null)
                    {
                        if (customResponse.Response.Declaration.DMExtensions != null)
                        {
                            if (customResponse.Response.Declaration.DMExtensions.VersionID != null)
                            {
                                this.MyRequestSheetParam.RequestDescription = "משוב להצהרה  " + customResponse.Response.Declaration.ID.Value + " " + customResponse.Response.Declaration.DMExtensions.VersionID.Value;
                            }
                        }
                    }
                }
            }
            //move to OnUpdating


            //Yuval Chalup 28.05.2015 TASK-13252 --->

            float oldVersionId;
            float.TryParse(_MyDeclarationPM.VersionId, out oldVersionId);
            if (string.IsNullOrWhiteSpace(_MyDeclarationPM.VersionId))
            {
                oldVersionId = 0.1F;
            }
            var declarationPaymentsPM = myDeclarationPaymentQueryService.GetSingle(requestParams.AppicationId, true, false);

            if (requestParams.InterfaceTypeCode == "2755")
            {
                this._IsSubmitDeclarationResponse = true;
            }

            //if (customResponse.ResponseContentHeader.Exception != null)
            // {
            //     string userMessage = customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription;
            //     this._MyDeclarationPM.MarkAsChanged = false;
            //     var swErrosXml = Stopwatch.StartNew();
            //     this._MyDeclarationPM.ErrosXml = mydDclarationErrorPointerService.AddDeclarationException(this._MyDeclarationPM.ErrosXml, "Buisness", customResponse.ResponseContentHeader.Exception[0]);
            //     LogMessagingUtil.Instance.AppendLine("ErrosXml:Took:" + swErrosXml.ElapsedMilliseconds);

            //     if (_IsSubmitDeclarationResponse != true) // only for sending declaration (2750)
            //     {
            //         _MyDeclarationPM.IsChanged = true;
            //     }
            //     //this._MyDeclarationPM.DeclarationStatusTypeCode = null; moran 14.6.16 - Task 21617 - commented
            //     if (customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionType == 2794)
            //     {
            //         this._MyDeclarationPM.DeclarationNumber = customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExceptionParms.FirstOrDefault();
            //         userMessage = "עודכן מספר ההצהרה לפי רשומת הסוכן - יש לשדר את ההצהרה מחדש";
            //     }
            //     if (_MyDeclarationPM.UserNotes == "LoadTestOnProgress")
            //     {
            //         _MyDeclarationPM.UserNotes = "LoadTest";
            //     }

            //     this._MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
            //     this._MyDeclarationPM.CurrentContextTag = Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.UpdateIIGExcptionConst;
            //     myDeclarationUpdateService.Update(this._MyDeclarationPM, true);

            //     this.MyResponseData.ApplicationID = requestParams.AppicationId;
            //     this.MyResponseData.Succeeded = true;
            //     this.MyResponseData.UserMessage = userMessage;
            //     this.MyResponseData.HasException = true;

            //     return;
            // }
            if (_MyDeclarationPM.DepositionStatusCode == "R") _MyDeclarationPM.DepositionStatusCode = null;
            if (customResponse.ResponseContentHeader.Exception != null)
            {
                string userMessage = "";
                this._MyDeclarationPM.MarkAsChanged = false;
                var swErrosXml = Stopwatch.StartNew();

                foreach (UnifreightIIG.Common.ImportDeclarationServiceReference.Exception exception in customResponse.ResponseContentHeader.Exception)
                {
                    if (!string.IsNullOrWhiteSpace(userMessage))
                    {
                        userMessage = userMessage + @"
";
                    }
                    userMessage = userMessage + exception.ExeptionDescription;
                    this._MyDeclarationPM.ErrosXml = mydDclarationErrorPointerService.AddDeclarationException(this._MyDeclarationPM.ErrosXml, "Buisness", exception, true);
                    LogMessagingUtil.Instance.AppendLine("ErrosXml:Took:" + swErrosXml.ElapsedMilliseconds);
                    switch (exception.ExeptionType)
                    {
                        case 2794:
                            {
                                this._MyDeclarationPM.DeclarationNumber = exception.ExceptionParms.FirstOrDefault();
                                if (!string.IsNullOrWhiteSpace(userMessage))
                                {
                                    userMessage = userMessage + @"
";
                                }
                                userMessage = userMessage + "עודכן מספר ההצהרה לפי רשומת הסוכן - יש לשדר את ההצהרה מחדש";
                                break;
                            }
                        case 1501:
                            {
                                UpdateUnifreightEvent("MPOA", requestParams.LoggingUserId);
                                userMessage = userMessage + "חסר יפוי כח";
                                break;
                            }
                        case 4589:
                            {
                                UpdateUnifreightEvent("MID", requestParams.LoggingUserId);
                                userMessage = userMessage + "חסר תצהיר יבואן";
                                if (string.IsNullOrWhiteSpace(_MyDeclarationPM.DepositionStatusCode)) _MyDeclarationPM.DepositionStatusCode = "R";
                                break;
                            }
                        case 2244:
                            {
                                UpdateUnifreightEvent("IDE", requestParams.LoggingUserId);
                                userMessage = userMessage + "תצהיר יבואן עומד לפוג";
                                break;
                            }
                    }
                }
                if (_IsSubmitDeclarationResponse != true) // only for sending declaration (2750)
                {
                    _MyDeclarationPM.IsChanged = true;
                }

                if (_MyDeclarationPM.UserNotes == "LoadTestOnProgress")
                {
                    _MyDeclarationPM.UserNotes = "LoadTest";
                }

                this._MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
                this._MyDeclarationPM.CurrentContextTag = Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.UpdateIIGExcptionConst;
                myDeclarationUpdateService.Update(this._MyDeclarationPM, true);

                if (this._MyDeclarationPM.IsCourierDeclaration)// due (customResponse.ResponseContentHeader.Exception != null)>> X
                {
                    //override DeclarationUpdateService.AfterUpdating/CalculateDeclarationCourierStatus
                    /// 	Logitude.Customs.BL.dll!Logitude.Customs.BL.EntityUpdateServices.DeclarationCourierStatusUpdateService.CalculateDeclarationCourierStatus(Logitude.Customs.Def.EntityPMs.DeclarationPM declarationPM) Line 67	C#
                    ///> Logitude.Customs.BL.dll!Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.AfterUpdating(Logitude.Customs.Def.EntityPMs.DeclarationPM entityPM, Logitude.Server.Tools.EntityPM entityParentPM) Line 859  C#

                    var calculateDeclarationCourierStatus = new CalculateDeclarationCourierStatus(this._MyDeclarationPM);
                    calculateDeclarationCourierStatus.Update(
                        (currentDeclarationCourierStatusPM) =>
                        {
                            
                            currentDeclarationCourierStatusPM.CourierDeclarationStatusCode = "X";
                        });
                }


                this.MyResponseData.ApplicationID = requestParams.AppicationId;
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.UserMessage = userMessage;
                this.MyResponseData.HasException = true;

                return;
            }









                if (customResponse.Response == null)
            {
                if (customResponse.ResponseContentHeader.Exception == null)
                {
                    string text = null;
                    if (String.IsNullOrWhiteSpace(customResponse.ResponseContentHeader.Remark))
                    {
                        text = "No Declaration details in the Response " + requestParams.AppicationId;
                        this.MyResponseData.UserMessage = text;
                        LogMessagingUtil.Instance.AppendLine(text);
                    }
                    else
                    {
                        text = "No Declaration details in the Response " + customResponse.ResponseContentHeader.Remark + requestParams.AppicationId;
                        this.MyResponseData.UserMessage = text;
                        LogMessagingUtil.Instance.AppendLine(text);
                    }
                    this.MyResponseData.ApplicationID = requestParams.AppicationId; //Yuval Chalup 28.05.2015 TASK-13252+13509
                    return;
                }
            }

            LogMessagingUtil.Instance.AppendLine("Analyze declaration response" + requestParams.AppicationId);
            _FastDelete = true;
            var sw = Stopwatch.StartNew();
            if (_FastDelete)
            {

                var myDeclarationKeys = new DeclarationKeys { Id = _MyDeclarationPM.Id };
                myDeclarationTaxUpdateService.FastDeleteComposition(myDeclarationKeys);
                mySupplierInvoiceItemVehicleModUpdateService.FastDeleteComposition(myDeclarationKeys);
                mySupplierInvoiceItemsTaxUpdateService.FastDeleteComposition(myDeclarationKeys);
                mySupplierInvoiceItemModVehicleUpdateService.FastDeleteComposition(myDeclarationKeys);
                //mySupplierInvioceItemCertificatUpdateService.FastDeleteComposition(myDeclarationKeys);
                (context as DbContextBase).SaveChanges();
                context = CustomContext.GetContext(requestParams.Tenant);
                myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);

            }
            else
            {
                DeleteDeclarationTaxes(myDeclarationTaxUpdateService);
                DeleteSupplierInvoiceItemsTaxes(mySupplierInvoiceItemsTaxUpdateService);
                DeleteSupplierInvoiceItemsVehicleMods(mySupplierInvoiceItemVehicleModUpdateService); // moran 20.10.15 - Task 17209
                DeleteSupplierInvoiceItemsModVehicles(mySupplierInvoiceItemModVehicleUpdateService); // moran 24.11.15 - Task 17424 
                //DeleteSupplierInvioceItemCertificates(mySupplierInvioceItemCertificatUpdateService);
            }
            LogMessagingUtil.Instance.AppendLine("IsFastDelete:" + _FastDelete.ToString() + ",Took :" + sw.ElapsedMilliseconds);

            if (String.IsNullOrWhiteSpace(_MyDeclarationPM.DeclarationNumber))
            {
                _MyDeclarationPM.DeclarationNumber = customResponse.Response.Declaration.ID.Value;
            }
            else
            {
                var customDeclarationNumber = customResponse.Response.Declaration.ID.Value;
                if (customDeclarationNumber != _MyDeclarationPM.DeclarationNumber)
                {
                    //throw new System.Exception("BL Error customDeclarationNumber != declarationPM.DeclarationNumber ");
                    _MyDeclarationPM.DeclarationNumber = customDeclarationNumber; //yaron !!
                }
            }

            //Update Declaration 
            _MyDeclarationPM.VersionId = customResponse.Response.Declaration.DMExtensions.VersionID.Value;
            _MyDeclarationPM.DeclarationStatusTypeCode = customResponse.Response.Status.NameCode.Value;
            _MyDeclarationPM.LoadingFactor = customResponse.Response.Declaration.DMExtensions.ExpenseLoadingFactor.Value;
            //_MyDeclarationPM.DealValue = customResponse.Response.Declaration.DMExtensions.CustomsValueComponent.TotalDealValueAmountNIS.Value;
            _MyDeclarationPM.DealValue = Math.Round(customResponse.Response.Declaration.DMExtensions.CustomsValueComponent.TotalDealValueAmountNIS.Value, 2);
            //_MyDeclarationPM.CIFValue = customResponse.Response.Declaration.DMExtensions.CustomsValueComponent.CifValueNIS.Value;
            _MyDeclarationPM.CIFValue = Math.Round(customResponse.Response.Declaration.DMExtensions.CustomsValueComponent.CifValueNIS.Value, 2);
            //_MyDeclarationPM.TotalTax = customResponse.Response.Declaration.DMExtensions.CustomsValueComponent.TaxAssessedAmount.Value;
            _MyDeclarationPM.TotalTax = Math.Round(customResponse.Response.Declaration.DMExtensions.CustomsValueComponent.TaxAssessedAmount.Value, 2);
            _MyDeclarationPM.DealValueWithFactor = Math.Round(customResponse.Response.Declaration.DMExtensions.CustomsValueComponent.TotalMADDealValueAmountNIS.Value, 2);
            _MyDeclarationPM.TaxationDateTime = Convert.ToDateTime(customResponse.Response.Declaration.DMExtensions.TaxationDateTime);
            //if (_IsSubmitDeclarationResponse != true) _MyDeclarationPM.IsChanged = false;
            if (requestParams.GetType() != typeof(DeclarationRestoreRequestParams))//Task 44715
            {
                if (_IsSubmitDeclarationResponse != true) _MyDeclarationPM.IsChanged = false;
            }

                //_MyDeclarationPM.DealValueWithoutFactor = customResponse.Response.Declaration.DMExtensions.CustomsValueComponent.TotalMADDealValueAmountNIS.Value;
                //_MyDeclarationPM.DealValueWithoutFactor = Math.Round(customResponse.Response.Declaration.DMExtensions.CustomsValueComponent.TotalMADDealValueAmountNIS.Value, 2);
                decimal DealValueWithoutFactor = 0;
            if (customResponse.Response.Declaration.GoodsShipment != null)
            {
                foreach (var goodsShipment in customResponse.Response.Declaration.GoodsShipment)
                {
                    if (goodsShipment.GovernmentAgencyGoodsItem != null)
                    {
                        foreach (var governmentAgencyGoodsItem in goodsShipment.GovernmentAgencyGoodsItem)
                        {
                            if (governmentAgencyGoodsItem.DMExtensions != null)
                            {
                                if (governmentAgencyGoodsItem.DMExtensions.GoodsItemAmount != null)
                                {
                                    foreach (var goodsItemAmount in governmentAgencyGoodsItem.DMExtensions.GoodsItemAmount)
                                    {
                                        if (goodsItemAmount.AmountType.Value == "15" && goodsItemAmount.CustomsValueAmount.currencyIDSpecified && goodsItemAmount.CustomsValueAmount.currencyID.ToString() == "ILS")
                                        {
                                            DealValueWithoutFactor += goodsItemAmount.CustomsValueAmount.Value;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (DealValueWithoutFactor != 0) _MyDeclarationPM.DealValueWithoutFactor = Math.Round(DealValueWithoutFactor, 2);

            if (requestParams.InterfaceTypeCode == "2750")
            {
                if (!string.IsNullOrWhiteSpace(RequestSheetContext.Current.GetContextOrDefault().SignByX509SubjectName))
                {
                    _MyDeclarationPM.IsSignedVersion = true;
                    _MyDeclarationPM.SignedByUserId = requestParams.LoggingUserId;
                    _MyDeclarationPM.SignerPersonalId = SignCertificateClass.GetPersonID(RequestSheetContext.Current.GetContextOrDefault().SignByX509SubjectName);
                }
                else
                {
                    _MyDeclarationPM.IsSignedVersion = false;
                    _MyDeclarationPM.SignedByUserId = null;
                    _MyDeclarationPM.SignerPersonalId = null;
                }
            }

            //Analyze the Errors section in the response XML 
            var swErrosXml1 = Stopwatch.StartNew();
            this._MyDeclarationPM.ErrosXml = mydDclarationErrorPointerService.AnalyzeErrorPionter(customResponse.Response.Error, _MyDeclarationPM, WCOTypeEnum.WCO, !_IsSubmitDeclarationResponse);
            this._MyDeclarationError = mydDclarationErrorPointerService._declarationErrorPointer;
            LogMessagingUtil.Instance.AppendLine("ErrosXml:Took:" + swErrosXml1.ElapsedMilliseconds);
           

            var swTax = Stopwatch.StartNew();
            //Update Declaration Taxes
            _MyDeclarationPM.DeclarationTaxes = GetDeclarationTaxesPM(customResponse);

            bool tester = false;
            if (tester)
            {
                int count = 0;
                foreach (var si in _MyDeclarationPM.SupplierInvoices)
                {
                    foreach (var sii in si.SupplierInvoiceItems)
                    {
                        foreach (var siic in sii.SupplierInvioceItemCertificats)
                        {
                            count++;
                        }
                    }
                }
            }

            //Update Declaration Item Taxes
            LogMessagingUtil.Instance.LogActionTime( () =>
            {
            _MyDeclarationPM.SupplierInvoices = GetSupplierInvoicesPM(customResponse);
            },"GetSupplierInvoicesPM");
            LogMessagingUtil.Instance.AppendLine("GetDeclarationTaxes+Item Tax:Took:" + swTax.ElapsedMilliseconds);

            if (tester)
            {
                int count = 0;
                foreach (var si in _MyDeclarationPM.SupplierInvoices)
                {
                    foreach (var sii in si.SupplierInvoiceItems)
                    {
                        foreach (var siic in sii.SupplierInvioceItemCertificats)
                        {
                            count++;
                        }
                    }
                }
            }

            //Build constraints
            if (this._IsSubmitDeclarationResponse != true)
            {
                DeleteDeclarationConstraints(myDeclarationConstraintUpdateService);
            }
            _MyDeclarationPM.DeclarationConstraints = BuildDeclarationConstraints(customResponse.Response.Error);

            //<--- Yuval Chalup 06.08.2015 TASK-15422
            _MyDeclarationPM.PaymentOrderNumber = null;
            _MyDeclarationPM.PaymentStatusCode = null;
            string paymentOrderNumber = null; //Yuval Chalup 10.08.2015 TASK-15472
            string paymentStatusCode = null; //Yuval Chalup 10.08.2015 TASK-15472
            if (customResponse.DeclarationPaymentDetails != null)
            {
                if (customResponse.DeclarationPaymentDetails.PaymentOrderNumber != null)
                {
                    _MyDeclarationPM.PaymentOrderNumber = customResponse.DeclarationPaymentDetails.PaymentOrderNumber.ToString();
                    paymentOrderNumber = customResponse.DeclarationPaymentDetails.PaymentOrderNumber.ToString(); //Yuval Chalup 10.08.2015 TASK-15472
                }
                if (customResponse.DeclarationPaymentDetails.PaymentOrderStatus != null)
                {
                    _MyDeclarationPM.PaymentStatusCode = customResponse.DeclarationPaymentDetails.PaymentOrderStatus.ToString();
                    paymentStatusCode = customResponse.DeclarationPaymentDetails.PaymentOrderStatus.ToString(); //Yuval Chalup 10.08.2015 TASK-15472

                }
            }
            //Yuval Chalup 06.08.2015 TASK-15422 --->

            //if(!(!string.IsNullOrWhiteSpace(paymentOrderNumber) && paymentStatusCode != "5"))
            //{
            //    _MyDeclarationPM.CurrentContextTag = Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.UpdateUnifreightBillingConst;
            //}
            //else
            //{
            //    //Payment date update
            //    if (declarationPaymentsPM.PaymentDate.HasValue)
            //    {
            //        if (customResponse != null)
            //        {
            //            bool setPaymentDateNull = false;
            //            if (customResponse.Response != null)
            //            {
            //                if (customResponse.Response.Status != null)
            //                {
            //                    if (customResponse.Response.Status.NameCode != null)
            //                    {
            //                        if (customResponse.Response.Status.NameCode.Value == "14")
            //                        {
            //                            setPaymentDateNull = true;
            //                            _MyDeclarationPM.PaymentDate = null;
            //                            LogMessagingUtil.Instance.AppendLine("Change Declaration Version From " + oldVersionId + "To " + _MyDeclarationPM.VersionId);
            //                            _MyDeclarationPM.CurrentContextTag = Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.UpdateUnifreightBillingConst; // moran 20.7.15 - Task 14521
            //                        }
            //                    }
            //                }
            //            }
            //            if (!setPaymentDateNull)
            //            {
            //                _MyDeclarationPM.PaymentDate = declarationPaymentsPM.PaymentDate;
            //            }
            //        }
            //        else
            //        {
            //            _MyDeclarationPM.PaymentDate = declarationPaymentsPM.PaymentDate;
            //        }
            //    }
            //    if (_MyDeclarationPM.CurrentContextTag != Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.UpdateUnifreightBillingConst) // moran 20.7.15 - Task 14521 - enter into 'if'
            //    {
            //        _MyDeclarationPM.CurrentContextTag = Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.CreateUnifreightPaymentConst;
            //    }
            //}
            if (!(!string.IsNullOrWhiteSpace(paymentOrderNumber) && paymentStatusCode != "5") && !(_IsSubmitDeclarationResponse == true && string.IsNullOrWhiteSpace(paymentOrderNumber) && _MyDeclarationPM.DeclarationStatusTypeCode == "5" && _MyDeclarationPM.TotalTax <= 5))
            {
                _MyDeclarationPM.CurrentContextTag = Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.UpdateUnifreightBillingConst;
                if (_MyDeclarationPM.IsCourierDeclaration && declarationPaymentsPM != null && declarationPaymentsPM.PaymentDate.HasValue && _IsSubmitDeclarationResponse == true && _MyDeclarationPM.DeclarationStatusTypeCode == "5")
                {
                    _MyDeclarationPM.PaymentDate = declarationPaymentsPM.PaymentDate;
                }

            }
            else
            {
                //Payment date update
                if (declarationPaymentsPM != null && declarationPaymentsPM.PaymentDate.HasValue)
                {
                    _MyDeclarationPM.PaymentDate = declarationPaymentsPM.PaymentDate;
                }
                //if (!string.IsNullOrWhiteSpace(customResponse.Response.Status.EffectiveDateTime))
                //{
                //    _MyDeclarationPM.PaymentDate = DateTime.Parse(customResponse.Response.Status.EffectiveDateTime);
                //}
                if (_MyDeclarationPM.CurrentContextTag != Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.UpdateUnifreightBillingConst)
                {
                    _MyDeclarationPM.CurrentContextTag = Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.CreateUnifreightPaymentConst;
                }
            }
            if (customResponse != null)
            {
                if (customResponse.Response != null)
                {
                    if (customResponse.Response.Status != null)
                    {
                        if (customResponse.Response.Status.NameCode != null)
                        {
                            if (customResponse.Response.Status.NameCode.Value == "14")
                            {
                                _MyDeclarationPM.PaymentDate = null;
                                LogMessagingUtil.Instance.AppendLine("Change Declaration Version From " + oldVersionId + "To " + _MyDeclarationPM.VersionId);
                                _MyDeclarationPM.CurrentContextTag = Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.UpdateUnifreightBillingConst;
                            }
                        }
                    }
                }
            }

            if (_TotalBtlCoverageNISSum > 0)
            {
                mydDclarationErrorPointerService = new DeclarationErrorPointerService();
                this._MyDeclarationPM.ErrosXml = mydDclarationErrorPointerService.AddErrorPionter(_MyDeclarationError, "", "", "", "", "", "", "", "A", @"לתיק זה קיימת הלוואת ביטוח לאומי ע""ס " + _TotalBtlCoverageNISSum.ToString() + @" ש""ח", "", "", "", "");
            } 
            
            if (customResponse.CollateralRequestDetails != null)
            {
                if (customResponse.CollateralRequestDetails.Count() > 0)
                {
                    foreach (UnifreightIIG.Common.ImportDeclarationServiceReference.CollateralRequestDetails collateralRequestItem in customResponse.CollateralRequestDetails)
                    {
                        this._MyDeclarationPM.ErrosXml = mydDclarationErrorPointerService.AddErrorPionter(_MyDeclarationError, "", "", "", "", "", "", "", "A", "המשוב להצהרה כולל דרישה לבטוחה " + " - מספר בטוחה " + collateralRequestItem.collateralRequestNumber, "", "", "", "");
                        //this._MyDeclarationPM.ErrosXml = mydDclarationErrorPointerService.AddErrorPionter(_MyDeclarationError, "", "", "", "", "", "", "", "A", @"המשוב להצהרה כולל דרישה לבטוחה ע""ס " + customResponse.CollateralRequestDetails[0].collateralRequestNumber + @" ש""ח - מספר בטוחה " + customResponse.CollateralRequestDetails[0].collateralRequestNumber, "", "", "", "");
                    }
                }
            }

            UpdateDepositionStatusCode();

            if (!String.IsNullOrWhiteSpace("itzik and yaron move to herer from DeclarationWebService.asmx"))
            {
                if (requestParams.GetType() != typeof(DeclarationRestoreRequestParams))//Task 44715 (add condition to itzik and yaron...
                {
                    _MyDeclarationPM.MarkAsChanged = false;
                    _MyDeclarationPM.IsChanged = false;
                }
            }
            _MyDeclarationPM.CustomsRequestsSheetId = requestParams.CustomsRequestsSheetId;
            _MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
            myDeclarationUpdateService.IsFromCustomsFeedback = true;
            myDeclarationUpdateService.Update(_MyDeclarationPM, true);

            if (_MyDeclarationPM.IsCourierDeclaration)
            {
                DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(context);
                DeclarationCourierStatusPM _MyDeclarationCourierStatusPM = new DeclarationCourierStatusPM();
                _MyDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(_MyDeclarationPM.Id, true, false);
                if (_MyDeclarationCourierStatusPM != null)
                {
                    DeclarationPendingPM declarationPendingPM_900 = null;
                    DeclarationPendingPM declarationPendingPM_901 = null;
                    if (_MyDeclarationCourierStatusPM.DeclarationPendings != null && _MyDeclarationCourierStatusPM.DeclarationPendings.Count() > 0)
                    {
                        declarationPendingPM_900 = _MyDeclarationCourierStatusPM.DeclarationPendings.Where(r => r.DeclarationID == _MyDeclarationCourierStatusPM.DeclarationId && r.CourierPendingReasonCode == "900").FirstOrDefault();
                        declarationPendingPM_901 = _MyDeclarationCourierStatusPM.DeclarationPendings.Where(r => r.DeclarationID == _MyDeclarationCourierStatusPM.DeclarationId && r.CourierPendingReasonCode == "901").FirstOrDefault();
                    }
                    // Pending 901
                    Boolean isSetPendingTo901 = false;
                    if (customResponse.Response.Error != null)
                    {
                        foreach (var errorItem in customResponse.Response.Error)
                        {
                            if (errorItem.ValidationCode != null && errorItem.ValidationCode.Value == "2382")
                            {
                                CourierPendingReasonQueryService myCourierPendingReasonQueryService = new CourierPendingReasonQueryService(context);
                                CourierPendingReasonPM courierPendingReasonPM = myCourierPendingReasonQueryService.GetSingle("901", false, false);
                                if (courierPendingReasonPM == null)
                                {
                                    LogMessagingUtil.Instance.AppendLine("לא קיים קוד Pending - הצהרה פלסטינאית = 901 בטבלת סיבות Pending");
                                    break;
                                }
                                LogMessagingUtil.Instance.AppendLine("Pending - הצהרה פלסטינאית = 901");
                                isSetPendingTo901 = true;
                                if (declarationPendingPM_901 == null)
                                {
                                    declarationPendingPM_901 = new DeclarationPendingPM();
                                    declarationPendingPM_901.CourierPendingReasonCode = "901";
                                    declarationPendingPM_901.Status = "A";
                                    declarationPendingPM_901.ChangeSetOp = ChangeSetOperation.Insert;
                                    _MyDeclarationCourierStatusPM.DeclarationPendings.Add(declarationPendingPM_901);
                                }
                                else if (declarationPendingPM_901.Status != "A")
                                {
                                    declarationPendingPM_901.ChangeSetOp = ChangeSetOperation.Update;
                                    declarationPendingPM_901.Status = "A";
                                }
                                if (declarationPendingPM_901.ChangeSetOp != ChangeSetOperation.None)
                                {
                                    LogMessagingUtil.Instance.AppendLine("Set Courier Pending Reason Code To 901");
                                    if (_MyDeclarationCourierStatusPM.ChangeSetOp != ChangeSetOperation.Update) _MyDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                                }
                            }
                        }
                    }
                    if (!isSetPendingTo901)
                    {
                        if (declarationPendingPM_901 != null)
                        {
                            declarationPendingPM_901.ChangeSetOp = ChangeSetOperation.Update;
                            declarationPendingPM_901.Status = "S";
                            if (_MyDeclarationCourierStatusPM.ChangeSetOp != ChangeSetOperation.Update) _MyDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                            LogMessagingUtil.Instance.AppendLine("Courier Pending Reason Code 901 Set as Solved");
                        }
                    }
                    // Pending 900
                    CourierMasterQueryService courierMasterService = new CourierMasterQueryService(requestParams.Tenant);
                    CourierMasterPM courierMaster = courierMasterService.GetSingle(_MyDeclarationPM.CourierMasterId, false, false);
                    if (courierMaster != null)
                    {
                        var myGDFDATAQueryService = new GDFDATAQueryService(AmitalContext.GetContext(requestParams.Tenant));
                        var def = myGDFDATAQueryService.GetSingle("ISRAEL", "CGO_ACT_COLLECT", "NON", courierMaster.IntegratorNumber, false, true);
                        bool isCollectActive = def.DEFDATA == "Y";
                        if (declarationPendingPM_900 == null)
                        {
                            CourierPendingReasonQueryService myCourierPendingReasonQueryService = new CourierPendingReasonQueryService(context);
                            CourierPendingReasonPM courierPendingReasonPM = myCourierPendingReasonQueryService.GetSingle("900", false, false);
                            if (courierPendingReasonPM == null)
                            {
                                LogMessagingUtil.Instance.AppendLine("לא קיים קוד תהליך גביה- במידה ומופעל בדיקה האם להגדיר גבייה = 900 בטבלת סיבות Pending");
                                isCollectActive = false;
                            }
                        }
                        if (isCollectActive)
                        {

                            LogMessagingUtil.Instance.AppendLine("תהליך גביה- במידה ומופעל בדיקה האם להגדיר גבייה = 900");
                            if (_MyDeclarationPM.SupplierInvoices != null && _MyDeclarationPM.SupplierInvoices.FirstOrDefault().IncotermCode != "DDP" && _MyDeclarationPM.TotalTax > 0)
                            {
                                if (declarationPendingPM_900 == null)
                                {
                                    declarationPendingPM_900 = new DeclarationPendingPM();
                                    declarationPendingPM_900.CourierPendingReasonCode = "900";
                                    declarationPendingPM_900.Status = "A";
                                    declarationPendingPM_900.ChangeSetOp = ChangeSetOperation.Insert;
                                    _MyDeclarationCourierStatusPM.DeclarationPendings.Add(declarationPendingPM_900);
                                }
                                else if (declarationPendingPM_900.Status != "A")
                                {
                                    declarationPendingPM_900.ChangeSetOp = ChangeSetOperation.Update;
                                    declarationPendingPM_900.Status = "A";
                                }
                                if (declarationPendingPM_900.ChangeSetOp != ChangeSetOperation.None)
                                {
                                    LogMessagingUtil.Instance.AppendLine("Set Courier Pending Reason Code 900");
                                    if (_MyDeclarationCourierStatusPM.ChangeSetOp != ChangeSetOperation.Update) _MyDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                                }
                            }
                            else if (declarationPendingPM_900 != null)
                            {
                                declarationPendingPM_900.ChangeSetOp = ChangeSetOperation.Update;
                                declarationPendingPM_900.Status = "S";
                                if (_MyDeclarationCourierStatusPM.ChangeSetOp != ChangeSetOperation.Update) _MyDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                                LogMessagingUtil.Instance.AppendLine("Courier Pending Reason Code 900 Set as Solved");
                            }
                        }
                    }

                }
                if (_MyDeclarationCourierStatusPM.ChangeSetOp == ChangeSetOperation.Update)
                {
                    DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(context, new Dictionary<string, IContext>(), _MyDeclarationPM.Tenant);
                    declarationCourierStatusUpdateService.Update(_MyDeclarationCourierStatusPM, true);
                }
                /*
                DeclarationPendingQueryService myCourierPendingReasonQueryService = new DeclarationPendingQueryService(context);
                DeclarationPendingPM declarationPendingPM_900 = myCourierPendingReasonQueryService.GetSingle(_MyDeclarationPM.Id, "900", false, false);
                DeclarationPendingPM declarationPendingPM_901 = myCourierPendingReasonQueryService.GetSingle(_MyDeclarationPM.Id, "901", false, false);
                
                //DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(context);
                //DeclarationCourierStatusPM currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(_MyDeclarationPM.Id, false, false);
                //if (currentDeclarationCourierStatusPM != null && 
                //    (string.IsNullOrEmpty(currentDeclarationCourierStatusPM.CourierPendingReasonCode) || currentDeclarationCourierStatusPM.CourierPendingReasonCode == "900" || currentDeclarationCourierStatusPM.CourierPendingReasonCode == "901"))
                
                {
                    // Pending 901
                    Boolean isSetPendingTo901 = false;
                    if (customResponse.Response.Error != null)
                    {
                        foreach (var errorItem in customResponse.Response.Error)
                        {
                            if (errorItem.ValidationCode != null && errorItem.ValidationCode.Value == "2382")
                            {
                                LogMessagingUtil.Instance.AppendLine("Pending - הצהרה פלסטינאית = 901");
                                isSetPendingTo901 = true;
                                //currentDeclarationCourierStatusPM.CourierPendingReasonCode = "901";
                                //currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                                if (declarationPendingPM_901 == null)
                                {
                                    declarationPendingPM_901 = new DeclarationPendingPM();
                                    declarationPendingPM_901.CourierPendingReasonCode = "901";
                                }
                                declarationPendingPM_901.ChangeSetOp = ChangeSetOperation.Update;
                                declarationPendingPM_901.Status = "A";
                                LogMessagingUtil.Instance.AppendLine("Set Courier Pending Reason Code To 901");
                            }
                        }
                    }
                    if (!isSetPendingTo901)
                    {

                        //if (currentDeclarationCourierStatusPM.CourierPendingReasonCode == "901")
                        if (declarationPendingPM_901 != null)
                        {
                            //currentDeclarationCourierStatusPM.CourierPendingReasonCode = null;
                            //currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                            declarationPendingPM_901.ChangeSetOp = ChangeSetOperation.Update;
                            declarationPendingPM_901.Status = "S";
                            LogMessagingUtil.Instance.AppendLine("Courier Pending Reason Code 901 Set as Solved");
                        }

                        // Pending 900
                        CourierMasterQueryService courierMasterService = new CourierMasterQueryService(requestParams.Tenant);
                        CourierMasterPM courierMaster = courierMasterService.GetSingle(_MyDeclarationPM.CourierMasterId, false, false);
                        if (courierMaster != null)
                        {
                            var myGDFDATAQueryService = new GDFDATAQueryService(AmitalContext.GetContext(requestParams.Tenant));
                            var def = myGDFDATAQueryService.GetSingle("ISRAEL", "CGO_ACT_COLLECT", "NON", courierMaster.IntegratorNumber, false, true);
                            bool isCollectActive = def.DEFDATA == "Y";
                            if (isCollectActive)
                            {
                                LogMessagingUtil.Instance.AppendLine("תהליך גביה- במידה ומופעל בדיקה האם להגדיר גבייה = 900");
                                if (_MyDeclarationPM.SupplierInvoices != null && _MyDeclarationPM.SupplierInvoices.FirstOrDefault().IncotermCode != "DDP" && _MyDeclarationPM.TotalTax > 0)
                                {
                                    //currentDeclarationCourierStatusPM.CourierPendingReasonCode = "900";
                                    //currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                                    if (declarationPendingPM_900 == null)
                                    {
                                        declarationPendingPM_900 = new DeclarationPendingPM();
                                        declarationPendingPM_900.CourierPendingReasonCode = "900";
                                    }
                                    declarationPendingPM_900.ChangeSetOp = ChangeSetOperation.Update;
                                    declarationPendingPM_900.Status = "A";
                                    LogMessagingUtil.Instance.AppendLine("Set Courier Pending Reason Code 900");
                                }
                                //else if (currentDeclarationCourierStatusPM.CourierPendingReasonCode == "900")
                                else if (declarationPendingPM_900 != null)
                                {
                                    //currentDeclarationCourierStatusPM.CourierPendingReasonCode = null;
                                    //currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                                    declarationPendingPM_900.ChangeSetOp = ChangeSetOperation.Update;
                                    declarationPendingPM_900.Status = "S";
                                    LogMessagingUtil.Instance.AppendLine("Courier Pending Reason Code 900 Set as Solved");
                                }
                            }
                        }
                    }

                    //if (currentDeclarationCourierStatusPM.ChangeSetOp == ChangeSetOperation.Update)
                    //{
                    //  DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
                    //declarationCourierStatusUpdateService.Update(currentDeclarationCourierStatusPM, true);
                    //}
                    
                    DeclarationPendingUpdateService declarationPendingUpdateService = new DeclarationPendingUpdateService(context, new Dictionary<string, IContext>(), _MyDeclarationPM.Tenant);
                    if (declarationPendingPM_900 != null && declarationPendingPM_900.ChangeSetOp == ChangeSetOperation.Update)
                    {
                        declarationPendingUpdateService.Update(declarationPendingPM_900, true);
                    }
                    if (declarationPendingPM_901 != null && declarationPendingPM_901.ChangeSetOp == ChangeSetOperation.Update)
                    {
                        declarationPendingUpdateService.Update(declarationPendingPM_901, true);
                    }
                }
                */
            }

            //var setting = CustomsSettingQueryService.GetSettingByTenant(_MyDeclarationPM.Tenant);
            //if (setting.IsConnectedToUniFreight)
            if (_MyDeclarationPM.IsConnectedToUnifreight)
            {
                //<--- Yuval Chalup 09.11.2015 TASK-16498 - Update PaymentOrderNumber in Payment
                if (customResponse.DeclarationPaymentDetails != null)
                {
                    if (customResponse.DeclarationPaymentDetails.PaymentOrderNumber != null || (_IsSubmitDeclarationResponse == true && string.IsNullOrWhiteSpace(paymentOrderNumber) && _MyDeclarationPM.DeclarationStatusTypeCode == "5" && _MyDeclarationPM.TotalTax <= 5))
                    {
                        var unifreightDeclarationPaymentUpdateService = new UnifreightDeclarationPaymentUpdateService(declarationPaymentsPM, _MyDeclarationPM);
                        unifreightDeclarationPaymentUpdateService.Update();
                    }
                }
            }
            //Yuval Chalup 09.11.2015 TASK-16498 --->

            if (customResponse.CollateralRequestDetails != null) // Create Collateral
            {
                LogMessagingUtil.Instance.AppendLine("CollateralRequestDetails: Create Collateral");
                //var headerXml = XmlGenericUtil<UnifreightIIG.Common.ImportDeclarationServiceReference.RequestContentHeader>
                //    .SerializeObject(customResponse.);
                //var header = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Collateral.RequestContentHeader>.DeSerializeObject(headerXml);
                var requestXml = XmlGenericUtil<UnifreightIIG.Common.ImportDeclarationServiceReference.CollateralRequestDetails[]>
                    .SerializeObject(customResponse.CollateralRequestDetails);
                var collateralArry = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Collateral.CollateralRequestDetails[]>.DeSerializeObject(requestXml);

                COLT_NG_8211_MSG10040_CollateralRequestMsg myCOLT_NG_8211_MSG10040_CollateralRequestMsg = new COLT_NG_8211_MSG10040_CollateralRequestMsg();
                myCOLT_NG_8211_MSG10040_CollateralRequestMsg.ResponseContentHeader = null;
                myCOLT_NG_8211_MSG10040_CollateralRequestMsg.CollateralRequestDetails = collateralArry;
                var xml = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Collateral.COLT_NG_8211_MSG10040_CollateralRequestMsg>
                    .SerializeObject(myCOLT_NG_8211_MSG10040_CollateralRequestMsg);

                var ser = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Collateral.COLT_NG_8211_MSG10040_CollateralRequestMsg>.DeSerializeObject(xml);
                var DF_MSG10040_CollateralRequestMsgResponseService = new DF_8211_CollateralRequestMsgResponseService();
                DF_MSG10040_CollateralRequestMsgResponseService.Update(ser, requestParams);
            }

            this.MyResponseData.UserMessage = "בקשה נשלחה בהצלחה";
            string declarationStatusTypeName = _MyDeclarationPM.DeclarationStatusTypeCode;
            if (!string.IsNullOrWhiteSpace(_MyDeclarationPM.DeclarationStatusTypeCode))
            {
                DeclarationStatusTypeQueryService declarationStatusTypeQueryService = new DeclarationStatusTypeQueryService(_MyDeclarationPM.Tenant);
                DeclarationStatusTypePM declarationStatusType = declarationStatusTypeQueryService.GetSingle(_MyDeclarationPM.DeclarationStatusTypeCode, false, true);
                if (declarationStatusType != null)
                {
                    declarationStatusTypeName = declarationStatusType.LocalName;
                }
                this.MyResponseData.UserMessage = "בקשה נשלחה - סטטוס הטיוטה " + declarationStatusTypeName;
            }

            MyResponseData.ApplicationID = requestParams.AppicationId;
            MyResponseData.Succeeded = true;
            if (_MyDeclarationPM.CurrentContextTag == Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.CreateUnifreightPaymentConst) // moran 28.1.15 - Task 10005
            {
                if (!_MyDeclarationPM.IsCourierDeclaration)
                {
                    SendDeclarationPrint(_MyDeclarationPM, SendRequestVIA.WebServiceBatch, requestParams);
                }
            }
            if (requestParams.InterfaceTypeCode == "8373")
            {
                myDeclarationUpdateService.SendDelayedDeclarationStatusRequest(_MyDeclarationPM);
            }
        }


        private void UpdateDepositionStatusCode()
        {
            if (_MyDeclarationError != null && _MyDeclarationError.Entitites != null && _MyDeclarationError.Entitites.Count > 0)
            {
                //Go over all the 'Entity'
                foreach (var entity in _MyDeclarationError.Entitites)
                {
                    if (entity.FieldErrors != null)
                    {
                        //Go over all the 'FieldErrors'
                        foreach (var fieldErrors in entity.FieldErrors)
                        {
                            //Get all 'FieldErrors' for the 'FieldError'
                            List<field> fieldList = (from a in entity.FieldErrors
                                                     where (a.Code == "4589")
                                                     select a).ToList();
                            if (fieldList.Count > 0)
                            {
                                if (string.IsNullOrWhiteSpace(_MyDeclarationPM.DepositionStatusCode)) _MyDeclarationPM.DepositionStatusCode = "R";
                                return;
                            }
                        }
                    }
                }
            }
            if (_MyDeclarationPM.DepositionStatusCode == "R") _MyDeclarationPM.DepositionStatusCode = null;
        }

        public void SendDeclarationPrint(DeclarationPM declarationPM, SendRequestVIA RequestVIA, GenericRequestParams requestParams) // moran 28.1.15 - Task 10005
        {

            LogMessagingUtil.Instance.AppendLine("SendDeclarationPrint");
            string decNum = declarationPM.DeclarationNumber;
            var decNumList = new List<string>();
            decNumList.Add(decNum);
            DF_NG_8302_Web03_DeclarationPrintRequestParams searchParams = new DF_NG_8302_Web03_DeclarationPrintRequestParams()
            {
                LoggingEnabled = true,
                CustomFileNo = declarationPM.CustomFileNo,
                DeclarationNumber = decNumList, //declarationPM.DeclarationNumber,
                Tenant = declarationPM.Tenant,
                RequestName = "Declaration Print (2750)",
                ResponseName = "Declaration Print (2750)",
                LoggingEntityId = declarationPM.Id,


                LoggingUserId = requestParams.LoggingUserId, //HD CALL#298426
            };
            
            searchParams.RequestVIA = RequestVIA; // SendRequestVIA.WebServiceBatch;
            var myRequestMessagingService = new DF_NG_8302_Web03_DeclarationPrintMessagingService();
            var resData = myRequestMessagingService.Send(searchParams);
            if (!resData.Succeeded)
            {
                LogMessagingUtil.Instance.AppendLine("Request Failed " + resData.CustomsRequestsSheetId + ", Message: " + resData.UserMessage);
                return;
            }
            LogMessagingUtil.Instance.AppendLine("Request Succeeded " + resData.CustomsRequestsSheetId);
        }

        //ITZIK+MIRT  private string GetErrosXmlFromResponseHeaderExeption()          {              return _ResponseHeaderExeption.ErrorDescription;         }

        private void DeleteDeclarationConstraints(DeclarationConstraintUpdateService myDeclarationConstraintUpdateService) // Delete old Constraints
        {
            if (this._MyDeclarationPM.DeclarationConstraints == null)
            {
                return;
            }

            foreach (var constraintItem in this._MyDeclarationPM.DeclarationConstraints)
            {
                constraintItem.ChangeSetOp = ChangeSetOperation.Delete;
                //myDeclarationConstraintUpdateService.Update(constraintItem, true);
                _MyDeclarationPM.DeletedDeclarationConstraints.Add(constraintItem);
            }
        }

        private List<DeclarationConstraintPM> BuildDeclarationConstraints(ResponseError[] responseError)
        {
            if (responseError == null)
            {
                return null;
            }
            //This section that stops analyzing when ConstraintID appears more than once in the response is CANCELLED - 
            //The new logic (below) analyze ONLY the first constraint of that ConstraintID
            //try
            //{
            //    responseError.Where(errorItem => errorItem.DMExtensions != null).ToList().ToDictionary(r => r.DMExtensions.ConstraintID.ToString());
            //}
            //catch (System.Exception)
            //{

            //    throw new System.Exception("הגיעה מספר אילוץ כפול, נא לפנות למלמ");
            //}

            var declarationConstraintPMList = new List<DeclarationConstraintPM>();
            foreach (var errorItem in responseError)
            {
                if (errorItem.DMExtensions != null)
                {
                    DeclarationConstraintPM declarationConstraintToCheckDistinct = null;
                    declarationConstraintToCheckDistinct = declarationConstraintPMList.Where(constraint => constraint.ConstraintNumber == errorItem.DMExtensions.ConstraintID.ToString()).FirstOrDefault();

                    if (declarationConstraintToCheckDistinct == null)
                    {
                        DeclarationConstraintPM declarationConstraint = null;
                        declarationConstraint = FindConstraintInList(errorItem.DMExtensions.ConstraintID.ToString(), _MyDeclarationPM.Tenant, _MyDeclarationPM.Id);

                        if (declarationConstraint == null)
                        {
                            declarationConstraint = new DeclarationConstraintPM();
                            declarationConstraint.ChangeSetOp = ChangeSetOperation.Insert;
                            declarationConstraint.ConstraintNumber = errorItem.DMExtensions.ConstraintID.ToString();
                            declarationConstraint.ConstraintTypeCode = errorItem.DMExtensions.ConstraintType.ToString();
                        }
                        else
                        {
                            declarationConstraint.ChangeSetOp = ChangeSetOperation.Update;
                            _MyDeclarationPM.DeletedDeclarationConstraints.Remove(declarationConstraint);
                        }

                        declarationConstraint.ConstraintStatusCode = errorItem.DMExtensions.ConstraintStatus.ToString();
                        declarationConstraintPMList.Add(declarationConstraint);
                    }
                }
            }
            return declarationConstraintPMList;
        }
        private List<DeclarationConstraintPM> BuildDeclarationConstraintsTEST(ResponseError[] responseError)
        {
            if (responseError == null)
            {
                return null;
            }

            var declarationConstraintPMList = new List<DeclarationConstraintPM>();

            try
            {
                responseError.Where(errorItem => errorItem.DMExtensions != null).ToList().ToDictionary(r => r.DMExtensions.ConstraintID.ToString());
            }
            catch (System.Exception)
            {

                throw new System.Exception("הגיעה מספר אילוץ כפול, נא לפנות למלמ");                
            }
            

            var myhshSet = new HashSet<string>();
            foreach (var item in responseError.Where(errorItem => errorItem.DMExtensions != null))
            {

                myhshSet.Add(item.DMExtensions.ConstraintID.ToString());
            }
            foreach (var hshSetKey in myhshSet)
            {
                ResponseError errorItem = null;
                errorItem = responseError.LastOrDefault(r => r.DMExtensions !=null && r.DMExtensions.ConstraintID.ToString() == hshSetKey);

                //if (errorItem.DMExtensions != null)
                {
                    DeclarationConstraintPM declarationConstraint = null;
                    declarationConstraint = FindConstraintInList(errorItem.DMExtensions.ConstraintID.ToString(), _MyDeclarationPM.Tenant, _MyDeclarationPM.Id);

                    if (declarationConstraint == null)
                    {
                        declarationConstraint = new DeclarationConstraintPM();
                        declarationConstraint.ChangeSetOp = ChangeSetOperation.Insert;
                        declarationConstraint.ConstraintNumber = errorItem.DMExtensions.ConstraintID.ToString();
                        declarationConstraint.ConstraintTypeCode = errorItem.DMExtensions.ConstraintType.ToString();
                    }
                    else
                    {
                        declarationConstraint.ChangeSetOp = ChangeSetOperation.Update;
                        _MyDeclarationPM.DeletedDeclarationConstraints.Remove(declarationConstraint);
                    }

                    declarationConstraint.ConstraintStatusCode = errorItem.DMExtensions.ConstraintStatus.ToString();
                    declarationConstraintPMList.Add(declarationConstraint);
                }
            }
            return declarationConstraintPMList;
        }

        private DeclarationConstraintPM FindConstraintInList(string constraintNumber, int tenant, string declarationID)
        {
            List<DeclarationConstraintPM> constraintList = (from a in _MyDeclarationPM.DeclarationConstraints
                                                            where a.ConstraintNumber == constraintNumber &&
                                                            a.Tenant == tenant && a.DeclarationID == declarationID
                                                            select a).ToList();

            if (constraintList.Count > 0)
            {
                return constraintList[0];
            }
            return null;
        }

        private void DeleteSupplierInvoiceItemsTaxes(SupplierInvoiceItemsTaxUpdateService mySupplierInvoiceItemsTaxUpdateService)
        {

            foreach (var si in _MyDeclarationPM.SupplierInvoices)
            {
                foreach (var supplierInvoiceItem in si.SupplierInvoiceItems)
                {
                    foreach (var item in supplierInvoiceItem.SupplierInvoiceItemTaxes)
                    {
                        item.ChangeSetOp = ChangeSetOperation.Delete;
                        mySupplierInvoiceItemsTaxUpdateService.Update(item, true);
                    }
                }
            }
        }

        private void DeleteDeclarationTaxes(DeclarationTaxUpdateService myDeclarationTaxUpdateService)
        {

            foreach (var item in _MyDeclarationPM.DeclarationTaxes)
            {
                item.ChangeSetOp = ChangeSetOperation.Delete;
                myDeclarationTaxUpdateService.Update(item, true);
            }
        }


        private void DeleteSupplierInvoiceItemsVehicleMods(SupplierInvoiceItemVehicleModUpdateService mySupplierInvoiceItemVehicleModUpdateService) // moran 20.10.15 - Task 17209
        {

            foreach (var si in _MyDeclarationPM.SupplierInvoices)
            {
                foreach (var supplierInvoiceItem in si.SupplierInvoiceItems)
                {
                    foreach (var vehicle in supplierInvoiceItem.SupplierInvoiceItemVehicles)
                    {
                        foreach (var vehicleMod in vehicle.SupplierInvoiceItemVehicleMods)
                        {
                            vehicleMod.ChangeSetOp = ChangeSetOperation.Delete;
                            mySupplierInvoiceItemVehicleModUpdateService.Update(vehicleMod, true);
                        }
                    }
                }
            }
        }


        private void DeleteSupplierInvoiceItemsModVehicles(SupplierInvoiceItemModVehicleUpdateService mySupplierInvoiceItemModVehicleUpdateService) // moran 24.11.15 - Task 17424
        {

            foreach (var si in _MyDeclarationPM.SupplierInvoices)
            {
                foreach (var supplierInvoiceItem in si.SupplierInvoiceItems)
                {
                    foreach (var modVehicle in supplierInvoiceItem.SupplierInvoiceItemModVehicles)
                    {
                        modVehicle.ChangeSetOp = ChangeSetOperation.Delete;
                        mySupplierInvoiceItemModVehicleUpdateService.Update(modVehicle, true);
                    }
                }
            }
        }

        private void DeleteSupplierInvioceItemCertificates(SupplierInvioceItemCertificatUpdateService mySupplierInvioceItemCertificatUpdateService)
        {
            foreach (var si in _MyDeclarationPM.SupplierInvoices)
            {
                foreach (var supplierInvoiceItem in si.SupplierInvoiceItems)
                {
                    foreach (var modCertificats in supplierInvoiceItem.SupplierInvioceItemCertificats)
                    {
                        modCertificats.ChangeSetOp = ChangeSetOperation.Delete;
                        mySupplierInvioceItemCertificatUpdateService.Update(modCertificats, true);
                    }
                }
            }
        }

        private List<SupplierInvoicePM> GetSupplierInvoicesPM(
            DF_NG_2754_MSG10004_ImportDeclarationResponse customResponse)
        {
            var supplierInvoicesPMList = new List<SupplierInvoicePM>();

            foreach (var goodsShipment in customResponse.Response.Declaration.GoodsShipment)
            {
                //var supplierInvoiceItemsTaxPM = new SupplierInvoiceItemsTaxPM();
                //var supplierInvoicePM = this._MyDeclarationPM.SupplierInvoices.FirstOrDefault(si => si.InvoiceNumber == goodsShipment.Invoice.ID.Value);
                var supplierInvoicePM = this._MyDeclarationPM.SupplierInvoices.FirstOrDefault(si => si.SequenceNumeric == goodsShipment.SequenceNumeric);

                if (supplierInvoicePM == null)
                {
                    throw new System.Exception(
                        "unable to find the supplierInvoicePM from goodsShipment.Invoice.ID.Value " + goodsShipment.Invoice.ID.Value);
                }
                var InvoiceCounterKey = supplierInvoicePM.InvoiceCounterKey;

                //Update supplier Valuation - Additional costs details from custom
                supplierInvoicePM.SupplierInvoiceModifications = GetSupplierInvoiceModifications(goodsShipment, ref supplierInvoicePM);
                //Update supplier items
                supplierInvoicePM.SupplierInvoiceItems = GetSupplierInvoiceItems(goodsShipment, ref supplierInvoicePM);

                if (goodsShipment.Invoice != null && goodsShipment.Invoice.DMExtensions != null && goodsShipment.Invoice.DMExtensions.RateNumeric != null)
                {
                    supplierInvoicePM.ExchangeRate = goodsShipment.Invoice.DMExtensions.RateNumeric.Value;
                }

                supplierInvoicePM.ChangeSetOp = ChangeSetOperation.Update;
                supplierInvoicesPMList.Add(supplierInvoicePM);
            }

            return supplierInvoicesPMList;
        }

        //private List<SupplierInvoiceModificationPM> GetSupplierInvoiceModifications(DeclarationGoodsShipment goodsShipment, ref SupplierInvoicePM supplierInvoicePM)
        //{
        //    // moran 26.5.15 - 13564 -->
        //    //var supplierInvoiceModificationPMList = new List<SupplierInvoiceModificationPM>();
        //    var supplierInvoiceModificationPMList = new List<SupplierInvoiceModificationPM>(supplierInvoicePM.SupplierInvoiceModifications);
        //    // moran 26.5.15 - 13564 <--
        //    if (goodsShipment.CustomsValuation == null)
        //    {
        //        return null;
        //    }

        //    //Check if there is a DECLARED Fee (I01) in message
        //    DeclarationGoodsShipmentCustomsValuation declarationGoodsShipmentCustomsValuation_I01 = goodsShipment.CustomsValuation.FirstOrDefault(rec => rec.ChargesTypeCode.Value == "I01");

        //    foreach (var valuationItem in goodsShipment.CustomsValuation)
        //    {
        //        if (valuationItem.ChargesTypeCode.Value != "67" && valuationItem.ChargesTypeCode.Value != "144")
        //        {
        //            //If there is a DECLARED Fee (I01) in message:
        //            //1 - Do NOT get the CALCULATED Fee (I02) from message
        //            //2 - Delete the CALCULATED from DB
        //            if (declarationGoodsShipmentCustomsValuation_I01 != null && valuationItem.ChargesTypeCode.Value == "I02")
        //            {
        //                var supplierInvoiceModificationPM = supplierInvoicePM.SupplierInvoiceModifications.FirstOrDefault(si => si.TypeCode == valuationItem.ChargesTypeCode.Value);
        //                if (supplierInvoiceModificationPM != null)
        //                {
        //                    //If exist delete
        //                    supplierInvoiceModificationPM.ChangeSetOp = ChangeSetOperation.Delete;
        //                    supplierInvoiceModificationPMList.Add(supplierInvoiceModificationPM);
        //                }
        //            }
        //            else
        //            {
        //                var supplierInvoiceModificationPM = supplierInvoicePM.SupplierInvoiceModifications.FirstOrDefault(si => si.TypeCode == valuationItem.ChargesTypeCode.Value);
        //                if (supplierInvoiceModificationPM != null)
        //                {
        //                    //If exist update
        //                    supplierInvoiceModificationPM.ChangeSetOp = ChangeSetOperation.Update;
        //                }
        //                else
        //                {
        //                    //Else create new SupplierInvoiceModification record
        //                    supplierInvoiceModificationPM = new SupplierInvoiceModificationPM();
        //                    supplierInvoiceModificationPM.ChangeSetOp = ChangeSetOperation.Insert;
        //                }
        //                supplierInvoiceModificationPM.TypeCode = valuationItem.ChargesTypeCode.Value;
        //                supplierInvoiceModificationPM.CurrencyTypeCode = valuationItem.OtherChargeDeductionAmount.currencyID.ToString(); // TO CHECK? ENUM?
        //                supplierInvoiceModificationPM.Amount = valuationItem.OtherChargeDeductionAmount.Value;
        //                if (supplierInvoiceModificationPM.ChangeSetOp == ChangeSetOperation.Insert)
        //                {
        //                    supplierInvoiceModificationPMList.Add(supplierInvoiceModificationPM);
        //                }
        //            }
        //        }
        //    }

        //    return supplierInvoiceModificationPMList;
        //}
#if refreshWSDL20141230
        private SupplierInvoiceItemsModificationPM GetSupplierInvoiceItemsTaxesModifications(
            DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsValuationDeductionAdjustment 
            declarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsValuationDeductionAdjustment)
        {

            return null;
        }
#endif


        private List<SupplierInvoiceItemPM> GetSupplierInvoiceItems(
            DeclarationGoodsShipment goodsShipment,
            ref SupplierInvoicePM supplierInvoicePM)
        {
            var supplierInvoiceItemsPMList = new List<SupplierInvoiceItemPM>();
            if (supplierInvoicePM.IsAccumalated == true && supplierInvoicePM.SupplierInvoiceItems != null && supplierInvoicePM.SupplierInvoiceItems.Count > 0)
            {
                supplierInvoicePM.SupplierInvoiceItems.RemoveAll(rec => rec.IsParent != true);
            }
            foreach (var governmentAgencyGoodsItem in goodsShipment.GovernmentAgencyGoodsItem)
            {
                var supplierInvoiceItemPM = supplierInvoicePM.SupplierInvoiceItems.FirstOrDefault(si => si.SequenceNumeric == governmentAgencyGoodsItem.SequenceNumeric);
                //<--- Added by Yuval Chalup 26.05.2015 TASK-13473
                if (supplierInvoiceItemPM == null)
                {
                    if (governmentAgencyGoodsItem.Commodity != null)
                    {
                        if (governmentAgencyGoodsItem.Commodity.Classification != null)
                        {
                            if (governmentAgencyGoodsItem.Commodity.Classification.Count() > 0)
                            {
                                if (governmentAgencyGoodsItem.Commodity.Classification[0] != null)
                                {
                                    throw new System.Exception(
                                       "unable to find the supplierInvoiceItemPM from governmentAgencyGoodsItem.Commodity.Classification " + governmentAgencyGoodsItem.Commodity.Classification[0].ID.Value);
                                }
                            }
                        }
                    }
                    throw new System.Exception(
                       "unable to find the supplierInvoiceItemPM from governmentAgencyGoodsItem.SequenceNumeric " + governmentAgencyGoodsItem.SequenceNumeric);
                }
                //Added by Yuval Chalup 26.05.2015 TASK-13473 --->

                var supplierInvoiceItemsTaxPMList = new List<SupplierInvoiceItemsTaxPM>();
                //Update supplier item Valuation Adjustment - Commodity price adjustments
                supplierInvoiceItemPM.SupplierInvoiceItemsMods = GetSupplierInvoiceItemsModifications(governmentAgencyGoodsItem.ValuationAdjustment, supplierInvoiceItemPM);
                // moran 24.11.15 - Task 17424 -->
                supplierInvoiceItemPM.SupplierInvoiceItemModVehicles = GetSupplierInvoiceItemsModVehicles(governmentAgencyGoodsItem.DMExtensions.VehicleValuationAdjustment, supplierInvoiceItemPM);
                // moran 24.11.15 - Task 17424 <--

                supplierInvoiceItemPM.SupplierInvioceItemCertificats = GetSupplierInvioceItemCertificats(supplierInvoicePM, supplierInvoiceItemPM);

                if (governmentAgencyGoodsItem.Commodity == null)
                {
                    continue;
                }
                //TODO:DDDD

                if (governmentAgencyGoodsItem.Commodity.DutyTaxFee != null)
                {
                    foreach (var dutyTaxFee in governmentAgencyGoodsItem.Commodity.DutyTaxFee)
                    {
                        var supplierInvoiceItemsTaxPM = new SupplierInvoiceItemsTaxPM();
                        supplierInvoiceItemsTaxPM.ChangeSetOp = ChangeSetOperation.Insert;
                        //supplierInvoiceItemsTaxPM.DeclarationId = this._MyDeclarationPM.Id; //Removed by Yuval Chalup 26.05.2015 TASK-13473 (Move to SupplierInvoiceItemsTaxUpdateService.OnUpdating)
                        supplierInvoiceItemsTaxPM.Tenant = this._MyDeclarationPM.Tenant;
                        //supplierInvoiceItemsTaxPM.InvoiceCounterKey = supplierInvoicePM.InvoiceCounterKey; //Removed by Yuval Chalup 26.05.2015 TASK-13473 (Move to SupplierInvoiceItemsTaxUpdateService.OnUpdating)
                        //supplierInvoiceItemsTaxPM.LineNumber = supplierInvoiceItemPM.LineNumber; //Removed by Yuval Chalup 26.05.2015 TASK-13473 (Move to SupplierInvoiceItemsTaxUpdateService.OnUpdating)
                        supplierInvoiceItemsTaxPM.TaxTypeCode = dutyTaxFee.TypeCode.Value;
                        if (dutyTaxFee.DutyRegimeCode != null)
                        {
                            supplierInvoiceItemsTaxPM.TradeAgreementTypeCode = dutyTaxFee.DutyRegimeCode.Value;
                        }
                        supplierInvoiceItemsTaxPM.TaxRate = dutyTaxFee.TaxRate;
                        supplierInvoiceItemsTaxPM.TaxBaseAmount = dutyTaxFee.AdValoremTaxBaseAmount.Value;
                        supplierInvoiceItemsTaxPM.TaxAmount = dutyTaxFee.DMExtensions.CalculatedTax.Amount.Value;
                        supplierInvoiceItemsTaxPM.DeferedTaxAmount = dutyTaxFee.DMExtensions.CalculatedTax.DeferedTaxAmount.Value;
                        if (dutyTaxFee.DMExtensions.CalculatedTax.DefinedPerUnitMethod != null)
                        {
                            supplierInvoiceItemsTaxPM.DefinedPerUnitMeasure = dutyTaxFee.DMExtensions.CalculatedTax.DefinedPerUnitMethod.Value;
                        }
                        supplierInvoiceItemsTaxPM.AlternateRate = dutyTaxFee.DMExtensions.CalculatedTax.AlternateRate.Value;
                        if (dutyTaxFee.DMExtensions.CalculatedTax.AlternateDefinedPerUnitMeasure != null)
                        {
                            supplierInvoiceItemsTaxPM.AlternateDefinedPerUnitMeasure = dutyTaxFee.DMExtensions.CalculatedTax.AlternateDefinedPerUnitMeasure.Value;
                        }
                        if (dutyTaxFee.DMExtensions.CalculatedTax.DefinedPerUnitQuantity != null)
                        {
                            supplierInvoiceItemsTaxPM.DefinedPerUnitQuantity = dutyTaxFee.DMExtensions.CalculatedTax.DefinedPerUnitQuantity.Value;
                        }
                        if (dutyTaxFee.DMExtensions.CalculatedTax.AlternateDefinedPerUnitQuantity != null)
                        {
                            supplierInvoiceItemsTaxPM.AlternateDefinedPerUnitQuant = dutyTaxFee.DMExtensions.CalculatedTax.AlternateDefinedPerUnitQuantity.Value;
                        }
                        if (dutyTaxFee.DMExtensions.CalculatedTax.MeasurementUnitCode != null)
                        {
                            supplierInvoiceItemsTaxPM.MeasurementUnitCode = dutyTaxFee.DMExtensions.CalculatedTax.MeasurementUnitCode.Value;
                        }
                        if (dutyTaxFee.DMExtensions.CalculatedTax.AlternateMeasurementUnit != null)
                        {
                            supplierInvoiceItemsTaxPM.AlternateMeasurementUnitCode = dutyTaxFee.DMExtensions.CalculatedTax.AlternateMeasurementUnit.Value;
                        }
                        if (dutyTaxFee.DMExtensions.CalculatedTax.TradeLevyNumber != null)
                        {
                            supplierInvoiceItemsTaxPM.TradeLevyNumber = dutyTaxFee.DMExtensions.CalculatedTax.TradeLevyNumber.Value;
                        }
                        if (dutyTaxFee.DMExtensions.CalculatedTax.TotalBtlCoverageNIS != null)
                        {
                            supplierInvoiceItemsTaxPM.TotalBtlCoverageNIS = dutyTaxFee.DMExtensions.CalculatedTax.TotalBtlCoverageNIS.Value;
                            _TotalBtlCoverageNISSum = _TotalBtlCoverageNISSum + dutyTaxFee.DMExtensions.CalculatedTax.TotalBtlCoverageNIS.Value;
                        }
                        // moran 21.11.13 - Bug 2083 - change handle -->
                        //AddSupplierInvoiceItemsTaxesModificationPM(supplierInvoiceItemsTaxPM, governmentAgencyGoodsItem.Commodity);
                        //supplierInvoiceItemsTaxPM.SupplierInvoiceItemsTaxesModifications = GetSupplierInvoiceItemsTaxesModifications(governmentAgencyGoodsItem.Commodity.DMExtensions.ValuationDeductionAdjustment, supplierInvoiceItemsTaxPM);
                        // moran 21.11.13 - Bug 2083 - change handle <--
                        supplierInvoiceItemsTaxPMList.Add(supplierInvoiceItemsTaxPM);
                    }
                }

                supplierInvoiceItemPM.SupplierInvoiceItemTaxes = supplierInvoiceItemsTaxPMList;

                totGeneralTaxCalc = 0;
                totPurchaseCalc = 0;
                totVatCalc = 0;

                generalTax = 0;
                purchase = 0;
                vat = 0;

                foreach (var tax in supplierInvoiceItemPM.SupplierInvoiceItemTaxes)
                {
                    if (tax.TaxTypeCode == "1")
                    {
                        generalTax += tax.TaxAmount;
                    }
                    if (tax.TaxTypeCode == "16")
                    {
                        purchase += tax.TaxAmount;
                    }
                    if (tax.TaxTypeCode == "15")
                    {
                        vat += tax.TaxAmount;
                    }
                }

                // moran 6.10.15 - Task 17209 --> 
                supplierInvoiceItemPM.SupplierInvoiceItemVehicles = GetSupplierInvoiceItemsVehicles(governmentAgencyGoodsItem.DMExtensions.Vehicle, supplierInvoiceItemPM);
                // moran 6.10.15 - Task 17209 <--
                if (supplierInvoiceItemPM.SupplierInvoiceItemVehicles != null && supplierInvoiceItemPM.SupplierInvoiceItemVehicles.Count() > 0 && supplierInvoiceItemPM.SupplierInvoiceItemVehicles.LastOrDefault().SupplierInvoiceItemVehicleAdds != null && supplierInvoiceItemPM.SupplierInvoiceItemVehicles.LastOrDefault().SupplierInvoiceItemVehicleAdds.Count() > 0)
                {
                    decimal? diff = 0;
                    if (generalTax != totGeneralTaxCalc)
                    {
                        diff = generalTax - totGeneralTaxCalc;
                        supplierInvoiceItemPM.SupplierInvoiceItemVehicles.LastOrDefault().SupplierInvoiceItemVehicleAdds.LastOrDefault().ChassisTax += diff;
                    }
                    if (purchase != totPurchaseCalc)
                    {
                        diff = purchase - totPurchaseCalc;
                        supplierInvoiceItemPM.SupplierInvoiceItemVehicles.LastOrDefault().SupplierInvoiceItemVehicleAdds.LastOrDefault().ChassisPurchaseTax += diff;
                    }
                    if (vat != totVatCalc)
                    {
                        diff = vat - totVatCalc;
                        supplierInvoiceItemPM.SupplierInvoiceItemVehicles.LastOrDefault().SupplierInvoiceItemVehicleAdds.LastOrDefault().ChassisVat += diff;
                    }
                }
                supplierInvoiceItemPM.ChangeSetOp = ChangeSetOperation.Update;
                supplierInvoiceItemsPMList.Add(supplierInvoiceItemPM);
                var NewSupplierInvioceItemCertificats = supplierInvoiceItemPM.SupplierInvioceItemCertificats.Where(r => r.ChangeSetOp == ChangeSetOperation.Insert).ToList();
                if (supplierInvoicePM.IsAccumalated == true && supplierInvoiceItemPM.IsParent == true && NewSupplierInvioceItemCertificats != null && NewSupplierInvioceItemCertificats.Count > 0)
                {
                    AddNewCertificatesToChildItems(supplierInvoiceItemsPMList, supplierInvoiceItemPM, NewSupplierInvioceItemCertificats);
                }
            }
            
            return supplierInvoiceItemsPMList;
        }

        private void AddNewCertificatesToChildItems(List<SupplierInvoiceItemPM> supplierInvoiceItemsPMList, SupplierInvoiceItemPM supplierInvoiceItemParentPM, List<SupplierInvioceItemCertificatPM> newSupplierInvioceItemCertificats)
        {
            
            List<SupplierInvioceItemCertificatPM> newSupplierInvioceItemCertificatsForChild = newSupplierInvioceItemCertificats;
            var qs = new SupplierInvoiceItemQueryService(this._MyDeclarationPM.Tenant);
            SupplierInvioceItemCertificatQueryService supplierInvioceItemsCertificateQueryService = new SupplierInvioceItemCertificatQueryService(this._MyDeclarationPM.Tenant);
            List<SupplierInvoiceItemPM> supplierInvoiceItemsPMListforcert = qs.GetSupplierInvoiceItemsByParentFullPM(supplierInvoiceItemParentPM.DeclarationId, supplierInvoiceItemParentPM.CounterKey, supplierInvoiceItemParentPM.LineNumber, this._MyDeclarationPM.Tenant);
            foreach (var childSupplierInvoiceItemPM in supplierInvoiceItemsPMListforcert)
            {
                //newSupplierInvioceItemCertificatsForChild.ForEach(i => i.LineNumber = childSupplierInvoiceItemPM.LineNumber);
                childSupplierInvoiceItemPM.SupplierInvioceItemCertificats.AddRange(newSupplierInvioceItemCertificatsForChild);

                childSupplierInvoiceItemPM.ChangeSetOp = ChangeSetOperation.Update;
                supplierInvoiceItemsPMList.Add(childSupplierInvoiceItemPM);
            }
        }

        private List<SupplierInvioceItemCertificatPM> GetSupplierInvioceItemCertificats(SupplierInvoicePM supplierInvoicePM, SupplierInvoiceItemPM supplierInvoiceItemPM)
        {
            //if (supplierInvoiceItemPM.SupplierInvioceItemCertificats != null && supplierInvoiceItemPM.SupplierInvioceItemCertificats.Count() > 0)
            //{
            //    return supplierInvoiceItemPM.SupplierInvioceItemCertificats;
            //}

            //var supplierInvioceItemCertificatPMList = new List<SupplierInvioceItemCertificatPM>();
            var supplierInvioceItemCertificatPMList = supplierInvoiceItemPM.SupplierInvioceItemCertificats;


            List<string> certificateCodeListFromErrosXml = GetCertificateCodeListFromErrosXml("SupplierInvoice", supplierInvoicePM.SequenceNumeric.ToString(), "SupplierInvoiceItem", supplierInvoiceItemPM.SequenceNumeric.ToString());

            if (certificateCodeListFromErrosXml == null)
            {
                return supplierInvioceItemCertificatPMList;
                return null;
            }
            foreach (var certificateCodeFromErrosXml in certificateCodeListFromErrosXml)
            {
                //Check if the code exists current SupplierInvioceItemCertificats
                List<string> entityList = (from a in supplierInvoiceItemPM.SupplierInvioceItemCertificats
                                           where (a.ReqConfirmationTypeCode == certificateCodeFromErrosXml)
                                           select a.ReqConfirmationTypeCode).ToList();

                //If it does NOT exist - Add it to SupplierInvioceItemCertificat
                if (entityList.Count == 0)
                {
                    SupplierInvioceItemCertificatPM supplierInvioceItemCertificatPM = new SupplierInvioceItemCertificatPM();
                    supplierInvioceItemCertificatPM.ChangeSetOp = ChangeSetOperation.Insert;

                    supplierInvioceItemCertificatPM.Tenant = this._MyDeclarationPM.Tenant;
                    supplierInvioceItemCertificatPM.ReqConfirmationTypeCode = certificateCodeFromErrosXml;

                    supplierInvioceItemCertificatPMList.Add(supplierInvioceItemCertificatPM);
                }
            }

            return supplierInvioceItemCertificatPMList;
        }

        private List<string> GetCertificateCodeListFromErrosXml(string myChild1Type, string myChild1Sequence, string myChild2Type, string myChild2Sequence)
        {
            if (_MyDeclarationError == null)
            {
                return null;
            } 
            if (_MyDeclarationError.Entitites.Count == 0)
            {
                return null;
            }

            List<string> certificateCodeListFromErrosXml = new List<string>();

            //Get all 'Entity' for the SupplierInvoiceItem
            List<Entity> entityList = (from a in _MyDeclarationError.Entitites
                                        where (a.Child1Type == myChild1Type && a.Child1Sequence == myChild1Sequence
                                        && a.Child2Type == myChild2Type && a.Child2Sequence == myChild2Sequence)
                                        select a).ToList();

            if (entityList.Count >0) //Bug 23715: שליחת הצהרה- מתקבלת שגיאה שקשורה לאישורים
            //if (entityList.Count != null)
            {
                //Go over all the 'Entity'
                foreach (var entity in entityList)
                {
                    if (entity.FieldErrors != null)
                    {
                        //Go over all the 'FieldErrors'
                        foreach (var fieldErrors in entity.FieldErrors)
                        {
                            //Get all 'FieldErrors' for the 'FieldError'
                            List<field> fieldList = (from a in entity.FieldErrors
                                                     where (a.Code == "2592" && a.Fieldcode == "ClassificationCode")
                                                     select a).ToList();
                            //if (fieldList.Count != null)
                            if (fieldList.Count > 0)// Bug 23715: שליחת הצהרה- מתקבלת שגיאה שקשורה לאישורים
                            {
                                //Go over all the 'FieldErrors'
                                foreach (var field in fieldList)
                                {
                                    var messageError = field.MessageError;
                                    messageError = messageError.Substring(messageError.IndexOf("#") + 1, messageError.LastIndexOf("#") - messageError.IndexOf("#") - 1);

                                    string[] codes = messageError.Split(new string[] { ";" }, StringSplitOptions.None);
                                    for (int i = 0; i < codes.Length; i++)
                                    {
                                        var code = codes[i];
                                        var charList = new List<char>(); // moran 19.10.16 - Bug 23715 - update handle -->
                                        charList.Add(' ');
                                        charList.Add(',');
                                        code = code.Trim(charList.ToArray());
                                        if (!string.IsNullOrWhiteSpace(code))
                                        {
                                            if (code.Length <= 4)//&& code.Length==3 Bug 23715: שליחת הצהרה- מתקבלת שגיאה שקשורה לאישורים
                                            {
                                                var confirmationType = new ConfirmationTypeRepository(_MyDeclarationPM.Tenant);
                                                var myConfirmationType = confirmationType.GetSingle(code);
                                                if (myConfirmationType != null)
                                                {
                                                    certificateCodeListFromErrosXml.Add(code);
                                                }
                                                else
                                                {
                                                    LogMessagingUtil.Instance.AppendLine("Certificate " + code + " does not exist in DB.");
                                                }
                                            }
                                            else
                                            {
                                                LogMessagingUtil.Instance.AppendLine("Certificate " + code + " is too large.");
                                            }
                                        } // moran 19.10.16 - Bug 23715 - update handle <--
                                    }
                                }
                            }
                        }
                    }
                }
                return certificateCodeListFromErrosXml;
            }
            else
            {
                return null;
            }
        }


        private List<SupplierInvoiceItemModVehiclePM> GetSupplierInvoiceItemsModVehicles(DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsVehicleValuationAdjustment[] declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsVehicleValuationAdjustment, SupplierInvoiceItemPM supplierInvoiceItemPM)
        {// moran 24.11.15 - Task 17424
            // moran 6.12.15 - Task 19037 -->
            //var supplierInvoiceItemModVehiclePMList = new List<SupplierInvoiceItemModVehiclePM>(supplierInvoiceItemPM.SupplierInvoiceItemModVehicles);
            var supplierInvoiceItemModVehiclePMList = new List<SupplierInvoiceItemModVehiclePM>();
            // moran 6.12.15 - Task 19037 <--
            if (declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsVehicleValuationAdjustment == null)
            {
                return null;
            }

            foreach (var valuationAdjustmentItem in declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsVehicleValuationAdjustment)
            {

                //var supplierInvoiceItemModVehiclePM = supplierInvoiceItemPM.SupplierInvoiceItemModVehicles.FirstOrDefault(si => si.AdjustmentTypeCode == valuationAdjustmentItem.AdjustmentType.Value);
                //if (supplierInvoiceItemModVehiclePM != null)
                //{
                //If exist update
                //  supplierInvoiceItemModVehiclePM.ChangeSetOp = ChangeSetOperation.Update;
                //}
                //else
                //{
                //Else create new SupplierInvoiceModification record
                // supplierInvoiceItemModVehiclePM = new SupplierInvoiceItemModVehiclePM();
                SupplierInvoiceItemModVehiclePM supplierInvoiceItemModVehiclePM = new SupplierInvoiceItemModVehiclePM();
                supplierInvoiceItemModVehiclePM.ChangeSetOp = ChangeSetOperation.Insert;
                //}
                supplierInvoiceItemModVehiclePM.Tenant = this._MyDeclarationPM.Tenant;
                supplierInvoiceItemModVehiclePM.AdjustmentTypeCode = valuationAdjustmentItem.AdjustmentType.Value;
                supplierInvoiceItemModVehiclePM.DeductAmount = valuationAdjustmentItem.DeductAmount.Value;

                supplierInvoiceItemModVehiclePMList.Add(supplierInvoiceItemModVehiclePM);
            }

            return supplierInvoiceItemModVehiclePMList;
        }


#if refreshWSDL20141230
        // moran 21.11.13 - Bug 2083 - add GetSupplierInvoiceItemsTaxesModifications
        private List<SupplierInvoiceItemsTaxesModificationPM> GetSupplierInvoiceItemsTaxesModifications(DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsValuationDeductionAdjustment[] declarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsValuationDeductionAdjustment, SupplierInvoiceItemsTaxPM supplierInvoiceItemsTaxPM)
        {
            var supplierInvoiceItemsTaxesModificationPMList = new List<SupplierInvoiceItemsTaxesModificationPM>();
            if (declarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsValuationDeductionAdjustment == null)
            {
                return null;
            }

            foreach (var valuationDeductionAdjustment in declarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsValuationDeductionAdjustment)
            {

                var supplierInvoiceItemsTaxesModificationPM = supplierInvoiceItemsTaxPM.SupplierInvoiceItemsTaxesModifications.FirstOrDefault(si => si.TypeCode == valuationDeductionAdjustment.ChargesTypeCode.Value);
                if (supplierInvoiceItemsTaxesModificationPM != null)
                {
                    //If exist update
                    supplierInvoiceItemsTaxesModificationPM.ChangeSetOp = ChangeSetOperation.Update;
                }
                else
                {
                    //Else create new SupplierInvoiceModification record
                    supplierInvoiceItemsTaxesModificationPM = new SupplierInvoiceItemsTaxesModificationPM();
                    supplierInvoiceItemsTaxesModificationPM.ChangeSetOp = ChangeSetOperation.Insert;
                }
                supplierInvoiceItemsTaxesModificationPM.TypeCode = valuationDeductionAdjustment.ChargesTypeCode.Value;
                supplierInvoiceItemsTaxesModificationPM.CurrencyTypeCode = valuationDeductionAdjustment.DeductAmount.currencyID.ToString();
                supplierInvoiceItemsTaxesModificationPM.Amount = valuationDeductionAdjustment.DeductAmount.Value;
                supplierInvoiceItemsTaxesModificationPM.LineNumber = supplierInvoiceItemsTaxPM.LineNumber;
                supplierInvoiceItemsTaxesModificationPM.TaxTypeCode = supplierInvoiceItemsTaxPM.TaxTypeCode;
                supplierInvoiceItemsTaxesModificationPM.Tenant = supplierInvoiceItemsTaxPM.Tenant;
                supplierInvoiceItemsTaxesModificationPM.DeclarationId = supplierInvoiceItemsTaxPM.DeclarationId;
                supplierInvoiceItemsTaxesModificationPMList.Add(supplierInvoiceItemsTaxesModificationPM);
            }

            return supplierInvoiceItemsTaxesModificationPMList;
        }

        
#endif

        // moran 6.10.15 - Task 17209 -->
        //private List<SupplierInvoiceItemVehiclePM> GetSupplierInvoiceItemsVehicles(DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification[] declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification, SupplierInvoiceItemPM supplierInvoiceItemPM)
        //{
        //    //var supplierInvoiceItemVehiclePMList = new List<SupplierInvoiceItemVehiclePM>(supplierInvoiceItemPM.SupplierInvoiceItemVehicles);
        //    var supplierInvoiceItemVehiclePMList = new List<SupplierInvoiceItemVehiclePM>();

        //    if (declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification == null || declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification.Count() < 1)
        //    {
        //        return null;
        //    }

        //    foreach (var vehicle in declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification)
        //    {

        //        var supplierInvoiceItemVehiclePM = supplierInvoiceItemPM.SupplierInvoiceItemVehicles.FirstOrDefault(si => si.VehicleTypeCode == vehicle.IDTypeCode.Value && (si.RichbitFileNumber == vehicle.ID.Value || si.VehicleChassisNumber == vehicle.ID.Value));
        //        if (supplierInvoiceItemVehiclePM == null)
        //        {

        //            throw new System.Exception(
        //               "unable to find the supplierInvoiceItemVehiclePM from vehicle ID " + vehicle.ID.Value + " and vehicle type " + vehicle.IDTypeCode.Value);
        //        }


        //        supplierInvoiceItemVehiclePM.ChangeSetOp = ChangeSetOperation.Update;

        //        supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleMods = GetSupplierInvoiceItemsVehicleMods(vehicle.VehicleValuationAdjustment, supplierInvoiceItemVehiclePM);
        //        // moran 21.3.16 - Task 20132 -->
        //        supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleAdds = GetSupplierInvoiceItemVehicleAdds(vehicle, supplierInvoiceItemVehiclePM, supplierInvoiceItemPM);
        //        // moran 21.3.16 - Task 20132 <--
        //        supplierInvoiceItemVehiclePMList.Add(supplierInvoiceItemVehiclePM); 
        //    }

        //    return supplierInvoiceItemVehiclePMList;
        //} // moran 6.10.15 - Task 17209 <--

        private List<SupplierInvoiceItemVehiclePM> GetSupplierInvoiceItemsVehicles(DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification[] declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification, SupplierInvoiceItemPM supplierInvoiceItemPM)
        {
            //var supplierInvoiceItemVehiclePMList = new List<SupplierInvoiceItemVehiclePM>(supplierInvoiceItemPM.SupplierInvoiceItemVehicles);
            var supplierInvoiceItemVehiclePMList = new List<SupplierInvoiceItemVehiclePM>();

            DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification vehicle = null;
            foreach (var supplierInvoiceItemVehiclePM in supplierInvoiceItemPM.SupplierInvoiceItemVehicles)
            {
                if (declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification == null || declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification.Count() < 1)
                {
                    vehicle = null;
                }
                else
                {
                    vehicle = declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification.FirstOrDefault(v => v.IDTypeCode.Value == supplierInvoiceItemVehiclePM.VehicleTypeCode && (v.ID.Value == supplierInvoiceItemVehiclePM.RichbitFileNumber || v.ID.Value == supplierInvoiceItemVehiclePM.VehicleChassisNumber));
                }
                supplierInvoiceItemVehiclePM.ChangeSetOp = ChangeSetOperation.Update;

                if (vehicle == null)
                {
                    supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleMods = GetSupplierInvoiceItemsVehicleMods(null, supplierInvoiceItemVehiclePM);
                }
                else
                {
                    supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleMods = GetSupplierInvoiceItemsVehicleMods(vehicle.VehicleValuationAdjustment, supplierInvoiceItemVehiclePM);
                }

                supplierInvoiceItemVehiclePMList.Add(supplierInvoiceItemVehiclePM);
            }


            vehicle = null;
            foreach (var supplierInvoiceItemVehiclePM in supplierInvoiceItemVehiclePMList)
            {
                if (declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification == null || declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification.Count() < 1)
                {
                    vehicle = null;
                }
                else
                {
                    vehicle = declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification.FirstOrDefault(v => v.IDTypeCode.Value == supplierInvoiceItemVehiclePM.VehicleTypeCode && (v.ID.Value == supplierInvoiceItemVehiclePM.RichbitFileNumber || v.ID.Value == supplierInvoiceItemVehiclePM.VehicleChassisNumber));
                }
                supplierInvoiceItemVehiclePM.ChangeSetOp = ChangeSetOperation.Update;

                supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleAdds = GetSupplierInvoiceItemVehicleAdds(vehicle, supplierInvoiceItemVehiclePM, supplierInvoiceItemPM, supplierInvoiceItemVehiclePMList);

            }

            return supplierInvoiceItemVehiclePMList;
        }


        private List<SupplierInvoiceItemVehicleModPM> GetSupplierInvoiceItemsVehicleMods(DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentificationVehicleValuationAdjustment[] declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentificationVehicleValuationAdjustment, SupplierInvoiceItemVehiclePM supplierInvoiceItemVehiclePM)
        { // moran 6.10.15 - Task 17209 -->
            //var supplierInvoiceItemVehicleModPMList = new List<SupplierInvoiceItemVehicleModPM>(supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleMods);
            var supplierInvoiceItemVehicleModPMList = new List<SupplierInvoiceItemVehicleModPM>();

            if (declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentificationVehicleValuationAdjustment == null || declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentificationVehicleValuationAdjustment.Count() < 1)
            {
                return null;
            }

            foreach (var vehicleMod in declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentificationVehicleValuationAdjustment)
            {

                //var supplierInvoiceItemVehicleModPM = supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleMods.FirstOrDefault(si => si.AdjustmentTypeCode == vehicleMod.AdjustmentType.Value);

                //if (supplierInvoiceItemVehicleModPM != null)
                //{
                //If exist update
                //    supplierInvoiceItemVehicleModPM.ChangeSetOp = ChangeSetOperation.Update;
                //}
                //else
                //{
                //Else create new  record
                SupplierInvoiceItemVehicleModPM supplierInvoiceItemVehicleModPM = new SupplierInvoiceItemVehicleModPM();
                supplierInvoiceItemVehicleModPM.ChangeSetOp = ChangeSetOperation.Insert;
                //}

                supplierInvoiceItemVehicleModPM.Tenant = this._MyDeclarationPM.Tenant;
                supplierInvoiceItemVehicleModPM.AdjustmentTypeCode = vehicleMod.AdjustmentType.Value;
                supplierInvoiceItemVehicleModPM.DeductAmount = vehicleMod.DeductAmount.Value;

                supplierInvoiceItemVehicleModPMList.Add(supplierInvoiceItemVehicleModPM);
            }

            return supplierInvoiceItemVehicleModPMList;
        } // moran 6.10.15 - Task 17209 <--

        private List<SupplierInvoiceItemVehicleAddPM> GetSupplierInvoiceItemVehicleAdds(DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification vehicle, SupplierInvoiceItemVehiclePM supplierInvoiceItemVehiclePM, SupplierInvoiceItemPM supplierInvoiceItemPM, List<SupplierInvoiceItemVehiclePM> supplierInvoiceItemVehiclePMList = null)
        { // moran 21.3.16 - Task 20132 
            
            var supplierInvoiceItemVehicleAddPM = supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleAdds.FirstOrDefault(si => si.DeclarationId == supplierInvoiceItemVehiclePM.DeclarationId && si.InvoiceItemLineNumber == supplierInvoiceItemVehiclePM.InvoiceItemLineNumber && si.LineNumber == supplierInvoiceItemVehiclePM.LineNumber);
            if (supplierInvoiceItemVehicleAddPM == null)
            {
                return null; // moran 24.4.16 - Task 21126
                throw new System.Exception(
                   "unable to find the supplierInvoiceItemVehicleAddPM from Declaration Id " + supplierInvoiceItemVehiclePM.DeclarationId + " and Supplier Invoice Item Line " + supplierInvoiceItemVehiclePM.InvoiceItemLineNumber + " and Supplier Invoice Item Vehicle Line " + supplierInvoiceItemVehiclePM.LineNumber);
            }


            supplierInvoiceItemVehicleAddPM.ChangeSetOp = ChangeSetOperation.Update;

            var supplierInvoiceItemVehicleAddPMList = new List<SupplierInvoiceItemVehicleAddPM>();

            decimal? allDeduction = 0;
            decimal? chassisDeduction = 0;

            if (supplierInvoiceItemVehiclePMList != null && supplierInvoiceItemVehiclePMList.Count() > 0)
            {
                foreach (var vehicleMod in supplierInvoiceItemVehiclePMList)
                {
                    foreach (var Deduction in vehicleMod.SupplierInvoiceItemVehicleMods)
                    {
                        if ((!string.IsNullOrWhiteSpace(vehicleMod.RichbitFileNumber) && vehicleMod.RichbitFileNumber == supplierInvoiceItemVehiclePM.RichbitFileNumber) || (!string.IsNullOrWhiteSpace(vehicleMod.VehicleChassisNumber) && vehicleMod.VehicleChassisNumber == supplierInvoiceItemVehiclePM.VehicleChassisNumber))
                        {
                            chassisDeduction += Deduction.DeductAmount;
                        }

                        allDeduction += Deduction.DeductAmount;

                    }
                }
            }

            supplierInvoiceItemVehicleAddPM.ChassisPurchaseTax = ((allDeduction + purchase) / supplierInvoiceItemPM.ItemPrice) * supplierInvoiceItemVehicleAddPM.VehicleValue - chassisDeduction;
            supplierInvoiceItemVehicleAddPM.ChassisTax = (generalTax / supplierInvoiceItemPM.ItemPrice) * supplierInvoiceItemVehicleAddPM.VehicleValue;
            supplierInvoiceItemVehicleAddPM.ChassisVat = (vat / supplierInvoiceItemPM.ItemPrice) * supplierInvoiceItemVehicleAddPM.VehicleValue;

            totGeneralTaxCalc += supplierInvoiceItemVehicleAddPM.ChassisTax;
            totPurchaseCalc += supplierInvoiceItemVehicleAddPM.ChassisPurchaseTax;
            totVatCalc += supplierInvoiceItemVehicleAddPM.ChassisVat;

            supplierInvoiceItemVehicleAddPMList.Add(supplierInvoiceItemVehicleAddPM);

            return supplierInvoiceItemVehicleAddPMList;
        }

        private List<SupplierInvoiceItemsModPM> GetSupplierInvoiceItemsModifications(DeclarationGoodsShipmentGovernmentAgencyGoodsItemValuationAdjustment[] declarationGoodsShipmentGovernmentAgencyGoodsItemValuationAdjustment, SupplierInvoiceItemPM supplierInvoiceItemsPM)
        {
            // moran 26.5.15 - 13564 -->
            //var supplierInvoiceItemsModificationPMList = new List<SupplierInvoiceItemsModificationPM>();
            var supplierInvoiceItemsModificationPMList = new List<SupplierInvoiceItemsModPM>(supplierInvoiceItemsPM.SupplierInvoiceItemsMods);
            // moran 26.5.15 - 13564 <--
            if (declarationGoodsShipmentGovernmentAgencyGoodsItemValuationAdjustment == null)
            {
                return null;
            }

            foreach (var valuationAdjustmentItem in declarationGoodsShipmentGovernmentAgencyGoodsItemValuationAdjustment)
            {

                var supplierInvoiceItemsModificationPM = supplierInvoiceItemsPM.SupplierInvoiceItemsMods.FirstOrDefault(si => si.TypeCode == valuationAdjustmentItem.AdditionCode.Value);
                if (supplierInvoiceItemsModificationPM != null)
                {
                    //If exist update
                    supplierInvoiceItemsModificationPM.ChangeSetOp = ChangeSetOperation.Update;
                }
                else
                {
                    //Else create new SupplierInvoiceModification record
                    supplierInvoiceItemsModificationPM = new SupplierInvoiceItemsModPM();
                    supplierInvoiceItemsModificationPM.ChangeSetOp = ChangeSetOperation.Insert;
                }
                supplierInvoiceItemsModificationPM.TypeCode = valuationAdjustmentItem.AdditionCode.Value;
                supplierInvoiceItemsModificationPM.CurrencyTypeCode = valuationAdjustmentItem.AmountAmount.currencyID.ToString();
                supplierInvoiceItemsModificationPM.Amount = valuationAdjustmentItem.AmountAmount.Value;

                supplierInvoiceItemsModificationPMList.Add(supplierInvoiceItemsModificationPM);
            }

            return supplierInvoiceItemsModificationPMList;
        }


        private void AddSupplierInvoiceItemsTaxesModificationPM(
            SupplierInvoiceItemsTaxPM supplierInvoiceItemsTaxPM,
            DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodity declarationGoodsShipmentGovernmentAgencyGoodsItemCommodity)
        {

            // return;
            //var currPM = //supplierInvoiceItemsTax.SupplierInvoiceItemsTaxesModifications[0] ;
            //        new SupplierInvoiceItemsTaxesModPM()
            //        {

            //            DeclarationId = supplierInvoiceItemsTaxPM.DeclarationId,

            //            InvoiceCounterKey = supplierInvoiceItemsTaxPM.InvoiceCounterKey,
            //            ChangeSetOp = ChangeSetOperation.Insert,
            //            Tenant = supplierInvoiceItemsTaxPM.Tenant,

            //            TaxTypeCode = supplierInvoiceItemsTaxPM.TaxTypeCode,
            //            TypeCode = "1",
            //            Amount = 0,
            //            LineNumber = 1,
            //            CurrencyTypeCode = "1"

            //        };
#if refreshWSDL20141230
            var valuationDeductionAdjustment = declarationGoodsShipmentGovernmentAgencyGoodsItemCommodity.DMExtensions.ValuationDeductionAdjustment.FirstOrDefault();
            if (valuationDeductionAdjustment != null)
            {
                currPM.Amount= valuationDeductionAdjustment.DeductAmount.Value ;
                currPM.CurrencyTypeCode = valuationDeductionAdjustment.DeductAmount.currencyID.ToString() ;//??
                currPM.LineNumber = 1;//??

                currPM.TypeCode = valuationDeductionAdjustment.ChargesTypeCode.Value;


            }

            
#endif
            //this._SupplierInvoiceItemsTaxesModificationPMList.Add(currPM);

            //declarationGoodsShipmentGovernmentAgencyGoodsItemCommodity.DMExtensions.DutyRegimeCode 
            //declarationGoodsShipmentGovernmentAgencyGoodsItemCommodity.DMExtensions.ValuationDeductionAdjustment[0].
            //myList.

            ///throw new NotImplementedException();
        }


        private List<DeclarationTaxPM> GetDeclarationTaxesPM(DF_NG_2754_MSG10004_ImportDeclarationResponse customResponse)
        {
            var declarationTaxPMList = new List<DeclarationTaxPM>();

            if (customResponse.Response.Declaration.DutyTaxFee == null)
            {
                return declarationTaxPMList;
            }

            foreach (var dutyTaxFee in customResponse.Response.Declaration.DutyTaxFee)
            {
                var declarationTaxPM = new DeclarationTaxPM();
                declarationTaxPM.ChangeSetOp = ChangeSetOperation.Insert;
                declarationTaxPM.DeclarationId = this._MyDeclarationPM.Id;
                declarationTaxPM.Tenant = this._MyDeclarationPM.Tenant;
                declarationTaxPM.TaxTypeCode = dutyTaxFee.TypeCode.Value;
                declarationTaxPM.TotalAmount = dutyTaxFee.DMExtensions.CalculatedTax.Amount.Value;
                declarationTaxPM.DeferredTaxAmount = dutyTaxFee.DMExtensions.CalculatedTax.DeferedTaxAmount.Value;
                declarationTaxPM.TaxBaseAmount = dutyTaxFee.AdValoremTaxBaseAmount.Value;

                declarationTaxPMList.Add(declarationTaxPM);
            }

            return declarationTaxPMList;
        }

        private void UpdateUnifreightEvent(string eventCode, string loggingUserId)
        {
            switch (eventCode)
            {
                case "MPOA":
                    RaiseUnifreightEvent("MPOA", "MPOA", "");
                    break;
                case "MID":
                    RaiseUnifreightEvent("MID", "MID", "");
                    break;
                case "IDE":
                    RaiseUnifreightEvent("IDE", "IDE", "");
                    break;
            }

        }

        private void RaiseUnifreightEvent(string eventCode, string unifrieghtEvent, string eventRemarks)
        {
            try
            {
                string loggingUserId = AuthenticationUtil.ResolveUserId(_MyDeclarationPM.Tenant);

                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {

                    Tenant = _MyDeclarationPM.Tenant,
                    objectTableName = "Customs.Declaration",
                    EventCode = eventCode,
                    notes = eventRemarks,
                    CommunicationLoggingEntityReference = _MyDeclarationPM.DeclarationNumber,
                    EntityId = _MyDeclarationPM.Id,
                    UserId = loggingUserId,
                    CommunicationSubject = "Event from logitude",
                    MyUnifreightEventParam = new UnifreightEventParam()
                    {
                        Code = unifrieghtEvent,
                        Mode = UnifreightEventMode.@new,
                        EventDateTime = DateTime.Now,
                        Entname = "CFIFILEM",
                        PrimaryNum = _MyDeclarationPM.CustomFileNo,
                        EventRemarks = eventRemarks,
                    }
                };

                LogMessagingUtil.Instance.AppendLine("AmitalEventTracer.CreateTraceEvent: eventCode = " + eventCode + " CustomFileNo= " + _MyDeclarationPM.CustomFileNo + "   ");
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel, true);

            }
            catch (System.Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }
        }

    }
}
