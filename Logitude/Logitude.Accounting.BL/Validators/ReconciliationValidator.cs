using Logitude.Server.Tools.Helpers;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Accounting.BL.Validators;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Security;
using Logitude.BL.CommonDataModel.EntityQueries;

namespace Logitude.Accounting.BL.Validators
{
    public partial class ReconciliationValidator
    {
        public static ValidationResult IsReconciliationValid(ReconciliationPM myReconciliationPM, System.ComponentModel.DataAnnotations.ValidationContext context)
        {

            //bool useLocal = !(GetLoggedContact(myReconciliationPM.Tenant).DontShowLocal);

            bool useLocal = true;
            var user = GetLoggedContact(myReconciliationPM.Tenant);
            if (user != null) useLocal = !(GetLoggedContact(myReconciliationPM.Tenant).DontShowLocal);

            string txt_YouShouldSelectTwoTransactions = TranslateTextsClass.Translate("Journal.M.YouShouldSelectTwoTransactions", 0, useLocal);

            List<string> errorsList = new List<string>();
            if (myReconciliationPM.ReconciliationLines.Count == 0)
            {
                //valid = false;
                //AddError(errorsList,TextCodesTranslator.TranslateText("Journal.M.YouShouldHaveOneLineAtLeast", myReconciliationPM.Tenant));
                AddError(errorsList, txt_YouShouldSelectTwoTransactions);
            }
            if (myReconciliationPM.ReconciliationLines.Count == 1)
            {
                //valid = false;
                AddError(errorsList, txt_YouShouldSelectTwoTransactions);
                //AddError(errorsList,"Reconciliation myReconciliationPM.ReconciliationLines.Count == 1");
            }
            var myDataProvider = context.GetService(typeof(IReconciliationValidatorContextDataProvider)) as IReconciliationValidatorContextDataProvider;
            
            var transactionIdList = myReconciliationPM.ReconciliationLines.Select(rec => rec.TransactionId).ToList();
            List<LedgerTransactionPM> ledgerTransactionPMs=null;
            //List<LedgerTransactionPM> ledgerTransactionPMsUpdated = null;
            GLAccountPM myGLAccount=null;
            if (myDataProvider != null)
            {
                ledgerTransactionPMs = myDataProvider.GetLedgerTransactionPMsByIdList(transactionIdList, myReconciliationPM.Tenant);
                myGLAccount = myDataProvider.GetGLAccount(myReconciliationPM.AccountId, myReconciliationPM.Tenant);
                
            }
            // no MultiCurrency Reconcile
            var listCurrency = myReconciliationPM.ReconciliationLines.Select(rec => rec.CurrencyId).Distinct().ToList();
            if (listCurrency.Count() > 1)
            {
                AddError(errorsList,"Eyal Said no MultiCurrency Reconcile " + string.Join(",",listCurrency.ToArray()));
            }

            //if (myReconciliationPM.ReconciliationLines.Exists(r => r.ChangeSetOp != ChangeSetOperation.Insert))
            //{
            //    AddError(errorsList,"ReconciliationLines can only be insert mode ");//i think so !!!????
            //}
            var repeating = myReconciliationPM.ReconciliationLines.GroupBy(r => r.TransactionId).Where(g => g.Count() > 1).ToList();
            if (repeating.Count > 0)
            {
                AddError(errorsList,"ReconciliationLines Have use TransactionId few times " + repeating.First().First().TransactionId);
            }
            if (myReconciliationPM.DeletedReconciliationLines.Any())
            {
                AddError(errorsList, "init DeletedReconciliationLines is not allowed ");
            }

            if (myReconciliationPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                if (myReconciliationPM.ReconciliationLines.Any(r => r.ChangeSetOp != ChangeSetOperation.None))
                {
                    AddError(errorsList, "ReconciliationLines can only insert ");
                }
            }
            decimal sum = 0;
            foreach (var reconciliationLine in myReconciliationPM.ReconciliationLines)
            {
                if (reconciliationLine.ChangeSetOp != ChangeSetOperation.None)// in 
                {
                    CheckReconciliationLine(errorsList, ledgerTransactionPMs, myGLAccount, ref sum, reconciliationLine);
                }
            }
            if (sum != 0)
            {
                AddError(errorsList,//TextCodesTranslator.TranslateText("Journal.M.JournalAmountNotMatched", myReconciliationPM.Tenant)
                    "the sum of Amount to reconcile of all selected transactions is not zero "
                    );
            }


            if (errorsList.Count == 0)
            {
                if (ledgerTransactionPMs!= null)
                {
                    //myReconciliationPM.CurrentContextTag = ledgerTransactionPMs;
                   
                }
                return ValidationResult.Success;
            }

            else
            {
                string errorString = String.Empty;
                foreach (string error in errorsList)
                {
                    errorString = errorString + error + ",";
                }

                errorString = errorString.Remove(errorString.Length - 1);
                return new ValidationResult(errorString);
            }
        }

