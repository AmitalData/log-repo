using System;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class IncotermTest
    {
        [TestMethod]
        public void Test_Incoterm_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Response serviceResponse = CallIncotermUpsert();
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_Incoterm_GetIncoterms()
        {
            IncotermServiceReference.IncotermWcfServiceClient serviceClient = new IncotermServiceReference.IncotermWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                IncotermServiceReference.IncotermList[] entityList = serviceClient.GetIncoterms(ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Get Incoterms Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get Incoterms Failed! " + serviceResponse.ErrorMessage);
                if (entityList.Length != 0)
                {
                    //
                }
                else
                {
                    Assert.Inconclusive("There Isn't Any Incoterm!");
                }
            }
        }

        public static Response CallIncotermUpsert()
        {
            IncotermServiceReference.IncotermWcfServiceClient serviceClient = new IncotermServiceReference.IncotermWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                IncotermServiceReference.IncotermPM entityPM = new IncotermServiceReference.IncotermPM()
                {
                    Code = HybridData.IncotermCode,
                    Name = "Hybrid Incoterm",
                    LocalName = "Hybrid Incoterm",
                    Freight = "C",
                    OtherCharges = "C",
                    Tenant = TestEnvironmentGlobalParameters.Tenant
                };
                Response serviceResponse = serviceClient.Upsert(entityPM, false);
                return serviceResponse;
            }
        }
    }
}