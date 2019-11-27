using System;
using Logitude.BL.GlobalModel.EntityDws;
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
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "TenantManagementDW",
                ServiceOperation = "GetTenantManagements",
                ServiceResponseIndex = 3,
                ServiceType = typeof(TenantManagementDW),
                ServiceFilterType = null,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { TestEnvironmentGlobalParameters.Tenant, 0, 10, serviceResponse };
            TenantManagementDW[] tenantManagementDW = (TenantManagementDW[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get Tenant Managements Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get Tenant Managements Failed! " + serviceResponse.Result);
            Assert.AreEqual(tenantManagementDW[0].PackageCode, "TNT0", "Tenant 0 Doesn't Exist! " + serviceResponse.ErrorMessage);
        }
    }
}
