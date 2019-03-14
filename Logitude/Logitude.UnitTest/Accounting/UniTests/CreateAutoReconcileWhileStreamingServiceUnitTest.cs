using System;
using System.Collections.Generic;
using FakeItEasy;
using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Logitude.UnitTest.Utils;

namespace Logitude.UnitTest.Accounting.UniTests
{
    [TestClass]
    public class CreateAutoReconcileWhileStreamingServiceUnitTest
    {

        [TestMethod]
        public void CreateAutoReconcileWhileStreaming100_GoodPartialExample_CheckSuccess()
        {


            IAccountingContext fakeIAccountingContext;
            fakeIAccountingContext = A.Fake<IAccountingContext>();

            var myCreateAutoReconcileWhileStreamingService = A.Fake<CreateAutoReconcileWhileStreamingService>(option => option.CallsBaseMethods());



            List<LedgerTransactionPM> myOrginalJournalTransaction, myNewLedgerTransactionPM;
            JournalStornoReconcileServiceUnitTest.GetExampleReconcile(out myOrginalJournalTransaction, out myNewLedgerTransactionPM);



            var myOrginalIds = myOrginalJournalTransaction.Select(r => r.Id).ToList();
            A.CallTo(() =>
                    myCreateAutoReconcileWhileStreamingService
                    .GetLedgerTransactionToReconcile(A<List<string>>.Ignored))
            .Returns(myOrginalJournalTransaction);

            A.CallTo(() =>
                    myCreateAutoReconcileWhileStreamingService.ValidateReconcile(A<ReconciliationPM>.Ignored))
                    .Returns(null);



            int lineJournalReconcile = 0;
            myOrginalJournalTransaction.ForEach(r => r.InReconcileProgress = true);
            var newJournal = new JournalPM()
            {
                UpdatedByUserId = "itzik",
                JournalReconciles = new List<JournalReconcilePM>() {
                        GetNextJournalReconcile(myOrginalJournalTransaction, ref lineJournalReconcile),
                        GetNextJournalReconcile(myOrginalJournalTransaction, ref lineJournalReconcile),
                        GetNextJournalReconcile(myOrginalJournalTransaction, ref lineJournalReconcile),
                    }
            };
            newJournal.JournalReconciles[1].ReconciliationAmount -= 1;
            myCreateAutoReconcileWhileStreamingService
                .MustInit(fakeIAccountingContext,
               newJournal,
                myNewLedgerTransactionPM);
            myCreateAutoReconcileWhileStreamingService.CreateAutoReconcileWhileStreaming();

            var reconciliationList = myCreateAutoReconcileWhileStreamingService.ReconciliationList;
            Assert.IsNotNull(reconciliationList);
            Assert.AreEqual(2, reconciliationList.Count);


            var reco = reconciliationList[0];
            Assert.AreEqual("AccountId1", reco.AccountId);
            Assert.AreEqual(Simplog.Server.Infrastructure.ChangeSetOperation.Insert, reco.ChangeSetOp);

            Assert.IsNotNull(reco.ReconciliationLines);
            Assert.AreEqual(4, reco.ReconciliationLines.Count);


            Assert.AreEqual("2", reco.ReconciliationLines[0].TransactionId);
            Assert.AreEqual(-100, reco.ReconciliationLines[0].ReconciliationAmount);
            Assert.AreEqual("usd", reco.ReconciliationLines[0].CurrencyId);
            Assert.AreEqual(Simplog.Server.Infrastructure.ChangeSetOperation.Insert, reco.ReconciliationLines[0].ChangeSetOp);


            Assert.AreEqual("6", reco.ReconciliationLines[1].TransactionId);
            Assert.AreEqual(-10, reco.ReconciliationLines[1].ReconciliationAmount);
            Assert.AreEqual("usd", reco.ReconciliationLines[1].CurrencyId);
            Assert.AreEqual(Simplog.Server.Infrastructure.ChangeSetOperation.Insert, reco.ReconciliationLines[1].ChangeSetOp);


            Assert.AreEqual("1", reco.ReconciliationLines[2].TransactionId);
            Assert.AreEqual(100, reco.ReconciliationLines[2].ReconciliationAmount);
            Assert.AreEqual("usd", reco.ReconciliationLines[2].CurrencyId);
            Assert.AreEqual(Simplog.Server.Infrastructure.ChangeSetOperation.Insert, reco.ReconciliationLines[2].ChangeSetOp);


            Assert.AreEqual("5", reco.ReconciliationLines[3].TransactionId);
            Assert.AreEqual(10, reco.ReconciliationLines[3].ReconciliationAmount);
            Assert.AreEqual("usd", reco.ReconciliationLines[3].CurrencyId);
            Assert.AreEqual(Simplog.Server.Infrastructure.ChangeSetOperation.Insert, reco.ReconciliationLines[2].ChangeSetOp);




            //var not4Insert = reco.ReconciliationLines.Where(r => r.ChangeSetOp != ).ToList();
            //Assert.AreEqual(0, not4Insert.Count);
            //not4Insert
        }



