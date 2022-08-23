using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Infrastructure.BL.EntityQueryServices
{
    public partial class WidgetQueryService
    {
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
