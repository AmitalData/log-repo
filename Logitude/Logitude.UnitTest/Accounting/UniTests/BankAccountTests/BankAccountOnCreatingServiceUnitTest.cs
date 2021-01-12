using System;
using FakeItEasy;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Helpers;
using Logitude.BL.Resolvers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.UnitTest.Accounting.UniTests.BankAccountTests
{
    [TestClass]
    public class BankAccountOnCreatingServiceUnitTest:TestBase
    {

        [TestMethod]
        public void OnCreating_IdMatchesExpected_Success()
        {
            //ContactPM loggedcontact = GetLoggedContactInstance();
            //string expectedLoggedUserId = loggedcontact.Id;
            //DateTime expectedDateTime = DateTime.Now;
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
            //A.CallTo(() => bankAccountOnCreatingService.GetLoggedContact(entityPM.Tenant)).Returns(loggedcontact);
           // A.CallTo(() => bankAccountOnCreatingService.GetCurrentDateTime(entityPM.Tenant)).Returns(expectedDateTime);
           // A.CallTo(() => bankAccountOnCreatingService.IdCounterWrapperGetNumber(entityPM.Tenant)).Returns(expectedIdCounter);
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
            //A.CallTo(() => bankAccountOnCreatingService.GetLoggedContact(entityPM.Tenant)).Returns(loggedcontact);
            //A.CallTo(() => bankAccountOnCreatingService.GetCurrentDateTime(entityPM.Tenant)).Returns(expectedDateTime);
            //A.CallTo(() => bankAccountOnCreatingService.IdCounterWrapperGetNumber(entityPM.Tenant)).Returns(expectedIdCounter);
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
            //A.CallTo(() => bankAccountOnCreatingService.GetLoggedContact(entityPM.Tenant)).Returns(loggedcontact);
            //A.CallTo(() => bankAccountOnCreatingService.GetCurrentDateTime(entityPM.Tenant)).Returns(expectedDateTime);
            //A.CallTo(() => bankAccountOnCreatingService.IdCounterWrapperGetNumber(entityPM.Tenant)).Returns(expectedIdCounter);
            bankAccountOnCreatingService.OnCreating(entityPM);
            Assert.AreEqual(entityPM.CreatedByUserId, expectedIdCounter);

        }

        //[TestMethod]
       
        //public void OnCreating_SearchFieldsMatchesExpected_Success()
        //{
           
        //    DateTime expectedDateTime = DateTime.Now;

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
        //        ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
        //    };

        //    var expectedSearchFields = entityPM.AccountNumber + "," + entityPM.EnglishName + "," + entityPM.LocalName + "," + entityPM.BranchNumber;

        //    var bankAccountOnCreatingService = A.Fake<BankAccountOnCreatingService>(option => option.CallsBaseMethods());
        //    //A.CallTo(() => bankAccountOnCreatingService.GetLoggedContact(entityPM.Tenant)).Returns(loggedcontact);
        //    A.CallTo(() => bankAccountOnCreatingService.GetCurrentDateTime(entityPM.Tenant)).Returns(expectedDateTime);
        //    bankAccountOnCreatingService.OnCreating(entityPM);

        //    Assert.AreEqual(entityPM.SearchFields, expectedSearchFields);

        //}

      
    }
}