        [TestMethod]
        public void CreateAutoReconcileWhileStreaming000_GoodFullExample_CheckSuccess()
        {


            IAccountingContext fakeIAccountingContext;
            fakeIAccountingContext = A.Fake<IAccountingContext>();

            var myCreateAutoReconcileWhileStreamingService = A.Fake<CreateAutoReconcileWhileStreamingService>(option => option.CallsBaseMethods());



            List<LedgerTransactionPM> myOrginalJournalTransaction, myNewLedgerTransactionPM;
            JournalStornoReconcileServiceUnitTest.GetExampleReconcile(out myOrginalJournalTransaction, out myNewLedgerTransactionPM);



            var myOrginalIds = myOrginalJournalTransaction.Select(r => r.Id).ToList();
            A.CallTo(() =>
                    myCreateAutoReconcileWhileStreamingService
                    .GetLedgerTransactionToReconcile(A<List<string>>.Ignored))
            .Returns(myOrginalJournalTransaction);

            A.CallTo(() =>
                    myCreateAutoReconcileWhileStreamingService.ValidateReconcile(A<ReconciliationPM>.Ignored))
                    .Returns(null);



            int lineJournalReconcile = 0;
            myOrginalJournalTransaction.ForEach(r => r.InReconcileProgress = true);
            myCreateAutoReconcileWhileStreamingService
                .MustInit(fakeIAccountingContext,
                new JournalPM()
                {
                    UpdatedByUserId = "itzik",
                    JournalReconciles = new List<JournalReconcilePM>() {
                        GetNextJournalReconcile(myOrginalJournalTransaction, ref lineJournalReconcile),
                        GetNextJournalReconcile(myOrginalJournalTransaction, ref lineJournalReconcile),
                        GetNextJournalReconcile(myOrginalJournalTransaction, ref lineJournalReconcile),
                    }
                },
                myNewLedgerTransactionPM);
            myCreateAutoReconcileWhileStreamingService.CreateAutoReconcileWhileStreaming();

            var reconciliationList = myCreateAutoReconcileWhileStreamingService.ReconciliationList;
            Assert.IsNotNull(reconciliationList);
            Assert.AreEqual(2, reconciliationList.Count);


            var reco = reconciliationList[0];
            Assert.AreEqual("AccountId1", reco.AccountId);
            Assert.AreEqual(Simplog.Server.Infrastructure.ChangeSetOperation.Insert, reco.ChangeSetOp);

            Assert.IsNotNull(reco.ReconciliationLines);
            Assert.AreEqual(4, reco.ReconciliationLines.Count);


            Assert.AreEqual("2", reco.ReconciliationLines[0].TransactionId);
            Assert.AreEqual(-100, reco.ReconciliationLines[0].ReconciliationAmount);
            Assert.AreEqual("usd", reco.ReconciliationLines[0].CurrencyId);
            Assert.AreEqual(Simplog.Server.Infrastructure.ChangeSetOperation.Insert, reco.ReconciliationLines[0].ChangeSetOp);


            Assert.AreEqual("6", reco.ReconciliationLines[1].TransactionId);
            Assert.AreEqual(-10, reco.ReconciliationLines[1].ReconciliationAmount);
            Assert.AreEqual("usd", reco.ReconciliationLines[1].CurrencyId);
            Assert.AreEqual(Simplog.Server.Infrastructure.ChangeSetOperation.Insert, reco.ReconciliationLines[1].ChangeSetOp);


            Assert.AreEqual("1", reco.ReconciliationLines[2].TransactionId);
            Assert.AreEqual(100, reco.ReconciliationLines[2].ReconciliationAmount);
            Assert.AreEqual("usd", reco.ReconciliationLines[2].CurrencyId);
            Assert.AreEqual(Simplog.Server.Infrastructure.ChangeSetOperation.Insert, reco.ReconciliationLines[2].ChangeSetOp);


            Assert.AreEqual("5", reco.ReconciliationLines[3].TransactionId);
            Assert.AreEqual(10, reco.ReconciliationLines[3].ReconciliationAmount);
            Assert.AreEqual("usd", reco.ReconciliationLines[3].CurrencyId);
            Assert.AreEqual(Simplog.Server.Infrastructure.ChangeSetOperation.Insert, reco.ReconciliationLines[2].ChangeSetOp);




            //var not4Insert = reco.ReconciliationLines.Where(r => r.ChangeSetOp != ).ToList();
            //Assert.AreEqual(0, not4Insert.Count);
            //not4Insert
        }


        


