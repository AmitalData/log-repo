using System;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class StateTest
    {
        [TestMethod]
        public void Test_State_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Response countyServiceResponse = CountryTest.CallCountryUpsert();
            Assert.IsFalse(countyServiceResponse.HasError, "Country Upsert Failed! " + countyServiceResponse.ErrorMessage);
            Assert.IsNotNull(countyServiceResponse.Result, "Country Upsert Failed! " + countyServiceResponse.ErrorMessage);
            Response serviceResponse = CallStateUpsert();
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
         }

        public static Response CallStateUpsert()
        {
            StateServiceReference.StateWcfServiceClient serviceClient = new StateServiceReference.StateWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                StateServiceReference.StatePM entityPM = new StateServiceReference.StatePM()
                {
                    Code = HybridCodes.StateCode,
                    EnglishName = "Hybrid State",
                    LocalName = "Hybrid State",
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
