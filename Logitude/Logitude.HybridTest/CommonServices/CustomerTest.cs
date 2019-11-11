using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class CustomerTest
    {
        [TestMethod]
        public void Test_Customer_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Server.Tools.Response countryServiceResponse = CountryTest.CallCountryUpsert();
            Assert.IsFalse(countryServiceResponse.HasError, "Country Upsert Failed! " + countryServiceResponse.ErrorMessage);
            Assert.IsNotNull(countryServiceResponse.Result, "Country Upsert Failed! " + countryServiceResponse.ErrorMessage);
            Server.Tools.Response serviceResponse = CallCustomerUpsert();
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
         }

        public static Server.Tools.Response CallCustomerUpsert()
        {

            CustomerServiceReference.CustomerWcfServiceClient serviceClient = new CustomerServiceReference.CustomerWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)serviceClient.InnerChannel))
            {

                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                CustomerServiceReference.CustomerPM entityPM = new CustomerServiceReference.CustomerPM()
                {
                    Code = HybridCodes.CustomerCode,
                    EnglishName = "Hybrid Customer",
                    LocalName = "Hybrid Customer",
                    CityName = "Hybrid City",
                    CountryCode = HybridCodes.CountryCode,
                    PartnerTypeId = "AG",
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                };

                Logitude.Server.Tools.Response serviceResponse = serviceClient.Upsert(entityPM, false);
                return serviceResponse;
            }
        }
    }
}
