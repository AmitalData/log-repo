using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrackingWinService.Helper
{
    public static class ApplicationInfo
    {
        public static string SourceConnection { get; set; }
        public static DateTime? StartDate { get; set; }
        public static DateTime? EndDate { get; set; }
        public static string DestinationConnection { get; set; }
        public static int UpdateCargoTrackingSleepTime { get; set; }
        public static bool RunCargoTrackingImmediately { get; set; }
        public static string Mode { get; set; }
        public static int UpdateCounter { get; set; }
        public static int Shipments { get; set; }
        public static int Ports { get; set; }
        public static int Cards { get; set; }
        public static int Countries { get; set; }
        public static int TransportModes { get; set; }
        public static int ShipmentComputedFields { get; set; }
        public static int ShipmentMasterDatas { get; set; }
        public static string ErrorLogs { get; set; }


    }
}
