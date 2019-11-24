using System;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class TruckerTest
    {
        [TestMethod]
        public void Test_Trucker_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Response serviceResponse = TruckerWcfCaller.CallTruckerUpsert();
            Assert.IsFalse(serviceResponse.HasError, serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }
    }
}
