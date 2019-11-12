using System;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class DepartmentTest
    {
        [TestMethod]
        public void Test_Department_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Response serviceResponse = CallDepartmentUpsert();
            Assert.AreEqual(serviceResponse.HasError, false, serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }

        public static Response CallDepartmentUpsert()
        {
            DepartmentServiceReference.DepartmentWcfServiceClient serviceClient = new DepartmentServiceReference.DepartmentWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                DepartmentServiceReference.DepartmentPM entityPM = new DepartmentServiceReference.DepartmentPM()
                {
                    Code = HybridCodes.DepartmentCode,
                    EnglishName = "Hybrid Department",
                    LocalName = "Hybrid Department",
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                };
                Response serviceResponse = serviceClient.Upsert(entityPM, false);
                return serviceResponse;
            }
        }
    }
}
