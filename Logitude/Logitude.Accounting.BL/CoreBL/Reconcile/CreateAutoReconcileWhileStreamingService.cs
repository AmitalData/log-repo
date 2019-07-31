
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
                return;//nothing to do !!!
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
                ReconciliationPM myReconciliationPM = CreateReconciliationPMPerAccount(oldLTransGroupByAccountId);
                ReconciliationList.Add(myReconciliationPM);
            }



        }

        private ReconciliationPM CreateReconciliationPMPerAccount(IGrouping<string, LedgerTransactionPM> oldLTransGroupByAccountId)
        {
            var currentAccountId = oldLTransGroupByAccountId.Key;
            var currrentAccountJournalReconcileList =
                (from jr in _JournalPM.JournalReconciles
                 join oldLTrans in oldLTransGroupByAccountId
                 on jr.LedgerTransactionId equals oldLTrans.Id
                 select jr
                 ).ToList();
            decimal totReconciliationAmount = currrentAccountJournalReconcileList.Sum(r => r.ReconciliationAmount);
            decimal totReconciliationAmountUseAsStack = totReconciliationAmount;








            var newLTranListOfAccountID = _NewLedgerTransactionsWithCounters.Where(r => r.AccountId == currentAccountId).ToList();
            decimal totNew = newLTranListOfAccountID.Sum(r => r.OpenAmount);
            //if (totNew + totReconciliationAmount != 0)
            if (Math.Abs(totNew) < Math.Abs( totReconciliationAmount))
            {
                throw new Exception($"for JournalPM.Id ={_JournalPM.Id} Account {currentAccountId}  (totNew != totReconciliationAmount) = ({totNew} >= -1* {totReconciliationAmount})");
            }

            ReconciliationPM myReconciliationPM = GetReconciliationPM(currentAccountId);

            int lineCounter = 1;
            AddRecoLines_FromNewTransaction_FromRecoStackAmount(ref totReconciliationAmountUseAsStack, newLTranListOfAccountID, myReconciliationPM, ref lineCounter);
            AddRecoLines_FromOldDBTransaction(oldLTransGroupByAccountId, myReconciliationPM, ref lineCounter);


            if (totReconciliationAmountUseAsStack < 0)
            {

                throw new Exception($"for JournalPM.Id ={_JournalPM.Id} Account {currentAccountId}  (if (totReconciliationAmount < 0)");
            }

            ValidationResult result = ValidateReconcile(myReconciliationPM);
            if (result != null)
            {

                throw new Exception(result.ErrorMessage);
            }

            return myReconciliationPM;
        }

        private ReconciliationPM GetReconciliationPM(string currentAccountId)
        {
            var myReconciliationPM = new ReconciliationPM();
            myReconciliationPM.Id = "new";
            myReconciliationPM.ChangeSetOp = ChangeSetOperation.Insert;
            myReconciliationPM.Tenant = _JournalPM.Tenant;
            myReconciliationPM.AccountId = currentAccountId;
            myReconciliationPM.Number = "get";
            myReconciliationPM.CreateDate = DateTime.UtcNow;
            myReconciliationPM.CreatedByUserId = _JournalPM.UpdatedByUserId; ///ResolvedCreatedByUserId();
            return myReconciliationPM;
        }


        private void AddRecoLines_FromOldDBTransaction(IGrouping<string, LedgerTransactionPM> oldLTransGroupByAccountId, ReconciliationPM myReconciliationPM, ref int lineCounter)
        {
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
                myReconciliationLinePM.ReconciliationAmount = journalReconcile.ReconciliationAmount;
                myReconciliationLinePM.GroupNumber = 1;
                myReconciliationPM.ReconciliationLines.Add(myReconciliationLinePM);
            }


        }
        private static void AddRecoLines_FromNewTransaction_FromRecoStackAmount(ref decimal totReconciliationAmountUseAsStack, List<LedgerTransactionPM> newLTranListOfAccountID, ReconciliationPM myReconciliationPM, ref int lineCounter)
        {
            foreach (var newLTran in newLTranListOfAccountID)
            {
                ReconciliationLinePM myReconciliationLinePM = GetRecoLineFromNewLTRansSetReconciliationAmountFromStack(ref totReconciliationAmountUseAsStack, myReconciliationPM, ref lineCounter, newLTran);
                myReconciliationPM.ReconciliationLines.Add(myReconciliationLinePM);

            }
        }
        private static ReconciliationLinePM GetRecoLineFromNewLTRansSetReconciliationAmountFromStack(ref decimal totReconciliationAmount, ReconciliationPM myReconciliationPM, ref int lineCounter, LedgerTransactionPM newLTran)
        {
            decimal newLTranReconciliationAmount = CalcNewLTranReconciliationAmount(ref totReconciliationAmount, newLTran);
            var myReconciliationLinePM = new ReconciliationLinePM();
            myReconciliationLinePM.ChangeSetOp = ChangeSetOperation.Insert; ;
            myReconciliationLinePM.ReconciliationId = myReconciliationPM.Id;
            myReconciliationLinePM.Tenant = myReconciliationPM.Tenant;
            myReconciliationLinePM.Line = lineCounter++;
            myReconciliationLinePM.CurrencyId = newLTran.OpenAmountCurrencyId;
            myReconciliationLinePM.TransactionId = newLTran.Id;
            myReconciliationLinePM.ReconciliationAmount = newLTranReconciliationAmount /*newLTran.OpenAmount*/;
            myReconciliationLinePM.GroupNumber = 1;
            return myReconciliationLinePM;
        }

        private static decimal CalcNewLTranReconciliationAmount(ref decimal totReconciliationAmount, LedgerTransactionPM newLTran)
        {
            decimal newLTranReconciliationAmount = 0;
            if (Math.Abs(newLTran.OpenAmount) <= Math.Abs(totReconciliationAmount))
            {
                newLTranReconciliationAmount = newLTran.OpenAmount;
                totReconciliationAmount = totReconciliationAmount + newLTran.OpenAmount;
            }
            else
            {
                newLTranReconciliationAmount = -1 * totReconciliationAmount;
                newLTranReconciliationAmount = -1 * totReconciliationAmount;
            }

            return newLTranReconciliationAmount;
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
