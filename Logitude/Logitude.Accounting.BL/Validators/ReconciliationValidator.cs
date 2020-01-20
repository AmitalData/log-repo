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
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.BL.Interfaces;
using Microsoft.Practices.Unity;
using Logitude.Server.Tools;
using Logitude.BL.Helpers;
using Logitude.BL.Resolvers;

namespace Logitude.Accounting.BL.Validators
{
    public partial class ReconciliationValidator
    {
        public const string M_YouShouldSelectTwoTransactions = "Journal.M.YouShouldSelectTwoTransactions";
        public const string M_SaidnoMultiCurrencyReconcile = "Eyal Said no MultiCurrency Reconcile ";
        public const string M_ReconciliationLinesHaveuseTransactionIdfewtimes = "ReconciliationLines Have use TransactionId few times ";
        public const string M_DeletedReconciliationLinesisnotallowed = "init DeletedReconciliationLines is not allowed ";
        public const string M_ReconciliationLinescanonlyinsert = "ReconciliationLines can only insert ";
        public const string M_CantIncludeTwoOrMorePayment = "Accounting.O.CantIncludeTwoOrMorePayment";
        public const string M_ReconciliationLinescanonlybeinsertmode = "ReconciliationLines can only be insert mode ";

        public const string M_reconciliationLineTransactionIdIsnull = "String.IsNullOrWhiteSpace(reconciliationLine.TransactionId) line ";

        public const string M_TransactionIdNotinDB = " TransactionId Not in DB";
        public const string M_ledgerTransactionalreadyReconciled = " ledgerTransaction already Reconciled  ?? ? TransactionId=";
        public const string M_OpenAmountCurrencyIdDiffreconciliationLineCurrencyId = "(ledgerTransactionPM.OpenAmountCurrencyId != reconciliationLine.CurrencyId)";
        public const string M_InsertledgerTransactionbutIsnotReconciled = "Insert ledger Transaction but Is not Reconciled ";

        public const string M_OpenAmountis0ReconcileNot = "Open Amount == 0 but (Reconciliation Amount not equal Open Amount of transaction) ";

        public const string M_OpenAmountGreater0ReconcileNot0toOpenAmout = "Reconciliation Amount have to be greter than 0 and less than Open Amount of transaction ";
        public const string M_OpenAmountLess0ReconcileNoOpenAmoutto0 = "ReconciliationAmount have to be Less than 0 and more than Open Amount of transaction ";
        public const string M_sumofAmounttoreconcilemUST0 = "the sum of Amount to reconcile of all selected transactions is not zero ";
        public const string M_AfterConversion_no_Journal_Line = "CreatedByReconciliationAfterConversion : no Journal Line";
        public const string M_AfterConversion_noallJournalLinehaveExternalReconcileNumber = "CreatedByReconciliationAfterConversion : no all Journal Line have ExternalReconcileNumber";
        public const string M_AfterConversion_JournalLinehavenotthesameExternalReconcileNumber = "CreatedByReconciliationAfterConversion : Journal Line have not the same ExternalReconcileNumber";

