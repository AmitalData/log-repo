using System;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class ShippingAgentTest
    {
        [TestMethod]
        public void Test_ShippingAgent_UPSERT()
        {
            ShippingAgentPM shippingAgentPM = new ShippingAgentPM()
            {
                Code = HybridData.ShippingAgentCodeHSAG,
                EnglishName = "Hybrid ShippingAgent",
                LocalName = "Hybrid ShippingAgent",
                CityName = "Hybrid City",
                CountryCode = HybridData.CountryCodeUS,
                PartnerTypeId = "SG",
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(shippingAgentPM);
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }
    }
}
