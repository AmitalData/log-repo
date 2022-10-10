using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.DashboardModule.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DashboardModule.BL.DataProviders.WidgetsDataProviders
{
    public class KpiDataProviderService : BaseDataProviderService
    {
        public KpiDataProviderService(WidgetPM widget, AnalyticsFactsMetaData entity) : base(widget, entity)
        {
        }
    }
}
