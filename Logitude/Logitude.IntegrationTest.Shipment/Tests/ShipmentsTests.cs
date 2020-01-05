using System;
using System.Threading.Tasks;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.IntegrationTest.Shipment
{
    [TestClass]
    public class ShipmentsTests
    {
        [TestMethod]
        public async Task GetSingleShipment()
        {
            ShipmentPM entityPM = GetShipment();
        }

        private ShipmentPM GetShipment()
        {
            ShipmentPM shipmentPM = new ShipmentPM();
            return shipmentPM;
        }
    }
}
