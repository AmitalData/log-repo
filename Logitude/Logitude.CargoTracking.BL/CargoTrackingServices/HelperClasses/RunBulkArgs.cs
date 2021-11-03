using Logitude.CargoTracking.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses
{
    public class RunBulkArgs
    {
        public List<CargoTrackingShipment> CargoTrackingShipments { get; set; }
        public string TableName { get; set; }
        public string InnerTableName { get; set; }
        public string ConnectionString { get; set; }
        public bool IsUpdateFromBuild { get; set; }
    }
}
