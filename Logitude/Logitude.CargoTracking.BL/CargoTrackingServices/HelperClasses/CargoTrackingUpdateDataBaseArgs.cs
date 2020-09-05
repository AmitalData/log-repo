using Logitude.CargoTracking.BL.CargoTrackingServices.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses
{
    public class CargoTrackingUpdateDataBaseArgs
    {
        public CargoArgs buildCargoArgs { get; set; }
        public int NumberOfBulkPerTime { get; set; }
        public bool? IsUpdateAfterFinished  { get; set; }
        public CargoTrackingArguments CargoTrackingArguments  { get; set; }
        public bool IsUpdateFromBuild { get; set; }
     }
}
