using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses
{
    public class CargoTrackingIncrementalArgs
    {
        public int RecordsWating { get; set; }
        public DateTime? IncrementalLastRun { get; set; }
        public string OldestUpdateStillWaiting { get; set; }
        public int TotalUpdatedLast10Minutes { get; set; }
        public DateTime? DateOfOldestUpdateStillWaiting { get; set; }
    }
}
