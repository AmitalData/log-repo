using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.Reconcile
{
    public class MultipleARPaymentReconciliationSplitter
    {
        int tenant;
        ReconciliationPM reconciliationPM;
        List<LedgerTransactionPM> recoTransactions;
        List<ReconciliationLinePM> paymentsRecoLines;
        List<ReconciliationLinePM> remainingPaymentRecoLines;

        public MultipleARPaymentReconciliationSplitter(ReconciliationPM reconcileToSplit)
        {
            tenant = reconcileToSplit.Tenant;

            reconciliationPM = reconcileToSplit;
            recoTransactions = GetReconcileTransactions();
        }

        public List<ReconciliationPM> Split()
        {
            remainingPaymentRecoLines = GetPaymentReconciliationLines();
            List<ReconciliationLinePM> paymentsRecoLines = GetPaymentReconciliationLines();
            List<ReconciliationLinePM> nonPaymentsRecoLines = GetNonPaymentReconciliationLines();
            List<ReconciliationPM> paymentReconciliations = new List<ReconciliationPM>();

            foreach (ReconciliationLinePM paymentReconcileLine in paymentsRecoLines)
            {
                remainingPaymentRecoLines.Remove(paymentReconcileLine);
                ReconciliationPM newReconciliation = CreateReconciliationForPayment(nonPaymentsRecoLines, paymentReconcileLine);
                paymentReconciliations.Add(newReconciliation);

                if(nonPaymentsRecoLines.Count == 0 && paymentsRecoLines.Count == 2)
                {
                    ReconciliationPM twoPaymentReconciliation = InitNewReconciliation();
                    twoPaymentReconciliation.ReconciliationLines.AddRange(paymentsRecoLines);
                    paymentReconciliations.Add(twoPaymentReconciliation);
                    break;
                }


            }



            return paymentReconciliations;
        }

        ReconciliationPM CreateReconciliationForPayment(List<ReconciliationLinePM> nonPaymentsRecoLines, ReconciliationLinePM paymentReconcileLine)
        {
            ReconciliationPM newReconciliation = InitNewReconciliation();
            newReconciliation.ReconciliationLines.Add(paymentReconcileLine);
            decimal paymentAmount2Reconcile = paymentReconcileLine.ReconciliationAmount;
            List<ReconciliationLinePM> oppositePaymentsRecoLines = GetNonUsedPaymentRecoLines(paymentReconcileLine);

            while (paymentAmount2Reconcile != 0)
            {
                ReconciliationLinePM oppositeRecoLine = nonPaymentsRecoLines.Count > 0 ? nonPaymentsRecoLines.First() : oppositePaymentsRecoLines.First();

                if (oppositeRecoLine != null) {

                    if (Math.Abs(oppositeRecoLine.ReconciliationAmount) > Math.Abs(paymentAmount2Reconcile))
                    {
                        SliceAndAddRecoLine(newReconciliation, paymentAmount2Reconcile, oppositeRecoLine);
                        paymentAmount2Reconcile = 0;
                    }
                    else
                    {
                        newReconciliation.ReconciliationLines.Add(oppositeRecoLine);
                        nonPaymentsRecoLines.Remove(oppositeRecoLine);

                        if (paymentReconcileLine.ReconciliationAmount > 0)
                            paymentAmount2Reconcile -= Math.Abs(oppositeRecoLine.ReconciliationAmount);
                        else
                            paymentAmount2Reconcile += Math.Abs(oppositeRecoLine.ReconciliationAmount);
                    }
                }

            }
            return newReconciliation;
        }

        private List<ReconciliationLinePM> GetNonUsedPaymentRecoLines(ReconciliationLinePM paymentReconcileLine)
        {
            if (paymentReconcileLine.ReconciliationAmount > 0)
                return remainingPaymentRecoLines.Where(d => d.ReconciliationAmount < 0).ToList();
            else
                return remainingPaymentRecoLines.Where(d => d.ReconciliationAmount > 0).ToList();
        }

        void SliceAndAddRecoLine(ReconciliationPM newReconciliation, decimal paymentAmount2Reconcile, ReconciliationLinePM otherRecoLine)
        {
            ReconciliationLinePM sliceLine = CloneReconcileLine(otherRecoLine);
            sliceLine.ReconciliationAmount = paymentAmount2Reconcile > 0 ? Math.Abs(paymentAmount2Reconcile)*-1 : Math.Abs(paymentAmount2Reconcile);
            newReconciliation.ReconciliationLines.Add(sliceLine);

            otherRecoLine.ReconciliationAmount -= sliceLine.ReconciliationAmount; // slice B - remaining

        }

        List<LedgerTransactionPM> GetReconcileTransactions()
        {
            List<string> transactionsIds = reconciliationPM.ReconciliationLines.Select(d => d.TransactionId).ToList();
            LedgerTransactionQueryService transactionQueryService = new LedgerTransactionQueryService(reconciliationPM.Tenant);
            List<LedgerTransactionPM> recoTransactions = transactionQueryService.GetLedgerTransactionPMsByIdList(transactionsIds, reconciliationPM.Tenant);
            return recoTransactions;
        }
        List<LedgerTransactionPM> GetPaymentTransactions()
        {
            return recoTransactions.Where(d => d.SourceTypeCode == AccountingEntities.ARPayment).OrderBy(d => d.DueDate).ThenBy(d => d.CreateDate).ToList();
        }
        ReconciliationLinePM FindCorrspondingRecoLineFromTransactionId(string transactionId)
        {
            return reconciliationPM.ReconciliationLines.Where(d => d.TransactionId == transactionId).FirstOrDefault();
        }
        List<ReconciliationLinePM> GetNonPaymentReconciliationLines()
        {
            List<LedgerTransactionPM> nonPaymentsTransactions = GetNonPaymentTransactions();

            List<string> transactionsIds = nonPaymentsTransactions.Select(d => d.Id).ToList();
            List<ReconciliationLinePM> lines = reconciliationPM.ReconciliationLines
                .Where(d => transactionsIds.Contains(d.TransactionId))
                .OrderBy(d=>d.DueDate)
                .ThenBy(d=>d.CreateDate)
                .ToList();
            return lines;
        }
        List<ReconciliationLinePM> GetPaymentReconciliationLines()
        {
            List<LedgerTransactionPM> paymentsTransactions = GetPaymentTransactions();

            List<string> transactionsIds = paymentsTransactions.Select(d => d.Id).ToList();
            List<ReconciliationLinePM> lines = reconciliationPM.ReconciliationLines
                .Where(d => transactionsIds.Contains(d.TransactionId))
                .OrderBy(d => d.DueDate)
                .ThenBy(d => d.CreateDate)
                .ToList();
            return lines;
        }
        ReconciliationPM InitNewReconciliation()
        {
            return new ReconciliationPM()
            {
                ChangeSetOp = ChangeSetOperation.Insert,
                Id = reconciliationPM.Id,
                Tenant = reconciliationPM.Tenant,
                AccountId = reconciliationPM.AccountId,
                Number = reconciliationPM.Number,
                CreateDate = reconciliationPM.CreateDate,
                CreatedByUserId = reconciliationPM.CreatedByUserId,
                SearchFields = reconciliationPM.SearchFields,
                CreatedByUserName = reconciliationPM.CreatedByUserName,
                AccountNumber = reconciliationPM.AccountNumber,
                AccountName = reconciliationPM.AccountName,
                IsCancelled = reconciliationPM.IsCancelled,
                CurrencyCode = reconciliationPM.CurrencyCode,
                AccountReconcileMethodCode = reconciliationPM.AccountReconcileMethodCode,
                AccountCurrencyId = reconciliationPM.AccountCurrencyId,                
            };
        }
        List<LedgerTransactionPM> GetNonPaymentTransactions()
        {
            return recoTransactions.Where(d => d.SourceTypeCode != AccountingEntities.ARPayment).OrderBy(d => d.DueDate).ThenBy(d => d.CreateDate).ToList();
        }
        ReconciliationLinePM CloneReconcileLine(ReconciliationLinePM line)
        {
            var newLine = new ReconciliationLinePM()
            {
                ChangeSetOp = ChangeSetOperation.Insert,
                ReconciliationId = line.ReconciliationId,
                Line = line.Line,
                Tenant = line.Tenant,
                CurrencyId = line.CurrencyId,
                CurrencyCode = line.CurrencyCode,
                CurrencyName = line.CurrencyName,
                TransactionId = line.TransactionId,
                ReconciliationAmount = line.ReconciliationAmount,
                IsPartial = line.IsPartial,
                GroupNumber = line.GroupNumber,
                IsAdjustTransaction = line.IsAdjustTransaction,
                ColorField = line.ColorField,
                CreateDate = line.CreateDate,
                DueDate = line.DueDate,
                AmountDebit = line.AmountDebit,
                AmountCredit = line.AmountCredit,
                Reference1 = line.Reference1,
                Reference2 = line.Reference2,
                Reference3 = line.Reference3,
                Notes = line.Notes,
                JournalId = line.JournalId,
                JournalNumber = line.JournalNumber,
                CurrencySign = line.CurrencySign,
                OpenAmountCurrencySign = line.OpenAmountCurrencySign,
                SearchFields = line.SearchFields,
                ReconciledWithTransactionId = line.ReconciledWithTransactionId,
                IsRecoCancelled = line.IsRecoCancelled,
            };
            return newLine;
        }

    }
}
