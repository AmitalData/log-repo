using System;
using System.Linq;
using System.Collections.Generic;
using FakeItEasy;
using Logitude.Accounting.BL.CoreBL.ExternalReconcile;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityLists;
using Logitude.BL.DataContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logitude.Accounting.Def.EntityPMs;
namespace Logitude.UnitTest.Accounting.UniTests.ExternalReconcile
{
    [TestClass]
    public class ExternalReconcileAdjustBankFeesServiceUnitTest
    {
        private int _Tenant;
        private string _AdjustGLAccountId;
        private string _screenNotes;
        private string _accountingCurrencyId;
        private string _BankCurrencyId;
        private string _BankAccountId;
        private string _GLAccountId;
        private DateTime _ReferenceDate;

        [TestMethod]
        public void CreateJournalWithExtReconcile01_PageLineinCreditAmount_OK()
        {
            bool allreconcileExternalPageLineListinDebit = false;
            checkCreateJournalWithExtReconcilePerExample(allreconcileExternalPageLineListinDebit);
        }
        [TestMethod]
        public void CreateJournalWithExtReconcile02_PageLineinDebitAmount_OK()
        {
            bool allreconcileExternalPageLineListinDebit = true;
            checkCreateJournalWithExtReconcilePerExample(allreconcileExternalPageLineListinDebit);
        }

