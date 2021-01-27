using System;
using Logitude.BL.GlobalModel.EntityDws;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class TenantManagementDWTest
    {
        public TestContext TestContext { get; set; }
        [TestMethod]
        public void Test_TenantManagementDW_GetTenantManagements()
        {
            try
            {
                RetryTest.InsertTestMethodToDictionary(TestContext.TestName); 
                InvokedProperties serviceProperties = new InvokedProperties
                {
                    ServiceName = "TenantManagementDW",
                    ServiceOperation = "GetTenantManagements",
                    ServiceResponseIndex = 3,
                    ServiceType = typeof(TenantManagementDW),
                    ServiceFilterType = null,
                };

                Response serviceResponse = new Response();
                object[] serviceParameters = new object[] { EnvironmentGlobalParams.MainTenant, 0, 10, serviceResponse };
                ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);
                TenantManagementDW[] tenantManagementDW = (TenantManagementDW[])serviceOutcome.Result;
                Assert.IsFalse(serviceOutcome.Response.HasError, "Get Tenant Managements Failed! " + serviceOutcome.Response.ErrorMessage);
                Assert.IsNull(serviceOutcome.Response.Result, "Get Tenant Managements Failed! " + serviceOutcome.Response.Result);
                //Assert.AreEqual(tenantManagementDW[0].PackageCode, "TNT0", "Tenant 0 Doesn't Exist! " + serviceOutcome.Response.ErrorMessage);
            }
            catch (Exception ex)
            {
                RetryTest.RetryFailTestRun(TestContext, this, ex.Message);
            }
        }
    }
}
