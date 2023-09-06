

using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.Validators;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.Data.Enums;

namespace Logitude.Accounting.BL.CoreBL
{
    public class JournalStornoPrepareJReconcileService 
        : IJournalStornoPrepareJReconcileService
    {
        private JournalPM _JournalToVoidPM;
        private IAccountingContext _AccountingContext;
        private List<LedgerTransactionPM> _OrginalJornalLedgerTransactions;
        private JournalPM _TheStorno;

        ///private List<LedgerTransactionPM> _StornoJornalLedgerTransactions;
        public void MustInitialize(IAccountingContext accountingContext, 
            JournalPM journalToVoidPM
            //, List<LedgerTransactionPM> stornoLedgerTransactionPM
            )
        {
            _JournalToVoidPM = journalToVoidPM;
            //_StornoJornalLedgerTransactions = stornoLedgerTransactionPM;
            _AccountingContext = accountingContext;
        }
        public bool CreateJournalReconcileFromStorno(JournalPM theStorno, StornoOverrideM stornoOverrideM)
        {
            _TheStorno = theStorno;
            if (!String.IsNullOrWhiteSpace(_JournalToVoidPM.ExternalNo))
            {
                return false;
            }
            if (!String.IsNullOrWhiteSpace(_JournalToVoidPM.ExternalSystem))
            {
                return false;
            }
            if (!IsStornoJournal())
            {
                return false;
            }

            if (theStorno.AccountingEntityCode == AccountingEntityValues.Revaluation)
            {
                return false;
            }

            FetchlTransactionOfOriginalJournal(_JournalToVoidPM.Id, _JournalToVoidPM.Tenant);
            if (!_OrginalJornalLedgerTransactions.Any())
            {
                return false;// did not stream to Accounting !!
            }
            if (_OrginalJornalLedgerTransactions.Any(r => r.InReconcileProgress))
            {
                return false;
            }
            bool isOrginalJournalReconcileAny = IsOrginalJournalReconcileAny(_JournalToVoidPM.Id, _JournalToVoidPM.Tenant);
            if (isOrginalJournalReconcileAny)
            {
                return false;
            }

            var ledgers = _OrginalJornalLedgerTransactions;

            ledgers = RemoveLedgersOfExcludedCheques(stornoOverrideM, ledgers);

            this.JournalReconciles2Insert = CreateJournalReconcileList(ledgers);


            return true;
            //_OrginalTransaction
        }

        private static List<LedgerTransactionPM> RemoveLedgersOfExcludedCheques(StornoOverrideM stornoOverrideM, List<LedgerTransactionPM> ledgers)
        {
            if (stornoOverrideM.ChequeNumbersToExcludeFromStorno != null && stornoOverrideM.ChequeNumbersToExcludeFromStorno.Count() > 0)
            {
                ledgers = ledgers.Where(t => !stornoOverrideM.ChequeNumbersToExcludeFromStorno.Contains(t.Reference2)).ToList();
            }

            return ledgers;
        }

        private bool IsStornoJournal()
        {
            if (String.IsNullOrWhiteSpace(_TheStorno.OriginalJournalId))
            {
                return false;
            }
            
            if (_TheStorno.OriginalJournalId!=  _JournalToVoidPM.Id)
            {
                return false;
            }
            return true;
        }

        public virtual //4 UnitTest
            List<JournalReconcilePM> CreateJournalReconcileList(
            List<LedgerTransactionPM> myOrginalJournalTransaction
            )
        {

            int i = 1;
           var  journalReconciles2Insert = myOrginalJournalTransaction.OrderBy(r => r.AccountId).ThenBy(r => r.JournalNumber).ThenBy(r => r.JournalLineNumber)
                .Select(trans =>
                new JournalReconcilePM()
                {
                    ChangeSetOp = ChangeSetOperation.Insert,
                    JournalId = "up.OnCreate",
                    LedgerTransactionId = trans.Id,
                    Line = i++,
                    Tenant = _JournalToVoidPM.Tenant,

                    CurrencyId = trans.OpenAmountCurrencyId,
                    ReconciliationAmount = trans.OpenAmount,

                    IsPartial = false ,                     
                     
                }
                ).ToList();
            return journalReconciles2Insert;

        }



        private bool IsOrginalJournalReconcileAny(string OriginalJournalId, int Tenant)
        {




            var orginalTransIdList = _OrginalJornalLedgerTransactions.Select(r => r.Id).ToList();
            var qsReconciliationLineQueryService = new ReconciliationLineQueryService(_AccountingContext);
            var isOrginalJournalReconcile = qsReconciliationLineQueryService.IsReconciledBy(Tenant, orginalTransIdList);
            return isOrginalJournalReconcile;
        }

        private void FetchlTransactionOfOriginalJournal(string OriginalJournalId, int Tenant)
        {
            var qs = new LedgerTransactionQueryService(_AccountingContext);
            _OrginalJornalLedgerTransactions = qs.GetByJournalId(OriginalJournalId, Tenant);
        }



        public List<JournalReconcilePM> JournalReconciles2Insert { get; set; }
    }



    public interface IJournalStornoPrepareJReconcileService
    {
        List<JournalReconcilePM> JournalReconciles2Insert { get; }
        void MustInitialize(IAccountingContext accountingContext, JournalPM journalToVoidPM
            //, List<LedgerTransactionPM> stornoLedgerTransactionPM
            );
        bool CreateJournalReconcileFromStorno(JournalPM journalStornoPM, StornoOverrideM stornoOverrideM);
    }
}