        [TestMethod]
        public void CreateAutoReconcileWhileStreaming001_NoJournalReconcile_ReturnZeroReconcilation()
        {


            IAccountingContext fakeIAccountingContext;
            fakeIAccountingContext = A.Fake<IAccountingContext>();

            var myCreateAutoReconcileWhileStreamingService = A.Fake<CreateAutoReconcileWhileStreamingService>(option => option.CallsBaseMethods());



            List<LedgerTransactionPM> myOrginalJournalTransaction, myNewLedgerTransactionPM;
            JournalStornoReconcileServiceUnitTest.GetExampleReconcile(out myOrginalJournalTransaction, out myNewLedgerTransactionPM);



            var myOrginalIds = myOrginalJournalTransaction.Select(r => r.Id).ToList();
            A.CallTo(() =>
                    myCreateAutoReconcileWhileStreamingService
                    .GetLedgerTransactionToReconcile(A<List<string>>.Ignored))
            .Returns(myOrginalJournalTransaction);

            A.CallTo(() =>
                    myCreateAutoReconcileWhileStreamingService.ValidateReconcile(A<ReconciliationPM>.Ignored))
                    .Returns(null);



            int lineJournalReconcile = 0;
            myOrginalJournalTransaction.ForEach(r => r.InReconcileProgress = true);
            myCreateAutoReconcileWhileStreamingService
                .MustInit(fakeIAccountingContext,
                new JournalPM()
                {
                    UpdatedByUserId = "itzik",
                   
                },
                myNewLedgerTransactionPM);
            myCreateAutoReconcileWhileStreamingService.CreateAutoReconcileWhileStreaming();

            var reconciliationList = myCreateAutoReconcileWhileStreamingService.ReconciliationList;
            Assert.IsNotNull(reconciliationList);
            Assert.AreEqual(0,reconciliationList.Count);




            //var not4Insert = reco.ReconciliationLines.Where(r => r.ChangeSetOp != ).ToList();
            //Assert.AreEqual(0, not4Insert.Count);
            //not4Insert
        }



