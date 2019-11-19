using System;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class ChargeTypeTest
    {
        [TestMethod]
        public void Test_ChargeType_GetChargeTypes()
        {
            LoginService.GetLoginTokenByCredentials();
            ChargeTypeServiceReference.ChargeTypeWcfServiceClient serviceClient = new ChargeTypeServiceReference.ChargeTypeWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                ChargeTypeServiceReference.ChargesTypeList[] entityList = serviceClient.GetChargesTypes(TestEnvironmentGlobalParameters.Tenant, 0, 10, ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Get Charge Types Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get Charge Types Failed! " + serviceResponse.ErrorMessage);
                if(entityList.Length != 0)
                {
                    //Assert.AreEqual(entityList.ExternalId, HybridCodes.AddressCode, serviceResponse.ErrorMessage);
                }
                else
                {
                    Assert.Inconclusive("There Isn't Charge Types!");
                }
            }
        }
    }
}
