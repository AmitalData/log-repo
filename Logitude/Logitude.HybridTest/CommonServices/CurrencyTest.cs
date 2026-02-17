using System;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class CurrencyTest
    {
        [TestMethod]
        public void Test_Currency_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Response countryServiceResponse = CountryTest.CallCountryUpsert();
            Assert.IsFalse(countryServiceResponse.HasError, "Country Upsert Failed! " + countryServiceResponse.ErrorMessage);
            Assert.IsNotNull(countryServiceResponse.Result, "Country Upsert Failed! " + countryServiceResponse.ErrorMessage);
            Response serviceResponse = CallCurrencyUpsert();
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
         }

        [TestMethod]
        public void Test_Currency_GETLIST()
        {
            Test_Currency_UPSERT();
            CurrencyServiceReference.CurrencyWcfServiceClient serviceClient = new CurrencyServiceReference.CurrencyWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                ApiSearchFilters filters = new ApiSearchFilters
                {
                    Take = 10,
                    SearchFields = HybridCodes.CurrencyCode
                };
                CurrencyServiceReference.CurrencyList[] serviceResult = serviceClient.GetList(filters, TestEnvironmentGlobalParameters.Tenant, ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.ErrorMessage);
                if (serviceResult.Length != 0)
                {
                    string currencyCode = serviceResult[0].Code;
                    Assert.IsTrue(currencyCode == "HCR", "Hybrid Currency Doesn't Exist!");
                }
                else
                {
                    Assert.Inconclusive("There Isn't Currency With This Code!");
                }
            }
        }

        public static Response CallCurrencyUpsert()
        {
            CurrencyServiceReference.CurrencyWcfServiceClient serviceClient = new CurrencyServiceReference.CurrencyWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                CurrencyServiceReference.CurrencyPM entityPM = new CurrencyServiceReference.CurrencyPM()
                {
                    Code = HybridCodes.CurrencyCode,
                    EnglishName = "Hybrid Currency",
                    LocalName = "Hybrid Currency",
                    AddedManually = true,
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                };
                Response serviceResponse = serviceClient.Upsert(entityPM, false);
                return serviceResponse;
            }
        }
    }
}
