
using Logitude.AmitalMessaging.Utils;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Def.EntityQueryServicesExt;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using Logitude.CustomsMessaging.Testers.Messages;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Utils;
using RabbitMQ.Client;
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
    public class DCAInUCBUD2LT_MsgMessagingService
        : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        SYSTBL_NG_9000_MSG_SystemTableRequest,
        DCAInUCBUD2LTWithResponseContentHeader,
        DCAInCustomReturnNullRequestService,
        UniCourierBatchSendUCBUD2LT_MsgResponseService, RequestHeader>

    {
        public override string MainInterfaceCode
        {
            get { return "UCBUD2LT"; }
        }

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(DCAInUCBUD2LTWithResponseContentHeader customsResponse)
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


                LoggingObjectTableId = objectTableId,
                LoggingEntityId = customsResponse.DeclarationId,
                LoggingObjectTableId2 = objectTableId2,
                LoggingEntityId2 = customsResponse.DocumentsFilingId,

                LoggingUserId = customsResponse.LoggingUserId,
                RequestName = $" UD2LT   קישור מסמך לטיקט" + customsResponse.DocumentsFilingCode + " "
            };

            return genericRequestParams;
        }

        protected override DCAInUCBUD2LTWithResponseContentHeader CallWS(SYSTBL_NG_9000_MSG_SystemTableRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }






        public string CreateCRS(int tenant, string LoggingUserId,
            //string DeclarationId, string master,string courierDeclarationStatusCode, List<string> DeclarationsList = null)
            DocumentsFilingPM documentsFilingPM/*, DeclarationPM declarationPM*/ ,string entityId)
        {

            var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            var objectTableDocumentsFilingId = ObjectTableRepository.GetObjectTableByName("DocumentsFiling");
            var customsRequestsSheetQS = new CustomsRequestsSheetQueryService(tenant);
            //bool simultaneousCheckGeneralLock = true;
            //if (simultaneousCheckGeneralLock)
            //{

            //}
            //else
            {
                var RequestInProgressList = customsRequestsSheetQS.GetRequestInProgress(tenant, this.MainInterfaceCode,
                    objectTableId, documentsFilingPM.EntityId,
                    objectTableDocumentsFilingId, documentsFilingPM.Id, null, true);
                if (RequestInProgressList != null && RequestInProgressList.Count > 0)
                {

                    ///throw new System.Exception("Requestsheet  with Interface Type  = UCBUD2LT  already in progress  !!!");
                    return "קיים מסר זהה בתהליך";

                }
            }
            LogMessagingUtil.Instance.AppendLine("Build !!!Requestsheet  with Interface Type  = UCBUD2LT  !!!");



            string uniComm = null;
            string fileName = null;
            var transmitionDateTime = DateTime.Now;
            string xmlESBResponseXmlClass = null;

            var myDCAInUCBUD2LTWithResponseContentHeader = new DCAInUCBUD2LTWithResponseContentHeader()
            {
                DeclarationId = !string.IsNullOrEmpty(documentsFilingPM.EntityId)? documentsFilingPM.EntityId : entityId,
                DocumentsFilingId = documentsFilingPM.Id,
                DocumentsFilingCode = documentsFilingPM.Code,
                LoggingUserId = LoggingUserId,
                //DocumentTypeId=documentsFilingPM.DocumentTypeId,
                DocumentTypeCode = documentsFilingPM.DocumentTypeCode,

                tenant = tenant,
                MyMoreParams = "",
                ResponseContentHeader = new DefaultResponseContentHeader()
                {
                    TransmitionDateTime = transmitionDateTime
                },
            };



            var body = XmlGenericUtil<DCAInUCBUD2LTWithResponseContentHeader>.SerializeObject(myDCAInUCBUD2LTWithResponseContentHeader);
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



                    var InterfaceManagementQS = new InterfaceManagementQueryService(tenant);
                    var InterfaceManagementPM = InterfaceManagementQS.GetSingleInterfaceManagementwithDefinition(
                        this.MainInterfaceCode, tenant);
                    fileName = fileName.Replace("DcaPrefixName.", InterfaceManagementPM.DcaPrefixName);
                    //ourRef = this.DcaReceivedCustomResponseCorrelation(InterfaceManagementPM, tenant, new Customs.BL.Utils.DCAFileModel()
                    //{
                    //    SelectedFileDownload = fileName,
                    //    TimStamp = transmitionDateTime

                    //}, xmlESBResponseXmlClass);

                    var factory = new ConnectionFactory() { HostName = "unimq", UserName = "v5101", Password = "Aa123" };
                    using (var connection = factory.CreateConnection())
                    using (var channel = connection.CreateModel())
                    {

                        channel.QueueDeclare(queue: "connectToTicket",
                                             durable: false,
                                             exclusive: false,
                                             autoDelete: false,
                                             arguments: null);

                        string message = xmlESBResponseXmlClass;
                        var body2 = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: "",
                                             routingKey: "connectToTicket",
                                             basicProperties: null,
                                             body: body2);

                        Console.WriteLine(" [x] Sent {0}", message);
                    }

                    trans.Complete();
                    return "המסר נבנה בהצלחה וישלח בתהליך רקע";
                }
                catch (CustomsRequestsSheetDomainModelServiceException myCustomsRequestsSheetServiceException)
                {
                    if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.SameRequestInProgress)
                    {
                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine(" UCBUD2LT SameRequestInProgress!! " + myCustomsRequestsSheetServiceException.Message);


                    }
                    else if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.NoAvailableSignServer)
                    {
                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("UCBUD2LT SameRequestInProgress!!  " + myCustomsRequestsSheetServiceException.Message);
                    }
                    return "קיים מסר זהה בתהליך";
                    //throw;
                }
            }



        }


    }



    [XmlRoot(Namespace = "http://amital.com/customs/Prod/DCAInUCBUD2LTWithResponseContentHeader", IsNullable = false)]
    [XmlType(AnonymousType = true, Namespace = "http://amital.com/customs/Prod/DCAInUCBUD2LTWithResponseContentHeader")]
    public class DCAInUCBUD2LTWithResponseContentHeader : IINF_MSG_Generic
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
        public string DocumentsFilingCode { get; set; }
        public string DocumentsFilingId { get; set; }
        public string DOCUMENTTYPEID { get; set; }
        public string DocumentTypeCode { get; set; }

        //public string DocumentTypeId { get; set; }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }



    public class CreateUD2LTService : ICreateUD2LTService
    {
        private DocumentsFilingPM _DocumentsFilingPM;
        
        public void //JustDoIt(string DocumentsFilingId, int tenant)
            JustDoIt(object documentsFilingPM)
        {
            DateTime stopLogAt = DateTime.MinValue; //new DateTime(2022, 01, 01);
            string UntilDateyyyyMMdd = ConfigurationManager.AppSettings["20220412HD367591.LogUntilDateyyyyMMdd"];
            if (!string.IsNullOrWhiteSpace(UntilDateyyyyMMdd))
            {
                stopLogAt = DateTime.ParseExact(UntilDateyyyyMMdd,
                                                    "yyyyMMdd",
                                                    CultureInfo.InvariantCulture,
                                                    DateTimeStyles.None);
            }

            
            DeclarationPM declarationPM;
            Debug.WriteLine("CreateUD2LTService");
            string logData = "";
            try
            {

                //var mappingService = new DocumentsFilingQueryService(tenant);
                //var entity = mappingService.GetDocumentsFilingById(DocumentsFilingId, tenant);
                //_DocumentsFilingPM = mappingService.DocumentsFilingCustomDataMappingAndValidating(entity, tenant, false);


                _DocumentsFilingPM = documentsFilingPM as DocumentsFilingPM;

                if (_DocumentsFilingPM == null)
                {
                    LogitudeSettings.HandleLogMe("_DocumentsFilingPM == null", false, "CreateUD2LTService", stopLogAt);
                    Debug.WriteLine("_DocumentsFilingPM == null");
                    return;
                }
                logData = $"DocumentsFilingPM.Id={_DocumentsFilingPM.Id},Code={_DocumentsFilingPM.Code}"; //Logitude.Server.Tools.Utils.ProxyUtil.JsonConvertSerialize(_DocumentsFilingPM);

                if (String.IsNullOrWhiteSpace(_DocumentsFilingPM.DocumentTypeCode))
                {
                    FixDocumentTypeCodeEmpty(logData);//hd367591
                }


                if (String.IsNullOrWhiteSpace(_DocumentsFilingPM.DocumentTypeCode))
                {
                    LogitudeSettings.HandleLogMe("_DocumentsFilingPM.DocumentTypeCode" + logData, false, "CreateUD2LTService.DOC_ID", stopLogAt);
                    Debug.WriteLine("CreateUD2LTService.DOC_ID== null");
                    return;
                }

                int tenant = _DocumentsFilingPM.Tenant;
                if (!IsConnected2Declaration())
                {
                    LogitudeSettings.HandleLogMe("!IsConnected2Decalaration()" + logData, false, "CreateUD2LTService", stopLogAt);
                    Debug.WriteLine("!IsConnected2Decalaration()");
                    return;
                }

                bool shouldCreateDCAComm = false;
                var decQS = new DeclarationQueryService(tenant);
                if (!string.IsNullOrEmpty(this._DocumentsFilingPM.EntityId))
                {
                    declarationPM = decQS.GetSingle(this._DocumentsFilingPM.EntityId, false, false);

                }
                else
                {
                declarationPM = decQS.GetSingleByCustomFileNo(this._DocumentsFilingPM.ExternalEntityReference, _DocumentsFilingPM.Tenant);

                }



                logData += $"declarationPM.id={declarationPM.Id},CustomFileNo={declarationPM.CustomFileNo}"; //Logitude.Server.Tools.Utils.ProxyUtil.JsonConvertSerialize(declarationPM);
                if (declarationPM.PaymentDate.HasValue)
                {
                    shouldCreateDCAComm = false;
                    LogitudeSettings.HandleLogMe("declarationPM.PaymentDate.HasValue" + logData, false, "CreateUD2LTService", stopLogAt);
                    Debug.WriteLine("Declaration has already been payed");
                    return;
                }
                if (/*CourierENV() */ declarationPM.IsCourierDeclaration)
                {
                    Debug.WriteLine("CourierENV");

                    shouldCreateDCAComm = true;
                }
                else
                {
                    if (declarationPM.IsDiamondDeclaration)
                    {
                        //CGG_DEC_DOC_CLT
                        var amitalContext = AmitalContext.GetContext(tenant);
                        var myGDFDATAQueryService = new GDFDATAQueryService(amitalContext);
                        var def = myGDFDATAQueryService.GetSingle("ISRAEL", "CGG_DEC_DOC_CLT", "NON", "NON", false, true);
                        def.DEFDATA = def.DEFDATA ?? "";
                        if (!String.IsNullOrWhiteSpace(def.DEFDATA))
                        {
                            var listStorageDefault = new List<string>();//&& declaration.Consignments.FirstOrDefault().StorageSiteCode == "ILOVL"

                            if (!String.IsNullOrWhiteSpace(declarationPM.CustomerCode) && def.DEFDATA.Contains(declarationPM.CustomerCode)) // Maman
                            {
                                Debug.WriteLine("def.DEFDATA.Contains(declarationPM.CustomerId)");
                                shouldCreateDCAComm = true;
                            }
                            else
                            {
                                LogitudeSettings.HandleLogMe("default CGG_DEC_DOC_CLT does not contains declarationPM.CustomerId " + declarationPM.CustomerId + logData, false, "CreateUD2LTService", stopLogAt);
                            }
                        }
                        else
                        {
                            LogitudeSettings.HandleLogMe("default CGG_DEC_DOC_CLT is empty " + logData, false, "CreateUD2LTService", stopLogAt);
                        }
                    }
                    else
                    {
                        LogitudeSettings.HandleLogMe("Not Diamond Declaration " + logData, false, "CreateUD2LTService", stopLogAt);
                    }

                }
                if (!shouldCreateDCAComm)
                {
                    LogitudeSettings.HandleLogMe("!shouldCreateDCAComm" + logData, false, "CreateUD2LTService", stopLogAt);
                    Debug.WriteLine("!shouldCreateDCAComm");
                    return;
                }

                if (TicketalreadyExistforthisDocument(declarationPM))
                {
                    LogitudeSettings.HandleLogMe("TicketalreadyExistforthisDocument()" + logData, false, "CreateUD2LTService", stopLogAt);
                    Debug.WriteLine("TicketalreadyExistforthisDocument");
                    return;

                }


                CustomsDocumentsTicketQueryService myCustomsDocumentsTicketQueryService = new CustomsDocumentsTicketQueryService(declarationPM.Tenant);
                List<CustomsDocumentsTicketPM> myCustomsDocumentsTicketPMList = myCustomsDocumentsTicketQueryService.GetCustomsDocumentsTicketsByDocumentsFilingId(_DocumentsFilingPM.Id, declarationPM.Tenant);
                if (myCustomsDocumentsTicketPMList != null && myCustomsDocumentsTicketPMList.Count() > 0)
                {
                    List<string> ticketdIds = myCustomsDocumentsTicketPMList.Select(r => r.Id).ToList();
                    if (ticketdIds != null && ticketdIds.Count() > 0)
                    {
                        CustomsDocumentPointerQueryService myCustomsDocumentPointerQueryService = new CustomsDocumentPointerQueryService(_DocumentsFilingPM.Tenant);
                        List<CustomsDocumentPointerPM> myCustomsDocumentPointerPMList = myCustomsDocumentPointerQueryService.GetPointersForMultipleTickets(ticketdIds, _DocumentsFilingPM.Tenant);
                        if (myCustomsDocumentPointerPMList != null && myCustomsDocumentPointerPMList.Count() > 0)
                        {
                            var myCustomsDocumentPointerPMListforDec = myCustomsDocumentPointerPMList.Where(o => o.ParentEntityCode == "Declaration" && o.ParentEntityId == declarationPM.Id);
                            if (myCustomsDocumentPointerPMListforDec != null && myCustomsDocumentPointerPMListforDec.Count() > 0)
                            {
                                LogitudeSettings.HandleLogMe("Ticket already Exist for this Document" + logData, false, "CreateUD2LTService", stopLogAt);
                                Debug.WriteLine("Ticket already Exist for this Document");
                                return;

                            }
                        }
                    }
                }



                Debug.WriteLine("CreateCRS");

                string loggingUserId = AuthenticationUtil.ResolveUserId(tenant);


                string key = ProcessLockTableUtil.Instance.GetKey4UCBUD2LT(_DocumentsFilingPM.Id, _DocumentsFilingPM.Tenant);
                using (var disposableToken =
                    //ProcessLockTableUtil.Instance.LockItAndGetReleaseToken(key, "5117ResponseService.Update")
                    ProcessLockTableUtil.Instance.GetProcessLockTableDisposable(_DocumentsFilingPM.Tenant, true, key, "UCBUD2LT.CRS", true)
                    )
                {
                    var myDCAInUCBUD2LT_MsgMessagingService = new DCAInUCBUD2LT_MsgMessagingService();
                    string crs = myDCAInUCBUD2LT_MsgMessagingService.CreateCRS(tenant, loggingUserId, _DocumentsFilingPM, declarationPM.Id);
                    LogitudeSettings.HandleLogMe(crs + " " + logData, false, "CreateUD2LTService.OK", stopLogAt);

                }

            }
            catch (Exception E)
            {

                LogitudeSettings.HandleLogMe(E.ToString() + logData, true, "CreateUD2LTService", stopLogAt);
                throw;
            }
            finally
            {

            }

        }

        private void FixDocumentTypeCodeEmpty(string logData)
        {
            var codeStart = _DocumentsFilingPM.DocumentTypeCode;
            try
            {
                //_DocumentsFilingPM.DocumentTypeCode = codeStart ?? _DocumentsFilingPM.DocumentTypeId;
                if (String.IsNullOrWhiteSpace(_DocumentsFilingPM.DocumentTypeCode))
                {
                    if (!String.IsNullOrEmpty(_DocumentsFilingPM.DocumentTypeId))
                    {
                        var documentTypeRepository = new DocumentTypeRepository(_DocumentsFilingPM.Tenant);
                        var poco = documentTypeRepository.GetSingleDocumentType(_DocumentsFilingPM.DocumentTypeId, _DocumentsFilingPM.Tenant);
                        if (poco != null)
                        {
                            _DocumentsFilingPM.DocumentTypeCode = poco.Code;
                        }

                    }

                    //var documentsFilingRepository = new DocumentsFilingRepository();
                    //var pm = documentsFilingRepository.GetSingleDocumentsFiling(_DocumentsFilingPM.Id);
                    //if (pm != null)
                    //{
                    //    _DocumentsFilingPM.DocumentTypeCode = pm.DocumentType?.Code;
                    //}
                     
                }
            }
            catch (Exception ee)
            {
                logData += $"FixDocumentTypeCodeEmpty:error:{ee.Message}";
                ///throw;logData
            }
            finally
            {
                if(codeStart!= _DocumentsFilingPM.DocumentTypeCode)
                {
                    logData += $"FixDocumentTypeCodeEmpty:Change:{_DocumentsFilingPM.DocumentTypeCode}";
                }
            } 

            ////
        }

        private string GetCustomsFileImportType(DeclarationPM entityPM)
        {
            if (entityPM == null || string.IsNullOrWhiteSpace(entityPM.CustomFileNo)) return null;

            var openReaderSingleResult = new OpenReaderSingleResult(AmitalContext.GetContext(entityPM.Tenant));
            string UserId = openReaderSingleResult.GetSchemaUserId();
            string theResult = "";
            var res1 = openReaderSingleResult.ExecuteReaderSingleResult<int>(
                $"select IMPORT_TYPE from {UserId}.CFIFILEM where FILE_NO='{entityPM.CustomFileNo}'"
                ,
                (dataReader) =>
                {
                    theResult = dataReader.GetString(0);
                    return 9999;

                });

            return theResult;
        }



        private bool TicketalreadyExistforthisDocument(DeclarationPM declarationPM)
        {
            CustomsDocumentsTicketQueryService myCustomsDocumentsTicketQueryService = new CustomsDocumentsTicketQueryService(_DocumentsFilingPM.Tenant);
            List<CustomsDocumentsTicketPM> myCustomsDocumentsTicketPMList = myCustomsDocumentsTicketQueryService.GetCustomsDocumentsTicketsByDocumentsFilingId(_DocumentsFilingPM.Id, _DocumentsFilingPM.Tenant);
            if (myCustomsDocumentsTicketPMList != null && myCustomsDocumentsTicketPMList.Count() > 0)
            {
                List<string> ticketdIds = myCustomsDocumentsTicketPMList.Select(r => r.Id).ToList();
                if (ticketdIds != null && ticketdIds.Count() > 0)
                {
                    CustomsDocumentPointerQueryService myCustomsDocumentPointerQueryService = new CustomsDocumentPointerQueryService(_DocumentsFilingPM.Tenant);
                    List<CustomsDocumentPointerPM> myCustomsDocumentPointerPMList = myCustomsDocumentPointerQueryService.GetPointersForMultipleTickets(ticketdIds, _DocumentsFilingPM.Tenant);
                    if (myCustomsDocumentPointerPMList != null && myCustomsDocumentPointerPMList.Count() > 0)
                    {
                        var myCustomsDocumentPointerPMListforDec = myCustomsDocumentPointerPMList.Where(o => o.ParentEntityCode == "Declaration" && o.ParentEntityId == declarationPM.Id);
                        if (myCustomsDocumentPointerPMListforDec != null && myCustomsDocumentPointerPMListforDec.Count() > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            
            return false;
        }

        private bool CourierENV()
        {
            var qs = new CustomsSettingQueryService(_DocumentsFilingPM.Tenant);
            var pm = qs.GetSettingByTenantN(_DocumentsFilingPM.Tenant);
            return false;// pm.companytype=="B"
        }

        private bool IsConnected2Declaration()
        {
            return (this._DocumentsFilingPM.ObjectTableId == ObjectTableRepository.GetObjectTableByName("Customs.Declaration") &&  ( !String.IsNullOrWhiteSpace(this._DocumentsFilingPM.EntityId) || !String.IsNullOrWhiteSpace(this._DocumentsFilingPM.ExternalEntityReference)));
        }
    }
}
