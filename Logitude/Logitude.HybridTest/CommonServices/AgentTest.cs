using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class AgentTest
    {
        [TestMethod]
        public void Test_Agent_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Server.Tools.Response cityServiceResponse = GlobalZoneTest.CallGlobalZoneUpsert();
            Assert.IsFalse(cityServiceResponse.HasError, "City Upsert Failed! " + cityServiceResponse.ErrorMessage);
            Assert.IsNotNull(cityServiceResponse.Result, "City Upsert Failed! " + cityServiceResponse.ErrorMessage);
            Server.Tools.Response serviceResponse = CallAgentUpsert();
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
         }

        public static Server.Tools.Response CallAgentUpsert()
        {

            AgentServiceReference.AgentWcfServiceClient serviceClient = new AgentServiceReference.AgentWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)serviceClient.InnerChannel))
            {

                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                AgentServiceReference.AgentPM entityPM = new AgentServiceReference.AgentPM()
                {
                    Code = HybridCodes.AgentCode,
                    EnglishName = "Hybrid Agent",
                    LocalName = "Hybrid Agent",
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
