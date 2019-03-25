using System;
using System.Collections.Generic;
using FakeItEasy;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools.Helpers;
using Logitude.UnitTest.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.UnitTest.Accounting.UniTests.BankAccountTests
{
    [TestClass]
    public class BankAccountTraceEventServiceUnitTest:TestBase
    {
        [TestMethod]
        public void Trace_InCaseCreate_CERV_EventCodeExpected()
        {
            // expected event code for creating a bank account
            string expectedEventCode = "CERV";

            // object to be created.
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

            BankAccount entity = new BankAccount()
            {
                AccountNumber = "123456",
                BranchNumber = "12",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
            };

            // entities called from DB
            EventType eventType = new EventType() 
            {
                Code = expectedEventCode,
                AddedManually = false,
                EnglishName = "Created",
                LocalName = "Created",
                Id = "1",
            };

            ObjectTable objectTable = new ObjectTable()
            {
                Id = "1",
            };

           

            string userId = null;
            // calling the trace service
            var bankAccountTraceEventService = A.Fake<BankAccountTraceEventService>(option => option.CallsBaseMethods());
            A.CallTo(() => bankAccountTraceEventService.GetEventType(A<string>.Ignored, A<int>.Ignored)).Returns(eventType);
            A.CallTo(() => bankAccountTraceEventService.GetObjectTable(A<string>.Ignored, A<int>.Ignored)).Returns(objectTable);
            A.CallTo(() => bankAccountTraceEventService.GetSystemUserCaseCustomerCare(out userId, A<string>.Ignored, A<int>.Ignored)).Returns("email");
            bankAccountTraceEventService.Trace(entityPM, entity, null);

            // check the traceevents list for a CERV
            TraceEventResponse traceEventResponse = bankAccountTraceEventService.TraceEventResponses.Where(d => d.EventTypeCode == expectedEventCode).FirstOrDefault();
            string traceEventCode = null;
            if (traceEventResponse != null)
            {
                traceEventCode = traceEventResponse.EventTypeCode;
            }

            //Assert
            Assert.AreEqual(expectedEventCode, traceEventCode);

        }

        [TestMethod]
        public void Trace_InCaseUpdate_CUPD_EventCodeExpected()
        {
            // expected event code for updating a bank account
            string expectedEventCode = "CUPD";

            // object to be created.
            BankAccountPM entityPM = new BankAccountPM()
            {
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
            };

            BankAccount entity = new BankAccount()
            {
                AccountNumber = "123456",
                BranchNumber = "12",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
            };

            // entities called from DB
            EventType eventType = new EventType()
            {
                Code = expectedEventCode,
                AddedManually = false,
                EnglishName = "Updated",
                LocalName = "Updated",
                Id = "1",
            };

            ObjectTable objectTable = new ObjectTable()
            {
                Id = "1",
            };

            // calling the trace service
            string userId = null;
            var bankAccountTraceEventService = A.Fake<BankAccountTraceEventService>(option => option.CallsBaseMethods());
            A.CallTo(() => bankAccountTraceEventService.GetEventType(A<string>.Ignored, A<int>.Ignored)).Returns(eventType);
            A.CallTo(() => bankAccountTraceEventService.GetObjectTable(A<string>.Ignored, A<int>.Ignored)).Returns(objectTable);
            A.CallTo(() => bankAccountTraceEventService.GetSystemUserCaseCustomerCare(out userId, A<string>.Ignored, A<int>.Ignored)).Returns("email");
            bankAccountTraceEventService.Trace(entityPM, entity, null);

            // check the traceevents list for a CERV
            TraceEventResponse traceEventResponse = bankAccountTraceEventService.TraceEventResponses.Where(d => d.EventTypeCode == expectedEventCode).FirstOrDefault();
            string traceEventCode = null;
            if (traceEventResponse != null)
            {
                traceEventCode = traceEventResponse.EventTypeCode;
            }

            //Assert
            Assert.AreEqual(expectedEventCode, traceEventCode);
        }

        [TestMethod]
        public void Trace_DeactivateBankAccount_TraceEventsCreated_DCTV()
        {
            // expected event code for deactivating a bank account
            string expectedEventCode = "DCTV";

            //Bank Account to be deactivated
            BankAccountPM bankAccountPM = new BankAccountPM()
            {
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
                Inactive = true,
            };

            BankAccount bankAccount = new BankAccount()
            {
                AccountNumber = "123456",
                BranchNumber = "12",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                Inactive = false,
            };

            // entities called from DB
            EventType eventType = new EventType()
            {
                Code = expectedEventCode,
                AddedManually = false,
                EnglishName = "Updated",
                LocalName = "Updated",
                Id = "1",
            };

            ObjectTable objectTable = new ObjectTable()
            {
                Id = "1",
            };

            // calling the trace service
            string userId = null;
            var bankAccountTraceEventService = A.Fake<BankAccountTraceEventService>(option => option.CallsBaseMethods());
            A.CallTo(() => bankAccountTraceEventService.GetEventType(A<string>.Ignored, A<int>.Ignored)).Returns(eventType);
            A.CallTo(() => bankAccountTraceEventService.GetObjectTable(A<string>.Ignored, A<int>.Ignored)).Returns(objectTable);
            A.CallTo(() => bankAccountTraceEventService.GetSystemUserCaseCustomerCare(out userId, A<string>.Ignored, A<int>.Ignored)).Returns("email");
            bankAccountTraceEventService.Trace(bankAccountPM, bankAccount, null);

            // check the traceevents list for a CERV
            TraceEventResponse traceEventResponse = bankAccountTraceEventService.TraceEventResponses.Where(d => d.EventTypeCode == expectedEventCode).FirstOrDefault();
            string traceEventCode = null;
            if (traceEventResponse != null)
            {
                traceEventCode = traceEventResponse.EventTypeCode;
            }

            //Assert
            Assert.AreEqual(expectedEventCode, traceEventCode);
        }

        [TestMethod]
        public void Trace_ActivateBankAccount_TraceEventsCreated_ACTV()
        {
            // expected event code for deactivating a bank account
            string expectedEventCode = "ACTV";

            //Bank Account to be deactivated
            BankAccountPM bankAccountPM = new BankAccountPM()
            {
                AccountNumber = "123456",
                BranchNumber = "12",
                BankCode = "10",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
                Inactive = false,
            };

            BankAccount bankAccount = new BankAccount()
            {
                AccountNumber = "123456",
                BranchNumber = "12",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
                Inactive = true,
            };

            // entities called from DB
            EventType eventType = new EventType()
            {
                Code = expectedEventCode,
                AddedManually = false,
                EnglishName = "Updated",
                LocalName = "Updated",
                Id = "1",
            };

            ObjectTable objectTable = new ObjectTable()
            {
                Id = "1",
            };

            // calling the trace service
            string userId = null;
            var bankAccountTraceEventService = A.Fake<BankAccountTraceEventService>(option => option.CallsBaseMethods());
            A.CallTo(() => bankAccountTraceEventService.GetEventType(A<string>.Ignored, A<int>.Ignored)).Returns(eventType);
            A.CallTo(() => bankAccountTraceEventService.GetObjectTable(A<string>.Ignored, A<int>.Ignored)).Returns(objectTable);
            A.CallTo(() => bankAccountTraceEventService.GetSystemUserCaseCustomerCare(out userId, A<string>.Ignored, A<int>.Ignored)).Returns("email");
            bankAccountTraceEventService.Trace(bankAccountPM, bankAccount, null);

            // check the traceevents list for a CERV
            TraceEventResponse traceEventResponse = bankAccountTraceEventService.TraceEventResponses.Where(d => d.EventTypeCode == expectedEventCode).FirstOrDefault();
            string traceEventCode = null;
            if (traceEventResponse != null)
            {
                traceEventCode = traceEventResponse.EventTypeCode;
            }

            //Assert
            Assert.AreEqual(expectedEventCode, traceEventCode);
        }

        [TestMethod]
        public void Trace_GetEventNotes_OldAndNewValuesForLocalNameEnglishNameBranchNumberAccountNumber()
        {
            // expected event code for updating a bank account
            string expectedEventCode = "CUPD";

            //Bank Account to be deactivated
            BankAccountPM bankAccountPM = new BankAccountPM()
            {
                AccountNumber = "1234567",
                BranchNumber = "123",
                BankCode = "10",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                LocalName="localName1",
                EnglishName="englishName1",
                Tenant = 1,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
            };

            BankAccount bankAccount = new BankAccount()
            {
                AccountNumber = "123456",
                BranchNumber = "12",
                LocalName = "localName",
                EnglishName = "englishName",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
            };

            // entities called from DB
            EventType eventType = new EventType()
            {
                Code = expectedEventCode,
                AddedManually = false,
                EnglishName = "Updated",
                LocalName = "Updated",
                Id = "1",
            };

            ObjectTable objectTable = new ObjectTable()
            {
                Id = "1",
            };

            // calling the trace service
            string userId = null;
            var bankAccountTraceEventService = A.Fake<BankAccountTraceEventService>(option => option.CallsBaseMethods());
            A.CallTo(() => bankAccountTraceEventService.GetEventType(A<string>.Ignored, A<int>.Ignored)).Returns(eventType);
            A.CallTo(() => bankAccountTraceEventService.GetObjectTable(A<string>.Ignored, A<int>.Ignored)).Returns(objectTable);
            A.CallTo(() => bankAccountTraceEventService.GetSystemUserCaseCustomerCare(out userId, A<string>.Ignored, A<int>.Ignored)).Returns("email");
            bankAccountTraceEventService.Trace(bankAccountPM, bankAccount, null);

            string accountNumberMessage = "Account Number Accounting.General.O.OldValue " + bankAccount.AccountNumber + " Accounting.General.O.NewValue " + bankAccountPM.AccountNumber;
            string localNameMessage = "Local Name Accounting.General.O.OldValue " + bankAccount.LocalName + " Accounting.General.O.NewValue " + bankAccountPM.LocalName;
            string engliahNameMessage = "English Name Accounting.General.O.OldValue " + bankAccount.EnglishName + " Accounting.General.O.NewValue " + bankAccountPM.EnglishName;
            string branchNumberMessage = "Branch Number Accounting.General.O.OldValue " + bankAccount.BranchNumber + " Accounting.General.O.NewValue " + bankAccountPM.BranchNumber;
            string allTheMessage = accountNumberMessage+Environment.NewLine+;
      //      Aggregate(
      //() => Assert.AreEqual(accountNumberMessage, bankAccountPM.InternalNumber, "Number not matches expected"),
      //() => Assert.AreEqual(expectedIdCounter, entityPM.Id, "Id not matches expected"),
      //  () => Assert.AreEqual(expectedLoggedUserId, entityPM.UpdatedByUserId, "Id not matches expected"));

        }

        public static List<Tuple<BankAccount, BankAccountPM, string>> GetEventNotesListOfState()
        {
            var listOfState = new List<Tuple<BankAccount, BankAccountPM, string>>();
            string accountNumberMessage = "Account Number Accounting.General.O.OldValue " + bankAccount.AccountNumber + " Accounting.General.O.NewValue " + bankAccountPM.AccountNumber;
            string localNameMessage = "Local Name Accounting.General.O.OldValue " + bankAccount.LocalName + " Accounting.General.O.NewValue " + bankAccountPM.LocalName;
            string engliahNameMessage = "English Name Accounting.General.O.OldValue " + bankAccount.EnglishName + " Accounting.General.O.NewValue " + bankAccountPM.EnglishName;
            string branchNumberMessage = "Branch Number Accounting.General.O.OldValue " + bankAccount.BranchNumber + " Accounting.General.O.NewValue " + bankAccountPM.BranchNumber;

            listOfState.Add(new BankAccount()
            {
                AccountNumber = "123456",
                BranchNumber = "12",
                LocalName = "localName",
                EnglishName = "englishName",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                Tenant = 1,
            }, new BankAccountPM()
            {
                AccountNumber = "1234567",
                BranchNumber = "123",
                BankCode = "10",
                BankId = "b1",
                GLAccountId = "GLA1",
                DeferredGLAccountId = "GLA2",
                LocalName = "localName1",
                EnglishName = "englishName1",
                Tenant = 1,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
            }, accountNumberMessage);
        }

        //[DataTestMethod] //failed to get params by attribute.
        //[DynamicData(nameof(GetActivateDeactivateParameters), DynamicDataSourceType.Method)]
        //public void Trace_ActivateDeactivateBankAccount_EventsCreated_ACTV_DCTV(List<Tuple<BankAccountPM, BankAccount, string>> listOfState)
        //{


        //    // entities called from DB
        //    EventType eventType = new EventType()
        //    {
        //        Code = "CUPD",
        //        AddedManually = false,
        //        EnglishName = "Updated",
        //        LocalName = "Updated",
        //        Id = "1",
        //    };

        //    ObjectTable objectTable = new ObjectTable()
        //    {
        //        Id = "1",
        //    };



        //    // calling the trace service
        //    string userId = null;
        //    var bankAccountTraceEventService = A.Fake<BankAccountTraceEventService>(option => option.CallsBaseMethods());
        //    A.CallTo(() => bankAccountTraceEventService.GetEventType(A<string>.Ignored, A<int>.Ignored)).Returns(eventType);
        //    A.CallTo(() => bankAccountTraceEventService.GetObjectTable(A<string>.Ignored, A<int>.Ignored)).Returns(objectTable);
        //    A.CallTo(() => bankAccountTraceEventService.GetSystemUserCaseCustomerCare(out userId, A<string>.Ignored, A<int>.Ignored)).Returns("email");
        //    //bankAccountTraceEventService.Trace(entityPM, entity, null);

        //    // check the traceevents list for a CERV
        //    TraceEventResponse traceEventResponse = bankAccountTraceEventService.TraceEventResponses.Where(d => d.EventTypeCode == "CUPD").FirstOrDefault();
        //    string traceEventCode = null;
        //    if (traceEventResponse != null)
        //    {
        //        traceEventCode = traceEventResponse.EventTypeCode;
        //    }

        //    //Assert
        //    Assert.AreEqual("CUPD", traceEventCode);
        //}

        //public static List<Tuple<BankAccountPM, BankAccount, string>> GetActivateDeactivateParameters()
        //{
        //    var listOfState = new List<Tuple<BankAccountPM, BankAccount, string>>();

        //    listOfState.Add(new Tuple<BankAccountPM, BankAccount, string>(new BankAccountPM()
        //    {
        //        AccountNumber = "123456",
        //        BranchNumber = "12",
        //        BankCode = "10",
        //        BankId = "b1",
        //        GLAccountId = "GLA1",
        //        DeferredGLAccountId = "GLA2",
        //        Tenant = 1,
        //        ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
        //        Inactive = true,
        //    }, new BankAccount()
        //    {
        //        AccountNumber = "123456",
        //        BranchNumber = "12",
        //        BankId = "b1",
        //        GLAccountId = "GLA1",
        //        DeferredGLAccountId = "GLA2",
        //        Tenant = 1,
        //        Inactive = false,
        //    }, "DCTV"));

        //    listOfState.Add(new Tuple<BankAccountPM, BankAccount, string>(new BankAccountPM()
        //    {
        //        AccountNumber = "123456",
        //        BranchNumber = "12",
        //        BankCode = "10",
        //        BankId = "b1",
        //        GLAccountId = "GLA1",
        //        DeferredGLAccountId = "GLA2",
        //        Tenant = 1,
        //        ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
        //        Inactive = false,
        //    }, new BankAccount()
        //    {
        //        AccountNumber = "123456",
        //        BranchNumber = "12",
        //        BankId = "b1",
        //        GLAccountId = "GLA1",
        //        DeferredGLAccountId = "GLA2",
        //        Tenant = 1,
        //        Inactive = true,
        //    }, "ACTV"));

        //    return listOfState;
        //}

    }
}
