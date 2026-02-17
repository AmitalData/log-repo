using System;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class PortTest
    {
        [TestMethod]
        public void Test_PORT_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Response countryServiceResponse = CountryTest.CallCountryUpsert();
            Assert.IsFalse(countryServiceResponse.HasError, "Country Upsert Failed! " + countryServiceResponse.ErrorMessage);
            Assert.IsNotNull(countryServiceResponse.Result, "Country Upsert Failed! " + countryServiceResponse.ErrorMessage);
            Response serviceResponse = CallFromPortUpsert();
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Response serviceResponse2 = CallToPortUpsert();
            Assert.IsFalse(serviceResponse2.HasError, "Upsert Failed! " + serviceResponse2.ErrorMessage);
            Assert.IsNotNull(serviceResponse2.Result, "Upsert Failed! " + serviceResponse2.ErrorMessage);
        }

        [TestMethod]
        public void Test_PORT_GetList()
        {
            LoginService.GetLoginTokenByCredentials();
            PortServiceReference.PortWcfServiceClient serviceClient = new PortServiceReference.PortWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                ApiSearchFilters filters = new ApiSearchFilters
                {
                    Take = 10,
                    SearchFields = HybridCodes.FromPortCode
                };
                PortServiceReference.PortList[] serviceResult = serviceClient.GetList(filters, TestEnvironmentGlobalParameters.Tenant, ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.ErrorMessage);
                if (serviceResult.Length != 0)
                {
                    string fromPortCode = serviceResult[0].Code;
                    Assert.IsTrue(fromPortCode == HybridCodes.FromPortCode, "From Port Doesn't Exist!");
                }
                else
                {
                    Assert.Inconclusive("There Isn't Port With This Code!");
                }
            }
        }

        [TestMethod]
        public void Test_PORT_GetPortId()
        {
            LoginService.GetLoginTokenByCredentials();
            Test_PORT_UPSERT();
            PortServiceReference.PortWcfServiceClient serviceClient = new PortServiceReference.PortWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                PortServiceReference.PortApiFilters filters = new PortServiceReference.PortApiFilters
                {
                    PortCode = "TLV",
                    CountryCode = "IL"
                };
                string serviceResult = serviceClient.GetPortId(filters, TestEnvironmentGlobalParameters.Tenant, ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNotNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.ErrorMessage);
                Assert.IsTrue(serviceResult != null, "From Port Doesn't Exist!");
            }
        }

        public static Response CallFromPortUpsert()
        {
            PortServiceReference.PortWcfServiceClient serviceClient = new PortServiceReference.PortWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                PortServiceReference.PortPM entityPM = new PortServiceReference.PortPM()
                {
                    Code = HybridCodes.FromPortCode,
                    EnglishName = "Hybrid From Port",
                    LocalName = "Hybrid From Port",
                    CountryCode = HybridCodes.CountryCode,
                    CountryId = HybridCodes.CountryCode,
                    AddedManually = true,
                    IsAir = true,
                    IsOcean = true,
                    IsInland = true,
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                };
                Response serviceResponse = serviceClient.Upsert(entityPM, false);
                return serviceResponse;
            }
        }

        public static Response CallToPortUpsert()
        {
            PortServiceReference.PortWcfServiceClient serviceClient = new PortServiceReference.PortWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                PortServiceReference.PortPM entityPM = new PortServiceReference.PortPM()
                {
                    Code = HybridCodes.ToPortCode,
                    EnglishName = "Hybrid To Port",
                    LocalName = "Hybrid To Port",
                    CountryCode = HybridCodes.CountryCode,
                    CountryId = HybridCodes.CountryCode,
                    AddedManually = true,
                    IsAir = true,
                    IsOcean = true,
                    IsInland = true,
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                };
                Response serviceResponse = serviceClient.Upsert(entityPM, false);
                return serviceResponse;
            }
        }
    }
}
