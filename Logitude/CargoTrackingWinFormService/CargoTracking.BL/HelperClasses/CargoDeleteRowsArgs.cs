using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrackingWinFormService.CargoTracking.BL.HelperClasses
{
    public class CargoDeleteRowsArgs
    {
        public string TableName { get; set; }
        public string KeyName { get; set; }
        public string ConnectionString { get; set; }
        public List<string> IdsList { get; set; }
        public string SingleId { get; set; }
        public bool ReturnDeleteIdsAsString { get; set; }
    }
}
