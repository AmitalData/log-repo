using System;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class ShipmentFollowerstTest
    {
        [TestMethod]
        public void Test_ShipmentFollowerst_GetShipmentFollowersByShipmentNumber()
        {
            LoginService.GetLoginTokenByCredentials();
            ShipmentFollowerstServiceReference.ShipmenFollowerstWcfServiceClient serviceClient = new ShipmentFollowerstServiceReference.ShipmenFollowerstWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                ShipmentFollowerstServiceReference.ContactList[] entityList = serviceClient.GetShipmentFollowersByShipmentNumber(HybridCodes.AgentCode, TestEnvironmentGlobalParameters.Tenant, ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Get Shipment Followers By Shipment Number Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get Shipment Followers By Shipment Number Failed! " + serviceResponse.ErrorMessage);
                if(entityList.Length == 0) {
                    Assert.Inconclusive("Doesn't Exist Any Shipment Followers!");
                }
            }
        }
    }
}
