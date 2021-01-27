using System;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class DepartmentTest
    {
        private static EntityWcfCaller entityWcfCaller = new EntityWcfCaller();
        public TestContext TestContext { get; set; }
        [TestMethod]
        public void Test_Department_UPSERT()
        {
            try
            {
                RetryTest.InsertTestMethodToDictionary(TestContext.TestName);
                DepartmentPM departmentPM = new DepartmentPM()
                {
                    Code = HybridData.DepartmentCodeHDEP,
                    EnglishName = "Hybrid Department",
                    LocalName = "Hybrid Department",
                    Tenant = EnvironmentGlobalParams.MainTenant,
                };
                ServiceOutcome serviceOutcome = entityWcfCaller.CallEntityUpsert(departmentPM);
                Assert.IsFalse(serviceOutcome.Response.HasError, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
                Assert.IsNotNull(serviceOutcome.Response.Result, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
            }
            catch (Exception ex)
            {
                RetryTest.RetryFailTestRun(TestContext, this, ex.Message);
            }
        }
    }
}
