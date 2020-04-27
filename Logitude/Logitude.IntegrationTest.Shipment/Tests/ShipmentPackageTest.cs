using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.IntegrationTest.Core;
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
    public class ShipmentPackageTest
    {
        string shipmentId;
        ShipmentPM shipmentPM;
        ShipmentTestService service;
        IEntityInitializer initializer;
        EntityInitializerFactory factory;

        [TestMethod]
        public async Task TestShipmentProfit()
        {
            service = new ShipmentTestService();
            factory = new EntityInitializerFactory();

            await CreateShipment();

            await GetShipment();

            await UpdateShipmentWithPackages();
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

        private async Task UpdateShipmentWithPackages()
        {
            EntityInitializerArguments args1 = new EntityInitializerArguments()
            {
                PackageQuantity = 10,
                PackageWeight = 100,
                PackageLength = 100,
                PackageWidth = 50,
                PackageHeight = 70,
                PackgeVolumetricWeight = 0.583,
            };

            EntityInitializerArguments args2 = new EntityInitializerArguments()
            {
                PackageQuantity = 200,
                PackageWeight = 150,
                PackageLength = 120,
                PackageWidth = 120,
                PackageHeight = 120,
                PackgeVolumetricWeight = 57.6,
            };

            EntityInitializerArguments args3 = new EntityInitializerArguments()
            {
                PackageQuantity = 40,
                PackageWeight = 60,
                PackageLength = 80,
                PackageWidth = 150,
                PackageHeight = 100,
                PackgeVolumetricWeight = 8,
            };
            
            initializer = factory.GetInitializer("ShipmentPackage");
            shipmentPM.ShipmentPackages.Add((ShipmentPackagePM)initializer.Create(args1));
            shipmentPM.ShipmentPackages.Add((ShipmentPackagePM)initializer.Create(args2));
            shipmentPM.ShipmentPackages.Add((ShipmentPackagePM)initializer.Create(args3));

            shipmentPM = await service.UpdateShipment(shipmentPM);
            Assert.IsTrue(shipmentPM.Volume == 397.100);
            Assert.IsTrue(shipmentPM.ChargeableWeight == 66.183);
            Assert.IsTrue(shipmentPM.VolumetricWeight == 66.183);
            Assert.IsTrue(shipmentPM.GrossWeight == 310.000);
            Assert.IsTrue(shipmentPM.NumberOfPackages == 250);
        }
    }
}
