using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTrackingTests.Models
{
    public class ShipmentContext
    {
        public ShipmentPM MasterShipment { get; set; }
        public ShipmentPM HouseShipment { get; set; }
        public ShipmentPM DirectShipment { get; set; }
    }
}
