using System;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class ContactPasswordTest
    {
        [TestMethod]
        public void Test_ContactPassword_ChangeContactPassword()
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "ContactPassword",
                ServiceOperation = "ChangeContactPassword",
                ServiceResponseIndex = 0,
                ServiceType = null,
                ServiceFilterType = null,
            };
            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { "Hybrid@fnarsoft.com", "!H0", "!H1" };
            WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Change Contact Password Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Change Contact Password Failed! " + serviceResponse.ErrorMessage);
            serviceParameters = new object[] { "Hybrid@fnarsoft.com", "!H1", "!H0" };
            WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Change Contact Password Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Change Contact Password Failed! " + serviceResponse.ErrorMessage);
        }
    }
}
