
using Logitude.AmitalMessaging.Utils;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnifreightIIG.Common.RetrieveExportOrTransshipmentDeclarationServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class DF_NG_2757_MSG10004_RetrieveExportDeclarationResponseService
        : ResponseServiceBase<DeclarationRestoreResponseData, DF_NG_2757_MSG10004_ExportDeclarationResponse, DeclarationRestoreRequestParams>
    {
        private Logitude.CustomsMessaging.ResponseServices.DF_NG_2757_MSG10004_ExportDeclarationResponseService _DF_NG_2754_MSG10004_ExportDeclarationResponseService;
        private DeclarationRestoreResponseData _MyDefaultResponseData;
        public UnifreightIIG.Common.CommonIIGInterface.IResponseHeaderOrFault _ResponseHeaderExeption { get; set; }

        public override void Update(DF_NG_2757_MSG10004_ExportDeclarationResponse customResponse, DeclarationRestoreRequestParams requestParams)
        {

            if (requestParams.ShowData)
            {
                GetDeclarationDataRespons(customResponse, requestParams.Tenant);
                return;


            }


            _MyDefaultResponseData = new DeclarationRestoreResponseData()
            {
                ApplicationID = customResponse.ResponseContentHeader.ApplicationID.ToString(),
                Succeeded = true,
                UserMessage = customResponse.ResponseContentHeader.Exception?.FirstOrDefault()?.ExeptionDescription,
                HasException = false,
                ResponseStatusXML = GetDummyXml(customResponse.ResponseContentHeader.Exception?.FirstOrDefault()?.ExeptionDescription + customResponse.ResponseContentHeader?.Remark, requestParams.IsAngularClient),
            };

          
                var declarationqueryService = new DeclarationQueryService(requestParams.Tenant);

            if (string.IsNullOrEmpty(requestParams.DeclarationId)|| (!string.IsNullOrEmpty(requestParams.DeclarationId) && requestParams.IsUpdateDB)) { //declaration not exits in db


                    var decId = declarationqueryService.GetIdByDeclarationNumber(requestParams.DeclarationNumber, requestParams.Tenant);
                    if (customResponse.Response != null && customResponse.Response.Declaration != null && (string.IsNullOrEmpty(decId) || requestParams.IsUpdateDB))
                        CreateDeclarationFromResponse(customResponse.Response.Declaration, requestParams.Tenant, customResponse, requestParams.IsUpdateDB, requestParams.DeclarationId);


              
                }
                if (!string.IsNullOrEmpty(requestParams.DeclarationId))
                {
                    if (customResponse?.Response?.Status[0]?.NameCode?.Value == "36")
                    {
                        var declaration = declarationqueryService.GetSingle(requestParams.DeclarationId, false, false);
                        if (!declaration.IsConvertedDeclaration && (declaration.DeclarationStatusTypeCode != customResponse.Response.Status[0].NameCode.Value))
                        {
                            RaiseEvent(declaration, null, status_id: "CLS");
                        }
                    }
                }

                if (customResponse.ResponseContentHeader != null &&
                    customResponse.Response == null &&
                    customResponse.AddAGlobalScannedAttachmentToEntityResponse == null &&
                    customResponse.CollateralRequestDetails == null)
                {
                    _MyDefaultResponseData = new DeclarationRestoreResponseData()
                    {
                        ApplicationID = customResponse.ResponseContentHeader.ApplicationID.ToString(),
                        Succeeded = true,
                        UserMessage = customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription,
                        HasException = false,
                        ResponseStatusXML = GetDummyXml(customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription + customResponse.ResponseContentHeader.Remark, requestParams.IsAngularClient),
                    };
                    return;
                }
                //[XmlType(AnonymousType = true, Namespace = "http://malam.com/customs/DealFile/Declaration/DF_MSG10000_ImportDeclaration")]
                //[XmlType(AnonymousType = true, Namespace = "http://malam.com/customs/DealFile/Declaration/DF_MSG10000_ImportDeclaration")]
                UnifreightIIG.Common.ExportDeclarationServiceReference.DF_NG_2757_MSG10004_ExportDeclarationResponse ser = null;
                var xml = XmlGenericUtil<DF_NG_2757_MSG10004_ExportDeclarationResponse>.SerializeObject(customResponse);
                ser = XmlGenericUtil<UnifreightIIG.Common.ExportDeclarationServiceReference.DF_NG_2757_MSG10004_ExportDeclarationResponse>.DeSerializeObject(xml);
                _DF_NG_2754_MSG10004_ExportDeclarationResponseService = new DF_NG_2757_MSG10004_ExportDeclarationResponseService();
                //ITZIK+MIRT  _DF_NG_2754_MSG10004_ExportDeclarationResponseService._ResponseHeaderExeption = _ResponseHeaderExeption;
                _DF_NG_2754_MSG10004_ExportDeclarationResponseService._IsRetrieveDeclarationResponse = true;
                _DF_NG_2754_MSG10004_ExportDeclarationResponseService.Update(ser, requestParams);

                //<--- Yuval Chalup 13.05.2015 TASK-11915
                if (customResponse.ResponseContentHeader != null &&
                    customResponse.Response != null &&
                    customResponse.AddAGlobalScannedAttachmentToEntityResponse == null &&
                    customResponse.CollateralRequestDetails == null)
                {
                    _MyDefaultResponseData = new DeclarationRestoreResponseData();
                    if (customResponse.Response != null)
                    {
                        if (requestParams.AppicationId != null && requestParams.DeclarationId == null) requestParams.DeclarationId = requestParams.AppicationId; // moran 10.1.16 Task 19724 + add to condition --><--
                        if (customResponse.Response.Declaration != null && _DF_NG_2754_MSG10004_ExportDeclarationResponseService.MyResponseData.HasException != true)
                        {
                            if (this.MyRequestSheetParam == null)
                                this.MyRequestSheetParam = new RequestSheetParam();



                             this.MyRequestSheetParam.CustomFileNo = !string.IsNullOrEmpty(requestParams?.CustomsFile) ? requestParams.CustomsFile :
                         Strings.Right(GetValueIDType(customResponse.Response.Declaration.DMExtensions?.ExternalDeclarationID), 10)?.TrimStart('0');




                        xml = XmlGenericUtil<Declaration>.SerializeObject(customResponse.Response.Declaration);
                            _MyDefaultResponseData.ResponseStatusXML = xml;
                            if (requestParams.IsAngularClient)
                            {
                                var doc = new XmlDocument();
                                doc.LoadXml(xml);

                                _MyDefaultResponseData.ResponseStatusXML = //JsonConvert.SerializeObject(customResponse.Response.Declaration);
                                    JsonConvert.SerializeXmlNode(doc);
                            }

                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(_DF_NG_2754_MSG10004_ExportDeclarationResponseService.MyResponseData.UserMessage))
                            {
                                _DF_NG_2754_MSG10004_ExportDeclarationResponseService.MyResponseData.HasException = false; // moran 10.1.16 Task 19724
                                _MyDefaultResponseData.ResponseStatusXML = GetDummyXml(_DF_NG_2754_MSG10004_ExportDeclarationResponseService.MyResponseData.UserMessage, requestParams.IsAngularClient);
                            }
                        }
                    }
                }
                //Yuval Chalup 13.05.2015 TASK-11915 --->

        }


           
        public override DeclarationRestoreResponseData GetResponse(DF_NG_2757_MSG10004_ExportDeclarationResponse customResponse, DeclarationRestoreRequestParams requestParams)
        {
            if (_DF_NG_2754_MSG10004_ExportDeclarationResponseService == null || _DF_NG_2754_MSG10004_ExportDeclarationResponseService.MyResponseData == null)
            {
                return _MyDefaultResponseData;
            }
            var response = new DeclarationRestoreResponseData()
            {
                ApplicationID = _DF_NG_2754_MSG10004_ExportDeclarationResponseService.MyResponseData.ApplicationID.ToString(),
                UserMessage = _DF_NG_2754_MSG10004_ExportDeclarationResponseService.MyResponseData.UserMessage,
                HasException = _DF_NG_2754_MSG10004_ExportDeclarationResponseService.MyResponseData.HasException,
                Succeeded = _DF_NG_2754_MSG10004_ExportDeclarationResponseService.MyResponseData.Succeeded,
            };

            if (!string.IsNullOrWhiteSpace(_DF_NG_2754_MSG10004_ExportDeclarationResponseService.MyResponseData.UserMessage) && _DF_NG_2754_MSG10004_ExportDeclarationResponseService.MyResponseData.UserMessage.Contains("נתוני ההצהרה לא עודכנו"))
            {
                response.IsShowUserMessage = true;
            }

            //<--- Yuval Chalup 13.05.2015 TASK-11915
            if (customResponse.ResponseContentHeader != null &&
                customResponse.Response != null &&
                customResponse.AddAGlobalScannedAttachmentToEntityResponse == null &&
                customResponse.CollateralRequestDetails == null)
            {
                if (this._MyDefaultResponseData != null)
                {
                    response.ResponseStatusXML = this._MyDefaultResponseData.ResponseStatusXML;
                    return response;
                }

                if (customResponse.Response != null)
                {
                    if (customResponse.Response.Declaration != null)
                    {
                        if (requestParams.IsAngularClient)
                        {
                            _MyDefaultResponseData.ResponseStatusXML = JsonConvert.SerializeObject(customResponse.Response.Declaration);
                        }
                        else
                        {
                            string xml = XmlGenericUtil<Declaration>.SerializeObject(customResponse.Response.Declaration);
                            _MyDefaultResponseData.ResponseStatusXML = xml;
                        }

                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(_DF_NG_2754_MSG10004_ExportDeclarationResponseService.MyResponseData.UserMessage))
                        {
                            _MyDefaultResponseData.ResponseStatusXML = GetDummyXml(_DF_NG_2754_MSG10004_ExportDeclarationResponseService.MyResponseData.UserMessage, requestParams.IsAngularClient);
                        }
                    }
                }
            }
            //Yuval Chalup 13.05.2015 TASK-11915 --->

            return response;
        }


        private string GetDummyXml(string message, bool toJson)
        {
            var myDummyXml = new GeneralMessage() { Message = message };
            if (toJson)
            {
                return JsonConvert.SerializeObject(myDummyXml);
            }
            var xml = XmlGenericUtil<GeneralMessage>.SerializeObject(myDummyXml);
            return xml;
        }

        public void GetDeclarationDataRespons(DF_NG_2757_MSG10004_ExportDeclarationResponse customResponse, int Tenant)
        {

            this.MyResponseData = new DeclarationRestoreResponseData();
            this.MyResponseData.exportDeclarationDataResponseData = new ExportDeclarationDataResponseData();
            this.MyResponseData.Succeeded = true;
            if (customResponse.ResponseContentHeader.Exception != null)
            {
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription;
                return;
            }


            if (customResponse.Response.Declaration != null)
            {
                if (this.MyRequestSheetParam == null)
                    this.MyRequestSheetParam = new RequestSheetParam();

                this.MyRequestSheetParam.CustomFileNo = Strings.Right(GetValueIDType(customResponse.Response.Declaration.DMExtensions?.ExternalDeclarationID), 10)?.TrimStart('0');
                this.MyResponseData.exportDeclarationDataResponseData.Title = customResponse.Response.Declaration.ID.Value;

                this.MyResponseData.exportDeclarationDataResponseData.CalculationDate = Convert.ToDateTime(customResponse.Response.Declaration.IssueDateTime).ToString("dd/MM/yyyy");
                if (customResponse.Response.Declaration.Agent != null && customResponse.Response.Declaration.Agent.Count() > 0)
                {
                    var agent = customResponse.Response.Declaration.Agent.FirstOrDefault();
                    if (agent != null)
                    {
                        this.MyResponseData.exportDeclarationDataResponseData.AgentCustomerExternalID = GetValueIDType(agent.ID);
                    }
                }


                if (customResponse.Response.Declaration.GoodsShipment != null)
                {
                    this.MyResponseData.exportDeclarationDataResponseData.InvoiceList = new List<Invoice>();
                    foreach (var invoiceItem in customResponse.Response.Declaration.GoodsShipment.OrderBy(x => x.SequenceNumeric))
                    {
                        Invoice invoiceResult = new Invoice();
                        invoiceResult.InvoiceId = GetValueIDType(invoiceItem.Invoice.ID) + invoiceItem.SequenceNumeric.ToString();
                        invoiceResult.SequenceNumber = invoiceItem.SequenceNumeric.ToString();
                        invoiceResult.ExternalID = GetValueIDType(invoiceItem.Invoice.ID);
                        invoiceResult.InvoiceAmount = GetValueAmountType(invoiceItem.Invoice.DMExtensions.InvoiceAmount).ToString();
                        invoiceResult.InvoiceAmountCurrency = GetValueAmountType(invoiceItem.Invoice.DMExtensions.InvoiceAmount).ToString(); ;

                        invoiceResult.InvoiceCurrency = invoiceItem.Invoice.DMExtensions.InvoiceAmount.currencyID.ToString();
                        invoiceResult.InvoiceAmountCurrency += " (" + invoiceResult.InvoiceAmountCurrency + ")";

                        this.MyResponseData.exportDeclarationDataResponseData.InvoiceList.Add(invoiceResult);

                        //GoodsItem
                        if (invoiceItem.GovernmentAgencyGoodsItem != null)
                        {
                            if (this.MyResponseData.exportDeclarationDataResponseData.RequestList == null)
                            {
                                this.MyResponseData.exportDeclarationDataResponseData.RequestList = new List<Request>();
                            }
                            foreach (var governmentAgencyGoodsItem in invoiceItem.GovernmentAgencyGoodsItem)
                            {
                                Request requestResult = new Request();
                                requestResult.InvoiceId = GetValueIDType(invoiceItem.Invoice.ID) + invoiceItem.SequenceNumeric.ToString();
                                requestResult.SequenceNumber = governmentAgencyGoodsItem.SequenceNumeric.ToString();
                                var classification2 = governmentAgencyGoodsItem.Commodity.Classification.FirstOrDefault(x => x != null && GetValueCodeType(x.IdentificationTypeCode) == "HS");

                                if (classification2 != null)
                                {
                                    requestResult.CustomsItem = GetValueIDType(classification2.ID);//.Replace("/", "");
                                }


                                foreach (var goodsMeasure in governmentAgencyGoodsItem.GoodsMeasure)
                                {
                                    if (goodsMeasure.DMExtensions != null && goodsMeasure.TariffQuantity != null)
                                    {
                                        switch (goodsMeasure.DMExtensions.MeasureQualifier.Value)
                                        {

                                            case "2":
                                                {
                                                    requestResult.ValueQuantity = goodsMeasure.TariffQuantity.Value.ToString();
                                                    break;
                                                }

                                        }
                                    }
                                }



                                if (governmentAgencyGoodsItem.Origin?.CountryCode != null)
                                {
                                    requestResult.OriginCountry = GetValueCodeType(governmentAgencyGoodsItem.Origin.CountryCode).ToString();
                                }

                                if (governmentAgencyGoodsItem != null && governmentAgencyGoodsItem.DMExtensions != null && governmentAgencyGoodsItem.DMExtensions.GoodsItemAmount != null)
                                {


                                    var cur = customResponse.Response.Declaration.GoodsShipment[0].Invoice.DMExtensions.InvoiceAmount.currencyID.ToString();
                                    var GoodsItemAmount = governmentAgencyGoodsItem.DMExtensions.GoodsItemAmount.Where(x => x.AmountType.Value == "1").FirstOrDefault();
                                    if (GoodsItemAmount != null)
                                    {
                                        requestResult.ForeignAmount = GetValueAmountType(GoodsItemAmount.CustomsValueAmount).ToString();
                                        requestResult.ForeignCurrency = GoodsItemAmount.CustomsValueAmount.currencyID.ToString();
                                        requestResult.ForeignCurrencyAmount = GetValueAmountType(GoodsItemAmount.CustomsValueAmount).ToString(); ;
                                        int i = 0;
                                        int.TryParse(GetValueCodeType(GoodsItemAmount.AmountType), out i);
                                        if (/*requestItem.CurrencyTypeID*/ i > 0)
                                        {
                                            requestResult.ForeignCurrencyAmount += " (" + GoodsItemAmount.CustomsValueAmount.currencyID.ToString() + ")";
                                        }
                                    }
                                }
                                //GovernmentProcedure
                                requestResult.GovernmentProcedureList = new List<GovernmentProcedure>();
                                if (governmentAgencyGoodsItem.GovernmentProcedure != null)
                                {
                                    foreach (var governmentProcedureItem in governmentAgencyGoodsItem.GovernmentProcedure)
                                    {
                                        if (governmentProcedureItem.CurrentCode != null)
                                        {
                                            GovernmentProcedure governmentProcedureResult = new GovernmentProcedure();
                                            governmentProcedureResult.ItemGovernmentProcedureType = governmentProcedureItem.CurrentCode.Value.ToString();
                                            if (!string.IsNullOrEmpty(governmentProcedureResult.ItemGovernmentProcedureType)) // Table 1422
                                            {
                                                ItemGovernmentProcedureTypeQueryService itemGovernmentProcedureTypeQueryService = new ItemGovernmentProcedureTypeQueryService(Tenant);
                                                ItemGovernmentProcedureTypePM itemGovernmentProcedureTypePM = itemGovernmentProcedureTypeQueryService.GetSingle(governmentProcedureResult.ItemGovernmentProcedureType, false, true);
                                                if (itemGovernmentProcedureTypePM != null)
                                                {
                                                    governmentProcedureResult.ItemGovernmentProcedureName = itemGovernmentProcedureTypePM.LocalName;
                                                }
                                            }
                                            requestResult.GovernmentProcedureList.Add(governmentProcedureResult);
                                        }
                                    }
                                }

                                //Vehicle
                                requestResult.VehicleList = new List<Vehicle>();
                                if (governmentAgencyGoodsItem.DMExtensions != null && governmentAgencyGoodsItem.DMExtensions.Vehicle != null)

                                {
                                    foreach (var vehicleItem in governmentAgencyGoodsItem.DMExtensions.Vehicle)
                                    {
                                        Vehicle vehicleResult = new Vehicle();

                                        vehicleResult.CargoIdentityQualifierID = vehicleItem.IDTypeCode.Value.ToString();

                                        vehicleResult.RichbitNumber = vehicleItem.ID.Value;
                                        requestResult.VehicleList.Add(vehicleResult);
                                    }
                                }

                                this.MyResponseData.exportDeclarationDataResponseData.RequestList.Add(requestResult);
                            }
                        }
                    }
                }


            }

            this.MyResponseData.HasException = false;
            this.MyResponseData.UserMessage = "ניתוח בוצע בהצלחה";

            return;
        }
        public void CreateDeclarationFromResponse(Declaration declaration, int tenant, DF_NG_2757_MSG10004_ExportDeclarationResponse customResponse, bool IsUpdateDB, string DeclarationId)
        {


            var context = CustomContext.GetContext(tenant);
            DeclarationRepository declarationRepository = new DeclarationRepository(context);
            var myQueryService = new DeclarationQueryService(context);
            DeclarationPM declarationOrg;
            DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), tenant);


            List<SupplierInvoicePM> invoicePMs = new List<SupplierInvoicePM>(); ;
            DeclarationPM declarationPM;
            if (!IsUpdateDB)
            {
                declarationPM = new DeclarationPM()
                {
                    ChangeSetOp = ChangeSetOperation.Insert,
                    DeclarationOfficeCode = GetValueIDType(declaration.DeclarationOfficeID),
                    Tenant = tenant,
                    Direction = "E",
                    IsConnectedToUnifreight = false,
                    AmendmentDontDisplayInList = false,
                    IsSubmitDeclaration = true,
                    ExportDeclarationOfficeCode = GetValueIDType(declaration.ExportDeclarationOfficeID),
                    DeclarationTypeCode = GetValueCodeType(declaration.TypeCode),
                    Consignments = GetConsignments(declaration, tenant, null, context, IsUpdateDB)
                };


                declarationPM.DeclarationNumber = customResponse.Response.Declaration.ID.Value;
                declarationPM.IsExportClosed = customResponse.Response.Status[0].NameCode.Value == "36" ? true : false;
                declarationPM.IsClose = customResponse.Response.Status[0].NameCode.Value == "36" ? true : false;

                declarationPM.AgentRoleCode = "A";
                declarationPM.TotalTax = Math.Round(declaration.DMExtensions.CustomsValueComponent.TaxAssessedAmount.Value, 2);
                // declarationPM.TaxationDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
                declarationPM.IsConvertedDeclaration = true;

                    declarationPM.TaxationDateTime = Convert.ToDateTime(declaration.DMExtensions?.ReferenceDateTime);



               
                decimal DealValueWithoutFactor = 0;
                if (declaration.GoodsShipment != null)
                {
                    if (!IsUpdateDB)
                        declarationPM.DeclarationExportRecipients = GetRecipients(declaration, tenant, declarationPM, context);

                    foreach (var goodsShipment in declaration.GoodsShipment)
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
                if (DealValueWithoutFactor != 0) declarationPM.DealValueWithoutFactor = Math.Round(DealValueWithoutFactor, 2);

                declarationPM.DepartmentId = null;
                switch (GetValueIDType(declaration.DeclarationOfficeID))
                {
                    case "4":
                        declarationPM.TransportModeId = "A";
                        break;
                    case "1":
                    case "2":
                        declarationPM.TransportModeId = "O";
                        break;
                    default:
                        declarationPM.TransportModeId = "L";
                        break;
                }
                declarationPM.ReferentUserId = null;

                declarationPM.PrimaryInvoiceCounterKey = "1";
                declarationPM.ExcludeConsignment = declaration.GoodsShipment[0].ExportConsignment != null ? false : true;
                declarationPM.IsDiamondDeclaration = false;
                declarationPM.ExportDeclarationOfficeCode = GetValueIDType(declaration.ExportDeclarationOfficeID);
                declarationPM.DeclarationTypeCode = GetValueCodeType(declaration.TypeCode);

            }
            else
            {
                declarationPM = myQueryService.GetSingle(DeclarationId, true, false);


                invoicePMs = GetSupplierInvoices(declaration, tenant, context, DeclarationId);
                DeleteSomeObjects(declarationPM, tenant, context);
                declarationPM.DeclarationOfficeCode = GetValueIDType(declaration.DeclarationOfficeID);
                declarationPM.ExportDeclarationOfficeCode = GetValueIDType(declaration.ExportDeclarationOfficeID);
                declarationPM.DeclarationTypeCode = GetValueCodeType(declaration.TypeCode);
                declarationPM.Tenant = tenant;
                declarationPM.Consignments = GetConsignments(declaration, tenant, declarationPM, context, IsUpdateDB);

                declarationPM.ChangeSetOp = ChangeSetOperation.Update;
            }
            if (customResponse.Response.Status != null && customResponse.Response.Status.Count() > 0)
                declarationPM.DeclarationStatusTypeCode = GetValueCodeType(customResponse.Response.Status[0].NameCode);
            GetAgent(declaration, ref declarationPM);

            if (declaration.GovernmentProcedure != null)
            {
                declarationPM.ProcedureCurrentCode = declaration.GovernmentProcedure.CurrentCode.Value;
            }
            if (declaration.DMExtensions != null)
            {
                if (IsUpdateDB)
                    declarationPM.DeclarationExportRecipients = GetRecipientsFromRestore(declaration, tenant, declarationPM, context);

                declarationPM.CustomFileNo = Strings.Right(GetValueIDType(declaration.DMExtensions.ExternalDeclarationID), 10)?.TrimStart('0');


                if (!IsUpdateDB)
                    declarationPM.ExportFile = GetValueIDType(declaration.DMExtensions.AgentFileReferenceID);
                declarationPM.ExternalDeclarationNumber = GetValueIDType(declaration.DMExtensions.ExternalDeclarationID);
                declarationPM.DestinationCountryCode = GetValueCodeType(declaration.DMExtensions.DestinationCountry);
                declarationPM.ExportAutonomyRegionTypeCode = GetValueIDType(declaration.DMExtensions.AutonomyRegionType);
                if (declaration.DMExtensions.TransferDeclarationToDestinationCountry != null)
                    declarationPM.IsExporterConfirmation = declaration.DMExtensions.TransferDeclarationToDestinationCountry.Value;

                if (IsUpdateDB && declaration.DMExtensions.ReferenceDateTime != null)
                    declarationPM.TaxationDateTime = Convert.ToDateTime(declaration.DMExtensions.ReferenceDateTime);
                if (declaration.DMExtensions.ReleaseDateTime != null)
                    declarationPM.HatraDate = Convert.ToDateTime(declaration.DMExtensions.ReleaseDateTime.Value);
                declarationPM.VersionId = GetValueIDType(declaration.DMExtensions.VersionID);

                if (declaration.PreviousDocument != null)
                {
                    declarationPM.DeclarationDocumentId = GetValueIDType(declaration.PreviousDocument.ID);
                    declarationPM.DeclarationDocumentTypeCode = GetValueCodeType(declaration.PreviousDocument.TypeCode);
                }
                if (declaration.DMExtensions.ExpenseLoadingFactorDetails != null)
                    declarationPM.LoadingFactor = declaration.DMExtensions.ExpenseLoadingFactorDetails.FirstOrDefault()?.ExpenseLoadingFactor.Value;

            }

            if (declaration.Exporter != null)
            {
                SetImporters(ref declarationPM, declaration, tenant, context);

                if (!IsUpdateDB)
                {
                    var queryService = new CardQueryService(tenant);
                    ICommonDataContext _CommonContext = CommonDataContext.GetContext(tenant);
                    var cardRepository = new CardRepository(_CommonContext);
                    var card = cardRepository.GetSingleCardByVatNumber(declaration.Exporter[0]?.ID?.Value, tenant);
                    if (card != null)
                    {

                        declarationPM.CustomerId = card.Id;
                    }
                }
            }

            context = CustomContext.GetContext(tenant);
            declarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), tenant);

            declarationUpdateService.Update(declarationPM, true);

            var exportDeclarationClosingData = GetClosingDetails(declaration, tenant, declarationPM, context);
            if (exportDeclarationClosingData != null)
            {
                exportDeclarationClosingData.DeclarationId = declarationPM.Id;
                exportDeclarationClosingData.Tenant = tenant;
                ExportDeclarationClosingDataUpdateService exportDeclarationClosingDataUpdateService = new ExportDeclarationClosingDataUpdateService(context, new Dictionary<string, IContext>(), tenant);
                exportDeclarationClosingDataUpdateService.Update(exportDeclarationClosingData, true);
            }
            string declarationId;
            declarationId = declarationPM.Id;
            declarationPM.DeclarationTaxes = GetDeclarationTaxesPM(declaration, declarationId, tenant);


            if (IsUpdateDB)
            {
                declarationPM.SupplierInvoices = invoicePMs;
            }
            else
            {
                declarationPM.SupplierInvoices = GetSupplierInvoices(declaration, tenant, context, declarationId);
            }
            declarationPM.ChangeSetOp = ChangeSetOperation.Update;
            declarationPM.Consignments.ForEach(x => x.ChangeSetOp = ChangeSetOperation.None);
            declarationPM.DeclarationExportRecipients.ForEach(x => x.ChangeSetOp = ChangeSetOperation.None);
            declarationUpdateService.Update(declarationPM, true);


            if (!IsUpdateDB)
            {
                CreateEvent(tenant, declarationId);
                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {

                    Tenant = declarationPM.Tenant,
                    objectTableName = "Customs.Declaration",
                    EventCode = null,
                    notes = "",
                    CommunicationLoggingEntityReference = declarationPM.DeclarationNumber,
                    EntityId = declarationPM.Id,
                    UserId = declarationPM.CreatedByUserId,

                    CommunicationSubject = "עדכון תיק מכס",

                };
                Logitude.AmitalMessaging.Infrastructure.FuStatus.LOGICUSTFILE logistictFile = AmitalInsertToQueueEzer.setLogistictFile(declarationPM);
                var amitalInsertToQueueService = new AmitalInsertToQueueService<Logitude.AmitalMessaging.Infrastructure.FuStatus.LOGICUSTFILE>(logistictFile);
                amitalInsertToQueueService.InsertToQueue(myAmitalEventTracerModel, "UpdateExportCustomsFile");
            }



        }
        private void CreateEvent(int tenant, string declarationId)
        {
            string loggingUserId = "";
            UserRepository userRepository = new UserRepository(tenant);
            var user1 = userRepository.GetSingleUserByCode("MEHES", tenant, true);
            if (user1 != null)
            {
                loggingUserId = user1.Id;
            }
            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                Tenant = tenant,
                EventTypeCode = "DCR",
                UserId = loggingUserId,
                EntityId = declarationId,
                ObjectTableName = "Customs.Declaration",
                Notes = "ההצהרה הוקמה כתוצאה משחזור",
            });
        }


        private void SetImporters(ref DeclarationPM declarationPM, Declaration declaration, int tenant, ICustomContext context)
        {
            foreach (var importer in declaration.Exporter)
            {
                if (importer.ID != null)
                {
                    switch (GetValueCodeType(importer.DMExtensions.RoleCode))
                    {
                        case "7":
                            {
                                declarationPM.ImporterTypeCode = importer.ID.schemeID;
                                declarationPM.ImporterAddress = importer.DMExtensions.Address;
                                declarationPM.ImporterName = importer.DMExtensions.Name;
                                //declarationPM.MainImporterEntitlemntTypeCode = GetValueCodeType(importer.DMExtensions.RoleCode.EntitlementTypeCode);
                                if (importer.ID.schemeID == "2" || importer.ID.schemeID == "3") declarationPM.ImporterPassCountryCode = GetValueTextType(importer.DMExtensions.IssueLocation);

                                switch (importer.ID.schemeID)
                                {
                                    case "1":
                                        {
                                            var queryService = new ClientQueryService(context);
                                            var importerPM = queryService.GetClientByCode(importer.ID.Value, tenant);
                                            if (importerPM == null)
                                                declarationPM.ImporterCode = importer.ID.Value;
                                            else
                                            {
                                                declarationPM.ImporterCode = importerPM.Code;
                                                declarationPM.ImporterId = importerPM.Id;
                                            }
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
                        case "12":
                            {
                                switch (importer.ID.schemeID)
                                {
                                    case "1":
                                        {
                                            var queryService = new ClientQueryService(context);
                                            var importerPM = queryService.GetClientByCode(importer.ID.Value, tenant);
                                            if (importerPM == null)
                                                declarationPM.TransferImporterCode = importer.ID.Value;
                                            else
                                            {
                                                declarationPM.TransferImporterCode = importerPM.Code;
                                                declarationPM.TransferImporterId = importerPM.Id;
                                            }
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
                                //declarationPM.TransImporterEntitleTypeCode = GetValueCodeType(importer.DMExtensions.EntitlementTypeCode);
                                if (importer.ID.schemeID == "2" || importer.ID.schemeID == "3") declarationPM.TransferImporterCountryCode = GetValueTextType(importer.DMExtensions.IssueLocation);
                                break;
                            }
                        case "6":
                            {
                                switch (importer.ID.schemeID)
                                {
                                    case "1":
                                        {
                                            var queryService = new ClientQueryService(context);
                                            var importerPM = queryService.GetClientByCode(importer.ID.Value, tenant);
                                            if (importerPM == null)
                                                declarationPM.EntitleImporterCode = importer.ID.Value;
                                            else
                                            {
                                                declarationPM.EntitleImporterCode = importerPM.Code;
                                                declarationPM.EntitleImporterId = importerPM.Id;
                                            }
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
                                //declarationPM.ImporterEntitlementTypeCode = GetValueCodeType(importer.DMExtensions.EntitlementTypeCode);
                                if (importer.ID.schemeID == "2" || importer.ID.schemeID == "3") declarationPM.EntitleImporterCountryCode = GetValueTextType(importer.DMExtensions.IssueLocation);
                                break;
                            }
                    }
                }
                else
                {
                    if (importer.DMExtensions != null)
                    {
                        switch (importer.DMExtensions.RoleCode.Value)
                        {
                            case "7":
                                {
                                    declarationPM.ImporterAddress = importer.DMExtensions.Address;
                                    declarationPM.ImporterName = importer.DMExtensions.Name;
                                    //   declarationPM.impo = GetValueTextType(importer.DMExtensions.IssueLocation);
                                    break;
                                }
                            case "12":
                                {
                                    declarationPM.TransferImporterAddress = importer.DMExtensions.Address;
                                    declarationPM.TransferImporterName = importer.DMExtensions.Name;
                                    declarationPM.TransferImporterCountryCode = GetValueTextType(importer.DMExtensions.IssueLocation);
                                    break;
                                }
                            case "6":
                                {  //  declarationPM.EntitleImporterTypeCode = "4";
                                    declarationPM.EntitleImporterAddress = importer.DMExtensions.Address;
                                    declarationPM.EntitleImporterName = importer.DMExtensions.Name;
                                    declarationPM.EntitleImporterCountryCode = GetValueTextType(importer.DMExtensions.IssueLocation);
                                    break;
                                }
                        }
                    }
                }
            }

        }

        private ExportDeclarationClosingDataPM GetClosingDetails(Declaration declaration, int tenant, DeclarationPM declarationPM, ICustomContext context)
        {
            if (declaration.DMExtensions.DeclarationClosingDetails != null)
            {
                var closingDetails = new ExportDeclarationClosingDataPM();
                closingDetails.FinalShipCode = GetValueIDType(declaration.DMExtensions.DeclarationClosingDetails.FinalShipID);
                closingDetails.FinalLoadingSite = GetValueIDType(declaration.DMExtensions.DeclarationClosingDetails.FinalLoadingSite);
                if (declaration.DMExtensions.DeclarationClosingDetails.DepartureDateTime != null)
                {
                    closingDetails.LoadingDateTime = declaration.DMExtensions.DeclarationClosingDetails.DepartureDateTime.Value;
                }
				if (declaration.DMExtensions.DeclarationClosingDetails.FinalTransportContractDocument!=null)
                { 
                   closingDetails.FinalManifestNumber = GetValueIDType(declaration.DMExtensions.DeclarationClosingDetails.FinalTransportContractDocument.FirstCargoID);
                   closingDetails.FinalCargoTypeCode = GetValueCodeType(declaration.DMExtensions.DeclarationClosingDetails.FinalTransportContractDocument.TypeCode);
                   closingDetails.FinalSecondCargoId = GetValueIDType(declaration.DMExtensions.DeclarationClosingDetails.FinalTransportContractDocument.SecondCargoID);
                   closingDetails.FinalThirdCargoId = GetValueIDType(declaration.DMExtensions.DeclarationClosingDetails.FinalTransportContractDocument.ThirdCargoID);
                }
                closingDetails.ChangeSetOp = ChangeSetOperation.Insert;
                return closingDetails;
            }
            return null;
        }
        private List<DeclarationExportRecipientPM> GetRecipientsFromRestore(Declaration declaration, int tenant, DeclarationPM declarationPM, ICustomContext context)
        {
            List<DeclarationExportRecipientPM> recipientPMs = new List<DeclarationExportRecipientPM>();
            if (declaration.DMExtensions.RecipientDetails != null && declaration.DMExtensions.RecipientDetails.Count() > 0)
            {
                foreach (var declarationExportRecipient in declaration.DMExtensions.RecipientDetails)
                {
                    DeclarationExportRecipientPM recipientPM = new DeclarationExportRecipientPM();
                    recipientPM.Tenant = tenant;
                    recipientPM.RecipientName = declarationExportRecipient.Name;
                    recipientPM.RecipientAddress = declarationExportRecipient.Address;
                    recipientPM.RecipientIssueCountryCode = GetValueCodeType(declarationExportRecipient.IssueLocation);
                    recipientPM.ChangeSetOp = ChangeSetOperation.Insert;
                    recipientPMs.Add(recipientPM);
                }
            }
            return recipientPMs;
        }
        private List<DeclarationExportRecipientPM> GetRecipients(Declaration declaration, int tenant, DeclarationPM declarationPM, ICustomContext context)
        {
            List<DeclarationExportRecipientPM> recipientPMs = new List<DeclarationExportRecipientPM>();
            if (declaration.DMExtensions.RecipientDetails != null && declaration.DMExtensions.RecipientDetails.Count() > 0)
            {
                foreach (var declarationExportRecipient in declaration.DMExtensions.RecipientDetails)
                {
                    DeclarationExportRecipientPM recipientPM = new DeclarationExportRecipientPM();
                    recipientPM.Tenant = tenant;
                    recipientPM.RecipientName = declarationExportRecipient.Name;
                    recipientPM.RecipientAddress = declarationExportRecipient.Address;
                    recipientPM.RecipientIssueCountryCode = GetValueCodeType(declarationExportRecipient.IssueLocation);
                    recipientPM.ChangeSetOp = ChangeSetOperation.Insert;
                    recipientPMs.Add(recipientPM);
                }
            }
            return recipientPMs;
        }

        private List<ConsignmentPM> GetConsignments(Declaration declaration, int tenant, DeclarationPM declarationPM, ICustomContext context, bool IsUpdateDB)
        {
            List<ConsignmentPM> consignmentPMs = new List<ConsignmentPM>();

            if (declaration.GoodsShipment == null || declaration.GoodsShipment.Count() == 0)
            {
                ConsignmentPM consignmentPM = new ConsignmentPM();
                consignmentPM.ChangeSetOp = ChangeSetOperation.Insert;
                consignmentPMs.Add(consignmentPM);
                return consignmentPMs;
            }
            if ((declaration.GoodsShipment[0].ExportConsignment == null && declaration.GoodsShipment[0].ImportConsignment == null) ||
                (declaration.GoodsShipment[0].ExportConsignment.Count() == 0 && declaration.GoodsShipment[0].ImportConsignment.Count() == 0)) 
            {
                ConsignmentPM consignmentPM = new ConsignmentPM();
                consignmentPM.ChangeSetOp = ChangeSetOperation.Insert;
                consignmentPMs.Add(consignmentPM);
                return consignmentPMs;
            }

            foreach (var consignment in declaration.GoodsShipment[0].ExportConsignment)
            {
                ConsignmentPM consignmentPM = new ConsignmentPM();
                consignmentPM.ChangeSetOp = ChangeSetOperation.Insert;
                consignmentPM.Tenant = tenant;
                consignmentPM.ConsignmentType = "E";
                var consignmentQueryService = new ConsignmentQueryService(context);

                if (declarationPM != null)
                {
                    var maxCounter = consignmentQueryService.GetMaxCounterKey(declarationPM.Id, tenant) ?? 0;
                    consignmentPM.SequenceNumeric = maxCounter + 1;
                }
                if (consignment.TransportContractDocument != null)
                {
                    consignmentPM.CargoTypeCode = GetValueCodeType(consignment.TransportContractDocument.TypeCode);
                    //if (consignment.TransportContractDocument.IssueDateTime != null) consignmentPM.ManifestDate = Convert.ToDateTime(consignment.TransportContractDocument.IssueDateTime);
                    consignmentPM.ManifestNumber = GetValueIDType(consignment.TransportContractDocument.ID);
                    if (consignment.TransportContractDocument.DMExtensions != null)
                    {
                        consignmentPM.SecondCargoID = GetValueIDType(consignment.TransportContractDocument.DMExtensions.SecondCargoID);
                        consignmentPM.ThirdCargoID = GetValueIDType(consignment.TransportContractDocument.DMExtensions.ThirdCargoID);
                    }
                    if (IsUpdateDB && declarationPM != null)
                    {
                        consignmentPM.ExportContainerizationID = declarationPM.Consignments.Where(x => x.CargoTypeCode == consignmentPM.CargoTypeCode && x.ManifestNumber == consignmentPM.ManifestNumber && x.SecondCargoID == consignmentPM.SecondCargoID && x.ThirdCargoID == consignmentPM.ThirdCargoID).Select(y => y.ExportContainerizationID).FirstOrDefault();
                    }
                }
                if (consignment.UnloadingLocation != null)
                {
                    consignmentPM.ExportUnloadingPortCode = GetValueIDType(consignment.UnloadingLocation.ID);
                }
                if (consignment.LoadingLocation != null)
                {
                    consignmentPM.ExportLoadingPortCode = GetValueIDType(consignment.LoadingLocation.ID);
                }
                if (consignment.DMExtensions != null)
                {
                    if (IsUpdateDB)
                        consignmentPM.CargoDescription = GetValueTextType(consignment.DMExtensions.CargoDescription);
                    if (!IsUpdateDB)
                        consignmentPM.CargoDescription = GetValueTextType(consignment.DMExtensions.PackagesMeasure[0]?.MarksNumbers);
                    consignmentPM.FinalDestinationPortCode = consignment.DMExtensions.FinalDestinationPort?.Value;
                    consignmentPM.ShipCode = consignment.DMExtensions.ShipID?.Value;


                    if (consignment.DMExtensions.RegisteredFacility != null && consignment.DMExtensions.RegisteredFacility.Count() > 0)
                    {
                        foreach (var registeredFacility in consignment.DMExtensions.RegisteredFacility)
                        {
                            switch (GetValueCodeType(registeredFacility.FacilityType))
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
                                       ///   DeclarationId = GetValueIDType(declaration.ID),
                                          ChangeSetOp = ChangeSetOperation.Insert,
                                          SiteCode=GetValueIDType(registeredFacility.ID)
                                        }
                                        };
                                        break;
                                    }
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(consignmentPM.ExportLoadingPortCode))
                    {
                        consignmentPM.ExportLoadingPortCode = consignmentPM.StorageSiteCode;
                    }
                }

                if (consignment.DMExtensions.PackagesMeasure != null)
                {
                    consignmentPM.ConsignmentPackages = GetConsignmentPackages(consignment.DMExtensions.PackagesMeasure, declaration, tenant);
                }

                consignmentPMs.Add(consignmentPM);
            }
            if (declaration.GoodsShipment[0].ImportConsignment != null)
            {
                foreach (var consignment in declaration.GoodsShipment[0].ImportConsignment)
                {
                    ConsignmentPM consignmentPM = new ConsignmentPM();
                    consignmentPM.ChangeSetOp = ChangeSetOperation.Insert;
                    consignmentPM.Tenant = tenant;
                    consignmentPM.ConsignmentType = "I";
                    var consignmentQueryService = new ConsignmentQueryService(context);

                    if (declarationPM != null)
                    {
                        var maxCounter = consignmentQueryService.GetMaxCounterKey(declarationPM.Id, tenant) ?? 0;
                        consignmentPM.SequenceNumeric = maxCounter + 1;
                    }
                    if (consignment.TransportContractDocument != null)
                    {
                        consignmentPM.CargoTypeCode = GetValueCodeType(consignment.TransportContractDocument.TypeCode);
                        //if (consignment.TransportContractDocument.IssueDateTime != null) consignmentPM.ManifestDate = Convert.ToDateTime(consignment.TransportContractDocument.IssueDateTime);
                        consignmentPM.ManifestNumber = GetValueIDType(consignment.TransportContractDocument.ID);
                        if (consignment.TransportContractDocument.DMExtensions != null)
                        {
                            consignmentPM.SecondCargoID = GetValueIDType(consignment.TransportContractDocument.DMExtensions.SecondCargoID);
                            consignmentPM.ThirdCargoID = GetValueIDType(consignment.TransportContractDocument.DMExtensions.ThirdCargoID);
                        }
                        if (IsUpdateDB && declarationPM != null)
                        {
                            consignmentPM.ExportContainerizationID = declarationPM.Consignments.Where(x => x.CargoTypeCode == consignmentPM.CargoTypeCode && x.ManifestNumber == consignmentPM.ManifestNumber && x.SecondCargoID == consignmentPM.SecondCargoID && x.ThirdCargoID == consignmentPM.ThirdCargoID).Select(y => y.ExportContainerizationID).FirstOrDefault();
                        }
                    }
                    if (consignment.UnloadingLocation != null)
                    {
                        consignmentPM.UnloadPortCode = GetValueIDType(consignment.UnloadingLocation.ID);

                    }
                    if (consignment.LoadingLocation != null)
                    {
                        consignmentPM.LoadingPortCode = GetValueIDType(consignment.LoadingLocation.ID);
                    }
                    if (consignment.DMExtensions != null)
                    {
                        consignmentPM.CargoDescription = GetValueTextType(consignment.DMExtensions.CargoDescription);
                        if (consignment.DMExtensions.LastReleaseFromWarehousInd != null)
                        {
                            if (consignment.DMExtensions.LastReleaseFromWarehousInd.Value == true)
                                consignmentPM.IsLastReleaseFromWarehous = "T";
                            else
                                consignmentPM.IsLastReleaseFromWarehous = "F";
                        }
                        else
                        {
                            consignmentPM.IsLastReleaseFromWarehous = "N";
                        }
                        consignmentPM.OriginCountryCode = GetValueCodeType(consignment.DMExtensions.ExportationCountryCode);
                        if (consignment.DMExtensions.RegisteredFacility != null && consignment.DMExtensions.RegisteredFacility.Count() > 0)
                        {
                            foreach (var registeredFacility in consignment.DMExtensions.RegisteredFacility)
                            {
                                switch (GetValueCodeType(registeredFacility.FacilityType))
                                {
                                    case "004":
                                        {
                                            consignmentPM.StorageSiteCode = GetValueIDType(registeredFacility.ID);
                                            break;
                                        }
                                    case "008":
                                        {
                                            consignmentPM.ExportRecieverWareHouseCode = GetValueIDType(registeredFacility.ID);
                                            break;
                                        }
                                    case "005":
                                        {
                                            consignmentPM.ConsignmentInternalTransitions = new List<ConsignmentInternalTransitionPM>()
                                        {
                                            new ConsignmentInternalTransitionPM (){
                                       ///   DeclarationId = GetValueIDType(declaration.ID),
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


                    consignmentPMs.Add(consignmentPM);
                }
            }
            return consignmentPMs;
        }

        private List<ConsignmentPackagePM> GetConsignmentPackages(DeclarationGoodsShipmentExportConsignmentDMExtensionsPackagesMeasure[] packagesMeasures, Declaration declaration, int tenant)
        {
            List<ConsignmentPackagePM> consignmentPackagePMs = new List<ConsignmentPackagePM>();
            foreach (var packagesMeasure in packagesMeasures)
            {
                ConsignmentPackagePM consignmentPackagePM = new ConsignmentPackagePM();
                consignmentPackagePM.ChangeSetOp = ChangeSetOperation.Insert;
                consignmentPackagePM.PackageMeasureQualifierCode = GetValueCodeType(packagesMeasure.PackageMeasureQualifier);
                consignmentPackagePM.PackageQuantityTypeCode = packagesMeasure.TotalPackageQuantity.unitCode.ToString();
                consignmentPackagePM.PackageQuantity = Convert.ToInt32(packagesMeasure.TotalPackageQuantity.Value);
                if (packagesMeasure.GrossMassMeasure != null)
                {
                    consignmentPackagePM.GrossMassMeasureTypeCode = packagesMeasure.GrossMassMeasure.unitCode.ToString();
                    consignmentPackagePM.GrossMassMeasure = packagesMeasure.GrossMassMeasure.Value;
                }
                consignmentPackagePM.PackageTypeCode = GetValueCodeType(packagesMeasure.TypeCode);
                consignmentPackagePM.MarksNumbers = GetValueTextType(packagesMeasure.MarksNumbers);
                consignmentPackagePM.Tenant = tenant;
                consignmentPackagePMs.Add(consignmentPackagePM);
            }
            return consignmentPackagePMs;
        }

        private string GetAgent(Declaration declaration, ref DeclarationPM declarationPM)
        {
            if (declaration.Agent != null && declaration.Agent.Count() > 0)
            {
                var agent = declaration.Agent.FirstOrDefault(x => GetValueCodeType(x.RoleCode) != "1");
                if (agent != null)
                {
                    declarationPM.AgentId = GetValueIDType(agent.ID);
                }
            }
            return null;
        }

        private List<SupplierInvoicePM> GetSupplierInvoices(Declaration declaration, int tenant, ICustomContext context, string declarationId)
        {
            List<SupplierInvoicePM> supplierInvoicePMs = new List<SupplierInvoicePM>();


            foreach (var item in declaration.GoodsShipment.OrderBy(x => x.SequenceNumeric))
            {
                SupplierInvoicePM supplierInvoicePM = new SupplierInvoicePM()
                {
                    ChangeSetOp = ChangeSetOperation.Insert,
                    SequenceNumeric = (int)item.SequenceNumeric,
                    InvoiceNumber = GetValueIDType(item.Invoice.ID),
                    AccountTypeCode = GetValueCodeType(item.Invoice.TypeCode),
                    DeclarationId = declarationId,
                    Tenant = tenant,
                };
                SupplierInvoiceQueryService supplierInvoiceQueryService = new SupplierInvoiceQueryService(tenant);

                if (item.Invoice.IssueDateTime != null) supplierInvoicePM.IssueDate = Convert.ToDateTime(item.Invoice.IssueDateTime);

                if (item.Invoice.DMExtensions != null)
                {
                    if (item.Invoice.DMExtensions.IsPreferenceDocumentInd != null) supplierInvoicePM.IsPreference = item.Invoice.DMExtensions.IsPreferenceDocumentInd.Value;
                    supplierInvoicePM.DutyRegimeProtocolCode = GetValueCodeType(item.Invoice.DMExtensions.DutyRegimeProtocolCode);
                    supplierInvoicePM.PreferenceDocumentTypeCode = GetValueCodeType(item.Invoice.DMExtensions.PreferenceDocumentType);
                    supplierInvoicePM.InvoiceAmount = GetValueAmountType(item.Invoice.DMExtensions.InvoiceAmount);
                    supplierInvoicePM.PartyRelationshipCode = GetValueCodeType(item.Invoice.DMExtensions.PartyRelationshipCode);
                    if (item.Invoice.DMExtensions.InvoiceAmount != null) supplierInvoicePM.InvoiceCurrencyTypeCode = item.Invoice.DMExtensions.InvoiceAmount.currencyID.ToString();
                    if (item.Invoice.DMExtensions.PaymentDetails != null && item.Invoice.DMExtensions.PaymentDetails.Count() > 0)
                    {
                        List<SupplierInvoicePaymentPM> paymentPMs = new List<SupplierInvoicePaymentPM>();
                        foreach (var payment in item.Invoice.DMExtensions.PaymentDetails)
                        {
                            var paymentPM = new SupplierInvoicePaymentPM
                            {
                                ChangeSetOp = ChangeSetOperation.Insert,
                                Tenant = tenant,
                                DeclarationId = declarationId,
                                SequenceNumeric = (int)payment.SequenceNumeric,
                                PaymentTypeCode = GetValueCodeType(payment.PaymentType),
                                PaymentAmount = payment.PaymentAmount.Value,
                            };
                            paymentPMs.Add(paymentPM);
                        }
                        supplierInvoicePM.SupplierInvoicePayments = paymentPMs;
                    }
                }
                if (item.Invoice.DMExtensions.BuyerDetails != null)
                {
                    supplierInvoicePM.BuyerAddress = item.Invoice.DMExtensions.BuyerDetails.Address;
                    supplierInvoicePM.BuyerName = item.Invoice.DMExtensions.BuyerDetails.Name;
                    supplierInvoicePM.BuyerRoleCode = GetValueCodeType(item.Invoice.DMExtensions.BuyerDetails.RoleCode);
                    supplierInvoicePM.BuyerCountryCode = GetValueCodeType(item.Invoice.DMExtensions.BuyerDetails.IssueLocation);
                }
                if (item.TradeTerms != null)
                {
                    supplierInvoicePM.IncotermCode = GetValueCodeType(item.TradeTerms.ConditionCode);
                }

                supplierInvoicePM.SupplierInvoiceModifications = GetSupplierInvoiceModifications(item, ref supplierInvoicePM, declaration, declarationId, tenant);

                supplierInvoicePM.SupplierInvoiceItems = GetSupplierInvoiceItems(item, declaration, declarationId, tenant, supplierInvoicePM, context);
                supplierInvoicePMs.Add(supplierInvoicePM);
            }
            return supplierInvoicePMs;
        }

        private List<SupplierInvoiceItemPM> GetSupplierInvoiceItems(DeclarationGoodsShipment item, Declaration declaration, string declarationId, int tenant, SupplierInvoicePM supplierInvoicePM, ICustomContext context)
        {
            List<SupplierInvoiceItemPM> supplierInvoiceItemPMs = new List<SupplierInvoiceItemPM>();
            if (item.GovernmentAgencyGoodsItem != null)
            {
                foreach (var governmentAgencyGoodsItem in item.GovernmentAgencyGoodsItem)
                {
                    SupplierInvoiceItemPM supplierInvoiceItemPM = new SupplierInvoiceItemPM();

                    supplierInvoiceItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                    supplierInvoiceItemPM.DeclarationId = declarationId;
                    supplierInvoiceItemPM.SequenceNumeric = (int)governmentAgencyGoodsItem.SequenceNumeric;
                    supplierInvoiceItemPM.OriginCountryCode = GetValueCodeType(governmentAgencyGoodsItem.Origin.CountryCode);
                    supplierInvoiceItemPM.ClaimReasonCode = GetValueCodeType(governmentAgencyGoodsItem.DMExtensions.ClaimReasonCode);
                    supplierInvoiceItemPM.Tenant = tenant;
                    if (governmentAgencyGoodsItem.Commodity.Classification != null || governmentAgencyGoodsItem.Commodity.Classification.Count() > 0)
                    {
                        var classification = governmentAgencyGoodsItem.Commodity.Classification.FirstOrDefault(x => x != null && GetValueCodeType(x.IdentificationTypeCode) == "SSO");

                        if (classification != null)
                        {
                            supplierInvoiceItemPM.DangerousClassificationCode = GetValueIDType(classification.ID);
                        }
                        var classification2 = governmentAgencyGoodsItem.Commodity.Classification.FirstOrDefault(x => x != null && GetValueCodeType(x.IdentificationTypeCode) == "HS");

                        if (classification2 != null)
                        {
                            supplierInvoiceItemPM.ClassificationCode = GetValueIDType(classification2.ID).Replace("/", "");
                        }
                        supplierInvoiceItemPM.DutyRegimeProtocolCode = GetValueCodeType(governmentAgencyGoodsItem.Commodity.Classification[0].DMExtensions.DutyRegimeProtocolCode);
                        supplierInvoiceItemPM.TradeAgreementCode = GetValueCodeType(governmentAgencyGoodsItem.Commodity.Classification[0].DMExtensions.DutyRegimeCode);
                        supplierInvoiceItemPM.ClassificationTypeCode = GetValueCodeType(governmentAgencyGoodsItem.Commodity.Classification[0].IdentificationTypeCode);
                        supplierInvoiceItemPM.TaxExemptCode = GetValueCodeType(governmentAgencyGoodsItem.Commodity.Classification[0].DMExtensions?.TaxExemptCode)?.Replace("/", "");


                    }

                    if (governmentAgencyGoodsItem.GovernmentProcedure != null && governmentAgencyGoodsItem.GovernmentProcedure.Count() > 0)
                    {
                        supplierInvoiceItemPM.SupplierInvoiceItemProcesTypes = new List<SupplierInvoiceItemProcesTypePM>();
                        foreach (var governmentProcedure in governmentAgencyGoodsItem.GovernmentProcedure)
                        {
                            SupplierInvoiceItemProcesTypePM supplierInvoiceItemProcesTypePM = new SupplierInvoiceItemProcesTypePM();
                            supplierInvoiceItemProcesTypePM.ChangeSetOp = ChangeSetOperation.Insert;
                            supplierInvoiceItemProcesTypePM.DeclarationId = declarationId;
                            supplierInvoiceItemProcesTypePM.Tenant = tenant;
                            supplierInvoiceItemProcesTypePM.ProcessTypeCode = GetValueCodeType(governmentProcedure.CurrentCode);
                            supplierInvoiceItemPM.SupplierInvoiceItemProcesTypes.Add(supplierInvoiceItemProcesTypePM);
                        }
                    }

                    foreach (var goodsMeasure in governmentAgencyGoodsItem.GoodsMeasure)
                    {
                        if (goodsMeasure.DMExtensions != null && goodsMeasure.TariffQuantity != null)
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

                    if (governmentAgencyGoodsItem.PreviousDocument != null && governmentAgencyGoodsItem.PreviousDocument.Count() > 0)
                    {
                        foreach (var previousDocument in governmentAgencyGoodsItem.PreviousDocument)
                        {
                            SupplierInvoiceItemsConDeclarPM supplierInvoiceItemsConDeclarPM = new SupplierInvoiceItemsConDeclarPM();
                            supplierInvoiceItemsConDeclarPM.ChangeSetOp = ChangeSetOperation.Insert;
                            supplierInvoiceItemsConDeclarPM.DeclarationNumber = GetValueIDType(previousDocument.ID);
                            supplierInvoiceItemsConDeclarPM.ItemSequence = (int)previousDocument.SequenceNumeric;
                            supplierInvoiceItemsConDeclarPM.DeclarationTypeCode = GetValueCodeType(previousDocument.TypeCode);
                            supplierInvoiceItemsConDeclarPM.Tenant = tenant;
                            if (previousDocument.DMExtensions != null)
                            {
                                supplierInvoiceItemsConDeclarPM.QuantityTypeCode = previousDocument.DMExtensions.QuantityQuantity.unitCode.ToString();
                                supplierInvoiceItemsConDeclarPM.Quantity = previousDocument.DMExtensions.QuantityQuantity.Value;
                                supplierInvoiceItemsConDeclarPM.InvoiceNumber = ((int?)previousDocument.DMExtensions.SequenceNumeric);
                            }
                            supplierInvoiceItemPM.SupplierInvoiceItemsConDeclars.Add(supplierInvoiceItemsConDeclarPM);
                        }
                    }


                    if (governmentAgencyGoodsItem.DMExtensions != null)
                    {
                        if (governmentAgencyGoodsItem.DMExtensions.GoodsItemAmount != null && governmentAgencyGoodsItem.DMExtensions.GoodsItemAmount.Count() > 0)
                        {
                            foreach (var goodsItemAmount in governmentAgencyGoodsItem.DMExtensions.GoodsItemAmount)
                            {

                                if (goodsItemAmount.CustomsValueAmount != null)
                                    switch (GetValueCodeType(goodsItemAmount.AmountType))
                                    {

                                        case "1":
                                            {
                                                if (item.Invoice != null && item.Invoice.DMExtensions != null && item.Invoice.DMExtensions.InvoiceAmount != null)
                                                {
                                                    if (goodsItemAmount.CustomsValueAmount.currencyID.ToString() == item.Invoice.DMExtensions.InvoiceAmount.currencyID.ToString())
                                                    {
                                                        supplierInvoiceItemPM.ItemPrice = GetValueAmountType(goodsItemAmount.CustomsValueAmount);

                                                    }
                                                }
                                                break;
                                            }
                                        case "11":
                                            {
                                                supplierInvoiceItemPM.NonCustomsItemPrice = GetValueAmountType(goodsItemAmount.CustomsValueAmount);
                                                supplierInvoiceItemPM.NonCustomsItemPriceCurCode = goodsItemAmount.CustomsValueAmount.currencyID.ToString();
                                                break;
                                            }
                                        case "5":
                                            {
                                                supplierInvoiceItemPM.WholeSaleItemPrice = GetValueAmountType(goodsItemAmount.CustomsValueAmount);
                                                supplierInvoiceItemPM.WholeSaleItemPriceCurrencyCode = goodsItemAmount.CustomsValueAmount.currencyID.ToString();
                                                break;
                                            }
                                    }
                            }
                        }


                        supplierInvoiceItemPM.TransactionNatureCode = GetValueCodeType(governmentAgencyGoodsItem.DMExtensions.TransactionNatureCode);

                        supplierInvoiceItemPM.SupplierInvoiceItemVehicles = GetSupplierInvoiceItemVehicles(governmentAgencyGoodsItem, declaration, declarationId, tenant);
                      
            
                    }
                    if (supplierInvoiceItemPM.WholeSaleItemPrice.HasValue || supplierInvoiceItemPM.AdditionalQuantity.HasValue || supplierInvoiceItemPM.StatisticQuantity.HasValue)
                    {
                        supplierInvoiceItemPM.ItemAdditionalStatus = true;
                    }
                    supplierInvoiceItemPM.PreferenceDocumentNumber = GetValueIDType(governmentAgencyGoodsItem.DMExtensions.PreferenceDocumentNumber);
                    supplierInvoiceItemPM.ActualInvoiceLines = governmentAgencyGoodsItem.DMExtensions.InvoiceLineNumbers;
                    supplierInvoiceItemPM.SupplierInvoiceItemsMods = GetSupplierInvoiceItemsMods(governmentAgencyGoodsItem, declaration, declarationId, tenant);
                    supplierInvoiceItemPM.SupplierInvoiceItemsPrices = GetSupplierInvoiceItemsPrices(governmentAgencyGoodsItem, declaration, declarationId, tenant);
                    supplierInvoiceItemPM.SuppInvoiceItemsAbachStatements = GetSupplierInvoiceItemsAbachStatements(governmentAgencyGoodsItem, declaration, declarationId, tenant);
                    supplierInvoiceItemPM.SupplierInvoiceItemsSerialNums = GetSupplierInvoiceItemsSerialNums(governmentAgencyGoodsItem, declaration, declarationId, tenant);
                    supplierInvoiceItemPM.SupplierInvoiceItemsProdIdents = GetSupplierInvoiceItemsProdIdents(governmentAgencyGoodsItem, declaration, declarationId, tenant);
                    supplierInvoiceItemPM.SupplierInvoiceItemsDescripts = GetSupplierInvoiceItemsDescript(governmentAgencyGoodsItem, declaration, declarationId, tenant);
                    supplierInvoiceItemPM.SupplierInvoiceItemLevies = GetSupplierInvoiceItemLevy(governmentAgencyGoodsItem, declaration, declarationId, tenant);
                    supplierInvoiceItemPM.SupplierInvioceItemCertificats = GetSupplierInvioceItemCertificats(governmentAgencyGoodsItem, declaration, declarationId, tenant);

                    supplierInvoiceItemPMs.Add(supplierInvoiceItemPM);
                }
            }
            return supplierInvoiceItemPMs;
        }


        private List<SupplierInvoiceItemsModPM> GetSupplierInvoiceItemsMods(DeclarationGoodsShipmentGovernmentAgencyGoodsItem governmentAgencyGoodsItem, Declaration declaration, string declarationId, int tenant)
        {
            List<SupplierInvoiceItemsModPM> supplierInvoiceItemsModPMs = new List<SupplierInvoiceItemsModPM>();

            if (governmentAgencyGoodsItem != null && governmentAgencyGoodsItem.DMExtensions.ValuationAdjustment != null)
            {
                foreach (var valuationAdjustment in governmentAgencyGoodsItem.DMExtensions.ValuationAdjustment)
                {
                    SupplierInvoiceItemsModPM supplierInvoiceItemsMod = new SupplierInvoiceItemsModPM();
                    supplierInvoiceItemsMod.DeclarationId = declarationId;
                    supplierInvoiceItemsMod.ChangeSetOp = ChangeSetOperation.Insert;
                    supplierInvoiceItemsMod.TypeCode = GetValueCodeType(valuationAdjustment.AdditionCode);
                    supplierInvoiceItemsMod.CurrencyTypeCode = valuationAdjustment.AmountAmount.currencyID.ToString();
                    supplierInvoiceItemsMod.Amount = GetValueAmountType(valuationAdjustment.AmountAmount);
                    supplierInvoiceItemsMod.Tenant = tenant;
                    supplierInvoiceItemsModPMs.Add(supplierInvoiceItemsMod);
                }
            }
            return supplierInvoiceItemsModPMs;
        }

        private List<SupplierInvoiceItemsPricePM> GetSupplierInvoiceItemsPrices(DeclarationGoodsShipmentGovernmentAgencyGoodsItem governmentAgencyGoodsItem, Declaration declaration, string declarationId, int tenant)
        {
            List<SupplierInvoiceItemsPricePM> SupplierInvoiceItemsPricePM = new List<SupplierInvoiceItemsPricePM>();


            if (governmentAgencyGoodsItem != null && governmentAgencyGoodsItem.DMExtensions.GoodsItemAmount != null)
            {
                var arrAmountType = new string[] { "1", "3", "7" };
                var cur = declaration.GoodsShipment[0].Invoice.DMExtensions.InvoiceAmount.currencyID.ToString();
                foreach (var GoodsItemAmount in governmentAgencyGoodsItem.DMExtensions.GoodsItemAmount)
                {
                    bool IsExist = SupplierInvoiceItemsPricePM.Any(x => x.AdditionalPriceTypeCode == GetValueCodeType(GoodsItemAmount.AmountType));
                    if (!IsExist && !(arrAmountType.Contains(GetValueCodeType(GoodsItemAmount.AmountType))) && GoodsItemAmount.CustomsValueAmount.currencyID.ToString() == cur)
                    {
                        SupplierInvoiceItemsPricePM supplierInvoiceItemsPrice = new SupplierInvoiceItemsPricePM();
                        supplierInvoiceItemsPrice.DeclarationId = declarationId;
                        supplierInvoiceItemsPrice.ChangeSetOp = ChangeSetOperation.Insert;
                        supplierInvoiceItemsPrice.AdditionalPrice = GetValueAmountType(GoodsItemAmount.CustomsValueAmount);

                        supplierInvoiceItemsPrice.AdditionalPriceTypeCode = GetValueCodeType(GoodsItemAmount.AmountType);
                        supplierInvoiceItemsPrice.Tenant = tenant;
                        SupplierInvoiceItemsPricePM.Add(supplierInvoiceItemsPrice);
                    }
                }
            }
            return SupplierInvoiceItemsPricePM;
        }


        private List<SuppInvoiceItemsAbachStatementPM> GetSupplierInvoiceItemsAbachStatements(DeclarationGoodsShipmentGovernmentAgencyGoodsItem governmentAgencyGoodsItem, Declaration declaration, string declarationId, int tenant)
        {
            List<SuppInvoiceItemsAbachStatementPM> supplierInvoiceItemsAbachStatementPM = new List<SuppInvoiceItemsAbachStatementPM>();


            if (governmentAgencyGoodsItem != null && governmentAgencyGoodsItem.Commodity.Classification[0].DangerousGoodsStatement != null)
            {
                foreach (var GoodsItemAbachStatement in governmentAgencyGoodsItem.Commodity.Classification[0].DangerousGoodsStatement)
                {
                    SuppInvoiceItemsAbachStatementPM supplierInvoiceItemsAbachStatement = new SuppInvoiceItemsAbachStatementPM();
                    supplierInvoiceItemsAbachStatement.DeclarationId = declarationId;
                    supplierInvoiceItemsAbachStatement.ChangeSetOp = ChangeSetOperation.Insert;
                    supplierInvoiceItemsAbachStatement.StatementTypeCode = GetValueIDType(GoodsItemAbachStatement.StatementType);
                    supplierInvoiceItemsAbachStatement.IsStatementInd = GoodsItemAbachStatement.DangerousGoodsStatementInd.Value;
                    supplierInvoiceItemsAbachStatement.SequenceNumeric = GoodsItemAbachStatement.SequenceNumeric;
                    supplierInvoiceItemsAbachStatement.Tenant = tenant;
                    supplierInvoiceItemsAbachStatementPM.Add(supplierInvoiceItemsAbachStatement);
                }
            }
            return supplierInvoiceItemsAbachStatementPM;
        }

        private List<SupplierInvoiceItemsSerialNumPM> GetSupplierInvoiceItemsSerialNums(DeclarationGoodsShipmentGovernmentAgencyGoodsItem governmentAgencyGoodsItem, Declaration declaration, string declarationId, int tenant)
        {
            List<SupplierInvoiceItemsSerialNumPM> supplierInvoiceItemsSerialNumPM = new List<SupplierInvoiceItemsSerialNumPM>();


            if (governmentAgencyGoodsItem != null && governmentAgencyGoodsItem.Commodity.Classification[0].SerialNumbers != null)
            {
                foreach (var GoodsItemSerialNumbers in governmentAgencyGoodsItem.Commodity.Classification[0].SerialNumbers)
                {
                    SupplierInvoiceItemsSerialNumPM supplierInvoiceItemsSerialNum = new SupplierInvoiceItemsSerialNumPM();
                    supplierInvoiceItemsSerialNum.DeclarationId = declarationId;
                    supplierInvoiceItemsSerialNum.ChangeSetOp = ChangeSetOperation.Insert;
                    supplierInvoiceItemsSerialNum.TypeCode = GetValueCodeType(GoodsItemSerialNumbers.IdentityQualifierCode);
                    supplierInvoiceItemsSerialNum.SerialNumber = GetValueIDType(GoodsItemSerialNumbers.ID);
                    supplierInvoiceItemsSerialNum.Tenant = tenant;
                    supplierInvoiceItemsSerialNumPM.Add(supplierInvoiceItemsSerialNum);
                }
            }
            return supplierInvoiceItemsSerialNumPM;
        }
        private List<SupplierInvoiceItemsProdIdentPM> GetSupplierInvoiceItemsProdIdents(DeclarationGoodsShipmentGovernmentAgencyGoodsItem governmentAgencyGoodsItem, Declaration declaration, string declarationId, int tenant)
        {
            List<SupplierInvoiceItemsProdIdentPM> supplierInvoiceItemsProdIdentPM = new List<SupplierInvoiceItemsProdIdentPM>();


            if (governmentAgencyGoodsItem != null && governmentAgencyGoodsItem.Commodity.Classification[0].ProductIdentification != null)
            {
                foreach (var GoodsItemProdIdent in governmentAgencyGoodsItem.Commodity.Classification[0].ProductIdentification)
                {
                    SupplierInvoiceItemsProdIdentPM supplierInvoiceItemsProdIdent = new SupplierInvoiceItemsProdIdentPM();
                    supplierInvoiceItemsProdIdent.DeclarationId = declarationId;
                    supplierInvoiceItemsProdIdent.ChangeSetOp = ChangeSetOperation.Insert;
                    supplierInvoiceItemsProdIdent.TypeCode = GetValueCodeType(GoodsItemProdIdent.IDTypeCode);
                    supplierInvoiceItemsProdIdent.Identification = GetValueIDType(GoodsItemProdIdent.ID);
                    supplierInvoiceItemsProdIdent.Tenant = tenant;
                    supplierInvoiceItemsProdIdentPM.Add(supplierInvoiceItemsProdIdent);
                }
            }
            return supplierInvoiceItemsProdIdentPM;
        }

        private List<SupplierInvoiceItemsDescriptPM> GetSupplierInvoiceItemsDescript(DeclarationGoodsShipmentGovernmentAgencyGoodsItem governmentAgencyGoodsItem, Declaration declaration, string declarationId, int tenant)
        {
            List<SupplierInvoiceItemsDescriptPM> supplierInvoiceItemsDescriptPM = new List<SupplierInvoiceItemsDescriptPM>();


            if (governmentAgencyGoodsItem != null && governmentAgencyGoodsItem.Commodity.Classification[0].ProductName != null)
            {
                foreach (var GoodsItemDescript in governmentAgencyGoodsItem.Commodity.Classification[0].ProductName)
                {
                    SupplierInvoiceItemsDescriptPM supplierInvoiceItemsDescript = new SupplierInvoiceItemsDescriptPM();
                    supplierInvoiceItemsDescript.DeclarationId = declarationId;
                    supplierInvoiceItemsDescript.ChangeSetOp = ChangeSetOperation.Insert;
                    supplierInvoiceItemsDescript.TypeCode = GetValueCodeType(GoodsItemDescript.NameQualifierCode);
                    supplierInvoiceItemsDescript.Description = GetValueTextType(GoodsItemDescript.Name);
                    supplierInvoiceItemsDescript.Tenant = tenant;
                    supplierInvoiceItemsDescriptPM.Add(supplierInvoiceItemsDescript);
                }
            }
            return supplierInvoiceItemsDescriptPM;
        }

        private List<SupplierInvoiceItemsLevyPM> GetSupplierInvoiceItemLevy(DeclarationGoodsShipmentGovernmentAgencyGoodsItem governmentAgencyGoodsItem, Declaration declaration, string declarationId, int tenant)
        {
            List<SupplierInvoiceItemsLevyPM> supplierInvoiceItemsLevyPM = new List<SupplierInvoiceItemsLevyPM>();


            if (governmentAgencyGoodsItem != null && governmentAgencyGoodsItem.Commodity.Classification[0].TradeLevyAndExampt != null)
            {
                foreach (var GoodsItemLevy in governmentAgencyGoodsItem.Commodity.Classification[0].TradeLevyAndExampt)
                {
                    SupplierInvoiceItemsLevyPM supplierInvoiceItemsLevy = new SupplierInvoiceItemsLevyPM();
                    supplierInvoiceItemsLevy.DeclarationId = declarationId;
                    supplierInvoiceItemsLevy.ChangeSetOp = ChangeSetOperation.Insert;
                    supplierInvoiceItemsLevy.TradeLevyExamptCode = GetValueCodeType(GoodsItemLevy.TradeLevyExamptCode);
                    supplierInvoiceItemsLevy.TradeLevyNumber = GetValueIDType(GoodsItemLevy.TradeLevyNumber);
                    supplierInvoiceItemsLevy.Tenant = tenant;
                    supplierInvoiceItemsLevyPM.Add(supplierInvoiceItemsLevy);
                }
            }
            return supplierInvoiceItemsLevyPM;
        }

        private List<SupplierInvoiceItemVehiclePM> GetSupplierInvoiceItemVehicles(DeclarationGoodsShipmentGovernmentAgencyGoodsItem governmentAgencyGoodsItem, Declaration declaration, string declarationId, int tenant)
        {
            List<SupplierInvoiceItemVehiclePM> SupplierInvoiceItemVehiclePMs = new List<SupplierInvoiceItemVehiclePM>();
            if (governmentAgencyGoodsItem.DMExtensions.Vehicle != null)
                foreach (var vehicle in governmentAgencyGoodsItem.DMExtensions.Vehicle)
                {
                    SupplierInvoiceItemVehiclePM supplierInvoiceItemVehiclePM = new SupplierInvoiceItemVehiclePM();
                    supplierInvoiceItemVehiclePM.ChangeSetOp = ChangeSetOperation.Insert;
                    supplierInvoiceItemVehiclePM.DeclarationId = declarationId;
                    supplierInvoiceItemVehiclePM.Tenant = tenant;
                    if (GetValueCodeType(vehicle.IDTypeCode) == "ZZZ")
                    {
                        supplierInvoiceItemVehiclePM.RichbitFileNumber = GetValueIDType(vehicle.ID);
                    }
                    if (GetValueCodeType(vehicle.IDTypeCode) == "CN")
                    {
                        supplierInvoiceItemVehiclePM.VehicleChassisNumber = GetValueIDType(vehicle.ID);
                    }
                    supplierInvoiceItemVehiclePM.VehicleTypeCode = GetValueCodeType(vehicle.IDTypeCode);
                    //vehicleDetails.ID.Value = supplierInvoiceItemVehicleItem.VehicleChassisNumber;
                    //vehicleDetails.IDTypeCode.Value = supplierInvoiceItemVehicleItem.VehicleTypeCode
                    SupplierInvoiceItemVehiclePMs.Add(supplierInvoiceItemVehiclePM);
                }
            return SupplierInvoiceItemVehiclePMs;
        }

        private List<SupplierInvoiceModificationPM> GetSupplierInvoiceModifications(DeclarationGoodsShipment declarationGoodsShipment,
            ref SupplierInvoicePM supplierInvoicePM, Declaration declaration, string declarationId, int tenant)
        {
            List<SupplierInvoiceModificationPM> supplierInvoiceModificationPMs = new List<SupplierInvoiceModificationPM>();

            SupplierInvoiceModificationQueryService supplierInvoiceModificationQueryService = new SupplierInvoiceModificationQueryService(tenant);

            if (declarationGoodsShipment.Invoice.DMExtensions.CustomsValuation != null)
            {
                foreach (var customsValuation in declarationGoodsShipment.Invoice.DMExtensions.CustomsValuation)
                {

                    if (GetValueAmountType(customsValuation.OtherChargeDeductionAmount) == 0) continue;

                    SupplierInvoiceModificationPM supplierInvoiceModificationPM = new SupplierInvoiceModificationPM()
                    {
                        ChangeSetOp = ChangeSetOperation.Insert,
                        DeclarationId = declarationId,
                        TypeCode = GetValueCodeType(customsValuation.ChargesTypeCode),
                        CurrencyTypeCode = customsValuation.OtherChargeDeductionAmount.currencyID.ToString(),
                        Amount = GetValueAmountType(customsValuation.OtherChargeDeductionAmount),
                        Tenant = tenant
                    };
                    supplierInvoiceModificationPMs.Add(supplierInvoiceModificationPM);

                    if (customsValuation.OtherChargeDeductionAmount != null)
                    {
                        if (customsValuation.ChargesTypeCode.Value == "67")
                        {
                            supplierInvoicePM.InsruanceCurrencyTypeCode = customsValuation.OtherChargeDeductionAmount.currencyID.ToString();
                            supplierInvoicePM.InsuranceAmount = GetValueAmountType(customsValuation.OtherChargeDeductionAmount);
                        }
                        if (customsValuation.ChargesTypeCode.Value == "144")
                        {
                            supplierInvoicePM.FreightCurrencyTypeCode = customsValuation.OtherChargeDeductionAmount.currencyID.ToString();
                            supplierInvoicePM.TotalFreightInFreightCurrency = GetValueAmountType(customsValuation.OtherChargeDeductionAmount);
                        }
                    }
                }

            }
            return supplierInvoiceModificationPMs;
        }

        private List<SupplierInvioceItemCertificatPM> GetSupplierInvioceItemCertificats(DeclarationGoodsShipmentGovernmentAgencyGoodsItem governmentAgencyGoodsItem, Declaration declaration, string declarationId, int tenant)
        {
            if (governmentAgencyGoodsItem == null || governmentAgencyGoodsItem.AdditionalDocument == null) return null;
            List<SupplierInvioceItemCertificatPM> supplierInvioceItemCertificatPMs = new List<SupplierInvioceItemCertificatPM>();
            foreach (var additionalDocument in governmentAgencyGoodsItem.AdditionalDocument)
            {
                SupplierInvioceItemCertificatPM supplierInvioceItemCertificatPM = new SupplierInvioceItemCertificatPM();
                supplierInvioceItemCertificatPM.CertificateNumber = GetValueIDType(additionalDocument.ID);
                supplierInvioceItemCertificatPM.CertificateExemptionTypeCode = GetValueCodeType(additionalDocument.LPCOExemptionCode);
                supplierInvioceItemCertificatPM.AttachmentTypeCode = GetValueCodeType(additionalDocument.TypeCode);
                if (additionalDocument.DMExtensions != null)
                {
                    supplierInvioceItemCertificatPM.ResConfirmationTypeCode = GetValueCodeType(additionalDocument.DMExtensions.LPCOTypeCode);
                    supplierInvioceItemCertificatPM.ReqConfirmationTypeCode = GetValueCodeType(additionalDocument.DMExtensions.RequirementLicenseType);
                    supplierInvioceItemCertificatPM.CustomsAttachmentID = GetValueIDType(additionalDocument.DMExtensions.ExternalAttachmentID);
                    supplierInvioceItemCertificatPM.SequenceNumeric = Convert.ToInt32(additionalDocument.DMExtensions.SequenceNumeric);

                }
                supplierInvioceItemCertificatPM.ChangeSetOp = ChangeSetOperation.Insert;
                supplierInvioceItemCertificatPM.Tenant = tenant;
                supplierInvioceItemCertificatPMs.Add(supplierInvioceItemCertificatPM);
            }
            return supplierInvioceItemCertificatPMs;
        }

        private List<DeclarationTaxPM> GetDeclarationTaxesPM(Declaration declaration, string declarationId, int tenant)
        {
            var declarationTaxPMList = new List<DeclarationTaxPM>();


            if (declaration.DutyTaxFee == null)
            {
                return null;
            }
            foreach (var dutyTaxFee in declaration?.DutyTaxFee)
            {
                var declarationTaxPM = new DeclarationTaxPM();
                declarationTaxPM.ChangeSetOp = ChangeSetOperation.Insert;
                declarationTaxPM.DeclarationId = declarationId;
                declarationTaxPM.Tenant = tenant;
                declarationTaxPM.TaxTypeCode = dutyTaxFee.TypeCode.Value;
                declarationTaxPM.TotalAmount = dutyTaxFee.DMExtensions.CalculatedTax.Amount.Value;
                //declarationTaxPM.DeferredTaxAmount = dutyTaxFee.DMExtensions.CalculatedTax.DeferedTaxAmount.Value;
                declarationTaxPM.TaxBaseAmount = dutyTaxFee.AdValoremTaxBaseAmount.Value;
                declarationTaxPMList.Add(declarationTaxPM);
            }
            return declarationTaxPMList;
        }

        private static void RaiseEvent(DeclarationPM dirtyDeclarationPM, string loggingUserId, string status_id)
        {
            //primary_number = $"{dirtyDeclarationPM.CustomFileNo},{dirtyDeclarationPM.TransportModeId == "A" ? "EFIFILEM" : "MFIFILEM" }",
            string primary_number = $"{dirtyDeclarationPM.CustomFileNo},EFIFILEM";
            if (dirtyDeclarationPM.TransportModeId != "A")
            {
                primary_number = $"{dirtyDeclarationPM.CustomFileNo},MFIFILEM";
            }

            var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
            {
                Tenant = dirtyDeclarationPM.Tenant,
                objectTableName = "Customs.Declaration",
                EventCode = status_id,
                notes = "",
                CommunicationLoggingEntityReference = dirtyDeclarationPM.DeclarationNumber,
                EntityId = dirtyDeclarationPM.Id,
                UserId = loggingUserId,

                CommunicationSubject = "FU Status " + status_id + " from logitude",
                MyFUStatus = new AmitalEventTracerModel.FUStatus()
                {
                    entname = dirtyDeclarationPM.Direction == "E" ? "BFIFILE" : "CFIFILEM",
                    primary_number = primary_number,
                    status = "new",
                    xml_status = "new",
                    status_id = status_id,
                    status_DateTime = DateTime.Now,
                    comments = "",
                }
            };

            AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel, suppress_RAISE_EVENT: true);


        }


        #region Helpers
        private string GetValueIDType<T>(T codeType) where T : IDType
        {
            if (codeType != null)
                return codeType.Value;
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


        private decimal GetValueAmountType<T>(T codeType) where T : AmountType
        {
            if (codeType != null)
                return codeType.Value;
            return 0;
        }

        #endregion
        public class GeneralMessage
        {
            public string Message { get; set; }
        }
        private void DeleteSomeObjects(DeclarationPM declarationPM, int tenant, ICustomContext context)
        {
            DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(tenant);
            declarationUpdateService.DeclarationConsignmentsFastDelete(declarationPM, context);
            declarationUpdateService.DeclarationRecipientFastDelete(declarationPM, context);
            declarationUpdateService.DeclarationClosingDataFastDelete(declarationPM, context);
            declarationUpdateService.DeclarationSupplierInvoicesFastDelete(declarationPM, context);

            var mySupplierInvoiceItemsTaxUpdateService = new SupplierInvoiceItemsTaxUpdateService(context, new Dictionary<string, IContext>(), tenant);
            var mySupplierInvoiceItemVehicleModUpdateService = new SupplierInvoiceItemVehicleModUpdateService(context, new Dictionary<string, IContext>(), tenant); // moran 20.10.15 - Task 17209 
            var mySupplierInvoiceItemModVehicleUpdateService = new SupplierInvoiceItemModVehicleUpdateService(context, new Dictionary<string, IContext>(), tenant); // moran 24.11.15 - Task 17424 
            var mySupplierInvoiceItemsPriceUpdateService = new SupplierInvoiceItemsPriceUpdateService(context, new Dictionary<string, IContext>(), tenant); // moran 24.11.15 - Task 17424
            var mySupplierInvoiceItemsLevyUpdateService = new SupplierInvoiceItemsLevyUpdateService(context, new Dictionary<string, IContext>(), tenant); // moran 24.11.15 - Task 17424
            var mySupplierInvoiceItemsDescriptUpdateService = new SupplierInvoiceItemsDescriptUpdateService(context, new Dictionary<string, IContext>(), tenant); // moran 24.11.15 - Task 17424
            var mySupplierInvoiceItemsProdIdentUpdateService = new SupplierInvoiceItemsProdIdentUpdateService(context, new Dictionary<string, IContext>(), tenant); // moran 24.11.15 - Task 17424
            var mySupplierInvoiceItemsSerialNumUpdateService = new SupplierInvoiceItemsSerialNumUpdateService(context, new Dictionary<string, IContext>(), tenant); // moran 24.11.15 - Task 17424
            var mySuppInvoiceItemsAbachStatementUpdateService = new SuppInvoiceItemsAbachStatementUpdateService(context, new Dictionary<string, IContext>(), tenant);                                                                                                       //var mySupplierInvoiceItemsTaxesModificationUpdateService = new SupplierInvoiceItemsTaxesModUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            var myDeclarationTaxUpdateService = new DeclarationTaxUpdateService(context, new Dictionary<string, IContext>(), tenant);





            var myDeclarationKeys = new DeclarationKeys { Id = declarationPM.Id };
            myDeclarationTaxUpdateService.FastDeleteComposition(myDeclarationKeys);
            mySupplierInvoiceItemVehicleModUpdateService.FastDeleteComposition(myDeclarationKeys);
            mySupplierInvoiceItemsTaxUpdateService.FastDeleteComposition(myDeclarationKeys);
            mySupplierInvoiceItemModVehicleUpdateService.FastDeleteComposition(myDeclarationKeys);
            mySupplierInvoiceItemsPriceUpdateService.FastDeleteComposition(myDeclarationKeys);
            mySupplierInvoiceItemsLevyUpdateService.FastDeleteComposition(myDeclarationKeys);
            mySupplierInvoiceItemsDescriptUpdateService.FastDeleteComposition(myDeclarationKeys);
            mySupplierInvoiceItemsProdIdentUpdateService.FastDeleteComposition(myDeclarationKeys);
            mySupplierInvoiceItemsSerialNumUpdateService.FastDeleteComposition(myDeclarationKeys);
            mySuppInvoiceItemsAbachStatementUpdateService.FastDeleteComposition(myDeclarationKeys);

            (context as DbContextBase).SaveChanges();
        }
    }
}
