using FakeItEasy;
using Logitude.Accounting.BL.CoreBL.ExternalReconcile;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.UnitTest.Accounting.UniTests
{
    [TestClass]
    public class CreateAutoExternalReconcileWhileStreamingServiceUnitTest
    {
        int _Tenant = 1;


        ///[TestMethod]
        public void CreateAutoEXTERNALReconcileWhileStreaming100_GoodExample_CheckSuccess()
        {
            BankAccountPM myBankAccountPM;
            ReconcileExternalPageLinePM myReconcileExternalPageLinePM;
            List<LedgerTransactionPM> myOrginalJournalTransaction, myNewLedgerTransactionList;
            LedgerTransactionPM myLedgerTransactionTransferInCredit;
            JournalPM newJournal;
            Example4AutoExternalReconcile(out myBankAccountPM, out myReconcileExternalPageLinePM, out myOrginalJournalTransaction, out myLedgerTransactionTransferInCredit, out newJournal, out myNewLedgerTransactionList);

            myOrginalJournalTransaction.ForEach(r => r.InProgressExternalReconcile = true);
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



            var myCreateAutoReconcileWhileStreamingService = A.Fake<CreateAutoExternalReconcileWhileStreamingService>(option => option.CallsBaseMethods());
            myCreateAutoReconcileWhileStreamingService
            .MustInit(fakeExternalReconcileDataProvider,
               newJournal,
            myNewLedgerTransactionList);
            myCreateAutoReconcileWhileStreamingService.CreateAutoExternalReconcileWhileStreaming();


            var ExternalReconciliationList = myCreateAutoReconcileWhileStreamingService.ExternalReconciliationList;
            Assert.IsNotNull(ExternalReconciliationList);
            Assert.AreEqual(2, ExternalReconciliationList.Count);

            var DebitCreditTransferGL = ExternalReconciliationList.First();

            Assert.AreEqual(2, DebitCreditTransferGL.ExternalReconciliationLines.Count);
            var myLedgerTransactionTransferInCreditId = DebitCreditTransferGL.ExternalReconciliationLines.First().LedgerTransactionId;
            Assert.AreEqual(myLedgerTransactionTransferInCredit.Id, myLedgerTransactionTransferInCreditId);
            var myNewLedgerTransactionTransferInDebitId = DebitCreditTransferGL.ExternalReconciliationLines.Last().LedgerTransactionId;
            Assert.AreEqual(myNewLedgerTransactionList.First(r => r.LocalAmountDebit > 0).Id, myNewLedgerTransactionTransferInDebitId);
            Assert.AreEqual(myBankAccountPM.TransferGLAcccountId, DebitCreditTransferGL.GLAccountId);

            bool notexistExternalPageLineId = DebitCreditTransferGL.ExternalReconciliationLines.Any(r => string.IsNullOrWhiteSpace(r.ExternalPageLineId));
            Assert.AreEqual(true, notexistExternalPageLineId);

            var DebitPage_CreditGLAccount = ExternalReconciliationList.Last();
            Assert.AreEqual(2, DebitPage_CreditGLAccount.ExternalReconciliationLines.Count);
            var pageLine = DebitPage_CreditGLAccount.ExternalReconciliationLines.First();
            Assert.AreEqual(myReconcileExternalPageLinePM.Id, pageLine.ExternalPageLineId);
            Assert.IsNull(pageLine.LedgerTransactionId);

            var GLAccountInCreditLine = DebitPage_CreditGLAccount.ExternalReconciliationLines.Last();
            Assert.IsNull(GLAccountInCreditLine.ExternalPageLineId);
            Assert.AreEqual("NewledgerTransaction:CreditAccountId", GLAccountInCreditLine.LedgerTransactionId);
            //Assert.AreEqual(myLedgerTransactionTransferInCredit.Id, myLedgerTransactionTransferInCreditId);
            //var myNewLedgerTransactionTransferInDebitId = DebitPage_CreditGLAccount.ExternalReconciliationLines.Last().LedgerTransactionId;
            //Assert.AreEqual(myNewLedgerTransactionList.First(r => r.LocalAmountDebit > 0).Id, myNewLedgerTransactionTransferInDebitId);
        }

        public static void Example4AutoExternalReconcile(out BankAccountPM myBankAccountPM, out ReconcileExternalPageLinePM myReconcileExternalPageLinePM, out List<LedgerTransactionPM> myOrginalJournalTransaction, out LedgerTransactionPM myLedgerTransactionTransferInCredit, out JournalPM newJournal, out List<LedgerTransactionPM> myNewLedgerTransactionList)
        {
            int _Tenant = 1;
            decimal myCheckLocalAmount = 102.1M;
            string OpenAmountCurrencyId = "usd";


            myBankAccountPM = new BankAccountPM()
            {
                Tenant = _Tenant,
                TransferGLAcccountId = "BankAccountPM:TransferGLAcccountId",
                GLAccountId = "BankAccountPM:GLAccountId",
                BankCode = "BankCode",
                BranchNumber = "BranchNumber",
                CurrencyId="NIS"

            };
            myReconcileExternalPageLinePM = new ReconcileExternalPageLinePM()
            {
                Tenant = _Tenant,

                Id = "MyReconcileExternalPageLineId",
                DebitAmount = myCheckLocalAmount,


            };




            myOrginalJournalTransaction = new List<LedgerTransactionPM>();
            myLedgerTransactionTransferInCredit = new LedgerTransactionPM()
            {
                Tenant = _Tenant,
                Id = "myLedgerTransactionTransferInCreditId",
                JournalId = "1",
                JournalLineNumber = 1,
                AccountId = myBankAccountPM.TransferGLAcccountId,
                OpenAmount = myCheckLocalAmount,
                //AmountToReconcile = 100,
                OpenAmountCurrencyId = OpenAmountCurrencyId,
                IsExternalReconcile = false,
                IsReconciled = false,
                LocalAmountCredit = myCheckLocalAmount,
            };
            myOrginalJournalTransaction.Add(myLedgerTransactionTransferInCredit);








            
            newJournal = new JournalPM()
            {
                Tenant = _Tenant,
                UpdatedByUserId = "itzik",
                JournalExternalReconciles = new List<JournalExternalReconcilePM>() {

                        new JournalExternalReconcilePM()
                        {
                            Tenant = _Tenant,
                             ReconcileExternalPageLineId= myReconcileExternalPageLinePM.Id,// "MyReconcileExternalPageLineId",
                            LedgerTransactionId=myOrginalJournalTransaction.First().Id
                        }

                    },
                JournalLines = new List<JournalLinePM>()
                 {
                      new JournalLinePM()
                      {
                          Line=1,
                          ActionTypeCodeEnum = JournalActionTypeEnum.Debit,
                          DebitAccountId = myBankAccountPM.TransferGLAcccountId,
                          CreditAccountId= myBankAccountPM.GLAccountId,

                          LocalAmount =myCheckLocalAmount,

                      },

                    new JournalLinePM()
                      {
                        Line=2,
                          ActionTypeCodeEnum = JournalActionTypeEnum.Credit,
                          DebitAccountId = myBankAccountPM.TransferGLAcccountId,
                          CreditAccountId= myBankAccountPM.GLAccountId,

                          LocalAmount =myCheckLocalAmount,

                      },


                }


            };

            myNewLedgerTransactionList = new List<LedgerTransactionPM>()
            {
                new LedgerTransactionPM()
            {
                Tenant = _Tenant,
                Id = "NewledgerTransaction:DebitAccountId",
                //JournalId = "1",
                //JournalLineNumber = 1,
                AccountId = newJournal.JournalLines.First().DebitAccountId,
                OpenAmount = myCheckLocalAmount,
                //AmountToReconcile = 100,
                OpenAmountCurrencyId = OpenAmountCurrencyId,
                IsExternalReconcile = false,
                IsReconciled = false,
                LocalAmountDebit = myCheckLocalAmount,
            },
                new LedgerTransactionPM()
            {
                Tenant = _Tenant,
                Id = "NewledgerTransaction:CreditAccountId",
                //JournalId = "1",
                //JournalLineNumber = 1,
                AccountId = newJournal.JournalLines.First().CreditAccountId,
                OpenAmount = myCheckLocalAmount,
                //AmountToReconcile = 100,
                OpenAmountCurrencyId = OpenAmountCurrencyId,
                IsExternalReconcile = false,
                IsReconciled = false,
                LocalAmountCredit = myCheckLocalAmount,
            },

            };
        }
    }
}