        private static void CheckReconciliationLine(List<string> errorsList, List<LedgerTransactionPM> ledgerTransactionPMs, GLAccountPM myGLAccount,ref decimal sum, ReconciliationLinePM reconciliationLine)
        {
            if (reconciliationLine.ChangeSetOp != ChangeSetOperation.Insert)
            {
                AddError(errorsList, "ReconciliationLines can only be insert mode ");//i think so !!!????
            }
            if (String.IsNullOrWhiteSpace(reconciliationLine.TransactionId))
            {
                AddError(errorsList, "String.IsNullOrWhiteSpace(reconciliationLine.TransactionId) line " + reconciliationLine.Line);
            }
            
            if (String.IsNullOrWhiteSpace(reconciliationLine.TransactionId))
            {
                AddError(errorsList, "String.IsNullOrWhiteSpace(reconciliationLine.TransactionId) line " + reconciliationLine.Line);
            }
            else
            {
                if (ledgerTransactionPMs != null)
                {
                    var ledgerTransactionPM = ledgerTransactionPMs.FirstOrDefault(rec => rec.Id == reconciliationLine.TransactionId);
                    if (ledgerTransactionPM == null)
                    {
                        AddError(errorsList, " TransactionId Not in DB" + reconciliationLine.TransactionId);
                    }
                    else
                    {
                        if (ledgerTransactionPM.IsReconciled)
                        {
                            AddError(errorsList, " ledgerTransaction already Reconciled  ?? ? TransactionId=" + reconciliationLine.TransactionId);
                        }
                        if (ledgerTransactionPM.OpenAmountCurrencyId != reconciliationLine.CurrencyId)
                        {
                            AddError(errorsList, " (ledgerTransactionPM.OpenAmountCurrencyId != reconciliationLine.CurrencyId)" + reconciliationLine.TransactionId);
                        }
                        if (myGLAccount.ReconcileMethodCode ==
                            //AccountingSettingResolver.ResolveLocalCurrency0ReconcileMethodCode()
            "0" //((int)Logitude.Accounting.BL.EntityPMs.ReconcileMethodPM.ReconcileMethodEnum.LocalCurrency).ToString()
            ) //Local Currency
                        {
                            var sumLocal = ledgerTransactionPM.LocalAmountDebit - ledgerTransactionPM.LocalAmountCredit;

                        }
                        else
                        {

                            var sumForeign = ledgerTransactionPM.ForeignAmountDebit - ledgerTransactionPM.ForeignAmountCredit;

                        }
                        if (ledgerTransactionPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
                        {
                            if (!ledgerTransactionPM.IsReconciled)
                            {
                                AddError(errorsList, "Insert ledger Transaction but Is not Reconciled " );
                            }
                        }
                        if (ledgerTransactionPM.OpenAmount == 0)
                        {
                            if (reconciliationLine.ReconciliationAmount != ledgerTransactionPM.OpenAmount)
                            {
                                AddError(errorsList, "Open Amount == 0 but (Reconciliation Amount not equal Open Amount of transaction) ");
                            }
                        }
                        else if (ledgerTransactionPM.OpenAmount > 0)
                        {
                            if (reconciliationLine.ReconciliationAmount <= 0 || reconciliationLine.ReconciliationAmount > ledgerTransactionPM.OpenAmount)
                            {
                                AddError(errorsList, "Reconciliation Amount have to be greter than 0 and less than Open Amount of transaction " );
                            }
                        }
                        else //if (ledgerTransactionPM.OpenAmount < 0)
                        {
                            if (reconciliationLine.ReconciliationAmount > 0 || reconciliationLine.ReconciliationAmount < ledgerTransactionPM.OpenAmount)
                            {
                                AddError(errorsList, "ReconciliationAmount have to be Less than 0 and more than Open Amount of transaction " );
                            }
                        }








                    }
                }
            }

            sum += reconciliationLine.ReconciliationAmount;
            
        }

        private static void AddError(List<string> errorsList, string mess)
        {
            errorsList.Add(mess);
        }

        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }


        private static ContactPM GetLoggedContact(int tenant)
        {

            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }
            ContactPM loggedContact = new ContactQuery(tenant).GetContactByEmailOnly(
                //SecurityUtility.GetAuthenticatedUser()
                AuthenticationUtil.ResolveUserIdentityName(tenant)
                , tenant);
            if (loggedContact == null)
            {
                loggedContact = new ContactQuery(tenant).GetContactByEmailOnly("system@tenant" + tenant + ".com", tenant);
            }
            loggedContact = loggedContact ?? new Logitude.BL.CommonDataModel.EntityPMs.ContactPM() { DontShowLocal = true };
            return loggedContact;
        }
    }
}
