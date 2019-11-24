using System;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.HybridTest.UserServiceReference;
using Logitude.HybridTest.WcfCallers;
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
            Response serviceResponse = UserWcfCaller.CallUserUpsert();
            Assert.IsFalse(serviceResponse.HasError, serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_User_GetUser()
        {
            LoginService.GetLoginTokenByCredentials();
            Response prepareResponse = UserWcfCaller.PrepareUser();
            Assert.IsFalse(prepareResponse.HasError, "Prepare User Failed! " + prepareResponse.ErrorMessage);
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "User",
                ServiceOperation = "GetUser",
                ServiceResponseIndex = 2,
                ServiceType = typeof(UserPM),
                ServiceFilterType = typeof(UserApiFilters),
            };
            UserApiFilters filters = new UserApiFilters
            {
                ByCode = true,
                SearchCode = HybridData.UserCode
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { filters, TestEnvironmentGlobalParameters.Tenant, serviceResponse };
            UserPM user = (UserPM)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsTrue(CheckResult(user), "Get Hybrid User Item From Users Failed!");
        }
        public bool CheckResult(UserPM user)
        {
            return user.EnglishName == "Hybrid User";
        }
    }
}