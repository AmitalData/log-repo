using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.IntegrationTest.Shipment.Services;
using Logitude.IntegrationTest.Shipment.EntitiesInitializer;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.Shipment.Tests
{
    [TestClass]
    public class ShipmentRoutingTest
    {
        string shipmentId;
        ShipmentPM shipmentPM;
        ShipmentTestService service;
        IEntityInitializer initializer;
        EntityInitializerFactory factory;

        [TestMethod]
        public async Task TestShipmentStatus()
        {
            service = new ShipmentTestService();
            factory = new EntityInitializerFactory();

            await CreateShipment();

            await GetShipment();

            await UpdateShipmentWitMainCarriageActualDates();
        }

        private async Task CreateShipment()
        {
            EntityInitializerArguments args = new EntityInitializerArguments()
            {
                DirectionId = "E",
                TransportModeId = "A",
                ShipmentLevelCode = "D",
            };

            initializer = factory.GetInitializer("Shipment");
            ShipmentPM entityPM = (ShipmentPM)initializer.Create(args);
            shipmentId = await service.CreateShipment(entityPM);
        }

        private async Task GetShipment()
        {
            shipmentPM = await service.GetShipment(shipmentId);
        }

        private async Task UpdateShipmentWitMainCarriageActualDates()
        {

        }
    }
}
