using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.Data;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
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
   public partial class ImporterDespositionUpdateService
    {
       protected override void OnCreating(ImporterDespositionPM entityPM, EntityPM entityParentPM)
       {
          entityPM.Id = IdCounter.GetNumber("Customs.ImporterDesposition", entityPM.Tenant);
       }

       protected override void OnUpdating(ImporterDespositionPM entityPM)
       {
           UpdateNotification(entityPM);
       }

       private void UpdateNotification(ImporterDespositionPM dirtyEntityPM)
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
                   case EventContextTagModel.ProccessEnum.VE_3700_ImporterPeriodicDeclarationReplyResponseServiceNew:
                       notificationDefinitionCode = "3700N";
                       break;
                   case EventContextTagModel.ProccessEnum.VE_3700_ImporterPeriodicDeclarationReplyResponseServiceUpdate:
                       notificationDefinitionCode = "3700U";
                       break;
               }
           }

           if (!string.IsNullOrWhiteSpace(notificationDefinitionCode))
           {
               DoUpdateNotification(dirtyEntityPM, loggingUserId, notificationDefinitionCode);
           }
       }

       private void DoUpdateNotification(ImporterDespositionPM dirtyEntityPM, string loggingUserId, string notificationDefinitionCode)
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
               case "3700N":
                   newNotificationPM.NotificationDefinitionCode = "3700N";
                   newNotificationPM.AssigneToNotificationTypeCode = "I";
                   desc = "תצהיר תקופתי חדש לספק ";
                   break;
               case "3700U":
                   newNotificationPM.NotificationDefinitionCode = "3700U";
                   newNotificationPM.AssigneToNotificationTypeCode = "I";
                   desc = "עדכון תצהיר תקופתי לספק ";
                   break;
           }

           if (!string.IsNullOrWhiteSpace(dirtyEntityPM.VendorID))
           {
               desc = desc + "\n" + dirtyEntityPM.VendorID + "(" + dirtyEntityPM.VendorName + ")";
           }

           if (!string.IsNullOrWhiteSpace(dirtyEntityPM.ImporterlId))
           {
               desc = desc + "\n" + "יבואן " + dirtyEntityPM.ImporterCode + "(" + dirtyEntityPM.ImporterName + ")";
           }

           if (!string.IsNullOrWhiteSpace(dirtyEntityPM.ImporterDepositionStatusCode))
           {
               ImporterPeriodicDeclarStatusQueryService statusQueryService = new ImporterPeriodicDeclarStatusQueryService(dirtyEntityPM.Tenant);
               ImporterPeriodicDeclarStatusPM importerPeriodicDeclarationStatusPM = statusQueryService.GetSingle(dirtyEntityPM.ImporterDepositionStatusCode, false, true);
               desc = desc + "\n" + "סטטוס " + dirtyEntityPM.ImporterDepositionStatusCode + "(" + importerPeriodicDeclarationStatusPM.LocalName + ")";
           }

           newNotificationPM.EntityId = dirtyEntityPM.Id;
           newNotificationPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.ImporterDesposition");
           newNotificationPM.Reference1Number = dirtyEntityPM.DepositionNumber;
           newNotificationPM.ResponseNotes = dirtyEntityPM.ErrorMessage + ", " + dirtyEntityPM.NotesToAgent;
           newNotificationPM.CreateDate = DateTime.Now;
           newNotificationPM.DueDate = DateTime.Now;
           newNotificationPM.Description = desc;
           if (dirtyEntityPM != null && !string.IsNullOrWhiteSpace(dirtyEntityPM.ImporterlId)) // newNotificationPM.CustomerId = dirtyEntityPM.ImporterlId; // moran 20.6.16 - Task 20789
           { // moran 13.9.16 - Bug 22397 - change handle
               var clientQueryService = new ClientQueryService(dirtyEntityPM.Tenant);
               var client = new ClientPM();

               client = clientQueryService.GetSingle(dirtyEntityPM.ImporterlId, true, false);
               if (client != null)
               {

                   ICommonDataContext CommonContext = CommonDataContext.GetContext(dirtyEntityPM.Tenant);
                   var cardRepository = new CardRepository(CommonContext);
                   Card card = cardRepository.GetSingleCardByVatNumber(client.Code, dirtyEntityPM.Tenant);
                   if (card != null && !String.IsNullOrWhiteSpace(card.Id))
                   {
                       newNotificationPM.CustomerId = card.Id;
                   }
               }
           }
           NotificationBase.CloseAllRelatedNotification(dbContext, newNotificationPM, newNotificationPM.NotificationDefinitionCode);
           notificationUpdateService.Update(newNotificationPM, true);
       }
    }
}
