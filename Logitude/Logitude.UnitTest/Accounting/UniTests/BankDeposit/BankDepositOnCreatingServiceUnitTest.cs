using System;
using System.Collections.Generic;
using FakeItEasy;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
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

            // Act
            bankAccountOnCreatingService.OnCreating(entityPM);

            // Assert
            var actualLineDepositId = entityPM.BankDepositLines[0].DepositId;
            Assert.AreEqual(expectedParentIdCounter, actualLineDepositId, "Deposit line does not take parent Id #DP03");

        }


        //[TestMethod]
        //public void OnCreating_UpdatedByUserIdMatchesExpected_Success()
        //{
        //    ContactPM loggedcontact = GetLoggedContactInstance();
        //    string expectedLoggedUserId = loggedcontact.Id;
        //    DateTime expectedDateTime = DateTime.Now;
        //    string expectedIdCounter = "myNewIdCounter";
        //    BankDepositPM entityPM = new BankDepositPM()
        //    {
        //        AccountNumber = "123456",
        //        BranchNumber = "12",
        //        BankCode = "10",
        //        BankId = "b1",
        //        GLAccountId = "GLA1",
        //        DeferredGLAccountId = "GLA2",
        //        Tenant = 1,
        //        ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
        //    };


        //    var bankAccountOnCreatingService = A.Fake<BankDepositOnCreatingService>(option => option.CallsBaseMethods());
        //    A.CallTo(() => bankAccountOnCreatingService.GetLoggedContact(entityPM.Tenant)).Returns(loggedcontact);
        //    A.CallTo(() => bankAccountOnCreatingService.GetCurrentDateTime(entityPM.Tenant)).Returns(expectedDateTime);
        //    A.CallTo(() => bankAccountOnCreatingService.IdCounterWrapperGetNumber(entityPM.Tenant)).Returns(expectedIdCounter);
        //    bankAccountOnCreatingService.OnCreating(entityPM);
        //    Assert.AreEqual(entityPM.UpdatedByUserId, expectedLoggedUserId);

        //}

        //[TestMethod]
        //public void OnCreating_CreatedByUserIdMatchesExpectedIfNull_Success()
        //{
        //    ContactPM loggedcontact = GetLoggedContactInstance();
        //    string expectedIdCounter = loggedcontact.Id;
        //    DateTime expectedDateTime = DateTime.Now;

        //    BankDepositPM entityPM = new BankDepositPM()
        //    {
        //        Id = "1",
        //        AccountNumber = "123456",
        //        BranchNumber = "12",
        //        BankCode = "10",
        //        BankId = "b1",
        //        GLAccountId = "GLA1",
        //        DeferredGLAccountId = "GLA2",
        //        Tenant = 1,
        //        ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
        //    };

        //    var bankAccountOnCreatingService = A.Fake<BankDepositOnCreatingService>(option => option.CallsBaseMethods());
        //    A.CallTo(() => bankAccountOnCreatingService.GetLoggedContact(entityPM.Tenant)).Returns(loggedcontact);
        //    A.CallTo(() => bankAccountOnCreatingService.GetCurrentDateTime(entityPM.Tenant)).Returns(expectedDateTime);
        //    A.CallTo(() => bankAccountOnCreatingService.IdCounterWrapperGetNumber(entityPM.Tenant)).Returns(expectedIdCounter);
        //    bankAccountOnCreatingService.OnCreating(entityPM);
        //    Assert.AreEqual(entityPM.CreatedByUserId, expectedIdCounter);

        //}

        //[TestMethod]

        //public void OnCreating_SearchFieldsMatchesExpected_Success()
        //{
        //    ContactPM loggedcontact = GetLoggedContactInstance();
        //    string expectedLoggedUserId = loggedcontact.Id;
        //    DateTime expectedDateTime = DateTime.Now;

        //    BankDepositPM entityPM = new BankDepositPM()
        //    {
        //        Id = "1",
        //        AccountNumber = "123456",
        //        BranchNumber = "12",
        //        BankCode = "10",
        //        BankId = "b1",
        //        GLAccountId = "GLA1",
        //        DeferredGLAccountId = "GLA2",
        //        Tenant = 1,
        //        CreatedByUserId = expectedLoggedUserId,
        //        ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
        //    };

        //    var expectedSearchFields = entityPM.AccountNumber + "," + entityPM.EnglishName + "," + entityPM.LocalName + "," + entityPM.BranchNumber;

        //    var bankAccountOnCreatingService = A.Fake<BankDepositOnCreatingService>(option => option.CallsBaseMethods());
        //    A.CallTo(() => bankAccountOnCreatingService.GetLoggedContact(entityPM.Tenant)).Returns(loggedcontact);
        //    A.CallTo(() => bankAccountOnCreatingService.GetCurrentDateTime(entityPM.Tenant)).Returns(expectedDateTime);
        //    bankAccountOnCreatingService.OnCreating(entityPM);

        //    Assert.AreEqual(entityPM.SearchFields, expectedSearchFields);

        //}

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