        public static ValidationResult IsReconciliationValid(ReconciliationPM myReconciliationPM, System.ComponentModel.DataAnnotations.ValidationContext context)
        {
            bool useLocal = true;
            var user = GetLoggedContact(myReconciliationPM.Tenant);
            if (user != null) useLocal = !(GetLoggedContact(myReconciliationPM.Tenant).DontShowLocal);
            string txt_YouShouldSelectTwoTransactions = TranslateMyTextCode(/*"Journal.M.YouShouldSelectTwoTransactions"*/M_YouShouldSelectTwoTransactions, 0, useLocal);
            List<string> errorsList = new List<string>();

            if (!myReconciliationPM.IsCancelled)
            {
                // minimum rows
                if (myReconciliationPM.ReconciliationLines.Count == 0)
                {
                    AddError(errorsList, txt_YouShouldSelectTwoTransactions);
                }
                if (myReconciliationPM.ReconciliationLines.Count == 1)
                {
                    AddError(errorsList, txt_YouShouldSelectTwoTransactions);
                }
            }
            

            // no MultiCurrency Reconcile
            var listCurrency = myReconciliationPM.ReconciliationLines.Select(rec => rec.CurrencyId).Distinct().ToList();
            if (listCurrency.Count() > 1)
            {
                AddError(errorsList,/*"Eyal Said no MultiCurrency Reconcile "*/ M_SaidnoMultiCurrencyReconcile + string.Join(",", listCurrency.ToArray()));
            }

            // repeate transaction
            var repeating = myReconciliationPM.ReconciliationLines.GroupBy(r => r.TransactionId).Where(g => g.Count() > 1).ToList();
            if (repeating.Count > 0)
            {
                AddError(errorsList,/*"ReconciliationLines Have use TransactionId few times "*/M_ReconciliationLinesHaveuseTransactionIdfewtimes + repeating.First().First().TransactionId);
            }

            // deleted lines
            if (myReconciliationPM.DeletedReconciliationLines.Any())
            {
                AddError(errorsList, /*"init DeletedReconciliationLines is not allowed "*/M_DeletedReconciliationLinesisnotallowed);
            }

            // updateed lines
            if (myReconciliationPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                if (myReconciliationPM.ReconciliationLines.Any(r => r.ChangeSetOp != ChangeSetOperation.None))
                {
                    AddError(errorsList, /*"ReconciliationLines can only insert "*/M_ReconciliationLinescanonlyinsert);
                }
            }
            var myDataProvider = context.GetService(typeof(IReconciliationValidatorContextDataProvider)) as IReconciliationValidatorContextDataProvider;

            // multiple payment check - TASK 44660
            int paymentsCount = 0;
            //LedgerTransactionQueryService transQuery = new LedgerTransactionQueryService(myReconciliationPM.Tenant);
            List<string> transactionsId = myReconciliationPM.ReconciliationLines.Where(d => d.TransactionId != null).Select(a => a.TransactionId).ToList();
            List<LedgerTransactionPM> transactionsPMList = //transQuery.GetLedgerTransactionPMsByIdList(transactionsId, myReconciliationPM.Tenant);
                myDataProvider.GetLedgerTransactionPMsByIdList(transactionsId, myReconciliationPM.Tenant);
            paymentsCount = transactionsPMList.Count(d => (d.SourceTypeCode == "3" || d.SourceTypeCode == "5") && d.OriginalJournalId == null); // 3- ARPayment , or 5- APPayment , and not storno
            if (paymentsCount > 1)
            {
                //Can’t include more than one payment in the same reconciliation” ?? ???? ????? ???? ????? ??? ????? ?????
                AddError(errorsList, TranslateMyTextCode(/*"Accounting.O.CantIncludeTwoOrMorePayment"*/M_CantIncludeTwoOrMorePayment, 0, useLocal));
            }

            //
            // Check Lines

            var transactionIdList = myReconciliationPM.ReconciliationLines.Select(rec => rec.TransactionId).ToList();
            List<LedgerTransactionPM> ledgerTransactionPMs = null;
            GLAccountPM myGLAccount = null;
            if (myDataProvider != null)
            {
                ledgerTransactionPMs = myDataProvider.GetLedgerTransactionPMsByIdList(transactionIdList, myReconciliationPM.Tenant);
                myGLAccount = myDataProvider.GetGLAccount(myReconciliationPM.AccountId, myReconciliationPM.Tenant);

            }
            if (myReconciliationPM.CreatedByReconciliationAfterConversion)
            {
                var jlList = myDataProvider.GetJournalLineByLedgerTransactionIdList(transactionIdList, myReconciliationPM.Tenant);

                Validate_CreatedByReconciliationAfterConversion(errorsList, jlList);
            }
            if (myReconciliationPM.CreatedByReconciliationStageB)
            {
                ///not neeed - ohad+ alex
            }
            decimal sum = 0;
            foreach (var reconciliationLine in myReconciliationPM.ReconciliationLines)
            {
                if (reconciliationLine.ChangeSetOp != ChangeSetOperation.None)// in 
                {
                    CheckReconciliationLine(errorsList, ledgerTransactionPMs, myGLAccount, ref sum, reconciliationLine, myReconciliationPM.CreatedByReconciliationAfterConversion);
                }
            }
            if (sum != 0)
            {

                AddError(errorsList, M_sumofAmounttoreconcilemUST0);
            }


            //
            // Check Errors:
            //
            if (errorsList.Count == 0)
            {
                if (ledgerTransactionPMs != null)
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
                    errorString = errorString + error + ";";
                }
                errorString = errorString.Remove(errorString.Length - 1);
                return new ValidationResult(errorString);
            }
        }

        public static void Validate_CreatedByReconciliationAfterConversion(List<string> errorsList, List<Data.EntityPOCOs.JournalLine> jlList)
        {
            if (jlList.Count == 0)
            {
                AddError(errorsList,
                    M_AfterConversion_no_Journal_Line// "CreatedByReconciliationAfterConversion : no Journal Line"
                    );
            }
            if (jlList.Any(r => string.IsNullOrWhiteSpace(r.ExternalReconcileNumber)))
            {
                AddError(errorsList,
                    M_AfterConversion_noallJournalLinehaveExternalReconcileNumber //"CreatedByReconciliationAfterConversion : no all Journal Line have ExternalReconcileNumber"
                    );
            }
            if (jlList.Select(r => r.ExternalReconcileNumber).Distinct().Count() > 1)
            {
                AddError(errorsList,
                    M_AfterConversion_JournalLinehavenotthesameExternalReconcileNumber //"CreatedByReconciliationAfterConversion : Journal Line have not the same ExternalReconcileNumber"
                    );
            }
        }

