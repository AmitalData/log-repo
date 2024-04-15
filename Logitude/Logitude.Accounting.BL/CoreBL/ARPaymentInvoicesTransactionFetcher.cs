using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InvoiceModel.CloseTables;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.Accounting.BL.CoreBL
{
    public class ARPaymentInvoicesTransactionFetcher
    {
        int tenant;
        string paymentId;
        string glaccountId;

        LedgerTransaction paymentTransaction;
        List<LedgerTransactionPM> transactions;
        private ARInvoiceQuery aRInvoiceQuery;
        public ARPaymentInvoicesTransactionFetcher(string arpaymentId, string glaccountId, int tenant)
        {
            this.tenant = tenant;
            paymentId = arpaymentId;
            this.glaccountId = glaccountId;

            if(paymentId != null)
                paymentTransaction = GetPaymentTransaction();

            transactions = new List<LedgerTransactionPM>();
            aRInvoiceQuery = new ARInvoiceQuery(tenant);
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

            foreach (LedgerTransactionPM transaction in transactions)
            {
                string recoNumbers = GetReconciliationNumbersForTransaction(reconciliationLines, reconciliations, transaction);
                transaction.RecoNumber = recoNumbers;
            }

            return transactions;
        }
        private string GetReconciliationNumbersForTransaction(List<ReconciliationLinePM> reconciliationLines, List<ReconciliationPM> reconciliations, LedgerTransactionPM transactions)
        {
            List<ReconciliationLinePM> transactionRecoLines = reconciliationLines
                                .Where(d => d.TransactionId == transactions.Id).ToList();

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
            reconciledTransactions = reconciledTransactions.Where(d => d.IsReconciled == true && d.SourceTypeCode == CloseTables.AccountingEntityValues.ARInvoice).ToList();

            FillTransactionsAmountToReconcile(reconciledTransactions);

            reconciledTransactions = SetRefernceToLedgerTransaction(reconciledTransactions);

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
            IQueryable<LedgerTransactionPM> invoicesTransactions = transactionQueryService.GetInvoicesTransactions(tenant, AccountingEntities.ARInvoice);
            invoicesTransactions = invoicesTransactions
                    .Where(d =>
                        d.AccountId == glaccountId
                        && d.Tenant == tenant
                        && d.IsReconciled == false
                        && d.SourceTypeCode == CloseTables.AccountingEntityValues.ARInvoice)
                    .OrderBy(b => b.AccountingDate).ThenByDescending(b => b.JournalId);
         
            var transactionsList = invoicesTransactions.ToList();
            transactionsList = SetRefernceToLedgerTransaction(transactionsList);
            return transactionsList;
        }

        private List<LedgerTransactionPM> SetRefernceToLedgerTransaction(List<LedgerTransactionPM> transactionsList)
        {
            var transactionsListSourceId = transactionsList.Select(tr => tr.SourceId).ToList();
            List<ARInvoicePM> aRInvoicePMList = aRInvoiceQuery.GetARInvoicePMsByIdList(transactionsListSourceId, tenant).ToList();
            
            foreach(LedgerTransactionPM ledgerTransaction in transactionsList)
            {
                ARInvoicePM aRInvoicePM = aRInvoicePMList.Where(arInvoice => arInvoice.Id == ledgerTransaction.SourceId).FirstOrDefault();
                if(aRInvoicePM != null)
                {
                    ledgerTransaction.Reference3 = aRInvoicePM.ShipmentsNumbers;
                }
            }
           
            return transactionsList;
        }

        private LedgerTransaction GetPaymentTransaction()
        {
            JournalPM paymentJournal = GetPaymentJournal(paymentId);
            if(paymentJournal != null)
            {
                LedgerTransaction paymentCreditTransaction = GetCreditTransactionByJournalId(paymentJournal.Id);

                if (paymentCreditTransaction == null)
                {
                    paymentId = null;
                    //throw new ApplicationException("[ARPaymentInvoicesTransactionFetcher] Couldn't found payment transaction!");
                }
                return paymentCreditTransaction;

            }
            else
            {
                return null;
            }


        }

        private JournalPM GetPaymentJournal(string arpaymentId)
        {
            JournalQueryService journalQueryService = new JournalQueryService(tenant);
            JournalPM paymentJournal = journalQueryService.GetJournalsByAccountingEntityIdAndCode(arpaymentId, AccountingEntities.ARPayment, tenant).FirstOrDefault();
            return paymentJournal;
        }

        public LedgerTransaction GetCreditTransactionByJournalId(string journalId)
        {
            LedgerTransactionRepository transactionRepository = new LedgerTransactionRepository(tenant);
            IQueryable<LedgerTransaction> ledgerTransactionPOCOs = transactionRepository.GetByJournalId(journalId, tenant);

            LedgerTransaction poco = ledgerTransactionPOCOs.Where(d => d.LocalAmountCredit != 0).FirstOrDefault();
            return poco;
        }

        private void SortTransactions()
        {
            transactions = transactions.OrderByDescending(d => d.IsReconciled).ThenByDescending(d => d.PaymentReconciledAmount).ToList();
        }
    }   

}
