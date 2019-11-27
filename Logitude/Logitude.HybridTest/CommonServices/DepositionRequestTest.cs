using System;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class DepositionRequestTest
    {
        [TestMethod]
        public void Test_DepositionRequest_UPSERT()
        {
            Assert.Inconclusive("Not implemented!");
            LoginService.GetLoginTokenByCredentials();
            Response serviceResponse = DepositionRequestWcfCaller.CallDepositionRequestUpsert();
            Assert.IsFalse(serviceResponse.HasError, serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }
    }
}
