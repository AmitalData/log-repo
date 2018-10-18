using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class NotificationTenantDefinitionQueryService : EntityQueryService<NotificationTenantDefinition, NotificationTenantDefinitionKeys, NotificationTenantDefinitionPM, object, NotificationTenantDefinitionKeys>
    {

        public NotificationTenantDefinitionPM GetNotificationTenantDefinition(int tenant, string Code)
        {
            var allNotification = (this.repository as NotificationTenantDefinitionRepository).GetAll(tenant)
                .Where(rec => rec.Code == Code);
            var notificationTenantDefinitionListPM = allNotification.ToList().Select(rec => this.GetEntityPM(rec)).ToList();
            return notificationTenantDefinitionListPM.FirstOrDefault();
        }

    }
}
