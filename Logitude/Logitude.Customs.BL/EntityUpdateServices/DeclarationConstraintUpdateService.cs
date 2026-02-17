using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Server.Tools.Helpers;
using Logitude.Customs.Data;
using Logitude.Customs.BL.EntityQueryServices;
using Simplog.Server.Infrastructure;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Models;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class DeclarationConstraintUpdateService : EntityUpdateService<DeclarationConstraint, DeclarationConstraintPM, DeclarationPM>
    {
        //Not relevant- there is a separate table for constraint
        //protected override void OnCreating(DeclarationConstraintPM entityPM, DeclarationPM entityParentPM)
        //{

        //    entityPM.DeclarationID = entityParentPM.Id;
        //    entityPM.ConstraintNumber = IdCounter.GetNumber("Customs.DeclarationConstraint", entityPM.Tenant);
        //    base.OnCreating(entityPM, entityParentPM);
        //}

        protected override void OnCreating(DeclarationConstraintPM entityPM, DeclarationPM entityParentPM)
        {
            if (entityParentPM == null)
            {
                //throw new Exception("Declaration Constraint Modification Update Service ,must be apart of Domain Model ");
                return;
            }
            entityPM.DeclarationID = entityParentPM.Id;
            entityPM.Tenant = entityParentPM.Tenant;

            base.OnCreating(entityPM, entityParentPM);
        }

        protected override void OnUpdating(DeclarationConstraintPM entityPM)
        {
            //var setting = CustomsSettingQueryService.GetSettingByTenant(entityPM.Tenant);
            //if (setting.IsConnectedToUniFreight)
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(entityPM.Tenant);
            DeclarationPM declarationPM = declarationQueryService.GetSingle(entityPM.DeclarationID, false, false);
            if(declarationPM!=null && declarationPM.IsConnectedToUnifreight)
            {
                UpdateUnifreight(entityPM);
            }
            // moran 18.9.14 - Task 7918 -->
            string notificationDefinitionCode = "";
            var eventContextTagModel = entityPM.CurrentContextTag as EventContextTagModel;
            if (eventContextTagModel != null)
            {
                if (entityPM.ApprovalDecision == "2")
                {   
                }

                switch (eventContextTagModel.CallProccessID)
                {
                    case EventContextTagModel.ProccessEnum.None:
                        break;
                    case EventContextTagModel.ProccessEnum.EV_NG_8215_ConstraintApprovalDecisionResponseServiceDeny:
                        notificationDefinitionCode = "8215D"; 
                        break;
                    case EventContextTagModel.ProccessEnum.EV_NG_8215_ConstraintApprovalDecisionResponseServiceConditionalApproval:
                        notificationDefinitionCode = "8215C";
                        break;
                    case EventContextTagModel.ProccessEnum.EV_NG_8215_ConstraintApprovalDecisionResponseServiceApproved:
                        notificationDefinitionCode = "8215A";
                        break;
                }
            }
            if (!string.IsNullOrWhiteSpace(notificationDefinitionCode))
            {
                UpdateNotification(entityPM, notificationDefinitionCode);
            }
            // moran 18.9.14 - Task 7918 <--
        }


        // moran 15.9.14 - Task 7918 -->
        private void UpdateNotification(DeclarationConstraintPM dirtyEntityPM, string notificationDefinitionCode)
        {
            string loggingUserId = AuthenticationUtil.ResolveUserId(dirtyEntityPM.Tenant);
            DeclarationPM connectedDeclarationPM = GetConnectedDeclarationPM(dirtyEntityPM);

            DoUpdateNotification(dirtyEntityPM, connectedDeclarationPM, loggingUserId, notificationDefinitionCode);

        }

        private void DoUpdateNotification(DeclarationConstraintPM dirtyEntityPM, DeclarationPM connectedDeclarationPM, string loggingUserId, string notificationDefinitionCode)
        {
            string desc = "מספר אילוץ: " + dirtyEntityPM.ConstraintNumber + "\n" + "סטטוס אילוץ: " + dirtyEntityPM.ConstraintStatusCode;
            if (!string.IsNullOrWhiteSpace(dirtyEntityPM.ConstraintStatusCode)) //Mirit 26/05/15 Task 13151
            {
                ConstraintStatusQueryService constraintStatusQueryService = new ConstraintStatusQueryService(dirtyEntityPM.Tenant);
                ConstraintStatusPM constraintStatus = constraintStatusQueryService.GetSingle(dirtyEntityPM.ConstraintStatusCode, false, true);
                desc = desc + " (" + constraintStatus.LocalName + ")";
            }
            if (!string.IsNullOrWhiteSpace(dirtyEntityPM.ApprovalUserName))
            {
                desc = desc + "\n" + "טופל ע''י: " + dirtyEntityPM.ApprovalUserName;
            }
            if (!string.IsNullOrWhiteSpace(dirtyEntityPM.ApprovalNote))
            {
                desc = desc + "\n" + "הערות: " + dirtyEntityPM.ApprovalNote;
            }

            //desc = desc + " לתיק עמילות " + connectedDeclarationPM.CustomFileNo;
            LogMessagingUtil.Instance.AppendLine("New Constraint Notification");
            ICustomContext dbContext = CustomContext.GetContext(dirtyEntityPM.Tenant);
            this.currentContext = dbContext;

            var notificationUpdateService = new NotificationUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), dirtyEntityPM.Tenant);
            var notificationQueryService = new NotificationQueryService(dbContext);

            var newNotificationPM = new NotificationPM();
            newNotificationPM.ChangeSetOp = ChangeSetOperation.Insert;
            newNotificationPM.Tenant = dirtyEntityPM.Tenant;
            newNotificationPM.NotificationDefinitionCode = notificationDefinitionCode;
            newNotificationPM.CreatedByRequestID = dirtyEntityPM.CustomsRequestsSheetId;
            if (string.IsNullOrWhiteSpace(newNotificationPM.CreatedByRequestID))
            {
                newNotificationPM.CreatedByRequestID = Logitude.Customs.Def.Messaging.Customs.RequestSheetContext.Current.GetContextOrDefault().CustomsRequestsSheetId;
            }
            newNotificationPM.EntityId = dirtyEntityPM.DeclarationID;
            //newNotificationPM.ObjectTableId = ObjectTabelRepository.GetObjectTableByName("Customs.DeclarationConstraint");
            newNotificationPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            
            var oldNotificationPM = new NotificationPM();
            oldNotificationPM = GetNotification(notificationQueryService, newNotificationPM, notificationDefinitionCode);

            newNotificationPM.CreateDate = DateTime.Now;
            newNotificationPM.Description = desc;
            string oldassigneId = null;
            string referentUserId = null;
            if (connectedDeclarationPM != null)
            {
                newNotificationPM.Reference1Number = connectedDeclarationPM.CustomFileNo;
                newNotificationPM.DepartmentId = connectedDeclarationPM.DepartmentId;
                newNotificationPM.DeclarationOfficeCode = connectedDeclarationPM.DeclarationOfficeCode;
                referentUserId = connectedDeclarationPM.ReferentUserId;
            }
            if (connectedDeclarationPM != null && !string.IsNullOrWhiteSpace(connectedDeclarationPM.CustomerId)) newNotificationPM.CustomerId = connectedDeclarationPM.CustomerId; // moran 20.6.16 - Task 20789

            newNotificationPM.Reference2Number = dirtyEntityPM.ConstraintNumber;//Eitan h 26/5/15 13631
            if (oldNotificationPM != null)
            {
                oldassigneId = oldNotificationPM.AssigneToId;
            }
            newNotificationPM.DueDate = DateTime.Now;
            newNotificationPM.AssigneToNotificationTypeCode = "A";
            
            newNotificationPM.AssigneToId =
                NotificationBase.
                CalcAssigneToId(dirtyEntityPM.Tenant, connectedDeclarationPM.CustomerId, referentUserId, notificationDefinitionCode, oldassigneId);

            if (connectedDeclarationPM != null && !string.IsNullOrWhiteSpace(connectedDeclarationPM.DeclarationOfficeCode))
            {
                newNotificationPM.IsHandledByCustomOffice = false;
            }

            notificationUpdateService.Update(newNotificationPM, true);

        }

        private NotificationPM GetNotification(NotificationQueryService notificationQueryService, NotificationPM newNotificationPM, string notificationDefinitionCode)
        {
            var oldNotificationPM = notificationQueryService.GetNotification(newNotificationPM.Tenant, newNotificationPM.ObjectTableId, newNotificationPM.EntityId, notificationDefinitionCode);
            return oldNotificationPM;
        }

        private DeclarationPM GetConnectedDeclarationPM(DeclarationConstraintPM dirtyEntityPM)
        {
            var declarationQueryService = new Logitude.Customs.BL.EntityQueryServices.DeclarationQueryService(dirtyEntityPM.Tenant);
            var myDBEntity = declarationQueryService.GetSingle(dirtyEntityPM.DeclarationID, false, false);
            return myDBEntity ?? new DeclarationPM();
        }
        //moran 15.9.14 - Task 7918 <--
 
    }
}
