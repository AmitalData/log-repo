using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrackingService.Helper
{
    public static class ApplicationInfo
    {
        public static string SourceConnection { get; set; }
        public static string DestinationConnection { get; set; }
        public static int UpdateCargoTrackingSleepTime { get; set; }
        public static bool RunCargoTrackingImmediately { get; set; }
        public static string Mode { get; set; }

    }
}
