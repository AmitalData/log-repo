using System;
using Logitude.HybridTest.AutoSignUpServiceReference;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class AutoSignUpTest
    {
        [TestMethod]
        public void Test_AutoSignUp_INSERT()
        {
            Assert.Inconclusive("FILL ?!");

            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "AutoSignUp",
                ServiceOperation = "Insert",
                ServiceResponseIndex = 0,
                ServiceType = typeof(AutoSignUpData),
                ServiceFilterType = null,
            };
            AutoSignUpData entityPM = new AutoSignUpData()
            {
                //Fill
            };
            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { entityPM, false };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);
            Assert.IsFalse(serviceOutcome.Response.HasError, "Insert Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Insert Failed! " + serviceOutcome.Response.Result);
        }
    }
}
