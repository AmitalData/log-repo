using System;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class VesselTest
    {
        [TestMethod]
        public void Test_Vessel_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Response serviceResponse = CallVesselUpsert();
            Assert.AreEqual(serviceResponse.HasError, false, serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
         }

        public static Response CallVesselUpsert()
        {
            VesselServiceReference.VesselWcfServcieClient serviceClient = new VesselServiceReference.VesselWcfServcieClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                VesselServiceReference.VesselPM entityPM = new VesselServiceReference.VesselPM()
                {
                    Code = HybridCodes.VesselCode,
                    EnglishName = "Hybrid Vessel",
                    LocalName = "Hybrid Vessel",
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                };
                Response serviceResponse = serviceClient.Upsert(entityPM, false);
                return serviceResponse;
            }
        }
    }
}
