using System;
using Logitude.HybridTest;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebFreight.Web.Helpers;

[assembly: Parallelize(Workers = 0, Scope = ExecutionScope.MethodLevel)] //0 means use as many workers as possible

namespace Logitude.LogboxIntegrationTest.PrepareSystem
{
    [TestClass]
    public class PrepareSystem
    {
        [AssemblyInitialize]
        public static void PrepareSystemVars(TestContext context)
        {
            GetAuthenticationCloudTenantToken();
            //GetAuthenticationLogboxTenantToken();
        }
        private static void GetAuthenticationCloudTenantToken()
        {
            var apiCred = new APICredentialsParameters() { PrimaryKey = EnvironmentParams.CloudTenant_APICredential_PrimaryKey, SecondaryKey = EnvironmentParams.CloudTenant_APICredential_SecondaryKey, Tenant = EnvironmentParams.CloudTenant };
            AdditionalIncludedData includedData = new AdditionalIncludedData
            {
                URL = EnvironmentParams.CloudServerURL
            };
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Login",
                ServiceOperation = "LoginByCredential",
                ServiceType = typeof(APICredentialsParameters),
                IncludedData = includedData
            };

            object[] serviceParameters = new object[] { "", apiCred };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);
            if (!serviceOutcome.Response.HasError)
                EnvironmentParams.CloudTenantToken = serviceOutcome.Response.Result;
            else
                Assert.Fail("Login Failed");
            Assert.IsNotNull(serviceOutcome.Response.Result, "The Token returned is null " + serviceOutcome.Response.ErrorMessage);
        }

        private static void GetAuthenticationLogboxTenantToken()
        {
            var apiCred = new APICredentialsParameters() { PrimaryKey = EnvironmentParams.LogboxTenant_APICredential_PrimaryKey, SecondaryKey = EnvironmentParams.LogboxTenant_APICredential_SecondaryKey, Tenant = EnvironmentParams.LogboxTenant };
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
                EnvironmentParams.LogboxTenantToken = serviceOutcome.Response.Result;
            else
                Assert.Fail("Login Failed");
            Assert.IsNotNull(serviceOutcome.Response.Result, "The Token returned is null " + serviceOutcome.Response.ErrorMessage);
        }
    }
}
