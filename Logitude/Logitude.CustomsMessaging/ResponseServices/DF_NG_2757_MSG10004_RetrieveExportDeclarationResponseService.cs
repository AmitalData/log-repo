
using Logitude.AmitalMessaging.Utils;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using Newtonsoft.Json;
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


            if (string.IsNullOrEmpty(requestParams.DeclarationId)) { //declaration not exits in db
                if(customResponse.Response!=null&& customResponse.Response.Declaration!=null)
                   CreateDeclarationFromResponse(customResponse.Response.Declaration, requestParams.Tenant,  customResponse);
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
                    ResponseStatusXML = GetDummyXml(customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription+customResponse.ResponseContentHeader.Remark,requestParams.IsAngularClient),
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
                            _MyDefaultResponseData.ResponseStatusXML = GetDummyXml(_DF_NG_2754_MSG10004_ExportDeclarationResponseService.MyResponseData.UserMessage,requestParams.IsAngularClient);
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


        private string GetDummyXml(string message,bool toJson)
        {
            var myDummyXml = new GeneralMessage() { Message = message };
            if (toJson)
            {
                return JsonConvert.SerializeObject(myDummyXml);
            }
            var xml = XmlGenericUtil<GeneralMessage>.SerializeObject(myDummyXml);
            return xml;
        }
       
        public void CreateDeclarationFromResponse(Declaration declaration, int tenant, DF_NG_2757_MSG10004_ExportDeclarationResponse customResponse)
        {
           
            try
            {
                var context = CustomContext.GetContext(tenant);
                DeclarationRepository declarationRepository = new DeclarationRepository(context);
                var myQueryService = new DeclarationQueryService(context);
                DeclarationPM declarationOrg;
                DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), tenant);
               

                List<SupplierInvoicePM> invoicePMs = new List<SupplierInvoicePM>(); ;
                DeclarationPM declarationPM;

                    declarationPM = new DeclarationPM()
                    {
                        ChangeSetOp = ChangeSetOperation.Insert,
                        DeclarationOfficeCode = GetValueIDType(declaration.DeclarationOfficeID),
                        Tenant = tenant,
                        Direction = "E",
                        IsConnectedToUnifreight = false,
                        AmendmentDontDisplayInList = false,                       
                        ExportDeclarationOfficeCode = GetValueIDType(declaration.ExportDeclarationOfficeID),
                        DeclarationTypeCode = GetValueCodeType(declaration.TypeCode),
                        Consignments = GetConsignments(declaration, tenant, null, context),
                    };
                declarationPM.DeclarationNumber = customResponse.Response.Declaration.ID.Value;
                declarationPM.IsExportClosed = customResponse.Response.Status[0].NameCode.Value=="36"?true:false;             
                declarationPM.AgentRoleCode = "A";     
                declarationPM.TotalTax = Math.Round(declaration.DMExtensions.CustomsValueComponent.TaxAssessedAmount.Value, 2);               
                declarationPM.TaxationDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
                decimal DealValueWithoutFactor = 0;
                if (declaration.GoodsShipment != null)
                {
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
                    case "1" :
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
                  
                GetAgent(declaration, ref declarationPM);

                if (declaration.GovernmentProcedure != null)
                {
                    declarationPM.ProcedureCurrentCode = declaration.GovernmentProcedure.CurrentCode.Value;
                }
                if (declaration.DMExtensions != null)
                {
                    declarationPM.DeclarationExportRecipients = GetRecipients(declaration, tenant, declarationPM, context);
                   
                    
                    declarationPM.CustomFileNo = GetValueIDType(declaration.DMExtensions.AgentFileReferenceID);
                    declarationPM.ExportFile = GetValueIDType(declaration.DMExtensions.AgentFileReferenceID);              
                    declarationPM.ExternalDeclarationNumber = GetValueIDType(declaration.DMExtensions.ExternalDeclarationID);
                    declarationPM.DestinationCountryCode = GetValueCodeType(declaration.DMExtensions.DestinationCountry);                  
                    declarationPM.ExportAutonomyRegionTypeCode = GetValueIDType(declaration.DMExtensions.AutonomyRegionType);
                    if (declaration.DMExtensions.TransferDeclarationToDestinationCountry != null)
                        declarationPM.IsExporterConfirmation = declaration.DMExtensions.TransferDeclarationToDestinationCountry.Value;
                    if (declaration.DMExtensions.ReferenceDateTime != null)
                        declarationPM.TaxationDateTime = Convert.ToDateTime(declaration.DMExtensions.ReferenceDateTime);

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
                    declarationPM.CustomerId = declarationPM.ImporterId;
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
                declarationPM.SupplierInvoices = GetSupplierInvoices(declaration, tenant, context, declarationId);               
                declarationPM.ChangeSetOp = ChangeSetOperation.Update;
                declarationPM.Consignments.ForEach(x => x.ChangeSetOp = ChangeSetOperation.None);
                declarationPM.DeclarationExportRecipients.ForEach(x => x.ChangeSetOp = ChangeSetOperation.None);
                declarationUpdateService.Update(declarationPM, true);
              


                CreateEvent( tenant,  declarationId);
            }
            catch (System.Exception ex)
            {
              
               // return null;
            }
        }
        private void CreateEvent(int tenant,string declarationId)
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
                closingDetails.FinalManifestNumber = GetValueIDType(declaration.DMExtensions.DeclarationClosingDetails.FinalTransportContractDocument.FirstCargoID);
                closingDetails.FinalCargoTypeCode = GetValueCodeType(declaration.DMExtensions.DeclarationClosingDetails.FinalTransportContractDocument.TypeCode);
                closingDetails.FinalSecondCargoId = GetValueIDType(declaration.DMExtensions.DeclarationClosingDetails.FinalTransportContractDocument.SecondCargoID);
                closingDetails.FinalThirdCargoId = GetValueIDType(declaration.DMExtensions.DeclarationClosingDetails.FinalTransportContractDocument.ThirdCargoID);
                closingDetails.ChangeSetOp = ChangeSetOperation.Insert;
                return closingDetails;
            }
            return null;
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

        private List<ConsignmentPM> GetConsignments(Declaration declaration, int tenant, DeclarationPM declarationPM, ICustomContext context)
        {
            if (declaration.GoodsShipment == null || declaration.GoodsShipment.Count() == 0) return null;
            if ((declaration.GoodsShipment[0].ExportConsignment == null && declaration.GoodsShipment[0].ImportConsignment == null) ||
                (declaration.GoodsShipment[0].ExportConsignment.Count() == 0 && declaration.GoodsShipment[0].ImportConsignment.Count() == 0)) return null;

            List<ConsignmentPM> consignmentPMs = new List<ConsignmentPM>();
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
                    consignmentPM.CargoDescription = GetValueTextType(consignment.DMExtensions.CargoDescription);
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
                var cur = declaration.GoodsShipment[0].Invoice.DMExtensions.InvoiceAmount.currencyID.ToString();
                foreach (var GoodsItemAmount in governmentAgencyGoodsItem.DMExtensions.GoodsItemAmount)
                {
                    bool IsExist = SupplierInvoiceItemsPricePM.Any(x => x.AdditionalPriceTypeCode == GetValueCodeType(GoodsItemAmount.AmountType));
                    if (!IsExist && GetValueCodeType(GoodsItemAmount.AmountType) != "1" && GoodsItemAmount.CustomsValueAmount.currencyID.ToString() == cur)
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
    }
}
