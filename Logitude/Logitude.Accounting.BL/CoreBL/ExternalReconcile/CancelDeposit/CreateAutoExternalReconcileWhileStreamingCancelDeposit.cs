using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.ExternalReconcile.CancelDeposit
{
    internal class CreateAutoExternalReconcileWhileStreamingCancelDeposit : WhileStreamingBase
    {

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
        public void CancelDeposit()
        {
            this.ExternalReconciliationList = new List<ExternalReconciliationPM>();
            if (_JournalPM.JournalExternalReconciles.Count != 1)
            {
                throw new ApplicationException("CancelDeposit only 1 JournalExternalReconciles");
            }
            var dJournalExternalReconcile = _JournalPM.JournalExternalReconciles[0];//
            if (string.IsNullOrWhiteSpace(dJournalExternalReconcile.LedgerTransactionId))
            {
                throw new ApplicationException("CancelDeposit LedgerTransactionId in null");
            }

            

            List<LedgerTransactionPM> allOldLedgerToReconcile = GetOldTransToReconcileThrowIfNotInProgress();
            var old2VoidLedgerTransactionDebitBank = allOldLedgerToReconcile.FirstOrDefault(r => r.Id == dJournalExternalReconcile.LedgerTransactionId);
            if (_JournalPM.Tenant != old2VoidLedgerTransactionDebitBank.Tenant)
            {
                throw new ApplicationException("_JournalPM.Tenant!= myOld2VoidLedgerTransactionDebitBank.Tenant");
            }
            var accountIds = allOldLedgerToReconcile.Select(r => r.AccountId).Distinct().ToList();
            if (accountIds.Count != 1)
            {
                throw new ApplicationException("All old ledger have to be in only 1 bank  account !!");
            }

            var newLedgerTransactionsDebitBank = _NewLedgerTransactionsWithCounters.Where(r => r.AccountId == accountIds.First()).ToList();
            if (newLedgerTransactionsDebitBank.Count != 1)
            {
                throw new ApplicationException("newLedgerTransactionsDebitBank have to be -one ledger !! with the same account id like ledger2void");
            }
            LedgerTransactionPM newDebitBankledgerTransactionPM = newLedgerTransactionsDebitBank.First();
            if (
                 Math.Abs(newDebitBankledgerTransactionPM.LocalAmountDebit + old2VoidLedgerTransactionDebitBank.LocalAmountDebit) != 0
                 ||     
                 Math.Abs(newDebitBankledgerTransactionPM.ForeignAmountDebit + old2VoidLedgerTransactionDebitBank.ForeignAmountDebit) != 0
                 ||
                 newDebitBankledgerTransactionPM.LocalAmountCredit != 0
                 ||
                 old2VoidLedgerTransactionDebitBank.LocalAmountCredit != 0
                 ||
                 newDebitBankledgerTransactionPM.ForeignAmountCredit != 0
                 ||
                 old2VoidLedgerTransactionDebitBank.ForeignAmountCredit != 0

                 )
            {
                throw new ApplicationException("The newDebitBankledgerTransactionPM is not balanced against the old2VoidLedgerTransactionDebitBank.");
            }
            if (newDebitBankledgerTransactionPM.AccountId != old2VoidLedgerTransactionDebitBank.AccountId)
            {
                throw new ApplicationException("The AccountId has to be the same in both newDebitBankledgerTransactionPM and old2VoidLedgerTransactionDebitBank.");
            }


            BankAccountPM bankAccount = GetBankAccountByAccountId(newDebitBankledgerTransactionPM.AccountId);

            AddExReconcileGLAccount_VoidAndStorno(
                dJournalExternalReconcile,
                newDebitBankledgerTransactionPM,
                bankAccount);
        }
        private BankAccountPM GetBankAccountByAccountId(string accountId)
        {
            BankAccountPM bankAccount =
                _ExternalReconcileDataProvider
                .GetBankAccountByGLAccountId(accountId, _JournalPM.Tenant);
            if (bankAccount == null)
                throw new ApplicationException("We need bank account id inorder to set in ExternalReconciliation !!");
            return bankAccount;
        }

        private void AddExReconcileGLAccount_VoidAndStorno(
            JournalExternalReconcilePM myJournalExternalReconcile,
            LedgerTransactionPM newLedgerTransactionsDebitBank,
            BankAccountPM bankAccount
            )
        {
            var cancelDepositExternalReconciliation = new ExternalReconciliationPM();
            cancelDepositExternalReconciliation.Tenant = _JournalPM.Tenant;
            cancelDepositExternalReconciliation.ChangeSetOp = ChangeSetOperation.Insert;
            cancelDepositExternalReconciliation.GLAccountId = newLedgerTransactionsDebitBank.AccountId;
            cancelDepositExternalReconciliation.Id = "new";
            cancelDepositExternalReconciliation.BankAccountId = bankAccount.Id;


            var reconcileLine_DebitPage = new ExternalReconciliationLinePM()
            {
                Tenant = _JournalPM.Tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                GroupNumber = 1,
                LedgerTransactionId = _JournalPM.JournalExternalReconciles[0].LedgerTransactionId,
                Line = 1,

                ExternalPageLineId = myJournalExternalReconcile.ReconcileExternalPageLineId,
            };
            cancelDepositExternalReconciliation.ExternalReconciliationLines.Add(reconcileLine_DebitPage);

            int l = 2;
            var reconcileLines_GLAccount_LedgerTransactionId =
            new ExternalReconciliationLinePM()
            {
                Tenant = _JournalPM.Tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                GroupNumber = 1,
                ReconciliationId = cancelDepositExternalReconciliation.Id,
                Line = l++,

                LedgerTransactionId = newLedgerTransactionsDebitBank.Id,
            };

            cancelDepositExternalReconciliation.ExternalReconciliationLines.Add(reconcileLines_GLAccount_LedgerTransactionId);
            this.ExternalReconciliationList.Add(cancelDepositExternalReconciliation);
        }



    }
}
