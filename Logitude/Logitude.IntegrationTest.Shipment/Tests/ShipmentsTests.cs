using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.IntegrationTest.Core;
using Logitude.IntegrationTest.Core.Login;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logitude.IntegrationTest.Shipment.Tests;

namespace Logitude.IntegrationTest.Shipment
{
    [TestClass]
    public class ShipmentsTests
    {

        ShipmentPM shipmentPM = new ShipmentPM();
        [TestMethod]
        public async Task TestDirectExportAirShipment()
        {
            await PostShipment("D", "E", "A");
            shipmentPM = await GetShipment(ShipmentVariables.ShipmentId);
            //ShipmentPM entityPM= UpdateShipmentAirExport(shipmentPM);
          
            //await TestReceivables(shipmentPM, quantity, unitPrice);

        }
        [TestMethod]
        private async Task TestReceivables()
        {
            int quantity = 5;
            int unitPrice = 5;
            await PostShipment("D", "E", "A");
            shipmentPM = await GetShipment(ShipmentVariables.ShipmentId);
            ShipmentPM entityPM = UpdateReceivables(shipmentPM, quantity, unitPrice);
            await PutShipment(entityPM);
            shipmentPM = await GetShipment(ShipmentVariables.ShipmentNumber);
            try
            {
                Assert.AreEqual("50", shipmentPM.OpenReceivablesInLocalCurrency);
            }
            catch (Exception ex)
            {

            }
            //if (shipmentPM.ShipmentReceivables[0].TotalAmountLocal == 25)
            //{
            //    if (shipmentPM.OpenReceivablesInLocalCurrency == 50)
            //    {

            //    }
            //}
        }
        public async Task<ShipmentPM> GetShipment(string shipmentId)
        {
            HttpResponseMessage response = await RestClientService.GetAsync("Shipment/GetSingle?id=" + shipmentId);
            ShipmentPM shipment = RestClientService.ParseResponse<ShipmentPM>(response);
            Assert.IsTrue(response.StatusCode.ToString() == "OK");
            return shipment;
        }
        public async Task PostShipment(string shipmentLevelCode, string directionId, string transportModeId)
        {
            shipmentPM = CreateShipmentPM(shipmentLevelCode, directionId, transportModeId);
            HttpResponseMessage response = await RestClientService.PostAsync(shipmentPM, "shipment");
            shipmentPM = RestClientService.ParseResponse<ShipmentPM>(response);
            ShipmentVariables.ShipmentId = shipmentPM.Id;
        }
        public async Task PutShipment(ShipmentPM shipmentPM)
        {
            HttpResponseMessage response = await RestClientService.PutAsync(shipmentPM, "shipment");
            shipmentPM = RestClientService.ParseResponse<ShipmentPM>(response);
            await GetShipment(ShipmentVariables.ShipmentId);
        }

        /*  public void EvaluateOpenReceivablesAmount(ShipmentReceivablePM[] receivables)
          {
              OpenAmountInLocal = 0;
              OpenAmpuntInProfit = 0;
              foreach(ShipmentReceivablePM item in receivables)
              {
                  OpenAmountInLocal += item.TotalAmountLocal != null ? (double) item.TotalAmountLocal : 0;
                  OpenAmpuntInProfit += item.AmountInProfitCurrency != null ? (double) item.AmountInProfitCurrency : 0;
              }
          }
          public void EvaluateOpenPayablesAmount(ShipmentPayablePM[] payables)
          {
              OpenAmountInLocal = 0;
              OpenAmpuntInProfit = 0;
              foreach (ShipmentPayablePM item in payables)
              {
                  OpenAmountInLocal += item.OpenAmountInLocalCurrency != null ? (double) item.OpenAmountInLocalCurrency : 0;
                  OpenAmpuntInProfit += item.OpenAmountInProfitCurrency != null ? (double) item.OpenAmountInProfitCurrency : 0;
              }
          }*/

