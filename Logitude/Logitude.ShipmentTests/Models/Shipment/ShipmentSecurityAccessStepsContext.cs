using Logitude.SecurityTests.Models.Login;
using System.Collections.Generic;

namespace Logitude.SecurityTests.Models.Shipment
{
    public class ShipmentSecurityAccessStepsContext
    {
        public ShipmentSecurityAccessStepsContext()
        {
            FirstUserShipment = new ShipmentPM();
            SecondUserShipment = new ShipmentPM();
        }

        public ShipmentPM FirstUserShipment { get; set; }
        public ShipmentPM SecondUserShipment { get; set; }
        public UserData FirstUser { get; set; }
        public UserData SecondUser { get; set; }
    }
}