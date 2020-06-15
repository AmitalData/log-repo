using System;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.HybridTest;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.LogboxIntegrationTest.Senarios
{
    [TestClass]
    public class OngoingShipmentsUNItoLogboxViaCloud
    {
        [TestMethod]
        public void Test_OngoingShipments_UNItoLogboxViaCloud()
        {
            CreateShipmentInCloud();
        }

        private void CreateShipmentInCloud()
        {
            ShipmentPM shipmentPM = ShipmentWcfFactory.GetShipmentPM();
            AdditionalIncludedData includedData = new AdditionalIncludedData
            {
                URL = EnvironmentParams.CloudServerURL,
                Token = EnvironmentParams.CloudTenantToken
            };
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(shipmentPM, null, includedData);
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }
    }
}
