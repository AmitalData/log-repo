using Logitude.Accounting.BL.Validators;
using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.ExternalReconcile.Utils
{
    public enum ExternalReconcileType
    {
        MoveBankCheckFromTransferExternalReconcile,
        AdjustExternalReconcile
    }

    public class ExternalReconcileTypeService
    {
        private IExternalReconcileDataProvider _ExternalReconcileDataProvider;



       
        public void MustInit(IExternalReconcileDataProvider externalReconcileDataProvider)
        {
            _ExternalReconcileDataProvider = externalReconcileDataProvider;
        }
        public ExternalReconcileType GetExternalReconcileTypeFrom(int tenant, List<string> reconcileExternalPageLineIdList, List<string> ledgerTransactionIds)
        {
            if (reconcileExternalPageLineIdList.Count!=1 || ledgerTransactionIds.Count()!=1)
            {
                return ExternalReconcileType.AdjustExternalReconcile;
            }
            var ledgerTransactionList = _ExternalReconcileDataProvider.GetLedgerTransactionList(ledgerTransactionIds, tenant);

            var listOfpageLineList = _ExternalReconcileDataProvider.GetReconcileExternalPageLineList(tenant, reconcileExternalPageLineIdList);
            var reconcileExternalPageIdList = listOfpageLineList.Select(r => r.ReconcileExternalPageId).Distinct().ToList();
            var listOfpageList = _ExternalReconcileDataProvider.GetReconcileExternalPageList(tenant, reconcileExternalPageIdList);
            if (ledgerTransactionList.First().AccountId == listOfpageList.First().GLAccountId)
            {
                return ExternalReconcileType.AdjustExternalReconcile; 
            }
            return ExternalReconcileType.MoveBankCheckFromTransferExternalReconcile; 

        }
        public ExternalReconcileType GetExternalReconcileTypeFromJournal(JournalPM journalPM)
        {
            if (TypeIs_CreateAutoExternalReconcileMoveBankCheckFromTransfer(journalPM))
            {
                return ExternalReconcileType.MoveBankCheckFromTransferExternalReconcile;
            }
            return ExternalReconcileType.AdjustExternalReconcile;

        }

        private bool TypeIs_CreateAutoExternalReconcileMoveBankCheckFromTransfer(JournalPM journalPM)
        {
            if (journalPM.JournalExternalReconciles.Count() /*!=*/< 1)
            {
                return false;
            }


            var my1stJournalExternalReconcile = journalPM.JournalExternalReconciles.First();
            if (String.IsNullOrWhiteSpace(my1stJournalExternalReconcile.LedgerTransactionId))
            {
                return false;
            }
            var ledgerIds=journalPM.JournalExternalReconciles.Select(r => r.LedgerTransactionId).ToList();
            var listLedger =
            _ExternalReconcileDataProvider.GetLedgerTransactionList(ledgerIds, journalPM.Tenant);
            //var myLedgerTransactionTransferInCredit = listLedger.First();
            foreach (var myLedgerTransactionTransferInCredit in listLedger)
            {
                if (myLedgerTransactionTransferInCredit.LocalAmountDebit != 0)
                {
                    return false;// not in credit
                }
            }
            var accIds=listLedger.Select(r => r.AccountId).Distinct().ToList();
            if (accIds.Count>1)
            {
                return false;//all ledager have to be in TRansfer account !!!!
            }

            var BankAccountFromTransferAccount = _ExternalReconcileDataProvider.GetBankAccountFromTransferAccount(/*myLedgerTransactionTransferInCredit.AccountId*/accIds.First(), journalPM.Tenant);
            return BankAccountFromTransferAccount != null;
        }
    }
}
