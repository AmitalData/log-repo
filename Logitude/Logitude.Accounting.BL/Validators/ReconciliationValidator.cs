using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Logitude.Accounting.BL.Validators
{
    public partial class ReconciliationValidator
    {
        public const string M_YouShouldSelectTwoTransactions = "Journal.M.YouShouldSelectTwoTransactions";
        public const string M_SaidnoMultiCurrencyReconcile = "Eyal Said no MultiCurrency Reconcile ";
        public const string M_ReconciliationLinesHaveuseTransactionIdfewtimes = "ReconciliationLines Have use TransactionId few times ";
        public const string M_DeletedReconciliationLinesisnotallowed = "init DeletedReconciliationLines is not allowed ";
        public const string M_ReconciliationLinescanonlyinsert = "ReconciliationLines can only insert ";
        public const string TextCode_CantIncludeTwoOrMorePayment = "Accounting.O.CantIncludeTwoOrMorePayment";
        public const string M_ReconciliationLinescanonlybeinsertmode = "ReconciliationLines can only be insert mode ";

        public const string M_reconciliationLineTransactionIdIsnull = "String.IsNullOrWhiteSpace(reconciliationLine.TransactionId) line ";

        public const string M_TransactionIdNotinDB = " TransactionId Not in DB";
        public const string M_ledgerTransactionalreadyReconciled = /*" ledgerTransaction already Reconciled  ?? ? TransactionId="*/"Reconciliation.M.LedgerTransactionAlreadyReconciled";
        public const string M_OpenAmountCurrencyIdDiffreconciliationLineCurrencyId = "(ledgerTransactionPM.OpenAmountCurrencyId != reconciliationLine.CurrencyId)";
        public const string M_InsertledgerTransactionbutIsnotReconciled = "Insert ledger Transaction but Is not Reconciled ";

        public const string M_OpenAmountis0ReconcileNot = "Open Amount == 0 but (Reconciliation Amount not equal Open Amount of transaction) ";

        public const string M_OpenAmountGreater0ReconcileNot0toOpenAmout = "Reconciliation Amount have to be greter than 0 and less than Open Amount of transaction ";
        public const string M_OpenAmountLess0ReconcileNoOpenAmoutto0 = "ReconciliationAmount have to be Less than 0 and more than Open Amount of transaction ";
        public const string M_sumofAmounttoreconcilemUST0 = "the sum of Amount to reconcile of all selected transactions is not zero ";
        public const string M_AfterConversion_no_Journal_Line = "CreatedByReconciliationAfterConversion : no Journal Line";
        public const string M_AfterConversion_noallJournalLinehaveExternalReconcileNumber = "CreatedByReconciliationAfterConversion : no all Journal Line have ExternalReconcileNumber";
        public const string M_AfterConversion_JournalLinehavenotthesameExternalReconcileNumber = "CreatedByReconciliationAfterConversion : Journal Line have not the same ExternalReconcileNumber";
        public const string M_InReconcileProgress = "Ledger Transaction was found InReconcileProgress";
        private const string DifferentAccountsBlockingMessage = "Reconciliation.O.DifferentAccounts";
        private static ReconciliationPM reconciliation;
        public static ValidationResult IsReconciliationValid(ReconciliationPM myReconciliationPM, ValidationContext context)
        {
            reconciliation = myReconciliationPM;


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

            // BlockMultiplePayments(errorsList, context);

            //
            // Check Lines

            List<LedgerTransactionJournalLineLT> transactionsPMs = GetLedgerTransactionJournalLineLTsByIdList(context);

            BlockDifferentAccountsReconciliation(errorsList, transactionsPMs);

            var transactionIdList = myReconciliationPM.ReconciliationLines.Select(rec => rec.TransactionId).ToList();
            ///List<LedgerTransactionPM> ledgerTransactionPMs = null;
            GLAccountPM myGLAccount = null;
            IReconciliationValidatorContextDataProvider myDataProvider = context.GetService(typeof(IReconciliationValidatorContextDataProvider)) as IReconciliationValidatorContextDataProvider;
            if (myDataProvider != null)
            {
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
            if (!myReconciliationPM.CreateAutoReconcileWhileStreamingService)
            {
                var inReconcileProgressIDs = transactionsPMs.Where(r => r.InReconcileProgress).Select(r => r.Id).ToList();
                if (inReconcileProgressIDs.Count > 0)
                {
                    AddError(errorsList, M_InReconcileProgress);
                    AddError(errorsList, string.Join(",", inReconcileProgressIDs));
                }
            }
            decimal sum = 0;
            foreach (var reconciliationLine in myReconciliationPM.ReconciliationLines)
            {
                if (reconciliationLine.ChangeSetOp != ChangeSetOperation.None)// in 
                {
                    CheckReconciliationLine(errorsList, transactionsPMs /*ledgerTransactionPMs*/, myGLAccount, ref sum, reconciliationLine
                        , myReconciliationPM.CreatedByReconciliationAfterConversion
                        , myReconciliationPM.CreatedByReconciliationStageB);
                }
            }

            decimal x = Math.Abs(sum);
            if (x >= 0.0001m)
            {
                AddError(errorsList, M_sumofAmounttoreconcilemUST0);
            }


            //
            // Check Errors:
            //
            if (errorsList.Count == 0)
            {
                //if (ledgerTransactionPMs != null)
                //{
                //    //myReconciliationPM.CurrentContextTag = ledgerTransactionPMs;
                //}
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

        private static List<LedgerTransactionPM> GetReconciliationTransactions(ReconciliationPM myReconciliationPM, ValidationContext context)
        {
            IReconciliationValidatorContextDataProvider myDataProvider = context.GetService(typeof(IReconciliationValidatorContextDataProvider)) as IReconciliationValidatorContextDataProvider;
            var transactionsId = reconciliation.ReconciliationLines.Where(d => d.TransactionId != null).Select(a => a.TransactionId).ToList();
            
            List<LedgerTransactionPM> transactionsPMs = myDataProvider.GetLedgerTransactionPMsByIdList(transactionsId, myReconciliationPM.Tenant);
            return transactionsPMs;
        }

        private static void BlockDifferentAccountsReconciliation(List<string> errorsList, List<LedgerTransactionJournalLineLT> transactionsPMList)
        {
            var isReconcileWithDifferentAccounts = transactionsPMList.GroupBy(transaction => transaction.AccountId).Count() > 1;
            if (isReconcileWithDifferentAccounts == true)
            {
                AddError(errorsList, DifferentAccountsBlockingMessage);
            }
        }




        //private static void BlockMultiplePayments(List<string> errorsList, ValidationContext context)
        //{
        //    List<LedgerTransactionPM> transactions = GetReconciliationTransactions(context);
        //    int paymentsCount = GetPaymentsCountFromTransactions(transactions);
        //    int arpaymentsCount = GetARPaymentsCountFromTransactions(transactions);
        //    int appaymentsCount = GetAPPaymentsCountFromTransactions(transactions);

        //    bool transactionsAreNotSameSource = transactions.GroupBy(d => d.SourceId).Count() > 1;
        //    if (transactionsAreNotSameSource)
        //    {
        //        if (arpaymentsCount > 1)
        //            //AddErrorByTextCode(errorsList, "Reconciliation.O.CantReconcileMutipleAPPayment");
        //            //if (appaymentsCount > 1)
        //            //    AddErrorByTextCode(errorsList, "Reconciliation.O.CantReconcileMutipleAPPayment");
        //            if (appaymentsCount == 1 && arpaymentsCount == 1)
        //                AddErrorByTextCode(errorsList, "Accounting.O.CantIncludeTwoOrMorePayment");
        //    }
        //}


        //private static string CheckMultipleAPPayment(List<LedgerTransactionPM> transactions)
        //{
        //    bool transactionsAreNotSameSource = transactions.GroupBy(d => d.SourceId).Count() > 1;
        //    int appaymentsCount = GetAPPaymentsCountFromTransactions(transactions);
        //    if (appaymentsCount > 1 && transactionsAreNotSameSource)
        //        return GetTranslatedText("Reconciliation.O.CantReconcileMutipleAPPayment");
        //    return null;
        //}

        //private static string CheckMultipleARPayments(List<LedgerTransactionPM> transactions)
        //{
        //    bool transactionsAreNotSameSource = transactions.GroupBy(d => d.SourceId).Count() > 1;

        //    int arpaymentsCount = GetARPaymentsCountFromTransactions(transactions);
        //    if (arpaymentsCount > 1 && transactionsAreNotSameSource)
        //        return GetTranslatedText("Reconciliation.O.CantReconcileMutipleARPayment");
        //    return null;
        //}

        //private static int GetPaymentsCountFromTransactions(List<LedgerTransactionPM> transactions)
        //{
        //    return transactions.Count(d =>
        //    {
        //        bool notStornoTransaction = d.OriginalJournalId == null;
        //        return notStornoTransaction && (d.SourceTypeCode == AccountingEntityValues.ARPayment || d.SourceTypeCode == AccountingEntityValues.APPayment);
        //    });
        //}
        //private static int GetARPaymentsCountFromTransactions(List<LedgerTransactionPM> transactions)
        //{
        //    return transactions.Count(d =>
        //    {
        //        bool notStornoTransaction = d.OriginalJournalId == null;
        //        return notStornoTransaction && (d.SourceTypeCode == AccountingEntityValues.ARPayment);
        //    });
        //}
        //private static int GetAPPaymentsCountFromTransactions(List<LedgerTransactionPM> transactions)
        //{
        //    return transactions.Count(d =>
        //    {
        //        bool notStornoTransaction = d.OriginalJournalId == null;
        //        return notStornoTransaction && (d.SourceTypeCode == AccountingEntityValues.APPayment);
        //    });
        //}




        //private static List<LedgerTransactionPM> GetReconciliationTransactions(ValidationContext context)
        //{
        //    var transactionsId = reconciliation.ReconciliationLines.Where(d => d.TransactionId != null).Select(a => a.TransactionId).ToList();
        //    var myDataProvider = context.GetService(typeof(IReconciliationValidatorContextDataProvider)) as IReconciliationValidatorContextDataProvider;
        //    var transactionsPMList = myDataProvider.GetLedgerTransactionPMsByIdList(transactionsId, reconciliation.Tenant);
        //    return transactionsPMList;
        //}


        private static List<LedgerTransactionJournalLineLT> GetLedgerTransactionJournalLineLTsByIdList(ValidationContext context)
        {
            var transactionsId = reconciliation.ReconciliationLines.Where(d => d.TransactionId != null).Select(a => a.TransactionId).ToList();
            var myDataProvider = context.GetService(typeof(IReconciliationValidatorContextDataProvider)) as IReconciliationValidatorContextDataProvider;
            var transactionsPMList = myDataProvider.GetLedgerTransactionJournalLineLTsByIdList(transactionsId, reconciliation.Tenant);
            return transactionsPMList;
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

        private static void CheckReconciliationLine(List<string> errorsList, List<LedgerTransactionJournalLineLT> ledgerTransactionPMs, GLAccountPM myGLAccount, ref decimal sum, ReconciliationLinePM reconciliationLine, bool CreatedByReconciliationAfterConversion, bool createdByReconciliationStageB)
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
                        if (ledgerTransactionPM.IsReconciled && ledgerTransactionPM.OpenAmount != 0)
                        {
                            bool useLocal_inner = true;
                            string txt_M_ledgerTransactionalreadyReconciled = TranslateMyTextCode(/*" ledgerTransaction already Reconciled  ?? ? TransactionId="*/M_ledgerTransactionalreadyReconciled, 0, useLocal_inner);
                            AddError(errorsList, txt_M_ledgerTransactionalreadyReconciled + reconciliationLine.TransactionId);
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
                        if (!(CreatedByReconciliationAfterConversion  /*|| createdByReconciliationStageB*/))
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
        private static void AddErrorByTextCode(List<string> errorsList, string textcode)
        {
            string message = TranslateMyTextCode(textcode, 0, LoggedContactResolver.GetLoggedContactShowLocal(reconciliation.Tenant));
            errorsList.Add(message);
        }
        private static string GetTranslatedText(string textcode)
        {
            return TranslateMyTextCode(textcode, 0, LoggedContactResolver.GetLoggedContactShowLocal(reconciliation.Tenant));
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
