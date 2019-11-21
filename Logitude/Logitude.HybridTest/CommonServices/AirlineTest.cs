using System;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class AirlineTest
    {
        [TestMethod]
        public void Test_Airline_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            AirlinePM entityPM = new AirlinePM()
            {
                Code = "HA",
                EnglishName = "Hybrid Airline",
                LocalName = "Hybrid Airline",
                Prefix = TestEnvironmentGlobalParameters.Tenant.ToString(),
                Tenant = TestEnvironmentGlobalParameters.Tenant,
                CarrierTypeId = "AL",
            };

            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Airline",
                IServiceName = "IAirlineWcfService",
                ServiceOperation = "Upsert",
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { entityPM, false };
            WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, typeof(AirlinePM), ref serviceResponse);

            Assert.IsFalse(serviceResponse.HasError, serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
         }

        //public static Response CallAirlineUpsert()
        //{
        //    AirlineServiceReference.AirlineWcfServiceClient serviceClient = new AirlineServiceReference.AirlineWcfServiceClient();
        //    string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
        //    serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
        //    using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
        //    {
        //        System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
        //        AirlineServiceReference.AirlinePM entityPM = new AirlineServiceReference.AirlinePM()
        //        {
        //            Code = "HA",
        //            EnglishName = "Hybrid Airline",
        //            LocalName = "Hybrid Airline",
        //            Prefix = TestEnvironmentGlobalParameters.Tenant.ToString(),
        //            Tenant = TestEnvironmentGlobalParameters.Tenant,
        //            CarrierTypeId = "AL",
        //        };
        //        Response serviceResponse = serviceClient.Upsert(entityPM, false);
        //        return serviceResponse;
        //    }
        //}
    }
}
