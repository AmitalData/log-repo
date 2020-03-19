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
            ChangeContactPassword(serviceProperties, serviceParameters);
            serviceParameters = new object[] { "Hybrid@fnarsoft.com", "!H1", "!H0" };
            ChangeContactPassword(serviceProperties, serviceParameters);
        }

        private static void ChangeContactPassword(InvokedProperties serviceProperties, object[] serviceParameters)
        {
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);
            Assert.IsFalse(serviceOutcome.Response.HasError, "Change Contact Password Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Change Contact Password Failed! " + serviceOutcome.Response.ErrorMessage);
        }
    }
}
