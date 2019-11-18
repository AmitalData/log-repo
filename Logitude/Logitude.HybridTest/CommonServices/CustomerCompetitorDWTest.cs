using System;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class CustomerCompetitorDWTest
    {
        [TestMethod]
        public void Test_CustomerCompetitorDW_GetCustomerCompetitors()
        {
            LoginService.GetLoginTokenByCredentials();
            CustomerCompetitorServiceReference.CustomerCompetitorDWWcfServiceClient serviceClient = new CustomerCompetitorServiceReference.CustomerCompetitorDWWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                CustomerCompetitorServiceReference.CustomerCompetitorDW[] entityList = serviceClient.GetCustomerCompetitors(TestEnvironmentGlobalParameters.Tenant, ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Get Tenant Managements Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get Tenant Managements Failed! " + serviceResponse.ErrorMessage);
                if (entityList.Length != 0)
                {
                     //Assert.AreEqual(entityList[0].CompetitorName,"lana", "lana competitor Doesn't Exist! " );
                }
                else
                {
                    Assert.Inconclusive("There Isn't Competitiors!");
                }
            }
        }
    }
}
