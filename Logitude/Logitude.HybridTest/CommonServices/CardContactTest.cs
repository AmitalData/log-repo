using System;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class CardContactTest
    {
        [TestMethod]
        public void Test_CardContact_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Response countryServiceResponse = CountryTest.CallCountryUpsert();
            Assert.IsFalse(countryServiceResponse.HasError, "Country Upsert Failed! " + countryServiceResponse.ErrorMessage);
            Assert.IsNotNull(countryServiceResponse.Result, "Country Upsert Failed! " + countryServiceResponse.ErrorMessage);
            Response customerServiceResponse = CustomerTest.CallCustomerUpsert();
            Assert.IsFalse(customerServiceResponse.HasError, "Customer Upsert Failed! " + customerServiceResponse.ErrorMessage);
            Assert.IsNotNull(customerServiceResponse.Result, "customer Upsert Failed! " + customerServiceResponse.ErrorMessage);
            Response serviceResponse = CallCardContactUpsert();
            Assert.AreEqual(serviceResponse.HasError, false, serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
         }

        public static Response CallCardContactUpsert()
        {
            CardContactServiceReference.CardContactWcfServiceClient serviceClient = new CardContactServiceReference.CardContactWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                CardContactServiceReference.CardContactPM entityPM = new CardContactServiceReference.CardContactPM()
                {
                    IsAll = true,
                    ContactId = HybridCodes.ContactCode,
                    CardId = HybridCodes.CustomerCode,
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                };
                Response serviceResponse = serviceClient.Upsert(entityPM, false);
                return serviceResponse;
            }
        }

        [TestMethod]
        public void Test_CardContact_DELETE()
        {
            Assert.Inconclusive("Not Implemented !");
            Test_CardContact_UPSERT();
            CardContactServiceReference.CardContactWcfServiceClient serviceClient = new CardContactServiceReference.CardContactWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = serviceClient.Delete("?", "?", TestEnvironmentGlobalParameters.Tenant, false);
                Assert.IsFalse(serviceResponse.HasError, "Delete Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Delete Failed! " + serviceResponse.ErrorMessage);
            }
        }

        [TestMethod]
        public void Test_CardContact_GetCardContactPM()
        {
            Assert.Inconclusive("Not Implemented !");
            Test_CardContact_UPSERT();
            CardContactServiceReference.CardContactWcfServiceClient serviceClient = new CardContactServiceReference.CardContactWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                CardContactServiceReference.CardContactPM entityPM = serviceClient.GetCardContactPM("?", "?", TestEnvironmentGlobalParameters.Tenant, ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Get Card Contact PM! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get Card Contact PM! " + serviceResponse.ErrorMessage);
            }
        }
    }
}
