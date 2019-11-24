using System;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class CustomAgentTest
    {
        [TestMethod]
        public void Test_CustomAgent_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Response serviceResponse = CustomAgentWcfCaller.CallCustomAgentUpsert();
            Assert.IsFalse(serviceResponse.HasError, serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }
    }
}
