using System;
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
            LoginService.GetLoginTokenByCredentials();
            Response serviceResponse = ShippingLineWcfCaller.CallShippingLineUpsert();
            Assert.IsFalse(serviceResponse.HasError, serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }
    }
}
