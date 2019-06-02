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
    public class ReconciliationAfterConversionBatch
    {
        private string _ResponseText;
        private HttpStatusCode _StatusCode;

        public ReconciliationAfterConversionBatch()
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
        public void RunReconciliationAfterConversion(int tenant, string fromExtNum, string toExtNum)
        {
            try
            {
                IAccountingContext context = AccountingContext.GetContext(tenant);
                JournalLineQueryService journalLineQueryService = new JournalLineQueryService(context);

                IQueryable<IGrouping<String, JournalLineLedgerTransactionDTO>> journalLineGroups = journalLineQueryService.GetQGJournalLinesByExternalRecoFromTo(tenant, fromExtNum, toExtNum);
                LedgerTransactionQueryService ledgerTransactionQueryService = new LedgerTransactionQueryService(context);
                List<ReconciableGroup> reconciableGroupList = new List<ReconciableGroup>();
                List<string> badList = new List<string>();
                List<string> goodList = new List<string>();
                List<Int64> madeList = new List<Int64>();

                foreach (IGrouping<String, JournalLineLedgerTransactionDTO> group in journalLineGroups)
                {
                    string groupKey = group.Key;
                    List<JournalLineReco> journalLineList = new List<JournalLineReco>();
                    foreach (JournalLineLedgerTransactionDTO journalLineLedgerTransactionDTO in group)
                    {
                        journalLineList.Add(new JournalLineReco(journalLineLedgerTransactionDTO.JournalLine, journalLineLedgerTransactionDTO.LedgerTransaction));
                    }
                    if (IsGroupReconciable(journalLineList))
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
                reconciableGroupList.Sort((x, y) => x._Ref.CompareTo(y._Ref));
                reconciableGroupList.ForEach(recoGroup =>
                {
                    string gLAccountId = GetGroupGLAccountId(recoGroup._LineGroup);
                    if (!String.IsNullOrWhiteSpace(gLAccountId))
                    {
                        ReconcileOneRef(recoGroup._LineGroup, gLAccountId, ledgerTransactionQueryService);
                        madeList.Add(recoGroup._Ref);
                    }
                });
                _ResponseText = $"Good: {goodList.Count},  Bad: {badList.Count},   Made: {madeList.Count}";
            }
            catch (Exception e)
            {
                throw new Exception("ReconciliationAfterConversionBatch failure ", e);
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
            rv = journalLineRecoList.Where(line => line._journalLine.ActionCode == "1").FirstOrDefault()._journalLine.CreditAccountId;
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

        private bool IsGroupReconciable(List<JournalLineReco> journalLineRecoList)
        {
            bool rv = true;
            if (journalLineRecoList.Count == 0)
            {
                rv = false;
            }
            else if (journalLineRecoList.Exists(line => line._journalLine.ActionCode != "1" && line._journalLine.ActionCode != "2"))
            {
                rv = false;
            }
            else if (!journalLineRecoList.Exists(line => line._journalLine.ActionCode == "1") ||  !journalLineRecoList.Exists(line => line._journalLine.ActionCode == "2"))
            {
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
                    rv = false;
                }
            }
            return rv;
        }

        private void ReconcileOneRef(List<JournalLineReco> journalLineRecoList, string gLAccountId, LedgerTransactionQueryService ledgerTransactionQueryService)
        {
            using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(5)))
            {
                try
                {
                    List<LedgerTransaction> ledger = GetLedger(journalLineRecoList);
                    List<LedgerTransactionPM> ledgerPMs = ledger.Select(poco => ledgerTransactionQueryService.GetEntityPM(poco)).ToList();
                    CreateReconciliationService createReconciliationService = new CreateReconciliationService();
                    ReconciliationPM reconciliationPM = createReconciliationService.GetReconciliation(ledgerPMs);
                    CreateReconciliationService service = new CreateReconciliationService();
                    RecoCallback recoCallback = service.CreateReconciliation(reconciliationPM);
                    scope.Complete();
                }
                catch (Exception e)
                {
                    //scope.Dispose();
                    throw new Exception("ReconciliationAfterConversionBatch failed while performing ReconcileOneRef ", e);
                }
            }
        }

        private class ReconciableGroup
        {
            public Int64 _Ref { get; set; }
            public List<JournalLineReco> _LineGroup { get; set; }
            public ReconciableGroup(string reference, List<JournalLineReco> lineGroup)
            {
                _LineGroup = lineGroup;
                _Ref = Int64.Parse(reference);
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
                    this._valueToMatch = journalLine.LocalAmount - (journalLine.ExternalOpenAmount ?? 0m);
                }
                else if (journalLine.ActionCode == "2")
                {
                    this._valueToMatch = - (journalLine.LocalAmount - (journalLine.ExternalOpenAmount ?? 0m));
                }
                this._oneLineLedger = ledgerTransaction; // ledgerTransactionQueryService.GetByJournalLineIdAndLine(journalLine.JournalId, journalLine.Line, journalLine.Tenant);
            }
        }
    }
}