using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.Server.Tools.Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DashboardModule.BL.EntityUpdateServices
{
    public partial class UserPinnedDashboardUpdateService
    {
        protected override void OnCreating(UserPinnedDashboardPM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("UserPinnedDashboard", entityPM.Tenant);               
            }
        }

        protected override void OnUpdating(UserPinnedDashboardPM entityPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
            }
        }
    }
}
