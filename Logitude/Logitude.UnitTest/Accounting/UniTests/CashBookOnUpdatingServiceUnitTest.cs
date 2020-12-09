using System;
using FakeItEasy;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Def.EntityPMs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.UnitTest.Accounting.UniTests
{
    [TestClass]
    public class CashBookOnUpdatingServiceUnitTest
    {
        [TestMethod]
        public void OnUpdating_FillUpdatedByUserId_Success()
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
                Id = expcted_IdCounter,
                Tenant = tenant,
                EnglishName = englishName,
                LocalName = localName,
                AccountNumber = accountNumber,
                AccountName = accountName,
                CashBookTypeName = cashBookTypeName
            };

            var fakeCashBookOnUpdatingUpdateService = A.Fake<CashBookOnUpdatingService>(option => option.Implements<ICashBookOnUpdatingUpdateService>());

            A.CallTo(() => fakeCashBookOnUpdatingUpdateService.GetLogContactId(entityPM)).Returns(expectedLoggedUserId);
            A.CallTo(() => fakeCashBookOnUpdatingUpdateService.GetCurrentDateTime(entityPM)).Returns(DateTime.Now);

            fakeCashBookOnUpdatingUpdateService.OnUpdating(entityPM);
            Assert.AreEqual(expectedLoggedUserId, entityPM.UpdatedByUserId);
        }
    }
}
