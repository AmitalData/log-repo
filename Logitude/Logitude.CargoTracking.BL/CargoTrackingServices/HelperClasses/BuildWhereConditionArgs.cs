using Logitude.CargoTracking.BL.CargoTrackingServices.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses
{
    public class BuildWhereConditionArgs
    {
        public string TableName { get; set; }
        public string LastUpdate { get; set; }
        public CargoTrackingArguments CargoTrackingArguments { get; set; }
        public string Condition { get; set; }
     }
}
