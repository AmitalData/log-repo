
using DocumentFormat.OpenXml.Office2010.Excel;
using Logitude.AmitalMessaging.Utils;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityMapping;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Def.EntityQueryServicesExt;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using Logitude.CustomsMessaging.Testers.Messages;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Utils;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Serialization;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.Repsitories;
using UnifreightIIG.Common.CommonIIGInterface;
using UnifreightIIG.Common.MessageLib.PhysicalCheck;
using UnifreightIIG.Common.SystemTableServiceReference;
using Exception = System.Exception;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInUCSBondedDocument_MessagingService
        : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        SYSTBL_NG_9000_MSG_SystemTableRequest,
        DCAInUCSBondedWithResponseContentHeader,
        DCAInCustomReturnNullRequestService,
        UniCustomBatchSend2715Bonded_MsgResponseService, RequestHeader>

    {

        public override string MainInterfaceCode
        {
            get { return "UCBNDCD"; }
        }

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(DCAInUCSBondedWithResponseContentHeader customsResponse)
        {
            var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            var objectTableId2 = ObjectTableRepository.GetObjectTableByName("DocumentsFiling");
            var genericRequestParams = new GenericRequestParams()
            {
                Tenant = customsResponse.tenant,
                AppicationId = customsResponse.DeclarationId,
                //RequestVIA = SendRequestVIA.WebServiceBatch,
                LoggingEnabled = true,
                InterfaceTypeCode = this.MainInterfaceCode,
                MainInterfaceCode = this.MainInterfaceCode,

                LoggingObjectTableId = objectTableId2,
                LoggingEntityId = customsResponse.DocumentsFilingId,
                LoggingEntityReference = customsResponse.LoggingEntityReference,
                //LoggingObjectTableId = objectTableId,
                //LoggingEntityId = customsResponse.DeclarationId,
                //LoggingObjectTableId2 = objectTableId2,
                //LoggingEntityId2 = customsResponse.DocumentsFilingId,

                LoggingUserId = customsResponse.LoggingUserId,
                RequestName = $" UCBNDCD   שליחת מסמך למכס " + customsResponse.LoggingEntityReference + " "
            };

            return genericRequestParams;
        }

        protected override DCAInUCSBondedWithResponseContentHeader CallWS(SYSTBL_NG_9000_MSG_SystemTableRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }






        public string CreateCRS(
            int tenant,
            string LoggingUserId,
            DocumentsFilingPM documentsFilingPM,
            string CustomsDoucumentTypeCode, out string RequestInProgressListOut
            )
        {
			Stopwatch _Stopwatch1;



            DateTime stopLogAt = DateTime.MinValue;//DateTime stopLogAt = new DateTime(2020, 09, 01);
            string UntilDateyyyyMMdd = ConfigurationManager.AppSettings["20220227T155633.LogUntilDateyyyyMMdd"];
            if (!string.IsNullOrWhiteSpace(UntilDateyyyyMMdd))
            {
                stopLogAt = DateTime.ParseExact(UntilDateyyyyMMdd,
                                                    "yyyyMMdd",
                                                    CultureInfo.InvariantCulture,
                                                    DateTimeStyles.None);
            }
			var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            var objectTableDocumentsFilingId = ObjectTableRepository.GetObjectTableByName("DocumentsFiling");
            var customsRequestsSheetQS = new CustomsRequestsSheetQueryService(tenant);

            List<CustomsRequestsSheetPM> RequestInProgressList;
            if (string.IsNullOrWhiteSpace(documentsFilingPM.EntityId))
            {
                RequestInProgressList = customsRequestsSheetQS
             .GetRequestInProgress(tenant, this.MainInterfaceCode,
             objectTableDocumentsFilingId, documentsFilingPM.Id,
             null, null,
              null, false);
            }
            else
            {
                RequestInProgressList = customsRequestsSheetQS
                   .GetRequestInProgress(tenant, this.MainInterfaceCode,
                   objectTableId, documentsFilingPM.EntityId,
                   objectTableDocumentsFilingId, documentsFilingPM.Id, null, true);

            }
            if (RequestInProgressList != null && RequestInProgressList.Count > 0)
            {
                LogitudeSettings.HandleLogMe("cresteCRS - קיים מסר זהה בתהליך", false, "sendOcrDocument", stopLogAt);

                LogMessagingUtil.Instance.AppendLine("קיים מסר זהה בתהליך");
                ///throw new System.Exception("Requestsheet  with Interface Type  = UCBUCBNDCD  already in progress  !!!");
                RequestInProgressListOut = string.Join(",", RequestInProgressList.Select(request => request.Id.ToString())); ;

                return "קיים מסר זהה בתהליך";

            }




            var RequestInProgressList2715 = customsRequestsSheetQS
                  .GetRequestInProgress(tenant, "2715",
                  null, null,
                  objectTableDocumentsFilingId, documentsFilingPM.Id, null, true);
            if (RequestInProgressList2715 != null && RequestInProgressList2715.Count > 0)
            {
                LogitudeSettings.HandleLogMe("cresteCRS -2715 קיים מסר זהה בתהליך", false, "sendOcrDocument", stopLogAt);

                LogMessagingUtil.Instance.AppendLine("2715 קיים מסר זהה בתהליך");
                RequestInProgressListOut = string.Join(",", RequestInProgressList2715.Select(request => request.Id.ToString())); ;

                return " 2715 קיים מסר זהה בתהליך";
            }


            LogMessagingUtil.Instance.AppendLine("Build !!!Requestsheet  with Interface Type  = UCBUCBNDCD  !!!");

            CustomsDocumentQueryService customsDocumentQueryService = new CustomsDocumentQueryService(documentsFilingPM.Tenant);
            var customDoc = customsDocumentQueryService.GetSingle(documentsFilingPM.Id, false, false);

            if (customDoc != null && !string.IsNullOrEmpty(customDoc.CustomsDocId))
            {
                LogitudeSettings.HandleLogMe("cresteCRS - קיים סימוכין מכס", false, "sendOcrDocument", stopLogAt);

                LogMessagingUtil.Instance.AppendLine("קיים סימוכין מכס");
                RequestInProgressListOut = "";
                return "קיים סימוכין מכס";

            }

            string uniComm = null;
            string fileName = null;
            var transmitionDateTime = DateTime.Now;
            string xmlESBResponseXmlClass = null;

            var myDCAInUCSBondedWithResponseContentHeader = new DCAInUCSBondedWithResponseContentHeader()
            {
                DeclarationId = documentsFilingPM.IsNotCustomsDocId ? documentsFilingPM.DeclarationId : documentsFilingPM.EntityId,
                DocumentsFilingId = documentsFilingPM.Id,
                CustomsDoucumentTypeCode = CustomsDoucumentTypeCode,
                LoggingUserId = documentsFilingPM.IsNotCustomsDocId ? documentsFilingPM.LoggedUserId : LoggingUserId,

                DocumentTypeCode = documentsFilingPM.DocumentTypeCode,
                LoggingEntityReference = documentsFilingPM.Code,
                tenant = tenant,
                MyMoreParams = "",
                ResponseContentHeader = new DefaultResponseContentHeader()
                {
                    TransmitionDateTime = transmitionDateTime
                },
                IsSendFromAutoClosing = documentsFilingPM.IsNotCustomsDocId,
            };



            var body = XmlGenericUtil<DCAInUCSBondedWithResponseContentHeader>.SerializeObject(myDCAInUCSBondedWithResponseContentHeader);
            //
            body = body.Substring(body.IndexOf(Environment.NewLine));
            var myESBResponseXmlClass = new ESBResponseXmlClass();
            var extrenalId = "62833ff7-1cd3-4faa-85a6-a4312ae4797a";
            extrenalId = uniComm ?? Guid.NewGuid().ToString();
            xmlESBResponseXmlClass = myESBResponseXmlClass.Get(Guid.NewGuid().ToString(), extrenalId, body);
            var transTime = "2016-04-19_13-35-13-481";

            transTime = transmitionDateTime.ToString("s").Replace("T", "_").Replace(":", "-");
            transTime += "-";
            transTime += transmitionDateTime.Millisecond.ToString();

            fileName = "DcaPrefixName.IL941079089." + transTime + "." + extrenalId + ".PLT.xml";
            //var messService = new Logitude.CustomsMessaging.MessagingServices.DF_MSG10000_ImportDeclarationMessagingService();
            //var responseData = messService.SendSheet(genericRequestParams);

            var ourRef = "";
            using (var trans = TransactionFactory.GetNewTransaction())
            {
                try
                {					

					_Stopwatch1 = Stopwatch.StartNew();


					var InterfaceManagementQS = new InterfaceManagementQueryService(tenant);
                    var InterfaceManagementPM = InterfaceManagementQS.GetSingleInterfaceManagementwithDefinition(
                        this.MainInterfaceCode, tenant);
                    fileName = fileName.Replace("DcaPrefixName.", InterfaceManagementPM.DcaPrefixName);
                    ourRef = this.DcaReceivedCustomResponseCorrelation(InterfaceManagementPM, tenant, new Customs.BL.Utils.DCAFileModel()
                    {
                        SelectedFileDownload = fileName,
                        TimStamp = transmitionDateTime

                    }, xmlESBResponseXmlClass);

					LogitudeSettings.HandleLogMe(Environment.NewLine + "2 Took: " + _Stopwatch1.Elapsed.ToString(), false, "CheckLogTime-SendMeces", stopLogAt); _Stopwatch1.Restart();


					trans.Complete();
                    LogitudeSettings.HandleLogMe("cresteCRS - המסר נבנה בהצלחה וישלח בתהליך רקע", false, "sendOcrDocument", stopLogAt);

                    LogMessagingUtil.Instance.AppendLine("המסר נבנה בהצלחה וישלח בתהליך רקע");
                    RequestInProgressListOut = "";
                    return "המסר נבנה בהצלחה וישלח בתהליך רקע";
                }
                catch (CustomsRequestsSheetDomainModelServiceException myCustomsRequestsSheetServiceException)
                {
                    if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.SameRequestInProgress)
                    {
                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine(" UCBUCBNDCD SameRequestInProgress!! " + myCustomsRequestsSheetServiceException.Message);


                    }
                    else if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.NoAvailableSignServer)
                    {
                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("UCBUCBNDCD SameRequestInProgress!!  " + myCustomsRequestsSheetServiceException.Message);
                    }
                    LogitudeSettings.HandleLogMe("cresteCRS - קיים מסר זהה בתהליך LINE 249", false, "sendOcrDocument", stopLogAt);
                    RequestInProgressListOut = myCustomsRequestsSheetServiceException.CustomsRequestsSheetId;
                    return "קיים מסר זהה בתהליך";
                    //throw;
                }
            }



        }


    }



    [XmlRoot(Namespace = "http://amital.com/customs/Prod/DCAInUCSBondedWithResponseContentHeader", IsNullable = false)]
    [XmlType(AnonymousType = true, Namespace = "http://amital.com/customs/Prod/DCAInUCSBondedWithResponseContentHeader")]
    public class DCAInUCSBondedWithResponseContentHeader : IINF_MSG_Generic
    {

        public IResponseContentHeader GetResponseContentHeader()
        {

            return ResponseContentHeader;
        }
        public Logitude.CustomsMessaging.Testers.Messages.DefaultResponseContentHeader ResponseContentHeader { get; set; }

        public int tenant { get; set; }
        public string LoggingUserId { get; set; }
        public string DeclarationId { get; set; }
        //public string master { get; set; }

        public LOGIDOCS MyLOGIDOCS { get; set; }

        public string MyMoreParams { get; set; }
        public string CustomsDoucumentTypeCode { get; set; }
        public string DocumentsFilingId { get; set; }
        public string DOCUMENTTYPEID { get; set; }
        public string DocumentTypeCode { get; set; }
        public string LoggingEntityReference { get; set; }
        public bool IsSendFromAutoClosing { get; set; }

        //public string DocumentTypeId { get; set; }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }


    public class SendBondedCustomDocumentService : ISendBondedCustomDocumentService
    {
        private DocumentsFilingPM _DocumentsFilingPM;
		Stopwatch _Stopwatch;

		public void
            JustDoIt(object documentsFilingPM)
        {

			DateTime stopLogAt = DateTime.MinValue;//DateTime stopLogAt = new DateTime(2020, 09, 01);
            string UntilDateyyyyMMdd = ConfigurationManager.AppSettings["20220227T155633.LogUntilDateyyyyMMdd"];
            if (!string.IsNullOrWhiteSpace(UntilDateyyyyMMdd))
            {
                stopLogAt = DateTime.ParseExact(UntilDateyyyyMMdd,
                                                    "yyyyMMdd",
                                                    CultureInfo.InvariantCulture,
                                                    DateTimeStyles.None);
            }
           
			Debug.WriteLine("SendBondedCustomDocument");
            string logData = "";
            try
            {

                //var mappingService = new DocumentsFilingQueryService(tenant);
                //var entity = mappingService.GetDocumentsFilingById(DocumentsFilingId, tenant);
                //_DocumentsFilingPM = mappingService.DocumentsFilingCustomDataMappingAndValidating(entity, tenant, false);


                _DocumentsFilingPM = documentsFilingPM as DocumentsFilingPM;
                if (!IsBonded(stopLogAt))
                {
                    return;
                }

                var DocumentsMetaDataTypeRepo = new DocumentsMetaDataTypeRepository(_DocumentsFilingPM.Tenant);
                var ENDOC = DocumentsMetaDataTypeRepo.GetSingleDocumentsMetaDataTypeByCode("ENDOC", _DocumentsFilingPM.Tenant);
                var myDocumentsFilingMetaDataValueReferenceAsDocType = "";
                if (!(CheckIsSendByDocType(logData) || _DocumentsFilingPM.IsNotCustomsDocId) && string.IsNullOrEmpty(_DocumentsFilingPM.OcrStatusCode))
                {
                    if (ENDOC == null)
                    {
                        LogitudeSettings.HandleLogMe("ENDOC not exist in DocumentsMetaDataType", false, "SendBondedCustomDocument", stopLogAt);
                        Debug.WriteLine("_DocumentsFilingPM == null");
                        return;

                    }
                    var myDocumentsFilingMetaDataValue = _DocumentsFilingPM.DocumentsFilingMetaDataValues
                        .FirstOrDefault(r => r.DocumentsMetaDataTypeCode == "ENDOC" || r.DocumentsMetaDataTypeId == ENDOC.Id);
                    if (myDocumentsFilingMetaDataValue == null)
                    {
                        LogitudeSettings.HandleLogMe("ENDOC not exist in DocumentsFilingMetaDataValues", false, "SendBondedCustomDocument", stopLogAt);
                        Debug.WriteLine("ENDOC not exist in DocumentsFilingMetaDataValues");
                        return;

                    }

                    myDocumentsFilingMetaDataValueReferenceAsDocType = myDocumentsFilingMetaDataValue.MetaDataValue;// <MetaDataValue>380</MetaDataValue>

                    if (!HaveTransDocumentTypeCode(myDocumentsFilingMetaDataValueReferenceAsDocType, stopLogAt))
                    {
                        return;
                    }
                }
                if (!String.IsNullOrWhiteSpace(logData))
                {
                    LogitudeSettings.HandleLogMe(logData, false, "after checks", stopLogAt);
                }
                CustomsDocumentPM customsDocumentPM;
                if (!IscustomsDocumentSent(stopLogAt, out customsDocumentPM))
                {
                    return;
                }

                Debug.WriteLine("Create.....");

                string loggingUserId = AuthenticationUtil.ResolveUserId(_DocumentsFilingPM.Tenant);
                bool interactive = false;
                if (interactive)
                {


                    (new Send2715Bonded()).Send(
                        _DocumentsFilingPM.Tenant,
                        _DocumentsFilingPM.Id, customsDocumentPM,
                        myDocumentsFilingMetaDataValueReferenceAsDocType);
                }
                else
                {
                    LogitudeSettings.HandleLogMe("!interactive before cresteCRS", false, "sendOcrDocument", stopLogAt);
                    LogitudeSettings.HandleLogMe("_DocumentsFilingPM.IsNotCustomsDocId" + _DocumentsFilingPM.IsNotCustomsDocId, false, "sendClosing", stopLogAt);

                    string key = ProcessLockTableUtil.Instance.GetKey4UCBUD2LT(_DocumentsFilingPM.Id, _DocumentsFilingPM.Tenant);
                    using (var disposableToken =
                        ProcessLockTableUtil.Instance.GetProcessLockTableDisposable(_DocumentsFilingPM.Tenant, true, key,
                        "UCBNDCD.CRS", true)
                        )
                    {
						_Stopwatch = Stopwatch.StartNew();
						
						var myDCAInUCBUD2LT_MsgMessagingService = new DCAInUCSBondedDocument_MessagingService();
                        string RequestInProgressList;
                        string crs = myDCAInUCBUD2LT_MsgMessagingService.CreateCRS(
                            _DocumentsFilingPM.Tenant,
                            loggingUserId,
                            _DocumentsFilingPM,
                            myDocumentsFilingMetaDataValueReferenceAsDocType, out RequestInProgressList);
                        logData = LogMessagingUtil.Instance.ToString();
                        LogitudeSettings.HandleLogMe("after cresteCRS", false, "sendOcrDocument", stopLogAt);

                        LogitudeSettings.HandleLogMe(crs + " " + logData + _DocumentsFilingPM.Code, false, "CreateUCBNDCDService.OK", stopLogAt);



					}
                }

            }
            catch (Exception E)
            {
                logData = LogMessagingUtil.Instance.ToString();
                LogitudeSettings.HandleLogMe(E.ToString() + logData + _DocumentsFilingPM.Code, true, "SendBondedCustomDocument", stopLogAt);
                LogitudeSettings.HandleLogMe(E.ToString() + logData + _DocumentsFilingPM.Code, true, "sendOcrDocumentError", stopLogAt);
				LogitudeSettings.HandleLogMe(Environment.NewLine + "1 Took: " + _Stopwatch.Elapsed.ToString(), false, "CheckLogTime-SendMeces", stopLogAt); _Stopwatch.Restart();

				throw;
            }
            finally
            {

			}

		}

        private bool CheckIsSendByDocType(string logData)
        {

            DateTime stopLogAt = DateTime.MinValue;//DateTime stopLogAt = new DateTime(2020, 09, 01);
            string UntilDateyyyyMMdd = ConfigurationManager.AppSettings["20222010T095633.LogUntilDateyyyyMMdd"];
            if (!string.IsNullOrWhiteSpace(UntilDateyyyyMMdd))
            {
                stopLogAt = DateTime.ParseExact(UntilDateyyyyMMdd,
                                                    "yyyyMMdd",
                                                    CultureInfo.InvariantCulture,
                                                    DateTimeStyles.None);
            }

            LogitudeSettings.HandleLogMe("CheckIsSendByDocTypeBonded  ", false, "SendBondedCustomDocument", stopLogAt);


            bool AutoSending = false;
            string CustomsDocumentUpload = "";
            try
            {
                DocumentTypeQueryService documentTypeQueryService = new DocumentTypeQueryService(_DocumentsFilingPM.Tenant);
                DocumentTypePM documentTypePM = documentTypeQueryService.GetDocumentTypeCodeById(_DocumentsFilingPM.DocumentTypeId, _DocumentsFilingPM.Tenant);
                if (_DocumentsFilingPM.ExternalEntityName == "EFIFILEM")
                {
                    var declarationQueryService = new Logitude.Customs.BL.EntityQueryServices.DeclarationQueryService(_DocumentsFilingPM.Tenant);
                    DeclarationPM declartionPM = declarationQueryService.GetDeclarationByExportFile(_DocumentsFilingPM.Tenant, _DocumentsFilingPM.ExternalEntityReference);
                }
                if (documentTypePM != null && !String.IsNullOrWhiteSpace(documentTypePM.Code))
                {
                    LogitudeSettings.HandleLogMe("  if (documentTypePM != null && !String.IsNullOrWhiteSpace(documentTypePM.Code))" + documentTypePM?.Code, false, "SendBondedCustomDocument", stopLogAt);
                    DocumentTypeCustomsDataQueryService documentTypeCustomsDataQueryService = new DocumentTypeCustomsDataQueryService(_DocumentsFilingPM.Tenant);
                    DocumentTypeCustomsDataPM documentTypeCustomsDataPM = documentTypeCustomsDataQueryService.GetSingle(documentTypePM.Code, false, true);

                    if (documentTypeCustomsDataPM != null && !String.IsNullOrWhiteSpace(documentTypeCustomsDataPM.CustomsDoucumentTypeCode))
                    {

                        LogitudeSettings.HandleLogMe("   if (documentTypeCustomsDataPM != null && !String.IsNullOrWhiteSpace(documentTypeCustomsDataPM.CustomsDoucumentTypeCode))" + documentTypeCustomsDataPM?.CustomsDoucumentTypeCode, false, "SendBondedCustomDocument", stopLogAt);

                        CustomDocumentTypeQueryService customDocumentTypeQueryService = new CustomDocumentTypeQueryService(_DocumentsFilingPM.Tenant);
                        CustomDocumentTypePM customDocumentTypePM = customDocumentTypeQueryService.GetSingleCustomDocumentTypeWithTenant(documentTypeCustomsDataPM.CustomsDoucumentTypeCode, _DocumentsFilingPM.Tenant);

                        if (customDocumentTypePM != null && !String.IsNullOrEmpty(customDocumentTypePM.CustomsDocumentUpload))
                        {

                            LogitudeSettings.HandleLogMe("  if (customDocumentTypePM != null && !String.IsNullOrEmpty(customDocumentTypePM.CustomsDocumentUpload))" + customDocumentTypePM?.CustomsDocumentUpload, false, "SendBondedCustomDocument", stopLogAt);

                            CustomsDocumentUpload = customDocumentTypePM.CustomsDocumentUpload;


                            if (customDocumentTypePM.CustomsDocumentUpload == "U" || customDocumentTypePM.CustomsDocumentUpload == "C")

                            {

                                LogitudeSettings.HandleLogMe("   if (customDocumentTypePM.CustomsDocumentUpload == U || customDocumentTypePM.CustomsDocumentUpload == C)  " + customDocumentTypePM?.CustomsDocumentUpload, false, "SendBondedCustomDocument", stopLogAt);

                                AutoSending = true;

                            }
                        }
                    }
                }
                if (declartionPM != null)
                {
                    if (declartionPM.Direction == "E" && declartionPM.IsDiamondDeclaration && declartionPM.AutoSending)
                    {
                        LogitudeSettings.HandleLogMe("  declartionPM.Direction == E && declartionPM.IsDiamondDeclaration && declartionPM.AutoSending  " + declartionPM?.Id, false, "SendBondedCustomDocument", stopLogAt);
                        AutoSending = true;

                    }
                   
                }

            }
            catch (Exception ee)
            {
                logData += $"CheckIsSendByDocType:error:{ee.Message}";
            }
            finally
            {
                logData += $"CheckIsSendByDocType:CustomsDocumentUpload:{CustomsDocumentUpload}";
            }
            return AutoSending;
        }

        private bool HaveTransDocumentTypeCode(string myDocumentsFilingMetaDataValueReferenceAsDocType, DateTime stopLogAt)
        {

            var myCustomDocumentTypeQueryService = new CustomDocumentTypeQueryService(_DocumentsFilingPM.Tenant);
            var myCustomDocumentTypePM = myCustomDocumentTypeQueryService.GetSingleCustomDocumentTypeWithTenant(myDocumentsFilingMetaDataValueReferenceAsDocType, _DocumentsFilingPM.Tenant);
            if (myCustomDocumentTypePM == null)
            {
                LogitudeSettings.HandleLogMe("myDocumentsFilingMetaDataValueReferenceAsDocType " + myDocumentsFilingMetaDataValueReferenceAsDocType + " but not found" + _DocumentsFilingPM.Code, false, "SendBondedCustomDocument", stopLogAt);
                //return;
                return false;

            }
            else
            {

                return true;
            }

            //var myDocumentTypeCustomsDataQueryService = new DocumentTypeCustomsDataQueryService(_DocumentsFilingPM.Tenant);
            //CustomsDoucumentTypeCode = myDocumentTypeCustomsDataQueryService.GetSingle(this._DocumentsFilingPM.DocumentTypeCode, true, true);
            //if (CustomsDoucumentTypeCode == null || String.IsNullOrWhiteSpace(CustomsDoucumentTypeCode.CustomsDoucumentTypeCode))
            //{


            //    LogitudeSettings.HandleLogMe("HaveTransDocumentTypeCode():DocumentTypeCustomsData  " + this._DocumentsFilingPM.DocumentTypeCode + " but not found" + _DocumentsFilingPM.Code, false, "SendBondedCustomDocument", stopLogAt);
            //    return false;
            //}
            //return true;
        }

        private bool IscustomsDocumentSent(DateTime stopLogAt, out CustomsDocumentPM customsDocumentPM)
        {
            ICustomContext dbContext = CustomContext.GetContext(_DocumentsFilingPM.Tenant);
            var myCustomsDocumentQueryService = new CustomsDocumentQueryService(dbContext);
            customsDocumentPM = myCustomsDocumentQueryService.GetSingle(this._DocumentsFilingPM.Id, true, false);
            if (customsDocumentPM != null)
            {
                if (!String.IsNullOrWhiteSpace(customsDocumentPM.CustomsDocId))
                {
                    LogitudeSettings.HandleLogMe("IscustomsDocumentSent(): myCustomsDocument already send !!" + _DocumentsFilingPM.Code, false, "SendBondedCustomDocument", stopLogAt);
                    Debug.WriteLine("myCustomsDocument already send !!");
                    return false;
                }
                //if (customsDocumentPM.DocumentStatusCode != "2")
                //{
                //    LogitudeSettings.HandleLogMe(" myCustomsDocumentPM.DocumentStatusCode!=2 !!", false, "SendBondedCustomDocument", stopLogAt);
                //    Debug.WriteLine("myCustomsDocumentPM.DocumentStatusCode!=2 !!");
                //    return false;
                //}
            }
            return true;
        }

        private bool IsBonded(DateTime stopLogAt)
        {
            if (_DocumentsFilingPM == null)
            {
                LogitudeSettings.HandleLogMe("_DocumentsFilingPM == null", false, "SendBondedCustomDocument", stopLogAt);
                Debug.WriteLine("_DocumentsFilingPM == null");
                return false;
            }

            //if (!_DocumentsFilingPM.DocumentsFilingMetaDataValues.Any(r => r.DocumentsMetaDataTypeCode == "ENDOC"))
            //{

            //    LogitudeSettings.HandleLogMe(" refernce where DocumentsMetaDataTypeCode is ENDOC not found "+_DocumentsFilingPM.Code, false, "SendBondedCustomDocument", stopLogAt);
            //    Debug.WriteLine("refernce where DocumentsMetaDataTypeCode is ENDOC not found ");
            //    return false;
            //}
            return true;
        }








    }

    class Send2715Bonded
    {
        public void Send(
            int Tenant,
            string DocumentsFilingPMId,
           CustomsDocumentPM customsDocumentPM,

           string CustomsDoucumentTypeCode,
           bool IsSendFromAutoClosing = false, string LogingUserId = "")
        {
            if (customsDocumentPM == null)
            {
                customsDocumentPM = new CustomsDocumentPM();
                customsDocumentPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                //customsDocumentPM.IsSendToQueue = true;



            }
            else
            {
                //myCustomsDocumentPM = myDocumentId;
                customsDocumentPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

            }
            customsDocumentPM.DocumentsFilingId = DocumentsFilingPMId;
            ICustomContext dbContext = CustomContext.GetContext(Tenant);
            var myDocumentTypeCustomsDatatQueryService = new DocumentTypeCustomsDataQueryService(dbContext);
            var DocumentTypeCustomsDatat = myDocumentTypeCustomsDatatQueryService.GetSingle(CustomsDoucumentTypeCode, true, false);
            customsDocumentPM.DocumentTypeCode = DocumentTypeCustomsDatat?.CustomsDoucumentTypeCode;//_DocumentsFilingPM.DocumentTypeCode;
                                                                                                    //myCustomsDocumentPM.CurrentCustomsDocumentsTicketId = customsDocumentsTicketPM.Id;
            customsDocumentPM.Tenant = Tenant;


            //try
            {
                //using (var scopeNewCRS = TransactionFactory.GetNewTransaction())
                {
                    var context1 = CustomContext.GetContext(Tenant);//context each CRS TRANS
                    var myCustomsDocumentUpdateService = new CustomsDocumentUpdateService(context1, new Dictionary<string, IContext>(), customsDocumentPM.Tenant);
                    myCustomsDocumentUpdateService.LoginUserId = LogingUserId;
                    myCustomsDocumentUpdateService.IsSendFromAutoClosing = IsSendFromAutoClosing;

                    customsDocumentPM.IsSendToQueue = false;
                    myCustomsDocumentUpdateService.AddPerfectCustomsDocumentMetaDataValues(customsDocumentPM);
                    myCustomsDocumentUpdateService.Update(customsDocumentPM, true);

                    customsDocumentPM.ChangeSetOp = ChangeSetOperation.Update;
                    customsDocumentPM.CustomsDocumentMetaDataValues.ForEach(r =>
                    {
                        r.ChangeSetOp = ChangeSetOperation.None;
                    });
                    customsDocumentPM.IsSendToQueue = true;
                    myCustomsDocumentUpdateService.IgnoreSendFailure = true;
                    myCustomsDocumentUpdateService.Update(customsDocumentPM, true);
                    LogMessagingUtil.Instance.AppendLine($" CreateSheetSBQMessage({DocumentsFilingPMId})");

                    // scopeNewCRS.Complete();
                }

            }
            //catch (System.Exception ee1)
            //{
            //    LogitudeSettings.HandleLogMe($"Exception!!!CreateSheetSBQMessage({_DocumentsFilingPM.Id}) : {ee1.ToString() }", true, "SendBondedCustomDocument", stopLogAt);

            //}

            //return customsDocumentPM;
        }

    }
}
