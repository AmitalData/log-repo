using System;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class SpecialServicesTypeTest
    {
        [TestMethod]
        public void Test_SpecialServicesType_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Response serviceResponse = SpecialServicesTypeWcfCaller.CallSpecialServicesTypeUpsert();
            Assert.IsFalse(serviceResponse.HasError, serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }
    }
}
