using System;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class VendorTest
    {
        [TestMethod]
        public void Test_Vendor_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Response serviceResponse = VendorWcfCaller.CallVendorUpsert();
            Assert.IsFalse(serviceResponse.HasError, serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }
    }
}
