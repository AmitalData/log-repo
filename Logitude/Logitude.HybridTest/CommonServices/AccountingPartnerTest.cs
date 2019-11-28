using System;
using Logitude.BL.CommonDataModel.EntityPMs;
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
            AccountingPartnerPM accountingPartnerPM = new AccountingPartnerPM()
            {
                Code = HybridData.AccountingPartnerCode,
                EnglishName = "Hybrid AccountingPartner",
                LocalName = "Hybrid AccountingPartner",
                CityName = "Hybrid City",
                CountryCode = HybridData.CountryCode,
                PartnerTypeId = "AC",
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(accountingPartnerPM);
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }
    }
}
