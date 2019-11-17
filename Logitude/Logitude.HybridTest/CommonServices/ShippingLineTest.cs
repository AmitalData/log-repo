using System;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class ShippingLineTest
    {
        [TestMethod]
        public void Test_ShippingLine_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Response serviceResponse = CallShippingLineUpsert();
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
         }

        public static Response CallShippingLineUpsert()
        {
            ShippingLineServiceReference.ShippingLineWcfServiceClient serviceClient = new ShippingLineServiceReference.ShippingLineWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                ShippingLineServiceReference.ShippingLinePM entityPM = new ShippingLineServiceReference.ShippingLinePM()
                {
                    Code = HybridCodes.ShippingLineCode,
                    SCACCode = HybridCodes.ShippingLineCode,
                    EnglishName = "Hybrid ShippingLine",
                    LocalName = "Hybrid ShippingLine",
                    CityName = "Hybrid City",
                    CountryCode = HybridCodes.CountryCode,
                    CarrierTypeId = "SL",
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                };
                Response serviceResponse = serviceClient.Upsert(entityPM, false);
                return serviceResponse;
            }
        }
    }
}
