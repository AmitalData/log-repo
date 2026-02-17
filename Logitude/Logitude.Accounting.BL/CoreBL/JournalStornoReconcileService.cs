using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.Validators;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL
{
    public class JournalStornoReconcileService : IJournalStornoReconcileService
    {
        private JournalPM _JournalPM;
        private IAccountingContext _AccountingContext;
        private List<LedgerTransactionPM> _OrginalJornalLedgerTransactions;
        private List<LedgerTransactionPM> _StornoJornalLedgerTransactions;
        public void MustInitialize(IAccountingContext accountingContext ,JournalPM journalPM,List<LedgerTransactionPM> stornoLedgerTransactionPM)
        {
            _JournalPM = journalPM;
            _StornoJornalLedgerTransactions = stornoLedgerTransactionPM;
            _AccountingContext = accountingContext;
        }
        public bool CreateReconcileFromStorno()
        {
            if (!IsStornoJournal())
            {
                return false;
            }
            FetchlTransactionOfOriginalJournal(_JournalPM.OriginalJournalId, _JournalPM.Tenant);

            bool isOrginalJournalReconcileAny =IsOrginalJournalReconcileAny(_JournalPM.OriginalJournalId, _JournalPM.Tenant);
            if (isOrginalJournalReconcileAny )
            {
                return false;
            }
            var reconciliationList  =CreateReconciliationList (_StornoJornalLedgerTransactions,_OrginalJornalLedgerTransactions);
            
            foreach (var itemReconciliation in reconciliationList)
            {
                var validContext = AccountingValidationContextServiceProvider.NewReconciliationValidatorContext((this._AccountingContext as IAccountingContext), itemReconciliation);
                ValidationResult result = ReconciliationValidator.IsReconciliationValid(itemReconciliation, validContext);
                if (result != null)
                {

                    throw new Exception(result.ErrorMessage);
                    return false;
                }
            }
            this.Reconciliations2Insert = reconciliationList;
            
            return true;
            //_OrginalTransaction
        }

        private bool IsStornoJournal()
        {
            if (String.IsNullOrWhiteSpace(_JournalPM.OriginalJournalId))
            {
                return false;
            }
            return true;
        }

        public virtual //4 UnitTest
            List<ReconciliationPM> CreateReconciliationList(
            List<LedgerTransactionPM> myOrginalJournalTransaction,
            List<LedgerTransactionPM> myStornoLedgerTransactionPM)
        {
            Reconciliations2Insert = new List<ReconciliationPM>();
            var union = myOrginalJournalTransaction.Union(myStornoLedgerTransactionPM).ToList();
            union = union.OrderBy(r => r.AccountId).ThenBy(r => r.JournalLineNumber).ToList();
            foreach (var transAccount in union.GroupBy(trans => trans.AccountId))
            {
                var myMatchLedgerTransactionList = new List<LedgerTransactionPM>();
                var groupByLine = transAccount.GroupBy(trans => trans.JournalLineNumber);
                foreach (var lt in groupByLine.SelectMany( trans=>trans))
                {
                    lt.GroupMatch = 1;
                    lt.AmountToReconcile = lt.OpenAmount;
                    myMatchLedgerTransactionList.Add(lt);
                    
                }
                var createReconciliationService = new CreateReconciliationService();
                var myReconciliation = createReconciliationService.GetReconciliation(myMatchLedgerTransactionList);
                 Reconciliations2Insert.Add(myReconciliation);
            }

            return Reconciliations2Insert;

        }
 


        private bool IsOrginalJournalReconcileAny(string OriginalJournalId, int Tenant)
        {


            

            var orginalTransIdList = _OrginalJornalLedgerTransactions.Select(r => r.Id).ToList();
            var qsReconciliationLineQueryService = new ReconciliationLineQueryService(_AccountingContext);
            var isOrginalJournalReconcile =qsReconciliationLineQueryService.IsReconciledBy(Tenant, orginalTransIdList);
            return isOrginalJournalReconcile;
        }

        private void FetchlTransactionOfOriginalJournal(string OriginalJournalId, int Tenant)
        {
            var qs = new LedgerTransactionQueryService(_AccountingContext);
            _OrginalJornalLedgerTransactions = qs.GetByJournalId(OriginalJournalId, Tenant);
        }



        public List<ReconciliationPM> Reconciliations2Insert { get; set; }
    }


   
    public interface IJournalStornoReconcileService
    {
        List<ReconciliationPM> Reconciliations2Insert { get; }
        void MustInitialize(IAccountingContext accountingContext, JournalPM journalPM, List<LedgerTransactionPM> stornoLedgerTransactionPM);
        bool CreateReconcileFromStorno();
    }
}
