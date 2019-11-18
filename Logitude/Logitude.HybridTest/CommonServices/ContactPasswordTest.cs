using System;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class ContactPasswordTest
    {
        [TestMethod]
        public void Test_ContactPassword_ChangeContactPassword()
        {
            LoginService.GetLoginTokenByCredentials();
            Response contactServiceResponse = UserTest.CallUserUpsert();
            Assert.IsFalse(contactServiceResponse.HasError, "Upsert User Failed! " + contactServiceResponse.ErrorMessage);
            Assert.IsNotNull(contactServiceResponse.Result, "Upsert User Failed! " + contactServiceResponse.ErrorMessage);
            Response serviceResponse = CallChangeContactPassword();
            Assert.IsFalse(serviceResponse.HasError, "Change Contact Password Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Change Contact Password Failed! " + serviceResponse.ErrorMessage);
        }

        public static Response CallChangeContactPassword()
        {
            ContactPasswordServiceReference.ContactPasswordServiceClient serviceClient = new ContactPasswordServiceReference.ContactPasswordServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = serviceClient.ChangeContactPassword("Hybrid@fnarsoft.com", "!H0","!H1");
                serviceResponse = serviceClient.ChangeContactPassword("Hybrid@fnarsoft.com", "!H1", "!H0");
                return serviceResponse;
            }
        }
    }
}