        [TestMethod]
        public void CreateAutoExternalReconcileWhileStreaming()
        {
            bool allreconcileExternalPageLineListinDebit = true;
            List<ReconcileExternalPageLineList> reconcileExternalPageLineLists;
            LastRate lastRate;
            ExternalReconcileDataProvider fakeExternalReconcileDataProvider =
            PrepareContext(out reconcileExternalPageLineLists, out lastRate, allreconcileExternalPageLineListinDebit);
            IExternalReconcileAdjustBankFeesService myExternalReconcileAdjustBankFeesService = new ExternalReconcileAdjustBankFeesService();
            myExternalReconcileAdjustBankFeesService.MustInit(fakeExternalReconcileDataProvider);
            var reconcileExternalPageLineIdList = reconcileExternalPageLineLists.Select(r => r.Id).ToList();
            
            myExternalReconcileAdjustBankFeesService.CreateJournalWithExtReconcile(_Tenant, reconcileExternalPageLineIdList, _AdjustGLAccountId, _screenNotes,new DateTime(2019,1,4));

            string myNewLedgerTransactionId = "myNewLedgerTransactionId";
            var newJournal = myExternalReconcileAdjustBankFeesService.TheNewJournal;
            var myNewLedgerTransactionList = new List<LedgerTransactionPM>()
            {
                new LedgerTransactionPM()
                {
                    Id=myNewLedgerTransactionId,
                     AccountId= _BankAccountId,

                },
            };
            reconcileExternalPageLineLists.ForEach(r =>
            {
                r.InProgressExternalReconcile = true;
            });
            var myCreateAutoReconcileWhileStreamingService = A.Fake<CreateAutoExternalReconcileWhileStreamingService>(option => option.CallsBaseMethods());
            myCreateAutoReconcileWhileStreamingService
            .MustInit(fakeExternalReconcileDataProvider,
               newJournal,
            myNewLedgerTransactionList);
            myCreateAutoReconcileWhileStreamingService.CreateAutoExternalReconcileWhileStreaming();




            Assert.IsNotNull(myCreateAutoReconcileWhileStreamingService.ExternalReconciliationList);
            Assert.AreEqual(1, myCreateAutoReconcileWhileStreamingService.ExternalReconciliationList.Count());
            var myExternalReconciliation = myCreateAutoReconcileWhileStreamingService.ExternalReconciliationList[0];
            Assert.AreEqual(3, myExternalReconciliation.ExternalReconciliationLines.Count());
            Assert.AreEqual(reconcileExternalPageLineLists[0].Id, myExternalReconciliation.ExternalReconciliationLines[0].ExternalPageLineId);
            Assert.AreEqual(reconcileExternalPageLineLists[1].Id, myExternalReconciliation.ExternalReconciliationLines[1].ExternalPageLineId);

            Assert.IsNull(myExternalReconciliation.ExternalReconciliationLines[0].LedgerTransactionId);
            Assert.IsNull(myExternalReconciliation.ExternalReconciliationLines[1].LedgerTransactionId);


            Assert.IsNull(myExternalReconciliation.ExternalReconciliationLines[2].ExternalPageLineId);
            Assert.AreEqual(myNewLedgerTransactionId, myExternalReconciliation.ExternalReconciliationLines[2].LedgerTransactionId);

            //Assert.AreEqual(3, myExternalReconciliationList[0].);


        }
        void checkCreateJournalWithExtReconcilePerExample(bool allreconcileExternalPageLineListinDebit)
        {
            List<ReconcileExternalPageLineList> reconcileExternalPageLineLists;
            LastRate lastRate;
            ExternalReconcileDataProvider fakeExternalReconcileDataProvider =
            PrepareContext(out reconcileExternalPageLineLists, out lastRate, allreconcileExternalPageLineListinDebit);


            IExternalReconcileAdjustBankFeesService myExternalReconcileAdjustBankFeesService = new ExternalReconcileAdjustBankFeesService();
            myExternalReconcileAdjustBankFeesService.MustInit(fakeExternalReconcileDataProvider);
            var reconcileExternalPageLineIdList = reconcileExternalPageLineLists.Select(r => r.Id).ToList();
            
            myExternalReconcileAdjustBankFeesService.CreateJournalWithExtReconcile(_Tenant, reconcileExternalPageLineIdList, _AdjustGLAccountId, _screenNotes, new DateTime(2019, 1, 4));





            Assert.IsNotNull(myExternalReconcileAdjustBankFeesService.TheNewJournal);
            var theCreatedJournal = myExternalReconcileAdjustBankFeesService.TheNewJournal;
            Assert.IsNotNull(theCreatedJournal.JournalLines);
            Assert.AreEqual(3, theCreatedJournal.JournalLines.Count);
            Assert.AreEqual(_ReferenceDate, theCreatedJournal.JournalLines[0].DueDate);
            Assert.AreEqual(_ReferenceDate, theCreatedJournal.JournalLines[1].DueDate);


            if (allreconcileExternalPageLineListinDebit)
            {
                Assert.AreEqual(1, theCreatedJournal.JournalLines.Count(r => r.ActionTypeCodeEnum == MyJournalActionTypeEnum.Debit));
                Assert.AreEqual(2, theCreatedJournal.JournalLines.Count(r => r.ActionTypeCodeEnum == MyJournalActionTypeEnum.Credit));


                Assert.AreEqual(_BankAccountId, theCreatedJournal.JournalLines[0].CreditAccountId);
                Assert.AreEqual(_BankAccountId, theCreatedJournal.JournalLines[1].CreditAccountId);
                Assert.AreEqual(MyJournalActionTypeEnum.Credit, theCreatedJournal.JournalLines[0].ActionTypeCodeEnum);
                Assert.AreEqual(MyJournalActionTypeEnum.Credit, theCreatedJournal.JournalLines[1].ActionTypeCodeEnum);

                Assert.AreEqual(_AdjustGLAccountId, theCreatedJournal.JournalLines[0].DebitAccountId);
                Assert.AreEqual(_AdjustGLAccountId, theCreatedJournal.JournalLines[1].DebitAccountId);


                Assert.AreEqual(reconcileExternalPageLineLists[0].DebitAmount, theCreatedJournal.JournalLines[0].ForeignAmount);
                Assert.AreEqual(reconcileExternalPageLineLists[1].DebitAmount, theCreatedJournal.JournalLines[1].ForeignAmount);

                Assert.AreEqual(reconcileExternalPageLineLists[0].DebitAmount * (decimal)lastRate.Rate, theCreatedJournal.JournalLines[0].LocalAmount);
                Assert.AreEqual(reconcileExternalPageLineLists[1].DebitAmount * (decimal)lastRate.Rate, theCreatedJournal.JournalLines[1].LocalAmount);


                Assert.AreEqual(theCreatedJournal.JournalLines[2].ActionTypeCodeEnum, MyJournalActionTypeEnum.Debit);
                Assert.AreEqual(
                    (
                    (reconcileExternalPageLineLists[0].DebitAmount + reconcileExternalPageLineLists[1].DebitAmount)
                    * (decimal)lastRate.Rate
                    )
                    , theCreatedJournal.JournalLines[2].LocalAmount);


            }
            else
            {
                Assert.AreEqual(1, theCreatedJournal.JournalLines.Count(r => r.ActionTypeCodeEnum == MyJournalActionTypeEnum.Credit));
                Assert.AreEqual(2, theCreatedJournal.JournalLines.Count(r => r.ActionTypeCodeEnum == MyJournalActionTypeEnum.Debit));

                Assert.AreEqual(_BankAccountId, theCreatedJournal.JournalLines[0].DebitAccountId);
                Assert.AreEqual(_BankAccountId, theCreatedJournal.JournalLines[1].DebitAccountId);
                Assert.AreEqual(MyJournalActionTypeEnum.Debit, theCreatedJournal.JournalLines[0].ActionTypeCodeEnum);
                Assert.AreEqual(MyJournalActionTypeEnum.Debit, theCreatedJournal.JournalLines[1].ActionTypeCodeEnum);

                Assert.AreEqual(_AdjustGLAccountId, theCreatedJournal.JournalLines[0].CreditAccountId);
                Assert.AreEqual(_AdjustGLAccountId, theCreatedJournal.JournalLines[1].CreditAccountId);


                Assert.AreEqual(reconcileExternalPageLineLists[0].CreditAmount, theCreatedJournal.JournalLines[0].ForeignAmount);
                Assert.AreEqual(reconcileExternalPageLineLists[1].CreditAmount, theCreatedJournal.JournalLines[1].ForeignAmount);

                Assert.AreEqual(reconcileExternalPageLineLists[0].CreditAmount * (decimal)lastRate.Rate, theCreatedJournal.JournalLines[0].LocalAmount);
                Assert.AreEqual(reconcileExternalPageLineLists[1].CreditAmount * (decimal)lastRate.Rate, theCreatedJournal.JournalLines[1].LocalAmount);

                Assert.AreEqual(theCreatedJournal.JournalLines[2].ActionTypeCodeEnum, MyJournalActionTypeEnum.Credit);
                Assert.AreEqual(
                    (
                    (reconcileExternalPageLineLists[0].CreditAmount + reconcileExternalPageLineLists[1].CreditAmount)
                    * (decimal)lastRate.Rate
                    )
                    , theCreatedJournal.JournalLines[2].LocalAmount);


            }




            Assert.IsTrue(theCreatedJournal.JournalLines[0].Notes.Contains(_screenNotes));
            Assert.IsTrue(theCreatedJournal.JournalLines[1].Notes.Contains(_screenNotes));
            Assert.IsTrue(theCreatedJournal.JournalLines[2].Notes.Contains(_screenNotes));






            Assert.IsNotNull(theCreatedJournal.JournalExternalReconciles);
            Assert.AreEqual(2, theCreatedJournal.JournalExternalReconciles.Count);


            Assert.IsFalse(theCreatedJournal.JournalExternalReconciles.Any(r => r.LedgerTransactionId != null));
            Assert.IsTrue(theCreatedJournal.JournalExternalReconciles.TrueForAll(r => r.ReconcileExternalPageLineId != null));



            Assert.AreEqual("ReconcileExternalPageLineListID", theCreatedJournal.JournalExternalReconciles[0].ReconcileExternalPageLineId);
            Assert.AreEqual("ReconcileExternalPageLineListID2", theCreatedJournal.JournalExternalReconciles[1].ReconcileExternalPageLineId);


        }




