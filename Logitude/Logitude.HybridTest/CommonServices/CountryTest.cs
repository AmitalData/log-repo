using System;
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
            Server.Tools.Response globalZoneServiceResponse = GlobalZoneTest.CallGlobalZoneUpsert();
            Assert.IsFalse(globalZoneServiceResponse.HasError, "Global Zone Upsert Failed! " + globalZoneServiceResponse.ErrorMessage);
            Assert.IsNotNull(globalZoneServiceResponse.Result, "Global Zone Upsert Failed! " + globalZoneServiceResponse.ErrorMessage);
            Server.Tools.Response serviceResponse = CallCountryUpsert();
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
         }

        public static Server.Tools.Response CallCountryUpsert()
        {

            CountryServiceReference.CountryWcfServiceClient serviceClient = new CountryServiceReference.CountryWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)serviceClient.InnerChannel))
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

                Logitude.Server.Tools.Response serviceResponse = serviceClient.Upsert(entityPM, false);
                return serviceResponse;
            }
        }
    }
}
