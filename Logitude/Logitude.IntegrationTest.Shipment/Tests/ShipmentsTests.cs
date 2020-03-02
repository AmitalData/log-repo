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
        // double OpenAmountInLocal;
        //double OpenAmpuntInProfit;
        ShipmentPM entityPM;
        ShipmentPM shipmentPM = new ShipmentPM();

        [TestMethod]
        public async Task PostShipment()
        {
            entityPM = CreateShipmentAirExport();
            HttpResponseMessage response = await RestClientService.PostAsync(entityPM, "shipment");
            ShipmentPM shipmentPM = RestClientService.ParseResponse<ShipmentPM>(response);
            ShipmentVariables.ShipmentId = shipmentPM.Id;
            await GetShipment(ShipmentVariables.ShipmentId);
            await PutShipment(ShipmentVariables.ShipmentId, shipmentPM);
            
        }

        [TestMethod]
        public async Task<ShipmentPM> GetShipment(string id)
        {
            HttpResponseMessage response = await RestClientService.GetAsync("Shipment/GetSingle?id=" + ShipmentVariables.ShipmentId);
            ShipmentPM shipment = RestClientService.ParseResponse<ShipmentPM>(response);
            Assert.IsTrue(response.StatusCode.ToString()=="OK");
            return shipment;
        }

        public async Task PutShipment(string id, ShipmentPM shipmentPM)
        {
            entityPM = UpdateShipmentAirExport(id, shipmentPM);
            HttpResponseMessage response = await RestClientService.PutAsync(entityPM, "shipment");
            ShipmentPM shipment = RestClientService.ParseResponse<ShipmentPM>(response);
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


        private ShipmentPM CreateShipmentAirExport()
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
            shipmentPM.DirectionId = "E";
            shipmentPM.TransportModeId = "A";
            //shipmentPM.MainCarriageTransportModeId = "";
            shipmentPM.FreightPrepaidCollectId = "C";
            shipmentPM.OtherPrepaidCollectId = "C";
            shipmentPM.ShipmentLevelCode = "D";
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
            shipmentPM.OriginMainCarriageFromPortId= ShipmentVariables.PortLHRId;
            shipmentPM.AWBCurrencyId = ShipmentVariables.CurrencyEURId;
            shipmentPM.ValueOfGoodsCurrencyId = ShipmentVariables.CurrencyEURId;
            shipmentPM.AccountManagerUserId = CorePreparationVariables.UserId;
            
            ShipmentVariables.ConcurrencyGUID = shipmentPM.NewConcurrencyGUID = Guid.NewGuid().ToString();
            shipmentPM.PackagesQuantity = 5;
            shipmentPM.GrossWeight = 100;
            shipmentPM.ChargeableWeight = 100;
            shipmentPM.NumberOfPackages = 5;


            return shipmentPM;
        }

        private ShipmentPM UpdateShipmentAirExport(string id, ShipmentPM shipmentPM)
        {
           
          // CreateShipmentAirExport();
           shipmentPM.Id = id;
           shipmentPM.ConcurrencyGUID= ShipmentVariables.ConcurrencyGUID;
           shipmentPM.NewConcurrencyGUID = ShipmentVariables.ConcurrencyGUID;
           shipmentPM.ShipmentPackages = IntegrationShipmentPackages.ShipmentPackages();
           shipmentPM.ShipmentReceivables = IntegrationShipmentReceivable.ShipmentReceivables();
           shipmentPM.ShipmentPayables = IntegrationShipmentPayable.ShipmentPayables();
           shipmentPM.ShipmentPickUps = IntegrationShipmentPickUps.ShipmentPickUps();
           shipmentPM.ShipmentDeliveries = IntegrationShipmentDeliveries.shipmentDelivey();
           return shipmentPM;
        }



    }
}
