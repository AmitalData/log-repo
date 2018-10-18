using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices.DeclarationErrorPointer;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.Storage;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class ST_NG_70_MSG3_StorageResponseFromWarehouseResponseService :
      ResponseServiceBase<INF_MSG_GenericResponseData, ST_NG_70_MSG3_StorageResponseFromWarehouse, GenericRequestParams>
    {
        DeclarationPM _MyDeclarationPM;

        public override INF_MSG_GenericResponseData GetResponse(ST_NG_70_MSG3_StorageResponseFromWarehouse customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(ST_NG_70_MSG3_StorageResponseFromWarehouse customResponse, GenericRequestParams requestParams)
        {
            if (customResponse.StorageResponseFromWarehouse == null)
            {
                LogMessagingUtil.Instance.AppendLine("No Storage details in the Response (StorageResponseFromWarehouse  =null) ");
                this.MyResponseData = new INF_MSG_GenericResponseData();
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = "No Storage details in the Response (StorageResponseFromWarehouse  =null) ";
                return;
            }

            var context = CustomContext.GetContext(requestParams.Tenant);
            var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            var myQueryService = new DeclarationQueryService(context);
            var declarationNumber = customResponse.StorageResponseFromWarehouse.declerationNumber;
            var responseName = requestParams.ResponseName;

            requestParams.AppicationId = myQueryService.GetIdByDeclarationNumber(declarationNumber, requestParams.Tenant);
            if (string.IsNullOrWhiteSpace(requestParams.AppicationId))
            {
                LogMessagingUtil.Instance.AppendLine("Can not found declaration (GetIdByDeclarationNumber) :" + declarationNumber);
                this.MyResponseData = new INF_MSG_GenericResponseData();
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = "Can not found declaration (GetIdByDeclarationNumber) : " + declarationNumber;
                return;
            }       

            this._MyDeclarationPM = myQueryService.GetSingle(requestParams.AppicationId, true, false);
            if (this._MyDeclarationPM == null)
            {
                LogMessagingUtil.Instance.AppendLine("Failed getting declaration details: " + requestParams.AppicationId);
                this.MyResponseData = new INF_MSG_GenericResponseData();
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = "Failed getting declaration details: " + declarationNumber;
                return;
            }
            LogMessagingUtil.Instance.AppendLine("Analyze Storage response" + requestParams.AppicationId);

            //Raise Storage event 
            string eventCode = "";
            string remarks = "";

            if (customResponse.StorageResponseFromWarehouse.responseStatus == 1)
            {
                eventCode = "BRA";
                remarks = "מספר בקשה: " + customResponse.StorageResponseFromWarehouse.storageRequestMessageNum + "\n" +
                   "מספר הצהרה: " + customResponse.StorageResponseFromWarehouse.declerationNumber + "\n" +
                   "גירסת הצהרה: " + customResponse.StorageResponseFromWarehouse.versionNumber;
            }
            else if (customResponse.StorageResponseFromWarehouse.responseStatus == 2)
            {
                string rejectMess = "";
                int reason = 0;
                if(customResponse.storageRejectionReason != null)
                {
                    reason = customResponse.storageRejectionReason[0];
                }
                switch (reason)
                {
                    case 1:
                        rejectMess = ", סיבת דחייה: ביטוח לא תואם";
                        break;
                    case 2:
                        rejectMess = ", סיבת דחייה: אופי טובין לא מתאים";
                        break;
                    case 3:
                        rejectMess = ", סיבת דחייה: מיקום במחסן";
                        break;
                    case 0:
                        rejectMess = ", לא התקבל קוד דחייה ";
                        break;
                    default:
                        rejectMess = ", קוד דחייה לא ניתן לתרגום " + customResponse.storageRejectionReason[0];
                        break;
                }

                eventCode = "BRD";
                remarks = "מספר בקשה: " + customResponse.StorageResponseFromWarehouse.storageRequestMessageNum + "\n" +
                   "מספר הצהרה: " + customResponse.StorageResponseFromWarehouse.declerationNumber + "\n" +
                   "גירסת הצהרה: " + customResponse.StorageResponseFromWarehouse.versionNumber + "\n" + 
                    "סיבת דחיה: " + rejectMess;
            }

            if (eventCode != "")
            {
                RaiseStorageEvent(_MyDeclarationPM, requestParams.LoggingUserId, eventCode, remarks);
            }

            UpdateNotification(this._MyDeclarationPM, customResponse.StorageResponseFromWarehouse.responseStatus.ToString(), remarks);
            UpdateDeclaration(customResponse.StorageResponseFromWarehouse.responseStatus, requestParams.Tenant);
            
            this.MyResponseData = new INF_MSG_GenericResponseData();
            MyResponseData.ApplicationID = _MyDeclarationPM.Id;
            MyResponseData.Succeeded = true;

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.EntityId1 = _MyDeclarationPM.Id;
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            this.MyRequestSheetParam.RequestDescription = "אישור/דחיית בקשה אחסנה - " + _MyDeclarationPM.CustomFileNo;
        }

        private void UpdateDeclaration(int responseStatus, int tenant)
        {
            var context = CustomContext.GetContext(tenant);
            var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), tenant);

            switch (responseStatus)
            {
                case 1:
                    this._MyDeclarationPM.StorageStatusCode = "1";
                    break;
                case 2:
                    this._MyDeclarationPM.StorageStatusCode = "2";
                    break;
            }
            this._MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
            myDeclarationUpdateService.Update(this._MyDeclarationPM, true);
        }

        private void RaiseStorageEvent(DeclarationPM _MyDeclarationPM, string loggingUserId, string eventCode, string remarks)
        {
            string eventType = "";
            if (eventCode == "BRA")
            {
                eventType = "approved";
            }
            else if (eventCode == "BRD")
            {
                eventType = "deny";
            }

            var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
            {
                Tenant = _MyDeclarationPM.Tenant,
                objectTableName = "Customs.Declaration",
                EventCode = eventCode,
                notes = remarks,
                CommunicationLoggingEntityReference = _MyDeclarationPM.DeclarationNumber,
                EntityId = _MyDeclarationPM.Id,
                UserId = loggingUserId,

                CommunicationSubject = "FU Status " + eventCode + " from logitude (Declaration Storage " + eventType + ")",
                MyFUStatus = new AmitalEventTracerModel.FUStatus()
                {
                    entname = "CFIFILEM",
                    primary_number = _MyDeclarationPM.CustomFileNo,
                    status = "new",
                    xml_status = "new",
                    status_id = eventCode,
                    status_DateTime = DateTime.Now,
                    //status_place = "FRA",
                    //status_save = "no_fail",
                    comments = remarks,
                }
            };

            LogMessagingUtil.Instance.AppendLine("AmitalEventTracer.CreateTraceEvent  eventCode =" + eventCode + "  CustomFileNo= " + _MyDeclarationPM.CustomFileNo + "   ");
            AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel);


        }

        private void UpdateNotification(DeclarationPM dirtyDeclarationPM, string responseStatus, string remarks)
        {

            var contactRep = new ContactRepository(dirtyDeclarationPM.Tenant);
            var contact = contactRep.GetSingleContactByEmail(AuthenticationUtil.ResolveUserIdentityName(dirtyDeclarationPM.Tenant), dirtyDeclarationPM.Tenant);
            
            string loggingUserId = "";
            loggingUserId = contact.Id;

            DoUpdateNotification(dirtyDeclarationPM, loggingUserId, responseStatus, remarks);      
        }

        private void DoUpdateNotification(DeclarationPM declarationPM, string loggingUserId, string responseStatus, string remarks)
        {
            LogMessagingUtil.Instance.AppendLine("New Bonded Request Notification");

            ICustomContext dbContext = CustomContext.GetContext(declarationPM.Tenant);
            var notificationUpdateService = new NotificationUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), declarationPM.Tenant);
            var notificationQueryService = new NotificationQueryService(dbContext);

            var newNotificationPM = new NotificationPM();
            newNotificationPM.ChangeSetOp = ChangeSetOperation.Insert;
            newNotificationPM.Tenant = declarationPM.Tenant;

            switch (responseStatus)
            {
                case "1":
                    newNotificationPM.NotificationDefinitionCode = "70N";
                    newNotificationPM.AssigneToNotificationTypeCode = "A";
                    break;
                case "2":
                    newNotificationPM.NotificationDefinitionCode = "70C";
                    newNotificationPM.AssigneToNotificationTypeCode = "A";
                    break;
            }

            newNotificationPM.EntityId = declarationPM.Id;
            newNotificationPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            newNotificationPM.Reference1Number = declarationPM.CustomFileNo;
            newNotificationPM.CreateDate = DateTime.Now;
            newNotificationPM.Description = remarks;
            newNotificationPM.DepartmentId = declarationPM.DepartmentId;
            newNotificationPM.DeclarationOfficeCode = declarationPM.DeclarationOfficeCode;
            newNotificationPM.DueDate = DateTime.Now;
            if (declarationPM != null && !string.IsNullOrWhiteSpace(declarationPM.CustomerId)) newNotificationPM.CustomerId = declarationPM.CustomerId; // moran 20.6.16 - Task 20789
            
            newNotificationPM.AssigneToId =
               NotificationBase.
               CalcAssigneToId(newNotificationPM.Tenant, declarationPM.CustomerId, declarationPM.ReferentUserId, newNotificationPM.NotificationDefinitionCode, "");

            if (declarationPM != null && !string.IsNullOrWhiteSpace(declarationPM.DeclarationOfficeCode))
            {
                newNotificationPM.IsHandledByCustomOffice = true;
            }

            notificationUpdateService.Update(newNotificationPM, true);

        }
        // moran 9.9.14 - Task 7885 <--
    }
}
