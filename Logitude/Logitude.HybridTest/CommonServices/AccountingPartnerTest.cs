using System;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class AccountingPartnerTest
    {
        [TestMethod]
        public void Test_AccountingPartner_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Response serviceResponse = AccountingPartnerWcfCaller.CallAccountingPartnerUpsert();
            Assert.IsFalse(serviceResponse.HasError, serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }
    }
}
