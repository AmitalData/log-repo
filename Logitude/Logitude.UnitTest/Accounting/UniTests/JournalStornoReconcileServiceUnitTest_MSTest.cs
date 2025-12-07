using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.Def.EntityPMs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Logitude.Test.Infrastructure;

namespace Logitude.UnitTest.Accounting.UniTests
{
    /// <summary>
    /// MSTest-compatible version of JournalStornoReconcileServiceUnitTest.
    /// Migrated to MSTest - no mocking required, simple service test.
    /// </summary>
    [TestClass]
    public class JournalStornoReconcileServiceUnitTest_MSTest : TestBase_MSTest
    {
        [TestMethod]
        public void CreateReconciliationList_()
        {
            List<LedgerTransactionPM> myOrginalJournalTransaction, myStornoLedgerTransactionPM;
            GetExampleReconcile(out myOrginalJournalTransaction, out myStornoLedgerTransactionPM);

            var myJournalStornoReconcileService = new JournalStornoReconcileService();
            var res = myJournalStornoReconcileService.CreateReconciliationList(myOrginalJournalTransaction, myStornoLedgerTransactionPM);
            Assert.IsNotNull(res);
            Assert.AreEqual(2, res.Count);

            var currReconcile = res[0];
            Assert.IsNotNull(currReconcile);
            Assert.AreEqual("new", currReconcile.Id);
            Assert.AreEqual(false, currReconcile.IsCancelled);
            Assert.AreEqual(Simplog.Server.Infrastructure.ChangeSetOperation.Insert, currReconcile.ChangeSetOp);
            Assert.AreEqual("AccountId1", currReconcile.AccountId);
            Assert.AreEqual(1, currReconcile.Tenant);
            Assert.IsNotNull(currReconcile.ReconciliationLines);
            Assert.AreEqual(4, currReconcile.ReconciliationLines.Count);

            var currReconciliationLine = currReconcile.ReconciliationLines[0];
            Assert.IsNotNull(currReconciliationLine);
            Assert.AreEqual(Simplog.Server.Infrastructure.ChangeSetOperation.Insert, currReconciliationLine.ChangeSetOp);
            Assert.AreEqual("usd", currReconciliationLine.CurrencyId);
            Assert.AreEqual("new", currReconciliationLine.ReconciliationId);
            Assert.AreEqual(1, currReconciliationLine.Tenant);
            Assert.AreEqual("1", currReconciliationLine.TransactionId);
            Assert.AreEqual(0, currReconciliationLine.Line);
            Assert.AreEqual(100, currReconciliationLine.ReconciliationAmount);
            Assert.AreEqual(1, currReconciliationLine.GroupNumber);

            currReconciliationLine = currReconcile.ReconciliationLines[1];
            Assert.AreEqual(Simplog.Server.Infrastructure.ChangeSetOperation.Insert, currReconciliationLine.ChangeSetOp);
            Assert.AreEqual("usd", currReconciliationLine.CurrencyId);
            Assert.AreEqual("new", currReconciliationLine.ReconciliationId);
            Assert.AreEqual(1, currReconciliationLine.Tenant);
            Assert.AreEqual("2", currReconciliationLine.TransactionId);
            Assert.AreEqual(1, currReconciliationLine.Line);
            Assert.AreEqual(-100, currReconciliationLine.ReconciliationAmount);
            Assert.AreEqual(1, currReconciliationLine.GroupNumber);

            currReconciliationLine = currReconcile.ReconciliationLines[2];
            Assert.IsNotNull(currReconciliationLine);
            Assert.AreEqual(Simplog.Server.Infrastructure.ChangeSetOperation.Insert, currReconciliationLine.ChangeSetOp);
            Assert.AreEqual("usd", currReconciliationLine.CurrencyId);
            Assert.AreEqual("new", currReconciliationLine.ReconciliationId);
            Assert.AreEqual(1, currReconciliationLine.Tenant);
            Assert.AreEqual("5", currReconciliationLine.TransactionId);
            Assert.AreEqual(2, currReconciliationLine.Line);
            Assert.AreEqual(10, currReconciliationLine.ReconciliationAmount);
            Assert.AreEqual(1, currReconciliationLine.GroupNumber);

            currReconciliationLine = currReconcile.ReconciliationLines[3];
            Assert.AreEqual(Simplog.Server.Infrastructure.ChangeSetOperation.Insert, currReconciliationLine.ChangeSetOp);
            Assert.AreEqual("usd", currReconciliationLine.CurrencyId);
            Assert.AreEqual("new", currReconciliationLine.ReconciliationId);
            Assert.AreEqual(1, currReconciliationLine.Tenant);
            Assert.AreEqual("6", currReconciliationLine.TransactionId);
            Assert.AreEqual(3, currReconciliationLine.Line);
            Assert.AreEqual(-10, currReconciliationLine.ReconciliationAmount);
            Assert.AreEqual(1, currReconciliationLine.GroupNumber);

            currReconcile = res[1];
            Assert.IsNotNull(currReconcile);
            Assert.AreEqual("new", currReconcile.Id);
            Assert.AreEqual(false, currReconcile.IsCancelled);
            Assert.AreEqual(Simplog.Server.Infrastructure.ChangeSetOperation.Insert, currReconcile.ChangeSetOp);
            Assert.AreEqual("AccountId2", currReconcile.AccountId);
            Assert.AreEqual(1, currReconcile.Tenant);
            Assert.IsNotNull(currReconcile.ReconciliationLines);
            Assert.AreEqual(2, currReconcile.ReconciliationLines.Count);

            currReconciliationLine = currReconcile.ReconciliationLines[0];
            Assert.IsNotNull(currReconciliationLine);
            Assert.AreEqual(Simplog.Server.Infrastructure.ChangeSetOperation.Insert, currReconciliationLine.ChangeSetOp);
            Assert.AreEqual("nis", currReconciliationLine.CurrencyId);
            Assert.AreEqual("new", currReconciliationLine.ReconciliationId);
            Assert.AreEqual(1, currReconciliationLine.Tenant);
            Assert.AreEqual("3", currReconciliationLine.TransactionId);
            Assert.AreEqual(0, currReconciliationLine.Line);
            Assert.AreEqual(23.01m, currReconciliationLine.ReconciliationAmount);
            Assert.AreEqual(1, currReconciliationLine.GroupNumber);

            currReconciliationLine = currReconcile.ReconciliationLines[1];
            Assert.AreEqual(Simplog.Server.Infrastructure.ChangeSetOperation.Insert, currReconciliationLine.ChangeSetOp);
            Assert.AreEqual("nis", currReconciliationLine.CurrencyId);
            Assert.AreEqual("new", currReconciliationLine.ReconciliationId);
            Assert.AreEqual(1, currReconciliationLine.Tenant);
            Assert.AreEqual("4", currReconciliationLine.TransactionId);
            Assert.AreEqual(1, currReconciliationLine.Line);
            Assert.AreEqual(-23.01m, currReconciliationLine.ReconciliationAmount);
            Assert.AreEqual(1, currReconciliationLine.GroupNumber);
        }

