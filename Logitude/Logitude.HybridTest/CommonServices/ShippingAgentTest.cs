using System;
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
            LoginService.GetLoginTokenByCredentials();
            Response serviceResponse = ShippingAgentWcfCaller.CallShippingAgentUpsert();
            Assert.IsFalse(serviceResponse.HasError, serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }
    }
}
