using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.CoreBL;

namespace Logitude.Accounting.BL.Utils
{
    public class ReconciliationStageBBatch
    {
        private string _ResponseText;
        private HttpStatusCode _StatusCode;
        private List<string> _NoLines;
        private List<string> _WrongAction;
        private List<string> _WrongSum;
        private List<string> _WrongSumToMatch;
        long _counter = 0;

        public ReconciliationStageBBatch()
        {
            _ResponseText = "";
            _StatusCode = HttpStatusCode.Accepted;
        }

        public string ResponseText()
        {
            return _ResponseText;
        }

        public HttpStatusCode StatusCode()
        {
            return _StatusCode;
        }
        public void RunReconciliationStageB(int tenant)
        {
            try
            {
                IAccountingContext context = AccountingContext.GetContext(tenant);
                JournalLineQueryService journalLineQueryService = new JournalLineQueryService(context);
                IQueryable<JournalLineLedgerTransactionAccDTO> journalLine_LT_DTOs = journalLineQueryService.GetQJournalLinesByExternalNo_NotReconciled(tenant);
                List<JournalLineLedgerTransactionAccDTO> journalLine_LT_DTOsList = journalLine_LT_DTOs.ToList().OrderBy(l => l.AccId).ToList();
                var journalLineGroups = journalLine_LT_DTOsList.GroupBy(l => l.AccId);
                // IQueryable<IGrouping<String, JournalLineLedgerTransactionDTO>> journalLineGroups = journalLineQueryService.GetQGJournalLinesByExternalRecoFromTo(tenant, fromExtNum, toExtNum);
                LedgerTransactionQueryService ledgerTransactionQueryService = new LedgerTransactionQueryService(context);
                List<ReconciableGroup> reconciableGroupList = new List<ReconciableGroup>();
                List<string> badList = new List<string>();
                List<string> goodList = new List<string>();
                List<string> madeList = new List<string>();
                _NoLines = new List<string>();
                _WrongAction = new List<string>();
                _WrongSum = new List<string>();
                _WrongSumToMatch = new List<string>();

                foreach (IGrouping<String, JournalLineLedgerTransactionAccDTO> group in journalLineGroups)
                {
                    string groupKey = group.Key;
                    List<JournalLineReco> journalLineList = new List<JournalLineReco>();
                    foreach (JournalLineLedgerTransactionAccDTO journalLineLedgerTransactionDTO in group)
                    {
                        journalLineList.Add(new JournalLineReco(journalLineLedgerTransactionDTO.JournalLine, journalLineLedgerTransactionDTO.LedgerTransaction));
                    }
                    if (IsGroupReconciable(journalLineList, groupKey))
                    {
                        ReconciableGroup recoGroup = new ReconciableGroup(groupKey, journalLineList);
                        reconciableGroupList.Add(recoGroup);
                        goodList.Add(groupKey);
                    }
                    else
                    {
                        badList.Add(groupKey);
                    }
                }
                reconciableGroupList.Sort((x, y) => x._Acc.CompareTo(y._Acc));
                reconciableGroupList.ForEach(recoGroup =>
                {
                    string gLAccountId = GetGroupGLAccountId(recoGroup._LineGroup);
                    if (!String.IsNullOrWhiteSpace(gLAccountId))
                    {
                        using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(5)))
                        {
                            ReconcileOneRef(recoGroup._LineGroup, gLAccountId, ledgerTransactionQueryService);
                            _counter++;
                            scope.Complete();
                        }
                        madeList.Add(recoGroup._Acc);
                   }
                });
                _ResponseText = $"Good: {goodList.Count},  Bad: {badList.Count},   Made: {madeList.Count}, No Lines: {String.Join(", ", _NoLines.ToArray())}, Wrong Action: {String.Join(", ", _WrongAction.ToArray())}, Wrong Sum: {String.Join(", ", _WrongSum.ToArray())}, Wrong Sum To Match: {String.Join(", ", _WrongSumToMatch.ToArray())}";
            }
            catch (Exception e)
            {
                throw new Exception($"ReconciliationStageBBatch failure {e.Message} Inner Exception: {e.InnerException.Message}", e);
            }
        }


        private List<LedgerTransaction> GetLedger(List<JournalLineReco> journalLineRecoList)
        {
            List<LedgerTransaction> rv = new List<LedgerTransaction>();
            journalLineRecoList.ForEach(journalLineReco =>
            {
                // rv.AddRange(journalLineReco._oneLineLedger);
                rv.Add(journalLineReco._oneLineLedger);
            });
            return rv;
        }

        private string GetGroupGLAccountId(List<JournalLineReco> journalLineRecoList)
        {

            string rv = "";
            JournalLineReco creditLine = journalLineRecoList.Where(line => line._journalLine.ActionCode == "1").FirstOrDefault();
            if (creditLine != null)
            {
                rv = creditLine._journalLine.CreditAccountId;
            }
            else
            {
                JournalLineReco debitLine = journalLineRecoList.Where(line => line._journalLine.ActionCode == "2").FirstOrDefault();
                if (debitLine != null)
                {
                    rv = debitLine._journalLine.DebitAccountId;
                }
            }
            if (!String.IsNullOrWhiteSpace(rv) && journalLineRecoList.Exists(line => line._journalLine.ActionCode == "1" && line._journalLine.CreditAccountId != rv))
            {
                rv = "";
            }
            if (!String.IsNullOrWhiteSpace(rv) && journalLineRecoList.Exists(line => line._journalLine.ActionCode == "2" && line._journalLine.DebitAccountId != rv))
            {
                rv = "";
            }
            return rv;
        }

        private bool IsGroupReconciable(List<JournalLineReco> journalLineRecoList, string groupKey)
        {
            bool rv = true;

            List<JournalLineReco> creditLines = journalLineRecoList.Where(line => line._journalLine.ActionCode == "1").ToList<JournalLineReco>();
            decimal credit_sum = 0m;
            if (creditLines != null) credit_sum = creditLines.Sum(line => line._journalLine.LocalAmount + (line._journalLine.ExternalOpenAmount ?? 0m)); // because in credit lines the ExternalOpenAmount is negative 

            List<JournalLineReco> debitLines = journalLineRecoList.Where(line => line._journalLine.ActionCode == "2").ToList<JournalLineReco>();
            decimal debit_sum = 0m;
            if (debitLines != null) debit_sum = debitLines.Sum(line => line._journalLine.LocalAmount - (line._journalLine.ExternalOpenAmount ?? 0m));


            if (journalLineRecoList.Count == 0)
            {
                _NoLines.Add(groupKey);
                rv = false;
            }
            else if (journalLineRecoList.Count == 1)
            {
                _WrongSumToMatch.Add(groupKey);
                rv = false;
            }
            else if (journalLineRecoList.Exists(line => line._journalLine.ActionCode != "1" && line._journalLine.ActionCode != "2"))
            {
                _WrongAction.Add(groupKey);
                rv = false;
            }
            //           else if (!journalLineRecoList.Exists(line => line._journalLine.ActionCode == "1") ||  !journalLineRecoList.Exists(line => line._journalLine.ActionCode == "2"))
            //  else if ((journalLineRecoList.Sum(line => line._journalLine.LocalAmount) != 0m))
            else if (credit_sum - debit_sum != 0m)
            {
                _WrongSum.Add(groupKey);
                rv = false;
            }
            else if (journalLineRecoList.Exists(line => line._oneLineLedger == null))
            {
                rv = false;
            }
            else
            {
                Decimal sum = 0m;
                sum = journalLineRecoList.Sum(line => line._valueToMatch);
                if (sum != 0m)
                {
                    _WrongSumToMatch.Add(groupKey);
                    rv = false;
                }
            }
            return rv;
        }

        private void ReconcileOneRef(List<JournalLineReco> journalLineRecoList, string gLAccountId, LedgerTransactionQueryService ledgerTransactionQueryService)
        {
 //           using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(5)))
 //           {
                try
                {
                    List<LedgerTransaction> ledger = GetLedger(journalLineRecoList);
                    List<LedgerTransactionPM> ledgerPMs = ledger.Select(poco => ledgerTransactionQueryService.GetEntityPM(poco)).ToList();
                    CreateReconciliationService createReconciliationService = new CreateReconciliationService();
                    ReconciliationPM reconciliationPM = createReconciliationService.GetReconciliation(ledgerPMs);
  //****//                  reconciliationPM.CreatedByReconciliationAfterConversion = true;
                    CreateReconciliationService service = new CreateReconciliationService();
                    RecoCallback recoCallback = service.CreateReconciliation(reconciliationPM);
 //                   scope.Complete();
                }
                catch (Exception e)
                {
                    //scope.Dispose();
                    throw new Exception("ReconciliationStageBBatch failed while performing ReconcileOneRef ", e);
                }
 //           }
        }

        private class ReconciableGroup
        {
            public string _Acc { get; set; }
            public List<JournalLineReco> _LineGroup { get; set; }
            public ReconciableGroup(string reference, List<JournalLineReco> lineGroup)
            {
                _LineGroup = lineGroup;
                _Acc = reference;
            }
        }


        private class JournalLineReco
        {

            public Decimal _valueToMatch { get; set; }
            public JournalLine _journalLine { get; set; }
            public LedgerTransaction _oneLineLedger { get; set; }
            public JournalLineReco(JournalLine journalLine, LedgerTransaction ledgerTransaction) // LedgerTransactionQueryService ledgerTransactionQueryService)
            {
                this._journalLine = journalLine;
                this._valueToMatch = 0m;
                if (journalLine.ActionCode == "1")
                {
                    this._valueToMatch = journalLine.LocalAmount + (journalLine.ExternalOpenAmount ?? 0m); // because in credit lines the ExternalOpenAmount is negative 
                }
                else if (journalLine.ActionCode == "2")
                {
                    this._valueToMatch = -(journalLine.LocalAmount - (journalLine.ExternalOpenAmount ?? 0m));
                }
                this._oneLineLedger = ledgerTransaction; // ledgerTransactionQueryService.GetByJournalLineIdAndLine(journalLine.JournalId, journalLine.Line, journalLine.Tenant);
                this._oneLineLedger.AmountToReconcile = _valueToMatch;
            }
        }
    }
}