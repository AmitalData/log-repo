using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Test.Infrastructure;
using FakeItEasy;

namespace Logitude.UnitTest.Accounting.UniTests.PaymentChequeTests
{
    /// <summary>
    /// Refactored version of PaymentChequeOnUpdatingServiceUnitTest using FakeItEasy.
    /// Migrated from MSTest Fakes to FakeItEasy for compatibility with Visual Studio Community.
    /// </summary>
    [TestClass]
    public class PaymentChequeOnUpdatingServiceUnitTest_MSTest : TestBase_MSTest
    {
        [TestMethod]
        public void OnUpdating_PaymentChequeUpdate_Success()
        {
            int tenant = 1;
            var expcted_IdCounter = "1-1";
            var expcted_Number = "1000";
            var expcted_LogId = GetLoggedContactInstance().Id;

            var PaymentChequePM = new PaymentChequePM()
            {
                Tenant = tenant,
                CreatedByUserId = expcted_LogId,
                Id = expcted_IdCounter,
                InternalNumber = expcted_Number,
            };

            var paymentChequeOnUpdatingService = A.Fake<PaymentChequeOnUpdatingService>(option => option.CallsBaseMethods());
            A.CallTo(() => paymentChequeOnUpdatingService.GetLogContactId(tenant)).Returns(expcted_LogId);
            A.CallTo(() => paymentChequeOnUpdatingService.GetCurrentDateTime(tenant)).Returns(DateTime.Now);

            paymentChequeOnUpdatingService.OnUpdating(PaymentChequePM, null);

            Assert.AreEqual(expcted_IdCounter, PaymentChequePM.Id);
            Assert.AreEqual(expcted_Number.ToString(), PaymentChequePM.InternalNumber, "does not exist");
        }

        [TestMethod]
        public void OnUpdating_CreatingJournal_Success()
        {
            int tenant = 1;
            var expcted_IdCounter = "1-1";
            var expcted_Number = "1000";
            var expcted_LogId = GetLoggedContactInstance().Id;

            var PaymentChequePM = new PaymentChequePM()
            {
                Tenant = tenant,
                CreatedByUserId = expcted_LogId,
                Id = expcted_IdCounter,
                InternalNumber = expcted_Number,
                ChequeNumber = "11",
                UpdatedByUserId = expcted_LogId,
                PaymentChequeStatusCode = "2",
                BankAccountId = "1-1"
            };

            JournalPM journalPM = new JournalPM()
            {
                Tenant = 1,
                AccountingEntityReference = PaymentChequePM.ChequeNumber,
                UpdatedByUserId = PaymentChequePM.UpdatedByUserId,
            };

            var paymentChequeOnUpdatingService = A.Fake<PaymentChequeOnUpdatingService>(option => option.CallsBaseMethods());
            A.CallTo(() => paymentChequeOnUpdatingService.GetCurrentDateTime(tenant)).Returns(DateTime.Now);
            A.CallTo(() => paymentChequeOnUpdatingService.CreateJournalPM(A<PaymentChequePM>.Ignored)).Returns(journalPM);
            
            // Test logic would go here if the original test had assertions
        }

        private ContactPM GetLoggedContactInstance()
        {
            string expectedLoggedUserId = "myUser";
            ContactPM loggedcontact = new ContactPM()
            {
                Id = expectedLoggedUserId,
                DontShowLocal = true,
            };
            return loggedcontact;
        }
    }
}

