using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
     public partial  class NotificationReplyUpdateService
    {

         protected override void OnCreating(NotificationReplyPM entityPM, NotificationPM entityParentPM)
         {
             entityPM.NotificationId = entityParentPM.Id;
             entityPM.Tenant = entityParentPM.Tenant;

         }

    }
}
