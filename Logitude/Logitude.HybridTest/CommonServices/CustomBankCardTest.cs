using System;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class CustomBankCardTest
    {
        [TestMethod]
        public void Test_CustomBankCard_UPSERT()
        {
            Assert.Inconclusive("Not Implemented !");
            //Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            //Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_CustomBankCard_DELETE()
        {
            Assert.Inconclusive("Not Implemented !");
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "CustomBankCard",
                ServiceOperation = "Delete",
                ServiceResponseIndex = 0,
                ServiceType = null,
                ServiceFilterType = null,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { "?", "?", TestEnvironmentGlobalParameters.Tenant };
            WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Delete Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Delete Failed! " + serviceResponse.ErrorMessage);
        }
    }
}