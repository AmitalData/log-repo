using System;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class ShippingLineTest
    {
        [TestMethod]
        public void Test_ShippingLine_UPSERT()
        {
            ShippingLinePM shippingLinePM = new ShippingLinePM()
            {
                Code = HybridData.ShippingLineCode,
                SCACCode = HybridData.ShippingLineCode,
                EnglishName = "Hybrid ShippingLine",
                LocalName = "Hybrid ShippingLine",
                CityName = "Hybrid City",
                CountryCode = HybridData.CountryCode,
                CarrierTypeId = "SL",
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(shippingLinePM);
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }
    }
}
