using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
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
   public partial class DepositUpdateService : EntityUpdateService<Deposit, DepositPM, EntityPM>
    {

       protected override void OnCreating(DepositPM entityPM, EntityPM entityParentPM)
       {
           entityPM.Id = IdCounter.GetNumber("Customs.Deposit", entityPM.Tenant);
           ICustomContext context = MainContext as CustomContext;
           TapagQueryService tapagQueryService = new TapagQueryService(context);

            TapagPM tapag = new TapagPM()
            {
                Id = IdCounter.GetNumber("Customs.Tapag", entityPM.Tenant),
                Tenant = entityPM.Tenant,
                CustomerId = entityPM.CustomerId,
                CustomsBranchCode = entityPM.CustomsBranchCode,
                ImporterId = entityPM.ImporterId,
                FollowDate = entityPM.FollowDate,
                IsClosed = entityPM.IsClosed,
                LeadingFileNumber = entityPM.LeadingFileNumber,
                ProfessionUnitTypeCode = entityPM.ProfessionUnitTypeCode,
                SpecializationTypeCode = entityPM.SpecializationTypeCode,
                TapagNumber = entityPM.TapagNumber,
                TapagTypeCode = entityPM.TapagTypeCode,
                ValidityDate = entityPM.ValidityDate,
                CreateDate = entityPM.CreateDate,
                ChangeSetOp = ChangeSetOperation.Insert,
            };

            TapagUpdateService tapagUpdate = new TapagUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), entityPM.Tenant);
            tapagUpdate.Update(tapag, false);
            entityPM.TapagID = tapag.Id;

           base.OnCreating(entityPM, entityParentPM);

       }

       private void OnUpdatingTapag(DepositPM entityPM)
       {
           if (string.IsNullOrWhiteSpace(entityPM.TapagID))
           {
               return;
           }
           ICustomContext context = MainContext as CustomContext;
           //var tapagQueryService = new TapagQueryService(context);
           var tapagUpdateService = new TapagUpdateService(context, new Dictionary<string, IContext>(), entityPM.Tenant);

           //TapagPM tapagPM = tapagQueryService.GetSingle(entityPM.TapagID, true, false);
           //if (tapagPM != null)
           //{
           //    tapagPM.ChangeSetOp = ChangeSetOperation.Update;
           //    tapagPM.TapagTypeCode = entityPM.TapagTypeCode;
           //    tapagPM.CustomsBranchCode = entityPM.CustomsBranchCode;
           //    tapagPM.ValidityDate = entityPM.ValidityDate;
           //    tapagPM.FollowDate = entityPM.FollowDate;
           //    tapagUpdateService.Update(tapagPM, false);
           //}
           tapagUpdateService.OnUpdatingTapag(entityPM,true);
       }

        protected override void OnUpdating(DepositPM entityPM)
        {
            OnUpdatingTapag(entityPM);

            //var setting = CustomsSettingQueryService.GetSettingByTenant(entityPM.Tenant);
            //if (setting.IsConnectedToUniFreight)
            DeclarationPM connectedDeclarationPM = GetConnectedDeclarationPM(entityPM);
            if (connectedDeclarationPM != null && (connectedDeclarationPM.IsConnectedToUnifreight || connectedDeclarationPM.IsAmendment==true))
            {
                UpdateUnifreight(entityPM);
            }

            UpdateNotification(entityPM);
        }

       protected override void UpdateComposition(DepositPM entityPM)
       {
           DepositConditionUpdateService depositConditionUpdateService = new DepositConditionUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
           depositConditionUpdateService.UpdateMulti(entityPM.DepositConditions, entityPM.DeletedDepositConditions, entityPM, false);
           base.UpdateComposition(entityPM);

       }

       private void UpdateNotification(DepositPM dirtyEntityPM)
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
                   case EventContextTagModel.ProccessEnum.DEPO_2020_DepositRefundOrderInfoResponseServiceRefund:
                       notificationDefinitionCode = "2020N";
                       break;
                   case EventContextTagModel.ProccessEnum.DEPO_2000_DepositForfeitOrderInfoResponseServiceForfeit:
                       notificationDefinitionCode = "2000N";
                       break;
                   case EventContextTagModel.ProccessEnum.DEPO_NG_5110_DepositRequestFulfillednfoMsg:
                       notificationDefinitionCode = "5110N";
                       break;
               }
           }

           if (!string.IsNullOrWhiteSpace(notificationDefinitionCode))
           {
               DeclarationPM connectedDeclarationPM = GetConnectedDeclarationPM(dirtyEntityPM);
               DoUpdateNotification(dirtyEntityPM, connectedDeclarationPM, loggingUserId, notificationDefinitionCode);
           }
       }

       private void DoUpdateNotification(DepositPM dirtyEntityPM, DeclarationPM connectedDeclarationPM, string loggingUserId, string notificationDefinitionCode)
       {
           ICustomContext dbContext = CustomContext.GetContext(dirtyEntityPM.Tenant);
           this.currentContext = dbContext;
           string desc = "";
           var notificationUpdateService = new NotificationUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), dirtyEntityPM.Tenant);
           var notificationQueryService = new NotificationQueryService(dbContext);

           var newNotificationPM = new NotificationPM();
           newNotificationPM.ChangeSetOp = ChangeSetOperation.Insert;
           newNotificationPM.Tenant = dirtyEntityPM.Tenant;

           switch (notificationDefinitionCode)
           {
               case "2020N":
                   newNotificationPM.NotificationDefinitionCode = "2020N";
                   desc = "החזר פיקדון " + dirtyEntityPM.CustomsTapagFile + "/" + dirtyEntityPM.CustomsNumeral;
                   newNotificationPM.AssigneToNotificationTypeCode = "I";
                   newNotificationPM.Reference2Number = dirtyEntityPM.PaymentOrderNumber.ToString();
                   newNotificationPM.ResponseNotes = "Amount " + dirtyEntityPM.DepositAmount + "Remarks " + dirtyEntityPM.Remarks;
                   if (!string.IsNullOrWhiteSpace(dirtyEntityPM.DecisionCode))
                   { // DepositFileRequestStatus - table code 1393 - does not exist
                       //DepositFileRequestStatusQueryService depositFileRequestStatusQueryService = new DepositFileRequestStatusQueryService(dirtyEntityPM.Tenant);
                       //DepositFileRequestStatuPM depositFileRequestStatuPM = depositFileRequestStatusQueryService.GetSingle(dirtyEntityPM.DecisionCode, false, true);
                       desc = desc + "\n" + "קוד החלטה- " + dirtyEntityPM.DecisionCode;// +" (" + depositFileRequestStatuPM.LocalName + ")";
                   }
                   desc = desc + "\n" + "סכום שהוחזר- " + dirtyEntityPM.DepositAmount + "\n" + "הערה- " + dirtyEntityPM.Remarks;
                   break;
               case "2000N":
                   newNotificationPM.NotificationDefinitionCode = "2000N";
                   desc = "חילוט פיקדון " + dirtyEntityPM.CustomsTapagFile + " + " + dirtyEntityPM.CustomsNumeral;
                   newNotificationPM.AssigneToNotificationTypeCode = "I";
                   newNotificationPM.Reference2Number = dirtyEntityPM.PaymentOrderNumber.ToString();
                   newNotificationPM.ResponseNotes = "סכום- " + dirtyEntityPM.DepositAmount + "הערה- " + dirtyEntityPM.Remarks;
                   break;
               case "5110N":
                   newNotificationPM.NotificationDefinitionCode = "5110N";
                   desc = "פיקדון חדש " + dirtyEntityPM.CustomsTapagFile + " " + dirtyEntityPM.CustomsNumeral;
                   newNotificationPM.AssigneToNotificationTypeCode = "I";
                   newNotificationPM.Reference1Number = dirtyEntityPM.TapagNumber;
                   newNotificationPM.ResponseNotes = dirtyEntityPM.Remarks;
                   break;
           }

           var oldNotificationPM = new NotificationPM();
           oldNotificationPM = GetNotification(notificationQueryService, newNotificationPM, notificationDefinitionCode);

           newNotificationPM.DueDate = DateTime.Now;
           newNotificationPM.EntityId = dirtyEntityPM.Id;
           newNotificationPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Deposit");

           string oldassigneId = null;
           string customerId = null;
           string referentUserId = null;

           if (connectedDeclarationPM != null)
           {
               newNotificationPM.EntityId = connectedDeclarationPM.Id;
               newNotificationPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
               newNotificationPM.DepartmentId = connectedDeclarationPM.DepartmentId;
               newNotificationPM.DeclarationOfficeCode = connectedDeclarationPM.DeclarationOfficeCode;
               newNotificationPM.Reference1Number = connectedDeclarationPM.CustomFileNo;
               customerId = connectedDeclarationPM.CustomerId;
               referentUserId = connectedDeclarationPM.ReferentUserId;
           }
           if (connectedDeclarationPM != null && !string.IsNullOrWhiteSpace(connectedDeclarationPM.CustomerId)) newNotificationPM.CustomerId = connectedDeclarationPM.CustomerId; // moran 20.6.16 - Task 20789

           newNotificationPM.CreateDate = DateTime.Now;
           newNotificationPM.Description = desc;

           newNotificationPM.AssigneToId =
               NotificationBase.
               CalcAssigneToId(newNotificationPM.Tenant, customerId, referentUserId, notificationDefinitionCode, oldassigneId);

           NotificationBase.CloseAllRelatedNotification(dbContext, newNotificationPM, newNotificationPM.NotificationDefinitionCode);
           notificationUpdateService.Update(newNotificationPM, true);
       }

       private NotificationPM GetNotification(NotificationQueryService notificationQueryService, NotificationPM newNotificationPM, string notificationDefinitionCode)
       {
           var oldNotificationPM = notificationQueryService.GetNotification(newNotificationPM.Tenant, newNotificationPM.ObjectTableId, newNotificationPM.EntityId, notificationDefinitionCode);
           return oldNotificationPM;
       }

       private DeclarationPM GetConnectedDeclarationPM(DepositPM dirtyEntityPM)
       {
           if (string.IsNullOrWhiteSpace(dirtyEntityPM.ConnectedDeclarationId))
       {
               return null;
           }

           var declarationQueryService = new Logitude.Customs.BL.EntityQueryServices.DeclarationQueryService(dirtyEntityPM.Tenant);
           var myDBEntity = declarationQueryService.GetSingle(dirtyEntityPM.ConnectedDeclarationId, false, false);
           return myDBEntity ?? new DeclarationPM();
       }

    }
}
