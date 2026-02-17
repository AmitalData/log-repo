using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class NotificationUpdateService : EntityUpdateService<Notification, NotificationPM, EntityPM>
    {

        protected override void OnCreating(NotificationPM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("Customs.Notification", entityPM.Tenant);
            entityPM.BadjCount = true;

        }
        protected override void OnUpdating(NotificationPM entityPM)
        {
            if (String.IsNullOrWhiteSpace( entityPM.CreatedByRequestID ))
            {
                entityPM.CreatedByRequestID = Logitude.Customs.Def.Messaging.Customs.RequestSheetContext.Current.GetContextOrDefault().CustomsRequestsSheetId;
            }
            LogMessagingUtil.Instance.Append("Notification : ")
                .Append("AssigneToName=").Append(entityPM.AssigneToName)
                .Append(";CustomerName=").Append(entityPM.CustomerName)
                .AppendLine(";CustomsRequestsSheetId=").Append(Logitude.Customs.Def.Messaging.Customs.RequestSheetContext.Current.GetContextOrDefault().CustomsRequestsSheetId);
                ;
        }

        protected override void UpdateComposition(NotificationPM entityPM)
        {
            NotificationReplyUpdateService notificationReplyUpdateService = new NotificationReplyUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            notificationReplyUpdateService.UpdateMulti(entityPM.NotificationRplies, entityPM.DeletedNotificationRplies, entityPM, false);

            base.UpdateComposition(entityPM);
        }

    }
}
