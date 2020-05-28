
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
using Logitude.Server.Tools.Utils;
using System.Diagnostics;

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
        public void CreateAutoReconcileWhileStreaming(bool CheckINprogress)
        {
            
            if (_JournalPM.JournalReconciles.Count == 0)
            {
                return;//nothing to do !!!
            }
            var theReconcileAgainstLTranIdList = _JournalPM.JournalReconciles.Select(r => r.LedgerTransactionId).ToList();
            List<LedgerTransactionPM> myOldTransToReconcile = GetLedgerTransactionToReconcile(theReconcileAgainstLTranIdList);
            if (CheckINprogress && myOldTransToReconcile.Any(r => !r.InReconcileProgress))
            {
                throw new Exception("_JournalPM.JournalReconciles have  myOldTransToReconcile.Any( r=> !r.InReconcileProgress) ");
            }
            MakeTesterIfNeeded(myOldTransToReconcile);
            //From JournalReconcile take old Ledger  - Match new Ledger 
            foreach (var oldLTransGroupByAccountId in myOldTransToReconcile.GroupBy(r => r.AccountId))
            {
                ReconciliationPM myReconciliationPM = CreateReconciliationPMPerAccount(oldLTransGroupByAccountId);
                ReconciliationList.Add(myReconciliationPM);
            }



        }

        private void MakeTesterIfNeeded(List<LedgerTransactionPM> myOldTransToReconcile)
        {
            bool MakeTester = false;
            if (!MakeTester) return;
            string jsonOldTransToReconcile =ProxyUtil.JsonConvertSerialize(myOldTransToReconcile);
            string jsonJournalPM = ProxyUtil.JsonConvertSerialize(_JournalPM);
            string jsonNewLedgerTransactionsWithCounters = ProxyUtil.JsonConvertSerialize(_NewLedgerTransactionsWithCounters);

            Debug.WriteLine("jsonOldTransToReconcile=");
            Debug.WriteLine(jsonOldTransToReconcile);

            Debug.WriteLine("jsonJournalPM=");
            Debug.WriteLine(jsonJournalPM);

            Debug.WriteLine("jsonNewLedgerTransactionsWithCounters=");
            Debug.WriteLine(jsonNewLedgerTransactionsWithCounters);

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
            decimal totalAmountFromJournalReconciliation = currrentAccountJournalReconcileList.Sum(r => r.ReconciliationAmount);
            decimal totalAmountFromJournalReconciliation_AsStack = totalAmountFromJournalReconciliation;








            var newLTranListOfAccountID = _NewLedgerTransactionsWithCounters.Where(r => r.AccountId == currentAccountId).ToList();
            decimal totalNewLedgerOpenAmount = newLTranListOfAccountID.Sum(r => r.OpenAmount);
            //if (totNew + totReconciliationAmount != 0)
            bool inMaynTheARPaymentCreateDebitCreditAgainstKUPA = true;
            if (Math.Abs(totalNewLedgerOpenAmount) < Math.Abs(totalAmountFromJournalReconciliation)) {
                
                if (inMaynTheARPaymentCreateDebitCreditAgainstKUPA &&
                currrentAccountJournalReconcileList.Count() == 1 &&
                _NewLedgerTransactionsWithCounters.Count == 2 &&
                _NewLedgerTransactionsWithCounters[0].AccountId == _NewLedgerTransactionsWithCounters[1].AccountId
                )
                {
                    Debug.WriteLine("במעיין ARPayment  שורה לחיוב ה הקופה ושורה לזיכוי הקופה");
                    Debug.WriteLine("צריך להשתמש בשורה לזכות !!");
                    newLTranListOfAccountID = newLTranListOfAccountID.Where(r => r.LocalAmountCredit != 0).ToList();
                    totalNewLedgerOpenAmount = newLTranListOfAccountID.Sum(r => r.OpenAmount);


                }
            }
            if (Math.Abs(totalNewLedgerOpenAmount) < Math.Abs(totalAmountFromJournalReconciliation))
            {
                throw new Exception($"for JournalPM.Id ={_JournalPM.Id} Account {currentAccountId}  (totNew != totReconciliationAmount) = ({totalNewLedgerOpenAmount} >= -1* {totalAmountFromJournalReconciliation})");
            }
            bool isPartialReconciliation = (Math.Abs(totalNewLedgerOpenAmount) > Math.Abs(totalAmountFromJournalReconciliation));

            if (isPartialReconciliation)
            {
                Debug.WriteLine("isPartialReconciliation!!!! eyal said only 1 oldTRans Against 1 newTrans");
                if (newLTranListOfAccountID.Count!=1 || oldLTransGroupByAccountId.Count() != 1)
                {
                    //throw new Exception("isPartialReconciliation!!!! eyal said only 1 oldTRans Against 1 newTrans");
                }

            }
            else
            {
                Debug.WriteLine("NOT!!! Partial Reconciliation!!!! new.amount againt  old.amount = (eyal said reference1 + 2 +2 + not the same- but who care - its all in 1 group )");
            }

            ReconciliationPM myReconciliationPM = GetReconciliationPM(currentAccountId);

            int lineCounter = 1;
            AddRecoLines_FromNewTransaction_FromRecoStackAmount(ref totalAmountFromJournalReconciliation_AsStack, newLTranListOfAccountID, myReconciliationPM, ref lineCounter, isPartialReconciliation);
            AddRecoLines_FromOldDBTransaction(oldLTransGroupByAccountId, myReconciliationPM, ref lineCounter);

            if (isPartialReconciliation)
            {
                if (totalAmountFromJournalReconciliation_AsStack < 0)
                {

                    throw new Exception($"for JournalPM.Id ={_JournalPM.Id} Account {currentAccountId}  (if (totReconciliationAmount < 0)");
                }
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
        private static void AddRecoLines_FromNewTransaction_FromRecoStackAmount(ref decimal totReconciliationAmountUseAsStack, List<LedgerTransactionPM> newLTranListOfAccountID, ReconciliationPM myReconciliationPM, ref int lineCounter, bool isPartialReconciliation)
        {
            foreach (var newLTran in newLTranListOfAccountID)
            {
                ReconciliationLinePM myReconciliationLinePM = GetRecoLineFromNewLTRansSetReconciliationAmountFromStack(ref totReconciliationAmountUseAsStack, myReconciliationPM, ref lineCounter, newLTran, isPartialReconciliation);
                myReconciliationPM.ReconciliationLines.Add(myReconciliationLinePM);

            }
        }
        private static ReconciliationLinePM GetRecoLineFromNewLTRansSetReconciliationAmountFromStack(ref decimal totReconciliationAmount, ReconciliationPM myReconciliationPM, ref int lineCounter, LedgerTransactionPM newLTran, bool isPartialReconciliation)
        {
            decimal newLTranReconciliationAmount = //CalcNewLTranReconciliationAmount(ref totReconciliationAmount, newLTran);
                newLTran.OpenAmount;
            if (isPartialReconciliation)
            {
                newLTranReconciliationAmount = CalcNewLTranReconciliationAmount(ref totReconciliationAmount, newLTran);
            }
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
                ///throw new Exception("test !!");
                newLTranReconciliationAmount = -1 * totReconciliationAmount;
                newLTranReconciliationAmount = -1 * totReconciliationAmount;
                totReconciliationAmount = totReconciliationAmount + newLTranReconciliationAmount;
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


    public class CreateAutoReconcileWhileStreamingService4JournalValidation: CreateAutoReconcileWhileStreamingService
    {
        private List<LedgerTransactionPM> _OldTransToReconcile;

        public void InitMe(List<LedgerTransactionPM> myOldTransToReconcile)
        {
            _OldTransToReconcile = myOldTransToReconcile;
        }
        public override ValidationResult ValidateReconcile(ReconciliationPM myReconciliationPM)
        {
            return ValidationResult.Success;
        }

        public override List<LedgerTransactionPM> GetLedgerTransactionToReconcile(List<string> theReconcileAgainstLTranIdList)
        {
            return _OldTransToReconcile;

        }
    }
}
