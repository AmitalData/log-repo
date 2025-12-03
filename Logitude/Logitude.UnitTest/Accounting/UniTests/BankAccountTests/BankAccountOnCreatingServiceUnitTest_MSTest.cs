using System;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Helpers;
using Logitude.BL.Resolvers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logitude.Test.Infrastructure;
using FakeItEasy;

namespace Logitude.UnitTest.Accounting.UniTests.BankAccountTests
{
    /// <summary>
    /// Refactored version of BankAccountOnCreatingServiceUnitTest using FakeItEasy.
    /// Migrated from MSTest Fakes to FakeItEasy for compatibility with Visual Studio Community.
    /// </summary>
    [TestClass]
    public class BankAccountOnCreatingServiceUnitTest_MSTest : TestBase_MSTest
    {
        [TestMethod]
        public void OnCreating_IdMatchesExpected_Success()
        {
            string expectedIdCounter = "Id_For_Mock";
            BankAccountPM entityPM = new BankAccountPM()
            {
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
            };

            var bankAccountOnCreatingService = A.Fake<BankAccountOnCreatingService>(option => option.CallsBaseMethods());
            bankAccountOnCreatingService.OnCreating(entityPM);
            Assert.AreEqual(expectedIdCounter, entityPM.Id);
        }

        [TestMethod]
        public void OnCreating_UpdatedByUserIdMatchesExpected_Success()
        {
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(1);
            string expectedLoggedUserId = loggedcontact.Id;
            BankAccountPM entityPM = new BankAccountPM()
            {
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
            };

            var bankAccountOnCreatingService = A.Fake<BankAccountOnCreatingService>(option => option.CallsBaseMethods());
            bankAccountOnCreatingService.OnCreating(entityPM);
            Assert.AreEqual(entityPM.UpdatedByUserId, expectedLoggedUserId);
        }

        [TestMethod]
        public void OnCreating_CreatedByUserIdMatchesExpectedIfNull_Success()
        {
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(1);
            string expectedIdCounter = loggedcontact.Id;
            DateTime expectedDateTime = DateTime.Now;

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
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
            };

            var bankAccountOnCreatingService = A.Fake<BankAccountOnCreatingService>(option => option.Implements<IBankAccountOnCreatingService>());
            bankAccountOnCreatingService.OnCreating(entityPM);
            Assert.AreEqual(entityPM.CreatedByUserId, expectedIdCounter);
        }
    }
}

