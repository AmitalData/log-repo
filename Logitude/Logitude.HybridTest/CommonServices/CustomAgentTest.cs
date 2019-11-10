using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class CustomAgentTest
    {
        [TestMethod]
        public void Test_CustomAgent_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Server.Tools.Response countryServiceResponse = CountryTest.CallCountryUpsert();
            Assert.IsFalse(countryServiceResponse.HasError, "Country Upsert Failed! " + countryServiceResponse.ErrorMessage);
            Assert.IsNotNull(countryServiceResponse.Result, "Country Upsert Failed! " + countryServiceResponse.ErrorMessage);
            Server.Tools.Response serviceResponse = CallCustomAgentUpsert();
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
         }

        public static Server.Tools.Response CallCustomAgentUpsert()
        {

            CustomAgentServiceReference.CustomAgentWcfServiceClient serviceClient = new CustomAgentServiceReference.CustomAgentWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)serviceClient.InnerChannel))
            {

                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                CustomAgentServiceReference.CustomAgentPM entityPM = new CustomAgentServiceReference.CustomAgentPM()
                {
                    Code = HybridCodes.CustomAgentCode,
                    EnglishName = "Hybrid CustomAgent",
                    LocalName = "Hybrid CustomAgent",
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
