using System;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class EventTypeTest
    {
        [TestMethod]
        public void Test_EventType_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Response serviceResponse = CallEventTypeUpsert();
            Assert.AreEqual(serviceResponse.HasError, false, serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
         }

        public static Response CallEventTypeUpsert()
        {
            EventTypeServiceReference.EventTypeWcfServiceClient serviceClient = new EventTypeServiceReference.EventTypeWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                EventTypeServiceReference.EventTypePM entityPM = new EventTypeServiceReference.EventTypePM()
                {
                    Code = HybridCodes.EventTypeCode,
                    EnglishName = "Hybrid EventType",
                    LocalName = "Hybrid EventType",
                    ObjectTableName = "Shipment",
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                };
                Response serviceResponse = serviceClient.Upsert(entityPM, false);
                return serviceResponse;
            }
        }
    }
}
