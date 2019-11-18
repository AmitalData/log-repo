using System;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class CustomerAdditionalServiceDWTest
    {
        [TestMethod]
        public void Test_CustomerAdditionalServiceDW_GetCustomerCompetitors()
        {
            LoginService.GetLoginTokenByCredentials();
            CustomerAdditionalServiceDWServiceReference.CustomerAdditionalServiceDWWcfServiceClient serviceClient = new CustomerAdditionalServiceDWServiceReference.CustomerAdditionalServiceDWWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                CustomerAdditionalServiceDWServiceReference.CustomerAdditionalServiceDW[] entityList = serviceClient.GetCustomerAdditionalServices(TestEnvironmentGlobalParameters.Tenant, ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Get Customer Additional Services Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get Customer Additional Services Failed! " + serviceResponse.ErrorMessage);
                if (entityList.Length != 0)
                {
                     //Assert.AreEqual(entityList[0].AdditionalServiceName,"B2B", "B2B Customer Additional Service Doesn't Exist! ");
                }
                else
                {
                    Assert.Inconclusive("There Isn't Customer Additional Services!");
                }
            }
        }
    }
}
