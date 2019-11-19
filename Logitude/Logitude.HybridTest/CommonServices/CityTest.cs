using System;
using Logitude.Server.Tools;
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
            Response countyServiceResponse = CountryTest.CallCountryUpsert();
            Assert.IsFalse(countyServiceResponse.HasError, "Country Upsert Failed! " + countyServiceResponse.ErrorMessage);
            Assert.IsNotNull(countyServiceResponse.Result, "Country Upsert Failed! " + countyServiceResponse.ErrorMessage);
            Response serviceResponse = CallCityUpsert();
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
         }

        [TestMethod]
        public void Test_City_GetCityListByCode()
        {
            LoginService.GetLoginTokenByCredentials();
            Test_City_UPSERT();
            CityServiceReference.CityWcfServiceClient serviceClient = new CityServiceReference.CityWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                string countryCode = HybridCodes.CountryCode;
                string cityCode = HybridCodes.CityCode;
                CityServiceReference.CountryCityList serviceResult = serviceClient.GetCityListByCode(cityCode, countryCode, TestEnvironmentGlobalParameters.Tenant, ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.ErrorMessage);
                if (serviceResult != null)
                {
                    string testCityCode = serviceResult.Code;
                    Assert.IsTrue(cityCode == testCityCode, "City Doesn't Exist In Country Cities!");
                }
                else
                {
                    Assert.Inconclusive("City Doesn't Exist In Country Cities!");
                }
            }
        }

        public static Response CallCityUpsert()
        {
            CityServiceReference.CityWcfServiceClient serviceClient = new CityServiceReference.CityWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
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
                Response serviceResponse = serviceClient.Upsert(entityPM, false);
                return serviceResponse;
            }
        }
    }
}
