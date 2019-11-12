using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class ContactTest
    {
        [TestMethod]
        public void Test_Contact_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Server.Tools.Response serviceResponse = CallContactUpsert();
            Assert.AreEqual(serviceResponse.HasError, false, serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
         }

        public static Server.Tools.Response CallContactUpsert()
        {

            ContactServiceReference.ContactWcfServiceClient serviceClient = new ContactServiceReference.ContactWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)serviceClient.InnerChannel))
            {

                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                ContactServiceReference.ContactPM entityPM = new ContactServiceReference.ContactPM()
                {
                    EnglishName = "Hybrid Contact",
                    LocalName = "Hybrid Contact",
                    ExternalId = HybridCodes.ContactCode,
                    Tenant = TestEnvironmentGlobalParameters.Tenant,

                };
                Logitude.Server.Tools.Response serviceResponse = serviceClient.Upsert(entityPM, false);
                return serviceResponse;
            }
        }
    }
}
