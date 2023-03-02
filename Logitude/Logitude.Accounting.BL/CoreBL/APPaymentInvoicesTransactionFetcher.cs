using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.Accounting.BL.CoreBL
{
    public class APPaymentInvoicesTransactionFetcher
    {
        int tenant;
        string paymentId;
        string glaccountId;
        bool _excludeCancelledReconciliations = false;
        private LedgerTransaction paymentTransaction;
        public LedgerTransactionPM paymentTransactionPM;
        List<LedgerTransactionPM> transactions;

        public APPaymentInvoicesTransactionFetcher(string appaymentId, string glaccountId, int tenant, bool? excludeCancelledReconciliations = null)
        {
            this.tenant = tenant;
            paymentId = appaymentId;
            this.glaccountId = glaccountId;

            if(paymentId != null)
                paymentTransaction = GetPaymentTransaction();

            if (paymentTransaction != null)
            {
                LedgerTransactionQueryService transactionsQuery = new LedgerTransactionQueryService(tenant);
                paymentTransactionPM = transactionsQuery.GetEntityPM(paymentTransaction);
            }

            transactions = new List<LedgerTransactionPM>();
            _excludeCancelledReconciliations = excludeCancelledReconciliations != null && excludeCancelledReconciliations == true;
        }


        public List<LedgerTransactionPM> FetchSorted()
        {
            transactions.AddRange(GetReconciledTransactions());
            transactions.AddRange(GetOpenTransactions());

            SortTransactions();

            return transactions;
        }





        private List<LedgerTransactionPM> GetReconciledTransactions()
        {
            var reconciledTransactions = new List<LedgerTransactionPM>();
            if (paymentTransaction != null)
            {
                reconciledTransactions = GetReconciledInvoicesTransactions();

                reconciledTransactions = FillReconciliationNumbersOnTransactions(reconciledTransactions);

                reconciledTransactions = FillReconciledPaymentAmountOnTransaction(reconciledTransactions);

            }

            return reconciledTransactions;
        }

        private List<LedgerTransactionPM> FillReconciliationNumbersOnTransactions(List<LedgerTransactionPM> transactions)
        {
            List<ReconciliationLinePM> reconciliationLines = GetReconciliationLinesForTransactions(transactions);

            List<ReconciliationPM> reconciliations = GetReconciliationsByReconcileLines(tenant, reconciliationLines);
            if (_excludeCancelledReconciliations) {
                reconciliations = reconciliations.Where(x => !x.IsCancelled).ToList();
            }

            string APPaymentRecoNumbers = GetReconciliationNumbersForAPPayment(reconciliationLines, reconciliations, paymentTransaction?.Id);
            foreach (LedgerTransactionPM transaction in transactions)
            {
                string recoNumbers = GetReconciliationNumbersForTransaction(reconciliationLines, reconciliations, transaction.Id);
                transaction.RecoNumber = recoNumbers;
                transaction.Reference3 = paymentId != null ? APPaymentRecoNumbers : null;
            }
            return transactions;
        }
        private string GetReconciliationNumbersForTransaction(List<ReconciliationLinePM> reconciliationLines, List<ReconciliationPM> reconciliations, string transactionId)
        {
            List<ReconciliationLinePM> transactionRecoLines = reconciliationLines
                                .Where(d => d.TransactionId == transactionId).ToList();

            List<string> reconciliationsId = transactionRecoLines.Select(a => a.ReconciliationId).ToList();
            List<ReconciliationPM> reconciliationsForTransaction = reconciliations.Where(d => reconciliationsId.Contains(d.Id)).ToList();

            string[] reconciliationsNumbersForTransaction = reconciliationsForTransaction
                .Where(d => d.IsCancelled == false).Select(d => d.Number).ToArray();

            string numbersString = string.Join(",", reconciliationsNumbersForTransaction);
            return numbersString;
        }

        private string GetReconciliationNumbersForAPPayment(List<ReconciliationLinePM> reconciliationLines, List<ReconciliationPM> reconciliations, string transactionId)
        {
            List<ReconciliationLinePM> transactionRecoLines = new List<ReconciliationLinePM>();
            if (transactionId != null)
            {
                transactionRecoLines = reconciliationLines
                                    .Where(d => d.ReconciledWithTransactionId == transactionId).ToList();
            }
            else {
                //transactionRecoLines = reconciliationLines.ToList();
            }
            List<string> reconciliationsId = transactionRecoLines.Select(a => a.ReconciliationId).ToList();
            List<ReconciliationPM> reconciliationsForTransaction = reconciliations.Where(d => reconciliationsId.Contains(d.Id)).ToList();

            string[] reconciliationsNumbersForTransaction = reconciliationsForTransaction
                .Where(d => d.IsCancelled == false).Select(d => d.Number).ToArray();

            string numbersString = string.Join(",", reconciliationsNumbersForTransaction);
            return numbersString;
        }

        private List<ReconciliationPM> GetReconciliationsByReconcileLines(int tenant, List<ReconciliationLinePM> recoLines)
        {
            List<string> recosIds = recoLines.Select(d => d.ReconciliationId).ToList();
            ReconciliationQueryService recoQuery = new ReconciliationQueryService(tenant);
            List<ReconciliationPM> recos = recoQuery.GetLightReconciliationsByIds(recosIds, tenant);

            return recos;
        }

        private List<ReconciliationLinePM> GetReconciliationLinesForTransactions(List<LedgerTransactionPM> invoicesTransactions)
        {
            List<string> transactionsIds = invoicesTransactions.Select(d => d.Id).ToList();

            ReconciliationLineQueryService recoLineQuery = new ReconciliationLineQueryService(tenant);
            List<ReconciliationLinePM> recoLines = recoLineQuery.GetLinesByTransactionIdsWithoutMapping(transactionsIds, tenant).ToList();
            return recoLines;
        }

        private List<LedgerTransactionPM> GetReconciledInvoicesTransactions()
        {
            List<LedgerTransactionPM> reconciledTransactions;
            List<ReconciliationLinePM> recoLinesOfPayment = GetPaymentReconciliationLines();

            reconciledTransactions = GetReconciledInvoicesTransactionsConnectedToReconciliationLines(recoLinesOfPayment);
            return reconciledTransactions;
        }

        private List<LedgerTransactionPM> GetReconciledInvoicesTransactionsConnectedToReconciliationLines(List<ReconciliationLinePM> recoLinesOfPayment)
        {
            List<LedgerTransactionPM> reconciledTransactions;
            List<string> recoLinesTransactionsId = recoLinesOfPayment.Select(d => d.TransactionId).ToList();

            reconciledTransactions = GetTransactionsById(recoLinesTransactionsId);

            // exclude partially reconcile transactions
            reconciledTransactions = reconciledTransactions.Where(d => d.SourceTypeCode == CloseTables.AccountingEntityValues.APInvoice).ToList();

            FillTransactionsAmountToReconcile(reconciledTransactions);

            return reconciledTransactions;
        }

        private static void FillTransactionsAmountToReconcile(List<LedgerTransactionPM> reconciledTransactions)
        {
            reconciledTransactions.ForEach(transaction =>
            {
                var foreignAmount = transaction.ForeignAmountCredit == 0 ? transaction.ForeignAmountDebit : transaction.ForeignAmountCredit;
            });
        }

        private List<LedgerTransactionPM> GetTransactionsById(List<string> ids)
        {
            List<LedgerTransactionPM> reconciledTransactions;
            LedgerTransactionQueryService transactionsQuery = new LedgerTransactionQueryService(tenant);
            reconciledTransactions = transactionsQuery.GetLedgerTransactionPMsByIdList(ids, tenant);
            return reconciledTransactions;
        }

        private List<ReconciliationLinePM> GetPaymentReconciliationLines()
        {
            ReconciliationLineQueryService recoLineQuery = new ReconciliationLineQueryService(tenant);
            List<ReconciliationLinePM> recoLines = recoLineQuery.GetLinesByReconciledWithTransactionIdWithoutMapping(paymentTransaction.Id, tenant);
            return recoLines;
        }






        private List<LedgerTransactionPM> GetOpenTransactions()
        {
            List<LedgerTransactionPM> openTransactions = GetInvoicesTransactions();

            openTransactions = FillReconciliationNumbersOnTransactions(openTransactions);

            openTransactions = FillReconciledPaymentAmountOnTransaction(openTransactions);


            return openTransactions;
        }

        private List<LedgerTransactionPM> FillReconciledPaymentAmountOnTransaction(List<LedgerTransactionPM> transactions)
        {
            if (paymentId != null)
            {
                List<ReconciliationLinePM> reconciliationLines = GetReconciliationLinesForTransactions(transactions);

                foreach (LedgerTransactionPM transaction in transactions)
                {
                    List<ReconciliationLinePM> transactionRecoLines = reconciliationLines.Where(d => d.TransactionId == transaction.Id).ToList();

                    decimal reconciledAmount = 0;
                    transactionRecoLines.ForEach(recoLine =>
                    {
                    if (paymentTransaction != null &&recoLine.ReconciledWithTransactionId == paymentTransaction.Id 
                    && paymentTransaction.Id != null && recoLine.IsRecoCancelled == false)
                            reconciledAmount += recoLine.ReconciliationAmount;
                    });

                    transaction.PaymentReconciledAmount = reconciledAmount;

                }
            }
            else
            {
                transactions.ForEach(transaction =>
                {
                    transaction.PaymentReconciledAmount = 0;
                });
            }

            return transactions;
        }

        private List<LedgerTransactionPM> GetInvoicesTransactions()
        {
            LedgerTransactionQueryService transactionQueryService = new LedgerTransactionQueryService(tenant);
            IQueryable<LedgerTransactionPM> invoicesTransactions = transactionQueryService.GetInvoicesTransactions(tenant, AccountingEntities.APInvoice);
            invoicesTransactions = invoicesTransactions
                    .Where(d =>
                        d.AccountId == glaccountId
                        && d.Tenant == tenant
                        && d.IsReconciled == false
                        && d.SourceTypeCode == CloseTables.AccountingEntityValues.APInvoice)
                    .OrderBy(b => b.AccountingDate).ThenByDescending(b => b.JournalId);

            var transactionsList = invoicesTransactions.ToList();
            return transactionsList;
        }

        private LedgerTransaction GetPaymentTransaction()
        {
            JournalPM paymentJournal = GetPaymentJournal(paymentId);
            if(paymentJournal != null)
            {
                LedgerTransaction paymentDebitTransaction = GetDebitTransactionByJournalId(paymentJournal.Id);

                if (paymentDebitTransaction == null)
                {
                    paymentId = null;
                    //throw new ApplicationException("[APPaymentInvoicesTransactionFetcher] Couldn't found payment transaction!");
                }
                return paymentDebitTransaction;

            }
            else
            {
                return null;
            }


        }

        private JournalPM GetPaymentJournal(string appaymentId)
        {
            JournalQueryService journalQueryService = new JournalQueryService(tenant);
            JournalPM paymentJournal = journalQueryService.GetJournalsByAccountingEntityIdAndCode(appaymentId, AccountingEntities.APPayment, tenant).FirstOrDefault();
            return paymentJournal;
        }

        public LedgerTransaction GetDebitTransactionByJournalId(string journalId)
        {
            LedgerTransactionRepository transactionRepository = new LedgerTransactionRepository(tenant);
            IQueryable<LedgerTransaction> ledgerTransactionPOCOs = transactionRepository.GetByJournalId(journalId, tenant);

            LedgerTransaction poco = ledgerTransactionPOCOs.Where(d => d.LocalAmountDebit != 0).FirstOrDefault();
            return poco;
        }

        private void SortTransactions()
        {
            transactions = transactions.OrderByDescending(d => d.IsReconciled).ThenByDescending(d => d.PaymentReconciledAmount).ToList();
        }
    }   

}
