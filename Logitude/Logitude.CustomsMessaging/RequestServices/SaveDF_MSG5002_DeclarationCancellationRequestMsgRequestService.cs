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
using UnifreightIIG.Common.DeclarationCancellationRequestMsgServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class SaveDF_MSG5002_DeclarationCancellationRequestMsgRequestService : RequestServiceBase<DF_NG_5002_MSG14001_DeclarationCancellationRequestMsg, GenericRequestParams>
    {
        private ICustomContext _context;
        private DeclarationPM _DeclarationPM;
 
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
 

        public override void PostGetRequest(DF_NG_5002_MSG14001_DeclarationCancellationRequestMsg customRequest, GenericRequestParams requestParams)
        {
            if (this._context == null)
            {
                this._context = CustomContext.GetContext(requestParams.Tenant);
            }
        }
 
        public override DF_NG_5002_MSG14001_DeclarationCancellationRequestMsg GetRequest(GenericRequestParams requestParams)
        {

            _context = CustomContext.GetContext(requestParams.Tenant);
            LogMessagingUtil.Instance.AppendLine("GetRequest:requestParams.RequestVIA = " + requestParams.RequestVIA.ToString());
           LogMessagingUtil.Instance.AppendLine("GetRequest:requestParams.RequestVIAChangeDue = " + requestParams.RequestVIAChangeDue);

            DeclarationQueryService declarationQueryService = new DeclarationQueryService(requestParams.Tenant);
            DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(_context, new Dictionary<string, IContext>(), requestParams.Tenant);

            var dec = declarationQueryService.GetSingle(requestParams.AppicationId,false,false);
            if(dec==null)
            {
                LogMessagingUtil.Instance.AppendLine("GetRequest:Decalartion not found = " + requestParams.AppicationId);

                return null;

            }

            DF_NG_5002_MSG14001_DeclarationCancellationRequestMsg req = new DF_NG_5002_MSG14001_DeclarationCancellationRequestMsg();
            req.GeneralData = new DF_NG_5002_MSG14001_DeclarationCancellationRequestMsgGeneralData();
            dec.CancelRequestNumber = GetNextCancelRequestNumber(dec);

            req.GeneralData.FunctionalReferenceID = Convert.ToInt32(dec.CancelRequestNumber);
            req.GeneralData.DeclarationID = dec.DeclarationNumber;
            req.GeneralData.DeclarationType = 1;
            req.GeneralData.CancellationReasonTypeId = Convert.ToInt32(dec.CancelRequestReasonCode);
            req.GeneralData.AgentCancellationRemarks = dec.CancelRequestReasonExplanation;
            req.Attachment = GetAttachments(dec);
                //new Attachment[1];
            //req.Attachment[0] = new Attachment() { IsAttachment = "false" , externalAttachmentID= "IIG-227-1" };
            dec.ChangeSetOp = ChangeSetOperation.Update;
            declarationUpdateService.Update(dec, true);

            var amitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
            {
                Tenant = dec.Tenant,
                objectTableName = "Customs.Declaration",
                EventCode = "CPO",
                notes = null,
                CommunicationLoggingEntityReference = dec.DeclarationNumber,
                EntityId = dec.Id,
                UserId = requestParams.LoggingUserId,

                CommunicationSubject = "FU Status CPO from logitude ",
                MyFUStatus = new AmitalEventTracerModel.FUStatus()
                {
                    entname = "CFIFILEM",
                    primary_number = dec.CustomFileNo,
                    status = "new",
                    xml_status = "new",
                    status_id = "CPO",
                    status_DateTime = Convert.ToDateTime(dec.CancelRequestApproveDate),
                    comments = null


                }
            };
            AmitalEventTracer.CreateTraceEvent(amitalEventTracerModel);


            _context = null;
            return req;
        }
        //private Attachment[] GetAttachments(string parentEntityId, int tenant)
        //{
        //    List<Attachment> attachments = new List<Attachment>();



        //    var customsDocumentQueryService = new CustomsDocumentQueryService(tenant);
        //    var customsDocumentPMList = customsDocumentQueryService.GetCustomsDocumentPMListWithoutRequestedDoc(new GetTicketsParams() { ParentEntityId = parentEntityId, ParentEntityCode = "SpecialRequest" }, tenant);

        //    foreach (var customsDocumentPM in customsDocumentPMList)
        //    {
        //        if (!string.IsNullOrWhiteSpace(customsDocumentPM.CustomsDocId))
        //        {
        //            var attachment = new Attachment();
        //            attachment.externalAttachmentID = customsDocumentPM.ExternalAttachmentId;
        //            attachment.IsAttachment = "false";
        //            //   attachment.keywords = customsDocumentPM.Name;
        //            //    attachment.fileName = customsDocumentPM.Name;

        //            //  attachment.documentType = customsDocumentPM.DocumentTypeCode;
        //            attachments.Add(attachment);
        //        }
        //    }
        //    return attachments.ToArray();

        //}

        private Attachment[] GetAttachments(DeclarationPM declaration)
        {
            List<Attachment> attachments = new List<Attachment>();



            var customsDocumentQueryService = new CustomsDocumentQueryService(_context);
            var customsDocumentPMList = customsDocumentQueryService.GetCustomsDocumentPMListWithoutRequestedDoc(new GetTicketsParams() { ParentEntityId = declaration.Id, ParentEntityCode = "DeclarationCancellation" }, declaration.Tenant);

            foreach (var customsDocumentPM in customsDocumentPMList)
            {
                if (!string.IsNullOrWhiteSpace(customsDocumentPM.CustomsDocId))
                {
                    var attachment = new Attachment();
                    attachment.externalAttachmentID = customsDocumentPM.ExternalAttachmentId;
                    attachment.IsAttachment = "false";
                    //attachment.keywords = customsDocumentPM.Name;
                    //attachment.fileName = customsDocumentPM.Name;

                   // attachment.documentType = customsDocumentPM.DocumentTypeCode;
                    attachments.Add(attachment);
                }
            }
            return attachments.ToArray();

        }


        private int GetNextCancelRequestNumber(DeclarationPM declarationPM)
        {
           DeclarationQueryService declarationQueryService = new DeclarationQueryService(declarationPM.Tenant);

            
            return (declarationQueryService.GetDeclarationMaxCancelRequestNumber(declarationPM.Tenant, declarationPM.Id) +1);


         }

  

        
    }
}