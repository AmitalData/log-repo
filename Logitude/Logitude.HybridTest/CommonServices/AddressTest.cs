using System;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class AddressTest
    {
        [TestMethod]
        public void Test_Address_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Server.Tools.Response countryServiceResponse = CountryTest.CallCountryUpsert();
            Assert.IsFalse(countryServiceResponse.HasError, "Country Upsert Failed! " + countryServiceResponse.ErrorMessage);
            Assert.IsNotNull(countryServiceResponse.Result, "Country Upsert Failed! " + countryServiceResponse.ErrorMessage);
            Server.Tools.Response customerServiceResponse = CustomerTest.CallCustomerUpsert();
            Assert.IsFalse(customerServiceResponse.HasError, "Customer Upsert Failed! " + customerServiceResponse.ErrorMessage);
            Assert.IsNotNull(customerServiceResponse.Result, "customer Upsert Failed! " + customerServiceResponse.ErrorMessage);
            Server.Tools.Response serviceResponse = CallAddressUpsert();
            Assert.AreEqual(serviceResponse.HasError, false, serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_Address_GETADDRESSBYEXTERNALID()
        {
            LoginService.GetLoginTokenByCredentials();
            Test_Address_UPSERT();
            AddressServiceReference.AddressWcfServiceClient serviceClient = new AddressServiceReference.AddressWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                AddressServiceReference.AddressPM serviceResult = serviceClient.GetAddressByExternalId(HybridCodes.AddressCode, TestEnvironmentGlobalParameters.Tenant, ref serviceResponse);
                Assert.AreEqual(serviceResponse.HasError, false, serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get Address By External Id Failed! " + serviceResponse.ErrorMessage);
                Assert.AreEqual(serviceResult.ExternalId, HybridCodes.AddressCode, serviceResponse.ErrorMessage);
            }
        }

        public static Response CallAddressUpsert()
        {
            AddressServiceReference.AddressWcfServiceClient serviceClient = new AddressServiceReference.AddressWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                AddressServiceReference.AddressPM entityPM = new AddressServiceReference.AddressPM()
                {
                    ExternalId = HybridCodes.AddressCode,
                    Name = "Hybrid Address",
                    City = "Hybrid City",
                    AddressTypeId = "M",
                    Description = "Main Address",
                    CountryId = HybridCodes.CountryCode,
                    CardId = HybridCodes.CustomerCode,
                    Tenant = TestEnvironmentGlobalParameters.Tenant,

                };
                Response serviceResponse = serviceClient.Upsert(entityPM, false);
                return serviceResponse;
            }
        }
    }
}