using Logitude.CargoTracking.BL.CargoTrackingServices.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses
{
    public class CargoTrackingUpdateDataBaseArgs
    {
        public CargoTrackingArgs BuildCargoArgs { get; set; }
        public int NumberOfBulkPerTime { get; set; }
        public bool? IsUpdateAfterFinished  { get; set; }
        public CargoTrackingArguments CargoTrackingArguments  { get; set; }
        public bool IsUpdateFromBuild { get; set; }
        public DataTable DataTableSchema { get; set; }
        public DateTime? MaxDate { get; internal set; }
        public List<string> ForwardingShipmentsIds { get; internal set; }
        public string ShipmentsWaterMark { get; set; }
    }
}
