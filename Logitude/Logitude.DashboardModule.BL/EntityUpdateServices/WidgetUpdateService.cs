using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DashboardModule.BL.EntityUpdateServices
{
    public partial class WidgetUpdateService
    {
        protected override void OnCreating(EntityPMs.WidgetPM entityPM, EntityPMs.DashboardPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("Widget", entityPM.Tenant);
            }

            entityPM.DashboardId = entityParentPM.Id;
        }

        protected override void UpdateComposition(WidgetPM entityPM)
        {
            WidgetMeasureUpdateService widgetMeasureUpdateService = new WidgetMeasureUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            widgetMeasureUpdateService.UpdateMulti(entityPM.WidgetMeasures, entityPM.DeletedWidgetMeasures, entityPM, false);
        }
    }
}
