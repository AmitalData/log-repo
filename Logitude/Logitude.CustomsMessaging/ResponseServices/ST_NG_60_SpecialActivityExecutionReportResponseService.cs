using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.Storage;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class ST_NG_60_SpecialActivityExecutionReportResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, ST_NG_60_MSG9_SpecialActivityExecutionReportMessage, GenericRequestParams>
    {
        DeclarationPM _MyDeclarationPM;

        public override void Update(ST_NG_60_MSG9_SpecialActivityExecutionReportMessage customResponse, GenericRequestParams requestParams)
        {
            //Analyze Message 60 - Special activity Execution By Warehouse

            if (customResponse.SpecialActivityExecutionReportMessage == null)
            {
                LogMessagingUtil.Instance.AppendLine("No Storage details in the Response (SpecialActivityExecutionReportMessage is null) ");
                this.MyResponseData = new INF_MSG_GenericResponseData();
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = "No Storage details in the Response (SpecialActivityExecutionReportMessage is null) ";
                return;
            }

            var context = CustomContext.GetContext(requestParams.Tenant);
            var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            var myQueryService = new DeclarationQueryService(context);

            var cargoIdentifierDetails = customResponse.SpecialActivityExecutionReportMessage.cargoIdentifier;
            if (cargoIdentifierDetails.cargoIdentifierType.ToString() == "8" && !string.IsNullOrWhiteSpace(cargoIdentifierDetails.cargoIdentifierKey1))
            {
                var myDeclarationQueryService = new DeclarationQueryService(requestParams.Tenant);
                string myDeclarationId = myDeclarationQueryService.GetIdByDeclarationNumber(cargoIdentifierDetails.cargoIdentifierKey1, requestParams.Tenant);
                if (!string.IsNullOrWhiteSpace(myDeclarationId))
                {
                    this._MyDeclarationPM = myDeclarationQueryService.GetSingle(myDeclarationId, false, false);
                }
            }

            if (this._MyDeclarationPM == null)
            {
                this._MyDeclarationPM = myQueryService.GetDeclarationPMByCargoIdentifiers(cargoIdentifierDetails.cargoIdentifierType.ToString(), cargoIdentifierDetails.cargoIdentifierKey1, cargoIdentifierDetails.cargoIdentifierKey2, requestParams.Tenant);
                if (this._MyDeclarationPM == null)
                {
                    LogMessagingUtil.Instance.AppendLine("Failed getting declaration details: \n" + "Type: " + cargoIdentifierDetails.cargoIdentifierType + " Key1: " + cargoIdentifierDetails.cargoIdentifierKey1 + " Key2: " + cargoIdentifierDetails.cargoIdentifierKey2);
                    this.MyResponseData = new INF_MSG_GenericResponseData();
                    this.MyResponseData.Succeeded = true;
                    this.MyResponseData.HasException = true;
                    this.MyResponseData.UserMessage = "Failed getting declaration details: \n" + "Type: " + cargoIdentifierDetails.cargoIdentifierType + " Key1: " + cargoIdentifierDetails.cargoIdentifierKey1 + " Key2: " + cargoIdentifierDetails.cargoIdentifierKey2;
                    return;
                }
            }
            LogMessagingUtil.Instance.AppendLine("Analyze Storage response" + requestParams.AppicationId);

            string storageMessageTypeName = "";
            if (customResponse.SpecialActivityExecutionReportMessage.specialActivityType != 0)
            {
                StorageMessageTypeQueryService storageMessageTypeQueryService = new StorageMessageTypeQueryService(requestParams.Tenant);
                StorageMessageTypePM storageMessageType = storageMessageTypeQueryService.GetSingle(customResponse.SpecialActivityExecutionReportMessage.specialActivityType.ToString(), false, true);
                storageMessageTypeName = " (" + storageMessageType.LocalName + ")";
            }

            string remarks = "סוג הפעולה: " + customResponse.SpecialActivityExecutionReportMessage.specialActivityType + storageMessageTypeName + "\n" +
                   "מספר בקשה: " + customResponse.SpecialActivityExecutionReportMessage.specialActivityRequestNumber + "\n" +
                   "תאריך ביצוע: " + customResponse.SpecialActivityExecutionReportMessage.actionDate + "\n" +
                   "שם המאשר: " + customResponse.SpecialActivityExecutionReportMessage.approvalName + "\n" +
                   "הערות: " + customResponse.SpecialActivityExecutionReportMessage.note;

            UpdateNotification(this._MyDeclarationPM, "60A", remarks);

            this.MyResponseData = new INF_MSG_GenericResponseData();
            this.MyResponseData.ApplicationID = _MyDeclarationPM.Id;
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;
            this.MyResponseData.UserMessage = "אישור ביצוע פעולה מיוחדת במחסן - " + _MyDeclarationPM.CustomFileNo;

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.EntityId1 = _MyDeclarationPM.Id;
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            this.MyRequestSheetParam.RequestDescription = "אישור ביצוע פעולה מיוחדת במחסן - " + _MyDeclarationPM.CustomFileNo;
            this.MyRequestSheetParam.CustomFileNo = _MyDeclarationPM.CustomFileNo;
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
                case "60A":
                    newNotificationPM.NotificationDefinitionCode = "60A";
                    newNotificationPM.AssigneToNotificationTypeCode = "I";
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
               NotificationBase.CalcAssigneToId(newNotificationPM.Tenant, declarationPM.CustomerId, declarationPM.ReferentUserId, newNotificationPM.NotificationDefinitionCode, "");

            if (declarationPM != null && !string.IsNullOrWhiteSpace(declarationPM.DeclarationOfficeCode))
            {
                newNotificationPM.IsHandledByCustomOffice = true;
            }

            notificationUpdateService.Update(newNotificationPM, true);
        }

        public override INF_MSG_GenericResponseData GetResponse(ST_NG_60_MSG9_SpecialActivityExecutionReportMessage customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }
    }
}
