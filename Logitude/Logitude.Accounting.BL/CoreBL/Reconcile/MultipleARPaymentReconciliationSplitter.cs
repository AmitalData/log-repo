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
        List<ReconciliationPM> paymentReconciliations = new List<ReconciliationPM>();

        public MultipleARPaymentReconciliationSplitter(ReconciliationPM reconcileToSplit)
        {
            tenant = reconcileToSplit.Tenant;

            reconciliationPM = reconcileToSplit;
            recoTransactions = GetReconcileTransactions();
        }

        public List<ReconciliationPM> Split()
        {
            List<ReconciliationLinePM> paymentsRecoLines = GetPaymentReconciliationLines();
            List<ReconciliationLinePM> nonPaymentsRecoLines = GetNonPaymentReconciliationLines();

            foreach (ReconciliationLinePM paymentReconcileLine in paymentsRecoLines)
            {
                ReconciliationPM newReconciliation;
                if (nonPaymentsRecoLines.Count > 0)
                {
                    newReconciliation = ReconcilePaymentWithOtherLines(nonPaymentsRecoLines, paymentReconcileLine);
                    paymentReconciliations.Add(newReconciliation);
                }
                else if (paymentsRecoLines.Count == 2)
                {
                    paymentReconciliations.Add(ReconcilePaymentsOnly(paymentsRecoLines));
                    break;
                }
            }
            return paymentReconciliations;
        }

        private ReconciliationPM ReconcilePaymentsOnly(List<ReconciliationLinePM> paymentsRecoLines)
        {
            ReconciliationPM newReconciliation = InitNewReconciliation();
            newReconciliation.ReconciliationLines.AddRange(paymentsRecoLines);
            return newReconciliation;
        }

        //List<ReconciliationLinePM> oppositePaymentsRecoLines = GetNonUsedPaymentRecoLines(paymentReconcileLine);
        //ReconciliationLinePM oppositeRecoLine = nonPaymentsRecoLines.Count > 0 ? nonPaymentsRecoLines.First() : oppositePaymentsRecoLines.First();

        ReconciliationPM ReconcilePaymentWithOtherLines(List<ReconciliationLinePM> nonPaymentsRecoLines, ReconciliationLinePM paymentReconcileLine)
        {
            ReconciliationPM newReconciliation = InitNewReconciliation();

            AddPaymentReconcileLine(paymentReconcileLine, newReconciliation);

            AddNonPaymentReconcileLine(nonPaymentsRecoLines, paymentReconcileLine, newReconciliation);

            return newReconciliation;
        }

        private static void AddPaymentReconcileLine(ReconciliationLinePM paymentReconcileLine, ReconciliationPM newReconciliation)
        {
            newReconciliation.ReconciliationLines.Add(paymentReconcileLine);
        }

        private void AddNonPaymentReconcileLine(List<ReconciliationLinePM> nonPaymentsRecoLines, ReconciliationLinePM paymentReconcileLine, ReconciliationPM newReconciliation)
        {
            while (paymentReconcileLine.ReconciliationAmount != 0)
            {
                ReconciliationLinePM lineToAdd = GetOppositeRecoLine(nonPaymentsRecoLines);

                if (IsLineHasAmountGreaterThanPaymentLine(paymentReconcileLine, lineToAdd))
                    lineToAdd = SliceLine(paymentReconcileLine.ReconciliationAmount, lineToAdd);

                AddLine(nonPaymentsRecoLines, newReconciliation, lineToAdd);
                SubtractLineAmountFromPaymentLine(paymentReconcileLine, lineToAdd);
            }
        }

        private static bool IsLineHasAmountGreaterThanPaymentLine(ReconciliationLinePM paymentReconcileLine, ReconciliationLinePM oppositeRecoLine)
        {
            return Math.Abs(oppositeRecoLine.ReconciliationAmount) > Math.Abs(paymentReconcileLine.ReconciliationAmount);
        }

        private static ReconciliationLinePM GetOppositeRecoLine(List<ReconciliationLinePM> nonPaymentsRecoLines)
        {
            return nonPaymentsRecoLines.First();
        }

        private static void AddLine(List<ReconciliationLinePM> nonPaymentsRecoLines, ReconciliationPM newReconciliation, ReconciliationLinePM oppositeRecoLine)
        {
            newReconciliation.ReconciliationLines.Add(oppositeRecoLine);

            if(nonPaymentsRecoLines.Exists(d=>d == oppositeRecoLine))
                nonPaymentsRecoLines.Remove(oppositeRecoLine);
        }

        private static void SubtractLineAmountFromPaymentLine(ReconciliationLinePM paymentReconcileLine, ReconciliationLinePM oppositeRecoLine)
        {
            if (oppositeRecoLine.ReconciliationAmount > paymentReconcileLine.ReconciliationAmount)
                paymentReconcileLine.ReconciliationAmount = 0;
            else if (paymentReconcileLine.ReconciliationAmount > 0)
                paymentReconcileLine.ReconciliationAmount -= Math.Abs(oppositeRecoLine.ReconciliationAmount);
            else
                paymentReconcileLine.ReconciliationAmount += Math.Abs(oppositeRecoLine.ReconciliationAmount);
        }

        private List<ReconciliationLinePM> GetNonUsedPaymentRecoLines(ReconciliationLinePM paymentReconcileLine)
        {
            if (paymentReconcileLine.ReconciliationAmount > 0)
                return remainingPaymentRecoLines.Where(d => d.ReconciliationAmount < 0).ToList();
            else
                return remainingPaymentRecoLines.Where(d => d.ReconciliationAmount > 0).ToList();
        }

        ReconciliationLinePM SliceLine(decimal paymentAmount2Reconcile, ReconciliationLinePM otherRecoLine)
        {
            ReconciliationLinePM sliceLine = CloneReconcileLine(otherRecoLine);
            sliceLine.ReconciliationAmount = paymentAmount2Reconcile > 0 ? Math.Abs(paymentAmount2Reconcile)*-1 : Math.Abs(paymentAmount2Reconcile);

            otherRecoLine.ReconciliationAmount -= sliceLine.ReconciliationAmount; // slice B - remaining

            return sliceLine;
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
