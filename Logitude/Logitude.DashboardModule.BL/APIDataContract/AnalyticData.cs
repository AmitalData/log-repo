using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.DashboardModule.Data.EntityPOCOs;
using System.Collections.Generic;

namespace Logitude.DashboardModule.BL.APIDataContract
{
    public class AnalyticData
    {
        public List<object> DataResult { get; set; }
        public List<AnalyticsFactsFieldsMetaData> Fields { get; set; }
    }
}
