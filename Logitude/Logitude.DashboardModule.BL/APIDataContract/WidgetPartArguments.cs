using Logitude.DashboardModule.BL.EntityPMs;
using System;
using System.Collections.Generic;

namespace Logitude.DashboardModule.BL.APIDataContract
{
    public class WidgetPartArguments
    {
        public WidgetPM Widget { get; set; }
        public string MeasureFieldId { get; set; }
        public string GroupById { get; set; }
    }
}
