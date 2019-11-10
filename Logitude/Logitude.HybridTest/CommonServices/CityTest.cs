using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class CityTest
    {
        [TestMethod]
        public void Test_City_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Server.Tools.Response countyServiceResponse = GlobalZoneTest.CallGlobalZoneUpsert();
            Assert.IsFalse(countyServiceResponse.HasError, "Country Upsert Failed! " + countyServiceResponse.ErrorMessage);
            Assert.IsNotNull(countyServiceResponse.Result, "Country Upsert Failed! " + countyServiceResponse.ErrorMessage);
            Server.Tools.Response serviceResponse = CallCityUpsert();
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
         }

        public static Server.Tools.Response CallCityUpsert()
        {

            CityServiceReference.CityWcfServiceClient serviceClient = new CityServiceReference.CityWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)serviceClient.InnerChannel))
            {

                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                CityServiceReference.CountryCityPM entityPM = new CityServiceReference.CountryCityPM()
                {
                    Code = HybridCodes.CityCode,
                    EnglishName = "Hybrid City",
                    LocalName = "Hybrid City",
                    CountryId = HybridCodes.CountryCode,
                    AddedManually = true,
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                };

                Logitude.Server.Tools.Response serviceResponse = serviceClient.Upsert(entityPM, false);
                return serviceResponse;
            }
        }
    }
}
