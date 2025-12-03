using System;
using Logitude.Accounting.Def.EntityPMs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Test.Infrastructure;
using FakeItEasy;

namespace Logitude.UnitTest.Accounting.UniTests
{
    /// <summary>
    /// Refactored version of CashBookOnCreatingServiceUnitTest using FakeItEasy.
    /// Migrated from MSTest Fakes to FakeItEasy for compatibility with Visual Studio Community.
    /// </summary>
    [TestClass]
    public class CashBookOnCreatingServiceUnitTest_MSTest : TestBase_MSTest
    {
        [TestMethod]
        public void OnCreating_FillDefaultFields_Success()
        {
            int tenant = 1;
            var expcted_IdCounter = "1-IdCounter";
            string englishName = "Mohammad";
            string localName = "محمد";
            string accountNumber = "1234";
            string accountName = "account1";
            string cashBookTypeName = "type1";
            string expectedLoggedUserId = "mohammadId";
            CashBookPM entityPM = new CashBookPM()
            {
                Tenant = tenant,
                EnglishName = englishName,
                LocalName = localName,
                AccountNumber = accountNumber,
                AccountName = accountName,
                CashBookTypeName = cashBookTypeName
            };
            string expectedSearchFields = entityPM.EnglishName + "," + entityPM.LocalName + "," + entityPM.AccountNumber + "," + entityPM.AccountName + "," + entityPM.CashBookTypeName;

            var fakeCashBookOnCreatingUpdateService = A.Fake<CashBookOnCreatingService>(option => option.Implements<ICashBookOnCreatingUpdateService>());

            A.CallTo(() => fakeCashBookOnCreatingUpdateService.GetLogContactId(entityPM)).Returns(expectedLoggedUserId);
            A.CallTo(() => fakeCashBookOnCreatingUpdateService.GetCurrentDateTime(entityPM)).Returns(DateTime.Now);
            A.CallTo(() => fakeCashBookOnCreatingUpdateService.IdCounterWrapperGetNumber(entityPM.Tenant)).Returns(expcted_IdCounter);

            fakeCashBookOnCreatingUpdateService.OnCreating(entityPM, null);
            Assert.AreEqual(expcted_IdCounter, entityPM.Id);
        }
    }
}

