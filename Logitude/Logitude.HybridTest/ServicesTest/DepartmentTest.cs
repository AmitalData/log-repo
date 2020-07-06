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
        [TestMethod]
        public void Test_Department_UPSERT()
        {
            DepartmentPM departmentPM = new DepartmentPM()
            {
                Code = HybridData.DepartmentCodeHDEP,
                EnglishName = "Hybrid Department",
                LocalName = "Hybrid Department",
                Tenant = EnvironmentGlobalParams.MainTenant,
            };
            ServiceOutcome serviceOutcome = EntityWcfCaller.CallEntityUpsert(departmentPM);
            Assert.IsFalse(serviceOutcome.Response.HasError, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNotNull(serviceOutcome.Response.Result, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
        }
    }
}
