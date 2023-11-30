using Logitude.Server.Tools.Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DashboardModule.BL.EntityUpdateServices
{
    public partial class DashboardSharedUserUpdateService
    {
        protected override void OnCreating(EntityPMs.DashboardSharedUserPM entityPM, EntityPMs.DashboardPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("DashboardSharedUser", entityPM.Tenant);
            }

            entityPM.DashboardId = entityParentPM.Id;
        }
    }
}
