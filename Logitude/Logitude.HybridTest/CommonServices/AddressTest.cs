using System;
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

        public static Server.Tools.Response CallAddressUpsert()
        {

            AddressServiceReference.AddressWcfServiceClient serviceClient = new AddressServiceReference.AddressWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)serviceClient.InnerChannel))
            {

                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                AddressServiceReference.AddressPM entityPM = new AddressServiceReference.AddressPM()
                {
                    Name = "Hybrid Address",
                    City = "Hybrid City",
                    AddressTypeId = "M",
                    Description = "Main Address",
                    CountryId = HybridCodes.CountryCode,
                    CardId = HybridCodes.CustomerCode,
                    Tenant = TestEnvironmentGlobalParameters.Tenant,

                };
                Logitude.Server.Tools.Response serviceResponse = serviceClient.Upsert(entityPM, false);
                return serviceResponse;
            }
        }
    }
}
