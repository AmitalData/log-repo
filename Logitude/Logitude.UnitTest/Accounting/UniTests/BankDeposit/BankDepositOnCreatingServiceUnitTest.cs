using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FakeItEasy;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.UnitTest.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Logitude.UnitTest.Accounting.UniTests
{
    [TestClass]
    public class BankDepositOnCreatingServiceUnitTest
    {

        [TestMethod]
        public void OnCreating_IdMatchesExpected_Success()
        {
            // Arrang
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


            var bankAccountOnCreatingService = A.Fake<BankDepositOnCreatingService>(option => option.CallsBaseMethods());
            A.CallTo(() => bankAccountOnCreatingService.GetLoggedContact(entityPM.Tenant)).Returns(loggedcontact);
            A.CallTo(() => bankAccountOnCreatingService.GetCurrentDateTime(entityPM.Tenant)).Returns(expectedDateTime);
            A.CallTo(() => bankAccountOnCreatingService.IdCounterWrapperGetNumber(entityPM.Tenant)).Returns(expectedIdCounter);
            A.CallTo(() => bankAccountOnCreatingService.CodeCounterWrapperGetNumber(entityPM.Tenant)).Returns(expectedCodeCounter);

            // Act
            bankAccountOnCreatingService.OnCreating(entityPM);

            // Assert
            Assert.AreEqual(expectedIdCounter, entityPM.Id, "Id not matches expected #DP01");

        }

        [TestMethod]
        public void OnCreating_CodeMatchesExpected_Success()
        {
            // Arrang
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


            var bankAccountOnCreatingService = A.Fake<BankDepositOnCreatingService>(option => option.CallsBaseMethods());
            A.CallTo(() => bankAccountOnCreatingService.GetLoggedContact(entityPM.Tenant)).Returns(loggedcontact);
            A.CallTo(() => bankAccountOnCreatingService.GetCurrentDateTime(entityPM.Tenant)).Returns(expectedDateTime);
            A.CallTo(() => bankAccountOnCreatingService.IdCounterWrapperGetNumber(entityPM.Tenant)).Returns(expectedIdCounter);
            A.CallTo(() => bankAccountOnCreatingService.CodeCounterWrapperGetNumber(entityPM.Tenant)).Returns(expectedCodeCounter);

            // Act
            bankAccountOnCreatingService.OnCreating(entityPM);

            // Assert
            Assert.AreEqual(expectedCodeCounter, entityPM.DepositNumber, "Code not matches expected #DP02");

        }

        [TestMethod]
        public void OnCreating_LineTakeParentId_Success()
        {
            // Arrang
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

            var bankAccountOnCreatingService = A.Fake<BankDepositOnCreatingService>(option => option.CallsBaseMethods());
            A.CallTo(() => bankAccountOnCreatingService.GetLoggedContact(entityPM.Tenant)).Returns(loggedcontact);
            A.CallTo(() => bankAccountOnCreatingService.GetCurrentDateTime(entityPM.Tenant)).Returns(expectedDateTime);
            A.CallTo(() => bankAccountOnCreatingService.IdCounterWrapperGetNumber(entityPM.Tenant)).Returns(expectedParentIdCounter);
            A.CallTo(() => bankAccountOnCreatingService.CodeCounterWrapperGetNumber(entityPM.Tenant)).Returns(expectedCodeCounter);
            A.CallTo(() => bankAccountOnCreatingService.LogActivity(entityPM)).DoesNothing();

            // Act
            bankAccountOnCreatingService.OnCreating(entityPM);

            // Assert
            var actualLineDepositId = entityPM.BankDepositLines[0].DepositId;
            Assert.AreEqual(expectedParentIdCounter, actualLineDepositId, "Deposit line does not take parent Id #DP03");

        }

        [DataTestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void OnCreating_CashJournalInitiated_Success(bool isCashDeposit)
        {
            // Arrang
            ContactPM loggedcontact = GetLoggedContactInstance();
            string expectedLoggedUserId = loggedcontact.Id;
            DateTime currentDateTime = DateTime.Now;
            string expectedParentIdCounter = "myNewIdCounter";
            int expectedCodeCounter = 1000;

            BankDepositPM entityPM = new BankDepositPM()
            {
                Tenant = 1,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                CreatedByUserId = expectedLoggedUserId,
                CreateDate = currentDateTime,
                UpdatedByUserId = expectedLoggedUserId,
                UpdateDate = currentDateTime,
                AccountingDate = currentDateTime,
                Id = "myDepositId",
                DepositNumber = 1000,
                IsCashDeposit = isCashDeposit,
            };

            JournalPM expectedJournal = new JournalPM()
            {
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                Tenant = entityPM.Tenant,
                CreateDate = currentDateTime,
                CreatedByUserId = entityPM.CreatedByUserId,
                UpdateDate = currentDateTime,
                UpdatedByUserId = entityPM.UpdatedByUserId,
                AccountingDate = entityPM.AccountingDate,
                TypeCode = "0",
                StatusCode = "2",
                AccountingEntityId = entityPM.Id,
                AccountingEntityReference = entityPM.DepositNumber.ToString(),
                ExternalNo = null,
                ApproveDate = entityPM.CreateDate,
                ApprovedByUserId = entityPM.CreatedByUserId,
                AccountingEntityCode = isCashDeposit == true ? "7" : "6",
            };

            var bankDepositOnCreatingService = A.Fake<BankDepositOnCreatingService>(option => option.CallsBaseMethods());
            A.CallTo(() => bankDepositOnCreatingService.GetLoggedContact(entityPM.Tenant)).Returns(loggedcontact);
            A.CallTo(() => bankDepositOnCreatingService.GetCurrentDateTime(entityPM.Tenant)).Returns(currentDateTime);
            A.CallTo(() => bankDepositOnCreatingService.IdCounterWrapperGetNumber(entityPM.Tenant)).Returns(expectedParentIdCounter);
            A.CallTo(() => bankDepositOnCreatingService.CodeCounterWrapperGetNumber(entityPM.Tenant)).Returns(expectedCodeCounter);
            A.CallTo(() => bankDepositOnCreatingService.LogActivity(entityPM)).DoesNothing();

            // Act
            JournalPM newJournal = bankDepositOnCreatingService.InitJournal(entityPM);

            // Assert
            AssertHelper.HasEqualFieldValues(expectedJournal, newJournal, Environment.NewLine + "[TEST ERROR] Initiated Jounal does not match expected #DP04");

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

    
