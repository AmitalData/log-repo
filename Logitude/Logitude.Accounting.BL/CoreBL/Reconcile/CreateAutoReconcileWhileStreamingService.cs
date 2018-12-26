
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logitude.Accounting.Data;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.Validators;
using System.ComponentModel.DataAnnotations;

namespace Logitude.Accounting.BL.CoreBL
{
    public class CreateAutoReconcileWhileStreamingService : ICreateAutoReconcileWhileStreamingService
    {
        private IAccountingContext _AccountingContext;
        private JournalPM _JournalPM;
        List<LedgerTransactionPM> _NewLedgerTransactionsWithCounters;
        public void MustInit(
            IAccountingContext accountingContext, 
            JournalPM JournalPM,
            List<LedgerTransactionPM> myNewLedgerTransactionsWithCounters
            )
        {
            _AccountingContext = accountingContext;
            _JournalPM = JournalPM;
            _NewLedgerTransactionsWithCounters = myNewLedgerTransactionsWithCounters;
            ReconciliationList = new List<ReconciliationPM>();
        }
        public List<ReconciliationPM> ReconciliationList { get; private set; }
        public void CreateAutoReconcileWhileStreaming()
        {

            if (_JournalPM.JournalReconciles.Count == 0)
            {
                return;
            }
            var theReconcileAgainstLTranIdList = _JournalPM.JournalReconciles.Select(r => r.LedgerTransactionId).ToList();
            List<LedgerTransactionPM> myOldTransToReconcile = GetLedgerTransactionToReconcile(theReconcileAgainstLTranIdList);
            if (myOldTransToReconcile.Any(r => !r.InReconcileProgress))
            {
                throw new Exception("_JournalPM.JournalReconciles have  myOldTransToReconcile.Any( r=> !r.InReconcileProgress) ");
            }

            //From JournalReconcile take old Ledger  - Match new Ledger 
            foreach (var oldLTransGroupByAccountId in myOldTransToReconcile.GroupBy(r => r.AccountId))
            {

                var currentAccountId = oldLTransGroupByAccountId.Key;
                var currrentAccountJournalReconcileList =
                    (from jr in _JournalPM.JournalReconciles
                     join oldLTrans in oldLTransGroupByAccountId
                     on jr.LedgerTransactionId equals oldLTrans.Id
                     select jr
                     ).ToList();
                var totReconciliationAmount = currrentAccountJournalReconcileList.Sum(r => r.ReconciliationAmount);

                //var journalReconcileWith_ReconciliationAmount_MoreThenItPossible =
                //    (
                //    from jr in _JournalPM.JournalReconciles
                //    join oldLTrans in oldLTransGroupByAccountId
                //    on jr.LedgerTransactionId equals oldLTrans.Id
                //    where jr.ReconciliationAmount > oldLTrans.OpenAmount
                //    select jr
                // ).ToList();
                //if (journalReconcileWith_ReconciliationAmount_MoreThenItPossible.Any())
                //{
                //    throw new Exception("(journalReconcileWith_ReconciliationAmount_MoreThenItPossible.Any())");
                //}








                var newLTranListOfAccountID = _NewLedgerTransactionsWithCounters.Where(r => r.AccountId == currentAccountId).ToList();
                decimal totNew = newLTranListOfAccountID.Sum(r => r.OpenAmount);
                if (totNew + totReconciliationAmount != 0)
                //if (totNew - totReconciliationAmount != 0)
                {
                    throw new Exception($"for JournalPM.Id ={_JournalPM.Id} Account {currentAccountId}  (totNew != totReconciliationAmount) = ({totNew} != {totReconciliationAmount})");
                }


                var myReconciliationPM = new ReconciliationPM();
                myReconciliationPM.Id = "new";
                myReconciliationPM.ChangeSetOp = ChangeSetOperation.Insert;
                myReconciliationPM.Tenant = _JournalPM.Tenant;
                myReconciliationPM.AccountId = currentAccountId;
                myReconciliationPM.Number = "get";
                myReconciliationPM.CreateDate = DateTime.UtcNow;
                myReconciliationPM.CreatedByUserId = _JournalPM.UpdatedByUserId; ///ResolvedCreatedByUserId();


                //new Journal/Ledger will be Close Totally !!
                int lineCounter = 1;
                foreach (var newLTran in newLTranListOfAccountID)
                {
                    var myReconciliationLinePM = new ReconciliationLinePM();
                    myReconciliationLinePM.ChangeSetOp = ChangeSetOperation.Insert; ;
                    myReconciliationLinePM.ReconciliationId = myReconciliationPM.Id;
                    myReconciliationLinePM.Tenant = myReconciliationPM.Tenant;
                    myReconciliationLinePM.Line = lineCounter++;
                    myReconciliationLinePM.CurrencyId = newLTran.OpenAmountCurrencyId;
                    myReconciliationLinePM.TransactionId = newLTran.Id;
                    myReconciliationLinePM.ReconciliationAmount = newLTran.OpenAmount; //new Journal/Ledger will be Close Totally !!
                    myReconciliationLinePM.GroupNumber = 1;
                    myReconciliationPM.ReconciliationLines.Add(myReconciliationLinePM);

                }

                foreach (var oldLedger in oldLTransGroupByAccountId)
                {
                    var journalReconcile = _JournalPM.JournalReconciles.First(r => r.LedgerTransactionId == oldLedger.Id);
                    var myReconciliationLinePM = new ReconciliationLinePM();
                    myReconciliationLinePM.ChangeSetOp = ChangeSetOperation.Insert; ;
                    myReconciliationLinePM.ReconciliationId = myReconciliationPM.Id;
                    myReconciliationLinePM.Tenant = myReconciliationPM.Tenant;
                    myReconciliationLinePM.Line = lineCounter++;
                    myReconciliationLinePM.CurrencyId = journalReconcile.CurrencyId;
                    myReconciliationLinePM.TransactionId = journalReconcile.LedgerTransactionId;
                    myReconciliationLinePM.ReconciliationAmount = journalReconcile.ReconciliationAmount; //new Journal/Ledger will be Close Totally !!
                    myReconciliationLinePM.GroupNumber = 1;
                    myReconciliationPM.ReconciliationLines.Add(myReconciliationLinePM);

                }

                ValidationResult result = ValidateReconcile(myReconciliationPM);
                if (result != null)
                {

                    throw new Exception(result.ErrorMessage);
                }
                ReconciliationList.Add(myReconciliationPM);
            }



        }

        public virtual ValidationResult ValidateReconcile(ReconciliationPM myReconciliationPM)
        {
            var validContext = AccountingValidationContextServiceProvider.NewReconciliationValidatorContext((this._AccountingContext as IAccountingContext), myReconciliationPM);
            var result = ReconciliationValidator.IsReconciliationValid(myReconciliationPM, validContext);
            return result;
        }

        public virtual List<LedgerTransactionPM> GetLedgerTransactionToReconcile(List<string> theReconcileAgainstLTranIdList)
        {
            var qs = new LedgerTransactionQueryService(_AccountingContext);

            var myOldTransToReconcile = qs.GetLedgerTransactionPMsByIdList(theReconcileAgainstLTranIdList, _JournalPM.Tenant);
            return myOldTransToReconcile;
        }

        private string ResolvedCreatedByUserId()
        {
            throw new NotImplementedException();
        }

        

       
    }
}
