using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.DashboardModule.Data;
using Logitude.DashboardModule.Data.EntityKeys;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DashboardModule.BL.EntityQueryServices
{
    public partial class WidgetQueryService
    {
        public override void GetComposition(EntityKeyFields entityKeys, WidgetPM entityPM)
        {
            IDashboardContext context = MainContext as IDashboardContext;
            WidgetKeys widgetKeys = entityKeys as WidgetKeys;

            WidgetMeasureQueryService widgetMeasureQueryService = new WidgetMeasureQueryService(context);
            entityPM.WidgetMeasures = widgetMeasureQueryService.GetMulti(widgetKeys, true);
        }

        public List<WidgetPM> GetWidgetsByDashboardId(string dashboardId, int tenant)
        {
            List<WidgetPM> result = new List<WidgetPM>();
            List<Widget> widgets = repository.GetWidgetsByDashboardId(dashboardId, tenant);

            foreach (Widget entity in widgets)
            {
                EntityPM = new WidgetPM();
                mapping.CustomPOCOToPM(EntityPM, entity);
                mapping.POCOToPM(EntityPM, entity);

                result.Add(EntityPM);
            }

            return result;
        }
    }
}
