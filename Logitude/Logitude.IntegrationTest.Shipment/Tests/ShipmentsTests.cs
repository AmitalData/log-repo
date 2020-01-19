using System;
using System.Net.Http;
using System.Threading.Tasks;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.IntegrationTest.Core;
using Logitude.IntegrationTest.Core.Login;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.IntegrationTest.Shipment
{
    [TestClass]
    public class ShipmentsTests
    {
        [TestMethod]
        public async Task GetSingleShipment()
        {
            ShipmentPM entityPM = CreateShipment();
            HttpResponseMessage response = await RestClientService.PostAsync(entityPM, "shipment");
            ShipmentPM shipmentPM = RestClientService.ParseResponse<ShipmentPM>(response);
            Assert.AreEqual(entityPM.Id, shipmentPM.Id);
        }

        private ShipmentPM CreateShipment()
        {
            ShipmentPM shipmentPM = new ShipmentPM();
            shipmentPM.Tenant = IntegrationTestLoginParameters.Tenant;
           // shipmentPM.ShipmentNumber = VariablesGenerater.GetUniqueIdByDate();
            shipmentPM.CreatedByUserId = IntegrationTestLoginParameters.LoginUserId;
            shipmentPM.BranchId = CorePreparationVariables.BranchId;
            shipmentPM.DepartmentId = CorePreparationVariables.DepartmentId;
            //shipmentPM.AWBCurrencyId = ShipmentVariables.AWBCurrencyId;
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
            shipmentPM.ConcurrencyGUID = "07e7ccc5 - 7fa3 - 4703 - 8cef - da92a5108b42";


            return shipmentPM;
        }
    }
}
