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
    [TestClass]
    public partial class ExternalReconcileJournalServiceUnitTest
    {
        int _Tenant = 1;


        [TestMethod]
        public void PrepareAndValidate_emptyledgerTransactionBankTransferId_crash()
        {
            BankAccountPM myBankAccountPM;
            ReconcileExternalPageLinePM myReconcileExternalPageLinePM;
            List<LedgerTransactionPM> myOrginalJournalTransaction, myNewLedgerTransactionList;
            LedgerTransactionPM myLedgerTransactionTransferInCredit;
            JournalPM newJournal;
            CreateAutoExternalReconcileWhileStreamingServiceUnitTest
                .Example4AutoExternalReconcile(out myBankAccountPM, out myReconcileExternalPageLinePM, out myOrginalJournalTransaction, out myLedgerTransactionTransferInCredit, out newJournal, out myNewLedgerTransactionList);
            myOrginalJournalTransaction.ForEach(r => r.InProgressExternalReconcile = false);


            IAccountingContext fakeIAccountingContext;
            fakeIAccountingContext = A.Fake<IAccountingContext>();

            var fakeExternalReconcileDataProvider = A.Fake<ExternalReconcileDataProvider>();


            A.CallTo(() =>
                   fakeExternalReconcileDataProvider.GetLedgerTransactionList(A<List<string>>.Ignored, newJournal.Tenant))
                   .Returns(myOrginalJournalTransaction);
            A.CallTo(() =>
                   fakeExternalReconcileDataProvider.GetReconcileExternalPageLinePM(_Tenant, myReconcileExternalPageLinePM.Id))
                   .Returns(myReconcileExternalPageLinePM);


            A.CallTo(() =>
       fakeExternalReconcileDataProvider.GetBankAccountFromTransferAccount(myLedgerTransactionTransferInCredit.AccountId, _Tenant))
       .Returns(myBankAccountPM);




            var myExternalReconcileJournalService = new ExternalReconcileMoveBankCheckFromTransfer2GLAccountService();
            myExternalReconcileJournalService.MustInit(fakeExternalReconcileDataProvider);
            TestsUtil.AssertThrows<Exception>(() =>
            {
                myExternalReconcileJournalService.CreateJournalWithExtReconcile(_Tenant, "", myReconcileExternalPageLinePM.Id);
            }, "ledgerTransactionBankTransferId is must", "if (string.IsNullOrWhiteSpace(ledgerTransactionBankTransferId))");
            

        }
        [TestMethod]
        public void PrepareAndValidate_emptyreconcileExternalPageLineId__crash()
        {
            BankAccountPM myBankAccountPM;
            ReconcileExternalPageLinePM myReconcileExternalPageLinePM;
            List<LedgerTransactionPM> myOrginalJournalTransaction, myNewLedgerTransactionList;
            LedgerTransactionPM myLedgerTransactionTransferInCredit;
            JournalPM newJournal;
            CreateAutoExternalReconcileWhileStreamingServiceUnitTest
                .Example4AutoExternalReconcile(out myBankAccountPM, out myReconcileExternalPageLinePM, out myOrginalJournalTransaction, out myLedgerTransactionTransferInCredit, out newJournal, out myNewLedgerTransactionList);
            myOrginalJournalTransaction.ForEach(r => r.InProgressExternalReconcile = false);


            IAccountingContext fakeIAccountingContext;
            fakeIAccountingContext = A.Fake<IAccountingContext>();

            var fakeExternalReconcileDataProvider = A.Fake<ExternalReconcileDataProvider>();


            A.CallTo(() =>
                   fakeExternalReconcileDataProvider.GetLedgerTransactionList(A<List<string>>.Ignored, newJournal.Tenant))
                   .Returns(myOrginalJournalTransaction);
            A.CallTo(() =>
                   fakeExternalReconcileDataProvider.GetReconcileExternalPageLinePM(_Tenant, myReconcileExternalPageLinePM.Id))
                   .Returns(myReconcileExternalPageLinePM);


            A.CallTo(() =>
       fakeExternalReconcileDataProvider.GetBankAccountFromTransferAccount(myLedgerTransactionTransferInCredit.AccountId, _Tenant))
       .Returns(myBankAccountPM);




            var myExternalReconcileJournalService = new ExternalReconcileMoveBankCheckFromTransfer2GLAccountService();
            myExternalReconcileJournalService.MustInit(fakeExternalReconcileDataProvider);
            TestsUtil.AssertThrows<Exception>(() =>
            {
                myExternalReconcileJournalService.CreateJournalWithExtReconcile(_Tenant, myOrginalJournalTransaction.First().Id,"" /*myReconcileExternalPageLinePM.Id*/);
            }, "reconcileExternalPageLineId is must", "if (string.IsNullOrWhiteSpace(reconcileExternalPageLineId))");


        }


        [TestMethod]
        public void PrepareAndValidate_BadLegdagernotexist__crash()
        {
            BankAccountPM myBankAccountPM;
            ReconcileExternalPageLinePM myReconcileExternalPageLinePM;
            List<LedgerTransactionPM> myOrginalJournalTransaction, myNewLedgerTransactionList;
            LedgerTransactionPM myLedgerTransactionTransferInCredit;
            JournalPM newJournal;
            CreateAutoExternalReconcileWhileStreamingServiceUnitTest
                .Example4AutoExternalReconcile(out myBankAccountPM, out myReconcileExternalPageLinePM, out myOrginalJournalTransaction, out myLedgerTransactionTransferInCredit, out newJournal, out myNewLedgerTransactionList);
            myOrginalJournalTransaction.ForEach(r => r.InProgressExternalReconcile = false);


            IAccountingContext fakeIAccountingContext;
            fakeIAccountingContext = A.Fake<IAccountingContext>();

            var fakeExternalReconcileDataProvider = A.Fake<ExternalReconcileDataProvider>();


            //A.CallTo(() =>
            //       fakeExternalReconcileDataProvider.GetLedgerTransactionList(A<List<string>>.Ignored, newJournal.Tenant))
            //       .Returns(myOrginalJournalTransaction);
            A.CallTo(() =>
                   fakeExternalReconcileDataProvider.GetReconcileExternalPageLinePM(_Tenant, myReconcileExternalPageLinePM.Id))
                   .Returns(myReconcileExternalPageLinePM);


            A.CallTo(() =>
       fakeExternalReconcileDataProvider.GetBankAccountFromTransferAccount(myLedgerTransactionTransferInCredit.AccountId, _Tenant))
       .Returns(myBankAccountPM);




            var myExternalReconcileJournalService = new ExternalReconcileMoveBankCheckFromTransfer2GLAccountService();
            myExternalReconcileJournalService.MustInit(fakeExternalReconcileDataProvider);
            TestsUtil.AssertThrows<Exception>(() =>
            {
                myExternalReconcileJournalService.CreateJournalWithExtReconcile(_Tenant, "my Bad Legdager not exist"/*myOrginalJournalTransaction.First().Id*/, myReconcileExternalPageLinePM.Id);
            }, "Ledger not exist", "_ExternalReconcileDataProvider.GetLedgerTransactionList");


        }



        [TestMethod]
        public void MoveBankCheckFromTransfer2GLAccount_LedgerNotInTransferBank_ErrorRaise()
        {
            BankAccountPM myBankAccountPM;
            ReconcileExternalPageLinePM myReconcileExternalPageLinePM;
            List<LedgerTransactionPM> myOrginalJournalTransaction, myNewLedgerTransactionList;
            LedgerTransactionPM myLedgerTransactionTransferInCredit;
            JournalPM newJournal;
            CreateAutoExternalReconcileWhileStreamingServiceUnitTest
                .Example4AutoExternalReconcile(out myBankAccountPM, out myReconcileExternalPageLinePM, out myOrginalJournalTransaction, out myLedgerTransactionTransferInCredit, out newJournal, out myNewLedgerTransactionList);
            myOrginalJournalTransaction.ForEach(r => r.InProgressExternalReconcile = false);


            IAccountingContext fakeIAccountingContext;
            fakeIAccountingContext = A.Fake<IAccountingContext>();

            var fakeExternalReconcileDataProvider = A.Fake<ExternalReconcileDataProvider>();


            A.CallTo(() =>
                   fakeExternalReconcileDataProvider.GetLedgerTransactionList(A<List<string>>.Ignored, newJournal.Tenant))
                   .Returns(myOrginalJournalTransaction);
            A.CallTo(() =>
                   fakeExternalReconcileDataProvider.GetReconcileExternalPageLinePM(_Tenant, myReconcileExternalPageLinePM.Id))
                   .Returns(myReconcileExternalPageLinePM);

            A.CallTo(() =>
                  fakeExternalReconcileDataProvider.GetaccountingCurrencyId(_Tenant))
                  .Returns("NIS");

            A.CallTo(() =>
       fakeExternalReconcileDataProvider.GetBankAccountFromTransferAccount(myLedgerTransactionTransferInCredit.AccountId, _Tenant))
       .Returns(/*myBankAccountPM*/null);







            var myExternalReconcileJournalService = new ExternalReconcileMoveBankCheckFromTransfer2GLAccountService();
            myExternalReconcileJournalService.MustInit(fakeExternalReconcileDataProvider);
            TestsUtil.AssertThrows<Exception>(() =>
            {
                myExternalReconcileJournalService.CreateJournalWithExtReconcile(_Tenant, myOrginalJournalTransaction.First().Id, myReconcileExternalPageLinePM.Id);
            }, ExternalReconcileMoveBankCheckFromTransfer2GLAccountService.M_LedgerNotInTransferBank, "if (bankAccountFromTransfer == null)");

        }




        ///abort test //[TestMethod]
        public void MoveBankCheckFromTransfer2GLAccount_M_InputPageLineNotInTransferBank_ErrorRaise()
        {
            BankAccountPM myBankAccountPM;
            ReconcileExternalPageLinePM myReconcileExternalPageLinePM;
            List<LedgerTransactionPM> myOrginalJournalTransaction, myNewLedgerTransactionList;
            LedgerTransactionPM myLedgerTransactionTransferInCredit;
            JournalPM newJournal;
            CreateAutoExternalReconcileWhileStreamingServiceUnitTest
                .Example4AutoExternalReconcile(out myBankAccountPM, out myReconcileExternalPageLinePM, out myOrginalJournalTransaction, out myLedgerTransactionTransferInCredit, out newJournal, out myNewLedgerTransactionList);
            myOrginalJournalTransaction.ForEach(r => r.InProgressExternalReconcile = false);


            IAccountingContext fakeIAccountingContext;
            fakeIAccountingContext = A.Fake<IAccountingContext>();

            var fakeExternalReconcileDataProvider = A.Fake<ExternalReconcileDataProvider>();


            A.CallTo(() =>
                   fakeExternalReconcileDataProvider.GetLedgerTransactionList(A<List<string>>.Ignored, newJournal.Tenant))
                   .Returns(myOrginalJournalTransaction);
            A.CallTo(() =>
                   fakeExternalReconcileDataProvider.GetReconcileExternalPageLinePM(_Tenant, myReconcileExternalPageLinePM.Id))
                   .Returns(myReconcileExternalPageLinePM);


            A.CallTo(() =>
       fakeExternalReconcileDataProvider.GetBankAccountFromTransferAccount(myLedgerTransactionTransferInCredit.AccountId, _Tenant))
       .Returns(myBankAccountPM);





            

            var myExternalReconcileJournalService = new ExternalReconcileMoveBankCheckFromTransfer2GLAccountService();
            myExternalReconcileJournalService.MustInit(fakeExternalReconcileDataProvider);
            TestsUtil.AssertThrows<Exception>(() =>
            {
                myExternalReconcileJournalService.CreateJournalWithExtReconcile(_Tenant, myOrginalJournalTransaction.First().Id, myReconcileExternalPageLinePM.Id);
            }, ExternalReconcileMoveBankCheckFromTransfer2GLAccountService.M_InputPageLineNotInTransferBank, "if (bankAccountFromReconcileExternalPageLine == null)");

        }

        ///have to teste
        [TestMethod]
        public void MoveBankCheckFromTransfer2GLAccount_M_BankBelongtoDifferentBankThanLedger_RaiseError()
        {
            BankAccountPM myBankAccountPM;
            ReconcileExternalPageLinePM myReconcileExternalPageLinePM;
            List<LedgerTransactionPM> myOrginalJournalTransaction, myNewLedgerTransactionList;
            LedgerTransactionPM myLedgerTransactionTransferInCredit;
            JournalPM newJournal;
            CreateAutoExternalReconcileWhileStreamingServiceUnitTest
                .Example4AutoExternalReconcile(out myBankAccountPM, out myReconcileExternalPageLinePM, out myOrginalJournalTransaction, out myLedgerTransactionTransferInCredit, out newJournal, out myNewLedgerTransactionList);
            myOrginalJournalTransaction.ForEach(r => r.InProgressExternalReconcile = false);


            IAccountingContext fakeIAccountingContext;
            fakeIAccountingContext = A.Fake<IAccountingContext>();

            var fakeExternalReconcileDataProvider = A.Fake<ExternalReconcileDataProvider>();


            A.CallTo(() =>
                   fakeExternalReconcileDataProvider.GetLedgerTransactionList(A<List<string>>.Ignored, newJournal.Tenant))
                   .Returns(myOrginalJournalTransaction);
            A.CallTo(() =>
                   fakeExternalReconcileDataProvider.GetReconcileExternalPageLinePM(_Tenant, myReconcileExternalPageLinePM.Id))
                   .Returns(myReconcileExternalPageLinePM);


            A.CallTo(() =>
       fakeExternalReconcileDataProvider.GetBankAccountFromTransferAccount(myLedgerTransactionTransferInCredit.AccountId, _Tenant))
       .Returns(myBankAccountPM);



            
            A.CallTo(() =>
fakeExternalReconcileDataProvider.GetBankAccountFromReconcileExternalPageLineId(myReconcileExternalPageLinePM.Id, _Tenant))
.Returns(new BankAccountPM() { Id= "DifferentBankThanLedger" });



            var myExternalReconcileJournalService = new ExternalReconcileMoveBankCheckFromTransfer2GLAccountService();
            myExternalReconcileJournalService.MustInit(fakeExternalReconcileDataProvider);
            TestsUtil.AssertThrows<Exception>(() =>
            {
                myExternalReconcileJournalService.CreateJournalWithExtReconcile(_Tenant, myOrginalJournalTransaction.First().Id, myReconcileExternalPageLinePM.Id);
            }, ExternalReconcileMoveBankCheckFromTransfer2GLAccountService.M_BankBelongtoDifferentBankThanLedger, "");

        }
        ///have to teste
        [TestMethod]
        public void MoveBankCheckFromTransfer2GLAccountAndBakFees_OnAdjustMustInit_RaiseError()
        {
            BankAccountPM myBankAccountPM;
            ReconcileExternalPageLinePM myReconcileExternalPageLinePM;
            List<LedgerTransactionPM> myOrginalJournalTransaction, myNewLedgerTransactionList;
            LedgerTransactionPM myLedgerTransactionTransferInCredit;
            JournalPM newJournal;
            CreateAutoExternalReconcileWhileStreamingServiceUnitTest
                .Example4AutoExternalReconcile(out myBankAccountPM, out myReconcileExternalPageLinePM, out myOrginalJournalTransaction, out myLedgerTransactionTransferInCredit, out newJournal, out myNewLedgerTransactionList);
            myOrginalJournalTransaction.ForEach(r => r.InProgressExternalReconcile = false);


            IAccountingContext fakeIAccountingContext;
            fakeIAccountingContext = A.Fake<IAccountingContext>();

            var fakeExternalReconcileDataProvider = A.Fake<ExternalReconcileDataProvider>();


            A.CallTo(() =>
                   fakeExternalReconcileDataProvider.GetLedgerTransactionList(A<List<string>>.Ignored, newJournal.Tenant))
                   .Returns(myOrginalJournalTransaction);
            A.CallTo(() =>
                   fakeExternalReconcileDataProvider.GetReconcileExternalPageLinePM(_Tenant, myReconcileExternalPageLinePM.Id))
                   .Returns(myReconcileExternalPageLinePM);


            A.CallTo(() =>
       fakeExternalReconcileDataProvider.GetBankAccountFromTransferAccount(myLedgerTransactionTransferInCredit.AccountId, _Tenant))
       .Returns(myBankAccountPM);





            myReconcileExternalPageLinePM.DebitAmount += 2;//2 nis bank fees


            var myExternalReconcileJournalService = new ExternalReconcileMoveBankCheckFromTransfer2GLAccountService();
            myExternalReconcileJournalService.MustInit(fakeExternalReconcileDataProvider);
            TestsUtil.AssertThrows<Exception>(() =>
            {
                myExternalReconcileJournalService.CreateJournalWithExtReconcile(_Tenant, myOrginalJournalTransaction.First().Id, myReconcileExternalPageLinePM.Id);
            }, ExternalReconcileMoveBankCheckFromTransfer2GLAccountService.M_OnAdjustMustInit, "");

        }
        ///have to teste
        [TestMethod]
        public void MoveBankCheckFromTransfer2GLAccount_M_CheckAmountInPageMustBeInDebit_RaiseError()
        {
            BankAccountPM myBankAccountPM;
            ReconcileExternalPageLinePM myReconcileExternalPageLinePM;
            List<LedgerTransactionPM> myOrginalJournalTransaction, myNewLedgerTransactionList;
            LedgerTransactionPM myLedgerTransactionTransferInCredit;
            JournalPM newJournal;
            CreateAutoExternalReconcileWhileStreamingServiceUnitTest
                .Example4AutoExternalReconcile(out myBankAccountPM, out myReconcileExternalPageLinePM, out myOrginalJournalTransaction, out myLedgerTransactionTransferInCredit, out newJournal, out myNewLedgerTransactionList);
            myOrginalJournalTransaction.ForEach(r => r.InProgressExternalReconcile = false);


            IAccountingContext fakeIAccountingContext;
            fakeIAccountingContext = A.Fake<IAccountingContext>();

            var fakeExternalReconcileDataProvider = A.Fake<ExternalReconcileDataProvider>();


            A.CallTo(() =>
                   fakeExternalReconcileDataProvider.GetLedgerTransactionList(A<List<string>>.Ignored, newJournal.Tenant))
                   .Returns(myOrginalJournalTransaction);
            A.CallTo(() =>
                   fakeExternalReconcileDataProvider.GetReconcileExternalPageLinePM(_Tenant, myReconcileExternalPageLinePM.Id))
                   .Returns(myReconcileExternalPageLinePM);


            A.CallTo(() =>
       fakeExternalReconcileDataProvider.GetBankAccountFromTransferAccount(myLedgerTransactionTransferInCredit.AccountId, _Tenant))
       .Returns(myBankAccountPM);





            myReconcileExternalPageLinePM.DebitAmount = 0;
            myReconcileExternalPageLinePM.CreditAmount = 10;

            var myExternalReconcileJournalService = new ExternalReconcileMoveBankCheckFromTransfer2GLAccountService();
            myExternalReconcileJournalService.MustInit(fakeExternalReconcileDataProvider);
            TestsUtil.AssertThrows<Exception>(() =>
            {
                myExternalReconcileJournalService.CreateJournalWithExtReconcile(_Tenant, myOrginalJournalTransaction.First().Id, myReconcileExternalPageLinePM.Id);
            }, ExternalReconcileMoveBankCheckFromTransfer2GLAccountService.M_CheckAmountInPageMustBeInDebit, "");

        }

        
        ///have to teste
        [TestMethod]
        public void MoveBankCheckFromTransfer2GLAccount_M_LedgerAlreadyHaveExternalReconcile_RaiseError()
        {
            BankAccountPM myBankAccountPM;
            ReconcileExternalPageLinePM myReconcileExternalPageLinePM;
            List<LedgerTransactionPM> myOrginalJournalTransaction, myNewLedgerTransactionList;
            LedgerTransactionPM myLedgerTransactionTransferInCredit;
            JournalPM newJournal;
            CreateAutoExternalReconcileWhileStreamingServiceUnitTest
                .Example4AutoExternalReconcile(out myBankAccountPM, out myReconcileExternalPageLinePM, out myOrginalJournalTransaction, out myLedgerTransactionTransferInCredit, out newJournal, out myNewLedgerTransactionList);
            myOrginalJournalTransaction.ForEach(r => r.InProgressExternalReconcile = false);


            IAccountingContext fakeIAccountingContext;
            fakeIAccountingContext = A.Fake<IAccountingContext>();

            var fakeExternalReconcileDataProvider = A.Fake<ExternalReconcileDataProvider>();


            A.CallTo(() =>
                   fakeExternalReconcileDataProvider.GetLedgerTransactionList(A<List<string>>.Ignored, newJournal.Tenant))
                   .Returns(myOrginalJournalTransaction);
            A.CallTo(() =>
                   fakeExternalReconcileDataProvider.GetReconcileExternalPageLinePM(_Tenant, myReconcileExternalPageLinePM.Id))
                   .Returns(myReconcileExternalPageLinePM);


            A.CallTo(() =>
       fakeExternalReconcileDataProvider.GetBankAccountFromTransferAccount(myLedgerTransactionTransferInCredit.AccountId, _Tenant))
       .Returns(myBankAccountPM);




            myLedgerTransactionTransferInCredit.IsExternalReconcile = true;
            var myExternalReconcileJournalService = new ExternalReconcileMoveBankCheckFromTransfer2GLAccountService();
            myExternalReconcileJournalService.MustInit(fakeExternalReconcileDataProvider);
            TestsUtil.AssertThrows<Exception>(() =>
            {
                myExternalReconcileJournalService.CreateJournalWithExtReconcile(_Tenant, myOrginalJournalTransaction.First().Id, myReconcileExternalPageLinePM.Id);
            }, ExternalReconcileMoveBankCheckFromTransfer2GLAccountService.M_LedgerAlreadyHaveExternalReconcile, "");

        }



        ///have to teste
        [TestMethod]
        public void MoveBankCheckFromTransfer2GLAccount_M_InProgressExternalReconcile_Ledger_RaiseError()
        {
            BankAccountPM myBankAccountPM;
            ReconcileExternalPageLinePM myReconcileExternalPageLinePM;
            List<LedgerTransactionPM> myOrginalJournalTransaction, myNewLedgerTransactionList;
            LedgerTransactionPM myLedgerTransactionTransferInCredit;
            JournalPM newJournal;
            CreateAutoExternalReconcileWhileStreamingServiceUnitTest
                .Example4AutoExternalReconcile(out myBankAccountPM, out myReconcileExternalPageLinePM, out myOrginalJournalTransaction, out myLedgerTransactionTransferInCredit, out newJournal, out myNewLedgerTransactionList);
            myOrginalJournalTransaction.ForEach(r => r.InProgressExternalReconcile = false);


            IAccountingContext fakeIAccountingContext;
            fakeIAccountingContext = A.Fake<IAccountingContext>();

            var fakeExternalReconcileDataProvider = A.Fake<ExternalReconcileDataProvider>();


            A.CallTo(() =>
                   fakeExternalReconcileDataProvider.GetLedgerTransactionList(A<List<string>>.Ignored, newJournal.Tenant))
                   .Returns(myOrginalJournalTransaction);
            A.CallTo(() =>
                   fakeExternalReconcileDataProvider.GetReconcileExternalPageLinePM(_Tenant, myReconcileExternalPageLinePM.Id))
                   .Returns(myReconcileExternalPageLinePM);


            A.CallTo(() =>
       fakeExternalReconcileDataProvider.GetBankAccountFromTransferAccount(myLedgerTransactionTransferInCredit.AccountId, _Tenant))
       .Returns(myBankAccountPM);




            myOrginalJournalTransaction.ForEach(r => r.InProgressExternalReconcile = true);

            var myExternalReconcileJournalService = new ExternalReconcileMoveBankCheckFromTransfer2GLAccountService();
            myExternalReconcileJournalService.MustInit(fakeExternalReconcileDataProvider);
            TestsUtil.AssertThrows<Exception>(() =>
            {
                myExternalReconcileJournalService.CreateJournalWithExtReconcile(_Tenant, myOrginalJournalTransaction.First().Id, myReconcileExternalPageLinePM.Id);
            }, ExternalReconcileMoveBankCheckFromTransfer2GLAccountService.M_InProgressExternalReconcile_Ledger, "");

        }


        ///have to teste
        [TestMethod]
        public void MoveBankCheckFromTransfer2GLAccount_M_InProgressExternalReconcile_Page_Ledger_RaiseError()
        {
            BankAccountPM myBankAccountPM;
            ReconcileExternalPageLinePM myReconcileExternalPageLinePM;
            List<LedgerTransactionPM> myOrginalJournalTransaction, myNewLedgerTransactionList;
            LedgerTransactionPM myLedgerTransactionTransferInCredit;
            JournalPM newJournal;
            CreateAutoExternalReconcileWhileStreamingServiceUnitTest
                .Example4AutoExternalReconcile(out myBankAccountPM, out myReconcileExternalPageLinePM, out myOrginalJournalTransaction, out myLedgerTransactionTransferInCredit, out newJournal, out myNewLedgerTransactionList);
            myOrginalJournalTransaction.ForEach(r => r.InProgressExternalReconcile = false);


            IAccountingContext fakeIAccountingContext;
            fakeIAccountingContext = A.Fake<IAccountingContext>();

            var fakeExternalReconcileDataProvider = A.Fake<ExternalReconcileDataProvider>();


            A.CallTo(() =>
                   fakeExternalReconcileDataProvider.GetLedgerTransactionList(A<List<string>>.Ignored, newJournal.Tenant))
                   .Returns(myOrginalJournalTransaction);
            A.CallTo(() =>
                   fakeExternalReconcileDataProvider.GetReconcileExternalPageLinePM(_Tenant, myReconcileExternalPageLinePM.Id))
                   .Returns(myReconcileExternalPageLinePM);


            A.CallTo(() =>
       fakeExternalReconcileDataProvider.GetBankAccountFromTransferAccount(myLedgerTransactionTransferInCredit.AccountId, _Tenant))
       .Returns(myBankAccountPM);




            myReconcileExternalPageLinePM.InProgressExternalReconcile = true;

            var myExternalReconcileJournalService = new ExternalReconcileMoveBankCheckFromTransfer2GLAccountService();
            myExternalReconcileJournalService.MustInit(fakeExternalReconcileDataProvider);
            TestsUtil.AssertThrows<Exception>(() =>
            {
                myExternalReconcileJournalService.CreateJournalWithExtReconcile(_Tenant, myOrginalJournalTransaction.First().Id, myReconcileExternalPageLinePM.Id);
            }, ExternalReconcileMoveBankCheckFromTransfer2GLAccountService.M_InProgressExternalReconcile_Page, "");

        }



        [TestMethod]
        public void MoveBankCheckFromTransfer2GLAccount_GoodExample_CheckSuccess()
        {
            BankAccountPM myBankAccountPM;
            ReconcileExternalPageLinePM myReconcileExternalPageLinePM;
            List<LedgerTransactionPM> myOrginalJournalTransaction, myNewLedgerTransactionList;
            LedgerTransactionPM myLedgerTransactionTransferInCredit;
            JournalPM newJournal;
            CreateAutoExternalReconcileWhileStreamingServiceUnitTest
                .Example4AutoExternalReconcile(out myBankAccountPM, out myReconcileExternalPageLinePM, out myOrginalJournalTransaction, out myLedgerTransactionTransferInCredit, out newJournal, out myNewLedgerTransactionList);
            myOrginalJournalTransaction.ForEach(r => r.InProgressExternalReconcile = false);


            IAccountingContext fakeIAccountingContext;
            fakeIAccountingContext = A.Fake<IAccountingContext>();

            var fakeExternalReconcileDataProvider = A.Fake<ExternalReconcileDataProvider>();


            A.CallTo(() =>
                   fakeExternalReconcileDataProvider.GetLedgerTransactionList(A<List<string>>.Ignored, newJournal.Tenant))
                   .Returns(myOrginalJournalTransaction);
            A.CallTo(() =>
                   fakeExternalReconcileDataProvider.GetReconcileExternalPageLinePM(_Tenant, myReconcileExternalPageLinePM.Id))
                   .Returns(myReconcileExternalPageLinePM);
            A.CallTo(() =>
                   fakeExternalReconcileDataProvider.GetaccountingCurrencyId(_Tenant))
                   .Returns("NIS");

            A.CallTo(() =>
       fakeExternalReconcileDataProvider.GetBankAccountFromTransferAccount(myLedgerTransactionTransferInCredit.AccountId, _Tenant))
       .Returns(myBankAccountPM);




            var myExternalReconcileJournalService = new ExternalReconcileMoveBankCheckFromTransfer2GLAccountService();
            myExternalReconcileJournalService.MustInit(fakeExternalReconcileDataProvider);
            myExternalReconcileJournalService.CreateJournalWithExtReconcile(_Tenant, myOrginalJournalTransaction.First().Id, myReconcileExternalPageLinePM.Id);

            Assert.IsNotNull(myExternalReconcileJournalService.TheJournalPM);
            var theCreatedJournal = myExternalReconcileJournalService.TheJournalPM;
            Assert.IsNotNull(theCreatedJournal.JournalLines);
            Assert.AreEqual(2, theCreatedJournal.JournalLines.Count);
            Assert.AreEqual(1, theCreatedJournal.JournalLines.Count(r => r.ActionTypeCodeEnum == MyJournalActionTypeEnum.Credit));
            Assert.AreEqual(1, theCreatedJournal.JournalLines.Count(r => r.ActionTypeCodeEnum == MyJournalActionTypeEnum.Debit));

            Assert.AreEqual(2, theCreatedJournal.JournalLines.Count(r => r.DebitAccountId == myBankAccountPM.TransferGLAcccountId));
            Assert.AreEqual(2, theCreatedJournal.JournalLines.Count(r => r.CreditAccountId == myBankAccountPM.GLAccountId));

            Assert.AreEqual(2, theCreatedJournal.JournalLines.Count(r => r.LocalAmount== myOrginalJournalTransaction.First().LocalAmountCredit));

            Assert.IsNotNull(theCreatedJournal.JournalExternalReconciles);
            Assert.AreEqual(1, theCreatedJournal.JournalExternalReconciles.Count);

            Assert.AreEqual(myReconcileExternalPageLinePM.Id, theCreatedJournal.JournalExternalReconciles[0].ReconcileExternalPageLineId);
            Assert.AreEqual(myOrginalJournalTransaction.First().Id, theCreatedJournal.JournalExternalReconciles[0].LedgerTransactionId);
        }



        [TestMethod]
        public void MoveBankCheckFromTransfer2GLAccount_foreign_GoodExample_CheckSuccess()
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


            IAccountingContext fakeIAccountingContext;
            fakeIAccountingContext = A.Fake<IAccountingContext>();

            var fakeExternalReconcileDataProvider = A.Fake<ExternalReconcileDataProvider>();


            A.CallTo(() =>
                   fakeExternalReconcileDataProvider.GetLedgerTransactionList(A<List<string>>.Ignored, newJournal.Tenant))
                   .Returns(myOrginalJournalTransaction);
            A.CallTo(() =>
                   fakeExternalReconcileDataProvider.GetReconcileExternalPageLinePM(_Tenant, myReconcileExternalPageLinePM.Id))
                   .Returns(myReconcileExternalPageLinePM);
            A.CallTo(() =>
                   fakeExternalReconcileDataProvider.GetaccountingCurrencyId(_Tenant))
                   .Returns("NIS");

            A.CallTo(() =>
       fakeExternalReconcileDataProvider.GetBankAccountFromTransferAccount(myLedgerTransactionTransferInCredit.AccountId, _Tenant))
       .Returns(myBankAccountPM);




            var myExternalReconcileJournalService = new ExternalReconcileMoveBankCheckFromTransfer2GLAccountService();
            myExternalReconcileJournalService.MustInit(fakeExternalReconcileDataProvider);
            myExternalReconcileJournalService.CreateJournalWithExtReconcile(_Tenant, myOrginalJournalTransaction.First().Id, myReconcileExternalPageLinePM.Id);

            Assert.IsNotNull(myExternalReconcileJournalService.TheJournalPM);
            var theCreatedJournal = myExternalReconcileJournalService.TheJournalPM;
            Assert.IsNotNull(theCreatedJournal.JournalLines);
            Assert.AreEqual(2, theCreatedJournal.JournalLines.Count);
            Assert.AreEqual(1, theCreatedJournal.JournalLines.Count(r => r.ActionTypeCodeEnum == MyJournalActionTypeEnum.Credit));
            Assert.AreEqual(1, theCreatedJournal.JournalLines.Count(r => r.ActionTypeCodeEnum == MyJournalActionTypeEnum.Debit));

            Assert.AreEqual(2, theCreatedJournal.JournalLines.Count(r => r.DebitAccountId == myBankAccountPM.TransferGLAcccountId));
            Assert.AreEqual(2, theCreatedJournal.JournalLines.Count(r => r.CreditAccountId == myBankAccountPM.GLAccountId));

            Assert.AreEqual(2, theCreatedJournal.JournalLines.Count(r => r.LocalAmount == myOrginalJournalTransaction.First().LocalAmountCredit));


            Assert.IsNotNull(theCreatedJournal.JournalExternalReconciles);
            Assert.AreEqual(1, theCreatedJournal.JournalExternalReconciles.Count);

            Assert.AreEqual(myReconcileExternalPageLinePM.Id, theCreatedJournal.JournalExternalReconciles[0].ReconcileExternalPageLineId);
            Assert.AreEqual(myOrginalJournalTransaction.First().Id, theCreatedJournal.JournalExternalReconciles[0].LedgerTransactionId);
        }

       
    }
}
