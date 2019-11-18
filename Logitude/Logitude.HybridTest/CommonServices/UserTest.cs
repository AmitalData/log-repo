using System;
using Logitude.Server.Tools;
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
            Response branchServiceResponse = BranchTest.CallBranchUpsert();
            Assert.IsFalse(branchServiceResponse.HasError, "Branch Upsert Failed! " + branchServiceResponse.ErrorMessage);
            Assert.IsNotNull(branchServiceResponse.Result, "Branch Upsert Failed! " + branchServiceResponse.ErrorMessage);
            Response departmentServiceResponse = DepartmentTest.CallDepartmentUpsert();
            Assert.IsFalse(departmentServiceResponse.HasError, "Departmnet Upsert Failed! " + departmentServiceResponse.ErrorMessage);
            Assert.IsNotNull(departmentServiceResponse.Result, "Department Upsert Failed! " + departmentServiceResponse.ErrorMessage);
            Response serviceResponse = CallUserUpsert();
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_User_GetUser()
        {
            Test_User_UPSERT();
            UserServiceReference.UserWcfServiceClient serviceClient = new UserServiceReference.UserWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                UserServiceReference.UserApiFilters filters = new UserServiceReference.UserApiFilters
                {
                    ByCode = true,
                    SearchCode = HybridCodes.UserCode
                };
                UserServiceReference.UserPM serviceResult = serviceClient.GetUser(filters, TestEnvironmentGlobalParameters.Tenant, ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.ErrorMessage);
                if (serviceResult != null)
                {
                    string userCode = serviceResult.Code;
                    Assert.AreEqual(userCode, HybridCodes.UserCode, "Hybrid User Doesn't Exist!");
                }
                else
                {
                    Assert.Inconclusive("There Isn't USer With This Code!");
                }
            }
        }

        public static Response CallUserUpsert()
        {
            UserServiceReference.UserWcfServiceClient serviceClient = new UserServiceReference.UserWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
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
                Response serviceResponse = serviceClient.Upsert(entityPM, false);
                return serviceResponse;
            }
        }
    }
}