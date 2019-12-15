using System;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class RoleTest
    {
        [TestMethod]
        public void Test_Role_GetRoles()
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Role",
                ServiceOperation = "GetRoles",
                ServiceResponseIndex = 1,
                ServiceType = typeof(RoleList),
                ServiceFilterType = null,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { EnvironmentGlobalParams.MainTenant, serviceResponse };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);
            RoleList[] roles = (RoleList[])serviceOutcome.Result;
            Assert.IsFalse(serviceOutcome.Response.HasError, "Get List Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Get List Failed! " + serviceOutcome.Response.Result);
            if (roles.Length == 0)
                Assert.Inconclusive("There Isn't Roles!");
        }        
    }
}