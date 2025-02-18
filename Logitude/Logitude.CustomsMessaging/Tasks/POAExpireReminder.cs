using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Def.EntityQueryServicesExt;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools.Contracts;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.CustomsMessaging.Tasks
{
    public class POAExpireReminder : IPOAExpireReminder
    {
        public void StartRun(string taskId, int seedDefaultTenant)
        {

            var customsSettingQueryService = new CustomsSettingQueryService(seedDefaultTenant);
            var allCustomsSetting = customsSettingQueryService.GetAll();
            allCustomsSetting.ForEach(t => RunPerTenant(t));


        }

        private void RunPerTenant(CustomsSettingPM t)
        {
            LogMessagingUtil.Instance.AppendLine("tenant: " + t.Tenant);

            ICustomContext dbContext = CustomContext.GetContext(t.Tenant);
            string desc = "";
            var notificationUpdateService = new NotificationUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), t.Tenant);
            var clientUpdateService = new ClientUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), t.Tenant);
            var notificationQueryService = new NotificationQueryService(dbContext);
            var clientQueryService = new ClientQueryService(t.Tenant);
            var clients = clientQueryService.GetAllClientsPOAExpire(t.Tenant);
            LogMessagingUtil.Instance.AppendLine(clients.Count + " clients found");

            foreach (var client in clients)
            {
                LogMessagingUtil.Instance.AppendLine("notification for client: id=" + client.Id + "code=" + client.Code);

                var newNotificationPM = new NotificationPM();
                newNotificationPM.ChangeSetOp = ChangeSetOperation.Insert;
                newNotificationPM.Tenant = t.Tenant;

                newNotificationPM.NotificationDefinitionCode = "3730P";
                newNotificationPM.AssigneToNotificationTypeCode = "A";
                desc = "תוקף יפוי כח עומד לפוג ללקוח ";
                newNotificationPM.Reference2Number = "";
                newNotificationPM.CreateDate = DateTime.Now;
                newNotificationPM.DueDate = DateTime.Now;
                newNotificationPM.Description = desc;

                ICommonDataContext CommonContext = CommonDataContext.GetContext(t.Tenant);
                var cardRepository = new CardRepository(CommonContext);
                Card card = cardRepository.GetSingleCardByVatNumber(client.Code, t.Tenant);
                if (card != null && !String.IsNullOrWhiteSpace(card.Id))
                {
                    newNotificationPM.CustomerId = card.Id;
                }
                //newNotificationPM.CustomerId = client.Id;
                newNotificationPM.AssigneToId = NotificationBase.
                    CalcAssigneToId(t.Tenant, client.Code, "", "3730P", "");

                NotificationBase.CloseAllRelatedNotification(dbContext, newNotificationPM, newNotificationPM.NotificationDefinitionCode);
                notificationUpdateService.Update(newNotificationPM, true);
                client.IsPOAExpireReminderSent = true;
                client.ChangeSetOp = ChangeSetOperation.Update;
                clientUpdateService.Update(client, true);

            }
        }
    }

    
}
