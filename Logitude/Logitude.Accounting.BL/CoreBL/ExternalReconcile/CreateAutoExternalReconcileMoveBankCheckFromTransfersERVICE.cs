#if false
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.ExternalReconcile
{
    class CreateAutoExternalReconcileMoveBankCheckFromTransferService
        : WhileStreamingBase
    {

        public void MoveBankCheckFromTransfer()
        {
            this.ExternalReconciliationList = new List<ExternalReconciliationPM>();
            if (_JournalPM.JournalExternalReconciles.Count > 1)
            {
                throw new ApplicationException("Sorry (MoveBankCheckFromTransfer) meanwhile only one Adjust Allowed !!!");
                //errorsList.Add(TranslateMyTextCode("Sorry meanwhile only one Adjust Allowed !!!", myJournalPM.Tenant));
            }
            var myJournalExternalReconcile = _JournalPM.JournalExternalReconciles[0];//meanwhile only one Adjust Allowed !!!

            List<LedgerTransactionPM> myOldTransToReconcile = GetOldTransToReconcileThrowIfNotInProgress();


            var myLedgerTransactionTransferInCredit = myOldTransToReconcile.FirstOrDefault(r => r.Id == myJournalExternalReconcile.LedgerTransactionId);
            if (_JournalPM.Tenant != myLedgerTransactionTransferInCredit.Tenant)
            {
                throw new ApplicationException("_JournalPM.Tenant!= myLedgerTransactionTransferInCredit.Tenant");
            }


            BankAccountPM bankAccountFromTransfer = _ExternalReconcileDataProvider.GetBankAccountFromTransferAccount(myLedgerTransactionTransferInCredit.AccountId, _JournalPM.Tenant);
            BankAccountPM bankAccountFromPage = _ExternalReconcileDataProvider.GetBankAccountFromReconcileExternalPageLineId(myJournalExternalReconcile.ReconcileExternalPageLineId, _JournalPM.Tenant);
            if (bankAccountFromTransfer.Id != bankAccountFromPage.Id)
            {
                throw new ApplicationException("bankAccountFromTransfer.Id != bankAccountFromPage.Id");
            }


            LedgerTransactionPM myNewLedgerTransactionTransferInDebit = GetTheNewLedgerTransactionTransferInDebit(myLedgerTransactionTransferInCredit);
            AddExReconcile_DebitCreditTransferGL(myLedgerTransactionTransferInCredit, myNewLedgerTransactionTransferInDebit);


            LedgerTransactionPM myNewLedgerTransactionBankGLAccountInCredit = GetTheNewLedgerTransactionBankGLAccountInCredit(bankAccountFromTransfer);
            AddExReconcile_DebitPage_CreditGLAccount(myJournalExternalReconcile, myNewLedgerTransactionBankGLAccountInCredit);
        }


        private void AddExReconcile_DebitPage_CreditGLAccount(JournalExternalReconcilePM myJournalExternalReconcile, LedgerTransactionPM myNewLedgerTransactionBankGLAccountInCredit)
        {
            var reconcile_DebitPage_CreditGLAccount = new ExternalReconciliationPM();
            reconcile_DebitPage_CreditGLAccount.Tenant = _JournalPM.Tenant;
            reconcile_DebitPage_CreditGLAccount.ChangeSetOp = ChangeSetOperation.Insert;
            reconcile_DebitPage_CreditGLAccount.GLAccountId = myNewLedgerTransactionBankGLAccountInCredit.AccountId;
            reconcile_DebitPage_CreditGLAccount.Id = "new";

            LedgerTransactionPM transferAccountTransaction = GetLedgerTransactionById(myJournalExternalReconcile.LedgerTransactionId);

            BankAccountPM bankAccount = GetBankAccountByTransferAccountId(transferAccountTransaction.AccountId);
            reconcile_DebitPage_CreditGLAccount.BankAccountId = bankAccount.Id;


            var reconcileLine_DebitPage = new ExternalReconciliationLinePM()
            {
                Tenant = _JournalPM.Tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                GroupNumber = 1,
                ReconciliationId = reconcile_DebitPage_CreditGLAccount.Id,
                Line = 1,

                ExternalPageLineId = myJournalExternalReconcile.ReconcileExternalPageLineId,
            };
            reconcile_DebitPage_CreditGLAccount.ExternalReconciliationLines.Add(reconcileLine_DebitPage);


            var reconcileLine_CreditGLAccount = new ExternalReconciliationLinePM()
            {
                Tenant = _JournalPM.Tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                GroupNumber = 1,
                ReconciliationId = reconcile_DebitPage_CreditGLAccount.Id,
                Line = 2,

                LedgerTransactionId = myNewLedgerTransactionBankGLAccountInCredit.Id,
            };

            reconcile_DebitPage_CreditGLAccount.ExternalReconciliationLines.Add(reconcileLine_CreditGLAccount);
            this.ExternalReconciliationList.Add(reconcile_DebitPage_CreditGLAccount);
        }

        private LedgerTransactionPM GetLedgerTransactionById(string id)
        {
            LedgerTransactionQueryService transactionQueryService = new LedgerTransactionQueryService(_JournalPM.Tenant);
            LedgerTransactionPM transferLedgerTransaction = transactionQueryService.GetSingle(id, false, false);
            return transferLedgerTransaction;
        }

        private List<LedgerTransactionPM> GetOldTransToReconcileThrowIfNotInProgress()
        {
            var theReconcileAgainstLTranIdList = _JournalPM.JournalExternalReconciles.Select(r => r.LedgerTransactionId).ToList();
            List<LedgerTransactionPM> myOldTransToReconcile = _ExternalReconcileDataProvider.GetLedgerTransactionList(theReconcileAgainstLTranIdList, _JournalPM.Tenant);
            if (!myOldTransToReconcile.Any())
            {
                throw new ApplicationException("!myOldTransToReconcile.Any()");
            }
            if (myOldTransToReconcile.Any(r => !r.InProgressExternalReconcile))
            {
                throw new ApplicationException("_JournalPM.JournalReconciles have  myOldTransToReconcile.Any( r=> !r.InProgressExternalReconcile) ");
            }

            return myOldTransToReconcile;
        }

        private LedgerTransactionPM GetTheNewLedgerTransactionBankGLAccountInCredit(BankAccountPM bankAccountPM)
        {
            var myNewLedgerTransactionBankGLAccountInCredit = _NewLedgerTransactionsWithCounters.FirstOrDefault(r => r.AccountId == bankAccountPM.GLAccountId && r.LocalAmountCredit > 0);
            if (myNewLedgerTransactionBankGLAccountInCredit == null)
            {
                throw new ApplicationException("Could not found the new Ledger in BankGLAccount In credit !?  התנועה לא נוצרה ");
            }

            return myNewLedgerTransactionBankGLAccountInCredit;
        }

        private LedgerTransactionPM GetTheNewLedgerTransactionTransferInDebit(LedgerTransactionPM myLedgerTransactionTransferInCredit)
        {
            var myNewLedgerTransactionTransferInDebit = _NewLedgerTransactionsWithCounters.FirstOrDefault(r => r.AccountId == myLedgerTransactionTransferInCredit.AccountId && r.LocalAmountDebit > 0);
            if (myNewLedgerTransactionTransferInDebit == null)
            {
                throw new ApplicationException("Could not found the new Ledger in TransferGL In Debit !?   התנועה לא נוצרה ");
            }

            return myNewLedgerTransactionTransferInDebit;
        }



        private void AddExReconcile_DebitCreditTransferGL(LedgerTransactionPM myLedgerTransactionTransferInCredit, LedgerTransactionPM myNewLedgerTransactionTransferInDebit)
        {
            var reconcile_DebitCreditTransferGL = new ExternalReconciliationPM();
            reconcile_DebitCreditTransferGL.Tenant = _JournalPM.Tenant;
            reconcile_DebitCreditTransferGL.ChangeSetOp = ChangeSetOperation.Insert;
            reconcile_DebitCreditTransferGL.GLAccountId = myLedgerTransactionTransferInCredit.AccountId;
            reconcile_DebitCreditTransferGL.Id = "new";

            BankAccountPM bankAccount = GetBankAccountByTransferAccountId(myLedgerTransactionTransferInCredit.AccountId);
            reconcile_DebitCreditTransferGL.BankAccountId = bankAccount.Id;

            //reconcile_DebitCreditTransferGL.AccountCurrencyId = myLedgerTransactionTransferInCredit.CurrencyId;
            //reconcile_DebitCreditTransferGL.CreatedByUserId = _JournalPM.CreatedByUserId;
            //reconcile_DebitCreditTransferGL.CreateDate = 


            var reconcileLine_CreditTransferGL = new ExternalReconciliationLinePM()
            {
                Tenant = _JournalPM.Tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                GroupNumber = 1,
                ReconciliationId = reconcile_DebitCreditTransferGL.Id,
                Line = 1,

                LedgerTransactionId = myLedgerTransactionTransferInCredit.Id,
            };
            reconcile_DebitCreditTransferGL.ExternalReconciliationLines.Add(reconcileLine_CreditTransferGL);

            var reconcileLine_DebitTransferGL = new ExternalReconciliationLinePM()
            {
                Tenant = _JournalPM.Tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                GroupNumber = 1,
                ReconciliationId = reconcile_DebitCreditTransferGL.Id,
                Line = 2,

                LedgerTransactionId = myNewLedgerTransactionTransferInDebit.Id,
            };

            reconcile_DebitCreditTransferGL.ExternalReconciliationLines.Add(reconcileLine_DebitTransferGL);
            this.ExternalReconciliationList.Add(reconcile_DebitCreditTransferGL);
        }

        private BankAccountPM GetBankAccountByTransferAccountId(string transferAccountId)
        {
            


            BankAccountQueryService bankAccountQuery = new BankAccountQueryService(_JournalPM.Tenant);
            BankAccountPM bankAccount = //bankAccountQuery.GetBankAccountByTransferGLAcccountId(transferAccountId, _JournalPM.Tenant);
                _ExternalReconcileDataProvider.GetBankAccountFromTransferAccount(transferAccountId, _JournalPM.Tenant);
            if (bankAccount == null)
                throw new ApplicationException("We need bank account id inorder to reconcile transfer transaction!!");
            return bankAccount;
        }
    }
}


#endif