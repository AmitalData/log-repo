using System;
using Logitude.HybridTest.LoginServiceReference;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class LoginTest
    {
        [TestMethod]
        public void Test_Login_With_Valid_APICredintials()
        {
            var apiCred = new APICredentialsParameters() { PrimaryKey = EnvironmentGlobalParams.MainTenant_APICredential_PrimaryKey, SecondaryKey = EnvironmentGlobalParams.MainTenant_APICredential_SecondaryKey, Tenant = EnvironmentGlobalParams.MainTenant };
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
                EnvironmentGlobalParams.MainTenantToken = serviceOutcome.Response.Result;
            else
                Assert.Fail("Login Failed");
            Assert.IsNotNull(serviceOutcome.Response.Result, "The Token returned is null " + serviceOutcome.Response.ErrorMessage);
        }

        [TestMethod]
        public void Test_Login_With_Invalid_APICredintials()
        {
            var apiCred = new APICredentialsParameters() { PrimaryKey = "1111", SecondaryKey = "2222", Tenant = EnvironmentGlobalParams.MainTenant };
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Login",
                ServiceOperation = "LoginByCredential",
                ServiceType = typeof(APICredentialsParameters),
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { "", apiCred };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);
            Assert.IsTrue(serviceOutcome.Response.HasError, serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "The Token returned is null " + serviceOutcome.Response.ErrorMessage);
        }

        [TestMethod]
        public void Test_Login_With_ValidPassword()
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Login",
                ServiceOperation = "Login",
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { "Hybrid@fnarsoft.com", "!H0" };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);
            Assert.IsFalse(serviceOutcome.Response.HasError, "Login Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNotNull(serviceOutcome.Response.Result, "Login Failed! " + serviceOutcome.Response.ErrorMessage);
        }

        [TestMethod]
        public void Test_Login_With_InvalidPassword()
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Login",
                ServiceOperation = "Login",
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { "Hybrid@fnarsoft.com", "WrongPass" };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);
            Assert.IsTrue(serviceOutcome.Response.HasError, "Login Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Login Failed! " + serviceOutcome.Response.ErrorMessage);
        }

        [TestMethod]
        public void Test_Login_GetUserTenants()
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Login",
                ServiceOperation = "GetUserTenants",
                ServiceResponseIndex = 1,
                ServiceType = typeof(TenantInfo),
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { "Hybrid@fnarsoft.com", serviceResponse };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

            TenantInfo[] userTenants = (TenantInfo[])serviceOutcome.Result;
            Assert.IsTrue(serviceOutcome.Response.HasError, "User Must Be Not Authorized! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "User Must Be Not Authorized! " + serviceOutcome.Response.ErrorMessage);
        }
    }
}
