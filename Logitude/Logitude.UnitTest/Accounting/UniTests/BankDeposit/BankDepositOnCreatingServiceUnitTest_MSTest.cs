using System;
using System.Collections.Generic;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.UnitTest.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logitude.Test.Infrastructure;
using FakeItEasy;

namespace Logitude.UnitTest.Accounting.UniTests
{
    /// <summary>
    /// Refactored version of BankDepositOnCreatingServiceUnitTest using FakeItEasy.
    /// Migrated from MSTest Fakes to FakeItEasy for compatibility with Visual Studio Community.
    /// </summary>
    [TestClass]
    public class BankDepositOnCreatingServiceUnitTest_MSTest : TestBase_MSTest
    {
        [TestMethod]
        public void OnCreating_IdMatchesExpected_Success()
        {
            ContactPM loggedcontact = GetLoggedContactInstance();
            string expectedLoggedUserId = loggedcontact.Id;
            DateTime expectedDateTime = DateTime.Now;
            string expectedIdCounter = "myNewIdCounter";
            int expectedCodeCounter = 1000;

            BankDepositPM entityPM = new BankDepositPM()
            {
                Tenant = 1,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
            };

            var bankDepositOnCreatingService = A.Fake<BankDepositOnCreatingService>(option => option.Implements<IBankDepositOnCreatingService>());
            A.CallTo(() => bankDepositOnCreatingService.GetLoggedContact(entityPM.Tenant)).Returns(loggedcontact);
            A.CallTo(() => bankDepositOnCreatingService.GetCurrentDateTime(entityPM.Tenant)).Returns(expectedDateTime);
            A.CallTo(() => bankDepositOnCreatingService.IdCounterWrapperGetNumber(entityPM.Tenant)).Returns(expectedIdCounter);
            A.CallTo(() => bankDepositOnCreatingService.CodeCounterWrapperGetNumber(entityPM.Tenant)).Returns(expectedCodeCounter);
            A.CallTo(() => bankDepositOnCreatingService.GetCashbookById(A<int>.Ignored, A<string>.Ignored)).Returns(new CashBookPM());
            A.CallTo(() => bankDepositOnCreatingService.CreateJournalForBankDeposit(A<BankDepositPM>.Ignored)).DoesNothing();
            A.CallTo(() => bankDepositOnCreatingService.DepositChequesForBankDeposit(A<BankDepositPM>.Ignored)).DoesNothing();
            A.CallTo(() => bankDepositOnCreatingService.SubmitCashbook(A<int>.Ignored, A<CashBookPM>.Ignored)).DoesNothing();
            A.CallTo(() => bankDepositOnCreatingService.LogActivity(A<BankDepositPM>.Ignored)).DoesNothing();

            bankDepositOnCreatingService.OnCreating(entityPM);

            Assert.AreEqual(expectedIdCounter, entityPM.Id, "Id not matches expected #DP01");
        }

        [TestMethod]
        public void OnCreating_CodeMatchesExpected_Success()
        {
            ContactPM loggedcontact = GetLoggedContactInstance();
            string expectedLoggedUserId = loggedcontact.Id;
            DateTime expectedDateTime = DateTime.Now;
            string expectedIdCounter = "myNewIdCounter";
            int expectedCodeCounter = 1000;

            BankDepositPM entityPM = new BankDepositPM()
            {
                Tenant = 1,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
            };

            var bankDepositOnCreatingService = A.Fake<BankDepositOnCreatingService>(option => option.Implements<IBankDepositOnCreatingService>());
            A.CallTo(() => bankDepositOnCreatingService.GetLoggedContact(entityPM.Tenant)).Returns(loggedcontact);
            A.CallTo(() => bankDepositOnCreatingService.GetCurrentDateTime(entityPM.Tenant)).Returns(expectedDateTime);
            A.CallTo(() => bankDepositOnCreatingService.IdCounterWrapperGetNumber(entityPM.Tenant)).Returns(expectedIdCounter);
            A.CallTo(() => bankDepositOnCreatingService.CodeCounterWrapperGetNumber(entityPM.Tenant)).Returns(expectedCodeCounter);
            A.CallTo(() => bankDepositOnCreatingService.GetCashbookById(A<int>.Ignored, A<string>.Ignored)).Returns(new CashBookPM());
            A.CallTo(() => bankDepositOnCreatingService.CreateJournalForBankDeposit(A<BankDepositPM>.Ignored)).DoesNothing();
            A.CallTo(() => bankDepositOnCreatingService.DepositChequesForBankDeposit(A<BankDepositPM>.Ignored)).DoesNothing();
            A.CallTo(() => bankDepositOnCreatingService.SubmitCashbook(A<int>.Ignored, A<CashBookPM>.Ignored)).DoesNothing();
            A.CallTo(() => bankDepositOnCreatingService.LogActivity(A<BankDepositPM>.Ignored)).DoesNothing();

            bankDepositOnCreatingService.OnCreating(entityPM);

            Assert.AreEqual(expectedCodeCounter, entityPM.DepositNumber, "Code not matches expected #DP02");
        }

        [TestMethod]
        public void OnCreating_LineTakeParentId_Success()
        {
            ContactPM loggedcontact = GetLoggedContactInstance();
            string expectedLoggedUserId = loggedcontact.Id;
            DateTime expectedDateTime = DateTime.Now;
            string expectedParentIdCounter = "myNewIdCounter";
            int expectedCodeCounter = 1000;

            BankDepositPM entityPM = new BankDepositPM()
            {
                Tenant = 1,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                BankDepositLines = new List<BankDepositLinePM> { new BankDepositLinePM() },
            };

            var bankDepositOnCreatingService = A.Fake<BankDepositOnCreatingService>(option => option.Implements<IBankDepositOnCreatingService>());
            A.CallTo(() => bankDepositOnCreatingService.GetLoggedContact(entityPM.Tenant)).Returns(loggedcontact);
            A.CallTo(() => bankDepositOnCreatingService.GetCurrentDateTime(entityPM.Tenant)).Returns(expectedDateTime);
            A.CallTo(() => bankDepositOnCreatingService.IdCounterWrapperGetNumber(entityPM.Tenant)).Returns(expectedParentIdCounter);
            A.CallTo(() => bankDepositOnCreatingService.CodeCounterWrapperGetNumber(entityPM.Tenant)).Returns(expectedCodeCounter);
            A.CallTo(() => bankDepositOnCreatingService.LogActivity(A<BankDepositPM>.Ignored)).DoesNothing();
            A.CallTo(() => bankDepositOnCreatingService.GetCashbookById(A<int>.Ignored, A<string>.Ignored)).Returns(new CashBookPM());
            A.CallTo(() => bankDepositOnCreatingService.CreateJournalForBankDeposit(A<BankDepositPM>.Ignored)).DoesNothing();
            A.CallTo(() => bankDepositOnCreatingService.DepositChequesForBankDeposit(A<BankDepositPM>.Ignored)).DoesNothing();
            A.CallTo(() => bankDepositOnCreatingService.SubmitCashbook(A<int>.Ignored, A<CashBookPM>.Ignored)).DoesNothing();

            bankDepositOnCreatingService.OnCreating(entityPM);

            var actualLineDepositId = entityPM.BankDepositLines[0].DepositId;
            Assert.AreEqual(expectedParentIdCounter, actualLineDepositId, "Deposit line does not take parent Id #DP03");
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

