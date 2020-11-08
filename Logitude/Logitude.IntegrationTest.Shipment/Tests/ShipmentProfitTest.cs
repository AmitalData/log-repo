using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.IntegrationTest.Core;
using Logitude.IntegrationTest.Shipment.EntitiesInitializer;
using Logitude.IntegrationTest.Shipment.Services;
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

            await UpdateShipmentWithEmptyAmounts();

            await UpdateShipmentWithFilledAmounts();
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

        private async Task UpdateShipmentWithEmptyAmounts()
        {
            EntityInitializerArguments args = new EntityInitializerArguments()
            {
                CurrencyId = CorePreparationVariables.LocalCurrencyId,
                CurrencyRate = 1,
                ProfitCurrencyRate = 4,
            };

            initializer = factory.GetInitializer("ShipmentPayable");
            shipmentPM.ShipmentPayables.Add((ShipmentPayablePM)initializer.Create(args));

            initializer = factory.GetInitializer("ShipmentReceivable");
            shipmentPM.ShipmentReceivables.Add((ShipmentReceivablePM)initializer.Create(args));

            shipmentPM = await service.UpdateShipment(shipmentPM);

            Assert.IsTrue(this.IsNullOrZero(shipmentPM.OpenPayablesInLocalCurrency));
            Assert.IsTrue(this.IsNullOrZero(shipmentPM.OpenPayablesInProfitCurrency));
            Assert.IsTrue(this.IsNullOrZero(shipmentPM.OpenReceivablesInLocalCurrency));
            Assert.IsTrue(this.IsNullOrZero(shipmentPM.OpenReceivablesInProfitCurrency));
            Assert.IsTrue(this.IsNullOrZero(shipmentPM.ProfitInLocalCurrency));
            Assert.IsTrue(this.IsNullOrZero(shipmentPM.ProfitInProfitCurrency));
            Assert.IsTrue(shipmentPM.ShipmentPayableStatusCode == "NOPA");
            Assert.IsTrue(shipmentPM.ShipmentReceivableStatusCode == "NORE");
        }

        private async Task UpdateShipmentWithFilledAmounts()
        {
            EntityInitializerArguments args1 = new EntityInitializerArguments()
            {
                CurrencyId = CorePreparationVariables.LocalCurrencyId,
                CurrencyRate = 1,
                ProfitCurrencyRate = 4,
                PayableQuantity = 1,
                PayableUnitPrice = 10,
                ReceivableQuantity = 2,
                ReceivableUnitPrice = 20,
            };

            EntityInitializerArguments args2 = new EntityInitializerArguments()
            {
                CurrencyId = CorePreparationVariables.ProfitCurrencyId,
                CurrencyRate = 4,
                ProfitCurrencyRate = 4,
                PayableQuantity = 1,
                PayableUnitPrice = 10,
                ReceivableQuantity = 2,
                ReceivableUnitPrice = 20,
            };

            initializer = factory.GetInitializer("ShipmentPayable");
            shipmentPM.ShipmentPayables.Add((ShipmentPayablePM)initializer.Create(args1));
            shipmentPM.ShipmentPayables.Add((ShipmentPayablePM)initializer.Create(args2));

            initializer = factory.GetInitializer("ShipmentReceivable");
            shipmentPM.ShipmentReceivables.Add((ShipmentReceivablePM)initializer.Create(args1));
            shipmentPM.ShipmentReceivables.Add((ShipmentReceivablePM)initializer.Create(args2));

            shipmentPM = await service.UpdateShipment(shipmentPM);

            Assert.IsTrue(shipmentPM.OpenPayablesInLocalCurrency == 50);
            Assert.IsTrue(shipmentPM.OpenPayablesInProfitCurrency == 12.5);
            Assert.IsTrue(shipmentPM.OpenReceivablesInLocalCurrency == 200);
            Assert.IsTrue(shipmentPM.OpenReceivablesInProfitCurrency == 50);
            Assert.IsTrue(shipmentPM.ProfitInLocalCurrency == 150);
            Assert.IsTrue(shipmentPM.ProfitInProfitCurrency == 37.5);
            Assert.IsTrue(shipmentPM.ShipmentPayableStatusCode == "OPEN");
            Assert.IsTrue(shipmentPM.ShipmentReceivableStatusCode == "OPEN");
        }

        private bool IsNullOrZero(double? value)
        {
            if (value == null)
            {
                return true;
            }

            else if (value.Value == 0)
            {
                return true;
            }

            return false;
        }
    }
}
