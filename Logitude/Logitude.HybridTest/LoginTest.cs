using System;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest
{
    [TestClass]
    public class LoginTest
    {
        [TestMethod]
        public void Test_Login_With_Valid_APICredintials()
        {
            string serverURL = System.Configuration.ConfigurationManager.AppSettings.Get("ServerURL");
            //P:9ae681d0-1d42-4293-9bea-aadef12e20dc
            //S:1dac32e1-84e4-496a-b4d2-687f16d04e3b
            var apiCred = new LoginServiceReference.APICredentialsParameters() { PrimaryKey = TestEnvironmentGlobalParameters.APICredential_PrimaryKey, SecondaryKey = TestEnvironmentGlobalParameters.APICredential_SecondaryKey, Tenant = TestEnvironmentGlobalParameters.Tenant };
            LoginServiceReference.LoginWcfServiceClient loginService = new LoginServiceReference.LoginWcfServiceClient();
            string serviceAddress = loginService.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            loginService.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            Logitude.Server.Tools.Response loginResponse = loginService.LoginByCredential("", apiCred);
            if (!loginResponse.HasError)
            {
                string Token = loginResponse.Result;
            }
            Assert.AreEqual(loginResponse.HasError, false, loginResponse.ErrorMessage);
            Assert.IsNotNull(loginResponse.Result, "The Token returned is null " + loginResponse.ErrorMessage);
            //return Token;
        }

        [TestMethod]
        public void Test_Login_With_Invalid_APICredintials()
        {
            string serverURL = System.Configuration.ConfigurationManager.AppSettings.Get("ServerURL");
             
            var apiCred = new LoginServiceReference.APICredentialsParameters() { PrimaryKey = "1111", SecondaryKey = "2222", Tenant = TestEnvironmentGlobalParameters.Tenant };
            LoginServiceReference.LoginWcfServiceClient loginService = new LoginServiceReference.LoginWcfServiceClient();
            loginService.Endpoint.Address = new System.ServiceModel.EndpointAddress(serverURL + "/WcfApi/LoginWcfService.svc");
            Logitude.Server.Tools.Response loginResponse = loginService.LoginByCredential("", apiCred);
            
            Assert.AreEqual(loginResponse.HasError, true, loginResponse.ErrorMessage);
            Assert.IsNull(loginResponse.Result, "The token was returned by the service " + loginResponse.Result);
            //return Token;
        }

        [TestMethod]
        public void Test_Login_With_ValidPassword()
        {
            LoginServiceReference.LoginWcfServiceClient serviceClient = new LoginServiceReference.LoginWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                Response serviceResponse = serviceClient.Login("Hybrid@fnarsoft.com", "!H0");
                Assert.IsFalse(serviceResponse.HasError, "Login Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNotNull(serviceResponse.Result, "Login Failed! " + serviceResponse.ErrorMessage);
            }
        }



        [TestMethod]
        public void Test_Login_With_InvalidPassword()
        {
            LoginServiceReference.LoginWcfServiceClient serviceClient = new LoginServiceReference.LoginWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                Response serviceResponse = serviceClient.Login("Hybrid@fnarsoft.com", "WrongPass");
                Assert.IsTrue(serviceResponse.HasError, "Login Succeeded With Invalid Email! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Login Succeeded With Invalid Email! " + serviceResponse.ErrorMessage);
            }
        }

        [TestMethod]
        public void Test_Login_GetUserTenants()
        {
            LoginServiceReference.LoginWcfServiceClient serviceClient = new LoginServiceReference.LoginWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                Response serviceResponse = new Response();
                LoginServiceReference.TenantInfo[] serviceResult = serviceClient.GetUserTenants("Hybrid@fnarsoft.com", ref serviceResponse);
                Assert.IsTrue(serviceResponse.HasError, "User Must Be Not Authorized! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "User Must Be Not Authorized! " + serviceResponse.ErrorMessage);
            }
        }
    }
}