        [TestMethod]
        public void CreateAutoReconcileWhileStreaming002_witoutInReconcileProgress_ThrowExecption()
        {


            IAccountingContext fakeIAccountingContext;
            fakeIAccountingContext = A.Fake<IAccountingContext>();

            var myCreateAutoReconcileWhileStreamingService = A.Fake<CreateAutoReconcileWhileStreamingService>(option => option.CallsBaseMethods());



            List<LedgerTransactionPM> myOrginalJournalTransaction, myNewLedgerTransactionPM;
            JournalStornoReconcileServiceUnitTest.GetExampleReconcile(out myOrginalJournalTransaction, out myNewLedgerTransactionPM);



            var myOrginalIds = myOrginalJournalTransaction.Select(r => r.Id).ToList();
            A.CallTo(() =>
                    myCreateAutoReconcileWhileStreamingService
                    .GetLedgerTransactionToReconcile(A<List<string>>.Ignored))
            .Returns(myOrginalJournalTransaction);

            A.CallTo(() =>
                    myCreateAutoReconcileWhileStreamingService.ValidateReconcile(A<ReconciliationPM>.Ignored))
                    .Returns(null);



            int lineJournalReconcile = 0;
            ///myOrginalJournalTransaction.ForEach(r => r.InReconcileProgress = true);
            myCreateAutoReconcileWhileStreamingService
                .MustInit(fakeIAccountingContext,
                new JournalPM()
                {
                    UpdatedByUserId = "itzik",
                    JournalReconciles = new List<JournalReconcilePM>() {
                        GetNextJournalReconcile(myOrginalJournalTransaction, ref lineJournalReconcile),
                        GetNextJournalReconcile(myOrginalJournalTransaction, ref lineJournalReconcile),
                        GetNextJournalReconcile(myOrginalJournalTransaction, ref lineJournalReconcile),
                    }
                },
                myNewLedgerTransactionPM);



            TestsUtil.AssertThrows<Exception>(delegate
            {
                // Arrange
                myCreateAutoReconcileWhileStreamingService.CreateAutoReconcileWhileStreaming();
            },
                // Assert: Verify the result:
                "_JournalPM.JournalReconciles have  myOldTransToReconcile.Any( r=> !r.InReconcileProgress)");
        }



        [TestMethod]
        public void CreateAutoReconcileWhileStreaming003_OpenAmountInOriginalTransNotEqual_ThrowExecption()
        {


            IAccountingContext fakeIAccountingContext;
            fakeIAccountingContext = A.Fake<IAccountingContext>();

            var myCreateAutoReconcileWhileStreamingService = A.Fake<CreateAutoReconcileWhileStreamingService>(option => option.CallsBaseMethods());



            List<LedgerTransactionPM> myOrginalJournalTransaction, myNewLedgerTransactionPM;
            JournalStornoReconcileServiceUnitTest.GetExampleReconcile(out myOrginalJournalTransaction, out myNewLedgerTransactionPM);



            var myOrginalIds = myOrginalJournalTransaction.Select(r => r.Id).ToList();
            A.CallTo(() =>
                    myCreateAutoReconcileWhileStreamingService
                    .GetLedgerTransactionToReconcile(A<List<string>>.Ignored))
            .Returns(myOrginalJournalTransaction);

            A.CallTo(() =>
                    myCreateAutoReconcileWhileStreamingService.ValidateReconcile(A<ReconciliationPM>.Ignored))
                    .Returns(null);



            int lineJournalReconcile = 0;
            myOrginalJournalTransaction.First().OpenAmount += 1;
            myOrginalJournalTransaction.ForEach(r => r.InReconcileProgress = true);
            myCreateAutoReconcileWhileStreamingService
                .MustInit(fakeIAccountingContext,
                new JournalPM()
                {
                    UpdatedByUserId = "itzik",
                    JournalReconciles = new List<JournalReconcilePM>() {
                        GetNextJournalReconcile(myOrginalJournalTransaction, ref lineJournalReconcile),
                        GetNextJournalReconcile(myOrginalJournalTransaction, ref lineJournalReconcile),
                        GetNextJournalReconcile(myOrginalJournalTransaction, ref lineJournalReconcile),
                    }
                },
                myNewLedgerTransactionPM);

            TestsUtil.AssertThrows<Exception>(delegate
            {
                // Arrange
                myCreateAutoReconcileWhileStreamingService.CreateAutoReconcileWhileStreaming();
            },
                // Assert: Verify the result:
                "(totNew != totReconciliationAmount) ");

        }

        private JournalReconcilePM GetNextJournalReconcile(List<LedgerTransactionPM> myOrginalJournalTransaction, ref int lineJournalReconcile)
        {

            var curr = myOrginalJournalTransaction[lineJournalReconcile];

            return new JournalReconcilePM()
            {
                Line = lineJournalReconcile++,
                ReconciliationAmount = curr.OpenAmount,
                LedgerTransactionId = curr.Id,
                CurrencyId = curr.OpenAmountCurrencyId,
                IsPartial = false,
                JournalId = curr.JournalId,
                Tenant = curr.Tenant,


            };
        }
    }
}
