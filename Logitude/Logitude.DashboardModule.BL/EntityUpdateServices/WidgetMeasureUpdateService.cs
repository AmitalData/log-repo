using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.Server.Tools.Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DashboardModule.BL.EntityUpdateServices
{
    public partial class WidgetMeasureUpdateService
    {
        protected override void OnCreating(WidgetMeasurePM entityPM, WidgetPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("WidgetMeasure", entityPM.Tenant);
            }

            entityPM.WidgetId = entityParentPM.Id;
        }
    }
}
