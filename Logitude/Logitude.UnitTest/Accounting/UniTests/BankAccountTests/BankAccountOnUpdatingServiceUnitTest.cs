using System;
using System.Collections.Generic;
using FakeItEasy;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Resolvers;
using Logitude.UnitTest.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.UnitTest.Accounting.UniTests.BankAccountTests
{
    [TestClass]
    public class BankAccountOnUpdatingServiceUnitTest:TestBase
    {
        [TestMethod]
        public void OnUpdating_UpdatedByUserIdMatchesExpected_Success()
        {
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(1);
            string expectedLoggedUserId = loggedcontact.Id;

            BankAccountPM entityPM = new BankAccountPM()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
            };

            BankAccount poco = new BankAccount()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankId = "b1",
                GLAccountId = "GLA11",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
            };

            var bankAccountOnUpdatingUpdateService = A.Fake<BankAccountOnUpdatingService>(option => option.CallsBaseMethods());
            //A.CallTo(() => bankAccountOnUpdatingUpdateService.GetLoggedContact(entityPM.Tenant)).Returns(loggedcontact);
            //A.CallTo(() => bankAccountOnUpdatingUpdateService.GetCurrentDateTime(entityPM.Tenant)).Returns(expectedDateTime);
            bankAccountOnUpdatingUpdateService.OnUpdating(entityPM, poco);
            Assert.AreEqual(expectedLoggedUserId, entityPM.UpdatedByUserId);

        }

        //[TestMethod]
        //public void OnUpdating_UpdateDateMatchesExpected_Success()
        //{
        //    ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(1);
        //    string expectedLoggedUserId = loggedcontact.Id;
           

        //    BankAccountPM entityPM = new BankAccountPM()
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

        //    BankAccount poco = new BankAccount()
        //    {
        //        Id = "1",
        //        AccountNumber = "123456",
        //        BranchNumber = "12",
        //        BankId = "b1",
        //        GLAccountId = "GLA11",
        //        DeferredGLAccountId = "GLA2",
        //        Tenant = 1,
        //        CreatedByUserId = expectedLoggedUserId,
        //    };

        //    var bankAccountOnUpdatingUpdateService = A.Fake<BankAccountOnUpdatingService>(option => option.CallsBaseMethods());
        //    A.CallTo(() => bankAccountOnUpdatingUpdateService.GetLoggedContact(entityPM.Tenant)).Returns(loggedcontact);
        //    A.CallTo(() => bankAccountOnUpdatingUpdateService.GetCurrentDateTime(entityPM.Tenant)).Returns(expectedDateTime);
        //    bankAccountOnUpdatingUpdateService.OnUpdating(entityPM, poco);
        //    Assert.AreEqual(expectedDateTime, entityPM.UpdateDate);

        //}

        [TestMethod]
        public void OnUpdating_CreatedByUserIdMatchesExpectedIfNull_Success()
        {
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(1);
            string expectedLoggedUserId = loggedcontact.Id;
            

            BankAccountPM entityPM = new BankAccountPM()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
            };

            BankAccount poco = new BankAccount()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankId = "b1",
                GLAccountId = "GLA11",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
            };

            var bankAccountOnUpdatingUpdateService = A.Fake<BankAccountOnUpdatingService>(option => option.CallsBaseMethods());
          
            bankAccountOnUpdatingUpdateService.OnUpdating(entityPM, poco);
            Assert.AreEqual(expectedLoggedUserId, entityPM.CreatedByUserId);

        }

        [TestMethod]
        public void OnUpdating_SearchFieldsMatchesExpected_Success()
        {
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(1);
            string expectedLoggedUserId = loggedcontact.Id;
            BankAccountPM entityPM = new BankAccountPM()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
            };

            BankAccount poco = new BankAccount()
            {
                Id = "1",
                AccountNumber = "123456",
                BranchNumber = "12",
                BankId = "b1",
                GLAccountId = "GLA11",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                CreatedByUserId = expectedLoggedUserId,
            };

            var expectedSearchFields = entityPM.AccountNumber + "," + entityPM.EnglishName + "," + entityPM.LocalName + "," + entityPM.BranchNumber;

            var bankAccountOnUpdatingUpdateService = A.Fake<BankAccountOnUpdatingService>(option => option.CallsBaseMethods());
          
            bankAccountOnUpdatingUpdateService.OnUpdating(entityPM, poco);
            Assert.AreEqual(expectedSearchFields, entityPM.SearchFields);

        }


    }
}
