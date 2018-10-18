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
    public class ST_NG_10_SpecialActivityResponseMessageResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, ST_NG_10_MSG08_SpecialActivityResponseMessage, GenericRequestParams>
    {
        DeclarationPM _MyDeclarationPM;
        public override void Update(ST_NG_10_MSG08_SpecialActivityResponseMessage customResponse, GenericRequestParams requestParams)
        {
            this.MyResponseData = new INF_MSG_GenericResponseData();
            this.MyResponseData.Succeeded = true;

            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);

            //Checking foe Exceptions
            if (customResponse.ResponseContentHeader.Exception != null)
            {
                if (customResponse.ResponseContentHeader.Exception != null)
                {
                    var errMess = "Special activity request error: " + customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription;
                    LogMessagingUtil.Instance.AppendLine(errMess);
                    this.MyResponseData.UserMessage = errMess;
                }              
                this.MyResponseData.HasException = true;
                return;
            }

            if (customResponse.SpecialActivityRequestMessage != null && customResponse.SpecialActivityRequestMessage.General.cargoIdentifier != null)
            {
                var myConsignmentQueryService = new ConsignmentQueryService(dbContext);
                var cargoIdentifier = customResponse.SpecialActivityRequestMessage.General.cargoIdentifier;
                string myDeclaration = myConsignmentQueryService.GetDeclarationIdByConsignmentCargoId(cargoIdentifier.cargoIdentifierKey1, cargoIdentifier.cargoIdentifierKey2, cargoIdentifier.cargoIdentifierKey3, requestParams.Tenant);
                if (!string.IsNullOrWhiteSpace(myDeclaration))
                {
                    string description = "מספר בקשה " + customResponse.SpecialActivityResponseMessage.specialActivityRequestNumber + "\n"
                        + "מספר בדיקה " + customResponse.SpecialActivityResponseMessage.CheckNumber + "\n";
                    if (customResponse.SpecialActivityResponseMessage.guaranteeRequiredSpecified == true && customResponse.SpecialActivityResponseMessage.guaranteeRequired == true)
                    {
                        description = description + "נדרשת ערבות" + "\n";
                    }
                    if (customResponse.SpecialActivityResponseMessage.escortRequiredSpecified == true && customResponse.SpecialActivityResponseMessage.escortRequired == true)
                    {
                        description = description + "נדרש ליווי ע''י " + customResponse.SpecialActivityResponseMessage.escortName + "\n";
                    }
                    description = description + "עובד מכס " + customResponse.SpecialActivityResponseMessage.name + "\n"
                        + "תאריך החלטה " + customResponse.SpecialActivityResponseMessage.decisionDate.Date.ToString("dd/MM/yyyy");

                    string notificationDefinitionCode = "";
                    switch (customResponse.SpecialActivityResponseMessage.approval)
                    {
                        case 1:
                            notificationDefinitionCode = "10A";
                            break;
                        case 2:
                            notificationDefinitionCode = "10D";
                            break;
                        default:
                            break;
                    }

                    var myDeclarationQueryService = new DeclarationQueryService(dbContext);
                    _MyDeclarationPM = myDeclarationQueryService.GetSingle(myDeclaration,false,false);

                    UpdateNotification(myDeclaration, notificationDefinitionCode, description, requestParams.Tenant);
                }
            }

            this.MyResponseData.UserMessage = "בקשה לפעולה מיוחדת במחסן";
            LogMessagingUtil.Instance.AppendLine("Special activity request Succeeded");
            this.MyResponseData.HasException = false;

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = "בקשה לפעולה מיוחדת במחסן";
            if (_MyDeclarationPM != null)
            {
                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                this.MyRequestSheetParam.EntityId1 = _MyDeclarationPM.Id;
                this.MyRequestSheetParam.CustomFileNo = _MyDeclarationPM.CustomFileNo;
            }
        }

        public override INF_MSG_GenericResponseData GetResponse(ST_NG_10_MSG08_SpecialActivityResponseMessage customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        private void UpdateNotification(string DeclarationId, string notificationDefinitionCode, string Description, int tenant)
        {
            if (string.IsNullOrWhiteSpace(DeclarationId) || string.IsNullOrWhiteSpace(notificationDefinitionCode))
            {
                return;
            }

            string loggingUserId = "";
            var contactRep = new ContactRepository(tenant);
            var contact = contactRep.GetSingleContactByEmail(AuthenticationUtil.ResolveUserIdentityName(tenant), tenant);
            loggingUserId = contact.Id;

            DoUpdateNotification(loggingUserId, notificationDefinitionCode, Description, tenant);
        }

        private void DoUpdateNotification(string loggingUserId, string notificationDefinitionCode, string description, int tenant)
        {
            LogMessagingUtil.Instance.AppendLine("New Bonded Special Request Notification");

            ICustomContext dbContext = CustomContext.GetContext(tenant);
            var notificationUpdateService = new NotificationUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
            var notificationQueryService = new NotificationQueryService(dbContext);

            var newNotificationPM = new NotificationPM();
            newNotificationPM.ChangeSetOp = ChangeSetOperation.Insert;
            newNotificationPM.Tenant = tenant;
            newNotificationPM.NotificationDefinitionCode = notificationDefinitionCode;
            newNotificationPM.CreateDate = DateTime.Now;
            newNotificationPM.Description = description;
            newNotificationPM.AssigneToNotificationTypeCode = "I";
            newNotificationPM.DueDate = DateTime.Now;

            string customerId = null;
            string referentUserId = null;
            if (_MyDeclarationPM != null)
            {
                newNotificationPM.EntityId = _MyDeclarationPM.Id; 
                newNotificationPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                newNotificationPM.Reference1Number = _MyDeclarationPM.CustomFileNo;
                newNotificationPM.DepartmentId = _MyDeclarationPM.DepartmentId;
                newNotificationPM.DeclarationOfficeCode = _MyDeclarationPM.DeclarationOfficeCode;
                customerId = _MyDeclarationPM.CustomerId;
                referentUserId = _MyDeclarationPM.ReferentUserId;
            }
            if (_MyDeclarationPM != null && !string.IsNullOrWhiteSpace(_MyDeclarationPM.CustomerId)) newNotificationPM.CustomerId = _MyDeclarationPM.CustomerId; // moran 20.6.16 - Task 20789
            
            newNotificationPM.AssigneToId =
               NotificationBase.
               CalcAssigneToId(newNotificationPM.Tenant, customerId, referentUserId, notificationDefinitionCode, "");

            if (_MyDeclarationPM != null && !string.IsNullOrWhiteSpace(_MyDeclarationPM.DeclarationOfficeCode))
            {
                newNotificationPM.IsHandledByCustomOffice = true;
            }
            NotificationBase.CloseAllRelatedNotification(dbContext, newNotificationPM, "10");
            
            notificationUpdateService.Update(newNotificationPM, true);

        }
        
    }
}
