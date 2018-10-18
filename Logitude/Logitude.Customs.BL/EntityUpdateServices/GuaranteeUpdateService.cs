using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.Data;
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
   public partial class GuaranteeUpdateService
    {
       protected override void OnCreating(GuaranteePM entityPM, EntityPM entityParentPM)
       {
           entityPM.Id = IdCounter.GetNumber("Customs.Guarantee", entityPM.Tenant);

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

       protected override void OnUpdating(GuaranteePM entityPM)
       {
           OnUpdatingTapag(entityPM);
           UpdateNotification(entityPM);
       }

       private void OnUpdatingTapag(GuaranteePM entityPM)
       {
           if (string.IsNullOrWhiteSpace(entityPM.TapagID))
           {
               return;
           }
           ICustomContext context = MainContext as CustomContext;
           var tapagUpdateService = new TapagUpdateService(context, new Dictionary<string, IContext>(), entityPM.Tenant);
           tapagUpdateService.OnUpdatingTapag(entityPM,true);
       }

       protected override void UpdateComposition(GuaranteePM entityPM)
       {
           GuaranteeConditionUpdateService guaranteeConditionUpdateService = new GuaranteeConditionUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
           guaranteeConditionUpdateService.UpdateMulti(entityPM.GuaranteeConditions, entityPM.DeletedGuaranteeConditions, entityPM, false);
           base.UpdateComposition(entityPM);

           RequiredGuaranteeTypeUpdateService requiredGuaranteeTypeUpdateService = new RequiredGuaranteeTypeUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
           requiredGuaranteeTypeUpdateService.UpdateMulti(entityPM.RequiredGuaranteeTypes, entityPM.DeletedRequiredGuaranteeTypes, entityPM, false);
           base.UpdateComposition(entityPM);
       }

       private void UpdateNotification(GuaranteePM dirtyEntityPM)
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
                   case EventContextTagModel.ProccessEnum.GRNT_MSG15_createGurateeRequestInfoResponseServiceNew:
                       notificationDefinitionCode = "1812N";
                       break;
                   case EventContextTagModel.ProccessEnum.GRNT_MSG15_createGurateeRequestInfoResponseServiceUpdate:
                       notificationDefinitionCode = "1812U";
                       break;
               }
           }

           if (!string.IsNullOrWhiteSpace(notificationDefinitionCode))
           {
               DeclarationPM connectedDeclarationPM = GetConnectedDeclarationPM(dirtyEntityPM);
               DoUpdateNotification(dirtyEntityPM, connectedDeclarationPM, loggingUserId, notificationDefinitionCode);
           }
       }

       private void DoUpdateNotification(GuaranteePM dirtyEntityPM, DeclarationPM connectedDeclarationPM, string loggingUserId, string notificationDefinitionCode)
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
               case "1812N":
                   newNotificationPM.NotificationDefinitionCode = "1812N";
                   newNotificationPM.AssigneToNotificationTypeCode = "A";
                   newNotificationPM.DueDate = dirtyEntityPM.RequestValidityDate;
                   desc = "בקשה להמצאת ערבות " + dirtyEntityPM.GuaranteeRequestNumber;
                   break;
               case "1812U":
                   newNotificationPM.NotificationDefinitionCode = "1812U";
                   newNotificationPM.AssigneToNotificationTypeCode = "A";
                   newNotificationPM.DueDate = dirtyEntityPM.RequestValidityDate;
                   desc = "בקשה להמצאת ערבות " + dirtyEntityPM.GuaranteeRequestNumber;
                   break;
           }

           newNotificationPM.EntityId = dirtyEntityPM.Id;
           newNotificationPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Guarantee");
           newNotificationPM.Reference2Number = dirtyEntityPM.GuaranteeRequestNumber;
           newNotificationPM.CreateDate = DateTime.Now;
           newNotificationPM.Description = desc;

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

           newNotificationPM.AssigneToId =
               NotificationBase.
               CalcAssigneToId(newNotificationPM.Tenant, customerId, referentUserId, notificationDefinitionCode, oldassigneId);

           NotificationBase.CloseAllRelatedNotification(dbContext, newNotificationPM, newNotificationPM.NotificationDefinitionCode);
           notificationUpdateService.Update(newNotificationPM, true);
       }

       private DeclarationPM GetConnectedDeclarationPM(GuaranteePM dirtyEntityPM)
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
