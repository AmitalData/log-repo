using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest
{
    public class LoginService
    {
        public static void GetLoginTokenByCredentials()
        {
            if (string.IsNullOrEmpty(TestEnvironmentGlobalParameters.Token))
            {
                string serverURL = System.Configuration.ConfigurationManager.AppSettings.Get("ServerURL");
                //P:9ae681d0-1d42-4293-9bea-aadef12e20dc
                //S:1dac32e1-84e4-496a-b4d2-687f16d04e3b
                var apiCred = new LoginServiceReference.APICredentialsParameters() { PrimaryKey = TestEnvironmentGlobalParameters.APICredential_PrimaryKey, SecondaryKey = TestEnvironmentGlobalParameters.APICredential_SecondaryKey, Tenant = TestEnvironmentGlobalParameters.Tenant };
                LoginServiceReference.LoginWcfServiceClient loginService = new LoginServiceReference.LoginWcfServiceClient();
                loginService.Endpoint.Address = new System.ServiceModel.EndpointAddress(serverURL + "/WcfApi/LoginWcfService.svc");
                Logitude.Server.Tools.Response loginResponse = loginService.LoginByCredential("", apiCred);
                if (!loginResponse.HasError)
                {
                    TestEnvironmentGlobalParameters.Token = loginResponse.Result;
                }
                else
                    throw new Exception("Login Failed! " + loginResponse.ErrorMessage);
            }
             
        }
    }
}
