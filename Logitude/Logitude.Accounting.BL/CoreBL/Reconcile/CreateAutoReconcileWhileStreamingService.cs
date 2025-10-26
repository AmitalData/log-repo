
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
using Logitude.Server.Tools.Helpers;
using Logitude.BL.Resolvers;
using Logitude.BL.Interfaces;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Microsoft.Practices.ObjectBuilder2;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;

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
            var myOldTransToReconcile = GetLedgerTransactionToReconcile(theReconcileAgainstLTranIdList);


            if (myOldTransToReconcile.Any(r => r.IsReconciled && r.AmountToReconcile!=0))
            {
                if (!_JournalPM.IsVoided.GetValueOrDefault())//while voiding -old transaction IsReconciled change after !!
                {
                    string errorMessage = "לא ניתן להמשיך כי לפחות אחת מהשורות כבר הותאמה ראה את : \n ";
                    foreach (var myOldTransToReconcileError in myOldTransToReconcile.Where(r => r.IsReconciled))
                    {
                       errorMessage += "שורה: "+ myOldTransToReconcileError.JournalLineNumber + ",בסכום: " + myOldTransToReconcileError.OpenAmount +"\n";
                        
                    }

                    throw new ApplicationException(errorMessage);
                }

            }


            if (CheckINprogress && myOldTransToReconcile.Any(r => !r.InReconcileProgress))
            {
                throw new ApplicationException("_JournalPM.JournalReconciles have  myOldTransToReconcile.Any( r=> !r.InReconcileProgress) ");
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

           NetCommonHelper.Logger.DevLog.Instance.WriteDebug("jsonOldTransToReconcile=");
           NetCommonHelper.Logger.DevLog.Instance.WriteDebug(jsonOldTransToReconcile);

           NetCommonHelper.Logger.DevLog.Instance.WriteDebug("jsonJournalPM=");
           NetCommonHelper.Logger.DevLog.Instance.WriteDebug(jsonJournalPM);

           NetCommonHelper.Logger.DevLog.Instance.WriteDebug("jsonNewLedgerTransactionsWithCounters=");
           NetCommonHelper.Logger.DevLog.Instance.WriteDebug(jsonNewLedgerTransactionsWithCounters);

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
            if (newLTranListOfAccountID == null || newLTranListOfAccountID.Count == 0)
            {
                string errortext = $"Account {currentAccountId} is not found in Ledger Transactions to be reconciled"; 
                throw new ApplicationException(errortext);
            }
            decimal totalNewLedgerOpenAmount = newLTranListOfAccountID.Sum(r => r.OpenAmount);

            bool Same_glaccount_for_debit_and_creditV2Enable = true;
            bool isAdjustJournalSameAccount = IsAdjustJournalSameAccount(totalNewLedgerOpenAmount);
            bool isAdjustJournalDifferentAccount = IsAdjustJournalDifferentAccount(totalNewLedgerOpenAmount);
            if (Same_glaccount_for_debit_and_creditV2Enable &&
                isAdjustJournalSameAccount
                    )
            {
                totalNewLedgerOpenAmount = OnAdjustJournalSameAccount_UseFirstLine(ref newLTranListOfAccountID);

            }
            else if (isAdjustJournalDifferentAccount)
            {
                //ILoggedContactUtil loggedContactUtil = ContainerAccessor.Container.Resolve(typeof(ILoggedContactUtil), "LoggedContactUtil", new ParameterOverride("", _JournalPM.Tenant)) as ILoggedContactUtil;
                //var loggedcontact = loggedContactUtil.GetLoggedContact(_JournalPM.Tenant);
                //bool showLocal = !(bool)loggedcontact?.DontShowLocal;
                // var contact = LoggedContactResolver.GetLoggedContact(_JournalPM.Tenant);
                throw new ApplicationException("Reconciliation.O.MultiCurrencyGlaccountReconciliation");
            }
            else
            {

                // if accountId is same for credit & debit tranasction, we get only credit tranasctions
                if (newLTranListOfAccountID.Count == _NewLedgerTransactionsWithCounters.Count && Math.Abs(totalNewLedgerOpenAmount) != Math.Abs(totalAmountFromJournalReconciliation))
                {
                    newLTranListOfAccountID = newLTranListOfAccountID.Where(r => r.LocalAmountCredit > 0).ToList();
                }
                //if (totNew + totReconciliationAmount != 0)
                bool inMaynTheARPaymentCreateDebitCreditAgainstKUPA = true;
                if (Math.Abs(totalNewLedgerOpenAmount) < Math.Abs(totalAmountFromJournalReconciliation))
                {

                    if (inMaynTheARPaymentCreateDebitCreditAgainstKUPA &&
                    currrentAccountJournalReconcileList.Count() == 1 &&
                    //_NewLedgerTransactionsWithCounters.Count == 2 &&
                    _NewLedgerTransactionsWithCounters[0].AccountId == _NewLedgerTransactionsWithCounters[1].AccountId
                    )
                    {
                       NetCommonHelper.Logger.DevLog.Instance.WriteDebug("במעיין ARPayment  שורה לחיוב ה הקופה ושורה לזיכוי הקופה");
                       NetCommonHelper.Logger.DevLog.Instance.WriteDebug("צריך להשתמש בשורה לזכות !!");
                        newLTranListOfAccountID = newLTranListOfAccountID.Where(r => r.LocalAmountCredit != 0).ToList();
                        totalNewLedgerOpenAmount = newLTranListOfAccountID.Sum(r => r.OpenAmount);


                    }
                }
            }
            if (Math.Abs(totalNewLedgerOpenAmount) < Math.Abs(totalAmountFromJournalReconciliation))
            {
                throw new ApplicationException($"for JournalPM.Id ={_JournalPM.Id} Account {currentAccountId}  (totNew != totReconciliationAmount) = ({totalNewLedgerOpenAmount} >= -1* {totalAmountFromJournalReconciliation})");
            }
            bool isPartialReconciliation = (Math.Abs(totalNewLedgerOpenAmount) > Math.Abs(totalAmountFromJournalReconciliation));

            if (isPartialReconciliation)
            {
               NetCommonHelper.Logger.DevLog.Instance.WriteDebug("isPartialReconciliation!!!! eyal said only 1 oldTRans Against 1 newTrans");
                if (newLTranListOfAccountID.Count != 1 || oldLTransGroupByAccountId.Count() != 1)
                {
                    //throw new ApplicationException("isPartialReconciliation!!!! eyal said only 1 oldTRans Against 1 newTrans");
                }

            }
            else
            {
               NetCommonHelper.Logger.DevLog.Instance.WriteDebug("NOT!!! Partial Reconciliation!!!! new.amount againt  old.amount = (eyal said reference1 + 2 +2 + not the same- but who care - its all in 1 group )");
            }

            ReconciliationPM myReconciliationPM = GetReconciliationPM(currentAccountId);

            int lineCounter = 1;
            AddRecoLines_FromNewTransaction_FromRecoStackAmount(ref totalAmountFromJournalReconciliation_AsStack, newLTranListOfAccountID, myReconciliationPM, ref lineCounter, isPartialReconciliation);
            AddRecoLines_FromOldDBTransaction(oldLTransGroupByAccountId, myReconciliationPM, ref lineCounter);

            if (isPartialReconciliation)
            {
                if (totalAmountFromJournalReconciliation_AsStack < 0)
                {

                    throw new ApplicationException($"for JournalPM.Id ={_JournalPM.Id} Account {currentAccountId}  (if (totReconciliationAmount < 0)");
                }
            }
            ValidationResult result = ValidateReconcile(myReconciliationPM);
            if (result != null)
            {

                throw new ApplicationException(result.ErrorMessage);
            }

            return myReconciliationPM;
        }

        private bool IsAdjustJournalSameAccount(decimal totalNewLedgerOpenAmount)
        {
            return _JournalPM.AccountingEntityCode == "10" /*Reconciliation*/ &&
                            totalNewLedgerOpenAmount == 0 && // its  adjust !!
                            _NewLedgerTransactionsWithCounters.Count == 2 &&
                            _NewLedgerTransactionsWithCounters[0].AccountId == _NewLedgerTransactionsWithCounters[1].AccountId;
        }

        private bool IsAdjustJournalDifferentAccount(decimal totalNewLedgerOpenAmount)
        {
            return _JournalPM.AccountingEntityCode == "10" /*Reconciliation*/ &&
                            totalNewLedgerOpenAmount == 0 && // its  adjust !!
                            _NewLedgerTransactionsWithCounters.Count == 2 &&
                            _NewLedgerTransactionsWithCounters[0].AccountId != _NewLedgerTransactionsWithCounters[1].AccountId;
        }

        private static decimal OnAdjustJournalSameAccount_UseFirstLine(ref List<LedgerTransactionPM> newLTranListOfAccountID)
        {
            decimal totalNewLedgerOpenAmount;
           NetCommonHelper.Logger.DevLog.Instance.WriteDebug(
@"Task 75738: ADJUST SERVICE- allow the user to define chose the same glaccount for debit and credit
במקרה שהחן הנגדי == החשבון
המטרה בעצם להעביר את ההפרש לתאריך אחר
אנו נתאים את כל שורות ההתאמה הישנות מול 
תנועה אחת *בלבד* מהתנעות החדשות מהפקודה שיצרנו
ללא התנועה השניה
");
           NetCommonHelper.Logger.DevLog.Instance.WriteDebug("באם הסכום של כל התנועות החדשות לחן הינו אפס דאז זה להתאמה ");
           NetCommonHelper.Logger.DevLog.Instance.WriteDebug("בשורה הראשונה יש את ההפרש להתאמה מול הכרטיס (בשורה השניה לחן ההפרשים) !!");
            //newLTranListOfAccountID = newLTranListOfAccountID.Where(r => r.LocalAmountCredit != 0).ToList();
            // adjust journal 
            //- the first line its the diff amount to adujust 
            //- the seond move the diff to the diffAccount
            newLTranListOfAccountID = new List<LedgerTransactionPM>() { newLTranListOfAccountID.First() };
            totalNewLedgerOpenAmount = newLTranListOfAccountID.Sum(r => r.OpenAmount);
            return totalNewLedgerOpenAmount;
        }

        private decimal MoveAdjustSum2SameAccountButDiffDate_useOnly1NewTransaction(ref List<LedgerTransactionPM> newLTranListOfAccountID)
        {
            decimal totalNewLedgerOpenAmount;
           NetCommonHelper.Logger.DevLog.Instance.WriteDebug(
@"Task 75738: ADJUST SERVICE- allow the user to define chose the same glaccount for debit and credit
במקרה שהחן הנגדי == החשבון
המטרה בעצם להעביר את ההפרש לתאריך אחר
אנו נתאים את כל שורות ההתאמה הישנות מול 
תנועה אחת *בלבד* מהתנעות החדשות מהפקודה שיצרנו
ללא התנועה השניה
");

            newLTranListOfAccountID = UseOnlyOneTransactionFromTheNewJournal(newLTranListOfAccountID);
            totalNewLedgerOpenAmount = newLTranListOfAccountID.Sum(r => r.OpenAmount);
            if (_JournalPM.JournalReconciles.Sum(r => r.ReconciliationAmount) != totalNewLedgerOpenAmount)
            {
                throw new ApplicationException("never tested- Task 75738: ADJUST SERVICE- allow the user to define chose the same glaccount for debit and credit");
            }

            return totalNewLedgerOpenAmount;
        }

        private List<LedgerTransactionPM> UseOnlyOneTransactionFromTheNewJournal(List<LedgerTransactionPM> newLTranListOfAccountID)
        {
            if (
                             //_JournalPM.JournalLines.First().ActionTypeCodeEnum == MyJournalActionTypeEnum.Credit
                             _JournalPM.JournalReconciles.Sum(r => r.ReconciliationAmount) > 0
                             )
            {
                newLTranListOfAccountID = newLTranListOfAccountID.Where(r => r.LocalAmountCredit != 0).ToList();
            }
            else
            {
                newLTranListOfAccountID = newLTranListOfAccountID.Where(r => r.LocalAmountDebit != 0).ToList();
            }

            return newLTranListOfAccountID;
        }

        private ReconciliationPM GetReconciliationPM(string currentAccountId)
        {
            var myReconciliationPM = new ReconciliationPM();
            myReconciliationPM.CreateAutoReconcileWhileStreamingService = true;
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
            TenantQuery tenantQuery = new TenantQuery(myReconciliationPM.Tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(myReconciliationPM.Tenant);
            string tenantCurrency = tenantPM.CurrencyId;

            foreach (var oldLedger in oldLTransGroupByAccountId)
            {
                var journalReconcile = _JournalPM.JournalReconciles.First(r => r.LedgerTransactionId == oldLedger.Id);
                var myReconciliationLinePM = new ReconciliationLinePM();
                myReconciliationLinePM.ChangeSetOp = ChangeSetOperation.Insert; ;
                myReconciliationLinePM.ReconciliationId = myReconciliationPM.Id;
                myReconciliationLinePM.Tenant = myReconciliationPM.Tenant;
                myReconciliationLinePM.Line = lineCounter++;
                myReconciliationLinePM.TransactionId = journalReconcile.LedgerTransactionId;
                myReconciliationLinePM.CurrencyRate = oldLedger.ExchangeRate;

                if (!String.IsNullOrEmpty(oldLedger.OpenAmountCurrencyId) && !String.IsNullOrEmpty(journalReconcile.CurrencyId)
                    && journalReconcile.CurrencyId != oldLedger.OpenAmountCurrencyId) // journalReconcile currency does not match the account's reco. method
                {
                    if (journalReconcile.CurrencyId != oldLedger.CurrencyId)
                    {
                        string errorMessage = "JournalReconcile Currency is not " + tenantPM.CurrencyCode + " and not " + oldLedger.CurrencyCode
                            + Environment.NewLine + ", Journal " + oldLedger.JournalNumber + ", Line " + oldLedger.JournalLineNumber
                            + Environment.NewLine + ", JournalReconcile " + journalReconcile.JournalNumber + ", LT=" + journalReconcile.LedgerTransactionId + ", Old Ledger Transactiom " + oldLedger.Id ;

                        throw new ApplicationException(errorMessage);

                    }

                    if (oldLedger.OpenAmountCurrencyId == tenantCurrency)  // Account's reco. method is local currency, journalReconcile must be in foreign
                    {
                        if (journalReconcile.ReconciliationAmount != oldLedger.ForeignAmountDebit - oldLedger.ForeignAmountCredit) // checking whole amount 
                        {
                            string errorMessage = "JournalReconcile amount " + journalReconcile.ReconciliationAmount.ToString() 
                                + " differs from " + oldLedger.CurrencyCode + " " + (oldLedger.ForeignAmountDebit - oldLedger.ForeignAmountCredit).ToString()
                                + Environment.NewLine + ", Journal " + oldLedger.JournalNumber + ", Line " + oldLedger.JournalLineNumber
                                + Environment.NewLine + ", JournalReconcile " + journalReconcile.JournalNumber + ", LT=" + journalReconcile.LedgerTransactionId + ", Old Ledger Transactiom " + oldLedger.Id;
                            // because we cannot say how much to reconcile in local

                            throw new ApplicationException(errorMessage);
                        }

                        // local.
                        myReconciliationLinePM.CurrencyId = tenantCurrency;
                        myReconciliationLinePM.ReconciliationAmount = oldLedger.LocalAmountDebit - oldLedger.LocalAmountCredit;

                    }

                    else // Account's reco. method is foreign currency, journalReconcile must be in local
                    {
                        if (journalReconcile.ReconciliationAmount != oldLedger.LocalAmountDebit - oldLedger.LocalAmountCredit) // checking whole amount
                        {
                            string errorMessage = "JournalReconcile amount " + journalReconcile.ReconciliationAmount.ToString()
                                + " differs from " + tenantPM.CurrencyCode + " " + (oldLedger.LocalAmountDebit - oldLedger.LocalAmountCredit).ToString()
                                + Environment.NewLine + ", Journal " + oldLedger.JournalNumber + ", Line " + oldLedger.JournalLineNumber
                                + Environment.NewLine + ", JournalReconcile " + journalReconcile.JournalNumber + ", LT=" + journalReconcile.LedgerTransactionId + ", Old Ledger Transactiom " + oldLedger.Id;
                            // because we cannot say how much to reconcile in foreign

                            throw new ApplicationException(errorMessage);
                        }

                        // foreign.
                        myReconciliationLinePM.CurrencyId = oldLedger.CurrencyId;
                        myReconciliationLinePM.ReconciliationAmount = oldLedger.ForeignAmountDebit - oldLedger.ForeignAmountCredit;
                    }
                }

                else
                {
                    myReconciliationLinePM.CurrencyId = journalReconcile.CurrencyId;
                    myReconciliationLinePM.ReconciliationAmount = journalReconcile.ReconciliationAmount;
                }



                myReconciliationLinePM.GroupNumber = 1;
                myReconciliationPM.ReconciliationLines.Add(myReconciliationLinePM);
            }


        }
        private static void AddRecoLines_FromNewTransaction_FromRecoStackAmount(ref decimal totReconciliationAmountUseAsStack, List<LedgerTransactionPM> newLTranListOfAccountID, ReconciliationPM myReconciliationPM, ref int lineCounter, bool isPartialReconciliation)
        {
            var orderedLedgerTransactions = newLTranListOfAccountID.OrderBy(e => e.ForeignAmountCredit);
            foreach (var newLTran in orderedLedgerTransactions)
            {
                ReconciliationLinePM myReconciliationLinePM = GetRecoLineFromNewLTRansSetReconciliationAmountFromStack(ref totReconciliationAmountUseAsStack, myReconciliationPM, ref lineCounter, newLTran, isPartialReconciliation);
                //if(myReconciliationLinePM.ReconciliationAmount != 0)
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
            myReconciliationLinePM.CurrencyRate = newLTran.ExchangeRate;
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
                ///throw new ApplicationException("test !!");
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

            var myOldTransToReconcile = qs.GetLedgerTransactionDTOByIdList(theReconcileAgainstLTranIdList, _JournalPM.Tenant);
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