        private static void GetExampleReconcile(out List<LedgerTransactionPM> myOrginalJournalTransaction, out List<LedgerTransactionPM> myStornoLedgerTransactionPM_newTrans)
        {
            int myid = 1;
            int myJournalLineNumber = 1;
            myOrginalJournalTransaction = new List<LedgerTransactionPM>();
            myStornoLedgerTransactionPM_newTrans = new List<LedgerTransactionPM>();
            myOrginalJournalTransaction.Add(new LedgerTransactionPM()
            {
                Tenant = 1,
                Id = (myid++).ToString(),
                JournalId = "1",
                JournalLineNumber = myJournalLineNumber,
                AccountId = "AccountId1",
                OpenAmount = 100,
                OpenAmountCurrencyId = "usd",
                IsReconciled = false,
            });
            myStornoLedgerTransactionPM_newTrans.Add(new LedgerTransactionPM()
            {
                Tenant = 1,
                Id = (myid++).ToString(),
                JournalId = "-1",
                JournalLineNumber = myJournalLineNumber,
                AccountId = "AccountId1",
                OpenAmount = -100,
                OpenAmountCurrencyId = "usd",
                IsReconciled = false,
            });

            myJournalLineNumber++;

            myOrginalJournalTransaction.Add(new LedgerTransactionPM()
            {
                Tenant = 1,
                Id = (myid++).ToString(),
                JournalId = "2",
                JournalLineNumber = myJournalLineNumber,
                AccountId = "AccountId2",
                OpenAmount = 23.01m,
                OpenAmountCurrencyId = "nis",
                IsReconciled = false,
            });
            myStornoLedgerTransactionPM_newTrans.Add(new LedgerTransactionPM()
            {
                Tenant = 1,
                Id = (myid++).ToString(),
                JournalId = "-2",
                JournalLineNumber = myJournalLineNumber,
                AccountId = "AccountId2",
                OpenAmount = -23.01m,
                OpenAmountCurrencyId = "nis",
                IsReconciled = false,
            });

            myJournalLineNumber++;

            myOrginalJournalTransaction.Add(new LedgerTransactionPM()
            {
                Tenant = 1,
                Id = (myid++).ToString(),
                JournalId = "1",
                JournalLineNumber = myJournalLineNumber,
                AccountId = "AccountId1",
                OpenAmount = 10,
                OpenAmountCurrencyId = "usd",
                IsReconciled = false,
            });
            myStornoLedgerTransactionPM_newTrans.Add(new LedgerTransactionPM()
            {
                Tenant = 1,
                Id = (myid++).ToString(),
                JournalId = "-1",
                JournalLineNumber = myJournalLineNumber,
                AccountId = "AccountId1",
                OpenAmount = -10,
                OpenAmountCurrencyId = "usd",
                IsReconciled = false,
            });
        }
    }
}

