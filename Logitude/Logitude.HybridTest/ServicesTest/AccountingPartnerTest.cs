using System;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class AccountingPartnerTest
    {
        public TestContext TestContext { get; set; }
        EntityWcfCaller entityWcfCaller = new EntityWcfCaller();
        [TestMethod]
        public void Test_AccountingPartner_UPSERT()
        {
            try
            {
                RetryTest.InsertTestMethodToDictionary(TestContext.TestName);
                AccountingPartnerPM accountingPartnerPM = new AccountingPartnerPM()
                {
                    Code = HybridData.AccountingPartnerCodeHAPartner,
                    EnglishName = "Hybrid AccountingPartner",
                    LocalName = "Hybrid AccountingPartner",
                    CityName = "Hybrid City",
                    CountryCode = HybridData.CountryCodeUS,
                    PartnerTypeId = "AC",
                    Tenant = EnvironmentGlobalParams.MainTenant,
                };
                ServiceOutcome serviceOutcome = entityWcfCaller.CallEntityUpsert(accountingPartnerPM);
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
