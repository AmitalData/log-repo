using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses
{
    public class CargoTrackingArguments
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? Tenant { get; set; }
        public bool? AllData { get; set; }
        public int? ThreadNumber { get; set; }
        public string FormTableName { get; set; }
    }
}
