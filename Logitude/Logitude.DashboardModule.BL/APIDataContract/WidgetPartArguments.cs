using Logitude.DashboardModule.BL.EntityPMs;
using System;
using System.Collections.Generic;

namespace Logitude.DashboardModule.BL.APIDataContract
{
    public class WidgetPartArguments : WidgetArguments
    {
        public WidgetPM Widget { get; set; }
    }

    public class WidgetArguments
    {
        public string MeasureFieldId { get; set; }
        public string GroupByValue { get; set; }
        public string GroupBySecValue { get; set; }
    }
}
