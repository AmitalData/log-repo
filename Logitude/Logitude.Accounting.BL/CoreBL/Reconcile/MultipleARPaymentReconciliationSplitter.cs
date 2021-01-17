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
        ReconciliationPM originalReconciliation;
        List<LedgerTransactionPM> recoTransactions;
        List<ReconciliationPM> createdReconciliations = new List<ReconciliationPM>();

        List<ReconciliationLinePM> paymentsRecoLines = new List<ReconciliationLinePM>();
        static List<ReconciliationLinePM> nonPaymentsRecoLines = new List<ReconciliationLinePM>();

        public MultipleARPaymentReconciliationSplitter(ReconciliationPM reconcileToSplit)
        {
            originalReconciliation = reconcileToSplit;
            
            InitLines();
        }
        public List<ReconciliationPM> SplitReconciliation()
        {
            ReconciliationLinePM line = GetNextReconcileLine();
            if (line != null)
            {
                CreateReconciliationForLine(line);

                return SplitReconciliation();
            }
            else
                return createdReconciliations;
        }


        private void InitLines()
        {
            recoTransactions = GetReconcileTransactions();
            paymentsRecoLines = GetPaymentReconciliationLines();
            nonPaymentsRecoLines = GetNonPaymentReconciliationLines();
        }

        private void CreateReconciliationForLine(ReconciliationLinePM line)
        {
            var newReco = InitNewReconciliation();

            AddReconcileLine(line, newReco);

            AddOppositLines(CloneReconcileLine(line), newReco);

            CommitReconcile(newReco);
        }

        private ReconciliationLinePM GetNextReconcileLine()
        {
            if (paymentsRecoLines.Count > 0)
                return PopLine(paymentsRecoLines);
            else if (nonPaymentsRecoLines.Count > 0)
                return PopLine(nonPaymentsRecoLines);
            return null;
        }

        private void CommitReconcile(ReconciliationPM newReco)
        {
            createdReconciliations.Add(newReco);
        }

        private ReconciliationLinePM PopLine(List<ReconciliationLinePM> reconcileLines)
        {
            if(reconcileLines.Count() > 0)
            {
                var line = reconcileLines.First();
                reconcileLines.RemoveAt(0);
                return line;
            }

            return null;
        }

        private void AddOppositLines(ReconciliationLinePM recoLine, ReconciliationPM reconciliation)
        {
            ReconciliationLinePM oppositeLine = GetOppositLine(recoLine);

            AddReconcileLine(oppositeLine, reconciliation);

            if (SumReconcileLinesTotal(reconciliation) == 0)
                return;
            else
            {
                recoLine.ReconciliationAmount += oppositeLine.ReconciliationAmount;
                AddOppositLines(recoLine, reconciliation);
            }
        }

        private ReconciliationLinePM GetOppositLine(ReconciliationLinePM recoLine)
        {
            ReconciliationLinePM suitableLine = GetNextSuitableNonPaymentLine(recoLine);
            if (suitableLine == null)
                suitableLine = GetNextSuitablePaymentLine(recoLine);

            ReconciliationLinePM refinedLine = RefineAmountToReconcile(suitableLine, recoLine.ReconciliationAmount);
            return refinedLine;
        }

        private static decimal SumReconcileLinesTotal(ReconciliationPM reconciliation)
        {
            return reconciliation.ReconciliationLines.Sum(r => r.ReconciliationAmount);
        }

        private ReconciliationLinePM RefineAmountToReconcile(ReconciliationLinePM suitableLine, decimal recoAmount)
        {
            ReconciliationLinePM lineToAdd;
            bool suitableLineIsBiggerThanReconcileLine = Math.Abs(suitableLine.ReconciliationAmount) > Math.Abs(recoAmount);
            if (suitableLineIsBiggerThanReconcileLine)
            {
                lineToAdd = GetSlicedLineWithSuitableReconcileAmount(recoAmount, suitableLine);
            }
            else
            {
                var nonPaymentsRecoLine = nonPaymentsRecoLines.FirstOrDefault(d => d.TransactionId == suitableLine.TransactionId);
                if(nonPaymentsRecoLine != null)
                    nonPaymentsRecoLines.Remove(nonPaymentsRecoLine);
                else
                {
                    var paymentsRecoLine = paymentsRecoLines.FirstOrDefault(d => d.TransactionId == suitableLine.TransactionId);
                    paymentsRecoLines.Remove(paymentsRecoLine);
                }

                lineToAdd = suitableLine;
            }

            return lineToAdd;
        }

        private ReconciliationLinePM GetNextSuitableNonPaymentLine(ReconciliationLinePM firstRecoLine, List<ReconciliationLinePM> recoLines = null)
        {
            if(recoLines == null)
                recoLines = CloneNonPaymentRecoLines();

            ReconciliationLinePM line = PopLine(recoLines);

            if (line != null)
            {
                var differentSign = Math.Sign(line.ReconciliationAmount) != Math.Sign(firstRecoLine.ReconciliationAmount);
                if (differentSign)
                    return line;
                else
                    return GetNextSuitableNonPaymentLine(firstRecoLine, recoLines);
            }
            else
                return null;
        }
        private ReconciliationLinePM GetNextSuitablePaymentLine(ReconciliationLinePM firstRecoLine, List<ReconciliationLinePM> recoLines = null)
        {
            if (recoLines == null)
                recoLines = ClonePaymentRecoLines();

            ReconciliationLinePM line = PopLine(recoLines);

            if (line != null)
            {
                var differentSign = Math.Sign(line.ReconciliationAmount) != Math.Sign(firstRecoLine.ReconciliationAmount);
                if (differentSign)
                    return line;
                else
                    return GetNextSuitablePaymentLine(firstRecoLine, recoLines);
            }
            else
                return null;
        }

        private List<ReconciliationLinePM> CloneNonPaymentRecoLines()
        {
            return nonPaymentsRecoLines.Select(a => a).ToList();
        }
        private List<ReconciliationLinePM> ClonePaymentRecoLines()
        {
            return paymentsRecoLines.Select(a => a).ToList();
        }

        private static void AddReconcileLine(ReconciliationLinePM reconcileLine, ReconciliationPM reconciliation)
        {
            reconciliation.ReconciliationLines.Add(reconcileLine);
        }

        ReconciliationLinePM GetSlicedLineWithSuitableReconcileAmount(decimal paymentAmount2Reconcile, ReconciliationLinePM otherRecoLine)
        {
            ReconciliationLinePM sliceLine = CloneReconcileLine(otherRecoLine);
            sliceLine.ReconciliationAmount = paymentAmount2Reconcile > 0 ? Math.Abs(paymentAmount2Reconcile)*-1 : Math.Abs(paymentAmount2Reconcile);

            otherRecoLine.ReconciliationAmount -= sliceLine.ReconciliationAmount; 
            

            return sliceLine;
        }

        List<LedgerTransactionPM> GetReconcileTransactions()
        {
            List<string> transactionsIds = originalReconciliation.ReconciliationLines.Select(d => d.TransactionId).ToList();
            LedgerTransactionQueryService transactionQueryService = new LedgerTransactionQueryService(originalReconciliation.Tenant);
            List<LedgerTransactionPM> recoTransactions = transactionQueryService.GetLedgerTransactionPMsByIdList(transactionsIds, originalReconciliation.Tenant);
            return recoTransactions;
        }
        List<LedgerTransactionPM> GetPaymentTransactions()
        {
            return recoTransactions.Where(d => d.SourceTypeCode == AccountingEntities.ARPayment).OrderBy(d => d.DueDate).ThenBy(d => d.CreateDate).ToList();
        }
        List<ReconciliationLinePM> GetNonPaymentReconciliationLines()
        {
            List<LedgerTransactionPM> nonPaymentsTransactions = GetNonPaymentTransactions();

            List<string> transactionsIds = nonPaymentsTransactions.Select(d => d.Id).ToList();
            List<ReconciliationLinePM> lines = originalReconciliation.ReconciliationLines
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
            List<ReconciliationLinePM> lines = originalReconciliation.ReconciliationLines
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
                Id = originalReconciliation.Id,
                Tenant = originalReconciliation.Tenant,
                AccountId = originalReconciliation.AccountId,
                Number = originalReconciliation.Number,
                CreateDate = originalReconciliation.CreateDate,
                CreatedByUserId = originalReconciliation.CreatedByUserId,
                SearchFields = originalReconciliation.SearchFields,
                CreatedByUserName = originalReconciliation.CreatedByUserName,
                AccountNumber = originalReconciliation.AccountNumber,
                AccountName = originalReconciliation.AccountName,
                IsCancelled = originalReconciliation.IsCancelled,
                CurrencyCode = originalReconciliation.CurrencyCode,
                AccountReconcileMethodCode = originalReconciliation.AccountReconcileMethodCode,
                AccountCurrencyId = originalReconciliation.AccountCurrencyId,                
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
