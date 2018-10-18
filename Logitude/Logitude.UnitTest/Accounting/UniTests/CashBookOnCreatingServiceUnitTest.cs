using System;
using Logitude.Accounting.Def.EntityPMs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using Logitude.Accounting.BL.EntityUpdateServices;

namespace Logitude.UnitTest.Accounting.UniTests
{
    [TestClass]
    public class CashBookOnCreatingServiceUnitTest
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
            // entityPM.SearchFields = entityPM.EnglishName + "," + entityPM.LocalName + "," + entityPM.AccountNumber + "," + entityPM.AccountName + "," + entityPM.CashBookTypeName;
            string expectedSearchFields= entityPM.EnglishName + "," + entityPM.LocalName + "," + entityPM.AccountNumber + "," + entityPM.AccountName + "," + entityPM.CashBookTypeName;

            var fakeCashBookOnCreatingUpdateService = A.Fake<CashBookOnCreatingService>(option=>option.CallsBaseMethods());

            A.CallTo(() => fakeCashBookOnCreatingUpdateService.GetLogContactId(entityPM)).Returns(expectedLoggedUserId);
            A.CallTo(() => fakeCashBookOnCreatingUpdateService.GetCurrentDateTime(entityPM)).Returns(DateTime.Now);
            A.CallTo(() => fakeCashBookOnCreatingUpdateService.IdCounterWrapperGetNumber(entityPM.Tenant)).Returns(expcted_IdCounter);

            fakeCashBookOnCreatingUpdateService.OnCreating(entityPM, null);
            Assert.AreEqual(expcted_IdCounter, entityPM.Id);

        }
    }
}
