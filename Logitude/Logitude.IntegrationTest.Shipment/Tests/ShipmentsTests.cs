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
            HttpResponseMessage response = await RestClientService.PostAsync(entityPM, "ARInvoices");
            ShipmentPM shipmentPM = RestClientService.ParseResponse<ShipmentPM>(response);
            Assert.AreEqual(entityPM.Id, shipmentPM.Id);
        }

        private ShipmentPM CreateShipment()
        {
            ShipmentPM shipmentPM = new ShipmentPM();
            shipmentPM.Tenant = IntegrationTestLoginParameters.Tenant;
            shipmentPM.ShipmentNumber = VariablesGenerater.GetUniqueIdByDate();
            shipmentPM.CreatedByUserId = IntegrationTestLoginParameters.LoginUserId;
            shipmentPM.BranchId = CorePreparationVariables.BranchId;
            shipmentPM.DepartmentId = CorePreparationVariables.DepartmentId;
            //shipmentPM.AWBCurrencyId = ShipmentVariables.AWBCurrencyId;
            shipmentPM.ProfitCurrencyId = CorePreparationVariables.TenantPM.ProfitCurrencyId;
            shipmentPM.VolumeUnitCode = CorePreparationVariables.TenantPM.VolumeUnitCode;
            shipmentPM.DimensionsUnitCode = CorePreparationVariables.TenantPM.DimensionsUnitCode;
            shipmentPM.GrossWeightUnitCode = CorePreparationVariables.TenantPM.GrossWeightUnitCode;
            
            return shipmentPM;
        }
    }
}
