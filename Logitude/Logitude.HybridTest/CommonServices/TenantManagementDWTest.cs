using System;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class TenantManagementDWTest
    {
        [TestMethod]
        public void Test_TenantManagementDW_GetTenantManagements()
        {
            LoginService.GetLoginTokenByCredentials();
            TenantManagementDWServiceReference.TenantManagementDWWcfServiceClient serviceClient = new TenantManagementDWServiceReference.TenantManagementDWWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                TenantManagementDWServiceReference.TenantManagementDW[] entityList = serviceClient.GetTenantManagements(TestEnvironmentGlobalParameters.Tenant, 0, 10, ref serviceResponse);
                Assert.AreEqual(entityList[0].PackageCode,"TNT0", "Tenant 0 Doesn't Exist! " + serviceResponse.ErrorMessage);
                Assert.IsFalse(serviceResponse.HasError, "Get Tenant Managements Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get Tenant Managements Failed! " + serviceResponse.ErrorMessage);
            }
        }
    }
}
