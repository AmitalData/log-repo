using System;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class SpecialServicesTypeTest
    {
        [TestMethod]
        public void Test_SpecialServicesType_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Response serviceResponse = CallSpecialServicesTypeUpsert();
            Assert.AreEqual(serviceResponse.HasError, false, serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
         }

        public static Response CallSpecialServicesTypeUpsert()
        {
            SpecialServicesTypeServiceReference.SpecialServicesTypeWcfServiceClient serviceClient = new SpecialServicesTypeServiceReference.SpecialServicesTypeWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                SpecialServicesTypeServiceReference.SpecialServicesTypePM entityPM = new SpecialServicesTypeServiceReference.SpecialServicesTypePM()
                {
                    Code = HybridCodes.SpecialServicesTypeCode,
                    EnglishName = "Hybrid SpecialServicesType",
                    LocalName = "Hybrid SpecialServicesType",
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                };
                Response serviceResponse = serviceClient.Upsert(entityPM, false);
                return serviceResponse;
            }
        }
    }
}
