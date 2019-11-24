using System;
using Logitude.HybridTest.AutoSignUpServiceReference;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class AutoSignUpTest
    {
        [TestMethod]
        public void Test_AutoSignUp_INSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Assert.Inconclusive("FILL ?!");

            LoginService.GetLoginTokenByCredentials();
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
            serviceResponse = (Response)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Insert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Insert Failed! " + serviceResponse.Result);
        }
    }
}
