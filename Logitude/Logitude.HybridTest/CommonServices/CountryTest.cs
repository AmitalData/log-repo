using System;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class CountryTest
    {
        [TestMethod]
        public void Test_Country_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Response globalZoneServiceResponse = GlobalZoneTest.CallGlobalZoneUpsert();
            Assert.IsFalse(globalZoneServiceResponse.HasError, "Global Zone Upsert Failed! " + globalZoneServiceResponse.ErrorMessage);
            Assert.IsNotNull(globalZoneServiceResponse.Result, "Global Zone Upsert Failed! " + globalZoneServiceResponse.ErrorMessage);
            Response serviceResponse = CallCountryUpsert();
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
         }

        [TestMethod]
        public void Test_Country_GETLIST()
        {
            LoginService.GetLoginTokenByCredentials();
            CountryServiceReference.CountryWcfServiceClient serviceClient = new CountryServiceReference.CountryWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                ApiSearchFilters filters = new ApiSearchFilters();
                filters.Take = 10;
                filters.SearchFields = HybridCodes.CountryCode;
                CountryServiceReference.CountryList[] serviceResult = serviceClient.GetList(filters, TestEnvironmentGlobalParameters.Tenant, ref serviceResponse);
                string countryCode = serviceResult[0].Code;
                Assert.IsTrue(countryCode == "HC", "Hybrid Country Doesn't Exist!");
                Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.ErrorMessage);
            }
        }

        public static Response CallCountryUpsert()
        {
            CountryServiceReference.CountryWcfServiceClient serviceClient = new CountryServiceReference.CountryWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                CountryServiceReference.CountryPM entityPM = new CountryServiceReference.CountryPM()
                {
                    Code = HybridCodes.CountryCode,
                    EnglishName = "Hybrid Country",
                    LocalName = "Hybrid Country",
                    GlobalZoneId = HybridCodes.GlobalZoneCode,
                    AddedManually = true,
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                };
                Response serviceResponse = serviceClient.Upsert(entityPM, false);
                return serviceResponse;
            }
        }
    }
}
