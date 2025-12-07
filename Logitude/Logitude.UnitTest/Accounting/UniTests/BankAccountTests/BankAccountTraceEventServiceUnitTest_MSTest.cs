using System;
using System.Collections.Generic;
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
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.Test.Infrastructure;
using FakeItEasy;

namespace Logitude.UnitTest.Accounting.UniTests.BankAccountTests
{
    /// <summary>
    /// Refactored version of BankAccountTraceEventServiceUnitTest using FakeItEasy.
    /// Migrated from MSTest Fakes to FakeItEasy for compatibility with Visual Studio Community.
    /// </summary>
    [TestClass]
    public class BankAccountTraceEventServiceUnitTest_MSTest : TestBase_MSTest
    {
        [TestMethod]
        public void Trace_InCaseCreate_CERV_EventCodeExpected()
        {
            string expectedEventCode = "CERV";

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

            var bankAccountTraceEventService = A.Fake<BankAccountTraceEventService>(option => option.CallsBaseMethods());
            A.CallTo(() => bankAccountTraceEventService.GetEventType(A<string>.Ignored, A<int>.Ignored)).Returns(eventType);
            A.CallTo(() => bankAccountTraceEventService.GetObjectTable(A<string>.Ignored, A<int>.Ignored)).Returns(objectTable);
            A.CallTo(() => bankAccountTraceEventService.GetSystemUserCaseCustomerCare(out userId, A<string>.Ignored, A<int>.Ignored)).Returns("email");
            
            bankAccountTraceEventService.Trace(entityPM, entity, null);

            TraceEventResponse traceEventResponse = bankAccountTraceEventService.TraceEventResponses.Where(d => d.EventTypeCode == expectedEventCode).FirstOrDefault();
            string traceEventCode = null;
            if (traceEventResponse != null)
            {
                traceEventCode = traceEventResponse.EventTypeCode;
            }

            Assert.AreEqual(expectedEventCode, traceEventCode);
        }

        [TestMethod]
        public void Trace_InCaseUpdate_CUPD_EventCodeExpected()
        {
            string expectedEventCode = "CUPD";

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

            string userId = null;

            var bankAccountTraceEventService = A.Fake<BankAccountTraceEventService>(option => option.CallsBaseMethods());
            A.CallTo(() => bankAccountTraceEventService.GetEventType(A<string>.Ignored, A<int>.Ignored)).Returns(eventType);
            A.CallTo(() => bankAccountTraceEventService.GetObjectTable(A<string>.Ignored, A<int>.Ignored)).Returns(objectTable);
            A.CallTo(() => bankAccountTraceEventService.GetSystemUserCaseCustomerCare(out userId, A<string>.Ignored, A<int>.Ignored)).Returns("email");
            
            bankAccountTraceEventService.Trace(entityPM, entity, null);

            TraceEventResponse traceEventResponse = bankAccountTraceEventService.TraceEventResponses.Where(d => d.EventTypeCode == expectedEventCode).FirstOrDefault();
            string traceEventCode = null;
            if (traceEventResponse != null)
            {
                traceEventCode = traceEventResponse.EventTypeCode;
            }

            Assert.AreEqual(expectedEventCode, traceEventCode);
        }

        [TestMethod]
        public void Trace_DeactivateBankAccount_TraceEventsCreated_DCTV()
        {
            string expectedEventCode = "DCTV";

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

            string userId = null;

            var bankAccountTraceEventService = A.Fake<BankAccountTraceEventService>(option => option.CallsBaseMethods());
            A.CallTo(() => bankAccountTraceEventService.GetEventType(A<string>.Ignored, A<int>.Ignored)).Returns(eventType);
            A.CallTo(() => bankAccountTraceEventService.GetObjectTable(A<string>.Ignored, A<int>.Ignored)).Returns(objectTable);
            A.CallTo(() => bankAccountTraceEventService.GetSystemUserCaseCustomerCare(out userId, A<string>.Ignored, A<int>.Ignored)).Returns("email");
            
            bankAccountTraceEventService.Trace(bankAccountPM, bankAccount, null);

            TraceEventResponse traceEventResponse = bankAccountTraceEventService.TraceEventResponses.Where(d => d.EventTypeCode == expectedEventCode).FirstOrDefault();
            string traceEventCode = null;
            if (traceEventResponse != null)
            {
                traceEventCode = traceEventResponse.EventTypeCode;
            }

            Assert.AreEqual(expectedEventCode, traceEventCode);
        }

        [TestMethod]
        public void Trace_ActivateBankAccount_TraceEventsCreated_ACTV()
        {
            string expectedEventCode = "ACTV";

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

            string userId = null;

            var bankAccountTraceEventService = A.Fake<BankAccountTraceEventService>(option => option.CallsBaseMethods());
            A.CallTo(() => bankAccountTraceEventService.GetEventType(A<string>.Ignored, A<int>.Ignored)).Returns(eventType);
            A.CallTo(() => bankAccountTraceEventService.GetObjectTable(A<string>.Ignored, A<int>.Ignored)).Returns(objectTable);
            A.CallTo(() => bankAccountTraceEventService.GetSystemUserCaseCustomerCare(out userId, A<string>.Ignored, A<int>.Ignored)).Returns("email");
            
            bankAccountTraceEventService.Trace(bankAccountPM, bankAccount, null);

            TraceEventResponse traceEventResponse = bankAccountTraceEventService.TraceEventResponses.Where(d => d.EventTypeCode == expectedEventCode).FirstOrDefault();
            string traceEventCode = null;
            if (traceEventResponse != null)
            {
                traceEventCode = traceEventResponse.EventTypeCode;
            }

            Assert.AreEqual(expectedEventCode, traceEventCode);
        }

        [TestMethod]
        public void Trace_GetEventNotes_OldAndNewValuesForLocalNameEnglishNameBranchNumberAccountNumber()
        {
            string expectedEventCode = "CUPD";

            BankAccountPM bankAccountPM = new BankAccountPM()
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
            string allTheMessage = accountNumberMessage + Environment.NewLine;
        }
    }
}

