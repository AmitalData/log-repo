using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.Data;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class CustomsCollateralUpdateService
    {
        protected override void OnCreating(CustomsCollateralPM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("Customs.CustomsCollateral", entityPM.Tenant);
            entityPM.CreateDateTime = DateTime.Now;
        }

        protected override void UpdateComposition(CustomsCollateralPM entityPM)
        {
            CustomsCollateralsAnswerUpdateService customsCollateralsAnswerUpdateService = new CustomsCollateralsAnswerUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            customsCollateralsAnswerUpdateService.UpdateMulti(entityPM.CustomsCollateralsAnswers, entityPM.DeletedCustomsCollateralsAnswers, entityPM, false);

            CustomsCollateralsConditionUpdateService customsCollateralsConditionUpdateService = new CustomsCollateralsConditionUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            customsCollateralsConditionUpdateService.UpdateMulti(entityPM.CustomsCollateralsConditions, entityPM.DeletedCustomsCollateralsConditions, entityPM, false);
        }

        protected override void OnUpdating(CustomsCollateralPM entityPM)
        {
 

            UpdateNotification(entityPM);
        }

        private void UpdateNotification(CustomsCollateralPM entityPM)
        {
            string loggingUserId = AuthenticationUtil.ResolveUserId(entityPM.Tenant);

            string notificationDefinitionCode = "";
            DeclarationPM connectedDeclarationPM = GetDeclarationEntity(entityPM);

            var eventContextTagModel = entityPM.CurrentContextTag as EventContextTagModel;
            if (eventContextTagModel != null)
            {
                switch (eventContextTagModel.CallProccessID)
                {
                    case EventContextTagModel.ProccessEnum.None:
                        break;
                    case EventContextTagModel.ProccessEnum.DF_8211_CollateralRequestMsgInsert:
                        notificationDefinitionCode = "8211N";
                        break;
                    case EventContextTagModel.ProccessEnum.DF_8211_CollateralRequestMsgUpdate:
                        notificationDefinitionCode = "8211U";
                        break;
                    case EventContextTagModel.ProccessEnum.DF_8213_CollateralAnswerApprovalMsgResponseServiceReply:
                        notificationDefinitionCode = "8213N";
                        break;
                }
            }

            if (!string.IsNullOrWhiteSpace(notificationDefinitionCode))
            {
                DoUpdateNotification(entityPM, connectedDeclarationPM, loggingUserId, notificationDefinitionCode);
            }
        }

        private void DoUpdateNotification(CustomsCollateralPM dirtyEntityPM, DeclarationPM connectedDeclarationPM, string loggingUserId, string notificationDefinitionCode)
        {
            ICustomContext dbContext = CustomContext.GetContext(dirtyEntityPM.Tenant);
            this.currentContext = dbContext;
            string desc = "";
            string referentUserId = null;
            string oldassigneId = null;
            var notificationUpdateService = new NotificationUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), dirtyEntityPM.Tenant);
            var notificationQueryService = new NotificationQueryService(dbContext);

            var newNotificationPM = new NotificationPM();
            newNotificationPM.ChangeSetOp = ChangeSetOperation.Insert;
            newNotificationPM.Tenant = dirtyEntityPM.Tenant;

            switch (notificationDefinitionCode)
            {
                case "8211N":
                    newNotificationPM.NotificationDefinitionCode = "8211N";
                    newNotificationPM.AssigneToNotificationTypeCode = "A";
                    desc = "דרישה לבטוחה " + dirtyEntityPM.CollateralRequestNumber + "\n" + "סטטוס הדרישה- " + dirtyEntityPM.CollateralRequestStatusCode ;
                    if (!string.IsNullOrWhiteSpace(dirtyEntityPM.CollateralRequestStatusCode))
                    {
                        CollateralRequestStatusQueryService collateralRequestStatusQueryService = new CollateralRequestStatusQueryService(dirtyEntityPM.Tenant);
                        CollateralRequestStatusPM collateralRequestStatusPM = collateralRequestStatusQueryService.GetSingle(dirtyEntityPM.CollateralRequestStatusCode, false, true);
                        desc = desc + " (" + collateralRequestStatusPM.LocalName + ")";
                    }
                    if (!string.IsNullOrWhiteSpace(dirtyEntityPM.WorkerName))
                    {
                        desc = desc + "\n" + "עובד מכס- " + dirtyEntityPM.WorkerName;
                    }
                    if (!string.IsNullOrWhiteSpace(dirtyEntityPM.Remarks))
                    {
                        desc = desc + "\n" + "הערות- " + dirtyEntityPM.Remarks;
                    }
                    break;
                case "8211U":
                    newNotificationPM.NotificationDefinitionCode = "8211U";
                    newNotificationPM.AssigneToNotificationTypeCode = "A";
                    desc = "עודכנה דרישה לבטוחה " + dirtyEntityPM.CollateralRequestNumber + "\n" + "סטטוס הדרישה- " + dirtyEntityPM.CollateralRequestStatusCode;
                    if (!string.IsNullOrWhiteSpace(dirtyEntityPM.CollateralRequestStatusCode))
                    {
                        if (string.IsNullOrWhiteSpace(dirtyEntityPM.CollateralRequestStatusName))
                        {
                            CollateralRequestStatusQueryService collateralRequestStatusQueryService = new CollateralRequestStatusQueryService(dirtyEntityPM.Tenant);
                            CollateralRequestStatusPM collateralRequestStatusPM = collateralRequestStatusQueryService.GetSingle(dirtyEntityPM.CollateralRequestStatusCode, false, true);
                            dirtyEntityPM.CollateralRequestStatusName = collateralRequestStatusPM.LocalName ;
                        }
                        desc = desc + " (" + dirtyEntityPM.CollateralRequestStatusName + ")";
                    }
                    if (!string.IsNullOrWhiteSpace(dirtyEntityPM.WorkerName))
                    {
                        desc = desc + "\n" + "עובד מכס- " + dirtyEntityPM.WorkerName;
                    }
                    if (!string.IsNullOrWhiteSpace(dirtyEntityPM.Remarks))
                    {
                        desc = desc + "\n" + "הערות- " + dirtyEntityPM.Remarks;
                    }
                    break;
                case "8213N":
                    newNotificationPM.NotificationDefinitionCode = "8213N";
                    newNotificationPM.AssigneToNotificationTypeCode = "A";
                    desc = "תיק " + connectedDeclarationPM.CustomFileNo + " בטוחה- " + dirtyEntityPM.CollateralRequestNumber;
                    if (!string.IsNullOrWhiteSpace(dirtyEntityPM.CollateralRequestStatusCode))
                    {
                        CollateralRequestStatusQueryService collateralRequestStatusQueryService = new CollateralRequestStatusQueryService(dirtyEntityPM.Tenant);
                        CollateralRequestStatusPM collateralRequestStatus = collateralRequestStatusQueryService.GetSingle(dirtyEntityPM.CollateralRequestStatusCode, false, true);
                        if (collateralRequestStatus != null)
                        {
                            desc = string.Concat(desc, "\n" + collateralRequestStatus.LocalName);
                        }
                    }
                    if (dirtyEntityPM.CustomsCollateralsAnswers != null)
                    {
                        if (dirtyEntityPM.CustomsCollateralsAnswers.Count > 0)
                        {
                            var customsCollateralsAnswersDesc = "";
                            foreach (var customsCollateralsAnswers in dirtyEntityPM.CustomsCollateralsAnswers)
                            {
                                if (!string.IsNullOrWhiteSpace(customsCollateralsAnswersDesc))
                                {
                                    //customsCollateralsAnswersDesc = customsCollateralsAnswersDesc + "\n";
                                    customsCollateralsAnswersDesc = customsCollateralsAnswersDesc + @"
";
                                }
                                if (!string.IsNullOrWhiteSpace(customsCollateralsAnswers.Errors))
                                {
                                    //customsCollateralsAnswersDesc = string.Concat(customsCollateralsAnswersDesc, "\n" + customsCollateralsAnswers.Errors);
                                    customsCollateralsAnswersDesc = customsCollateralsAnswersDesc + @"
" + customsCollateralsAnswers.Errors;
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(customsCollateralsAnswersDesc))
                            {
                                // = string.Concat(desc, "\n" + " שגיאות שהתקבלו- ", "\n" + customsCollateralsAnswersDesc);
                                desc = desc + @"
" + " שגיאות שהתקבלו- " + @"
" + customsCollateralsAnswersDesc;
                            }
                        }
                    }
                    newNotificationPM.DueDate = DateTime.Now;
                    break;
            }

            newNotificationPM.DueDate = dirtyEntityPM.RequestValidityDate;
            newNotificationPM.EntityId = dirtyEntityPM.Id;
            newNotificationPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.CustomsCollateral");
            newNotificationPM.CreateDate = DateTime.Now;
            newNotificationPM.Description = desc;
            newNotificationPM.Reference2Number = dirtyEntityPM.CollateralRequestNumber;

            var oldNotificationPM = new NotificationPM();
            oldNotificationPM = GetNotification(notificationQueryService, newNotificationPM, notificationDefinitionCode);

            if (oldNotificationPM != null)
            {
                oldassigneId = oldNotificationPM.AssigneToId;
            }

            if (connectedDeclarationPM != null && !string.IsNullOrWhiteSpace(connectedDeclarationPM.CustomFileNo))
            {
                //newNotificationPM.EntityId = dirtyEntityPM.DeclarationId; // moran 17.11 - task 23119 - commented
                //newNotificationPM.ObjectTableId = ObjectTabelRepository.GetObjectTableByName("Customs.Declaration"); // moran 17.11 - task 23119 - commented
                newNotificationPM.Reference1Number = connectedDeclarationPM.CustomFileNo;
                newNotificationPM.DepartmentId = connectedDeclarationPM.DepartmentId;
                newNotificationPM.DeclarationOfficeCode = connectedDeclarationPM.DeclarationOfficeCode;
                referentUserId = connectedDeclarationPM.ReferentUserId;
            }
            if (connectedDeclarationPM != null && !string.IsNullOrWhiteSpace(connectedDeclarationPM.CustomerId)) newNotificationPM.CustomerId = connectedDeclarationPM.CustomerId; // moran 20.6.16 - Task 20789

            newNotificationPM.AssigneToId =
                NotificationBase.
                CalcAssigneToId(dirtyEntityPM.Tenant, connectedDeclarationPM.CustomerId, referentUserId, notificationDefinitionCode, oldassigneId);

            NotificationBase.CloseAllRelatedNotification(dbContext, newNotificationPM, "8211");
            if (notificationDefinitionCode == "8213N")
            {
                NotificationBase.CloseAllRelatedNotification(dbContext, newNotificationPM, "8213");
            }
            notificationUpdateService.Update(newNotificationPM, true);
        }

        private DeclarationPM GetDeclarationEntity(CustomsCollateralPM dirtyEntityPM)
        {
            var declarationQueryService = new DeclarationQueryService(dirtyEntityPM.Tenant);

            var myDBEntity = declarationQueryService.GetSingle(dirtyEntityPM.DeclarationId, false, false);
            return myDBEntity ?? new DeclarationPM();
        }

        private NotificationPM GetNotification(NotificationQueryService notificationQueryService, NotificationPM newNotificationPM, string notificationDefinitionCode)
        {
            var oldNotificationPM = notificationQueryService.GetNotification(newNotificationPM.Tenant, newNotificationPM.ObjectTableId, newNotificationPM.EntityId, notificationDefinitionCode);
            return oldNotificationPM;
        }
    }
}
