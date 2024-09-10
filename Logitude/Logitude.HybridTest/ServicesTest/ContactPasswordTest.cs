using System;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class ContactPasswordTest
    {
        public TestContext TestContext { get; set; }
        [TestMethod]
        public void Test_ContactPassword_ChangeContactPassword()
        {
            try
            {
                RetryTest.InsertTestMethodToDictionary(TestContext.TestName);
                InvokedProperties serviceProperties = new InvokedProperties
                {
                    ServiceName = "ContactPassword",
                    ServiceOperation = "ChangeContactPassword",
                    ServiceResponseIndex = 0,
                    ServiceType = null,
                    ServiceFilterType = null,
                };
                Response serviceResponse = new Response();
                object[] serviceParameters = new object[] { "Hybrid@amital.co.il", "!H0", "!H1" };
                ChangeContactPassword(serviceProperties, serviceParameters);
                serviceParameters = new object[] { "Hybrid@amital.co.il", "!H1", "!H0" };
                ChangeContactPassword(serviceProperties, serviceParameters);
            }
            catch (Exception ex)
            {
                RetryTest.RetryFailTestRun(TestContext, this, ex.Message);
            }
        }

        private static void ChangeContactPassword(InvokedProperties serviceProperties, object[] serviceParameters)
        {
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);
            if(serviceOutcome.Response.ErrorMessage == "bad user email or password")
            {
                object[] newServiceParameters = new object[] { "Hybrid@fnarsoft.com", "!H1", "!H0" };
                serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, newServiceParameters);
            }
            Assert.IsFalse(serviceOutcome.Response.HasError, "Change Contact Password Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Change Contact Password Failed! " + serviceOutcome.Response.ErrorMessage);
        }
    }
}
