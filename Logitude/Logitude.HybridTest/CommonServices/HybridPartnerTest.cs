using System;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class HybridPartnerTest
    {
        [TestMethod]
        public void Test_HybridPartner_GetMislakaPartners()
        {
            LoginService.GetLoginTokenByCredentials();
            HybridPartnerServiceReference.HybridPartnerWcfServiceClient serviceClient = new HybridPartnerServiceReference.HybridPartnerWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                HybridPartnerServiceReference.HybridPartnerList[] entityList = serviceClient.GetMislakaPartners(TestEnvironmentGlobalParameters.Tenant, ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Get Customer Additional Services Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get Mislaka Partners Failed! " + serviceResponse.ErrorMessage);
                if (entityList.Length != 0)
                {
                     Assert.AreEqual(entityList[0].LocalName, "Forwarder", "Forwarder Mislaka Partner Doesn't Exist! ");
                }
                else
                {
                    Assert.Inconclusive("There Isn't Mislaka Partners!");
                }
            }
        }
    }
}
