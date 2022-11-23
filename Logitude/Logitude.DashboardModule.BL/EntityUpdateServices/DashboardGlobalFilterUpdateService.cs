using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.Server.Tools.Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DashboardModule.BL.EntityUpdateServices
{
    public partial class DashboardGlobalFilterUpdateService
    {
        protected override void OnCreating(DashboardGlobalFilterPM entityPM, DashboardPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("DashboardGlobalFilter", entityPM.Tenant);
            }

            entityPM.DashboardId = entityParentPM.Id;
        }
    }
}
