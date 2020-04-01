using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.IntegrationTest.Core;
using Logitude.IntegrationTest.Shipment.EntitiesInitializer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.Shipment.Tests
{
    [TestClass]
    public class ShipmentProfitTest
    {
        [TestMethod]
        public async Task TestShipmentProfit()
        {
            EntityInitializerArguments args = new EntityInitializerArguments()
            {
                DirectionId = "E",
                TransportModeId = "A",
                ShipmentLevelCode = "D",
                ReceivableQuantity = 1,
                ReceivableUnitPrice = 20,
            };

            EntityInitializerFactory factory = new EntityInitializerFactory();
            IEntityInitializer initializer;

            initializer = factory.GetInitializer("Shipment");
            ShipmentPM entityPM = (ShipmentPM)initializer.Create(args);
            HttpResponseMessage postShipmentResponse = await RestClientService.PostAsync(entityPM, "shipment");
            Assert.IsTrue(postShipmentResponse.StatusCode.ToString() == "OK");

            ShipmentPM postedShipmentPM = RestClientService.ParseResponse<ShipmentPM>(postShipmentResponse);
            string shipmentId = postedShipmentPM.Id;
            HttpResponseMessage requestedShipmentResponse = await RestClientService.GetAsync("Shipment/GetSingle?id=" + shipmentId);
            Assert.IsTrue(requestedShipmentResponse.StatusCode.ToString() == "OK");

            initializer = factory.GetInitializer("ShipmentReceivable");
            ShipmentPM requestedShipment = RestClientService.ParseResponse<ShipmentPM>(requestedShipmentResponse);
            requestedShipment.ShipmentReceivables.Add((ShipmentReceivablePM)initializer.Create(args));
            requestedShipment.ShipmentReceivables.Add((ShipmentReceivablePM)initializer.Create(args));

            //shipment.PostShipment
            //await shipment.PostShipment("D", "E", "A");
            //shipmentPM = await shipment.GetShipment(ShipmentVariables.ShipmentId);
            //ShipmentPM entityPM= UpdateShipmentAirExport(shipmentPM);
            //await TestReceivables(shipmentPM, quantity, unitPrice);
        }
    }
}
