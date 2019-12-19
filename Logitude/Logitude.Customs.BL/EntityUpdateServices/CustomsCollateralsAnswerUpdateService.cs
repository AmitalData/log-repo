using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.Data;
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
    public partial class CustomsCollateralsAnswerUpdateService
    {
        protected override void OnCreating(CustomsCollateralsAnswerPM entityPM, CustomsCollateralPM entityParentPM)
        {
            entityPM.CustomsCollateralId = entityParentPM.Id;
            entityParentPM.CustomsCollateralsAnswerLineNumber += 1;
            entityPM.LineNumber = entityParentPM.CustomsCollateralsAnswerLineNumber;
            entityPM.Tenant = entityParentPM.Tenant;
            entityPM.AllocatedAmount = entityParentPM.CustomsCollateralsConditions.Sum(x => x.RequestedAmount);
        }

        protected override void OnUpdating(CustomsCollateralsAnswerPM entityPM, Data.EntityPOCOs.CustomsCollateralsAnswer entityPOCO)
        {
            //entityPM.Tenant = entityPOCO.Tenant;
            //UpdateNotification(entityPM);
        }

        private void UpdateNotification(CustomsCollateralsAnswerPM entityPM)
        {
            string loggingUserId = AuthenticationUtil.ResolveUserId(entityPM.Tenant);

            string notificationDefinitionCode = "";
            var eventContextTagModel = entityPM.CurrentContextTag as EventContextTagModel;
            if (eventContextTagModel != null)
            {
                switch (eventContextTagModel.CallProccessID)
                {
                    case EventContextTagModel.ProccessEnum.None:
                        break;
                }
            }

            if (!string.IsNullOrWhiteSpace(notificationDefinitionCode))
            {
                var customsCollateralQueryService = new CustomsCollateralQueryService(entityPM.Tenant);
                CustomsCollateralPM customsCollateralPM = customsCollateralQueryService.GetSingle(entityPM.CustomsCollateralId, false, false);
                DeclarationPM connectedDeclarationPM = GetDeclarationEntity(customsCollateralPM);
                DoUpdateNotification(entityPM, connectedDeclarationPM, loggingUserId, notificationDefinitionCode);
            }
        }

        private DeclarationPM GetDeclarationEntity(CustomsCollateralPM customsCollateralPM)
        {
            var declarationQueryService = new DeclarationQueryService(customsCollateralPM.Tenant);
            var myDBEntity = declarationQueryService.GetSingle(customsCollateralPM.DeclarationId, false, false);
            return myDBEntity ?? new DeclarationPM();
        }

        private void DoUpdateNotification(CustomsCollateralsAnswerPM entityPM, DeclarationPM connectedDeclarationPM, string loggingUserId, string notificationDefinitionCode)
        {
            ICustomContext dbContext = CustomContext.GetContext(entityPM.Tenant);
            this.currentContext = dbContext;
            string desc = "";
            string referentUserId = null;
            string oldassigneId = null;
            var notificationUpdateService = new NotificationUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), EntityPM.Tenant);
            var notificationQueryService = new NotificationQueryService(dbContext);

            EventContextTagModel entityEventContext = entityPM.CurrentContextTag as EventContextTagModel;
            var newNotificationPM = new NotificationPM();
            newNotificationPM.ChangeSetOp = ChangeSetOperation.Insert;
            newNotificationPM.Tenant = entityPM.Tenant;

            switch (notificationDefinitionCode)
            {
                case "8213N":
                    newNotificationPM.NotificationDefinitionCode = "8213N";

                    CustomsCollateralQueryService customsCollateralQueryService = new CustomsCollateralQueryService(entityPM.Tenant);
                    CustomsCollateralPM customsCollateralPM = customsCollateralQueryService.GetSingle(entityPM.CustomsCollateralId, false, true);
                    if (customsCollateralPM != null)
                    {
                        desc = "תיק " + connectedDeclarationPM.CustomFileNo + " בטוחה- " + customsCollateralPM.CollateralRequestNumber;
                        if (!string.IsNullOrWhiteSpace(customsCollateralPM.CollateralRequestStatusCode))
                        {
                            CollateralRequestStatusQueryService collateralRequestStatusQueryService = new CollateralRequestStatusQueryService(customsCollateralPM.Tenant);
                            CollateralRequestStatusPM collateralRequestStatus = collateralRequestStatusQueryService.GetSingle(customsCollateralPM.CollateralRequestStatusCode, false, true);
                            if (collateralRequestStatus != null)
                            {
                                desc = string.Concat(desc, "\n" + collateralRequestStatus.LocalName);
                            }
                        }
                    }
                    else
                    {
                        desc = "תיק " + connectedDeclarationPM.CustomFileNo;
                    }

                    if (!string.IsNullOrWhiteSpace(entityPM.Errors))
                    {
                        desc = string.Concat(desc, "\n" + " שגיאות שהתקבלו- " + entityPM.Errors);
                    }

                    newNotificationPM.AssigneToNotificationTypeCode = "A";
                    newNotificationPM.DueDate = DateTime.Now;
                    newNotificationPM.Reference2Number = customsCollateralPM.CollateralRequestNumber;
                    break;
            }

            newNotificationPM.EntityId = entityPM.CustomsCollateralId;
            newNotificationPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.CustomsCollateral");
            newNotificationPM.CreateDate = DateTime.Now;
            newNotificationPM.Description = desc;

            if (connectedDeclarationPM != null && !string.IsNullOrWhiteSpace(connectedDeclarationPM.CustomFileNo))
            {
                newNotificationPM.EntityId = connectedDeclarationPM.Id;
                newNotificationPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                newNotificationPM.Reference1Number = connectedDeclarationPM.CustomFileNo;
                newNotificationPM.DepartmentId = connectedDeclarationPM.DepartmentId;
                newNotificationPM.DeclarationOfficeCode = connectedDeclarationPM.DeclarationOfficeCode;
                referentUserId = connectedDeclarationPM.ReferentUserId;
            }
            if (connectedDeclarationPM != null && !string.IsNullOrWhiteSpace(connectedDeclarationPM.CustomerId)) newNotificationPM.CustomerId = connectedDeclarationPM.CustomerId; // moran 20.6.16 - Task 20789

            newNotificationPM.AssigneToId =
                NotificationBase.
                CalcAssigneToId(entityPM.Tenant, connectedDeclarationPM.CustomerId, referentUserId, notificationDefinitionCode, oldassigneId);

            NotificationBase.CloseAllRelatedNotification(dbContext, newNotificationPM, "8213");
            if (notificationDefinitionCode == "8213N")
            {
                NotificationBase.CloseAllRelatedNotification(dbContext, newNotificationPM, "8211");
            }
            notificationUpdateService.Update(newNotificationPM, true);
        }

        protected override void UpdateComposition(CustomsCollateralsAnswerPM entityPM)
        {
            CollateralsRequestFileCondUpdateService collateralsRequestFileConditionUpdateService = new CollateralsRequestFileCondUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            collateralsRequestFileConditionUpdateService.UpdateMulti(entityPM.CollateralsRequestFileConds, entityPM.DeletedCollateralsRequestFileConds, entityPM, false);

        }
    }
}
