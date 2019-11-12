using System;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class GlobalZoneTest
    {
        [TestMethod]
        public void Test_GlobalZone_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Response serviceResponse = CallGlobalZoneUpsert();
            Assert.AreEqual(serviceResponse.HasError, false, serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
         }

        public static Response CallGlobalZoneUpsert()
        {
            GlobalZoneServiceReference.GlobalZoneWcfServiceClient serviceClient = new GlobalZoneServiceReference.GlobalZoneWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                GlobalZoneServiceReference.GlobalZonePM entityPM = new GlobalZoneServiceReference.GlobalZonePM()
                {
                    Code = HybridCodes.GlobalZoneCode,
                    EnglishName = "Hybrid GlobalZone",
                    LocalName = "Hybrid GlobalZone",
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                };
                Response serviceResponse = serviceClient.Upsert(entityPM, false);
                return serviceResponse;
            }
        }
    }
}
