using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class UserTest
    {
        [TestMethod]
        public void Test_User_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Server.Tools.Response branchServiceResponse = BranchTest.CallBranchUpsert();
            Assert.IsFalse(branchServiceResponse.HasError, "Branch Upsert Failed! " + branchServiceResponse.ErrorMessage);
            Assert.IsNotNull(branchServiceResponse.Result, "Branch Upsert Failed! " + branchServiceResponse.ErrorMessage);
            Server.Tools.Response departmentServiceResponse = DepartmentTest.CallDepartmentUpsert();
            Assert.IsFalse(departmentServiceResponse.HasError, "Departmnet Upsert Failed! " + departmentServiceResponse.ErrorMessage);
            Assert.IsNotNull(departmentServiceResponse.Result, "Department Upsert Failed! " + departmentServiceResponse.ErrorMessage);
            Server.Tools.Response serviceResponse = CallUserUpsert();
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }

        public static Server.Tools.Response CallUserUpsert()
        {

            UserServiceReference.UserWcfServiceClient serviceClient = new UserServiceReference.UserWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)serviceClient.InnerChannel))
            {

                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                UserServiceReference.UserPM entityPM = new UserServiceReference.UserPM()
                {
                    Code = HybridCodes.UserCode,
                    EnglishName = "Hybrid User",
                    LocalName = "Hybrid User",
                    Email = "Hybrid@fnarsoft.com",
                    Password = "!H0",
                    BusinessUnitId = TestEnvironmentGlobalParameters.Tenant.ToString(),
                    BranchId = HybridCodes.BranchCode,
                    DepartmentId = HybridCodes.DepartmentCode,
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                    DocumentFilingInbox = "HybridInbox"
                };

                Logitude.Server.Tools.Response serviceResponse = serviceClient.Upsert(entityPM, false);
                return serviceResponse;
            }
        }
    }
}
