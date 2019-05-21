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
        public void RunReconciliationAfterConversion(int tenant)
        {
            try
            {
                IAccountingContext context = AccountingContext.GetContext(tenant);
                JournalLineQueryService journalLineQueryService = new JournalLineQueryService(context);
                IQueryable<IGrouping<String, JournalLine>> journalLineGroups = journalLineQueryService.GetQGJournalLinesByExternalReco(tenant);
                LedgerTransactionQueryService ledgerTransactionQueryService = new LedgerTransactionQueryService(context);


                foreach (IGrouping<String, JournalLine> group in journalLineGroups)
                {
                    string groupKey = group.Key;
                    List<JournalLineReco> journalLineList = new List<JournalLineReco>();
                    foreach (JournalLine journalLine in group)
                    {
                        journalLineList.Add(new JournalLineReco(journalLine, ledgerTransactionQueryService));
                    }
                    if (IsGroupReconciable(journalLineList))
                    {
                        string gLAccountId = GetGroupGLAccountId(journalLineList);
                        if (!String.IsNullOrWhiteSpace(gLAccountId))
                        {
                            ReconcileOneRef(groupKey, journalLineList, gLAccountId);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("ReconciliationAfterConversionBatch failure ", e);
            }
        }


        private List<LedgerTransactionPM> GetLedger(List<JournalLineReco> journalLineRecoList)
        {
            List<LedgerTransactionPM> rv = new List<LedgerTransactionPM>();
            journalLineRecoList.ForEach(journalLineReco =>
            {
                rv.AddRange(journalLineReco._oneLineLedger);
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
            else if (journalLineRecoList.Exists(line => line._oneLineLedger is null || line._oneLineLedger.Count == 0))
            {
                rv = false;
            }
            else if (journalLineRecoList.Exists(line => line._oneLineLedger.Exists(lt => lt.IsReconciled)))
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

        private void ReconcileOneRef(string externalReconcileNo, List<JournalLineReco> journalLineRecoList, string gLAccountId)
        {
            using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(5)))
            {
                try
                {
                    List<LedgerTransactionPM> ledger = GetLedger(journalLineRecoList);
                    CreateReconciliationService createReconciliationService = new CreateReconciliationService();
                    ReconciliationPM reconciliationPM = createReconciliationService.GetReconciliation(ledger);
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

        private class JournalLineReco
        {

            public Decimal _valueToMatch { get; set; }
            public JournalLine _journalLine { get; set; }
            public List<LedgerTransactionPM> _oneLineLedger { get; set; }
            public JournalLineReco(JournalLine journalLine, LedgerTransactionQueryService ledgerTransactionQueryService)
            {
                this._journalLine = journalLine;
                this._valueToMatch = 0m;
                if (journalLine.ActionCode == "1")
                {
                    this._valueToMatch = journalLine.LocalAmount + (journalLine.ExternalOpenAmount ?? 0m);
                }
                else if (journalLine.ActionCode == "2")
                {
                    this._valueToMatch = journalLine.LocalAmount - (journalLine.ExternalOpenAmount ?? 0m);
                }
                this._oneLineLedger = ledgerTransactionQueryService.GetByJournalLineIdAndLine(journalLine.JournalId, journalLine.Line, journalLine.Tenant);
            }
        }
    }
}