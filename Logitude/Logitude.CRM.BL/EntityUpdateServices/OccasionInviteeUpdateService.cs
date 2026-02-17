using Logitude.Server.Tools.Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.EntityUpdateServices
{
    public partial class OccasionInviteeUpdateService
    {
        protected override void OnCreating(EntityPMs.OccasionInviteePM entityPM, EntityPMs.OccasionPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("OccasionInvitee", entityPM.Tenant);
            }

            entityPM.OccasionId = entityParentPM.Id;
        }
    }
}
