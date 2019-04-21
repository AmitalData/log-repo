using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDataService
{
    public static class ApplicationInfo
    {
        public static string SourceConnection { get; set; }
        public static string DestinationConnection { get; set; }
        public static int UpdateWarehouseSleepTime { get; set; }
        public static List<DayOfWeekClass> WarehouseBuildDays { get; set; }
        public static string WarehouseBuildHours { get; set; }
        public static int RetryBuildWithinHours { get; set; }
        
        public static string Mode { get; set; }
        public static bool UpdatingServiceWorking { get; set; }
        public static bool BliudingServiceWorking { get; set; }
        public static List<DayOfWeekClass> Days { get; set; }
        public static string GlobalSourceConnection { get; set; }

    }
}
