using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest
{
    [TestClass]
    public class LoginTest
    {
        [TestMethod]
        public void TestLogin_With_Valid_APICredintials()
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
        public void TestLogin_With_Invalid_APICredintials()
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
    }
}
