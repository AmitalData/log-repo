using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.Data;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial  class ProceduralFaultUpdateService
    {
        protected override void OnCreating(ProceduralFaultPM entityPM, EntityPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("Customs.ProceduralFault", entityPM.Tenant);
        }

        protected override void UpdateComposition(ProceduralFaultPM entityPM)
        {
            ProceduralFaultsConnEntityUpdateService proceduralFaultsConnectedEntityUpdateService = new ProceduralFaultsConnEntityUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            proceduralFaultsConnectedEntityUpdateService.UpdateMulti(entityPM.ProceduralFaultsConnEntities, entityPM.DeletedProceduralFaultsConnEntities, entityPM, false);
            base.UpdateComposition(entityPM);
        }

        protected override void OnUpdating(ProceduralFaultPM entityPM)
        {
            //var setting = CustomsSettingQueryService.GetSettingByTenant(entityPM.Tenant);
            //if (setting.IsConnectedToUniFreight)
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(entityPM.Tenant);
            DeclarationPM declarationPM = declarationQueryService.GetSingle(entityPM.DeclarationId, false, false);
            if (declarationPM != null && declarationPM.IsConnectedToUnifreight)
            {
                UpdateUnifreight(entityPM);
            }
            UpdateNotification(entityPM);
        }

        private void UpdateNotification(ProceduralFaultPM dirtyEntityPM) 
        {
            string loggingUserId = AuthenticationUtil.ResolveUserId(dirtyEntityPM.Tenant);

            string notificationDefinitionCode = "";
            var eventContextTagModel = dirtyEntityPM.CurrentContextTag as EventContextTagModel;
            if (eventContextTagModel != null)
            {
                switch (eventContextTagModel.CallProccessID)
                {
                    case EventContextTagModel.ProccessEnum.None:
                        break;
                    case EventContextTagModel.ProccessEnum.EV_NG_8218_MSG14100_ProceduralFaultMsgInsert:
                        notificationDefinitionCode = "8218N";
                        break;
                    case EventContextTagModel.ProccessEnum.EV_NG_8218_MSG14100_ProceduralFaultMsgUpdate:
                        notificationDefinitionCode = "8218U";
                        break;
                    case EventContextTagModel.ProccessEnum.EV_NG_8218_MSG14100_ProceduralFaultMsgCancel:
                        notificationDefinitionCode = "8219C";
                        break;
                    default:
                        break;
                }
            }

            if (!String.IsNullOrWhiteSpace(notificationDefinitionCode))
            {
                DoUpdateNotification(dirtyEntityPM, loggingUserId, notificationDefinitionCode);
            }
        }

        private void DoUpdateNotification(ProceduralFaultPM dirtyEntityPM, string loggingUserId, string notificationDefinitionCode)
        {
            ICustomContext dbContext = CustomContext.GetContext(dirtyEntityPM.Tenant);
            this.currentContext = dbContext;
            string desc = "";
            var notificationUpdateService = new NotificationUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), dirtyEntityPM.Tenant);
            var notificationQueryService = new NotificationQueryService(dbContext);
            string oldassigneId = null;
            string customerId = null;
            string referentUserId = null;

            var eventContextTagModel = dirtyEntityPM.CurrentContextTag as EventContextTagModel;
            var newNotificationPM = new NotificationPM();
            if (eventContextTagModel.MyNotificationPM != null)
            {
                newNotificationPM = eventContextTagModel.MyNotificationPM;
            }

            newNotificationPM.ChangeSetOp = ChangeSetOperation.Insert;
            newNotificationPM.Tenant = dirtyEntityPM.Tenant;
            newNotificationPM.CreateDate = DateTime.Now;
            newNotificationPM.Reference2Number = dirtyEntityPM.ProceduralFaultNumber;
      
            switch (notificationDefinitionCode)
            {
                case "8218N":
                    newNotificationPM.NotificationDefinitionCode = "8218N";
                    newNotificationPM.AssigneToNotificationTypeCode = "A";
                    newNotificationPM.DueDate = DateTime.Now;
                    desc = "התקבל ליקוי מכס";
                    break;
                case "8218U":
                    newNotificationPM.NotificationDefinitionCode = "8218U";
                    newNotificationPM.AssigneToNotificationTypeCode = "I";
                    newNotificationPM.DueDate = DateTime.Now;
                    desc = "עודכן ליקוי מכס";
                    break;
                case "8219C":
                    newNotificationPM.NotificationDefinitionCode = "8219C";
                    newNotificationPM.AssigneToNotificationTypeCode = "I";
                    newNotificationPM.DueDate = DateTime.Now;
                    desc ="בוטל ליקוי מכס";
                    break;
            }
            if (!string.IsNullOrWhiteSpace(desc) && !string.IsNullOrWhiteSpace(dirtyEntityPM.DeclarationNumber)) desc = desc + " - " + "הצהרה " + dirtyEntityPM.DeclarationNumber;
            desc += "\n" + "מספר ליקוי " + dirtyEntityPM.ProceduralFaultNumber;
            desc += "\n" + "הערות המכס " + dirtyEntityPM.Remarks;

            NotificationPM oldNotificationPM = null;
            oldNotificationPM = GetNotification(notificationQueryService, newNotificationPM, notificationDefinitionCode);
            if (oldNotificationPM != null)
            {
                oldassigneId = oldNotificationPM.AssigneToId;
            }
            
            if (eventContextTagModel.MyNotificationPM == null)
            {
                newNotificationPM.EntityId = dirtyEntityPM.Id;
                newNotificationPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.ProceduralFaults");
//                newNotificationPM.Description = desc;
                newNotificationPM.IsHandledByCustomOffice = true;
            }
            else
            {
                customerId = eventContextTagModel.MyNotificationPM.CustomerId; // moran 13.9.16 - Bug 22397 change from MyNotificationPM.AssigneToId to MyNotificationPM.CustomerId
                referentUserId = eventContextTagModel.MyNotificationPM.Reference1Number;
                newNotificationPM.AssigneToId = NotificationBase.CalcAssigneToId(newNotificationPM.Tenant, customerId, referentUserId, notificationDefinitionCode, oldassigneId);
                newNotificationPM.Reference1Number = dirtyEntityPM.CustomFileNo;
            }
            if (customerId != null) newNotificationPM.CustomerId = customerId; // moran 20.6.16 - Task 20789

            newNotificationPM.Description = desc;//5/11/15 15957
            NotificationBase.CloseAllRelatedNotification(dbContext, newNotificationPM, newNotificationPM.NotificationDefinitionCode);
            notificationUpdateService.Update(newNotificationPM, true);
        }


        private NotificationPM GetNotification(NotificationQueryService notificationQueryService, NotificationPM newNotificationPM, string notificationDefinitionCode)
        {
            var oldNotificationPM = notificationQueryService.GetNotification(newNotificationPM.Tenant, newNotificationPM.ObjectTableId, newNotificationPM.EntityId, notificationDefinitionCode);
            return oldNotificationPM;
        }
    }
}
