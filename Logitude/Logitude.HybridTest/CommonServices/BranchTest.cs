using System;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class BranchTest
    {
        [TestMethod]
        public void Test_Branch_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Response serviceResponse = CallBranchUpsert();
            Assert.AreEqual(serviceResponse.HasError, false, serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
         }

        public static Response CallBranchUpsert()
        {
            BranchServiceReference.BranchWcfServiceClient serviceClient = new BranchServiceReference.BranchWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                BranchServiceReference.BranchPM entityPM = new BranchServiceReference.BranchPM()
                {
                    Code = HybridCodes.BranchCode,
                    EnglishName = "Hybrid Branch",
                    LocalName = "Hybrid Branch",
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                };
                Response serviceResponse = serviceClient.Upsert(entityPM, false);
                return serviceResponse;
            }
        }
    }
}