        private ShipmentPM CreateShipmentPM(string shipmentLevelCode, string directionId, string transportModeId)
        {

            shipmentPM.Tenant = IntegrationTestLoginParameters.Tenant;
            shipmentPM.CreatedByUserId = IntegrationTestLoginParameters.LoginUserId;
            shipmentPM.BranchId = CorePreparationVariables.BranchId;
            shipmentPM.DepartmentId = CorePreparationVariables.DepartmentId;
            shipmentPM.ProfitCurrencyId = CorePreparationVariables.TenantPM.ProfitCurrencyId;
            shipmentPM.VolumeUnitCode = CorePreparationVariables.TenantPM.VolumeUnitCode;
            shipmentPM.DimensionsUnitCode = CorePreparationVariables.TenantPM.DimensionsUnitCode;
            shipmentPM.GrossWeightUnitCode = CorePreparationVariables.TenantPM.GrossWeightUnitCode;
            shipmentPM.ChargeableWeightUnitCode = CorePreparationVariables.TenantPM.ChargeableWeightUnitCode;
            shipmentPM.ShipmentLevelCode = shipmentLevelCode;
            shipmentPM.DirectionId = directionId;
            shipmentPM.TransportModeId = transportModeId;
            //shipmentPM.MainCarriageTransportModeId = "";
            shipmentPM.FreightPrepaidCollectId = "C";
            shipmentPM.OtherPrepaidCollectId = "C";

            shipmentPM.CreatedByUserId = CorePreparationVariables.UserId;
            shipmentPM.UpdatedByUserId = CorePreparationVariables.UserId;
            shipmentPM.CustomerId = ShipmentVariables.ShipperExport1;
            shipmentPM.ShipperId = ShipmentVariables.ShipperExport1;
            shipmentPM.IssuingCarrierAgentId = ShipmentVariables.AgentId;
            shipmentPM.AgentId = ShipmentVariables.AgentId;
            shipmentPM.FromPortId = ShipmentVariables.PortLHRId;
            shipmentPM.ToPortId = ShipmentVariables.PortJFKId;
            shipmentPM.MainCarriageFromPortId = ShipmentVariables.PortLHRId; ;
            shipmentPM.MainCarriageToPortId = ShipmentVariables.PortJFKId;
            shipmentPM.OriginMainCarriageFromPortId = ShipmentVariables.PortLHRId;
            shipmentPM.AWBCurrencyId = ShipmentVariables.CurrencyEURId;
            shipmentPM.ValueOfGoodsCurrencyId = ShipmentVariables.CurrencyEURId;
            shipmentPM.AccountManagerUserId = CorePreparationVariables.UserId;

            ShipmentVariables.ConcurrencyGUID = shipmentPM.NewConcurrencyGUID = Guid.NewGuid().ToString();
            //shipmentPM.PackagesQuantity = 5;
            //shipmentPM.GrossWeight = 100;
            //shipmentPM.ChargeableWeight = 100;
            //shipmentPM.NumberOfPackages = 5;


            return shipmentPM;
        }
        private ShipmentPM UpdateShipmentAirExport(ShipmentPM entityPM)
        {
            entityPM.ConcurrencyGUID = ShipmentVariables.ConcurrencyGUID;
            entityPM.NewConcurrencyGUID = ShipmentVariables.ConcurrencyGUID;
            entityPM.ShipmentPackages = IntegrationShipmentPackages.ShipmentPackages();
            //entityPM.ShipmentReceivables = IntegrationShipmentReceivable.ShipmentReceivables();
            entityPM.ShipmentPayables = IntegrationShipmentPayable.ShipmentPayables();
            entityPM.ShipmentPickUps = IntegrationShipmentPickUps.ShipmentPickUps();
            entityPM.ShipmentDeliveries = IntegrationShipmentDeliveries.shipmentDelivey();
            return entityPM;
        }
       
        private ShipmentPM UpdateReceivables(ShipmentPM shipmentPM, int quantity, int unitPrice)
        {
            shipmentPM.ShipmentReceivables = IntegrationShipmentReceivable.ShipmentReceivables(quantity, unitPrice);
            return shipmentPM;
        }


    }
}
