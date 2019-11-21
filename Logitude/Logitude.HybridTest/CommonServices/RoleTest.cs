using System;
using Logitude.BL.CommonDataModel.EntityLists;
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
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Role",
                ServiceOperation = "GetRoles",
                ServiceResponseIndex = 1,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { TestEnvironmentGlobalParameters.Tenant, serviceResponse };
            RoleList[] roles = (RoleList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, typeof(RoleList), ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.ErrorMessage);
            if(roles.Length == 0)
                Assert.Inconclusive("There Isn't Roles!");
        }        
    }
}