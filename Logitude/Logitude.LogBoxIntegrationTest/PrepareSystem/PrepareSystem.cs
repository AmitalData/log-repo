using System;
using Logitude.HybridTest;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebFreight.Web.Helpers;

[assembly: Parallelize(Workers = 0, Scope = ExecutionScope.MethodLevel)] //0 means use as many workers as possible

namespace Logitude.LogBoxIntegrationTest.PrepareSystem
{
    [TestClass]
    public class PrepareSystem
    {
        [AssemblyInitialize]
        public static void PrepareSystemVars(TestContext context)
        {
            GetAuthenticationLogBoxTenantToken();
            Assert.IsFalse(false);
        }

        private static void GetAuthenticationLogBoxTenantToken()
        {
            var apiCred = new APICredentialsParameters() { PrimaryKey = EnvironmentParams.LogBoxTenant_APICredential_PrimaryKey, SecondaryKey = EnvironmentParams.LogBoxTenant_APICredential_SecondaryKey, Tenant = EnvironmentParams.LogBoxTenant };
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Login",
                ServiceOperation = "LoginByCredential",
                ServiceType = typeof(APICredentialsParameters),
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { "", apiCred };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);
            if (!serviceOutcome.Response.HasError)
                EnvironmentParams.LogBoxTenantToken = serviceOutcome.Response.Result;
            else
                Assert.Fail("Login Failed");
            Assert.IsNotNull(serviceOutcome.Response.Result, "The Token returned is null " + serviceOutcome.Response.ErrorMessage);
        }
    }
}
