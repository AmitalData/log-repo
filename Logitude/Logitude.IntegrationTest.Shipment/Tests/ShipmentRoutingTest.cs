using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.IntegrationTest.Shipment.EntitiesInitializer;
using Logitude.IntegrationTest.Shipment.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        DateTime todayDate;

        [TestMethod]
        public async Task TestShipmentRouting()
        {
            service = new ShipmentTestService();
            factory = new EntityInitializerFactory();
            todayDate = DateTime.UtcNow.Date;

            await CreateShipment();

            await GetShipment();

            await UpdateShipmentWithRoutingPorts();

            await UpdateShipmentWithMainCarriageDates();

            await UpdateShipmentWitTransshipment1Dates();

            // .. to be continue
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
            Assert.IsTrue(shipmentPM.Routing == "LHR , JFK");
            Assert.IsTrue(shipmentPM.OperationalDate == shipmentPM.CreateDateTime);
            Assert.IsNull(shipmentPM.MainCarriageFinalDestinationETA);
            Assert.IsNull(shipmentPM.MainCarriageFinalDestinationATA);

            //Assert.IsNull(shipmentPM.FinalArrivalDate);
            //Assert.IsNull(shipmentPM.EstimatedFinalArrivalDate);
            //Assert.IsNull(shipmentPM.ActualFinalArrivalDate);
            // DepartureArrivalFromDate
            // DepartureArrivalToDate
        }

        private async Task UpdateShipmentWithRoutingPorts()
        {
            shipmentPM.PreCarriageFromPortId = ShipmentVariables.PortMIAId;
            shipmentPM.PreCarriageToPortId = ShipmentVariables.PortLHRId;
            shipmentPM.PreCarriageTransportModeId = shipmentPM.TransportModeId;

            shipmentPM.Transshipment1FromPortId = ShipmentVariables.PortJFKId;
            shipmentPM.Transshipment1ToPortId = ShipmentVariables.PortSOUId;

            shipmentPM.Transshipment2FromPortId = ShipmentVariables.PortSOUId;
            shipmentPM.Transshipment2ToPortId = ShipmentVariables.PortNYCId;

            shipmentPM.Transshipment3FromPortId = ShipmentVariables.PortNYCId;
            shipmentPM.Transshipment3ToPortId = ShipmentVariables.PortJFKId;

            shipmentPM.OnCarriageFromPortId = ShipmentVariables.PortJFKId;
            shipmentPM.OnCarriageToPortId = ShipmentVariables.PortMANId;
            shipmentPM.OnCarriageTransportModeId = shipmentPM.TransportModeId;

            shipmentPM = await service.UpdateShipment(shipmentPM);
            Assert.IsTrue(shipmentPM.TransportModeId == shipmentPM.PreCarriageTransportModeId);
            Assert.IsTrue(shipmentPM.TransportModeId == shipmentPM.OnCarriageTransportModeId);
            Assert.IsTrue(shipmentPM.PreCarriageToPortId == shipmentPM.MainCarriageFromPortId);
            Assert.IsTrue(shipmentPM.MainCarriageToPortId == shipmentPM.Transshipment1FromPortId);
            Assert.IsTrue(shipmentPM.Transshipment1ToPortId == shipmentPM.Transshipment2FromPortId);
            Assert.IsTrue(shipmentPM.Transshipment2ToPortId == shipmentPM.Transshipment3FromPortId);
            Assert.IsTrue(shipmentPM.Transshipment3ToPortId == shipmentPM.OnCarriageFromPortId);
            Assert.IsTrue(shipmentPM.Transshipment3ToPortId == shipmentPM.MainCarriageFinalDestinationPortId);
            Assert.IsTrue(shipmentPM.Routing == "MIA , LHR , JFK , MAN");
        }

        private async Task UpdateShipmentWithMainCarriageDates()
        {
            shipmentPM.MainCarriageETD = todayDate.AddDays(-10);
            shipmentPM.MainCarriageETA = todayDate.AddDays(-10).AddHours(1);
            shipmentPM.MainCarriageATD = todayDate.AddDays(-9);
            shipmentPM.MainCarriageATA = todayDate.AddDays(-9).AddHours(1);

            shipmentPM = await service.UpdateShipment(shipmentPM);
            Assert.IsTrue(shipmentPM.OperationalDate == shipmentPM.MainCarriageATD);
            Assert.IsTrue(shipmentPM.MainCarriageFinalDestinationETA == shipmentPM.MainCarriageETA);
            Assert.IsTrue(shipmentPM.MainCarriageFinalDestinationATA == shipmentPM.MainCarriageATA);
        }

        private async Task UpdateShipmentWitTransshipment1Dates()
        {
            shipmentPM.Transshipment1ETD = todayDate.AddDays(-8);
            shipmentPM.Transshipment1ETA = todayDate.AddDays(-8).AddHours(1);
            shipmentPM.Transshipment1ATD = todayDate.AddDays(-7);
            shipmentPM.Transshipment1ATA = todayDate.AddDays(-7).AddHours(1);

            shipmentPM = await service.UpdateShipment(shipmentPM);
            Assert.IsTrue(shipmentPM.OperationalDate == shipmentPM.MainCarriageATD);
            Assert.IsTrue(shipmentPM.MainCarriageFinalDestinationETA == shipmentPM.Transshipment1ETA);
            Assert.IsTrue(shipmentPM.MainCarriageFinalDestinationATA == shipmentPM.Transshipment1ATA);
        }


    }
}
