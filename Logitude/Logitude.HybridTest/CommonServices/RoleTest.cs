using System;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class RoleTest
    {
        [TestMethod]
        public void Test_Role_GetRoles()
        {
            LoginService.GetLoginTokenByCredentials();
            RoleServiceReference.RoleWcfServiceClient serviceClient = new RoleServiceReference.RoleWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                RoleServiceReference.RoleList[] serviceResult = serviceClient.GetRoles(TestEnvironmentGlobalParameters.Tenant, ref serviceResponse);
                Assert.AreNotEqual(serviceResult.Length, 0, "There Isn't Roles!");
                Assert.IsFalse(serviceResponse.HasError, "Get Roles Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get Roles Failed! " + serviceResponse.ErrorMessage);
            }
        }        
    }
}