        private static void CheckReconciliationLine(List<string> errorsList, List<LedgerTransactionPM> ledgerTransactionPMs, GLAccountPM myGLAccount, ref decimal sum, ReconciliationLinePM reconciliationLine,bool CreatedByReconciliationAfterConversion)
        {
            if (reconciliationLine.ChangeSetOp != ChangeSetOperation.Insert)
            {
                AddError(errorsList, /*"ReconciliationLines can only be insert mode "*/M_ReconciliationLinescanonlybeinsertmode);//i think so !!!????
            }
            if (String.IsNullOrWhiteSpace(reconciliationLine.TransactionId))
            {
                AddError(errorsList,
                    M_reconciliationLineTransactionIdIsnull/*"String.IsNullOrWhiteSpace(reconciliationLine.TransactionId) line "*/ + reconciliationLine.Line);
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
                        AddError(errorsList, /*" TransactionId Not in DB"*/M_TransactionIdNotinDB + reconciliationLine.TransactionId);
                    }
                    else
                    {
                        if (ledgerTransactionPM.IsReconciled)
                        {
                            AddError(errorsList, M_ledgerTransactionalreadyReconciled /*" ledgerTransaction already Reconciled  ?? ? TransactionId="*/ + reconciliationLine.TransactionId);
                        }
                        if (ledgerTransactionPM.OpenAmountCurrencyId != reconciliationLine.CurrencyId)
                        {
                            AddError(errorsList, M_OpenAmountCurrencyIdDiffreconciliationLineCurrencyId/*"(ledgerTransactionPM.OpenAmountCurrencyId != reconciliationLine.CurrencyId)" */+ reconciliationLine.TransactionId);
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
                                AddError(errorsList, /*"Insert ledger Transaction but Is not Reconciled "*/M_InsertledgerTransactionbutIsnotReconciled);
                            }
                        }
                        if (!CreatedByReconciliationAfterConversion)
                        {


                            if (ledgerTransactionPM.OpenAmount == 0)
                            {
                                if (reconciliationLine.ReconciliationAmount != ledgerTransactionPM.OpenAmount)
                                {
                                    AddError(errorsList, M_OpenAmountis0ReconcileNot);
                                }
                            }
                            else if (ledgerTransactionPM.OpenAmount > 0)
                            {
                                if (reconciliationLine.ReconciliationAmount <= 0 || reconciliationLine.ReconciliationAmount > ledgerTransactionPM.OpenAmount)
                                {
                                    AddError(errorsList, M_OpenAmountGreater0ReconcileNot0toOpenAmout/*"Reconciliation Amount have to be greter than 0 and less than Open Amount of transaction "*/ );
                                }
                            }
                            else //if (ledgerTransactionPM.OpenAmount < 0)
                            {
                                if (reconciliationLine.ReconciliationAmount > 0 || reconciliationLine.ReconciliationAmount < ledgerTransactionPM.OpenAmount)
                                {

                                    AddError(errorsList, M_OpenAmountLess0ReconcileNoOpenAmoutto0 /*"ReconciliationAmount have to be Less than 0 and more than Open Amount of transaction " */);
                                }
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



        public static ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }

            //ILoggedContactUtil loggedContactUtil = ContainerAccessor.Container.Resolve(typeof(ILoggedContactUtil), "LoggedContactUtil", new ParameterOverride("", tenant)) as ILoggedContactUtil;
            //ContactPM loggedcontact = loggedContactUtil.GetLoggedContact(tenant);

            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }

        public static ITextCodeTranslator OverrideITextCodeTranslator { get; set; }
        static string TranslateMyTextCode(string textCodeCode, int tenant, bool useLocal)
        {
            string trans = "";
            if (OverrideITextCodeTranslator != null)
            {
                trans = OverrideITextCodeTranslator.Translate(textCodeCode, tenant);
            }
            else
            {


                //bool useLocal = true;
                //var user = GetLoggedContact(tenant);
                //if (user != null) useLocal = !(GetLoggedContact(tenant).DontShowLocal);


                trans = TranslateTextsClass.Translate(textCodeCode, tenant, useLocal);
            }
            if (string.IsNullOrWhiteSpace(trans))
            {
                trans = "$Text(" + textCodeCode + ")";//Our Version Of Uniface Convention
            }
            return trans;
        }

    }
}
