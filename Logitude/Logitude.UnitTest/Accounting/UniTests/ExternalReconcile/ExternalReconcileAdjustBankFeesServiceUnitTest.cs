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
        [TestMethod]
        public void TestMethod1()
        {
            int tenant = 1;
            string adjustGLAccountId = "adjustGLAccountId";
            string screenNotes = "screenNotes";
            string accountingCurrencyId = "NIS";
            string BankCurrencyId = "usd";
            string BankAccountId = "BankAccountId";
            string GLAccountId = "GLAccountId";
            DateTime ReferenceDate = new DateTime(2019, 09, 09);
            List<ReconcileExternalPageLineList> reconcileExternalPageLineLists =
                new List<ReconcileExternalPageLineList>()
                {
                new ReconcileExternalPageLineList
                {
                Tenant= tenant,
                Id="ReconcileExternalPageLineListID",
                 CreditAmount =100,
                  DebitAmount = 0,
                   InProgressExternalReconcile=false,
                    ReconcileExternalPageId="ReconcileExternalPageId",
                     Reference="Reference",
                      ReferenceDate=  ReferenceDate,
                },
                new ReconcileExternalPageLineList
                {
                Tenant= tenant,
                Id="ReconcileExternalPageLineListID2",

                 CreditAmount =150,
                  DebitAmount = 0,
                   InProgressExternalReconcile=false,
                    ReconcileExternalPageId="ReconcileExternalPageId",
                     Reference="Reference",
                      ReferenceDate=  ReferenceDate,
                }
                };
            List<ReconcileExternalPageList> reconcileExternalPageLists = new List<ReconcileExternalPageList>()
            {
                new ReconcileExternalPageList()
                {
                      Tenant= tenant,
                       BankAccountId =BankAccountId,
                        GLAccountId= GLAccountId
                }
            };

            LastRate lastRate = new LastRate()
            {
                Rate = 5
            };

            //string accountingCurrencyId = _ExternalReconcileDataProvider.GetaccountingCurrencyId(tenant);
            List<GLAccountList> gLAccountLists = new List<GLAccountList>()
            {
                new GLAccountList()
                {
                     Tenant = tenant,
                      CurrencyId = BankCurrencyId,
                       Id= BankAccountId,

                }
            };




            IAccountingContext fakeIAccountingContext = A.Fake<IAccountingContext>();

            var fakeExternalReconcileDataProvider = A.Fake<ExternalReconcileDataProvider>();


            A.CallTo(() =>
                   fakeExternalReconcileDataProvider.GetReconcileExternalPageLineList(tenant, A<List<string>>.Ignored))
                   .Returns(reconcileExternalPageLineLists);
            A.CallTo(() =>
                   fakeExternalReconcileDataProvider.GetReconcileExternalPageList(tenant, A<List<string>>.Ignored))
                   .Returns(reconcileExternalPageLists);


            A.CallTo(() =>
       fakeExternalReconcileDataProvider.GetaccountingCurrencyId(tenant))
       .Returns(accountingCurrencyId);

            A.CallTo(() =>
       fakeExternalReconcileDataProvider.GetListOfGLAccountList(tenant, A<List<string>>.Ignored))
       .Returns(gLAccountLists);

            A.CallTo(() =>
       fakeExternalReconcileDataProvider.GetLastRateByValueDate(tenant, BankCurrencyId, accountingCurrencyId, ReferenceDate))
       .Returns(lastRate);




            var myExternalReconcileAdjustBankFeesService = new ExternalReconcileAdjustBankFeesService();
            myExternalReconcileAdjustBankFeesService.MustInit(fakeExternalReconcileDataProvider);
            var reconcileExternalPageLineIdList = reconcileExternalPageLineLists.Select(r => r.Id).ToList();

            myExternalReconcileAdjustBankFeesService.CreateJournalWithExtReconcile(tenant, reconcileExternalPageLineIdList, adjustGLAccountId, screenNotes);


            Assert.IsNotNull(myExternalReconcileAdjustBankFeesService.TheNewJournal);
            var theCreatedJournal = myExternalReconcileAdjustBankFeesService.TheNewJournal;
            Assert.IsNotNull(theCreatedJournal.JournalLines);
            Assert.AreEqual(3, theCreatedJournal.JournalLines.Count);
            Assert.AreEqual(1, theCreatedJournal.JournalLines.Count(r => r.ActionTypeCodeEnum == MyJournalActionTypeEnum.Credit));
            Assert.AreEqual(2, theCreatedJournal.JournalLines.Count(r => r.ActionTypeCodeEnum == MyJournalActionTypeEnum.Debit));
            

            Assert.AreEqual(adjustGLAccountId, theCreatedJournal.JournalLines[0].DebitAccountId);
            Assert.AreEqual(adjustGLAccountId, theCreatedJournal.JournalLines[1].DebitAccountId);
            Assert.AreEqual(MyJournalActionTypeEnum.Debit, theCreatedJournal.JournalLines[0].ActionTypeCodeEnum );
            Assert.AreEqual(MyJournalActionTypeEnum.Debit, theCreatedJournal.JournalLines[1].ActionTypeCodeEnum);
            Assert.AreEqual(ReferenceDate, theCreatedJournal.JournalLines[0].DueDate);
            Assert.AreEqual(ReferenceDate, theCreatedJournal.JournalLines[1].DueDate);

            Assert.AreEqual(BankAccountId, theCreatedJournal.JournalLines[0].CreditAccountId);
            Assert.AreEqual(BankAccountId, theCreatedJournal.JournalLines[1].CreditAccountId);

            Assert.AreEqual(reconcileExternalPageLineLists[0].CreditAmount, theCreatedJournal.JournalLines[0].ForeignAmount);
            Assert.AreEqual(reconcileExternalPageLineLists[1].CreditAmount, theCreatedJournal.JournalLines[1].ForeignAmount);

            Assert.AreEqual(reconcileExternalPageLineLists[0].CreditAmount* (decimal)lastRate.Rate, theCreatedJournal.JournalLines[0].LocalAmount);
            Assert.AreEqual(reconcileExternalPageLineLists[1].CreditAmount * (decimal)lastRate.Rate, theCreatedJournal.JournalLines[1].LocalAmount);

            Assert.IsTrue(theCreatedJournal.JournalLines[0].Notes.Contains(screenNotes));
            Assert.IsTrue(theCreatedJournal.JournalLines[1].Notes.Contains(screenNotes));
            Assert.IsTrue(theCreatedJournal.JournalLines[2].Notes.Contains(screenNotes));




            Assert.AreEqual(theCreatedJournal.JournalLines[2].ActionTypeCodeEnum, MyJournalActionTypeEnum.Credit);
            Assert.AreEqual((reconcileExternalPageLineLists[0].CreditAmount+ reconcileExternalPageLineLists[1].CreditAmount) * (decimal)lastRate.Rate, theCreatedJournal.JournalLines[2].LocalAmount);
            //Assert.AreEqual(2, theCreatedJournal.JournalLines.Count(r => r.CreditAccountId == GLAccountId));

            //Assert.AreEqual(2, theCreatedJournal.JournalLines.Count(r => r.LocalAmount == myOrginalJournalTransaction.First().LocalAmountCredit));

            //Assert.IsNotNull(theCreatedJournal.JournalExternalReconciles);
            //Assert.AreEqual(1, theCreatedJournal.JournalExternalReconciles.Count);

            //Assert.AreEqual(myReconcileExternalPageLinePM.Id, theCreatedJournal.JournalExternalReconciles[0].ReconcileExternalPageLineId);
            //Assert.AreEqual(myOrginalJournalTransaction.First().Id, theCreatedJournal.JournalExternalReconciles[0].LedgerTransactionId);
        }


    }
}
