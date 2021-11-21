using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.ExternalReconcile
{
    class CreateAutoExternalReconcileWhileStreamingFeesService: WhileStreamingBase
    {
        public void AdjustBankFees()
        {
            // must have rows with 

            //if (!_JournalPM.JournalExternalReconciles.TrueForAll(r => string.IsNullOrWhiteSpace(r.LedgerTransactionId)))
            if (_JournalPM.JournalExternalReconciles.Any(r=> string.IsNullOrWhiteSpace(r.ReconcileExternalPageLineId) && string.IsNullOrWhiteSpace(r.LedgerTransactionId)))
            {
                throw new Exception("there is line with empty ReconcileExternalPageLineId && empty LedgerTransactionId");
            }
            if (!_JournalPM.JournalExternalReconciles.Where(r => !string.IsNullOrWhiteSpace(r.ReconcileExternalPageLineId) && string.IsNullOrWhiteSpace(r.LedgerTransactionId)).Any())
            {
                throw new Exception("at least one JournalExternalReconciles LedgerTransactionId must be empty   ");
            }

            List<ReconcileExternalPageList> listOfpageList = RecheckAndGetListOfpageList();

            GLAccountList bankGLAccountList = GetbankGLAccountList(listOfpageList);




            this.ExternalReconciliationList = new List<ExternalReconciliationPM>();

            ExternalReconciliationPM reconcile_BankFees = CreateReconcile_BankFees(bankGLAccountList);
            this.ExternalReconciliationList.Add(reconcile_BankFees);

        }

        private List<ReconcileExternalPageList> RecheckAndGetListOfpageList()
        {
            List<string> reconcileExternalPageLineIdList = _JournalPM.JournalExternalReconciles.Where(r => !String.IsNullOrWhiteSpace(r.ReconcileExternalPageLineId)).Select(r => r.ReconcileExternalPageLineId).ToList();
            //reconcileExternalPageLineIdList
            IExternalReconcileAdjustBankFees_Validate myExternalReconcileAdjustBankFeesService = new ExternalReconcileAdjustBankFeesService();
            myExternalReconcileAdjustBankFeesService.MustInit(_ExternalReconcileDataProvider);

            string adjustGLAccountId = CreateAutoExternalReconcileWhileStreamingService.GetAdjustGLAccountId(_JournalPM);

            List<ReconcileExternalPageLineList> listOfpageLineList;
            List<ReconcileExternalPageList> listOfpageList;
            bool CheckWhileStreaming = true;
            List<string> ledgerTransactionIds= _JournalPM.JournalExternalReconciles.Where(r => !String.IsNullOrWhiteSpace(r.LedgerTransactionId)).Select(r => r.LedgerTransactionId).ToList();

            var skipValidation = _JournalPM.JournalExternalReconciles.Any(r => r.SkipAccountsValidation == true);

            myExternalReconcileAdjustBankFeesService.PrapareAndValid(_JournalPM.Tenant, reconcileExternalPageLineIdList, adjustGLAccountId, out listOfpageLineList, out listOfpageList, CheckWhileStreaming,
                 
                ledgerTransactionIds,
                out string accountingCurrencyId, out List<LedgerTransactionPM> ledgerTransactionList, skipValidation
                );

            if (listOfpageLineList.Any(r => !r.InProgressExternalReconcile))
            {
                throw new Exception("All connected ReconcileExternalPageLineList  must be InProgress");
            }

            return listOfpageList;
        }

        private ExternalReconciliationPM CreateReconcile_BankFees(GLAccountList bankGLAccountList)
        {
            ExternalReconciliationPM reconcile_BankFees = CreateHeaderExternalReconciliation(bankGLAccountList);

            int line = 1;

            line = AddFromUserSelected_ExternalReconciliationLines(reconcile_BankFees, line);

            AddFrom_NewLedger_OfBankGLAccount(bankGLAccountList, reconcile_BankFees, line);
            return reconcile_BankFees;
        }

        private ExternalReconciliationPM CreateHeaderExternalReconciliation(GLAccountList bankGLAccountList)
        {
            var reconcile_BankFees = new ExternalReconciliationPM();
            reconcile_BankFees.Tenant = _JournalPM.Tenant;
            reconcile_BankFees.ChangeSetOp = ChangeSetOperation.Insert;
            reconcile_BankFees.GLAccountId = bankGLAccountList.Id;
            reconcile_BankFees.Id = "new";
            return reconcile_BankFees;
        }

        private void AddFrom_NewLedger_OfBankGLAccount(GLAccountList bankGLAccountList, ExternalReconciliationPM reconcile_BankFees, int line)
        {
            var bankGLAccountNewLedgerList = _NewLedgerTransactionsWithCounters.Where(r => r.AccountId == bankGLAccountList.Id).ToList();
            var reconcileLineList_NewbankGLAccountLedger =
            bankGLAccountNewLedgerList.Select(
                bankGLAccountNewLedger =>
                    new ExternalReconciliationLinePM()
                    {
                        Tenant = _JournalPM.Tenant,
                        ChangeSetOp = ChangeSetOperation.Insert,
                        GroupNumber = 1,
                        ReconciliationId = reconcile_BankFees.Id,
                        Line = line++,

                        LedgerTransactionId = bankGLAccountNewLedger.Id,
                    }
                );


            reconcile_BankFees.ExternalReconciliationLines.AddRange(reconcileLineList_NewbankGLAccountLedger);
        }

        private int AddFromUserSelected_ExternalReconciliationLines(ExternalReconciliationPM reconcile_BankFees, int line)
        {
            reconcile_BankFees.ExternalReconciliationLines.AddRange(_JournalPM.JournalExternalReconciles.Select(r =>
            new ExternalReconciliationLinePM()
            {
                Tenant = _JournalPM.Tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                GroupNumber = 1,
                ReconciliationId = reconcile_BankFees.Id,
                Line = line++,
                ExternalPageLineId = r.ReconcileExternalPageLineId,
                LedgerTransactionId = r.LedgerTransactionId,
            })
            );
            return line;
        }

        private GLAccountList GetbankGLAccountList(List<ReconcileExternalPageList> listOfpageList)
        {
            GLAccountList bankGLAccountList;

            List<GLAccountList> ListOfGLAccountList = _ExternalReconcileDataProvider.GetListOfGLAccountList(_JournalPM.Tenant, listOfpageList.Select(r => r.GLAccountId).Distinct().ToList());
            bankGLAccountList = ListOfGLAccountList.First();
            return bankGLAccountList;
        }

    }
}
