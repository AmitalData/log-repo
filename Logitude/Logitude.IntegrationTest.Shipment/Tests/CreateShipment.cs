using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.IntegrationTest.Core;
using Logitude.IntegrationTest.Core.Login;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace Logitude.IntegrationTest.Shipment.Tests
{
    class CreateShipment
    {
        ShipmentPM shipmentPM = new ShipmentPM();
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
            Assert.IsTrue(response.StatusCode.ToString() == "OK");
            ShipmentVariables.ShipmentId = shipmentPM.Id;
        }
        public async Task PutShipment(ShipmentPM shipmentPM)
        {
            HttpResponseMessage response = await RestClientService.PutAsync(shipmentPM, "shipment");
            shipmentPM = RestClientService.ParseResponse<ShipmentPM>(response);
            Assert.IsTrue(response.StatusCode.ToString() == "OK");
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
            shipmentPM.ShipmentPackages = IntegrationShipmentPackages.ShipmentPackages();

            ShipmentVariables.ConcurrencyGUID = shipmentPM.NewConcurrencyGUID = Guid.NewGuid().ToString();
            return shipmentPM;
        }
        private ShipmentPM UpdateShipmentAirExport(ShipmentPM entityPM)
        {
            entityPM.ConcurrencyGUID = ShipmentVariables.ConcurrencyGUID;
            entityPM.NewConcurrencyGUID = ShipmentVariables.ConcurrencyGUID;
            entityPM.ShipmentPackages = IntegrationShipmentPackages.ShipmentPackages();
            //entityPM.ShipmentReceivables = IntegrationShipmentReceivable.ShipmentReceivables();
            //entityPM.ShipmentPayables = IntegrationShipmentPayable.ShipmentPayables();
            entityPM.ShipmentPickUps = IntegrationShipmentPickUps.ShipmentPickUps();
            entityPM.ShipmentDeliveries = IntegrationShipmentDeliveries.shipmentDelivey();
            return entityPM;
        }
        public ShipmentPM UpdateReceivables(ShipmentPM shipmentPM, int quantity, int unitPrice)
        {
            shipmentPM.ShipmentReceivables = IntegrationShipmentReceivable.ShipmentReceivables(quantity, unitPrice);
            return shipmentPM;
        }
        public ShipmentPM UpdatePayables(ShipmentPM shipmentPM, int quantity, int unitPrice)
        {
            shipmentPM.ShipmentPayables = IntegrationShipmentPayable.ShipmentPayables(quantity, unitPrice);
            return shipmentPM;
        }

    }
}

