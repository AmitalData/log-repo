using System;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.UserServiceReference;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class UserTest
    {
        [TestMethod]
        public void Test_User_UPSERT()
        {
            UserPM userPM = new UserPM()
            {
                Code = HybridData.UserCodeHU,
                EnglishName = "Hybrid User",
                LocalName = "Hybrid User",
                Email = "Hybrid@fnarsoft.com",
                Password = "!H0",
                BusinessUnitId = EnvironmentGlobalParams.MainTenant.ToString(),
                BranchId = HybridData.BranchCodeHBRA,
                DepartmentId = HybridData.DepartmentCodeHDEP,
                Tenant = EnvironmentGlobalParams.MainTenant,
                DocumentFilingInbox = "HybridInbox"
            };
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(userPM);
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_User_GetUser()
        {
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
                SearchCode = HybridData.UserCodeHU
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { filters, EnvironmentGlobalParams.MainTenant, serviceResponse };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);
            UserPM user = (UserPM)serviceOutcome.Result;
            Assert.IsFalse(serviceOutcome.Response.HasError, "Get List Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Get List Failed! " + serviceOutcome.Response.Result);
            Assert.AreEqual(user.EnglishName, "Hybrid User", "Get Hybrid User From Users Failed!");
        }
    }
}