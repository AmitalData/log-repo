using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using System.Data.Common;
using System.Data.SqlClient;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Customs.BL.TraceEvents;
using Logitude.AmitalMessaging.Infrastructure;

using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Helpers;
using Logitude.CustomsMessaging.Common.RequestParams;
using System;

using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.Customs.BL.Messaging.Amital.CustomFile;
using Logitude.Customs.BL.Models;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Server.Tools.Models;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Simplog.Server.Infrastructure.Helpers;
using System.Transactions;
//using Unifreight.BL.EntityQueryServices;
//using Unifreight.BL.EntityUpdateServices;
using Unifreight.BL.EntityPMs.UGenerated;
using Logitude.Customs.Def.Messaging.Customs;
using Simplog.Data.CommonDataModel;
//using Unifreight.BL.EntityPMs;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.Repsitories;
using Simplog.Data.Helpers;
using Logitude.Customs.BL.BL;
using UnifreightIIG.Common.ExportDeclarationAmendmentRequestMsgRequestServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class DF_MSG8235_ExportDeclarationAmendmentRequestService : RequestServiceBase<DF_NG_8235_MSG14000_ExportDeclarationAmendmentRequestMsg, GenericRequestParams>
    {
        private ICustomContext _context;
        private DeclarationPM _DeclarationPM;
        private DeclarationPM _DeclarationPMOrg;
        string functionalReferenceID = "";
        private Stopwatch _Stopwatch;
        private AmitalContext _AmitalContext;
         private string _userId;
        public override void ManipulateRequestParams(GenericRequestParams requestParams)
        {
            if (requestParams.RequestVIA == SendRequestVIA.DCABatch)
            {
                return;
            }
            int countItems = 0;
            int countSI = 0;
            int backgroundcountItems = 0;
            var fast = true;
            var sw = Stopwatch.StartNew();
            bool onlyAlwaysAccumulate = false;
            int existSupplierInvoiceItemsWithoutHash = 0;
            int existSupplierInvoiceItemsWithParent = 0;
            int SItoAccumulate = 0;
            try
            {
                _userId = requestParams.LoggingUserId;
                   var siqs = new SupplierInvoiceQueryService(requestParams.Tenant);
                SItoAccumulate = siqs.GetSupplierInvoiceToAccumulateCount(requestParams.Tenant, requestParams.AppicationId);
                var ssiqs = new SupplierInvoiceItemQueryService(requestParams.Tenant);
                bool noAccumulateForNow = true;//itzik +ihab 
                if (noAccumulateForNow)
                {
                    countSI = siqs.GetSupplierInvoiceCountForDeclaration(requestParams.AppicationId, requestParams.Tenant);
                    countItems = ssiqs.GetDeclarationCountOfSupplierInvoiceItems(requestParams.Tenant, requestParams.AppicationId,true);
                    LogMessagingUtil.Instance.AppendLine("GetDeclarationCountOfSupplierInvoiceItems: " + countItems.ToString());
                }
                else
                {
                    countItems = siqs.GetDeclarationCountOfSupplierInvoiceItemsForAccumulation(requestParams.Tenant, requestParams.AppicationId);
                    LogMessagingUtil.Instance.AppendLine("GetDeclarationCountOfSupplierInvoiceItemsForAccumulation: " + countItems.ToString());
                }
                
                backgroundcountItems = countItems;
                existSupplierInvoiceItemsWithParent = ssiqs.ExistSupplierInvoiceItemsWithParent(requestParams.Tenant, requestParams.AppicationId);
                if ((countItems > 100 || SItoAccumulate > 0) && existSupplierInvoiceItemsWithParent > 0)
                {
                    //existSupplierInvoiceItemsWithoutHash = qs.ExistSupplierInvoiceItemsWithoutHash(requestParams.Tenant, requestParams.AppicationId
                    if (countItems < 999 && SItoAccumulate > 0) onlyAlwaysAccumulate = true;
                    existSupplierInvoiceItemsWithoutHash = siqs.ExistSupplierInvoiceItemsWithoutHashForAccumulation(requestParams.Tenant, requestParams.AppicationId, onlyAlwaysAccumulate);
                    if (existSupplierInvoiceItemsWithParent > 0 && existSupplierInvoiceItemsWithoutHash < 1)
                    {
                        if (countItems > 998)
                        {
                            countItems = existSupplierInvoiceItemsWithParent;
                        }
                        if (backgroundcountItems > 100)
                        {
                            backgroundcountItems = existSupplierInvoiceItemsWithParent;
                        }
                    }
                }

#if false
1>                This take All SupplierInvoiceItems  include parent !!!

2>               **maybe** if >998 and all SIItems have  accurate itemHash 
                - Then not need to ReCalcAccumulation & to send InterActive 
                - Ask Yaron 
#endif

            }
            finally
            {
                LogMessagingUtil.Instance.AppendLine("LogitudeSettings.LogitudeURL = " + LogitudeSettings.LogitudeURL);
                LogMessagingUtil.Instance.AppendLine("ManipulateRequestParams:requestParams.RequestVIA = " + requestParams.RequestVIA.ToString());
                if (countItems > 998)
                {
                    if (LogitudeSettings.LogitudeURL.Contains("http://192.116.221.103/Oracle"))
                    {
                        requestParams.RequestVIA = SendRequestVIA.WebServiceBatch;
                    }
                    else
                    {
                        requestParams.RequestVIA = SendRequestVIA.DCABatch;
                    }

                    requestParams.RequestVIAChangeDue = ("הצהרה זו מכילה מעל 998 פרטי מכס ולכן תשלח לכספת");
                    LogMessagingUtil.Instance.AppendLine("ManipulateRequestParams:requestParams.RequestVIA = " + requestParams.RequestVIA.ToString());
                    LogMessagingUtil.Instance.AppendLine("הצהרה זו מכילה מעל 998 פרטי מכס ולכן תשלח לכספת");
                }
                else
                {
                    if (backgroundcountItems >= 100)
                    {
                        if (requestParams.RequestVIA == SendRequestVIA.DCABatch)
                        {
                            LogMessagingUtil.Instance.AppendLine("***User**** Send this request VIA DCABatch-- no need to change !!!");
                            LogMessagingUtil.Instance.AppendLine("הצהרה זו מכילה מעל 100 פרטי מכס ולכן תשודר ברקע");
                        }
                        else
                        {
                            requestParams.RequestVIA = SendRequestVIA.WebServiceBatch;
                            LogMessagingUtil.Instance.AppendLine("ManipulateRequestParams:requestParams.RequestVIA = SendRequestVIA.WebServiceBatch");
                            LogMessagingUtil.Instance.AppendLine("הצהרה זו מכילה מעל 100 פרטי מכס ולכן תשודר ברקע");
                            requestParams.RequestVIAChangeDue = ("הצהרה זו מכילה מעל 100 פרטי מכס ולכן תשודר ברקע");
                        }

                    }
                    if (
                        ( requestParams.RequestVIA == SendRequestVIA.WebServiceInteractive 
                        || requestParams.RequestVIA == SendRequestVIA.Default) 
                        &&  countSI > 15)
                    {
                        requestParams.RequestVIA = SendRequestVIA.WebServiceBatch;
                        LogMessagingUtil.Instance.AppendLine("ManipulateRequestParams:requestParams.RequestVIA = SendRequestVIA.WebServiceBatch");
                        LogMessagingUtil.Instance.AppendLine("הצהרה זו מכילה מעל 15 חן ספק ולכן תשודר ברקע");
                        requestParams.RequestVIAChangeDue = ("הצהרה זו מכילה מעל 15 חן ספק ולכן תשודר ברקע");


                    }
                    if (SItoAccumulate > 0)
                    {
                        requestParams.RequestVIAChangeDue = ("בהצהרה זו יש חשבון ספק שמסומן לצבירה ולכן ההצהרה תיצבר");
                        LogMessagingUtil.Instance.AppendLine("ManipulateRequestParams:requestParams.RequestVIA = " + requestParams.RequestVIA.ToString());
                        LogMessagingUtil.Instance.AppendLine("בהצהרה זו יש חשבון ספק שמסומן לצבירה ולכן ההצהרה תיצבר");
                    }
                }

                LogMessagingUtil.Instance.AppendLine("ManipulateRequestParams:requestParams.RequestVIA = " + requestParams.RequestVIA.ToString());
                LogMessagingUtil.Instance.AppendLine("ManipulateRequestParams:fast=" + fast.ToString() + ":Took:" + sw.ElapsedMilliseconds);
            }
        }

        private void CreateDeclarationPM(GenericRequestParams requestParams)
        {
            if (this._context == null) this._context = CustomContext.GetContext(requestParams.Tenant);
            var declarationQueryService = new DeclarationQueryService(_context);
            declarationQueryService.LoadSupplierInvoicesItemsParentsOnly = true;
            _DeclarationPM = declarationQueryService.GetSingle(requestParams.AppicationId, true, false);
            _DeclarationPMOrg = declarationQueryService.GetAcceptDeclarationAmendment(_DeclarationPM.AmendmentOriginalDeclartation, _DeclarationPM.Tenant);
        }


        public override void PostGetRequest(DF_NG_8235_MSG14000_ExportDeclarationAmendmentRequestMsg customRequest, GenericRequestParams requestParams)
        {
            if (this._context == null)
            {
                this._context = CustomContext.GetContext(requestParams.Tenant);
            }
        }
 
        public override DF_NG_8235_MSG14000_ExportDeclarationAmendmentRequestMsg GetRequest(GenericRequestParams requestParams)
        {

            

            LogMessagingUtil.Instance.AppendLine("GetRequest:requestParams.RequestVIA = " + requestParams.RequestVIA.ToString());
            LogMessagingUtil.Instance.AppendLine("GetRequest:requestParams.RequestVIAChangeDue = " + requestParams.RequestVIAChangeDue);
            if (((requestParams.RequestVIA == SendRequestVIA.DCABatch || requestParams.RequestVIA == SendRequestVIA.WebServiceBatch) &&
                    requestParams.RequestVIAChangeDue == ("הצהרה זו מכילה מעל 998 פרטי מכס ולכן תשלח לכספת")) || requestParams.RequestVIAChangeDue == ("בהצהרה זו יש חשבון ספק שמסומן לצבירה ולכן ההצהרה תיצבר"))
            {
                var mySIAccumulationUtil = new SIAccumulationUtil();
                bool onlyAlwaysAccumulate = false;
                mySIAccumulationUtil.SetParam(requestParams);
                if (requestParams.RequestVIAChangeDue == ("בהצהרה זו יש חשבון ספק שמסומן לצבירה ולכן ההצהרה תיצבר")) onlyAlwaysAccumulate = true;
                bool isAccurate = mySIAccumulationUtil.Fast_IsAllItemsHaveHash_IsAccurate(onlyAlwaysAccumulate);
                LogMessagingUtil.Instance.AppendLine(Environment.NewLine + "Is All Items Have Hash=" + isAccurate.ToString());
                
                if (!isAccurate)
                {
                    _Stopwatch = Stopwatch.StartNew();
                    using (var tran = TransactionFactory.GetNewTransaction())
                    {
                        mySIAccumulationUtil.FastDeleteAllParent(); 
                        tran.Complete();
                    }
                    LogMessagingUtil.Instance.AppendLine(Environment.NewLine + "Fast Delete All Parent Items Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                    using (var tran = TransactionFactory.GetNewTransaction())
                    {
                        mySIAccumulationUtil.Run();
                        this._DeclarationPM = mySIAccumulationUtil.GetDeclarationPM();
                        tran.Complete();
                    }
                    LogMessagingUtil.Instance.AppendLine(Environment.NewLine + "SI Accumulation Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                }

            }
            else
            {
                int countItems = 0;
                int existSupplierInvoiceItemsWithParent = 0;
                var qs = new SupplierInvoiceItemQueryService(requestParams.Tenant);
                 var siqs = new SupplierInvoiceQueryService(requestParams.Tenant);
                countItems = siqs.GetDeclarationCountOfSupplierInvoiceItemsForAccumulation(requestParams.Tenant, requestParams.AppicationId);

                existSupplierInvoiceItemsWithParent = qs.ExistSupplierInvoiceItemsWithParent(requestParams.Tenant, requestParams.AppicationId);
                if (existSupplierInvoiceItemsWithParent > 0 && countItems < 999)
                {
                    LogMessagingUtil.Instance.AppendLine(Environment.NewLine + "Less than 999 items(" + (countItems - existSupplierInvoiceItemsWithParent) + ") with accumulation - accumulation data will be cleared");
                    _Stopwatch = Stopwatch.StartNew();
                    var mySIAccumulationUtil = new SIAccumulationUtil();
                    mySIAccumulationUtil.SetParam(requestParams);
                    using (var tran = TransactionFactory.GetNewTransaction())
                    {
                        mySIAccumulationUtil.FastDeleteAllParent();
                        tran.Complete();
                    }
                    LogMessagingUtil.Instance.AppendLine(Environment.NewLine + "Fast Delete All Parent Items Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();

                    using (var tran = TransactionFactory.GetNewTransaction())
                    {
                        mySIAccumulationUtil.Run();
                        this._DeclarationPM = mySIAccumulationUtil.GetDeclarationPM();
                        tran.Complete();
                    }
                    LogMessagingUtil.Instance.AppendLine(Environment.NewLine + "SI Accumulation clearance Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                 }

             }

             bool fromMevaker = false;
            if (!string.IsNullOrWhiteSpace(requestParams.UnifreightListOnServerOnly))
            {
                var dic = UnifreightListsUtil.Deserialize(requestParams.UnifreightListOnServerOnly);
                fromMevaker = !String.IsNullOrWhiteSpace(UnifreightListsUtil.GetValue(ref dic, "FromMevaker"));
            }
            var req = new DF_NG_8235_MSG14000_ExportDeclarationAmendmentRequestMsg();
            CreateDeclarationPM(requestParams);
            var objectTableIdCourierMaster = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");
            if (requestParams.LoggingObjectTableId2 == objectTableIdCourierMaster || fromMevaker)
            {
                if (!fromMevaker)
                {
                    UCBatchCheckLock(requestParams, _DeclarationPM);
                }
                CheckTaxationDateTime(_DeclarationPM);
            }

            LogMessagingUtil.Instance.AppendLine("declaration retrieve from db");

             req.Response = new UnifreightIIG.Common.ExportDeclarationAmendmentRequestMsgRequestServiceReference.Response();
            req.Response.Declaration =  Getdeclaration(_DeclarationPM , _DeclarationPMOrg);

            req.Response.FunctionalReferenceID = new ResponseFunctionalReferenceIDType { Value = string.IsNullOrEmpty(_DeclarationPM.AmendmentRequestNumber) ? GetNextAmendmentRequestNumber() : _DeclarationPM.AmendmentRequestNumber
            };
            functionalReferenceID = req.Response.FunctionalReferenceID.Value;

            req.Response.IssueDateTime = DataTypeConvertorUtil.Convert(DateTime.Now);
            req.Response.AdditionalInformation = AdditionalInformation();
            if(_DeclarationPM.AmedmentType == "2")
            {
                req.Response.FunctionCode = new ResponseFunctionCodeType { Value = "3" };
            }
            else
            {
                req.Response.FunctionCode = new ResponseFunctionCodeType { Value = "1" };
            }
            //req.Attachments = GetAttachments();
            LogMessagingUtil.Instance.AppendLine("declaration build" + requestParams.AppicationId);
             UpdateDeclaration(req.Response, requestParams.LoggingUserId);
            _context = null;
            return req;
        }

        private Attachment[] GetAttachments()
        {
            List<Attachment> attachments = new List<Attachment>();



             var customsDocumentQueryService = new CustomsDocumentQueryService(_context);
            var customsDocumentPMList = customsDocumentQueryService.GetCustomsDocumentPMListWithoutRequestedDoc(new GetTicketsParams() { ParentEntityId = _DeclarationPM.Id, ParentEntityCode = "Declaration" }, _DeclarationPM.Tenant);

            foreach (var customsDocumentPM in customsDocumentPMList)
            {
                if (!string.IsNullOrWhiteSpace(customsDocumentPM.CustomsDocId))
                {
                    var attachment = new Attachment();
                    attachment.externalAttachmentID = customsDocumentPM.ExternalAttachmentId;
                    attachment.IsAttachment = "false";
                    attachment.keywords = customsDocumentPM.Name;
                    attachment.fileName = customsDocumentPM.Name;

                    attachment.documentType = customsDocumentPM.DocumentTypeCode;
                    attachments.Add(attachment);
                }
            }
            return attachments.ToArray();

        }

        private void UpdateDeclaration(UnifreightIIG.Common.ExportDeclarationAmendmentRequestMsgRequestServiceReference.Response response ,string LoggingUserId)
        {
            DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(_context, new Dictionary<string, IContext>(), _DeclarationPM.Tenant);
            _DeclarationPM.AmendmentissueDate = DateTime.Now;
            _DeclarationPM.AmendmentRequestNumber = response.FunctionalReferenceID.Value;
            _DeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
            //var myUpdateEventContextTagModel = new EventContextTagModel()
            //{
            //    CallProccessID = EventContextTagModel.ProccessEnum.None,
            //    EventCode = "DCH",
            //    EventRemarks = "Declaration Changed By Customs",
            //    FUStatusRemarks = "בוצע תיקון הצהרה" + _DeclarationPM.DeclarationNumber,
            //};


            //_DeclarationPM.CurrentContextTag = myUpdateEventContextTagModel;

            //EventTracer.CreateTraceEvent(new EventTracerArgs()
            //{
            //    Tenant = _DeclarationPM.Tenant,
            //    EventTypeCode = "DCH",
            //    UserId = _userId,
            //    EntityId = _DeclarationPMOrg.Id,
            //    ObjectTableName = "Customs.Declaration",
            //    Notes = "-  בוצע תיקון הצהרה" + (_DeclarationPMOrg != null ? _DeclarationPMOrg.DeclarationNumber : _DeclarationPM.DeclarationNumber)
            //});


            var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
            {
                Tenant = _DeclarationPM.Tenant,
                objectTableName = "Customs.Declaration",
                EventCode = "DCH",
                notes = "בוצע תיקון הצהרה - " + (_DeclarationPMOrg != null ? _DeclarationPMOrg.DeclarationNumber : _DeclarationPM.DeclarationNumber) + " מספר בקשה  - " + _DeclarationPM.AmendmentRequestNumber,
                CommunicationLoggingEntityReference = _DeclarationPM.DeclarationNumber,
                EntityId = _DeclarationPMOrg.Id,
                UserId = _userId,

                CommunicationSubject = "FU Status DCH from logitude ",
                MyFUStatus = new AmitalEventTracerModel.FUStatus()
                {
                    entname = "CFIFILEM",
                    primary_number = _DeclarationPMOrg.CustomFileNo,
                    status = "new",
                    xml_status = "new",
                    status_id = "DCH",
                    status_DateTime = DateTime.Now,
                    //status_place = "FRA",
                    //status_save = "no_fail",
                    comments = "בוצע תיקון הצהרה - " + (_DeclarationPMOrg != null ? _DeclarationPMOrg.DeclarationNumber : _DeclarationPM.DeclarationNumber) + " מספר בקשה  - " + _DeclarationPM.AmendmentRequestNumber,
                }
            };
            AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel);



          var  myUpdateEventContextTagModel = new EventContextTagModel()
            {
                CallProccessID = EventContextTagModel.ProccessEnum.DF_NG_5117_ImportDeclerationAmendmentReplyResponseService,
                EventCode = "DCH",
                EventRemarks = "Declaration Changed By Customs",
                FUStatusRemarks = "בוצע תיקון הצהרה" + (_DeclarationPMOrg != null ? _DeclarationPMOrg.DeclarationNumber : _DeclarationPM.DeclarationNumber) + " מספר בקשה  - " + _DeclarationPM.AmendmentRequestNumber,
            };
            _DeclarationPM.CurrentContextTag = myUpdateEventContextTagModel;

            //var myUpdateEventContextTagModel = new EventContextTagModel()
            //{
            //    CallProccessID = EventContextTagModel.ProccessEnum.DF_NG_5117_ImportDeclerationAmendmentReplyResponseService,
            //    EventCode = "DCH",
            //    EventRemarks = "Declaration Changed By Customs",
            //    FUStatusRemarks = "בוצע תיקון הצהרה" +  _DeclarationPMOrg.DeclarationNumber,
            //};

            //_DeclarationPMOrg.CurrentContextTag = myUpdateEventContextTagModel;
            //declarationUpdateService.Update(_DeclarationPMOrg, true);

            declarationUpdateService.Update(_DeclarationPM, true);
        }

        private string GetNextAmendmentRequestNumber()
        {
           DeclarationQueryService declarationQueryService = new DeclarationQueryService(_DeclarationPMOrg.Tenant);

            //var declarations=  declarationQueryService.GetDeclarationAmendmentsById(_DeclarationPMOrg.Tenant, _DeclarationPMOrg.Id);

            //return (Convert.ToInt32( declarations.Max(x => x.AmendmentRequestNumber) )+ 1).ToString();


            return (declarationQueryService.GetDeclarationMaxAmendmentRequestNumber(_DeclarationPMOrg.Tenant)+1).ToString();


         }

        private ResponseAdditionalInformation[] AdditionalInformation()
        {
          List< ResponseAdditionalInformation>  responseAdditionalInformation = new List<ResponseAdditionalInformation>();

            if (!string.IsNullOrEmpty(_DeclarationPM.AmendmentRemarks))
            {
                
                responseAdditionalInformation.Add(  new ResponseAdditionalInformation { StatementTypeCode = new AdditionalInformationStatementTypeCodeType { Value = "29"  } , Content = new AdditionalDocumentTypeTextType { Value = _DeclarationPM.AmendmentRemarks }  });
              }

            if (_DeclarationPM.AmendmentDeficitInitiated==true)
            {
                responseAdditionalInformation.Add(  new ResponseAdditionalInformation { StatementTypeCode = new AdditionalInformationStatementTypeCodeType { Value = "25" }, Content = new AdditionalDocumentTypeTextType { Value = "1" } } );
            }

            if (!string.IsNullOrEmpty(_DeclarationPM.AmendDeficitInitiatedReasTo))
            {
                responseAdditionalInformation.Add(  new ResponseAdditionalInformation { StatementTypeCode = new AdditionalInformationStatementTypeCodeType { Value = "26" }, Content = new AdditionalDocumentTypeTextType { Value = _DeclarationPM.AmendDeficitInitiatedReasTo  } });
            }
           if(!string.IsNullOrEmpty(_DeclarationPM.ReplacingRepairRequest))
            { 
                responseAdditionalInformation.Add(new ResponseAdditionalInformation { StatementTypeCode = new AdditionalInformationStatementTypeCodeType { Value = "24" }, Content = new AdditionalDocumentTypeTextType { Value = _DeclarationPM.ReplacingRepairRequest } });
            }
            if (_DeclarationPM.AmedmentType == "2")
            {
                responseAdditionalInformation.Add(new ResponseAdditionalInformation { StatementTypeCode = new AdditionalInformationStatementTypeCodeType { Value = "28" }, Content = new AdditionalDocumentTypeTextType { Value = "declaration closed" } });
            }
            return responseAdditionalInformation.ToArray();
         }

        private void UCBatchCheckLock(GenericRequestParams requestParams, DeclarationPM declarationPM)
        {
  
            LogMessagingUtil.Instance.AppendLine("CourierMaster Send Batch===> CheckLock");

            long lCUSTOMFILENO;
            if (!long.TryParse(declarationPM.CustomFileNo, out lCUSTOMFILENO))
            {
                throw new BusinessErrorException("_DirtyDeclarationPaymentPM.DeclarationId could not convert to long ");
            }
            var myCCUFILEMRepository = new CCUFILEMRepository(declarationPM.Tenant);
            var ccufilem = myCCUFILEMRepository.GetFILENOByCUSTOMFILENO(lCUSTOMFILENO);


            var myCCUQUELOCKRepository = new CCUQUELOCKRepository(requestParams.Tenant);
            try
            {
                var cculock = myCCUQUELOCKRepository.GetSingleGeneralLockNOWAIT("CCUFILEM", ccufilem.ToString());

            }
            catch (System.Exception)
            {

                LogMessagingUtil.Instance.AppendLine($"GetSingleGeneralLockNOWAIT(CCUFILEM, {ccufilem.ToString()}) ==> Already Lock => try later (*5) ");
                throw;
            }


        }

        private void CheckTaxationDateTime(DeclarationPM declarationPM)
        {
            //If TaxationDateTime is not Today change it before sending
            if (!_DeclarationPM.TaxationDateTime.HasValue ||
                (_DeclarationPM.TaxationDateTime.HasValue && _DeclarationPM.TaxationDateTime.Value.Date < DateTime.Now.Date))
            {               
                DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(this._context, new Dictionary<string, IContext>(), _DeclarationPM.Tenant);
                _DeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
                _DeclarationPM.TaxationDateTime = TenantServerConfigration.GetCurrentDateTime(_DeclarationPM.Tenant);

      
                declarationUpdateService.Update(_DeclarationPM, true);
            }
        }


        public static byte[] stringToBase64ByteArray(String input)
        {
            byte[] ret = System.Text.Encoding.Unicode.GetBytes(input);
            string s = Convert.ToBase64String(ret);
            ret = System.Text.Encoding.Unicode.GetBytes(s);
            return ret;
        }

        private Attachment[] GetAddAGlobalScannedAttachmentToEntity()
        {
            byte[] myContent = stringToBase64ByteArray("moran test !!!!GetApproveChangeTimeListXML()");
            var requestMessage = new Attachment();
            var Attachments = new Attachment[] {
                new Attachment()
            {

                documentType = "380",
                 AdditionalData =new AttachmentAdditionalData[] 
                { 
                    new  AttachmentAdditionalData (){fieldID = 3,fieldData="US"  } ,//ארץ חשבון 
                    new  AttachmentAdditionalData (){fieldID = 39,fieldData="5520"} ,///מספר חשבון
                    new  AttachmentAdditionalData (){fieldID = 55,fieldData=DataTypeConvertorUtil .Convert(DateTime.Now)  },//תאריך החשבון
                    new  AttachmentAdditionalData (){fieldID = 87,fieldData=false.ToString()  } ,//האם מסמך מקורי
                },
                fileName = "mmmmsdd99000.txt",
                 externalAttachmentID = "EMTYC-99000",
                Remark = "mY Remark ",
                content = myContent

            } };
            return Attachments;
        }

        T SetAmountTypeValue<T>(string CurrencyCode, decimal val)
              where T : AmountType, new()
        {

            ISO3AlphaCurrencyCodeContentType isoCurrency;
            var success = Enum.TryParse<ISO3AlphaCurrencyCodeContentType>(CurrencyCode, out isoCurrency);
            ;
            var cur1 = Enum.GetNames(typeof(ISO3AlphaCurrencyCodeContentType)).ToList().FirstOrDefault(cur => cur == CurrencyCode);
            if (cur1 == null)
            {
                ///throw new System.Exception("CurrencyCode is not valid " + CurrencyCode);  
            }

            if (!success)
            {
                ///throw new System.Exception("CurrencyCode is not valid " + CurrencyCode);  
            }
            return new T()
            {
                currencyID = isoCurrency,
                currencyIDSpecified = success,
                Value = val
            };

        }


        T SetQuantityTypeValue<T>(string measurementUnit, decimal val)
              where T : QuantityType, new()
        {
            var measurementUnitRealString = "";
            if (String.IsNullOrWhiteSpace(measurementUnit)) // hard coded
            {
             }
            else
            {
                var list = Enum.GetNames(typeof(MeasurementUnitCommonCodeContentType)).ToList();
                if (list.Exists(unit => unit == measurementUnit))
                {
                    measurementUnitRealString = list.FirstOrDefault(unit => unit == measurementUnit);
                }
                else if (list.Exists(unit => unit == measurementUnit.Reverse()))
                {
                    measurementUnitRealString = list.FirstOrDefault(unit => unit == measurementUnit.Reverse());
                }
            }
            
            if (String.IsNullOrWhiteSpace(measurementUnitRealString))
            {
             }
            
            MeasurementUnitCommonCodeContentType measurementCommonUnit;
            var success = Enum.TryParse(measurementUnitRealString, out measurementCommonUnit);
            
 
            if (!success)
            {
             }
            ;

            return new T()
            {
                unitCode = measurementCommonUnit,
                unitCodeSpecified = success,
                Value = val
            };

        }


        T SetMeasureTypeValue<T>(string measurementUnit, decimal val)
              where T : MeasureType, new()
        {

            var measurementUnitRealString = "";
            if (String.IsNullOrWhiteSpace(measurementUnit)) // hard coded
            {
                measurementUnitRealString = MeasurementUnitCommonCodeContentType.KGM.ToString();
            }
            else
            {
                var list = Enum.GetNames(typeof(MeasurementUnitCommonCodeContentType)).ToList();
                if (list.Exists(unit => unit == measurementUnit))
                {
                    measurementUnitRealString = list.FirstOrDefault(unit => unit == measurementUnit);
                }
                else if (list.Exists(unit => unit == measurementUnit.Reverse()))
                {
                    measurementUnitRealString = list.FirstOrDefault(unit => unit == measurementUnit.Reverse());
                }
            }
            if (String.IsNullOrWhiteSpace(measurementUnitRealString))
            {
                return null;
            }

            MeasurementUnitCommonCodeContentType measurementCommonUnit;
            var success = Enum.TryParse(measurementUnitRealString, out measurementCommonUnit);

            if (!success)
            {
                //measurementUnit = MeasurementUnitCommonCodeContentType.A1.ToString() ; // hard coded
                //success = Enum.TryParse<MeasurementUnitCommonCodeContentType>(measurementUnit, out measurementCommonUnit);
                ///throw new System.Exception("measurementUnit is not valid " + measurementUnit);  
            }
            ;

            return new T()
            {
                unitCode = measurementCommonUnit,
                unitCodeSpecified = success,
                Value = val
            };
        }


        T SetCodeTypeValue<T>(string val)
            where T : CodeType, new()
        {
            if (string.IsNullOrWhiteSpace(val))
            {
                return null;
            }
            return new T()
            {
                listID = "",
                listAgencyName = "",
                listName = "",
                listVersionID = "",
                name = "",
                listURI = "",
                listSchemeURI = "",
                Value = val
            };

        }

        T SetIDTypeValue<T>(string val, string schemeId = "")
            where T : IDType, new()
        {
            if (string.IsNullOrWhiteSpace(val))
            {
                return null;
            }
            return new T()
            {
                schemeID = schemeId,
                schemeName = "",
                schemeAgencyName = "",
                schemeVersionID = "",
                schemeDataURI = "",
                schemeURI = "",
                Value = val
            };
        }

        private Declaration Getdeclaration(Customs.Def.EntityPMs.DeclarationPM declarationPM, Customs.Def.EntityPMs.DeclarationPM declarationPMOrg)
        {
            var customDeclaration = new Declaration();
            customDeclaration.ID = new DeclarationIdentificationIDType() { Value = declarationPMOrg.DeclarationNumber };


            customDeclaration.DeclarationOfficeID = SetIDTypeValue<DeclarationDeclarationOfficeIDType>(declarationPM.DeclarationOfficeCode);

            customDeclaration.ExportDeclarationOfficeID = SetIDTypeValue<DeclarationDeclarationOfficeIDType>(declarationPM.ExportDeclarationOfficeCode);
            customDeclaration.TypeCode = SetCodeTypeValue<DeclarationTypeCodeType>(declarationPM.DeclarationTypeCode);// MUST  hard coded
            if (!String.IsNullOrWhiteSpace(declarationPM.DeclarationDocumentId))
            {
                customDeclaration.PreviousDocument = new DeclarationPreviousDocument
                {
                    ID = SetIDTypeValue<PreviousDocumentIdentificationIDType>(declarationPM.DeclarationDocumentId),
                    TypeCode = SetCodeTypeValue<PreviousDocumentTypeCodeType>(declarationPM.DeclarationDocumentTypeCode)
                };
            }
            customDeclaration.DMExtensions = GetDMExtensions(declarationPM);
            customDeclaration.AdditionalDocument = GetDeclarationAdditionalDocuments(declarationPM);
            customDeclaration.Agent = GetDeclarationAgent(declarationPM);
            customDeclaration.Exporter = GetImporter(declarationPM);


            //new DeclarationExporter[]
            //{
            //    new DeclarationExporter()
            //    {
            //        ID = SetIDTypeValue<ExporterIdentificationIDType>(declarationPM.ImporterCode),
            //       DMExtensions = new DeclarationExporterDMExtensions()
            //    {
            //        RoleCode = SetCodeTypeValue<DeclarationExporterDMExtensionsRoleCode>("7"),

            //        IssueLocation =  new DeclarationExporterDMExtensionsIssueLocation() { Value ="IL"}
            //    }

            //    }
            //};

            //  customDeclaration.Exporter[0].ID.schemeID= "1";

            if (!String.IsNullOrWhiteSpace(declarationPM.ProcedureCurrentCode))
            {
                customDeclaration.GovernmentProcedure = new DeclarationGovernmentProcedure()
                {
                    CurrentCode = SetCodeTypeValue<GovernmentProcedureCurrentCodeType>(declarationPM.ProcedureCurrentCode) //  declarationPM.ProcedureCurrentCodenew GovernmentProcedureCurrentCodeType()
                };
            }

            //   customDeclaration.Importer = GetImporter(declarationPM);
            customDeclaration.GoodsShipment = GetDeclarationGoodsShipment(declarationPM).ToArray();
            // moran 25.5.14 - Bug 6059 - commented -->
            //customDeclaration.DutyTaxFee = GetDeclarationDutyTaxFee(declarationPM).ToArray();

            return customDeclaration;
        }




        private DeclarationAgent[] GetDeclarationAgent(DeclarationPM declarationPM) // moran 31.5.15 - Task 13475
        {
            var declarationAgentList = new List<DeclarationAgent>();

            var declarationAgent = new DeclarationAgent()
            {

                ID = SetIDTypeValue<AgentIdentificationIDType>(declarationPM.AgentId),
                RoleCode = new AgentRoleCodeType()
                {
                    Value = "1" //hard coded
                }
            };

            declarationAgentList.Add(declarationAgent);

            if (declarationPM.Consignments != null && declarationPM.Consignments[0].CargoTypeCode == "17")
            {
                var declarationAgentSecond = new DeclarationAgent()
                {
                    ID = SetIDTypeValue<AgentIdentificationIDType>(declarationPM.Consignments[0].SecondCargoID),
                    RoleCode = new AgentRoleCodeType()
                    {
                        Value = "11" //hard coded
                    }
                };
                declarationAgentList.Add(declarationAgentSecond);
            }

            return declarationAgentList.ToArray();
        }

        private DeclarationAdditionalDocument[] GetDeclarationAdditionalDocuments(DeclarationPM declarationPM)
        {
            var declarationDMExtensionsAdditionalDocumentList = new List<DeclarationAdditionalDocument>();
            var customsDocumentQueryService = new CustomsDocumentQueryService(_context);
            var customsDocumentPMList = customsDocumentQueryService.GetCustomsDocumentPMListWithoutRequestedDoc(new GetTicketsParams() { ParentEntityId = declarationPM.Id, ParentEntityCode = "Declaration" }, declarationPM.Tenant);

            foreach (var customsDocumentPM in customsDocumentPMList)
            {
                //if (documentPointerItem.Child1EntityCode == null && documentPointerItem.Child2EntityCode == null && documentPointerItem.Child3EntityCode == null) this condition exists inside the query of get tickets for parent.
                //{
                //if (customsDocumentPM.DocumentsFilingId != null) // Only if there is a document ///mohammad.... customsdocuemntId is replaced by doucmentinid it's the same.
                if (!string.IsNullOrWhiteSpace(customsDocumentPM.CustomsDocId)) // Mirit 22/12/15 19136
                {
                    var declarationDMExtensionsAdditionalDocument = new DeclarationAdditionalDocument();
                    declarationDMExtensionsAdditionalDocument.DMExtensions = new DeclarationAdditionalDocumentDMExtensions();
                    declarationDMExtensionsAdditionalDocument.DMExtensions.ExternalAttachmentID = new ExternalAttachmentIDType();
                    declarationDMExtensionsAdditionalDocument.DMExtensions.ExternalAttachmentID.Value = customsDocumentPM.ExternalAttachmentId;

                    declarationDMExtensionsAdditionalDocumentList.Add(declarationDMExtensionsAdditionalDocument);
                }
            }
            return declarationDMExtensionsAdditionalDocumentList.ToArray();
        }




        private DeclarationDMExtensions GetDMExtensions(DeclarationPM declarationPM)
        {
            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.CustomFileNo = _DeclarationPMOrg.CustomFileNo;
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            this.MyRequestSheetParam.EntityId1 = declarationPM.Id;
            this.MyRequestSheetParam.RequestDescription = "תיקון הצהרת יצוא" + declarationPM.DeclarationNumber + " " + declarationPM.VersionId;

            var DMExtensions = new DeclarationDMExtensions();
            DMExtensions.AgentFileReferenceID = SetIDTypeValue<AgentFileReferenceIDType>(_DeclarationPMOrg.CustomFileNo); //new AgentFileReferenceIDType() { Value = declarationPM.CustomFileNo };

            DMExtensions.ReferenceDateTime = DataTypeConvertorUtil.Convert(declarationPM.TaxationDateTime);
                                                                                                                      // moran 25.5.14 - Bug 6059 - commented -->
            DMExtensions.VersionID = SetIDTypeValue<DeclarationVersionIDType>(declarationPM.VersionId); // new DeclarationDMExtensionsVersionID() { Value = declarationPM.VersionId };
            DMExtensions.ExternalDeclarationID = SetIDTypeValue<ExternalDeclarationIDType>(String.IsNullOrWhiteSpace(declarationPM.ExternalDeclarationNumber) ? declarationPM.CustomFileNo : declarationPM.ExternalDeclarationNumber); // hard coded - mandatory - takes from field other than the mapped if empty 
            // DMExtensions.ExternalDeclarationID = SetIDTypeValue<ExternalDeclarationIDType>(declarationPM.CustomFileNo); //new AgentFileReferenceIDType() { Value = declarationPM.CustomFileNo };
            //{
            //    Value = String.IsNullOrWhiteSpace(declarationPM.ExternalDeclarationNumber) ? "10008879" : declarationPM.ExternalDeclarationNumber 
            //};

            if (declarationPM.LoadingDateTime != null)
            {
                //    DMExtensions.DepartureDateTime = new DepartureDateTimeType() { Value =declarationPM.LoadingDateTime.Value };
            }
            DMExtensions.AutonomyRegionType = SetIDTypeValue<OriginRegionIDType>(declarationPM.ExportAutonomyRegionTypeCode); //new OriginRegionIDType() { Value = declarationPM.AutonomyRegionTypeCode };

            if (declarationPM.DestinationCountryCode != null)
            {
                DMExtensions.DestinationCountry = SetCodeTypeValue<DeclarationDMExtensionsDestinationCountry>(declarationPM.DestinationCountryCode);
            }

            //if (declarationPM.LoadingFactor.HasValue)
            //{
            //    DMExtensions.ExpenseLoadingFactor = new DeclarationDMExtensionsExpenseLoadingFactor()
            //    {
            //        Value = declarationPM.LoadingFactor.Value
            //    };
            //}


            DMExtensions.TransferDeclarationToDestinationCountry = new TransferDeclarationToDestinationCountryIndType() { Value = declarationPM.IsExporterConfirmation };


            if (declarationPM.DeclarationExportRecipients != null && declarationPM.DeclarationExportRecipients.Count() > 0)
            {
                List<DeclarationDMExtensionsRecipientDetails> declarationDMExtensionsRecipientDetails = new List<DeclarationDMExtensionsRecipientDetails>();
                foreach (var declarationExportRecipient in declarationPM.DeclarationExportRecipients)
                {
                    DeclarationDMExtensionsRecipientDetails declarationDMExtensionsRecipientDetails1 = new DeclarationDMExtensionsRecipientDetails();
                    declarationDMExtensionsRecipientDetails1.Name = declarationExportRecipient.RecipientName;
                    declarationDMExtensionsRecipientDetails1.Address = declarationExportRecipient.RecipientAddress;
                    declarationDMExtensionsRecipientDetails1.IssueLocation = SetCodeTypeValue<DeclarationDMExtensionsRecipientDetailsIssueLocation>(declarationExportRecipient.RecipientIssueCountryCode);
                    declarationDMExtensionsRecipientDetails.Add(declarationDMExtensionsRecipientDetails1);

                }
                DMExtensions.RecipientDetails = declarationDMExtensionsRecipientDetails.ToArray();
            }

            //}
            DMExtensions.DeclarationClosingDetails = GetDeclarationDMExtensionsDeclarationClosingDetails(declarationPM);
            //DMExtensions. = GetDeclarationDMExtensionsAdditionalDocument(declarationPM);
            return DMExtensions;
        }

        private DeclarationDMExtensionsDeclarationClosingDetails GetDeclarationDMExtensionsDeclarationClosingDetails(DeclarationPM declarationPM)
        {
            var closingDetails = new DeclarationDMExtensionsDeclarationClosingDetails();
            var exportDeclarationClosingDataRepository = new ExportDeclarationClosingDataRepository(declarationPM.Tenant);
            var entityClosingDeclaration = exportDeclarationClosingDataRepository.getByDecId(declarationPM.Id, declarationPM.Tenant);
            if (entityClosingDeclaration != null)
            {
                closingDetails.FinalShipID = new SeaTransportationIDType { Value = entityClosingDeclaration.FinalShipCode };
                closingDetails.FinalLoadingSite = new FinalLoadingSiteIDType { Value = entityClosingDeclaration.FinalLoadingSite };
                if (entityClosingDeclaration.LoadingDateTime.HasValue)
                {
                    closingDetails.DepartureDateTime = new DepartureDateTimeType { Value = (DateTime)entityClosingDeclaration.LoadingDateTime };
                }
                closingDetails.FinalTransportContractDocument = new DeclarationDMExtensionsDeclarationClosingDetailsFinalTransportContractDocument
                {
                    FirstCargoID = new TransportContractDocumentIdentificationIDType { Value = entityClosingDeclaration.FinalManifestNumber },
                    TypeCode = new TransportContractDocumentTypeCodeType { Value = entityClosingDeclaration.FinalCargoTypeCode },
                    SecondCargoID = new SecondCargoIDType { Value = entityClosingDeclaration.FinalSecondCargoId },
                    ThirdCargoID = new ThirdCargoIDType { Value = entityClosingDeclaration.FinalThirdCargoId }

                };
            }
            return closingDetails;
        }


        
        private string ResolveFromGlobalScannedAttachmentToEntityOperation780()
        {
            return "90025241";//780

        }
        private string ResolveFromGlobalScannedAttachmentToEntityOperation707()
        {


            return "90025366"; // 707 doc type
        }
        private string ResolveFromGlobalScannedAttachmentToEntityOperation703()
        {

            return "90024967"; // 703\

        }

        public string ImportersCheck(string importerField, string importerCode, string importerId, string importerType,
   string importerName, string importerAddress, string importerPassportNumber, string importerPassCountryCode)
        {
            var errorMessage = "";
            var importer = "";
            if (_DeclarationPM != null)
            {
                switch (importerType)
                {
                    case "1":
                        if (string.IsNullOrWhiteSpace(importerId))
                        {
                            if (!string.IsNullOrWhiteSpace(importerCode))
                            {
                                importer = importerCode;
                            }
                            else
                            {
                                //Check if ImporterName & ImporterAddrress has value
                                if (string.IsNullOrWhiteSpace(importerName) && string.IsNullOrWhiteSpace(importerAddress))
                                {
                                    errorMessage = "יש להזין נתוני יבואן " + importerField + " לפני שליחה";
                                }
                            }
                        }
                        else
                        {
                            var queryService = new ClientQueryService(_context);
                            var importerPM = queryService.GetSingle(importerId, true, false);
                            if (importerPM == null) return "";
                            importer = importerPM.Code;
                        }
                        break;
                    case "2":
                    case "3":
                        {
                            //Check That Both PassportCountry & PassportNumber has values
                            if (string.IsNullOrWhiteSpace(importerPassportNumber) || string.IsNullOrWhiteSpace(importerPassCountryCode))
                            {
                                errorMessage = "יש להזין נתוני יבואן " + importerField + " לפני שליחה";
                            }
                            else
                            {
                                importer = importerPassportNumber;
                            }
                            break;
                        }
                    default:
                        {
                            if (string.IsNullOrWhiteSpace(importerId))
                            {
                                if (!string.IsNullOrWhiteSpace(importerCode))
                                {
                                    errorMessage = "יש לשלוף לקוח מהמכס עבור יבואן " + importerField + " לפני שליחה";
                                }
                                else
                                {
                                    errorMessage = "מספר יבואן " + importerField + " הוא שדה חובה";
                                }
                            }
                            break;

                        }
                }
            }
            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                throw new BusinessErrorException(errorMessage);
            }
            return (importer);
        }


        private DeclarationExporter GetDeclarationImporterRole4(DeclarationPM declarationPM)
        {
            //<--- Yuval Chalup 15.11.2016 TASK-24438 - CHANGED FROM:
            //var ImporterId = GetImporterCode(declarationPM.ImporterId);
            //if (String.IsNullOrWhiteSpace(ImporterId)) // Task 6440 - add ImporterCode fields check
            //{
            //    ImporterId = GetImporterCode(declarationPM.ImporterCode);
            //}
            //if (String.IsNullOrWhiteSpace(ImporterId))
            //{
            //    throw new BusinessErrorException("Importer is empty");
            //}
            //TO:
            //ImporterId = GetImporterCode(declarationPM.ImporterId);
            var ImporterId = ImportersCheck("", declarationPM.ImporterCode, declarationPM.ImporterId, declarationPM.ImporterTypeCode, declarationPM.ImporterName, declarationPM.ImporterAddress, declarationPM.ImporterPassportNumber, declarationPM.ImporterPassCountryCode);
            //if (!string.IsNullOrWhiteSpace(errorMessage))
            //{
            //    throw new BusinessErrorException(errorMessage);
            //}
            //Yuval Chalup 15.11.2016 TASK-24438 --->

            var declarationImporter = new DeclarationExporter();
            declarationImporter.ID = SetIDTypeValue<ExporterIdentificationIDType>(ImporterId, declarationPM.ImporterTypeCode); // new ImporterIdentificationIDType() // moran 24.3.15 - Task 11461 - use declarationPM.ImporterTypeCode instead of hard coded "1"

            string importerAddress = null;
            string importerName = null;
            if (string.IsNullOrWhiteSpace(ImporterId))//task 45505
            {
                if (!string.IsNullOrWhiteSpace(declarationPM.ImporterAddress)) { importerAddress = declarationPM.ImporterAddress; }
                if (!string.IsNullOrWhiteSpace(declarationPM.ImporterName)) { importerName = declarationPM.ImporterName; }
            }
            declarationImporter.DMExtensions = new DeclarationExporterDMExtensions()
            {
                //                Address = declarationPM.ImporterAddress,
                //Name = declarationPM.ImporterName,

                //Address = importerAddress,//task 45505
                //Name = importerName,
                //EntitlementTypeCode = new EntitlementTypeCodeType()
                //{
                //    Value = declarationPM.MainImporterEntitlemntTypeCode
                //},
                RoleCode = new DeclarationExporterDMExtensionsRoleCode()
                {
                    Value = "7"
                }
            };
            if (declarationPM.ImporterTypeCode == "2" || declarationPM.ImporterTypeCode == "3")
            {
                declarationImporter.DMExtensions.IssueLocation = new DeclarationExporterDMExtensionsIssueLocation() { Value = declarationPM.ImporterPassCountryCode };
            }

            return declarationImporter;
        }
        private DeclarationExporter GetDeclarationImporterRole5(DeclarationPM declarationPM)
        {
            //<--- Yuval Chalup 15.11.2016 TASK-24438 - CHANGED FROM:
            //var declarationImporter = new DeclarationImporter();
            // Task 6440 - add ImporterCode fields check
            //if (!String.IsNullOrWhiteSpace(declarationPM.TransferImporterId))
            //{
            //    declarationImporter.ID = SetIDTypeValue<ImporterIdentificationIDType>(GetImporterCode(declarationPM.TransferImporterId), declarationPM.TransferImporterTypeCode); // new ImporterIdentificationIDType()  // moran 24.3.15 - Task 11461 - add declarationPM.ImporterTypeCode
            //}
            // else
            //{
            //    declarationImporter.ID = SetIDTypeValue<ImporterIdentificationIDType>(declarationPM.TransferImporterCode, declarationPM.TransferImporterTypeCode); // moran 24.3.15 - Task 11461 - add declarationPM.ImporterTypeCode // Mirit 15/11/15 - Change to TransferImporterCode
            //}
            //TO:
            var ImporterId = ImportersCheck("מעביר", declarationPM.TransferImporterCode, declarationPM.TransferImporterId, declarationPM.TransferImporterTypeCode, declarationPM.TransferImporterName, declarationPM.TransferImporterAddress, declarationPM.TransferPassportNumber, declarationPM.TransferImporterCountryCode);
            var declarationImporter = new DeclarationExporter();
            declarationImporter.ID = SetIDTypeValue<ExporterIdentificationIDType>(ImporterId, declarationPM.TransferImporterTypeCode);
            //Yuval Chalup 15.11.2016 TASK-24438 --->

            declarationImporter.DMExtensions = new DeclarationExporterDMExtensions()
            {
                RoleCode = new DeclarationExporterDMExtensionsRoleCode()
                {
                    Value = "12"
                }
            };
            if (declarationPM.TransferImporterTypeCode == "2" || declarationPM.TransferImporterTypeCode == "3")
            {
                declarationImporter.DMExtensions.IssueLocation = new DeclarationExporterDMExtensionsIssueLocation() { Value = declarationPM.TransferImporterCountryCode };
            }
            return declarationImporter;
        }


        private DeclarationExporter[] GetImporter(DeclarationPM declarationPM)
        {
            var declarationExporterList = new List<DeclarationExporter>();

            // if (!String.IsNullOrWhiteSpace(declarationPM.ImporterId))
            //  {
            declarationExporterList.Add(GetDeclarationImporterRole4(declarationPM));
            //  }
            // Task 6440 - add ImporterCode fields check
            if (!String.IsNullOrWhiteSpace(declarationPM.TransferImporterId) || !String.IsNullOrWhiteSpace(declarationPM.TransferImporterCode))
            {
                declarationExporterList.Add(GetDeclarationImporterRole5(declarationPM));
            }
            //if (!String.IsNullOrWhiteSpace(declarationPM.EntitleImporterId) || !String.IsNullOrWhiteSpace(declarationPM.EntitleImporterCode))
            //{
            //    declarationExporterList.Add(GetDeclarationImporterRole6(declarationPM));
            //}

            return declarationExporterList.ToArray();
        }
       
        private List<DeclarationGoodsShipment> GetDeclarationGoodsShipment(DeclarationPM declarationPM)
        {
            var declarationGoodsShipmentList = new List<DeclarationGoodsShipment>();
            //declarationGoodsShipmentList
            // declarationPM\



            //for (int supplierInvoiceSeq = 0; supplierInvoiceSeq < declarationPM.SupplierInvoices.Count(); supplierInvoiceSeq++)
            //{
            //var supplierInvoicePM =declarationPM.SupplierInvoices[supplierInvoiceSeq];
            //declarationGoodsShipment.SequenceNumeric = supplierInvoiceSeq + 1;

            foreach (var supplierInvoicePM in declarationPM.SupplierInvoices
                ///.Where( rec => rec.SequenceNumeric !=null)
                .OrderBy(rec => rec.SequenceNumeric).ToList())
            {
                if (supplierInvoicePM.IsAccumalated == true && supplierInvoicePM.SupplierInvoiceItems != null && supplierInvoicePM.SupplierInvoiceItems.Count > 0)
                {
                    supplierInvoicePM.SupplierInvoiceItems.RemoveAll(rec => rec.IsParent != true);
                }

                var declarationGoodsShipment = new DeclarationGoodsShipment();

                declarationGoodsShipment.SequenceNumeric = supplierInvoicePM.SequenceNumeric.Value;

                // declarationGoodsShipment.SequenceNumericSpecified = true;
                declarationGoodsShipment.Invoice = GetDeclarationGoodsShipmentInvoice(supplierInvoicePM);
                //CustomContext context = new CustomContext();
                //ConnectedEntity conn = new ConnectedEntity();
                //var vendorNumber = (from v in context.Vendors where v.Id == supplierInvoicePM.VendorId select v.VendorNumber);
                string vendorNumber = GetVendorNumber(supplierInvoicePM.VendorId);
                //if (!String.IsNullOrWhiteSpace(vendorNumber)) // moran 13.7.14 - Task 6817 - enter into 'if'
                //{
                //    declarationGoodsShipment.Supplier = new DeclarationGoodsShipmentSupplier()
                //    {
                //        //hardcoded VendorId
                //        // ID = SetIDTypeValue<SupplierIdentificationIDType>(String.IsNullOrWhiteSpace(supplierInvoicePM.VendorId) ? "2000012" : supplierInvoicePM.VendorId) // new SupplierIdentificationIDType() { Value = String.IsNullOrWhiteSpace(supplierInvoicePM.VendorId) ? "04" : supplierInvoicePM.VendorId } //HARDCODED
                //        // moran 13.7.14 - Task 6817 - cancel hard code
                //        //ID = SetIDTypeValue<SupplierIdentificationIDType>(String.IsNullOrWhiteSpace(vendorNumber) ? "2000012" : vendorNumber) // hard coded
                //        ID = SetIDTypeValue<SupplierIdentificationIDType>(vendorNumber) // hard coded

                //    };
                //}
                declarationGoodsShipment.TradeTerms = new DeclarationGoodsShipmentTradeTerms() // MUST 
                {
                    //ConditionCode = SetCodeTypeValue<TradeTermsConditionCodeType>(String.IsNullOrWhiteSpace(supplierInvoicePM.IncotermCode) ? "FOB" : supplierInvoicePM.IncotermCode), 
                    ConditionCode = SetCodeTypeValue<TradeTermsConditionCodeType>(supplierInvoicePM.IncotermCode),
                    //LocationID = SetIDTypeValue <TradeTermsLocationIDType >(String.IsNullOrWhiteSpace(supplierInvoicePM.IssueCountryCode) ? "CN" : supplierInvoicePM.IssueCountryCode) 
                    //   LocationID = SetIDTypeValue<TradeTermsLocationIDType>(supplierInvoicePM.IssueCountryCode)
                };
                //    declarationGoodsShipment.CustomsValuation = GetcustomsValuation(supplierInvoicePM).ToArray();

                var declarationConsignmentList = new List<DeclarationGoodsShipmentExportConsignment>();
                for (int consignmentSeq = 0; consignmentSeq < declarationPM.Consignments.Count(); consignmentSeq++)
                {
                    string consignmentType = declarationPM.Consignments[consignmentSeq].ConsignmentType;
                    if (supplierInvoicePM.SequenceNumeric.Value == 1 && !declarationPM.ExcludeConsignment && consignmentType == "I") // I=Import
                    {
                        declarationGoodsShipment.ImportConsignment = GetDeclarationImportConsignment(declarationPM.Consignments[consignmentSeq], consignmentSeq).ToArray();
                    }
                    else if (supplierInvoicePM.SequenceNumeric.Value == 1 && !declarationPM.ExcludeConsignment)
                    {
                        declarationConsignmentList.AddRange(GetDeclarationExportConsignment(declarationPM.Consignments[consignmentSeq], consignmentSeq));
                    }
                }
                declarationGoodsShipment.ExportConsignment = declarationConsignmentList.ToArray();
                declarationGoodsShipment.AdditionalDocument = GetDeclarationGoodsShipmentAdditionalDocument(supplierInvoicePM);
                declarationGoodsShipment.GovernmentAgencyGoodsItem = GetDeclarationGoodsItems(supplierInvoicePM).ToArray();


                if (supplierInvoicePM.SupplierInvoiceUCRs != null && supplierInvoicePM.SupplierInvoiceUCRs.Count() > 0)
                {
                    List<DeclarationGoodsShipmentUCR> declarationGoodsShipmentUCRs = new List<DeclarationGoodsShipmentUCR>();
                    foreach (var supplierInvoiceUCR in supplierInvoicePM.SupplierInvoiceUCRs)
                    {
                        DeclarationGoodsShipmentUCR declarationGoodsShipmentUCR = new DeclarationGoodsShipmentUCR();
                        declarationGoodsShipmentUCR.ID = SetIDTypeValue<UCRIdentificationIDType>(supplierInvoiceUCR.SupplierChargeID);
                        declarationGoodsShipmentUCR.TraderAssignedReferenceID = SetIDTypeValue<UCRTraderAssignedReferenceIDType>(supplierInvoiceUCR.AgentChargeID);
                        declarationGoodsShipmentUCRs.Add(declarationGoodsShipmentUCR);
                    }

                    declarationGoodsShipment.UCR = declarationGoodsShipmentUCRs.ToArray();

                }
                declarationGoodsShipmentList.Add(declarationGoodsShipment);

            }
            return declarationGoodsShipmentList;
        }
        private string GetVendorNumber(string vendorId)
        {
            var queryService = new CustomsVendorQueryService(_context);
            var vendorPM = queryService.GetSingle(vendorId, true, false);
            if (vendorPM == null) return "";
            return vendorPM.VendorNumber;

        }

        private DeclarationGoodsShipmentImportConsignmentDMExtensions GetImportConsignmentDMExtensions(ConsignmentPM consignmentPM)
        {
            var dmExtensions = new DeclarationGoodsShipmentImportConsignmentDMExtensions
            {
                CargoDescription = new DeclarationGoodsShipmentImportConsignmentDMExtensionsCargoDescription()
                {
                    Value = consignmentPM.CargoDescription
                },
                ExportationCountryCode = new ExportationCountryCodeType()
                {
                    Value = consignmentPM.OriginCountryCode
                },
            };
            dmExtensions.LastReleaseFromWarehousInd = new LastReleaseFromWarehousIndType();
            if (consignmentPM.IsLastReleaseFromWarehous == "T")
            {
                dmExtensions.LastReleaseFromWarehousInd.Value = true;
            }
            else
            {
                dmExtensions.LastReleaseFromWarehousInd.Value = false;
            }
            var registeredFacilitylist = new List<DeclarationGoodsShipmentImportConsignmentDMExtensionsRegisteredFacility>();
            int seqnum = 0;
            if (!String.IsNullOrWhiteSpace(consignmentPM.StorageSiteCode))
            {
                seqnum++;
                registeredFacilitylist.Add(GetImportRegisteredFacility(consignmentPM.StorageSiteCode, "004", seqnum, consignmentPM));
            }
            if (!String.IsNullOrWhiteSpace(consignmentPM.ReceiverWarehouseCode))
            {
                seqnum++;
                registeredFacilitylist.Add(GetImportRegisteredFacility(consignmentPM.ReceiverWarehouseCode, "003", seqnum, consignmentPM));
            }
            if (consignmentPM.ConsignmentInternalTransitions != null)
            {
                if (consignmentPM.ConsignmentInternalTransitions.FirstOrDefault() != null)
                {
                    if (!String.IsNullOrWhiteSpace(consignmentPM.ConsignmentInternalTransitions.FirstOrDefault().SiteCode))
                    {
                        seqnum++;
                        registeredFacilitylist.Add(GetImportRegisteredFacility(consignmentPM.ConsignmentInternalTransitions.FirstOrDefault().SiteCode, "005", seqnum, consignmentPM));
                    }
                }
            }
            if (seqnum > 0)
            {
                dmExtensions.RegisteredFacility = registeredFacilitylist.ToArray();
            }
            return dmExtensions;
        }
        private DeclarationGoodsShipmentImportConsignmentDMExtensionsRegisteredFacilityDMExtensions GetImportRegisteredFacilityDMExtensions(ConsignmentPM consignmentPM)
        {
            var dmExtensions = new DeclarationGoodsShipmentImportConsignmentDMExtensionsRegisteredFacilityDMExtensions();
            if (consignmentPM.ConsignmentInternalTransitions != null)
            {
                if (consignmentPM.ConsignmentInternalTransitions.FirstOrDefault() != null)
                {
                    dmExtensions.ArrivalOrder = consignmentPM.ConsignmentInternalTransitions.FirstOrDefault().LineNumber;
                }
            }
            dmExtensions.PackagesMeasure = GetDeclarationImportConsignmentPackages(consignmentPM).ToArray();
            return dmExtensions;
        }

        private List<DeclarationGoodsShipmentImportConsignmentDMExtensionsRegisteredFacilityDMExtensionsPackagesMeasure> GetDeclarationImportConsignmentPackages(ConsignmentPM consignmentPM)
        {
            var declarationConsignmentPackageList = new List<DeclarationGoodsShipmentImportConsignmentDMExtensionsRegisteredFacilityDMExtensionsPackagesMeasure>();
            for (int consignmentPackageSeq = 0; consignmentPackageSeq < consignmentPM.ConsignmentPackages.Count(); consignmentPackageSeq++)
            {
                var consignmentPackagePM = consignmentPM.ConsignmentPackages[consignmentPackageSeq];
                var declarationConsignmentPackage = new DeclarationGoodsShipmentImportConsignmentDMExtensionsRegisteredFacilityDMExtensionsPackagesMeasure
                {
                    SequenceNumeric = consignmentPackageSeq + 1,
                    TotalPackageQuantity = SetQuantityTypeValue<ConsignmentTotalPackageQuantityType>(consignmentPackagePM.PackageQuantityTypeCode, consignmentPackagePM.PackageQuantity.Value)
                };
                if (consignmentPackagePM.GrossMassMeasure.HasValue)
                {
                    declarationConsignmentPackage.GrossMassMeasure = SetMeasureTypeValue<DeclarationGoodsShipmentImportConsignmentDMExtensionsRegisteredFacilityDMExtensionsPackagesMeasureGrossMassMeasure>(consignmentPackagePM.GrossMassMeasureTypeCode, consignmentPackagePM.GrossMassMeasure.Value);
                }
                declarationConsignmentPackage.TypeCode = new DeclarationGoodsShipmentImportConsignmentDMExtensionsRegisteredFacilityDMExtensionsPackagesMeasureTypeCode() { Value = consignmentPackagePM.PackageTypeCode };
                declarationConsignmentPackage.MarksNumbers = new DeclarationGoodsShipmentImportConsignmentDMExtensionsRegisteredFacilityDMExtensionsPackagesMeasureMarksNumbers() { Value = consignmentPackagePM.MarksNumbers };
                declarationConsignmentPackageList.Add(declarationConsignmentPackage);

            }
            return declarationConsignmentPackageList;
        }

        private DeclarationGoodsShipmentImportConsignmentDMExtensionsRegisteredFacility GetImportRegisteredFacility(string p1, string p2, int seqnum, ConsignmentPM consignmentPM)
        {
            var registeredFacility = new DeclarationGoodsShipmentImportConsignmentDMExtensionsRegisteredFacility
            {
                ID = new DeclarationGoodsShipmentImportConsignmentDMExtensionsRegisteredFacilityID() { Value = p1 },
                FacilityType = new DeclarationGoodsShipmentImportConsignmentDMExtensionsRegisteredFacilityFacilityType() { Value = p2 },
                SequenceNumeric = seqnum,
                DMExtensions = GetImportRegisteredFacilityDMExtensions(consignmentPM)
            };
            return registeredFacility;
        }

        private List<DeclarationGoodsShipmentExportConsignment> GetDeclarationExportConsignment(ConsignmentPM consignmentPM, int consignmentSeq)
        {
            var declarationConsignmentList = new List<DeclarationGoodsShipmentExportConsignment>();
            var declarationConsignment = new DeclarationGoodsShipmentExportConsignment()
            {
                SequenceNumeric = consignmentSeq + 1
            };
            declarationConsignment.TransportContractDocument = new DeclarationGoodsShipmentExportConsignmentTransportContractDocument()
            {
                TypeCode = SetCodeTypeValue<TransportContractDocumentTypeCodeType>(consignmentPM.CargoTypeCode), //new TransportContractDocumentTypeCodeType() {Value =  "IL1"}, //hardcoded ask yaron + consignmentPM.CargoTypeCode },
                                                                                                                 // IssueDateTime = consignmentPM.ManifestDate.HasValue ? DataTypeConvertorUtil.Convert(consignmentPM.ManifestDate.Value) : null, // DataTypeConvertorUtil.Convert(declarationPM.IssueDateTime.Value), // hard coded
                ID = SetIDTypeValue<TransportContractDocumentIdentificationIDType>(consignmentPM.ManifestNumber), // new TransportContractDocumentIdentificationIDType() { Value = consignmentPM.ManifestNumber },
                                                                                                                  //ID = SetIDTypeValue<TransportContractDocumentIdentificationIDType>("123456"), // new TransportContractDocumentIdentificationIDType() { Value = consignmentPM.ManifestNumber }, HARD CODED
                DMExtensions = new DeclarationGoodsShipmentExportConsignmentTransportContractDocumentDMExtensions()
                {

                    SecondCargoID = SetIDTypeValue<SecondCargoIDType>(consignmentPM.SecondCargoID), // new SecondCargoIDType() { Value = consignmentPM.SecondCargoID },
                    ThirdCargoID = SetIDTypeValue<ThirdCargoIDType>(consignmentPM.ThirdCargoID) // new ThirdCargoIDType() { Value = consignmentPM.ThirdCargoID }
                }
            };
            if (consignmentPM.CargoTypeCode == "17" && !string.IsNullOrWhiteSpace(consignmentPM.ThirdCargoID))
            {
                var thirdCargoID = consignmentPM.ThirdCargoID;
                if (consignmentPM.ThirdCargoID.Length >= 8)
                {
                    thirdCargoID = consignmentPM.ThirdCargoID.Substring(0, 4) + consignmentPM.ThirdCargoID.Substring(6, 2);
                }
                declarationConsignment.TransportContractDocument.DMExtensions.ThirdCargoID = SetIDTypeValue<ThirdCargoIDType>(thirdCargoID);
            }
            //}

            /*  if (consignmentPM.ManifestDate.HasValue)
              {
                  declarationConsignment.TransportContractDocument.IssueDateTime = DataTypeConvertorUtil.Convert(consignmentPM.ManifestDate.Value);
              } 

              if (consignmentPM.UnloadDate.HasValue)
              {
                  declarationConsignment.UnloadingLocation.ArrivalDateTime = DataTypeConvertorUtil.Convert(consignmentPM.UnloadDate.Value);
              }*/

            declarationConsignment.UnloadingLocation = new DeclarationGoodsShipmentExportConsignmentUnloadingLocation()
            {
                ID = SetIDTypeValue<DeclarationGoodsShipmentExportConsignmentUnloadingLocationID>(consignmentPM.ExportUnloadingPortCode), //consignmentPM.UnloadPortCode// new UnloadingLocationIdentificationIDType() { Value = consignmentPM.UnloadPortCode },
                                                                                                                                          // ArrivalDateTime = consignmentPM.UnloadDate.HasValue ? DataTypeConvertorUtil.Convert(consignmentPM.UnloadDate.Value) : null,
            };
            declarationConsignment.LoadingLocation = new DeclarationGoodsShipmentExportConsignmentLoadingLocation()
            {
                ID = SetIDTypeValue<DeclarationGoodsShipmentExportConsignmentLoadingLocationID>(consignmentPM.ExportLoadingPortCode) //consignmentPM.LoadingPortCode new LoadingLocationIdentificationIDType() { Value = consignmentPM.LoadingPortCode }
            };
            declarationConsignment.DMExtensions = GetDMExtensionsConsignment(consignmentPM);


            declarationConsignmentList.Add(declarationConsignment);


            return declarationConsignmentList;
        }



        private List<DeclarationGoodsShipmentImportConsignment> GetDeclarationImportConsignment(ConsignmentPM consignmentPM, int consignmentSeq)
        {
            var declarationConsignmentList = new List<DeclarationGoodsShipmentImportConsignment>();
            var declarationConsignment = new DeclarationGoodsShipmentImportConsignment()
            {
                SequenceNumeric = consignmentSeq + 1
            };
            declarationConsignment.DMExtensions = GetImportConsignmentDMExtensions(consignmentPM);
            declarationConsignment.LoadingLocation = new DeclarationGoodsShipmentImportConsignmentLoadingLocation
            {
                ID = SetIDTypeValue<DeclarationGoodsShipmentImportConsignmentLoadingLocationID>(consignmentPM.LoadingPortCode)
            };
            declarationConsignment.UnloadingLocation = new DeclarationGoodsShipmentImportConsignmentUnloadingLocation()
            {
                ID = SetIDTypeValue<DeclarationGoodsShipmentImportConsignmentUnloadingLocationID>(consignmentPM.UnloadPortCode)
            };
            declarationConsignment.TransportContractDocument = new DeclarationGoodsShipmentImportConsignmentTransportContractDocument()
            {
                TypeCode = SetCodeTypeValue<TransportContractDocumentTypeCodeType>(consignmentPM.CargoTypeCode),
                ID = SetIDTypeValue<TransportContractDocumentIdentificationIDType>(consignmentPM.ManifestNumber),
                DMExtensions = new DeclarationGoodsShipmentImportConsignmentTransportContractDocumentDMExtensions
                {
                    SecondCargoID = SetIDTypeValue<SecondCargoIDType>(consignmentPM.SecondCargoID),
                    ThirdCargoID = SetIDTypeValue<ThirdCargoIDType>(consignmentPM.ThirdCargoID),
                }
            };

            if (consignmentPM.CargoTypeCode == "17" && !string.IsNullOrWhiteSpace(consignmentPM.ThirdCargoID))
            {
                var thirdCargoID = consignmentPM.ThirdCargoID;
                if (consignmentPM.ThirdCargoID.Length >= 8)
                {
                    thirdCargoID = consignmentPM.ThirdCargoID.Substring(0, 4) + consignmentPM.ThirdCargoID.Substring(6, 2);
                }
                declarationConsignment.TransportContractDocument.DMExtensions.ThirdCargoID = SetIDTypeValue<ThirdCargoIDType>(thirdCargoID);
            }
            declarationConsignmentList.Add(declarationConsignment);

            return declarationConsignmentList;
        }


        private DeclarationGoodsShipmentAdditionalDocument[] GetDeclarationGoodsShipmentAdditionalDocument(SupplierInvoicePM supplierInvoicePM)
        {
            var declarationGoodsShipmentAdditionalDocumentList = new List<DeclarationGoodsShipmentAdditionalDocument>();
            //CustomsDocumentsTicketId adjustment mohammad 18.10.14
            //var customsDocumentPointerPMList = new List<CustomsDocumentPointerPM>();
            //var customsDocumentPointerQueryService = new CustomsDocumentPointerQueryService(_context);

            //customsDocumentPointerPMList = customsDocumentPointerQueryService.GetParentDocumentPointer(supplierInvoicePM.DeclarationId, "Declaration", supplierInvoicePM.Tenant);
            //var customsDocumentsTicketQueryService = new CustomsDocumentsTicketQueryService(_context);
            //var customsDocumentsTicketPMList = customsDocumentsTicketQueryService.GetCustomsDocumentsTickets(new GetTicketsParams() { ParentEntityId = supplierInvoicePM.DeclarationId, ParentEntityCode = "Declaration", Child1EntityCode = "SupplierInvoice", Child1EntityId = supplierInvoicePM.InvoiceCounterKey.ToString() }, supplierInvoicePM.Tenant);


            var customsDocumentQueryService = new CustomsDocumentQueryService(_context);
            var customsDocumentPMList = customsDocumentQueryService.GetCustomsDocumentPMListWithoutRequestedDoc(new GetTicketsParams() { ParentEntityId = supplierInvoicePM.DeclarationId, ParentEntityCode = "Declaration", Child1EntityCode = "SupplierInvoice", Child1EntityId = supplierInvoicePM.InvoiceCounterKey.ToString() }, supplierInvoicePM.Tenant);

            foreach (var customsDocumentPM in customsDocumentPMList)
            {
                //if (documentPointerItem.Child1EntityCode == "SupplierInvoice" && documentPointerItem.Child1EntityId == supplierInvoicePM.InvoiceCounterKey.ToString() && documentPointerItem.Child2EntityCode == null && documentPointerItem.Child3EntityCode == null)
                //{
                if (!string.IsNullOrWhiteSpace(customsDocumentPM.CustomsDocId)) // Mirit 22/12/15 19136
                {
                    var declarationGoodsShipmentAdditionalDocument = new DeclarationGoodsShipmentAdditionalDocument();
                    declarationGoodsShipmentAdditionalDocument.DMExtensions = new DeclarationGoodsShipmentAdditionalDocumentDMExtensions();
                    //mirit20131222 declarationGoodsShipmentAdditionalDocument.DMExtensions.AttachmentID = new AttachmentIDType();
                    declarationGoodsShipmentAdditionalDocument.DMExtensions.ExternalAttachmentID = new ExternalAttachmentIDType();

                    //mirit20131222 declarationGoodsShipmentAdditionalDocument.DMExtensions.AttachmentID.Value = documentPointerItem.CustomsDocId;             // From CustomsDocuments (Ref to custom Id) 
                    declarationGoodsShipmentAdditionalDocument.DMExtensions.ExternalAttachmentID.Value = customsDocumentPM.ExternalAttachmentId; //customsDocumentPM.DocumentsFilingId; // From CustomsDocumentPointers (Logitude Filling)

                    declarationGoodsShipmentAdditionalDocumentList.Add(declarationGoodsShipmentAdditionalDocument);
                }
                //}
            }
            return declarationGoodsShipmentAdditionalDocumentList.ToArray();
        }


        private string ResolveFromGlobalScannedAttachmentToEntityOperation380()
        {
            return "90019251";
        }


        private DeclarationGoodsShipmentInvoice GetDeclarationGoodsShipmentInvoice(SupplierInvoicePM supplierInvoicePM)
        {
            var declarationGoodsShipmentInvoice = new DeclarationGoodsShipmentInvoice();
            declarationGoodsShipmentInvoice.ID = SetIDTypeValue<InvoiceIdentificationIDType>(supplierInvoicePM.InvoiceNumber);
            // moran 18.11.15 - Task 18415 - not to send if no value -->
            //declarationGoodsShipmentInvoice.IssueDateTime = supplierInvoicePM.IssueDate.HasValue ? DataTypeConvertorUtil.Convert(supplierInvoicePM.IssueDate) : DataTypeConvertorUtil.Convert(DateTime.Now);
            if (supplierInvoicePM.IssueDate.HasValue)
            {
                declarationGoodsShipmentInvoice.IssueDateTime = DataTypeConvertorUtil.Convert(supplierInvoicePM.IssueDate);
            } // <--
            //IssueDateTimeType
            // moran 13.7.14 - Task 6817 - enter into 'if' - cancel hard code -->
            //declarationGoodsShipmentInvoice.TypeCode = SetCodeTypeValue<InvoiceTypeCodeType>(String.IsNullOrWhiteSpace(supplierInvoicePM.AccountTypeCode) ? "380" : supplierInvoicePM.AccountTypeCode); // hard coded
            if (!String.IsNullOrWhiteSpace(supplierInvoicePM.AccountTypeCode))
            {
                declarationGoodsShipmentInvoice.TypeCode = SetCodeTypeValue<InvoiceTypeCodeType>(supplierInvoicePM.AccountTypeCode);
            } // moran 13.7.14 - Task 6817 <--
            declarationGoodsShipmentInvoice.DMExtensions = GetDMExtensionsGoodsShipment(supplierInvoicePM);

            return declarationGoodsShipmentInvoice;
        }


        private DeclarationGoodsShipmentInvoiceDMExtensions GetDMExtensionsGoodsShipment(SupplierInvoicePM supplierInvoicePM)
        {

            var DMExtensions = new DeclarationGoodsShipmentInvoiceDMExtensions();
            DMExtensions.IsPreferenceDocumentInd = new IsPrefarenceDocumentIndType() { Value = supplierInvoicePM.IsPreference };
            if (!String.IsNullOrWhiteSpace(supplierInvoicePM.PreferenceDocumentTypeCode)) //דורית שורר <PrefarenceDocumentType/>   יש לאתחל אותו כ- NULL
            {
                //not valid  <PrefarenceDocumentType/>   
                ///valid <PrefarenceDocumentType xsi:nil="true"/>
                DMExtensions.PreferenceDocumentType = new DeclarationGoodsShipmentInvoiceDMExtensionsPreferenceDocumentType() { Value = supplierInvoicePM.PreferenceDocumentTypeCode };
            }
            //DMExtensions.PaymentType = SetCodeTypeValue<DeclarationGoodsShipmentInvoiceDMExtensionsPaymentType>(supplierInvoicePM.PaymentTypeCode); // new DeclarationGoodsShipmentInvoiceDMExtensionsPaymentType() { Value = supplierInvoicePM.PaymentTypeCode };
            //DMExtensions.InvoiceAmount = new InvoiceAmountType() { Value = supplierInvoicePM.InvoiceAmount.HasValue ? supplierInvoicePM.InvoiceAmount.Value : 0 };
            //DMExtensions.InvoiceAmount = new InvoiceAmountType() {currencyIDSpecified=true,  currencyID = entityPM.USD, Value = supplierInvoicePM.InvoiceAmount.HasValue ? supplierInvoicePM.InvoiceAmount.Value : 0 };
            if (supplierInvoicePM.InvoiceAmount.HasValue && supplierInvoicePM.InvoiceAmount != decimal.Zero)//18202
            {
                DMExtensions.InvoiceAmount = SetAmountTypeValue<InvoiceAmountType>(supplierInvoicePM.InvoiceCurrencyTypeCode, supplierInvoicePM.InvoiceAmount.Value);

            }

            //DMExtensions.InvoiceCurrency // ???

            //if (supplierInvoicePM.ActualPayedAmount.HasValue)
            //{
            //    DMExtensions.ActualPayedAmount = SetAmountTypeValue<ActualPayedAmountType>(supplierInvoicePM.ActualPayedCurrencyTypeCode, supplierInvoicePM.ActualPayedAmount.Value);
            //}
            //DMExtensions.RateNumeric = supplierInvoicePM.ExchangeRate; // moran 18.11.15 - Task 18415 - not to send - commented
            //DMExtensions.RateNumericSpecified = supplierInvoicePM.ExchangeRate != null ? true : false; // moran 18.11.15 - Task 18415 - not to send - commented
            if (supplierInvoicePM.PartyRelationshipCode != null)
                DMExtensions.PartyRelationshipCode = SetCodeTypeValue<DeclarationGoodsShipmentInvoiceDMExtensionsPartyRelationshipCode>(supplierInvoicePM.PartyRelationshipCode);
            if (supplierInvoicePM.SupplierInvoicePayments != null && supplierInvoicePM.SupplierInvoicePayments.Count() > 0)
            {
                List<DeclarationGoodsShipmentInvoiceDMExtensionsPaymentDetails> DeclarationGoodsShipmentInvoiceDMExtensionsPaymentDetails = new List<DeclarationGoodsShipmentInvoiceDMExtensionsPaymentDetails>();
                foreach (var supplierInvoicePayments in supplierInvoicePM.SupplierInvoicePayments)
                {
                    DeclarationGoodsShipmentInvoiceDMExtensionsPaymentDetails declarationGoodsShipmentInvoiceDMExtensionsPaymentDetails = new DeclarationGoodsShipmentInvoiceDMExtensionsPaymentDetails();
                    declarationGoodsShipmentInvoiceDMExtensionsPaymentDetails.SequenceNumeric = supplierInvoicePayments.SequenceNumeric;
                    declarationGoodsShipmentInvoiceDMExtensionsPaymentDetails.PaymentType = SetCodeTypeValue<PaymentType>(supplierInvoicePayments.PaymentTypeCode);
                    declarationGoodsShipmentInvoiceDMExtensionsPaymentDetails.PaymentAmount = new PaymentAmountAmountType() { Value = supplierInvoicePayments.PaymentAmount, currencyID = ISO3AlphaCurrencyCodeContentType.USD, currencyIDSpecified = true };


                    DeclarationGoodsShipmentInvoiceDMExtensionsPaymentDetails.Add(declarationGoodsShipmentInvoiceDMExtensionsPaymentDetails);
                }

                DMExtensions.PaymentDetails = DeclarationGoodsShipmentInvoiceDMExtensionsPaymentDetails.ToArray();
            }


            DMExtensions.BuyerDetails = new DeclarationGoodsShipmentInvoiceDMExtensionsBuyerDetails();

            DMExtensions.BuyerDetails.Name = supplierInvoicePM.BuyerName;
            DMExtensions.BuyerDetails.Address = supplierInvoicePM.BuyerAddress;
            DMExtensions.BuyerDetails.IssueLocation = SetCodeTypeValue<DeclarationGoodsShipmentInvoiceDMExtensionsBuyerDetailsIssueLocation>(supplierInvoicePM.BuyerCountryCode);
            DMExtensions.BuyerDetails.RoleCode = SetCodeTypeValue<DeclarationGoodsShipmentInvoiceDMExtensionsBuyerDetailsRoleCode>(supplierInvoicePM.BuyerRoleCode);
            DMExtensions.CustomsValuation = GetcustomsValuation(supplierInvoicePM).ToArray();
            return DMExtensions;
        }

        private List<DeclarationGoodsShipmentInvoiceDMExtensionsCustomsValuation> GetcustomsValuation(SupplierInvoicePM supplierInvoicePM)
        {
            var customsValuationlist = new List<DeclarationGoodsShipmentInvoiceDMExtensionsCustomsValuation>();
            var sequence = 0;
            for (int modificationsSeq = 0; modificationsSeq < supplierInvoicePM.SupplierInvoiceModifications.Count(); modificationsSeq++)
            {
                var supplierInvoiceModificationPM = supplierInvoicePM.SupplierInvoiceModifications[modificationsSeq];
                if (supplierInvoiceModificationPM.TypeCode != "I02")
                {
                    var customsValuation = new DeclarationGoodsShipmentInvoiceDMExtensionsCustomsValuation();

                    customsValuation.SequenceNumeric = ++sequence;
                    customsValuation.ChargesTypeCode = new DeclarationGoodsShipmentInvoiceDMExtensionsCustomsValuationChargesTypeCode();
                    customsValuation.ChargesTypeCode = SetCodeTypeValue<DeclarationGoodsShipmentInvoiceDMExtensionsCustomsValuationChargesTypeCode>(supplierInvoiceModificationPM.TypeCode);
                    customsValuation.OtherChargeDeductionAmount = new DeclarationGoodsShipmentInvoiceDMExtensionsCustomsValuationOtherChargeDeductionAmount();
                    customsValuation.OtherChargeDeductionAmount = SetAmountTypeValue<DeclarationGoodsShipmentInvoiceDMExtensionsCustomsValuationOtherChargeDeductionAmount>(supplierInvoiceModificationPM.CurrencyTypeCode, supplierInvoiceModificationPM.Amount.GetValueOrDefault());
                    customsValuationlist.Add(customsValuation);
                }
            }
            return customsValuationlist;
        }


        private DeclarationGoodsShipmentGovernmentAgencyGoodsItemGovernmentProcedure[] GetGoodsItemGovernmentProcedure(List<SupplierInvoiceItemProcesTypePM> SupplierInvoiceItemsProcessTypesPM)
        {

            var goodsItemCommodityGovernmentProcedureList = new List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemGovernmentProcedure>();
            foreach (var SupplierInvoiceItemsProcessType in SupplierInvoiceItemsProcessTypesPM)
            {
                var GoodsItemCommodityGovernmentProcedure = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemGovernmentProcedure();
                GoodsItemCommodityGovernmentProcedure.CurrentCode = SetCodeTypeValue<GovernmentProcedureCurrentCodeType>(SupplierInvoiceItemsProcessType.ProcessTypeCode);
                goodsItemCommodityGovernmentProcedureList.Add(GoodsItemCommodityGovernmentProcedure);
            }
            return goodsItemCommodityGovernmentProcedureList.ToArray();
        }

        private DeclarationGoodsShipmentGovernmentAgencyGoodsItemGoodsMeasure[] GetGoodsMeasure(SupplierInvoiceItemPM supplierInvoiceItemPM)
        {
            var goodsMeasureList = new List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemGoodsMeasure>();
            // moran 5.12.13 - Task 2296 -->
            if (supplierInvoiceItemPM.InvoiceQuantity.HasValue)
            {
                var goodsMeasure = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemGoodsMeasure();
                goodsMeasure.DMExtensions = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemGoodsMeasureDMExtensions()
                {
                    MeasureQualifier = SetCodeTypeValue<DeclarationGoodsShipmentGovernmentAgencyGoodsItemGoodsMeasureDMExtensionsMeasureQualifier>("1")
                };
                goodsMeasure.TariffQuantity = SetQuantityTypeValue<GoodsMeasureTariffQuantityType>(supplierInvoiceItemPM.InvoiceQuantityType, supplierInvoiceItemPM.InvoiceQuantity.Value);
                goodsMeasureList.Add(goodsMeasure);
            }
            if (supplierInvoiceItemPM.StatisticQuantity.HasValue)
            {
                var goodsMeasure = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemGoodsMeasure();
                goodsMeasure.DMExtensions = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemGoodsMeasureDMExtensions()
                {
                    MeasureQualifier = SetCodeTypeValue<DeclarationGoodsShipmentGovernmentAgencyGoodsItemGoodsMeasureDMExtensionsMeasureQualifier>("2")
                };
                goodsMeasure.TariffQuantity = SetQuantityTypeValue<GoodsMeasureTariffQuantityType>(supplierInvoiceItemPM.StatisticQuantityType, supplierInvoiceItemPM.StatisticQuantity.Value);
                goodsMeasureList.Add(goodsMeasure);
            }
            if (supplierInvoiceItemPM.AdditionalQuantity.HasValue)
            {
                var goodsMeasure = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemGoodsMeasure();
                goodsMeasure.DMExtensions = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemGoodsMeasureDMExtensions()
                {
                    MeasureQualifier = SetCodeTypeValue<DeclarationGoodsShipmentGovernmentAgencyGoodsItemGoodsMeasureDMExtensionsMeasureQualifier>("3")
                };
                goodsMeasure.TariffQuantity = SetQuantityTypeValue<GoodsMeasureTariffQuantityType>(supplierInvoiceItemPM.AdditionalQuantityType, supplierInvoiceItemPM.AdditionalQuantity.Value);
                goodsMeasureList.Add(goodsMeasure);
            }

            return goodsMeasureList.ToArray();

        }

        private DeclarationGoodsShipmentGovernmentAgencyGoodsItemPreviousDocumentDMExtensions GetItemPreviousDocumentDMExtensions(SupplierInvoiceItemsConDeclarPM supplierInvoiceItemConnectedDeclaration)
        {
            var previousDocumentDMExtensions = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemPreviousDocumentDMExtensions();
            if (supplierInvoiceItemConnectedDeclaration.Quantity.HasValue)
            {// moran 24.7.14 - Task 6817 change from KGM to EA --> // moran 22.12.15 - Task 19549 change from EA to new field
                //previousDocumentDMExtensions.QuantityQuantity = SetQuantityTypeValue<DeclarationGoodsShipmentGovernmentAgencyGoodsItemPreviousDocumentDMExtensionsQuantityQuantity>(MeasurementUnitCommonCodeContentType.KGM.ToString(), supplierInvoiceItemConnectedDeclaration.Quantity.Value); // hard coded KGM
                previousDocumentDMExtensions.QuantityQuantity = SetQuantityTypeValue<DeclarationGoodsShipmentGovernmentAgencyGoodsItemPreviousDocumentDMExtensionsQuantityQuantity>(supplierInvoiceItemConnectedDeclaration.QuantityTypeCode, supplierInvoiceItemConnectedDeclaration.Quantity.Value);
            }
            if (supplierInvoiceItemConnectedDeclaration.InvoiceNumber.HasValue)
            {
                previousDocumentDMExtensions.SequenceNumeric = supplierInvoiceItemConnectedDeclaration.InvoiceNumber.Value;
                previousDocumentDMExtensions.SequenceNumericSpecified = true;
            }
            return previousDocumentDMExtensions;
        }


        private DeclarationGoodsShipmentGovernmentAgencyGoodsItemPreviousDocument[] GetPreviousDocument(SupplierInvoiceItemPM supplierInvoiceItemPM)
        {

            var previousDocumentList = new List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemPreviousDocument>();
            for (int ConnectedDeclarationsSeq = 0; ConnectedDeclarationsSeq < supplierInvoiceItemPM.SupplierInvoiceItemsConDeclars.Count(); ConnectedDeclarationsSeq++)
            {
                var supplierInvoiceItemConnectedDeclaration = supplierInvoiceItemPM.SupplierInvoiceItemsConDeclars[ConnectedDeclarationsSeq];

                var previousDocument = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemPreviousDocument();
                previousDocument.ID = SetIDTypeValue<PreviousDocumentIdentificationIDType>(supplierInvoiceItemConnectedDeclaration.DeclarationNumber);
                if (supplierInvoiceItemConnectedDeclaration.ItemSequence.HasValue)
                {
                    previousDocument.SequenceNumeric = supplierInvoiceItemConnectedDeclaration.ItemSequence.Value; // ConnectedDeclarationsSeq + 1;
                    previousDocument.SequenceNumericSpecified = true;

                }
                previousDocument.TypeCode = SetCodeTypeValue<PreviousDocumentTypeCodeType>(supplierInvoiceItemConnectedDeclaration.DeclarationTypeCode);
                previousDocument.DMExtensions = GetItemPreviousDocumentDMExtensions(supplierInvoiceItemConnectedDeclaration);
                previousDocumentList.Add(previousDocument);
            }
            return previousDocumentList.ToArray();

        }

        private List<DeclarationGoodsShipmentGovernmentAgencyGoodsItem> GetDeclarationGoodsItems(SupplierInvoicePM supplierInvoicePM)
        {

            var declarationGoodsShipmentGovernmentAgencyGoodsItemList = new List<DeclarationGoodsShipmentGovernmentAgencyGoodsItem>();

            for (int goodsItemSeq = 0; goodsItemSeq < supplierInvoicePM.SupplierInvoiceItems.Count(); goodsItemSeq++)
            {
                var supplierInvoiceItemPM = supplierInvoicePM.SupplierInvoiceItems[goodsItemSeq];
                var declarationGoodsShipmentGovernmentAgencyGoodsItem = new DeclarationGoodsShipmentGovernmentAgencyGoodsItem();
                declarationGoodsShipmentGovernmentAgencyGoodsItem.SequenceNumeric = supplierInvoiceItemPM.SequenceNumeric.Value; // moran 23.8.16 - changed from goodsItemSeq + 1;
                                                                                                                                 // declarationGoodsShipmentGovernmentAgencyGoodsItem.SequenceNumericSpecified = true;
                declarationGoodsShipmentGovernmentAgencyGoodsItem.Origin = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemOrigin()
                {
                    CountryCode = new OriginCountryCodeType() { Value = supplierInvoiceItemPM.OriginCountryCode }
                };
                declarationGoodsShipmentGovernmentAgencyGoodsItem.GovernmentProcedure = GetGoodsItemGovernmentProcedure(supplierInvoiceItemPM.SupplierInvoiceItemProcesTypes);
                declarationGoodsShipmentGovernmentAgencyGoodsItem.Commodity = GetGoodsItemCommodity(supplierInvoiceItemPM);
                declarationGoodsShipmentGovernmentAgencyGoodsItem.GoodsMeasure = GetGoodsMeasure(supplierInvoiceItemPM); // MUST

                declarationGoodsShipmentGovernmentAgencyGoodsItem.PreviousDocument = GetPreviousDocument(supplierInvoiceItemPM);
                //if (!String.IsNullOrWhiteSpace(supplierInvoiceItemPM.ManufactureIdentifier))
                //{
                //   // declarationGoodsShipmentGovernmentAgencyGoodsItem.Manufacturer = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemManufacturer()
                //    {
                //        ID = SetIDTypeValue<ManufacturerIdentificationIDType>(supplierInvoiceItemPM.ManufactureIdentifier) // new ManufacturerIdentificationIDType() { Value = supplierInvoiceItemPM.ManufactureIdentifier }
                //    };
                //}

                declarationGoodsShipmentGovernmentAgencyGoodsItem.DMExtensions = GetDMExtensionsGoodsItem(supplierInvoiceItemPM, supplierInvoicePM);
                declarationGoodsShipmentGovernmentAgencyGoodsItem.AdditionalDocument = GetGoodsItemAdditionalDocument(supplierInvoiceItemPM);
                //  declarationGoodsShipmentGovernmentAgencyGoodsItem.ValuationAdjustment = GetGoodsItemValuationAdjustment(supplierInvoiceItemPM);


                declarationGoodsShipmentGovernmentAgencyGoodsItemList.Add(declarationGoodsShipmentGovernmentAgencyGoodsItem);
            }
            return declarationGoodsShipmentGovernmentAgencyGoodsItemList;
        }

        private DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsValuationAdjustment[] GetGoodsItemValuationAdjustment(SupplierInvoiceItemPM supplierInvoiceItemPM)
        {
            if (supplierInvoiceItemPM.SupplierInvoiceItemsMods == null)
            {
                return null;
            }
            var valuationAdjustmentList = new List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsValuationAdjustment>();
            foreach (var valuationAdjustmentItem in supplierInvoiceItemPM.SupplierInvoiceItemsMods)
            {
                var valuationAdjustment = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsValuationAdjustment();
                valuationAdjustment.AdditionCode = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsValuationAdjustmentAdditionCode();
                valuationAdjustment.AdditionCode.Value = valuationAdjustmentItem.TypeCode;
                valuationAdjustment.AmountAmount = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsValuationAdjustmentAmountAmount();
                valuationAdjustment.AmountAmount = SetAmountTypeValue<DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsValuationAdjustmentAmountAmount>(valuationAdjustmentItem.CurrencyTypeCode, valuationAdjustmentItem.Amount > 0 ? (Decimal)valuationAdjustmentItem.Amount : 0);
                //  valuationAdjustment.SequenceNumeric = valuationAdjustmentItem.
                valuationAdjustmentList.Add(valuationAdjustment);
            }
            return valuationAdjustmentList.ToArray();
        }

        private DeclarationGoodsShipmentGovernmentAgencyGoodsItemAdditionalDocument[] GetGoodsItemAdditionalDocument(SupplierInvoiceItemPM supplierInvoiceItemPM)
        {
            var goodsItemAdditionalDocumentList = new List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemAdditionalDocument>();
            //CustomsDocumentsTicketId adjustment mohammad 18.10.14
            //var customsDocumentPointerQueryService = new CustomsDocumentPointerQueryService(_context);
            //var customsDocumentPointerPMList = new List<CustomsDocumentPointerPM>();

            //Get supplier Item Document - From CustomsDocumentPointer Table
            //customsDocumentPointerPMList = customsDocumentPointerQueryService.GetParentDocumentPointer(supplierInvoiceItemPM.DeclarationId, "Declaration", supplierInvoiceItemPM.Tenant);

            //var customsDocumentsTicketQueryService = new CustomsDocumentsTicketQueryService(_context);
            //var customsDocumentsTicketPMList = customsDocumentsTicketQueryService.GetCustomsDocumentsTickets(new GetTicketsParams() { ParentEntityId = supplierInvoiceItemPM.DeclarationId, ParentEntityCode = "Declaration", Child1EntityCode = "SupplierInvoice", Child1EntityId = supplierInvoiceItemPM.CounterKey.ToString(), Child2EntityCode = "SupplierInvoiceItem", Child2EntityId = supplierInvoiceItemPM.LineNumber.ToString() }, supplierInvoiceItemPM.Tenant);

            var customsDocumentQueryService = new CustomsDocumentQueryService(_context);
            var customsDocumentPMList = customsDocumentQueryService.GetCustomsDocumentPMListWithoutRequestedDoc(new GetTicketsParams() { ParentEntityId = supplierInvoiceItemPM.DeclarationId, ParentEntityCode = "Declaration", Child1EntityCode = "SupplierInvoice", Child1EntityId = supplierInvoiceItemPM.CounterKey.ToString(), Child2EntityCode = "SupplierInvoiceItem", Child2EntityId = supplierInvoiceItemPM.LineNumber.ToString() }, supplierInvoiceItemPM.Tenant);
            foreach (var customsDocumentPM in customsDocumentPMList)
            {
                //if (documentTicketItem.Child1EntityCode == "SupplierInvoice" && documentTicketItem.Child1EntityId == supplierInvoiceItemPM.CounterKey.ToString() && documentTicketItem.Child2EntityCode == "SupplierInvoiceItem" && documentTicketItem.Child2EntityId == supplierInvoiceItemPM.LineNumber.ToString())
                //{
                //if (documentTicketItem.Child3EntityId == null && documentTicketItem.Child3EntityCode == null && documentTicketItem.CustomDocumentId != null)
                //{
                if (!string.IsNullOrWhiteSpace(customsDocumentPM.CustomsDocId)) // Mirit 22/12/15 19136
                {
                    var declarationGoodsShipmentAdditionalDocument = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemAdditionalDocument();
                    declarationGoodsShipmentAdditionalDocument.TypeCode = new AdditionalDocumentTypeCodeType();
                    declarationGoodsShipmentAdditionalDocument.TypeCode.Value = "3";
                    declarationGoodsShipmentAdditionalDocument.DMExtensions = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemAdditionalDocumentDMExtensions();
                    declarationGoodsShipmentAdditionalDocument.DMExtensions.ExternalAttachmentID = new ExternalAttachmentIDType();
                    declarationGoodsShipmentAdditionalDocument.DMExtensions.ExternalAttachmentID.Value = customsDocumentPM.ExternalAttachmentId; //customsDocumentPM.DocumentsFilingId; // From CustomsDocumentPointers (Logitude Filling)

                    goodsItemAdditionalDocumentList.Add(declarationGoodsShipmentAdditionalDocument);
                }
                //}
            }

            //Get supplier Item Certificate - From SupplierInvioceItemsCertificates Table
            foreach (var CertificateItem in supplierInvoiceItemPM.SupplierInvioceItemCertificats)
            {
                if (!(string.IsNullOrWhiteSpace(CertificateItem.ResConfirmationTypeCode) && string.IsNullOrWhiteSpace(CertificateItem.CertificateNumber) && string.IsNullOrWhiteSpace(CertificateItem.CertificateExemptionTypeCode) && string.IsNullOrWhiteSpace(CertificateItem.AttachmentTypeCode) && string.IsNullOrWhiteSpace(CertificateItem.CustomsAttachmentID)))
                { // moran 26.9.16 - Task 22961 - enter into 'if' fields are empty
                    var declarationGoodsShipmentAdditionalDocument = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemAdditionalDocument();
                    declarationGoodsShipmentAdditionalDocument.ID = SetIDTypeValue<AdditionalDocumentIdentificationIDType>(CertificateItem.CertificateNumber);
                    declarationGoodsShipmentAdditionalDocument.LPCOExemptionCode = SetCodeTypeValue<AdditionalDocumentLPCOExemptionCodeType>(CertificateItem.CertificateExemptionTypeCode);
                    declarationGoodsShipmentAdditionalDocument.TypeCode = SetCodeTypeValue<AdditionalDocumentTypeCodeType>(CertificateItem.AttachmentTypeCode);
                    declarationGoodsShipmentAdditionalDocument.DMExtensions = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemAdditionalDocumentDMExtensions();
                    //mirit20131222 declarationGoodsShipmentAdditionalDocument.DMExtensions.AttachmentID = SetIDTypeValue<AttachmentIDType>(CertificateItem.CustomsAttachmentID);
                    declarationGoodsShipmentAdditionalDocument.DMExtensions.LPCOTypeCode = SetCodeTypeValue<LpcoTypeCodeType>(CertificateItem.ResConfirmationTypeCode);
                    // declarationGoodsShipmentAdditionalDocument.DMExtensions.requirementLicenseType = SetCodeTypeValue<DeclarationGoodsShipmentGovernmentAgencyGoodsItemAdditionalDocumentDMExtensionsRequirementLicenseType>(CertificateItem.ReqConfirmationTypeCode);
                    declarationGoodsShipmentAdditionalDocument.DMExtensions.ExternalAttachmentID = SetIDTypeValue<ExternalAttachmentIDType>(CertificateItem.CustomsAttachmentID);
                    declarationGoodsShipmentAdditionalDocument.DMExtensions.SequenceNumeric = CertificateItem.SequenceNumeric; // moran 1.8.16 - Task 21933
                                                                                                                               //   declarationGoodsShipmentAdditionalDocument.DMExtensions.SequenceNumericSpecified = true; // moran 8.8.16 - Task 21933
                    goodsItemAdditionalDocumentList.Add(declarationGoodsShipmentAdditionalDocument);
                }
            }

            return goodsItemAdditionalDocumentList.ToArray();
        }
        
        private DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodity GetGoodsItemCommodity(SupplierInvoiceItemPM supplierInvoiceItemPM)
        {
            decimal sequenceNumeric;
            decimal.TryParse(supplierInvoiceItemPM.ActualInvoiceLines, out sequenceNumeric);
            DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassification myDeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassification = null;

            if (!String.IsNullOrWhiteSpace(supplierInvoiceItemPM.DangerousClassificationCode))
            {
                if (supplierInvoiceItemPM.DangerousClassificationCode.Length == 11) // moran 1.9.14 - uncommented
                {
                    supplierInvoiceItemPM.DangerousClassificationCode = supplierInvoiceItemPM.DangerousClassificationCode.Insert(10, "/");
                }
                myDeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassification = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassification()
                {
                    ID = SetIDTypeValue<ClassificationIdentificationIDType>(supplierInvoiceItemPM.DangerousClassificationCode), // new ClassificationIdentificationIDType()
                    //{
                    //    Value = supplierInvoiceItemPM.DangerousClassificationCode
                    //},
                    IdentificationTypeCode = SetCodeTypeValue<ClassificationIdentificationTypeCodeType>("SSO"),

                    //{
                    //    Value = "SSO"
                    //}
                    // moran 12.4.16 - Bug 20650 -->
                    //DMExtensions = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassificationDMExtensions()
                    //{
                    //    DangerousGoodsPackingRequirementsGroupCode = SetCodeTypeValue<DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassificationDMExtensionsDangerousGoodsPackingRequirementsGroupCode>(supplierInvoiceItemPM.DangerousPackingGroupTypeCode)
                    //}
                    // moran 12.4.16 - Bug 20650 <--
                };
            }
            var declarationGoodsItemCommodity = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodity();
            //declarationGoodsItemCommodity.DMExtensions = GetGoodsItemCommodityDMExtensions(supplierInvoiceItemPM);
            // moran 25.5.14 - Bug 6059 - commented -->
            //declarationGoodsItemCommodity.DutyTaxFee = GetGoodsItemCommodityDutyTaxFees(supplierInvoiceItemPM);
            //declarationGoodsItemCommodity.DMExtensions = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensions()
            //{
            //    DutyRegimeCode = new DutyTaxFeeDutyRegimeCodeType() { Value = supplierInvoiceItemPM.TariffCode }
            //};
            if (!string.IsNullOrWhiteSpace(supplierInvoiceItemPM.ClassificationCode))
            {
                if (supplierInvoiceItemPM.ClassificationCode.Length == 11) // moran 1.9.14 - uncommented
                {
                    supplierInvoiceItemPM.ClassificationCode = supplierInvoiceItemPM.ClassificationCode.Insert(10, "/");
                }
                if (string.IsNullOrEmpty(supplierInvoiceItemPM.ClassificationTypeCode)) supplierInvoiceItemPM.ClassificationTypeCode = "HS";
                declarationGoodsItemCommodity.Classification = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassification[]
                {
                    new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassification()
                    {
                        ID = SetIDTypeValue<ClassificationIdentificationIDType>(supplierInvoiceItemPM.ClassificationCode), // new ClassificationIdentificationIDType()
                    // {
                      //   Value = supplierInvoiceItemPM.ClassificationCode
                     //},
                        IdentificationTypeCode = SetCodeTypeValue < ClassificationIdentificationTypeCodeType>(supplierInvoiceItemPM.ClassificationTypeCode),
                        DangerousGoodsStatement = GetDangerousGoodsStatement(supplierInvoiceItemPM.SuppInvoiceItemsAbachStatements),
                      //  TaxExemptCode =SetCodeTypeValue < DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassificationTaxExemptCode>(supplierInvoiceItemPM.TaxExemptCode),
                        ProductName = GetGetDeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsProductName(supplierInvoiceItemPM),
                        ProductIdentification =  GetGetDeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsProductIdentification(supplierInvoiceItemPM),
                        SerialNumbers= GetGetDeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsSerialNumbers(supplierInvoiceItemPM),
                        TradeLevyAndExampt =GetDeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsTradeLevyAndExampt(supplierInvoiceItemPM)

                    },

                    myDeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassification
                };
            }


            return declarationGoodsItemCommodity;
        }

        private DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassificationTradeLevyAndExampt[] GetDeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsTradeLevyAndExampt(SupplierInvoiceItemPM supplierInvoiceItemPM)
        {
            if (supplierInvoiceItemPM.SupplierInvoiceItemLevies == null)
            {
                return null;
            }
            if (supplierInvoiceItemPM.SupplierInvoiceItemLevies.Count < 1)
            {
                return null;
            }

            List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassificationTradeLevyAndExampt> declarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsTradeLevyAndExamptList = new List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassificationTradeLevyAndExampt>();

            foreach (var supplierInvoiceItemLevies in supplierInvoiceItemPM.SupplierInvoiceItemLevies)
            {
                DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassificationTradeLevyAndExampt declarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsTradeLevyAndExampt = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassificationTradeLevyAndExampt();
                if (!string.IsNullOrWhiteSpace(supplierInvoiceItemLevies.TradeLevyExamptCode))
                {
                    declarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsTradeLevyAndExampt.TradeLevyExamptCode = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassificationTradeLevyAndExamptTradeLevyExamptCode
                    {
                        Value = supplierInvoiceItemLevies.TradeLevyExamptCode
                    };
                }
                if (!string.IsNullOrEmpty(supplierInvoiceItemLevies.TradeLevyNumber)) // changed by Alaa WI:13229
                {
                    declarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsTradeLevyAndExampt.TradeLevyNumber = new TradeLevyNumberType
                    {
                        Value = supplierInvoiceItemLevies.TradeLevyNumber
                    };
                }

                declarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsTradeLevyAndExamptList.Add(declarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsTradeLevyAndExampt);

            }
            return declarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsTradeLevyAndExamptList.ToArray();
        }


        private DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassificationSerialNumbers[] GetGetDeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsSerialNumbers(SupplierInvoiceItemPM supplierInvoiceItemPM)
        {
            if (supplierInvoiceItemPM.SupplierInvoiceItemsSerialNums == null || supplierInvoiceItemPM.SupplierInvoiceItemsSerialNums.Count < 1)
            {
                return null;
            }

            List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassificationSerialNumbers> myDMExtensionsSerialNumbers = new List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassificationSerialNumbers>();
            foreach (var serialNumbersItem in supplierInvoiceItemPM.SupplierInvoiceItemsSerialNums)
            {
                var serialNumber = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassificationSerialNumbers();
                serialNumber.ID = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassificationSerialNumbersID();
                serialNumber.ID.Value = serialNumbersItem.SerialNumber;
                serialNumber.IdentityQualifierCode = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassificationSerialNumbersIdentityQualifierCode();
                serialNumber.IdentityQualifierCode.Value = serialNumbersItem.TypeCode;
                myDMExtensionsSerialNumbers.Add(serialNumber);
            }

            return myDMExtensionsSerialNumbers.ToArray(); ;
        }

        private DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassificationProductName[] GetGetDeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsProductName(SupplierInvoiceItemPM supplierInvoiceItemPM)
        {
            if (supplierInvoiceItemPM.SupplierInvoiceItemsDescripts == null || supplierInvoiceItemPM.SupplierInvoiceItemsDescripts.Count < 1)
            {
                return null;
            }

            List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassificationProductName> myDMExtensionsProductName = new List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassificationProductName>();
            foreach (var productNameItem in supplierInvoiceItemPM.SupplierInvoiceItemsDescripts)
            {
                var productName = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassificationProductName();
                productName.Name = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassificationProductNameName();
                productName.Name.Value = productNameItem.Description;
                productName.NameQualifierCode = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassificationProductNameNameQualifierCode();
                productName.NameQualifierCode.Value = productNameItem.TypeCode;
                myDMExtensionsProductName.Add(productName);
            }

            return myDMExtensionsProductName.ToArray(); ;
        }

        private DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassificationProductIdentification[] GetGetDeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsProductIdentification(SupplierInvoiceItemPM supplierInvoiceItemPM)
        {
            if (supplierInvoiceItemPM.SupplierInvoiceItemsProdIdents == null || supplierInvoiceItemPM.SupplierInvoiceItemsProdIdents.Count < 1)
            {
                return null;
            }

            List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassificationProductIdentification> myDMExtensionsProductIdentification = new List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassificationProductIdentification>();
            foreach (var productIdentificationItem in supplierInvoiceItemPM.SupplierInvoiceItemsProdIdents)
            {
                var productIdentification = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassificationProductIdentification();
                productIdentification.ID = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassificationProductIdentificationID();
                productIdentification.ID.Value = productIdentificationItem.Identification;
                productIdentification.IDTypeCode = new CommodityIDTypeCodeType();
                productIdentification.IDTypeCode.Value = productIdentificationItem.TypeCode;
                myDMExtensionsProductIdentification.Add(productIdentification);
            }

            return myDMExtensionsProductIdentification.ToArray(); ;
        }

        private DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassificationDangerousGoodsStatement[] GetDangerousGoodsStatement(List<SuppInvoiceItemsAbachStatementPM> suppInvoiceItemsAbachStatements)
        {
            if (suppInvoiceItemsAbachStatements == null || suppInvoiceItemsAbachStatements.Count() == 0) return null;
            List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassificationDangerousGoodsStatement> dangerousGoodsStatements = new List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassificationDangerousGoodsStatement>();

            foreach (var suppInvoiceItemsAbachStatement in suppInvoiceItemsAbachStatements)
            {
                DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassificationDangerousGoodsStatement dangerousGoodsStatement = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassificationDangerousGoodsStatement();
                dangerousGoodsStatement.SequenceNumeric = Convert.ToInt32(suppInvoiceItemsAbachStatement.SequenceNumeric);
                dangerousGoodsStatement.StatementType = SetIDTypeValue<DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassificationDangerousGoodsStatementStatementType>(suppInvoiceItemsAbachStatement.StatementTypeCode);
                dangerousGoodsStatement.DangerousGoodsStatementInd = new DangerousGoodsStatementIndType() { Value = suppInvoiceItemsAbachStatement.IsStatementInd }; //change to StatementInd field 
                dangerousGoodsStatements.Add(dangerousGoodsStatement);
            }
            return dangerousGoodsStatements.ToArray();
        }

        private DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsGoodsItemAmount GetDeclarationGoodsItemAmount(decimal ItemPricePM, string ItemPriceTypePM, string ItemPriceCurrencyPM)
        {
            var declarationGoodsItemAmount = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsGoodsItemAmount();
            declarationGoodsItemAmount.CustomsValueAmount = SetAmountTypeValue<DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsGoodsItemAmountCustomsValueAmount>(ItemPriceCurrencyPM, ItemPricePM);
            //declarationGoodsItemAmount.CustomsValueAmount = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsGoodsItemAmountCustomsValueAmount()
            //{
            //    Value =ItemPricePM,
            //    currencyIDSpecified=true,
            //    currencyID = entityPM.USD
            // };
            declarationGoodsItemAmount.AmountType = SetCodeTypeValue<DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsGoodsItemAmountAmountType>(ItemPriceTypePM);

            // declarationGoodsItemAmount.AmountType = new CodeAmountType()
            // {
            //     Value = ItemPriceTypePM
            // };
            return declarationGoodsItemAmount;
        }

        private DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensions GetDMExtensionsGoodsItem(SupplierInvoiceItemPM supplierInvoiceItemPM, SupplierInvoicePM supplierInvoicePM)
        {
            var DMExtensions = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensions();
            var declarationGoodsItemAmountList = new List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsGoodsItemAmount>();
            string cur = supplierInvoicePM.InvoiceCurrencyTypeCode;

            if (!String.IsNullOrWhiteSpace(cur))
            {
                if (supplierInvoiceItemPM.SupplierInvoiceItemsPrices != null && supplierInvoiceItemPM.SupplierInvoiceItemsPrices.Count() > 0)
                {
                    foreach (var price in supplierInvoiceItemPM.SupplierInvoiceItemsPrices)
                    {
                        if (price.AdditionalPrice != null)
                            declarationGoodsItemAmountList.Add(GetDeclarationGoodsItemAmount(Convert.ToDecimal(price.AdditionalPrice), price.AdditionalPriceTypeCode, cur));

                    }
                }

                if (supplierInvoiceItemPM.ItemPrice.HasValue)
                {

                    declarationGoodsItemAmountList.Add(GetDeclarationGoodsItemAmount(supplierInvoiceItemPM.ItemPrice.Value, "1", cur));

                }
            }

            //if (supplierInvoiceItemPM.ItemPrice.HasValue)
            //{
            //    if (!String.IsNullOrWhiteSpace(supplierInvoiceItemPM.ItemPriceCurrencyCode))
            //    {
            //        declarationGoodsItemAmountList.Add(GetDeclarationGoodsItemAmount(supplierInvoiceItemPM.ItemPrice.Value, "1", supplierInvoiceItemPM.ItemPriceCurrencyCode));
            //    }
            //    else if (!String.IsNullOrWhiteSpace(cur))
            //    {
            //        declarationGoodsItemAmountList.Add(GetDeclarationGoodsItemAmount(supplierInvoiceItemPM.ItemPrice.Value, "1", cur));
            //    }
            //}
            //if (supplierInvoiceItemPM.NonCustomsItemPrice.HasValue && supplierInvoiceItemPM.NonCustomsItemPrice != decimal.Zero && !String.IsNullOrWhiteSpace(supplierInvoiceItemPM.NonCustomsItemPriceCurCode))//17997
            //{

            //    declarationGoodsItemAmountList.Add(GetDeclarationGoodsItemAmount(supplierInvoiceItemPM.NonCustomsItemPrice.Value, "11", supplierInvoiceItemPM.NonCustomsItemPriceCurCode));

            //}
            //if (supplierInvoiceItemPM.WholeSaleItemPrice.HasValue && supplierInvoiceItemPM.WholeSaleItemPrice != decimal.Zero && !String.IsNullOrWhiteSpace(supplierInvoiceItemPM.WholeSaleItemPriceCurrencyCode))//17997
            //{

            //    declarationGoodsItemAmountList.Add(GetDeclarationGoodsItemAmount(supplierInvoiceItemPM.WholeSaleItemPrice.Value, "5", supplierInvoiceItemPM.WholeSaleItemPriceCurrencyCode));

            //}
            DMExtensions.GoodsItemAmount = declarationGoodsItemAmountList.ToArray();

            if (!String.IsNullOrWhiteSpace(supplierInvoiceItemPM.TaxExemptCode))
            {
                if (supplierInvoiceItemPM.TaxExemptCode.Length == 12)
                {
                    supplierInvoiceItemPM.TaxExemptCode = supplierInvoiceItemPM.TaxExemptCode.Insert(11, "/");
                }
                if (supplierInvoiceItemPM.TaxExemptCode.Length == 11)
                {
                    supplierInvoiceItemPM.TaxExemptCode = supplierInvoiceItemPM.TaxExemptCode.Insert(10, "/");
                }
                //SetCodeTypeValue<DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsTaxExemptCode>(String.IsNullOrWhiteSpace(supplierInvoiceItemPM.TaxExemptCode) ? "1" : supplierInvoiceItemPM.TaxExemptCode)
            }

            DMExtensions.Vehicle = GetDeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification(supplierInvoiceItemPM.SupplierInvoiceItemVehicles); // Mirit 16/08/15 Task 15960
                                                                                                                                                                             // if (!String.IsNullOrWhiteSpace(supplierInvoiceItemPM.PreferenceDocumentNumber)) // moran 9.3.15 - Task 11774
                                                                                                                                                                             //SetIDTypeValue<PreferenceDocumentNumberType>("11"); //                                                                                                                                                            // SetIDTypeValue<PreferenceDocumentNumberType>("11"); //
            DMExtensions.PreferenceDocumentNumber = SetIDTypeValue<PreferenceDocumentNumberType>(supplierInvoiceItemPM.PreferenceDocumentNumber);
            DMExtensions.InvoiceLineNumbers = "1";// supplierInvoiceItemPM.ActualInvoiceLines;
            DMExtensions.TransactionNatureCode = SetCodeTypeValue<DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsTransactionNatureCode>(supplierInvoiceItemPM.TransactionNatureCode);
            DMExtensions.ClaimReasonCode = SetCodeTypeValue<DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsClaimReasonCode>(supplierInvoiceItemPM.ClaimReasonCode);
            DMExtensions.ValuationAdjustment = GetGoodsItemValuationAdjustment(supplierInvoiceItemPM);
            return DMExtensions;
        }

        private DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification[] GetDeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification(List<SupplierInvoiceItemVehiclePM> supplierInvoiceItemVehiclesList)
        {
            if (supplierInvoiceItemVehiclesList == null || supplierInvoiceItemVehiclesList.Count < 1)
            {
                return null;
            }

            List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification> mySupplierInvoiceItemVehiclesList = new List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification>();
            foreach (var supplierInvoiceItemVehicleItem in supplierInvoiceItemVehiclesList)
            {
                if (!supplierInvoiceItemVehicleItem.ExcludeFromInterface)
                {
                    var vehicleDetails = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification();
                    vehicleDetails.ID = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentificationID();
                    vehicleDetails.IDTypeCode = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentificationIDTypeCode();
                    if (!string.IsNullOrWhiteSpace(supplierInvoiceItemVehicleItem.RichbitFileNumber))
                    {
                        vehicleDetails.ID.Value = supplierInvoiceItemVehicleItem.RichbitFileNumber;
                        vehicleDetails.IDTypeCode.Value = supplierInvoiceItemVehicleItem.VehicleTypeCode; // "ZZZ"; // moran 28.1.16 - Bug 19966 - change to take from DB instead of constant
                    }
                    else if (!string.IsNullOrWhiteSpace(supplierInvoiceItemVehicleItem.VehicleChassisNumber))
                    {
                        vehicleDetails.ID.Value = supplierInvoiceItemVehicleItem.VehicleChassisNumber;
                        vehicleDetails.IDTypeCode.Value = supplierInvoiceItemVehicleItem.VehicleTypeCode; // "CN"; // moran 28.1.16 - Bug 19966 - change to take from DB instead of constant
                    }

                    mySupplierInvoiceItemVehiclesList.Add(vehicleDetails);
                }
            }

            return mySupplierInvoiceItemVehiclesList.ToArray(); ;


        }

        private List<DeclarationGoodsShipmentExportConsignmentDMExtensionsPackagesMeasure> GetDeclarationConsignmentPackages(ConsignmentPM consignmentPM)
        {
            /*//<--- HARD CODED
            var declarationConsignmentPackageList = new List<DeclarationGoodsShipmentConsignmentDMExtensionsPackagesMeasure>();

            var declarationConsignmentPackage = new DeclarationGoodsShipmentConsignmentDMExtensionsPackagesMeasure();
            declarationConsignmentPackage.SequenceNumeric = 1;
            declarationConsignmentPackage.SequenceNumericSpecified = true;
            declarationConsignmentPackage.PackageMeasureQualifier = new PackageMeasureQualifierType() { Value = "2"}; //HARDCODED
            declarationConsignmentPackage.TotalPackageQuantity = new DeclarationGoodsShipmentConsignmentDMExtensionsPackagesMeasureTotalPackageQuantity() { Value = 100.00M };
            declarationConsignmentPackage.GrossMassMeasure = new DeclarationGoodsShipmentConsignmentDMExtensionsPackagesMeasureGrossMassMeasure()
            {
                Value = 10.00M,
                unitCode = MeasurementUnitCommonCodeContentType.KGM,
                unitCodeSpecified = true
            };
            declarationConsignmentPackage.TypeCode = new DeclarationGoodsShipmentConsignmentDMExtensionsPackagesMeasureTypeCode() { Value = "UN" };
            declarationConsignmentPackage.MarksNumbers = new DeclarationGoodsShipmentConsignmentDMExtensionsPackagesMeasureMarksNumbers() { Value = "MarksNumbers" };




            declarationConsignmentPackageList.Add(declarationConsignmentPackage);

            return declarationConsignmentPackageList;
            //<--- HARD CODED */

            var declarationConsignmentPackageList = new List<DeclarationGoodsShipmentExportConsignmentDMExtensionsPackagesMeasure>();

            for (int consignmentPackageSeq = 0; consignmentPackageSeq < consignmentPM.ConsignmentPackages.Count(); consignmentPackageSeq++)
            {
                var consignmentPackagePM = consignmentPM.ConsignmentPackages[consignmentPackageSeq];

                var declarationConsignmentPackage = new DeclarationGoodsShipmentExportConsignmentDMExtensionsPackagesMeasure();
                declarationConsignmentPackage.SequenceNumeric = consignmentPackageSeq + 1;
                //declarationConsignmentPackage.SequenceNumericSpecified = true;
                //declarationConsignmentPackage.PackageMeasureQualifier = new PackageMeasureQualifierType() { Value = consignmentPackagePM.PackageMeasureQualifierCode };
                declarationConsignmentPackage.PackageMeasureQualifier = SetCodeTypeValue<DeclarationGoodsShipmentExportConsignmentDMExtensionsPackagesMeasurePackageMeasureQualifier>(consignmentPackagePM.PackageMeasureQualifierCode);
                if (consignmentPackagePM.PackageQuantity.HasValue)
                {// moran 5.1.16 - Task 19549 - change to EA hard coded instead of ""
                    declarationConsignmentPackage.TotalPackageQuantity = SetQuantityTypeValue<ConsignmentTotalPackageQuantityType>(consignmentPackagePM.PackageQuantityTypeCode, consignmentPackagePM.PackageQuantity.Value); // hard coded null - mapping missing  // MeasurementUnitCommonCodeContentType.EA.ToString()
                }
                //declarationConsignmentPackage.TotalPackageQuantity = new DeclarationGoodsShipmentConsignmentDMExtensionsPackagesMeasureTotalPackageQuantity() { Value = (Decimal)consignmentPackagePM.PackageQuantity };
                if (consignmentPackagePM.GrossMassMeasure.HasValue)
                {
                    declarationConsignmentPackage.GrossMassMeasure = SetMeasureTypeValue<DeclarationGoodsShipmentExportConsignmentDMExtensionsPackagesMeasureGrossMassMeasure>(consignmentPackagePM.GrossMassMeasureTypeCode, consignmentPackagePM.GrossMassMeasure.Value);
                }

                declarationConsignmentPackage.TypeCode = new DeclarationGoodsShipmentExportConsignmentDMExtensionsPackagesMeasureTypeCode() { Value = consignmentPackagePM.PackageTypeCode };
                declarationConsignmentPackage.MarksNumbers = new DeclarationGoodsShipmentExportConsignmentDMExtensionsPackagesMeasureMarksNumbers() { Value = consignmentPackagePM.MarksNumbers };

                declarationConsignmentPackageList.Add(declarationConsignmentPackage);
            }

            return declarationConsignmentPackageList;
        }




        private DeclarationGoodsShipmentExportConsignmentDMExtensions GetDMExtensionsConsignment(ConsignmentPM consignmentPM)
        {
            var DMExtensions = new DeclarationGoodsShipmentExportConsignmentDMExtensions();
            DMExtensions.CargoDescription = new DeclarationGoodsShipmentExportConsignmentDMExtensionsCargoDescription() { Value = consignmentPM.CargoDescription };
            //DMExtensions.LastReleaseFromWarehousInd = new LastReleaseFromWarehousIndType() { Value = consignmentPM.IsLastReleaseFromWarehous };
            //if (consignmentPM.IsLastReleaseFromWarehous == "T") // temporary treatment - Task 9683
            //{
            //    //mohammad temp treatment due to the change of task 9684
            //    DMExtensions.LastReleaseFromWarehousInd = new LastReleaseFromWarehousIndType() { Value = true };//consignmentPM.IsLastReleaseFromWarehous 
            //}
            //else if (consignmentPM.IsLastReleaseFromWarehous == "F") // moran 9.3.15 - Task 11761 
            //{
            //    DMExtensions.LastReleaseFromWarehousInd = new LastReleaseFromWarehousIndType() { Value = false };
            //}

            //DMExtensions.ExportationCountryCode = new DeclarationGoodsShipmentConsignmentDMExtensionsExportationCountryCode() { Value = consignmentPM.OriginCountryCode };

            var registeredFacilitylist = new List<DeclarationGoodsShipmentExportConsignmentDMExtensionsRegisteredFacility>();
            int seqnum = 0;

            if (!String.IsNullOrWhiteSpace(consignmentPM.StorageSiteCode))
            {
                seqnum++;
                registeredFacilitylist.Add(GetExportRegisteredFacility(consignmentPM.StorageSiteCode, "004", seqnum));
            }
            if (!String.IsNullOrWhiteSpace(consignmentPM.ExportRecieverWareHouseCode))
            {
                seqnum++;
                registeredFacilitylist.Add(GetExportRegisteredFacility(consignmentPM.ExportRecieverWareHouseCode, "006", seqnum));
            }
            if (consignmentPM.ConsignmentInternalTransitions != null)
            {
                if (consignmentPM.ConsignmentInternalTransitions.FirstOrDefault() != null)
                {
                    if (!String.IsNullOrWhiteSpace(consignmentPM.ConsignmentInternalTransitions.FirstOrDefault().SiteCode))
                    {
                        seqnum++;
                        registeredFacilitylist.Add(GetExportRegisteredFacility(consignmentPM.ConsignmentInternalTransitions.FirstOrDefault().SiteCode, "005", seqnum));
                    }
                }
            }
            if (seqnum > 0)
            {
                DMExtensions.RegisteredFacility = registeredFacilitylist.ToArray();
            }

            DMExtensions.PackagesMeasure = GetDeclarationConsignmentPackages(consignmentPM).ToArray();
            DMExtensions.DangerousGoodsIndicator = new DangerousGoodsIndicatorIndType() { Value = consignmentPM.IsDangerousGoods };
            DMExtensions.FinalDestinationPort = new DeclarationGoodsShipmentExportConsignmentDMExtensionsFinalDestinationPort()
            {
                Value = consignmentPM.FinalDestinationPortCode
            };
            DMExtensions.ShipID = new SeaTransportationIDType()
            {
                Value = consignmentPM.ShipCode
            };
            return DMExtensions;
        }

        private DeclarationGoodsShipmentExportConsignmentDMExtensionsRegisteredFacility GetExportRegisteredFacility(string p1, string p2, int seqnum)
        {
            var registeredFacility = new DeclarationGoodsShipmentExportConsignmentDMExtensionsRegisteredFacility();
            //registeredFacility.ID = SetIDTypeValue<DeclarationGoodsShipmentConsignmentDMExtensionsRegisteredFacilityID>(p1);
            registeredFacility.ID = new DeclarationGoodsShipmentExportConsignmentDMExtensionsRegisteredFacilityID() { Value = p1 };
            registeredFacility.FacilityType = new DeclarationGoodsShipmentExportConsignmentDMExtensionsRegisteredFacilityFacilityType() { Value = p2 };
            registeredFacility.SequenceNumeric = seqnum;

            //  registeredFacility.SequenceNumericSpecified = true;

            return registeredFacility;
        }

    }
}