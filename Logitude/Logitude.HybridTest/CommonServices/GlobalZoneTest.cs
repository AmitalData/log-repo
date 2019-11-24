using System;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class GlobalZoneTest
    {
        [TestMethod]
        public void Test_GlobalZone_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Response serviceResponse = GlobalZoneWcfCaller.CallGlobalZoneUpsert();
            Assert.IsFalse(serviceResponse.HasError, serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }
    }
}
