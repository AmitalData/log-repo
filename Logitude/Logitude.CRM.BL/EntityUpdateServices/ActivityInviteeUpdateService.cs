using Logitude.Server.Tools.Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.EntityUpdateServices
{
    public partial class ActivityInviteeUpdateService
    {
        protected override void OnCreating(EntityPMs.ActivityInviteePM entityPM, EntityPMs.ActivityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("ActivityInvitee", entityPM.Tenant);
            }

            entityPM.ActivityId = entityParentPM.Id;             
        }
    }
}
