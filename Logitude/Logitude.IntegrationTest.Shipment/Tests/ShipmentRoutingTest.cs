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

            await UpdateShipmentWithRoutingPorts();

            await UpdateShipmentWithPreCarriageATD();
            await UpdateShipmentWithPreCarriageATA();

            await UpdateShipmentWitMainCarriageATD();
            await UpdateShipmentWitMainCarriageATA();

            await UpdateShipmentWitTransshipment1ATD();
            await UpdateShipmentWitTransshipment1ATA();

            await UpdateShipmentWitTransshipment2ATD();
            await UpdateShipmentWitTransshipment2ATA();

            await UpdateShipmentWitTransshipment3ATD();
            await UpdateShipmentWitTransshipment3ATA();

            await UpdateShipmentWithOnCarriageATD();
            await UpdateShipmentWithOnCarriageATA();
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
            this.StatusShouldBe("Order", 0);
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
        }

        private async Task UpdateShipmentWithPreCarriageATD()
        {
            // EventTypes.Code = 'PRCD'
            shipmentPM.PreCarriageATD = DateTime.UtcNow;
            shipmentPM = await service.UpdateShipment(shipmentPM);
            this.StatusShouldBe("Departed", 60);

            shipmentPM.PreCarriageATD = null;
            shipmentPM = await service.UpdateShipment(shipmentPM);
            this.StatusShouldBe("Order", 0);
        }
        private async Task UpdateShipmentWithPreCarriageATA()
        {
            // EventTypes.Code = 'PRCA'
            shipmentPM.PreCarriageATA = DateTime.UtcNow;
            shipmentPM = await service.UpdateShipment(shipmentPM);
            this.StatusShouldBe("Arrived", 70);

            shipmentPM.PreCarriageATA = null;
            shipmentPM = await service.UpdateShipment(shipmentPM);
            this.StatusShouldBe("Order", 0);
        }

        private async Task UpdateShipmentWitMainCarriageATD()
        {
            // EventTypes.Code = 'DEP'
            shipmentPM.MainCarriageATD = DateTime.UtcNow;
            shipmentPM = await service.UpdateShipment(shipmentPM);
            this.StatusShouldBe("Departed", 80);

            shipmentPM.MainCarriageATD = null;
            shipmentPM = await service.UpdateShipment(shipmentPM);
            this.StatusShouldBe("Order", 0);
        }
        private async Task UpdateShipmentWitMainCarriageATA()
        {
            // EventTypes.Code = 'ARR'
            shipmentPM.MainCarriageATA = DateTime.UtcNow;
            shipmentPM = await service.UpdateShipment(shipmentPM);
            this.StatusShouldBe("Arrived", 90);

            shipmentPM.MainCarriageATA = null;
            shipmentPM = await service.UpdateShipment(shipmentPM);
            this.StatusShouldBe("Order", 0);
        }

        private async Task UpdateShipmentWitTransshipment1ATD()
        {
            // EventTypes.Code = 'T1DP'
            shipmentPM.Transshipment1ATD = DateTime.UtcNow;
            shipmentPM = await service.UpdateShipment(shipmentPM);
            this.StatusShouldBe("Departed", 100);

            shipmentPM.Transshipment1ATD = null;
            shipmentPM = await service.UpdateShipment(shipmentPM);
            this.StatusShouldBe("Order", 0);
        }
        private async Task UpdateShipmentWitTransshipment1ATA()
        {
            // EventTypes.Code = 'T1AR'
            shipmentPM.Transshipment1ATA = DateTime.UtcNow;
            shipmentPM = await service.UpdateShipment(shipmentPM);
            this.StatusShouldBe("Arrived", 110);

            shipmentPM.Transshipment1ATA = null;
            shipmentPM = await service.UpdateShipment(shipmentPM);
            this.StatusShouldBe("Order", 0);
        }

        private async Task UpdateShipmentWitTransshipment2ATD()
        {
            // EventTypes.Code = 'T2DP'
            shipmentPM.Transshipment2ATD = DateTime.UtcNow;
            shipmentPM = await service.UpdateShipment(shipmentPM);
            this.StatusShouldBe("Departed", 120);

            shipmentPM.Transshipment2ATD = null;
            shipmentPM = await service.UpdateShipment(shipmentPM);
            this.StatusShouldBe("Order", 0);
        }
        private async Task UpdateShipmentWitTransshipment2ATA()
        {
            // EventTypes.Code = 'T2AR'
            shipmentPM.Transshipment2ATA = DateTime.UtcNow;
            shipmentPM = await service.UpdateShipment(shipmentPM);
            this.StatusShouldBe("Arrived", 130);

            shipmentPM.Transshipment2ATA = null;
            shipmentPM = await service.UpdateShipment(shipmentPM);
            this.StatusShouldBe("Order", 0);
        }

        private async Task UpdateShipmentWitTransshipment3ATD()
        {
            // EventTypes.Code = 'T3DP'
            shipmentPM.Transshipment3ATD = DateTime.UtcNow;
            shipmentPM = await service.UpdateShipment(shipmentPM);
            this.StatusShouldBe("Departed", 140);

            shipmentPM.Transshipment3ATD = null;
            shipmentPM = await service.UpdateShipment(shipmentPM);
            this.StatusShouldBe("Order", 0);
        }
        private async Task UpdateShipmentWitTransshipment3ATA()
        {
            // EventTypes.Code = 'T3AR'
            shipmentPM.Transshipment3ATA = DateTime.UtcNow;
            shipmentPM = await service.UpdateShipment(shipmentPM);
            this.StatusShouldBe("Arrived", 150);

            shipmentPM.Transshipment3ATA = null;
            shipmentPM = await service.UpdateShipment(shipmentPM);
            this.StatusShouldBe("Order", 0);
        }

        private async Task UpdateShipmentWithOnCarriageATD()
        {
            // EventTypes.Code = 'ONCD'
            shipmentPM.OnCarriageATD = DateTime.UtcNow;
            shipmentPM = await service.UpdateShipment(shipmentPM);
            this.StatusShouldBe("Departed", 160);

            shipmentPM.OnCarriageATD = null;
            shipmentPM = await service.UpdateShipment(shipmentPM);
            this.StatusShouldBe("Order", 0);
        }
        private async Task UpdateShipmentWithOnCarriageATA()
        {
            // EventTypes.Code = 'ONCA'
            shipmentPM.OnCarriageATA = DateTime.UtcNow;
            shipmentPM = await service.UpdateShipment(shipmentPM);
            this.StatusShouldBe("Arrived", 170);

            shipmentPM.OnCarriageATA = null;
            shipmentPM = await service.UpdateShipment(shipmentPM);
            this.StatusShouldBe("Order", 0);
        }

        private void StatusShouldBe(string statusName, int statusWeight)
        {
            Assert.IsTrue(shipmentPM.StatusName == statusName);
            Assert.IsTrue(shipmentPM.StatusWeight == statusWeight);
        }
    }
}

/*
select 
EventTypes.Code, EventTypes.EnglishName,
EntityStatus.Code, EntityStatus.Name, EntityStatus.StatusWeight
from
EventTypes join EntityStatus
on EventTypes.EntityStatusId = EntityStatus.Id
where EventTypes.Tenant = 0 and EntityStatus.Tenant = 0
and EventTypes.Code = 'T1DP'
*/
