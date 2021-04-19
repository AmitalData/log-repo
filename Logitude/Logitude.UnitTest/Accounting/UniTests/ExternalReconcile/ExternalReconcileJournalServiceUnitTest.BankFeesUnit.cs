using FakeItEasy;
using Logitude.Accounting.BL.CoreBL.ExternalReconcile;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.UnitTest.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.UnitTest.Accounting.UniTests
{
   
    public partial class ExternalReconcileJournalServiceUnitTest
    {
        [TestMethod]
        public void MoveBankCheckFromTransfer2GLAccountAndBankFees_foreign_GoodExample_CheckSuccess()
        {
            BankAccountPM myBankAccountPM;
            ReconcileExternalPageLinePM myReconcileExternalPageLinePM;
            List<LedgerTransactionPM> myOrginalJournalTransaction, myNewLedgerTransactionList;
            LedgerTransactionPM myLedgerTransactionTransferInCredit;
            JournalPM newJournal;
            CreateAutoExternalReconcileWhileStreamingServiceUnitTest
                .Example4AutoExternalReconcile(out myBankAccountPM, out myReconcileExternalPageLinePM, out myOrginalJournalTransaction, out myLedgerTransactionTransferInCredit, out newJournal, out myNewLedgerTransactionList);
            myOrginalJournalTransaction.ForEach(r => r.InProgressExternalReconcile = false);

            myBankAccountPM.CurrencyId = "USD";
            myOrginalJournalTransaction.First().CurrencyId = "USD";
            myOrginalJournalTransaction.First().ForeignAmountCredit = myOrginalJournalTransaction.First().LocalAmountCredit;
            myOrginalJournalTransaction.First().LocalAmountCredit = 122;

            int bankFees = 1;
            // *בנק לשלם כרטיס 102.1 בזכות - תמיד בזכות
            ///* -דף בנק 102.1 +1  בחובה
            myReconcileExternalPageLinePM.DebitAmount += bankFees;//bankfees in page line - adjust !!

            IAccountingContext fakeIAccountingContext;
            fakeIAccountingContext = A.Fake<IAccountingContext>();

            var fakeExternalReconcileDataProvider = A.Fake<ExternalReconcileDataProvider>();
            myReconcileExternalPageLinePM.ReconcileExternalPageId = "ReconcileExternalPageId";
            myReconcileExternalPageLinePM.ReferenceDate = new DateTime(2020, 12, 28);
            //myReconcileExternalPageLinePM.ReconcileExternalPageId=  myBankAccountPM.Id = "myBankAccountPM.Id";

            A.CallTo(() =>
                   fakeExternalReconcileDataProvider.GetLedgerTransactionList(A<List<string>>.Ignored, newJournal.Tenant))
                   .Returns(myOrginalJournalTransaction);
            A.CallTo(() =>
                   fakeExternalReconcileDataProvider.GetReconcileExternalPageLinePM(_Tenant, myReconcileExternalPageLinePM.Id))
                   .Returns(myReconcileExternalPageLinePM);
            A.CallTo(() => fakeExternalReconcileDataProvider
                  .GetReconcileExternalPageLineList(_Tenant,
                  A<List<string>>.Ignored//() { myReconcileExternalPageLinePM.Id }
                  ))
                  .Returns(
                new List<ReconcileExternalPageLineList>()
                {
                      new ReconcileExternalPageLineList()
                      {
                      ReconcileExternalPageId = myReconcileExternalPageLinePM.ReconcileExternalPageId,
                      DebitAmount = myReconcileExternalPageLinePM.DebitAmount,
                      CreditAmount = myReconcileExternalPageLinePM.CreditAmount,
                      ReferenceDate= myReconcileExternalPageLinePM.ReferenceDate,



                      }
                });
            // 
            A.CallTo(() => fakeExternalReconcileDataProvider
                  .GetReconcileExternalPageList(_Tenant,
                  A<List<string>>.Ignored//new List<string>() { myReconcileExternalPageLinePM.ReconcileExternalPageId }
                  ))
                  .Returns(
                new List<ReconcileExternalPageList>()
                {
                      new ReconcileExternalPageList()
                      {
                      GLAccountId = myBankAccountPM.GLAccountId,


                      }
                });

            A.CallTo(() => fakeExternalReconcileDataProvider
                  .GetListOfGLAccountList(_Tenant,
                  A<List<string>>.Ignored//new List<string>() { myReconcileExternalPageLinePM.ReconcileExternalPageId }
                  ))
                  .Returns(
                new List<GLAccountList>()
                {
                      new GLAccountList()
                      {
                      Id = myBankAccountPM.GLAccountId,
                      CurrencyId =myBankAccountPM.CurrencyId


                      }
                });

            A.CallTo(() =>
                   fakeExternalReconcileDataProvider.GetaccountingCurrencyId(_Tenant))
                   .Returns("NIS");

            A.CallTo(() =>
       fakeExternalReconcileDataProvider.GetBankAccountFromTransferAccount(myLedgerTransactionTransferInCredit.AccountId, _Tenant))
       .Returns(myBankAccountPM);




            var myExternalReconcileJournalService = new ExternalReconcileMoveBankCheckFromTransfer2GLAccountService();
            myExternalReconcileJournalService.MustInit(fakeExternalReconcileDataProvider);
            string adjustGLAccountId = "adjustGLAccountId";
            myExternalReconcileJournalService.OnAdjustMustInit(adjustGLAccountId, "screenNotes");
            myExternalReconcileJournalService.CreateJournalWithExtReconcile(_Tenant, myOrginalJournalTransaction.First().Id, myReconcileExternalPageLinePM.Id);

            Assert.IsNotNull(myExternalReconcileJournalService.TheJournalPM);
            var theCreatedJournal = myExternalReconcileJournalService.TheJournalPM;
            Assert.IsNotNull(theCreatedJournal.JournalLines);
            Assert.AreEqual(4, theCreatedJournal.JournalLines.Count);
            Assert.AreEqual(2, theCreatedJournal.JournalLines.Count(r => r.ActionTypeCodeEnum == MyJournalActionTypeEnum.Credit));
            Assert.AreEqual(2, theCreatedJournal.JournalLines.Count(r => r.ActionTypeCodeEnum == MyJournalActionTypeEnum.Debit));

            Assert.AreEqual(0, theCreatedJournal.JournalLines.Count(r => string.IsNullOrWhiteSpace(r.DebitAccountId)));
            Assert.AreEqual(0, theCreatedJournal.JournalLines.Count(r => string.IsNullOrWhiteSpace(r.CreditAccountId)));

        

            //*פקודת יומן
            ///* חייב את בנק לשלם ב 102.1 - JLINE1
            Assert.AreEqual(1, theCreatedJournal.JournalLines.Count(r =>
            r.ForeignAmount == myLedgerTransactionTransferInCredit.ForeignAmountCredit &&
            r.ActionTypeCodeEnum == MyJournalActionTypeEnum.Debit &&
            r.DebitAccountId == myBankAccountPM.TransferGLAcccountId
            && r.AccountingDate== r.DueDate && r.DocumentDate== myReconcileExternalPageLinePM.ReferenceDate
            ));

            ///*זכה את העוש ב 100 - JLINE2
            Assert.AreEqual(1, theCreatedJournal.JournalLines.Count(r =>
           r.ForeignAmount == myLedgerTransactionTransferInCredit.ForeignAmountCredit &&
           r.ActionTypeCodeEnum == MyJournalActionTypeEnum.Credit &&
           r.CreditAccountId == myBankAccountPM.GLAccountId
           && r.AccountingDate == r.DueDate && r.DocumentDate == myReconcileExternalPageLinePM.ReferenceDate
           ));

            //*באם
            //* יש הפרש של שקל עמלה
            //*זכה את הבנק עוש ב  שקל - JLINE3
            Assert.AreEqual(1, theCreatedJournal.JournalLines.Count(r =>
                    r.ForeignAmount == bankFees &&
                    r.ActionTypeCodeEnum == MyJournalActionTypeEnum.Credit &&
                    r.CreditAccountId == myBankAccountPM.GLAccountId
                    && r.AccountingDate == r.DueDate && r.DocumentDate == myReconcileExternalPageLinePM.ReferenceDate
                    ));

            ///* חייב את כ ההפרשים ב שקל - JLINE4
            Assert.AreEqual(1, theCreatedJournal.JournalLines.Count(r =>
                    r.ForeignAmount == bankFees &&
                    r.ActionTypeCodeEnum == MyJournalActionTypeEnum.Debit &&
                    r.DebitAccountId == adjustGLAccountId
                    && r.AccountingDate == r.DueDate && r.DocumentDate == myReconcileExternalPageLinePM.ReferenceDate
                    ));

          

            Assert.IsNotNull(theCreatedJournal.JournalExternalReconciles);
            Assert.AreEqual(1, theCreatedJournal.JournalExternalReconciles.Count);

            Assert.AreEqual(myReconcileExternalPageLinePM.Id, theCreatedJournal.JournalExternalReconciles[0].ReconcileExternalPageLineId);
            Assert.AreEqual(myOrginalJournalTransaction.First().Id, theCreatedJournal.JournalExternalReconciles[0].LedgerTransactionId);
        }




        [TestMethod]
        public void MoveBankCheckFromTransfer2GLAccountAndDebitBankFees_foreign_GoodExample_CheckSuccess()
        {
            BankAccountPM myBankAccountPM;
            ReconcileExternalPageLinePM myReconcileExternalPageLinePM;
            List<LedgerTransactionPM> myOrginalJournalTransaction, myNewLedgerTransactionList;
            LedgerTransactionPM myLedgerTransactionTransferInCredit;
            JournalPM newJournal;
            CreateAutoExternalReconcileWhileStreamingServiceUnitTest
                .Example4AutoExternalReconcile(out myBankAccountPM, out myReconcileExternalPageLinePM, out myOrginalJournalTransaction, out myLedgerTransactionTransferInCredit, out newJournal, out myNewLedgerTransactionList);
            myOrginalJournalTransaction.ForEach(r => r.InProgressExternalReconcile = false);

            myBankAccountPM.CurrencyId = "USD";
            myOrginalJournalTransaction.First().CurrencyId = "USD";
            myOrginalJournalTransaction.First().ForeignAmountCredit = myOrginalJournalTransaction.First().LocalAmountCredit;
            myOrginalJournalTransaction.First().LocalAmountCredit = 122;

            int bankFees = -1;
            // *בנק לשלם כרטיס 102.1 בזכות - תמיד בזכות
            ///* -דף בנק 102.1 +1  בחובה
            myReconcileExternalPageLinePM.DebitAmount += bankFees;//bankfees in page line - adjust !!

            IAccountingContext fakeIAccountingContext;
            fakeIAccountingContext = A.Fake<IAccountingContext>();

            var fakeExternalReconcileDataProvider = A.Fake<ExternalReconcileDataProvider>();
            myReconcileExternalPageLinePM.ReconcileExternalPageId = "ReconcileExternalPageId";
            myReconcileExternalPageLinePM.ReferenceDate = new DateTime(2020, 12, 28);
            //myReconcileExternalPageLinePM.ReconcileExternalPageId=  myBankAccountPM.Id = "myBankAccountPM.Id";

            A.CallTo(() =>
                   fakeExternalReconcileDataProvider.GetLedgerTransactionList(A<List<string>>.Ignored, newJournal.Tenant))
                   .Returns(myOrginalJournalTransaction);
            A.CallTo(() =>
                   fakeExternalReconcileDataProvider.GetReconcileExternalPageLinePM(_Tenant, myReconcileExternalPageLinePM.Id))
                   .Returns(myReconcileExternalPageLinePM);
            A.CallTo(() => fakeExternalReconcileDataProvider
                  .GetReconcileExternalPageLineList(_Tenant,
                  A<List<string>>.Ignored//() { myReconcileExternalPageLinePM.Id }
                  ))
                  .Returns(
                new List<ReconcileExternalPageLineList>()
                {
                      new ReconcileExternalPageLineList()
                      {
                      ReconcileExternalPageId = myReconcileExternalPageLinePM.ReconcileExternalPageId,
                      DebitAmount = myReconcileExternalPageLinePM.DebitAmount,
                      CreditAmount = myReconcileExternalPageLinePM.CreditAmount,
                      ReferenceDate= myReconcileExternalPageLinePM.ReferenceDate,



                      }
                });
            // 
            A.CallTo(() => fakeExternalReconcileDataProvider
                  .GetReconcileExternalPageList(_Tenant,
                  A<List<string>>.Ignored//new List<string>() { myReconcileExternalPageLinePM.ReconcileExternalPageId }
                  ))
                  .Returns(
                new List<ReconcileExternalPageList>()
                {
                      new ReconcileExternalPageList()
                      {
                      GLAccountId = myBankAccountPM.GLAccountId,


                      }
                });

            A.CallTo(() => fakeExternalReconcileDataProvider
                  .GetListOfGLAccountList(_Tenant,
                  A<List<string>>.Ignored//new List<string>() { myReconcileExternalPageLinePM.ReconcileExternalPageId }
                  ))
                  .Returns(
                new List<GLAccountList>()
                {
                      new GLAccountList()
                      {
                      Id = myBankAccountPM.GLAccountId,
                      CurrencyId =myBankAccountPM.CurrencyId


                      }
                });

            A.CallTo(() =>
                   fakeExternalReconcileDataProvider.GetaccountingCurrencyId(_Tenant))
                   .Returns("NIS");

            A.CallTo(() =>
       fakeExternalReconcileDataProvider.GetBankAccountFromTransferAccount(myLedgerTransactionTransferInCredit.AccountId, _Tenant))
       .Returns(myBankAccountPM);




            var myExternalReconcileJournalService = new ExternalReconcileMoveBankCheckFromTransfer2GLAccountService();
            myExternalReconcileJournalService.MustInit(fakeExternalReconcileDataProvider);
            string adjustGLAccountId = "adjustGLAccountId";
            myExternalReconcileJournalService.OnAdjustMustInit(adjustGLAccountId, "screenNotes");
            myExternalReconcileJournalService.CreateJournalWithExtReconcile(_Tenant, myOrginalJournalTransaction.First().Id, myReconcileExternalPageLinePM.Id);

            Assert.IsNotNull(myExternalReconcileJournalService.TheJournalPM);
            var theCreatedJournal = myExternalReconcileJournalService.TheJournalPM;
            Assert.IsNotNull(theCreatedJournal.JournalLines);
            Assert.AreEqual(4, theCreatedJournal.JournalLines.Count);
            Assert.AreEqual(2, theCreatedJournal.JournalLines.Count(r => r.ActionTypeCodeEnum == MyJournalActionTypeEnum.Credit));
            Assert.AreEqual(2, theCreatedJournal.JournalLines.Count(r => r.ActionTypeCodeEnum == MyJournalActionTypeEnum.Debit));

            Assert.AreEqual(0, theCreatedJournal.JournalLines.Count(r => string.IsNullOrWhiteSpace(r.DebitAccountId)));
            Assert.AreEqual(0, theCreatedJournal.JournalLines.Count(r => string.IsNullOrWhiteSpace(r.CreditAccountId)));



            //*פקודת יומן
            ///* חייב את בנק לשלם ב 102.1 - JLINE1
            Assert.AreEqual(1, theCreatedJournal.JournalLines.Count(r =>
            r.ForeignAmount == myLedgerTransactionTransferInCredit.ForeignAmountCredit &&
            r.ActionTypeCodeEnum == MyJournalActionTypeEnum.Debit &&
            r.DebitAccountId == myBankAccountPM.TransferGLAcccountId
            && r.AccountingDate == r.DueDate && r.DocumentDate == myReconcileExternalPageLinePM.ReferenceDate
            ));

            ///*זכה את העוש ב 100 - JLINE2
            Assert.AreEqual(1, theCreatedJournal.JournalLines.Count(r =>
           r.ForeignAmount == myLedgerTransactionTransferInCredit.ForeignAmountCredit &&
           r.ActionTypeCodeEnum == MyJournalActionTypeEnum.Credit &&
           r.CreditAccountId == myBankAccountPM.GLAccountId
           && r.AccountingDate == r.DueDate && r.DocumentDate == myReconcileExternalPageLinePM.ReferenceDate
           ));

            //*באם
            //* יש הפרש של שקל עמלה **במינוס**
            //*חייב את הבנק עוש ב  שקל - JLINE3
            Assert.AreEqual(1, theCreatedJournal.JournalLines.Count(r =>
                    r.ForeignAmount == Math.Abs(bankFees) &&
                    r.ActionTypeCodeEnum == MyJournalActionTypeEnum.Debit &&
                    r.DebitAccountId == myBankAccountPM.GLAccountId
                    && r.AccountingDate == r.DueDate && r.DocumentDate == myReconcileExternalPageLinePM.ReferenceDate
                    ));

            ///* זכה את כ ההפרשים ב שקל - JLINE4
            Assert.AreEqual(1, theCreatedJournal.JournalLines.Count(r =>
                    r.ForeignAmount == Math.Abs(bankFees) &&
                    r.ActionTypeCodeEnum == MyJournalActionTypeEnum.Credit &&
                    r.CreditAccountId == adjustGLAccountId
                    && r.AccountingDate == r.DueDate && r.DocumentDate == myReconcileExternalPageLinePM.ReferenceDate
                    ));



            Assert.IsNotNull(theCreatedJournal.JournalExternalReconciles);
            Assert.AreEqual(1, theCreatedJournal.JournalExternalReconciles.Count);

            Assert.AreEqual(myReconcileExternalPageLinePM.Id, theCreatedJournal.JournalExternalReconciles[0].ReconcileExternalPageLineId);
            Assert.AreEqual(myOrginalJournalTransaction.First().Id, theCreatedJournal.JournalExternalReconciles[0].LedgerTransactionId);
        }
    }
}
