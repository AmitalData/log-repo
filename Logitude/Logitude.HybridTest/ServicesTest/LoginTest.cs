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
            var apiCred = new APICredentialsParameters() { PrimaryKey = TestEnvironmentGlobalParameters.APICredential_PrimaryKey1, SecondaryKey = TestEnvironmentGlobalParameters.APICredential_SecondaryKey1, Tenant = TestEnvironmentGlobalParameters.Tenant1 };
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Login",
                ServiceOperation = "LoginByCredential",
                ServiceType = typeof(APICredentialsParameters),
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { "", apiCred };
            Response loginResponse = (Response)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            if (!loginResponse.HasError)
                TestEnvironmentGlobalParameters.Token1 = loginResponse.Result;
            else
                Assert.Fail("Login Failed");
            Assert.IsNotNull(loginResponse.Result, "The Token returned is null " + loginResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_Login_With_Invalid_APICredintials()
        {
            var apiCred = new APICredentialsParameters() { PrimaryKey = "1111", SecondaryKey = "2222", Tenant = TestEnvironmentGlobalParameters.Tenant1 };
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Login",
                ServiceOperation = "LoginByCredential",
                ServiceType = typeof(APICredentialsParameters),
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { "", apiCred };
            Response loginResponse = (Response)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsTrue(loginResponse.HasError, loginResponse.ErrorMessage);
            Assert.IsNull(loginResponse.Result, "The Token returned is null " + loginResponse.ErrorMessage);
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
            Response loginResponse = (Response)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Login Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Login Failed! " + serviceResponse.ErrorMessage);
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
            Response loginResponse = (Response)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsTrue(serviceResponse.HasError, "Login Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Login Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_Login_GetUserTenants()
        {
            //InvokedProperties serviceProperties = new InvokedProperties
            //{
            //    ServiceName = "Login",
            //    ServiceOperation = "GetUserTenants",
            //    ServiceResponseIndex = 1,
            //    ServiceType = typeof(TenantInfo),
            //};

            //Response serviceResponse = new Response();
            //object[] serviceParameters = new object[] { "Hybrid@fnarsoft.com", serviceResponse };
            //TenantInfo[] userTenants = (TenantInfo[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            //Assert.IsTrue(serviceResponse.HasError, "User Must Be Not Authorized! " + serviceResponse.ErrorMessage);
            //Assert.IsNull(serviceResponse.Result, "User Must Be Not Authorized! " + serviceResponse.ErrorMessage);
        }
    }
}
