using Logitude.Test.Base.Models.Login;

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
        public User FirstUser { get; set; }
        public User SecondUser { get; set; }
    }
}