using System;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class DepartmentTest
    {
        [TestMethod]
        public void Test_Department_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Response serviceResponse = DepartmentWcfCaller.CallDepartmentUpsert();
            Assert.IsFalse(serviceResponse.HasError, serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }
    }
}