        private ExternalReconcileDataProvider PrepareContext(out List<ReconcileExternalPageLineList> reconcileExternalPageLineLists, out LastRate lastRate, bool AlllinDebit)
        {
            ExternalReconcileDataProvider fakeExternalReconcileDataProvider = null;
            _Tenant = 1;
            _AdjustGLAccountId = "adjustGLAccountId";
            _screenNotes = "screenNotes";
            _accountingCurrencyId = "NIS";
            _BankCurrencyId = "usd";
            _BankAccountId = "BankAccountId";
            _GLAccountId = "GLAccountId";

            _ReferenceDate = new DateTime(2019, 09, 09);
            reconcileExternalPageLineLists = new List<ReconcileExternalPageLineList>()
                {
                new ReconcileExternalPageLineList
                {
                Tenant= _Tenant,
                Id="ReconcileExternalPageLineListID",
                 CreditAmount =AlllinDebit?0:100,
                  DebitAmount = AlllinDebit?100:0,
                   InProgressExternalReconcile=false,
                    ReconcileExternalPageId="ReconcileExternalPageId",
                     Reference="Reference",
                      ReferenceDate=  _ReferenceDate,
                },
                new ReconcileExternalPageLineList
                {
                    Tenant= _Tenant,
                    Id="ReconcileExternalPageLineListID2",

                    CreditAmount = AlllinDebit?0: 150,
                    DebitAmount = AlllinDebit?150: 0,

                    InProgressExternalReconcile=false,
                    ReconcileExternalPageId="ReconcileExternalPageId",
                    Reference="Reference",
                    ReferenceDate=  _ReferenceDate,
                }
                };

            List<ReconcileExternalPageList> reconcileExternalPageLists = new List<ReconcileExternalPageList>()
            {
                new ReconcileExternalPageList()
                {
                      Tenant= _Tenant,
                       //BankAccountId =_BankAccountId,
                        GLAccountId= _GLAccountId
                }
            };

            lastRate = new LastRate()
            {
                Rate = 5
            };

            //string accountingCurrencyId = _ExternalReconcileDataProvider.GetaccountingCurrencyId(tenant);
            List<GLAccountList> gLAccountLists = new List<GLAccountList>()
            {
                new GLAccountList()
                {
                     Tenant = _Tenant,
                      CurrencyId = _BankCurrencyId,
                       Id= _BankAccountId,

                }
            };




            IAccountingContext fakeIAccountingContext = A.Fake<IAccountingContext>();

            fakeExternalReconcileDataProvider = A.Fake<ExternalReconcileDataProvider>();

            A.CallTo(() =>
                   fakeExternalReconcileDataProvider.GetReconcileExternalPageLineList(_Tenant, A<List<string>>.Ignored))
                   .Returns(reconcileExternalPageLineLists);
            A.CallTo(() =>
                   fakeExternalReconcileDataProvider.GetReconcileExternalPageList(_Tenant, A<List<string>>.Ignored))
                   .Returns(reconcileExternalPageLists);


            A.CallTo(() =>
       fakeExternalReconcileDataProvider.GetaccountingCurrencyId(_Tenant))
       .Returns(_accountingCurrencyId);

            A.CallTo(() =>
       fakeExternalReconcileDataProvider.GetListOfGLAccountList(_Tenant, A<List<string>>.Ignored))
       .Returns(gLAccountLists);

            A.CallTo(() =>
       fakeExternalReconcileDataProvider.GetLastRateByValueDate(_Tenant, _BankCurrencyId, _accountingCurrencyId, _ReferenceDate))
       .Returns(lastRate);
            return fakeExternalReconcileDataProvider;
        }

    }
}
