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
            RoleList[] roles = (RoleList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.Result);
            if(roles.Length == 0)
                Assert.Inconclusive("There Isn't Roles!");
        }        
    }
}