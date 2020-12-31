using System.Collections.Generic;

namespace Logitude.SecurityTests.Models.Shipment
{
    public class ShipmentSecurityAccessStepsContext
    {
        public ShipmentSecurityAccessStepsContext()
        {
            ShipmentPM = new ShipmentPM();
            OtherShipmentPM = new ShipmentPM();
        }

        public ShipmentPM ShipmentPM { get; set; }
        public ShipmentPM OtherShipmentPM { get; set; }
        public IEnumerable<ShipmentPM> ShipmentPMs { get; set; }
    }
}