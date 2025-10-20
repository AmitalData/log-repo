
using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.BL.CoreBL.ExternalReconcile;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.Interfaces;
using Logitude.BL.Resolvers;
using Logitude.BL.Security;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Utils;
using Microsoft.Practices.Unity;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs; 
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.Data.Enums;
[assembly: InternalsVisibleTo("My1stUnitTestProject")]


namespace Logitude.Accounting.BL.Validators
{
    public partial class JournalValidator
    {
        public const string M_YouShouldHaveOneLineAtLeast = "Journal.M.YouShouldHaveOneLineAtLeast";
        public const string M_ClosedMonth = "ARInvoice.M.ClosedMonth"; //"AccountingPeriod.F.ClosedMonth";
        public const string M_ExternalNoAlreadyExists_1 = /*"Journals.O.ExternalNoAlreadyExists_1"*/"There is a Journal(";
        public const string M_ExternalNoAlreadyExists_2 = /*"Journals.O.ExternalNoAlreadyExists_2"*/ ") with the same ExternalNo And ExternalSystem";
        //public const string M_ExternalNoAlreadyExists_3 = "Journals.O.ExternalNoAlreadyExists_3";
        //public const string M_ExternalNoAlreadyExists_4 = "Journals.O.ExternalNoAlreadyExists_4";
        public const string M_ExchangeRateEmpty = "Journal.M.ExchangeRateEmpty";
        public const string M_LocalAmountNotZero = "Allowed  !!!Journal.M.LocalAmountNotZero"; // LocalAmountNotZero   Allowed  
        public const string M_ActionCode = "Journal.M.ActionCode";
        public const string M_ActionCodeDebit = "Journal.M.ActionCodeDebit";
        public const string M_ActionCodeCredit = "Journal.M.ActionCodeCredit";
        public const string M_ActionCodeCreditAndCreditMeanDebit = "Journal.M.ActionCodeCreditAndCredit";
        public const string M_DocumentDateBiggerDueDate = "Journal.M.DocumentDateBiggerDueDate";
        public const string M_DueDateMustgreaterthancurrent = "Journal.M.DueDateMustgreaterthancurrent";
        public const string M_currencydoesnotexist = "Journal.M.Journal.currencydoesnotexist";
        public const string M_JournalAmountNotMatched = "Journal.M.JournalAmountNotMatched";
        public const string M_LineSequence = "Check for missing Line number in sequence ";

        public const string M_GetGLAccountReturnNull = "myGLAccountDataProvider.GetGLAccount return null";
        public const string M_BlockedGLAccount = "Journal.M.AccountIsBlocked";//"Blocked GLAccounts(Inactive=True)";

        public const string M_GLAccountIsControl = "GLAccount IsControl=True";
        public const string M_ForeignDiffLocalAmountButTenantCurrency= "Journal.M.ForeignDiffLocalAmountButTenantCurrency";

        //public const string M_ButAccountCurrencyisDifferent =
        //    ///" But Account Currency is Different ";
        //    "Accounting.General.O.ButAccountCurrencyDifferent";

        public const string M_PaymentBankAccountCurrencyDifferent =
        "Accounting.General.O.PaymentBankAccountCurrencyDifferent";
        //ohad
        public const string M_JLAccountingDateMustWithinJournalMonth = "Journal.M.JLAccountingDateMustWithinJournalMonth";

        public const string M_AccountingFutureDateForbidden = "Journal.M.FutureDateForbidden";

        public const string M_ControlAccountIdIsMust =
            "Journal.M.ControlAccountIdIsMust";

        public const string M_ControlAccountIdIsNotMatch =
            "Journal.M.ControlAccountIdIsNotMatch";


        public const string M_DueDateIsMust = "Journal.M.DueDateIsMust";

        public const string M_FAMltiExchangerateNELA = "Journal.M.FAMltiExchangerateNELA";////Foreign amount ({0}) multiplied by the exchange rate ({1}) does not equal the local amount ({2})


        public const string K_AccountingPeriodsByTypeRegular = "AccountingPeriodsByTypeRegular";
        public const string K_TenantCurrencyId = "K_TenantCurrencyId";
        public const string K_DateTimeUtcNow = "K_DateTimeUtcNow";
        public const string K_FullAccountingSettingPM = "FullAccountingSettingPM";
        public const string K_SuppressCheckGLAccountIsMultiCurrencyWI40640 = "K_SuppressCheckGLAccountIsMultiCurrencyWI40640";//Task 40640: טיפול בסרביס לפקודת יומן - במקרה של כרטיס מפוצל לרשום על הפיצול
        public const string M_AccountingSameOppositeReference = "Journal.M.SameOppositeReference";

        public const string M_AllDateMustInit = "Journal.M.AllDateMustInit";
        public const string M_FutureDateIsNotAllowedInLine = "Journal.O.haveFutureAccountingorReferenceDate";

        public static ValidationResult IsJournalValid(
  JournalPM myJournalPM,
  System.ComponentModel.DataAnnotations.ValidationContext accountingValidationContextServiceProvider)
        {
            return (new JournalValidatorNotStatic()).IsJournalValid(
            myJournalPM,
            accountingValidationContextServiceProvider);
        }
    }
    class JournalValidatorNotStatic
    {



        const string statusCode_JournalCancelled = "5";
        private string _JLineNumberTExt;
        string TenantCurrency;
        RatesTableRepository ratesTableRepository;
        
        bool IsFutureDateErrorsExist = false;
        string FutureDateErrorsMessage = "";

        public ValidationResult IsJournalValidThin(
      JournalPM myJournalPM,
      System.ComponentModel.DataAnnotations.ValidationContext accountingValidationContextServiceProvider)
        {
 
            decimal creditTotal = 0;
            decimal debitTotal = 0;

   
            var errorsList = new MyList<string>();


            bool valid = true;

            var myDataProvider = accountingValidationContextServiceProvider.GetService(typeof(IJournalValidatorContextDataProvider)) as IJournalValidatorContextDataProvider;

            if (!String.IsNullOrWhiteSpace(myJournalPM.ExternalNo) && !String.IsNullOrWhiteSpace(myJournalPM.ExternalSystem))
            {
                string journalNumber = "";
                bool clientExists = false;
                if (myDataProvider != null)
                {
                    journalNumber = myDataProvider.CheckExternalNoAndSystemReturnJournalNumber(myJournalPM.ExternalNo, myJournalPM.ExternalSystem, myJournalPM.Tenant);
                    clientExists = 
                        (!String.IsNullOrWhiteSpace(journalNumber) && myJournalPM.JournalNumber != journalNumber);
                }
                //OHAD
                if (clientExists == true)
                {
                    string basic_text_ExternalExist =
                        JournalValidator.M_ExternalNoAlreadyExists_1 + journalNumber + JournalValidator.M_ExternalNoAlreadyExists_2;
                    errorsList.AddNew(basic_text_ExternalExist);
                }
            }
            DateTime? currDateTimeUtcNow = null;

            if (accountingValidationContextServiceProvider.Items.ContainsKey(JournalValidator.K_DateTimeUtcNow))
            {
                currDateTimeUtcNow = (DateTime)accountingValidationContextServiceProvider.Items[JournalValidator.K_DateTimeUtcNow];
            }
            currDateTimeUtcNow = currDateTimeUtcNow ?? TenantServerConfigration.GetCurrentDateTime(myJournalPM.Tenant); //DateTime.UtcNow;

            //ohad
            if (currDateTimeUtcNow.GetValueOrDefault().Date < myJournalPM.AccountingDate.Date)
            {
                errorsList.AddNew(TranslateMyTextCode(JournalValidator.M_AccountingFutureDateForbidden, myJournalPM.Tenant));
                valid = false;
            }
            //ohad

            if (myJournalPM.DocumentDate != null && (currDateTimeUtcNow.GetValueOrDefault().Date < myJournalPM.DocumentDate.Value.Date))
            {
                errorsList.AddNew(TranslateMyTextCode(JournalValidator.M_AccountingFutureDateForbidden, myJournalPM.Tenant));
                valid = false;
            }

            if (myJournalPM.JournalLines.Any(l => currDateTimeUtcNow.GetValueOrDefault().Date < l.AccountingDate.Date))
            {

                valid = false;
            }
            //OHAD
            if (myJournalPM.JournalLines.Any(l =>
                l.AccountingDate.Date.Year != myJournalPM.AccountingDate.Date.Year ||
                l.AccountingDate.Date.Month != myJournalPM.AccountingDate.Date.Month
                ))
            {
                errorsList.AddNew(TranslateMyTextCode(JournalValidator.M_JLAccountingDateMustWithinJournalMonth, myJournalPM.Tenant));
            }

            //ohad
            if (myJournalPM.JournalLines.Any(l => l.AccountingDate == DateTime.MinValue))
            {
                errorsList.AddNew(TranslateMyTextCode(JournalValidator.M_AllDateMustInit, myJournalPM.Tenant));
            }
            if (myJournalPM.JournalLines.Any(l => l.DueDate == DateTime.MinValue))
            {
                errorsList.AddNew(TranslateMyTextCode(JournalValidator.M_AllDateMustInit, myJournalPM.Tenant));
            }
            if (myJournalPM.JournalLines.Any(l => l.DocumentDate == DateTime.MinValue))
            {
                errorsList.AddNew(TranslateMyTextCode(JournalValidator.M_AllDateMustInit, myJournalPM.Tenant));
            }
            int seq = 0;
            foreach (JournalLinePM currJournalLinePM in myJournalPM.JournalLines
                .Where(jl => jl.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete)
                // i don't know how HTML5 Work But I Soppsed that I Get (postback) All  JournalLines All the Time 
                .OrderBy(r => r.Line))
            {
                this._JLineNumberTExt = ToUseLocalText(myJournalPM.Tenant) ? $"(שורת פקודה {currJournalLinePM.Line})" : $"(Journal Line {currJournalLinePM.Line})";
                try
                {

                    CalculateTotals(currJournalLinePM, ref creditTotal, ref debitTotal);

                    if (myDataProvider != null)
                    {
                     
                        var glCreditAccId = currJournalLinePM.CreditAccountId;


                        var debitAccountId = currJournalLinePM.DebitAccountId;



                        bool? SuppressCheckGLAccountIsMultiCurrencyWI40640 = false;
                        if (true)
                        {
                            SuppressCheckGLAccountIsMultiCurrencyWI40640 = true;//im+yaron - all the time !!
                        }
                        else
                        {
                            if (accountingValidationContextServiceProvider.Items.ContainsKey(JournalValidator.K_SuppressCheckGLAccountIsMultiCurrencyWI40640))
                            {
                                SuppressCheckGLAccountIsMultiCurrencyWI40640 = accountingValidationContextServiceProvider.Items[JournalValidator.K_SuppressCheckGLAccountIsMultiCurrencyWI40640] as bool?;
                            }
                            else
                            {
                                throw new ApplicationException("Dear Programmer U must initialize in context SuppressCheckGLAccountIsMultiCurrencyWI40640");
                            }
                        }

                        bool suppressInactiveCheck = myJournalPM.AccountingEntityCode == "11";//  העברת שנה   Year Transfer
                        var jlCurrencyId = currJournalLinePM.CurrencyId;
                        currJournalLinePM.ActionTypeCode = currJournalLinePM.ActionCode ?? string.Empty;
                        FullAccountingSettingPM tenantFullAccountingSettingPM = null;
                        TenantCurrency = GetTenantCurrency(myJournalPM, accountingValidationContextServiceProvider);
                        //IWebFreightContext webFreightContext = WebFreightContext.GetContext(myJournalPM.Tenant);
                        //ratesTableRepository = new RatesTableRepository(webFreightContext);

                        if (accountingValidationContextServiceProvider.Items.ContainsKey(JournalValidator.K_FullAccountingSettingPM))
                        {
                            tenantFullAccountingSettingPM = accountingValidationContextServiceProvider.Items[JournalValidator.K_FullAccountingSettingPM] as FullAccountingSettingPM;
                        }

                        switch (currJournalLinePM.ActionTypeCode.ToString())//will be valid on server side only
                        {
                            case "1"://MyJournalActionTypeEnum.Credit:
                                CheckGLAccountThin(true, errorsList, myDataProvider, glCreditAccId, jlCurrencyId, tenantFullAccountingSettingPM, myJournalPM, currJournalLinePM
                                    , SuppressCheckGLAccountIsMultiCurrencyWI40640,
                                    suppressInactiveCheck);
                                break;
                            case "2": //MyJournalActionTypeEnum.Debit:
                                CheckGLAccountThin(false, errorsList, myDataProvider, debitAccountId, jlCurrencyId, tenantFullAccountingSettingPM, myJournalPM, currJournalLinePM
                                    , SuppressCheckGLAccountIsMultiCurrencyWI40640,
                                    suppressInactiveCheck);
                                break;
                            case "3"://MyJournalActionTypeEnum.DebitAndCredit:
                            case "4"://MyJournalActionTypeEnum.DebitCreditAndVatdeduction:
                                CheckGLAccountThin(true, errorsList, myDataProvider, glCreditAccId, jlCurrencyId, tenantFullAccountingSettingPM, myJournalPM, currJournalLinePM
                                    , SuppressCheckGLAccountIsMultiCurrencyWI40640,
                                    suppressInactiveCheck);
                                CheckGLAccountThin(false, errorsList, myDataProvider, debitAccountId, jlCurrencyId, tenantFullAccountingSettingPM, myJournalPM, currJournalLinePM
                                    , SuppressCheckGLAccountIsMultiCurrencyWI40640,
                                    suppressInactiveCheck);
                                break;
                            default:
                                break;
                        }

                    }
 
                }
                finally
                {
                    this._JLineNumberTExt = null;
                }
            }



            var myIExternalReconcileDataProvider = accountingValidationContextServiceProvider.GetService(typeof(IExternalReconcileDataProvider)) as IExternalReconcileDataProvider;

            if (errorsList.Count == 0)
            {
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
                return new ValidationResult(TranslateMyTextCode("Accounting.General.O.JournalNotValid", 0) + ": " + errorString, errorsList);
            }



        }

        public ValidationResult IsJournalValid(
          JournalPM myJournalPM,
          System.ComponentModel.DataAnnotations.ValidationContext accountingValidationContextServiceProvider)
        {
            if (myJournalPM.StatusCodeEnum == JournalStatusTypePM.StatusCodeEnum.Cancelled)
                return ValidationResult.Success;
            decimal creditTotal = 0;
            decimal debitTotal = 0;

            bool debugit = false;
            if (debugit)
            {
                var serializedObject = ProxyUtil.JsonConvertSerialize(myJournalPM);
               NetCommonHelper.Logger.DevLog.Instance.WriteDebug(serializedObject);
            }
            var errorsList = new MyList<string>();
            

            bool valid = true;

            if (myJournalPM.JournalLines.Count == 0)
            {
                valid = false;

                //   bool useLocal = !(GetLoggedContact(myJournalPM.Tenant).DontShowLocal);

                bool useLocal = true;
                var user = GetLoggedContact(myJournalPM.Tenant);
                if (user != null) useLocal = !(GetLoggedContact(myJournalPM.Tenant).DontShowLocal);

                string msg =
                    TranslateMyTextCode("Journal.M.YouShouldHaveOneLineAtLeast", 0);
                //TranslateMyTextCode("Journal.M.YouShouldHaveOneLineAtLeast", 0, useLocal);
                if (myJournalPM.AccountingEntityCode == "3")
                {
                    msg = "The ARPayment can't be voided, one of the ARPayment cheques has been returned to customer";
                }
                errorsList.AddNew(msg);
            }
            var myDataProvider = accountingValidationContextServiceProvider.GetService(typeof(IJournalValidatorContextDataProvider)) as IJournalValidatorContextDataProvider;
            var myIExternalReconcileDataProvider = accountingValidationContextServiceProvider.GetService(typeof(IExternalReconcileDataProvider)) as IExternalReconcileDataProvider;
            FullAccountingSettingPM tenantFullAccountingSettingPM = null;
             TenantCurrency = GetTenantCurrency(myJournalPM, accountingValidationContextServiceProvider);
            //IWebFreightContext webFreightContext = WebFreightContext.GetContext(myJournalPM.Tenant);
             //ratesTableRepository = new RatesTableRepository(webFreightContext);

            if (accountingValidationContextServiceProvider.Items.ContainsKey(JournalValidator.K_FullAccountingSettingPM))
            {
                tenantFullAccountingSettingPM = accountingValidationContextServiceProvider.Items[JournalValidator.K_FullAccountingSettingPM] as FullAccountingSettingPM;
            }
            DateTime? currDateTimeUtcNow = null; 

            if (accountingValidationContextServiceProvider.Items.ContainsKey(JournalValidator.K_DateTimeUtcNow))
            {
                currDateTimeUtcNow = (DateTime)accountingValidationContextServiceProvider.Items[JournalValidator.K_DateTimeUtcNow];
            }
            currDateTimeUtcNow = currDateTimeUtcNow ?? TenantServerConfigration.GetCurrentDateTime(myJournalPM.Tenant); //DateTime.UtcNow;

            //Use SearchFields as an indicator to apply validation only for one-line reconciliation if the journal is one or split.
            if (!(myJournalPM.SearchFields== "OneLineReconciliation") && accountingValidationContextServiceProvider.Items.ContainsKey(JournalValidator.K_AccountingPeriodsByTypeRegular))
            {
                var accountingPeriodsByTypeRegular = accountingValidationContextServiceProvider.Items[JournalValidator.K_AccountingPeriodsByTypeRegular] as List<AccountingPeriodPM>;
                if (accountingPeriodsByTypeRegular != null)
                {
                    string transText = "";
                    transText = TranslateMyTextCode(JournalValidator.M_ClosedMonth, myJournalPM.Tenant);
                    if (String.IsNullOrWhiteSpace(transText))
                    {
                        transText = "Closed Month";
                    }

                    if (myJournalPM.APPaymentCancelDate==null)
                    {
                        if (!IsMonthOpenForAccountingDate(accountingPeriodsByTypeRegular.AsQueryable(), myJournalPM.AccountingDate, myJournalPM.AccountingEntityCode, myJournalPM.ExternalSystem))
                        {   
                            errorsList.AddNew(transText);
                            valid = false;
                            NetCommonHelper.Logger.DevLog.Instance.WriteError(
                                "[Closed Month] Validation failed: Attempted to post journal using AccountingDate, but the period is closed. " +
                                $"Tenant={myJournalPM.Tenant}, JournalNumber={myJournalPM.JournalNumber}, AccountingDate={myJournalPM.AccountingDate} , ExternalSystem={myJournalPM.ExternalSystem}"
                            );

                        }
                    }
                    else
                    {
                        if (!IsMonthOpenForAccountingDate(accountingPeriodsByTypeRegular.AsQueryable(),(DateTime) myJournalPM.APPaymentCancelDate, myJournalPM.AccountingEntityCode, myJournalPM.ExternalSystem))
                        {
                            errorsList.AddNew(transText);
                            valid = false;
                            NetCommonHelper.Logger.DevLog.Instance.WriteError(
                                "[Closed Month] Validation failed: Attempted to post journal using APPaymentCancelDate, but the period is closed. " +
                                $"Tenant={myJournalPM.Tenant}, JournalNumber={myJournalPM.JournalNumber}, APPaymentCancelDate={myJournalPM.APPaymentCancelDate}, ExternalSystem={myJournalPM.ExternalSystem}"
                            );

                        }

                    }

                }
            }

            if (!String.IsNullOrWhiteSpace(myJournalPM.ExternalNo) && !String.IsNullOrWhiteSpace(myJournalPM.ExternalSystem))
            {
                string journalNumber = "";
                bool clientExists = false;
                if (myDataProvider == null)
                {
                    //clientExists = CheckExternalNoAndSystem(myJournalPM.ExternalNo, myJournalPM.ExternalSystem, ref journalNumber, myJournalPM.Tenant);
                }
                else
                {
                    journalNumber = myDataProvider.CheckExternalNoAndSystemReturnJournalNumber(myJournalPM.ExternalNo, myJournalPM.ExternalSystem, myJournalPM.Tenant);
                    clientExists = //!string.IsNullOrWhiteSpace(journalNumber);
                        (!String.IsNullOrWhiteSpace(journalNumber) && myJournalPM.JournalNumber != journalNumber);
                }
                //OHAD
                if (clientExists == true)
                {
                    string basic_text_ExternalExist =
                        //"There is a Journal ("+ journalNumber + ") with the same ExternalNo And ExternalSystem";
                        JournalValidator.M_ExternalNoAlreadyExists_1 + journalNumber + JournalValidator.M_ExternalNoAlreadyExists_2;
                    errorsList.AddNew(basic_text_ExternalExist);
                    //TranslateMyTextCode(JournalValidator.M_ExternalNoAlreadyExists_1, myJournalPM.Tenant)
                    //    + myJournalPM.ExternalNo
                    //    + TranslateMyTextCode(JournalValidator.M_ExternalNoAlreadyExists_2, myJournalPM.Tenant)
                    //    + myJournalPM.ExternalSystem
                    //    + TranslateMyTextCode(JournalValidator.M_ExternalNoAlreadyExists_3, myJournalPM.Tenant);

                    //if (String.IsNullOrWhiteSpace(myJournalPM.JournalNumber))
                    //{
                    //    errorsList.Add(basic_text_ExternalExist);
                    //}
                    //else
                    //{
                    //    //errorsList.Add(basic_text_ExternalExist + journalNumber
                    //    //    + TranslateMyTextCode(JournalValidator.M_ExternalNoAlreadyExists_4, myJournalPM.Tenant));
                    //}
                }
            }

#if false
            var raiseM_ExchangeRateEmpty = false;
            var alexCheckExchangeRate = false;
            if (alexCheckExchangeRate && (myJournalPM.JournalLines.Where(d => d.ExchangeRate == null && (!d.ForeignAmount.HasValue || d.ForeignAmount.Value == 0m) && (!d.LocalAmount.HasValue || d.LocalAmount.Value == 0m)).Any()))
            {
                raiseM_ExchangeRateEmpty = true;
            }
            else
            {
                var linesWithoutExchangeRate = myJournalPM.JournalLines.Where(d => d.ExchangeRate == null).ToList();
                raiseM_ExchangeRateEmpty = linesWithoutExchangeRate.Where(l => l.LocalAmount.HasValue || l.LocalAmount.HasValue).Any();
            }

            if (raiseM_ExchangeRateEmpty)
            {
                errorsList.Add(TranslateMyTextCode(JournalValidator.M_ExchangeRateEmpty, myJournalPM.Tenant));
            }

            
#endif
            //ohad
            if (currDateTimeUtcNow.GetValueOrDefault().Date < myJournalPM.AccountingDate.Date)
            {
                errorsList.AddNew(TranslateMyTextCode(JournalValidator.M_AccountingFutureDateForbidden, myJournalPM.Tenant));
                valid = false;
            }
            //ohad

            if (myJournalPM.DocumentDate != null && (currDateTimeUtcNow.GetValueOrDefault().Date < myJournalPM.DocumentDate.Value.Date))
            {
                errorsList.AddNew(TranslateMyTextCode(JournalValidator.M_AccountingFutureDateForbidden, myJournalPM.Tenant));
                valid = false;
            }

            if (myJournalPM.JournalLines.Any(l => currDateTimeUtcNow.GetValueOrDefault().Date < l.AccountingDate.Date))
            {

                valid = false;
            }
            if (myJournalPM.JournalLines.Any(l =>
                l.AccountingDate.Date.Year != myJournalPM.AccountingDate.Date.Year ||
                l.AccountingDate.Date.Month != myJournalPM.AccountingDate.Date.Month
                ))
            {
                errorsList.AddNew(TranslateMyTextCode(JournalValidator.M_JLAccountingDateMustWithinJournalMonth, myJournalPM.Tenant));
            }

            //47045 onRegilarJournalAvoidTheSameReference4DebitOrCredit_DochMaaam(errorsList,myJournalPM);
            //ohad
            if (myJournalPM.JournalLines.Any(l => l.AccountingDate == DateTime.MinValue))
            {
                errorsList.AddNew(TranslateMyTextCode(JournalValidator.M_AllDateMustInit, myJournalPM.Tenant));
            }
            if (myJournalPM.JournalLines.Any(l => l.DueDate == DateTime.MinValue))
            {
                errorsList.AddNew(TranslateMyTextCode(JournalValidator.M_AllDateMustInit, myJournalPM.Tenant));
            }
            if (myJournalPM.JournalLines.Any(l => l.DocumentDate == DateTime.MinValue))
            {
                errorsList.AddNew(TranslateMyTextCode(JournalValidator.M_AllDateMustInit, myJournalPM.Tenant));
            }
            int seq = 0;
            foreach (JournalLinePM currJournalLinePM in myJournalPM.JournalLines
                .Where(jl => jl.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete)
                // i don't know how HTML5 Work But I Soppsed that I Get (postback) All  JournalLines All the Time 
                .OrderBy(r => r.Line))
            {
                this._JLineNumberTExt = ToUseLocalText(myJournalPM.Tenant) ? $"(שורת פקודה {currJournalLinePM.Line})" : $"(Journal Line {currJournalLinePM.Line})";
                try
                {


                    seq++;
                    //Canceled by Task 110526
                    //if (currJournalLinePM.Line != seq && myJournalPM.StatusCode != statusCode_JournalCancelled)
                    //{
                    //    errorsList.AddNew(JournalValidator.M_LineSequence + seq.ToString() + " !=" + currJournalLinePM.Line.ToString());
                    //}
                    //if (item.ForeignAmount == 0)
                    if (currJournalLinePM.ForeignAmount
                        //.GetValueOrDefault()
                        == 0)
                    {
                        //errorsList.Add(TranslateMyTextCode(JournalValidator.M_ForeignAmountNotZero, myJournalPM.Tenant));
                    }
                    //if (item.LocalAmount == 0)
                    if (currJournalLinePM.LocalAmount
                        //.GetValueOrDefault() 
                        == 0)
                    {
                        //errorsList.Add(TranslateMyTextCode(JournalValidator.M_LocalAmountNotZero, myJournalPM.Tenant));
                    }
                    if (string.IsNullOrWhiteSpace(currJournalLinePM.ActionCode) && string.IsNullOrWhiteSpace(currJournalLinePM.ActionTypeCode))
                    {
                        errorsList.AddNew(TranslateMyTextCode(JournalValidator.M_ActionCode, myJournalPM.Tenant));
                    }
                    if (currJournalLinePM.ActionTypeCode == "2" && (currJournalLinePM.DebitAccountId == null))
                    {
                        errorsList.AddNew(TranslateMyTextCode(JournalValidator.M_ActionCodeDebit, myJournalPM.Tenant));
                    }
                    if (currJournalLinePM.ActionTypeCode == "1" && (currJournalLinePM.CreditAccountId == null))
                    {
                        errorsList.AddNew(TranslateMyTextCode(JournalValidator.M_ActionCodeCredit, myJournalPM.Tenant));
                    }
                    if ((currJournalLinePM.ActionTypeCode == "3" || currJournalLinePM.ActionTypeCode == "4") && ((currJournalLinePM.DebitAccountId == null) || (currJournalLinePM.CreditAccountId == null)))
                    {
                        errorsList.AddNew(TranslateMyTextCode(JournalValidator.M_ActionCodeCreditAndCreditMeanDebit, myJournalPM.Tenant));
                    }

                    if (TenantCurrency == currJournalLinePM.CurrencyId)
                    {
                        if (currJournalLinePM.ForeignAmount != currJournalLinePM.LocalAmount)
                        {
                            if (myJournalPM.ExternalSystem == "AMITAL" && !string.IsNullOrWhiteSpace(myJournalPM.ExternalNo))
                            {
                                /// LET IT GO !!
                            }
                            else
                            {
                                //המטבע הוא שח והסכום במטז שונה מסכום שח
                                errorsList.AddNew(TranslateMyTextCodeDisplay(JournalValidator.M_ForeignDiffLocalAmountButTenantCurrency, myJournalPM.Tenant, currJournalLinePM));
                            }
                        }
                    }
                    //if (!currJournalLinePM.DueDate.HasValue)
                    //{
                    //    errorsList.Add(TranslateMyTextCode(JournalValidator.M_DueDateIsMust, myJournalPM.Tenant));
                    //}
                    bool baselSuppressDocGreaterThenDue = true;
                    if (!baselSuppressDocGreaterThenDue)
                    {
                        if ((currJournalLinePM.DocumentDate) > (currJournalLinePM.DueDate))
                        {
                            errorsList.AddNew(TranslateMyTextCode(JournalValidator.M_DocumentDateBiggerDueDate, myJournalPM.Tenant));
                        }
                    }
                    if (currJournalLinePM.ForeignAmount == 0)
                    {
                        if (currJournalLinePM.ExchangeRate.GetValueOrDefault() != 0)
                        {
                            ///Task 51197: Cancel the validation -Which checks that a local amount multiplied by a currency exchange rate is equal to the amount in the foreign currency
                            ///
                            bool Task51197 = true;
                            if (!Task51197)
                            {
                                var mM_FAMltiExchangerateNELA = TranslateMyTextCode(JournalValidator.M_FAMltiExchangerateNELA, myJournalPM.Tenant);
                                //mM_FAMltiExchangerateNELA=mM_FAMltiExchangerateNELA??"Foreign amount ({0}) multiplied by the exchange rate ({1}) does not equal the local amount ({2})";
                                mM_FAMltiExchangerateNELA = String.Format(mM_FAMltiExchangerateNELA, currJournalLinePM.ForeignAmount, currJournalLinePM.ExchangeRate, currJournalLinePM.LocalAmount);
                                errorsList.AddNew(mM_FAMltiExchangerateNELA);
                            }
                        }
                    }
                    //{{"JournalId":"1-736052","Tenant":1,"Line":1,"ActionCode":"1-1","DebitControlAccountId":null,"DebitAccountId":null,"CreditControlAccountId":"1-5","CreditAccountId":"1-19152","DocumentDate":"2016-11-22T09:35:38.5272647+02:00","AccountingDate":"2017-01-23T00:00:00","DueDate":"2017-01-16T00:00:00","LocalAmount":0.0,"CurrencyId":"1-7","ForeignAmount":1.0,"ExchangeRate":0.0,"Reference1":null,"Reference2":null,"Reference3":null,"ActionName":"Credit","DebitControlAccountName":null,"CreditAccountName":null,"DebitAccountName":null,"CreditControlAccountName":null,"CreditControlAccountNumber":null,"DebitControlAccountNumber":null,"CreditAccountNumber":null,"DebitAccountNumber":null,"CurrencyName":null,"Notes":null,"CurrencyCode":null,"ActionTypeCode":"1","ExternalOpenAmount":null,"IsCreditAccountMulti":null,"IsDebitAccountMulti":null,"ActionTypeCodeEnum":1,"ChangeSetOp":1,"EncodeBase64NVARCHARFieldsBy":null}}
                    else if (currJournalLinePM.LocalAmount == 0)
                    {
                        if (currJournalLinePM.ExchangeRate.GetValueOrDefault() != 0)
                        {

                            ///Task 51197: Cancel the validation -Which checks that a local amount multiplied by a currency exchange rate is equal to the amount in the foreign currency
                            ///
                            bool Task51197 = true;
                            if (!Task51197)
                            {
                                var mM_FAMltiExchangerateNELA = TranslateMyTextCode(JournalValidator.M_FAMltiExchangerateNELA, myJournalPM.Tenant);
                                //mM_FAMltiExchangerateNELA=mM_FAMltiExchangerateNELA??"Foreign amount ({0}) multiplied by the exchange rate ({1}) does not equal the local amount ({2})";
                                mM_FAMltiExchangerateNELA = String.Format(mM_FAMltiExchangerateNELA, currJournalLinePM.ForeignAmount, currJournalLinePM.ExchangeRate, currJournalLinePM.LocalAmount);
                                errorsList.AddNew(mM_FAMltiExchangerateNELA);
                            }
                        }
                    }
                    else
                    {
                        if (currJournalLinePM.ExchangeRate.HasValue && currJournalLinePM.ExchangeRate.GetValueOrDefault() != 0)
                        {

                            decimal div =
                                //Math.Round((decimal)(currJournalLinePM.LocalAmount / currJournalLinePM.ForeignAmount), 2);
                                Math.Round((decimal)(currJournalLinePM.LocalAmount / currJournalLinePM.ExchangeRate), 2);
                            //מן הסתם פעולת החילוק "יושבת" יותר טוב מכפל !
                            //"בדיקה טובה" :
                            //150/41.66 == 3.6 
                            //150/3.6 ==  41.66
                            //"בדיקה לא טובה" :
                            //41.66*3.6 != 150

                            if (div != currJournalLinePM.ForeignAmount)
                            //if (currJournalLinePM.ForeignAmount * currJournalLinePM.ExchangeRate  != currJournalLinePM.LocalAmount)
                            {
                                decimal newrate = currJournalLinePM.LocalAmount / currJournalLinePM.ForeignAmount;
                                newrate = Math.Round(newrate, 5);
                                if (newrate != currJournalLinePM.ExchangeRate)
                                {
                                    //foreign amount (33.33) multiplied by the exchange rate (1.4) does not equal the local amount (46.67)

                                    ///Task 51197: Cancel the validation -Which checks that a local amount multiplied by a currency exchange rate is equal to the amount in the foreign currency
                                    ///
                                    bool Task51197 = true;
                                    if (!Task51197)
                                    {
                                        var mM_FAMltiExchangerateNELA = TranslateMyTextCode(JournalValidator.M_FAMltiExchangerateNELA, myJournalPM.Tenant);
                                        //mM_FAMltiExchangerateNELA=mM_FAMltiExchangerateNELA??"Foreign amount ({0}) multiplied by the exchange rate ({1}) does not equal the local amount ({2})";
                                        mM_FAMltiExchangerateNELA = String.Format(mM_FAMltiExchangerateNELA, currJournalLinePM.ForeignAmount, currJournalLinePM.ExchangeRate, currJournalLinePM.LocalAmount);
                                        errorsList.AddNew(mM_FAMltiExchangerateNELA);
                                    }
                                }
                            }
                        }
                    }


                    bool eyalSuppress = true;
                    if (!eyalSuppress)
                    {
                        if (//currJournalLinePM.DueDate.HasValue && 
                            currJournalLinePM.DueDate.Date < myJournalPM.CreateDate.Date) //itzik + basel  DateTime.Now)
                        {
                            var Mustbegreaterthancurrentdate = TranslateMyTextCode(JournalValidator.M_DueDateMustgreaterthancurrent, myJournalPM.Tenant);
                            if (String.IsNullOrWhiteSpace(Mustbegreaterthancurrentdate))
                            {
                                Mustbegreaterthancurrentdate = "Due Date ,Must be greater than current date ";
                            }
                            errorsList.AddNew(Mustbegreaterthancurrentdate);
                        }
                    }
                    CalculateTotals(currJournalLinePM, ref creditTotal, ref debitTotal);

                    if (myDataProvider != null)
                    {
                        if (!String.IsNullOrWhiteSpace(currJournalLinePM.CurrencyId))
                        {
                            CurrencyPM currency = myDataProvider.GetCurrency(currJournalLinePM.CurrencyId, currJournalLinePM.Tenant);
                            if (currency == null)
                            {
                                errorsList.AddNew(TranslateMyTextCode(JournalValidator.M_currencydoesnotexist, myJournalPM.Tenant));
                            }
                        }


                        var glCreditAccId = currJournalLinePM.CreditAccountId;


                        var debitAccountId = currJournalLinePM.DebitAccountId;



                        bool? SuppressCheckGLAccountIsMultiCurrencyWI40640 = false;
                        if (true)
                        {
                            SuppressCheckGLAccountIsMultiCurrencyWI40640 = true;//im+yaron - all the time !!
                        }
                        else
                        {
                            if (accountingValidationContextServiceProvider.Items.ContainsKey(JournalValidator.K_SuppressCheckGLAccountIsMultiCurrencyWI40640))
                            {
                                SuppressCheckGLAccountIsMultiCurrencyWI40640 = accountingValidationContextServiceProvider.Items[JournalValidator.K_SuppressCheckGLAccountIsMultiCurrencyWI40640] as bool?;
                            }
                            else
                            {
                                throw new ApplicationException("Dear Programmer U must initialize in context SuppressCheckGLAccountIsMultiCurrencyWI40640");
                            }
                        }

                        bool suppressInactiveCheck = myJournalPM.AccountingEntityCode == "11";//  העברת שנה   Year Transfer
                        var jlCurrencyId = currJournalLinePM.CurrencyId;
                        currJournalLinePM.ActionTypeCode = currJournalLinePM.ActionTypeCode ?? string.Empty;
                        switch (currJournalLinePM.ActionTypeCode.ToString())//will be valid on server side only
                        {
                            case "1"://MyJournalActionTypeEnum.Credit:
                                CheckGLAccount(true, errorsList, myDataProvider, glCreditAccId, jlCurrencyId, tenantFullAccountingSettingPM, myJournalPM, currJournalLinePM
                                    , SuppressCheckGLAccountIsMultiCurrencyWI40640, 
                                    suppressInactiveCheck);
                                break;
                            case "2": //MyJournalActionTypeEnum.Debit:
                                CheckGLAccount(false, errorsList, myDataProvider, debitAccountId, jlCurrencyId, tenantFullAccountingSettingPM, myJournalPM, currJournalLinePM
                                    , SuppressCheckGLAccountIsMultiCurrencyWI40640,
                                    suppressInactiveCheck);
                                break;
                            case "3"://MyJournalActionTypeEnum.DebitAndCredit:
                            case "4"://MyJournalActionTypeEnum.DebitCreditAndVatdeduction:
                                CheckGLAccount(true, errorsList, myDataProvider, glCreditAccId, jlCurrencyId, tenantFullAccountingSettingPM, myJournalPM, currJournalLinePM
                                    , SuppressCheckGLAccountIsMultiCurrencyWI40640,
                                    suppressInactiveCheck);
                                CheckGLAccount(false, errorsList, myDataProvider, debitAccountId, jlCurrencyId, tenantFullAccountingSettingPM, myJournalPM, currJournalLinePM
                                    , SuppressCheckGLAccountIsMultiCurrencyWI40640,
                                    suppressInactiveCheck);
                                break;
                            default:
                                break;
                        }

                    }
                    ValidateExchangeRate(accountingValidationContextServiceProvider,currJournalLinePM, errorsList);

                }
                finally
                {
                    this._JLineNumberTExt = null;
                }
            }

            if (debitTotal != creditTotal)
            {
                if ((myJournalPM.StatusCode == "1")  || (myJournalPM.StatusCode == "6"))//Draft = 0,//WaitingforApprove = 1,//Approved = 2,//Voided = 3
                {
                   errorsList.AddNew(TranslateMyTextCode(JournalValidator.M_JournalAmountNotMatched, myJournalPM.Tenant)+ " " + Math.Abs(debitTotal - creditTotal));
                }
            }
            ValidateJournalLinesForFutureDate(myJournalPM);
            bool journalHasFutureDateErrors = CheckIfFutureDateErrorsExist(myJournalPM);
            if(journalHasFutureDateErrors)
            {
                errorsList.Add(FutureDateErrorsMessage);
            }

            ValidateJournalReconciles(myJournalPM, accountingValidationContextServiceProvider, errorsList, myIExternalReconcileDataProvider);

            ValidateJournalExternalReconciles(myJournalPM, errorsList, myIExternalReconcileDataProvider);
          
            if (errorsList.Count == 0)
            {
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
                return new ValidationResult(TranslateMyTextCode("Accounting.General.O.JournalNotValid", 0) + ": " + errorString, errorsList);
            }



        }

        private MyList<string> ValidateExchangeRate(ValidationContext accountingValidationContextServiceProvider, JournalLinePM journalLine, MyList<string> errors)
        {
            if (journalLine.CurrencyId == TenantCurrency) return errors;
            var myIJournalValidatorRateDataProvider = accountingValidationContextServiceProvider.GetService(typeof(IJournalValidatorRateDataProvider)) as IJournalValidatorRateDataProvider;
            if (myIJournalValidatorRateDataProvider==null)
            {
                return errors; 
            }
            //RatesTable entityPoco = ratesTableRepository.GetExchageRateByValueAndDate(TenantCurrency, journalLine.CurrencyId, journalLine.AccountingDate, journalLine.Tenant);
            bool existRate =myIJournalValidatorRateDataProvider.ExistRate(TenantCurrency, journalLine.CurrencyId, journalLine.AccountingDate, journalLine.Tenant);
            if (!existRate /*entityPoco == null*/)
            {
                errors.Add(TranslateMyTextCode("Journal.O.ExchangeRateValidation", journalLine.Tenant) + " " + journalLine.Line);
            }

            return errors;
        }
        private string GetTenantCurrency(JournalPM journal, ValidationContext accountingValidationContextServiceProvider)
        {
            string tenantCurrencyId = "";
            if (accountingValidationContextServiceProvider.Items.ContainsKey(JournalValidator.K_TenantCurrencyId))
            {
                tenantCurrencyId = accountingValidationContextServiceProvider.Items[JournalValidator.K_TenantCurrencyId] as string;
            }
            return tenantCurrencyId;
            //TenantQuery tenantQuery = new TenantQuery(journal.Tenant);
            //TenantPM tenantPM = tenantQuery.GetSinglePM(journal.Tenant);
            //return tenantPM.CurrencyId;

        }

        private void ValidateJournalLinesForFutureDate(JournalPM myJournalPM)
        {
            if (myJournalPM.ExternalSystem=="AMITAL")
            {
                return;//Task 139496: נטרול ולידציה בפק יומן מהסבות - תאריך אסמכתא
            }
            foreach (JournalLinePM journalLinePM in myJournalPM.JournalLines)
            {
                bool journalLineHasFutureDate = CheckJournalLineForFutureDate(journalLinePM);
                if (journalLineHasFutureDate)
                {
                    FutureDateErrorsMessage += TranslateMyTextCode("Journal.O.haveFutureAccountingorReferenceDate", journalLinePM.Tenant);
                    IsFutureDateErrorsExist = true;
                    return;
                }
            }
        }
   
        private bool CheckJournalLineForFutureDate(JournalLinePM journalLinePM)
        {
            DateTime? currDateTimeUtcNow = null;
            currDateTimeUtcNow = TenantServerConfigration.GetCurrentDateTime(journalLinePM.Tenant);
            return currDateTimeUtcNow.GetValueOrDefault().Date < journalLinePM.AccountingDate.Date || currDateTimeUtcNow.GetValueOrDefault().Date < journalLinePM.DocumentDate.Date; ;
        }
   
        private bool CheckIfFutureDateErrorsExist(JournalPM myJournalPM)
        {
            const string statusCode_JournalProgress = "6";
            const string AccountingEntityCode_Journal = "1";
            var isJournalManuallyCreated = myJournalPM.AccountingEntityCode == AccountingEntityCode_Journal;
            var isIsFutureDateErrorsExistAndJournalApproved = IsFutureDateErrorsExist && myJournalPM.StatusCode == statusCode_JournalProgress;
            if (isIsFutureDateErrorsExistAndJournalApproved && isJournalManuallyCreated)
                return true;
            else
                return false;
        }

        private void ValidateJournalReconciles(JournalPM myJournalPM, ValidationContext accountingValidationContextServiceProvider, List<string> errorsList, IExternalReconcileDataProvider myIExternalReconcileDataProvider)

        {
            if (myJournalPM.AccountingEntityCode == AccountingEntityValues.TaxReport && FeatureToggleHelper.HasFeatureToggle("TRO", myJournalPM.Tenant))
                return;


            if (myJournalPM.JournalReconciles
                .Where(r => r.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
                .Count() == 0)
            {
                return;
            }

            bool tested = true;
            if (!tested)
            {
                return;
            }
            var journalApproveParser = new JournalApproveParser(myJournalPM, false,
accountingValidationContextServiceProvider
);
            journalApproveParser.CreateLedger_MapByJournalActionType();
            var newExpectedLedgerTransactions = journalApproveParser.LedgerTransactions;
            if (newExpectedLedgerTransactions == null || newExpectedLedgerTransactions.Count < 1)
            {
                errorsList.Add(TranslateMyTextCode("JournalReconciles-Check:newExpectedLedgerTransactions.Count < 1", myJournalPM.Tenant));

            }
            else
            {
                var theReconcileAgainstLTranIdList = myJournalPM.JournalReconciles.Select(r => r.LedgerTransactionId).ToList();
                List<LedgerTransactionPM> myOldTransToReconcile = myIExternalReconcileDataProvider.GetLedgerTransactionList(theReconcileAgainstLTranIdList, myJournalPM.Tenant);


                try
                {

                    var createAutoReconcileWhileStreamingService4JournalValidation = new CreateAutoReconcileWhileStreamingService4JournalValidation();
                    createAutoReconcileWhileStreamingService4JournalValidation.MustInit(null, myJournalPM, newExpectedLedgerTransactions);
                    createAutoReconcileWhileStreamingService4JournalValidation.InitMe(myOldTransToReconcile);
                    createAutoReconcileWhileStreamingService4JournalValidation.CreateAutoReconcileWhileStreaming(false);

                }
                catch (Exception eeee)
                {
                    if (eeee.Message == "Reconciliation.O.MultiCurrencyGlaccountReconciliation")
                    {
                        //var errorMessage = OverrideITextCodeTranslator.Translate(eeee.Message, myJournalPM.Tenant);
                        bool useLocal = ToUseLocalText(myJournalPM.Tenant);

                        //trans = TextCodesTranslator.TranslateText(textCodeCode, tenant);
                        var errorMessage = TranslateTextsClass.Translate(eeee.Message, myJournalPM.Tenant, useLocal) /*+ " " + _JLineNumberTExt*/;
                        errorsList.Add(errorMessage);
                    }
                    else {
                        errorsList.Add(eeee.Message);
                    }
                }
            }



        }

        private  void ValidateJournalExternalReconciles(JournalPM myJournalPM, MyList<string> errorsList, IExternalReconcileDataProvider myIExternalReconcileDataProvider)
        {
            if (myJournalPM.JournalExternalReconciles
                .Where(r => r.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
                .Count() == 0)
            {
                return;
            }
            if (!myJournalPM.JournalExternalReconciles.Any(r => r.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.None))
            {
                return;
            }


            if (!string.IsNullOrWhiteSpace(myJournalPM.OriginalJournalId))
            {
                //CancelDeposit no validation needed
            }
            else if (myJournalPM.JournalExternalReconciles.Any(r => string.IsNullOrWhiteSpace(r.LedgerTransactionId)))
            {
                var myExternalReconcileAdjustBankFeesService = new ExternalReconcileAdjustBankFeesService();
                myExternalReconcileAdjustBankFeesService.MustInit(myIExternalReconcileDataProvider);

                List<string> reconcileExternalPageLineIdList = myJournalPM.JournalExternalReconciles.Where(r => !string.IsNullOrWhiteSpace(r.ReconcileExternalPageLineId)).Select(r => r.ReconcileExternalPageLineId).ToList();
                string adjustGLAccountId = CreateAutoExternalReconcileWhileStreamingService.GetAdjustGLAccountId(myJournalPM);

                List<ReconcileExternalPageLineList> listOfpageLineList;
                List<ReconcileExternalPageList> listOfpageList;
                bool CheckWhileStreaming = false;

                List<string> ledgerTransactionIds = myJournalPM.JournalExternalReconciles.Where(r => !String.IsNullOrWhiteSpace(r.LedgerTransactionId)).Select(r => r.LedgerTransactionId).ToList();

                var skipAccountValidation = myJournalPM.JournalExternalReconciles.Any(r => r.SkipAccountsValidation == true);

                myExternalReconcileAdjustBankFeesService.PrapareAndValid(myJournalPM.Tenant, reconcileExternalPageLineIdList, adjustGLAccountId, out listOfpageLineList, out listOfpageList, CheckWhileStreaming,


                    ledgerTransactionIds,
                    out string accountingCurrencyId, out List<LedgerTransactionPM> ledgerTransactionList,
                    skipAccountValidation
                    );


            }
            else
            {
                //if (myJournalPM.JournalExternalReconciles.Count > 1)
                //{
                //    //throw new ApplicationException("Sorry meanwhile only one Adjust Allowed !!!");
                //    errorsList.Add(TranslateMyTextCode("Sorry meanwhile only one Adjust Allowed !!!", myJournalPM.Tenant));
                //}
                //            if (myJournalPM.JournalExternalReconciles.Count == 1)
                {
                    var myExternalReconcileMoveBankCheckFromTransfer2GLAccountService = new ExternalReconcileMoveBankCheckFromTransfer2GLAccountService();
                    myExternalReconcileMoveBankCheckFromTransfer2GLAccountService.MustInit(myIExternalReconcileDataProvider);
                    if (myJournalPM.JournalLines.Count() == 4)//
                    {
                        myExternalReconcileMoveBankCheckFromTransfer2GLAccountService.OnAdjustMustInit(myJournalPM.JournalLines[3].DebitAccountId, "");
                    }
                    List<LedgerTransactionPM> myLedgerTransactionBankTransferPMs;
                    BankAccountPM bankAccountFromTransfer;
                    ReconcileExternalPageLinePM myReconcileExternalPageLinePM;
                    string errString;
                    myExternalReconcileMoveBankCheckFromTransfer2GLAccountService.PrepareAndValidate(myJournalPM.Tenant, false,
                        ///myJournalPM.JournalExternalReconciles[0].LedgerTransactionId
                        myJournalPM.JournalExternalReconciles.Select(r => r.LedgerTransactionId).ToList()
                        , myJournalPM.JournalExternalReconciles[0].ReconcileExternalPageLineId, out myLedgerTransactionBankTransferPMs, out bankAccountFromTransfer, out myReconcileExternalPageLinePM, out errString);
                    if (!string.IsNullOrWhiteSpace(errString))
                    {
                        errorsList.Add(errString);
                    }

                }
            }
        }


         void onRegilarJournalAvoidTheSameReference4DebitOrCredit_DochMaaam(MyList<string> errorsList, JournalPM myJournalPM)
        {
            var regular = new JournalTypeDetails() { JournalTypeID = "0", EnglishName = "Regular", LocalName = "רגיל" };
                    if (myJournalPM.TypeCode == regular.Code) //AddClosedTables.AddJournalType(new JournalTypeDetails() { JournalTypeID = "0", EnglishName = "Regular", LocalName = "רגיל" }, journalTypeRepository);
            {
                var creditCardWithTheSameReference1 =
                    (from jl in myJournalPM.JournalLines
                     .Where(r => !string.IsNullOrWhiteSpace(r.Reference1))
                     .Where(r => r.ActionTypeCodeEnum == JournalActionTypeEnum.Credit)
                     group jl by new { jl.CreditAccountId, jl.Reference1 } into jlGroup
                     select new { jlGroup.Key, c = jlGroup.Count() });
                var creditCardWithTheSameReference1example = creditCardWithTheSameReference1.FirstOrDefault(r => r.c > 1);

                if (creditCardWithTheSameReference1example!=null)
                {
                    errorsList.Add(TranslateMyTextCode(JournalValidator.M_AccountingSameOppositeReference, myJournalPM.Tenant));
                }


                var debitCardWithTheSameReference1 =
                    (from jl in myJournalPM.JournalLines
                     .Where(r => !string.IsNullOrWhiteSpace(r.Reference1))
                     .Where(r => r.ActionTypeCodeEnum == JournalActionTypeEnum.Debit)
                     group jl by new { jl.CreditAccountId, jl.Reference1 } into jlGroup
                     select new { jlGroup.Key, c = jlGroup.Count() });
                var debitCardWithTheSameReference1example = debitCardWithTheSameReference1.FirstOrDefault(r => r.c > 1);
                if (debitCardWithTheSameReference1example != null)
                {
                    errorsList.Add(TranslateMyTextCode(JournalValidator.M_AccountingSameOppositeReference, myJournalPM.Tenant));
                }
            }
        }

        public static bool IsMonthOpenForAccountingDate(
            IQueryable<AccountingPeriodPM> accountingPeriodsByTypeRegular,
            //JournalPM myJournalPM
            DateTime AccountingDate,
            string accountingEntityCode = null,
            string externalSystem = null
            )
        {
            bool valid = true;
            var currentAccountingPeriodPM = accountingPeriodsByTypeRegular.FirstOrDefault(periods =>  
                periods.PeriodTypeCode=="1" && periods.Year == /*myJournalPM.*/AccountingDate.Date.Year);
            if (currentAccountingPeriodPM == null)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError(
                    "[Closed Month] Validation failed: No accounting period found for the given year. " +
                    $"AccountingDate={AccountingDate}, EntityCode={accountingEntityCode}, ExternalSystem={externalSystem}"
                );

                valid = false;
               
            }
            else
            {
                var accountingDateMonth = /*myJournalPM.*/AccountingDate.Date.Month;

                if (accountingDateMonth > currentAccountingPeriodPM.ClosedMonth.GetValueOrDefault())
                {
                    //Valid ... AccountingDateMonth must be greater than close Mounth
                }
                else
                {
                    //Not Valid ... AccountingDateMonth must be greater than close Mounth
                    //not valid  8>=8 
                    //not valid  0>=1 - Must Open mounth before work on year !!
                    NetCommonHelper.Logger.DevLog.Instance.WriteError(
                        "[Closed Month] Validation failed: The month is closed for posting (accountingDateMonth <= ClosedMonth). " +
                        $"ClosedMonth={currentAccountingPeriodPM.ClosedMonth}, AccountingDate={AccountingDate}, EntityCode={accountingEntityCode}, ExternalSystem={externalSystem}"
                    );


                    valid = false;
                }
                if (accountingDateMonth == currentAccountingPeriodPM.OpenMonth)
                {
                    //valid ... accountingDateMonth can be  equal to OpenMonth
                }
                else if (accountingDateMonth < currentAccountingPeriodPM.OpenMonth)
                {
                    //valid ... accountingDateMonth can be  less than OpenMonth
                } else if (((!string.IsNullOrEmpty(externalSystem) && accountingEntityCode == "1") || accountingEntityCode == "2" 
                    || accountingEntityCode == "3" || accountingEntityCode == "4") &&  accountingDateMonth > currentAccountingPeriodPM.OpenMonth)
                {
                    //valid from API
                }
                else
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteError(
                        "[Closed Month] Validation failed: The month is not open for posting (accountingDateMonth > OpenMonth and not allowed by entity/system). " +
                        $"OpenMonth={currentAccountingPeriodPM.OpenMonth}, AccountingDate={AccountingDate}, EntityCode={accountingEntityCode}, ExternalSystem={externalSystem}"
                    );
                    valid = false;
                }
            }
            return valid;
        }



        public static ITextCodeTranslator OverrideITextCodeTranslator { get; set; }
        string TranslateMyTextCode(string textCodeCode, int tenant)
        {
            string trans = "";
            if (OverrideITextCodeTranslator != null)
            {
                trans = OverrideITextCodeTranslator.Translate(textCodeCode, tenant);
            }
            else
            {
                //bool useLocal = !(GetLoggedContact(tenant).DontShowLocal);

                bool useLocal = ToUseLocalText(tenant);

                //trans = TextCodesTranslator.TranslateText(textCodeCode, tenant);
                trans = TranslateTextsClass.Translate(textCodeCode, tenant, useLocal) /*+ " " + _JLineNumberTExt*/;
                if (string.IsNullOrWhiteSpace(trans))
                {
                    trans = "$Text(" + textCodeCode + ")";//Our Version Of Uniface Convention
                }
                trans += " " + _JLineNumberTExt; // shoul use string builder !!!!
            }
            if (string.IsNullOrWhiteSpace(trans))
            {
                trans = "$Text(" + textCodeCode + ")";//Our Version Of Uniface Convention
            }
            return trans;
        }
        string TranslateMyTextCodeDisplay(string textCodeCode, int tenant, JournalLinePM currJournalLinePM)
        {
            string trans = "";
            if (OverrideITextCodeTranslator != null)
            {
                trans = OverrideITextCodeTranslator.Translate(textCodeCode, tenant);
            }
            else
            {
                //bool useLocal = !(GetLoggedContact(tenant).DontShowLocal);

                bool useLocal = ToUseLocalText(tenant);

                //trans = TextCodesTranslator.TranslateText(textCodeCode, tenant);
                trans = TranslateTextsClass.Translate(textCodeCode, tenant, useLocal) /*+ " " + _JLineNumberTExt*/;
                if (string.IsNullOrWhiteSpace(trans))
                {
                    trans = "$Text(" + textCodeCode + ")";//Our Version Of Uniface Convention
                }
                var accountingContext = AccountingContext.GetContext(tenant);
                GLAccountQueryService glaq = new GLAccountQueryService(accountingContext);
                string debitDisplay = glaq.GetDisplayNumberByGLAccountId(currJournalLinePM.DebitAccountId,tenant);
                string creditDisplay = glaq.GetDisplayNumberByGLAccountId(currJournalLinePM.CreditAccountId, tenant);

                string myJLineNumberTExt = ToUseLocalText(tenant) ? $"(שורת פקודה {currJournalLinePM.Line}, חשבון חובה {debitDisplay}, חשבון זכות {creditDisplay})"
                                                                  : $"(Journal Line {currJournalLinePM.Line}, Debit Account {debitDisplay}, Credit Account {creditDisplay})";

                trans += " " + myJLineNumberTExt; // should use string builder 


            }
            if (string.IsNullOrWhiteSpace(trans))
            {
                trans = "$Text(" + textCodeCode + ")";//Our Version Of Uniface Convention
            }
            return trans;
        }

        private bool ToUseLocalText(int tenant)
        {
            bool useLocal = true;
            var user = GetLoggedContact(tenant);
            if (user != null) useLocal = !(GetLoggedContact(tenant).DontShowLocal);
            return useLocal;
        }

        private  void CheckGLAccountCurrency(MyList<string> errorsList, IJournalValidatorContextDataProvider myDataProvider, string glAccId, string jlCurrencyId)
        {
            throw new NotImplementedException();
        }

        private  void CheckGLAccount(
            bool isCreditSide,
            MyList<string> errorsList,
            IJournalValidatorContextDataProvider myGLAccountDataProvider, 
            string glAccId, string jlCurrencyId
            , FullAccountingSettingPM tenantFullAccountingSettingPM,
            JournalPM myJournalPM,JournalLinePM myJournalLinePM,
            bool? SuppressCheckGLAccountIsMultiCurrencyWI40640,
            bool SuppressInactiveCheck
            )
        {

            ContactPM loggedUser = GetLoggedContact(myJournalPM.Tenant);
            bool showLocal = !((bool)loggedUser?.DontShowLocal);

            var tenant=myJournalPM.Tenant;
            if (String.IsNullOrWhiteSpace(glAccId))
            {
                return;
            }
            if (tenantFullAccountingSettingPM != null)
            {
                // אסור להזין כרטיסים ראשיים  ב   JournalLineS הם אמוריים להיות IsControlAccount (– אבל כמובן שאצלנו ב DB  הם לא !) 
                if (tenantFullAccountingSettingPM.AirExportJobControlAccountId == glAccId)
                {
                    errorsList.Add(TranslateSimilarAccountId(isCreditSide, myGLAccountDataProvider, glAccId, "AirExportJobControlAccountId", tenant));
                }
                if (tenantFullAccountingSettingPM.AirImportJobControlAccountId == glAccId)
                {
                    errorsList.Add(TranslateSimilarAccountId(isCreditSide, myGLAccountDataProvider, glAccId, "AirImportJobControlAccountId", tenant));
                }
                if (tenantFullAccountingSettingPM.CustomerControlAccountId == glAccId)
                {
                    errorsList.Add(TranslateSimilarAccountId(isCreditSide, myGLAccountDataProvider, glAccId, "CustomerControlAccountId", tenant));
                }
                if (tenantFullAccountingSettingPM.FileControlAccountId == glAccId)
                {
                    errorsList.Add(TranslateSimilarAccountId(isCreditSide, myGLAccountDataProvider, glAccId, "FileControlAccountId", tenant));
                }
                if (tenantFullAccountingSettingPM.OceanExportJobControlAccountId == glAccId)
                {
                    errorsList.Add(TranslateSimilarAccountId(isCreditSide, myGLAccountDataProvider, glAccId, "OceanExportJobControlAccountId", tenant));
                }
                if (tenantFullAccountingSettingPM.OceanImportJobControlAccountId == glAccId)
                {
                    errorsList.Add(TranslateSimilarAccountId(isCreditSide, myGLAccountDataProvider, glAccId, "OceanImportJobControlAccountId", tenant));
                }
                if (tenantFullAccountingSettingPM.VendorControlAccountId == glAccId)
                {
                    errorsList.Add(TranslateSimilarAccountId(isCreditSide, myGLAccountDataProvider, glAccId, "VendorControlAccountId", tenant));
                }
            }
            var pmAcc = myGLAccountDataProvider.GetGLAccount(glAccId, myJournalPM.Tenant);
            if (pmAcc == null)
            {
                errorsList.Add(TranslateMyTextCode(JournalValidator.M_GetGLAccountReturnNull,tenant) + 
                    //glAccId
                    GetAccountName(myGLAccountDataProvider, glAccId, myJournalPM.Tenant)
                    );
                return;
            }

            if (pmAcc.Inactive.GetValueOrDefault())
            {
                string msg = TranslateMyTextCode(JournalValidator.M_BlockedGLAccount, tenant);
                msg = msg.Replace("%name", GetAccountName(myGLAccountDataProvider, glAccId, myJournalPM.Tenant, showLocal));
                if (!SuppressInactiveCheck)
                {
                    errorsList.Add(msg);
                }
                

                //errorsList.Add(TranslateMyTextCode(JournalValidator.M_BlockedGLAccount,tenant) + 
                //    //glAccId
                //    GetAccountName(myGLAccountDataProvider, glAccId, myJournalPM.Tenant) + "-  " + glAccId
                //    );

                return;
            }
            if (pmAcc.AccountTypeCode == "1")// card    1	כרטיס	Card
            {

            }
            else
            {
                /// כל כרטיס שהוא לא מסוג 1 חייב  להיות לו CONTROL ACCOUNT  - בסטטוס APPROVE  משלים את השדה ברמת ה JournalLineS (בהעברה להנה"ח מעביר ל LedgerTransactions)
                if (String.IsNullOrWhiteSpace(pmAcc.ControlAccountId))
                {
                    errorsList.Add(TranslateMyTextCode(JournalValidator.M_ControlAccountIdIsMust,tenant) + 
                        //glAccId
                        GetAccountName(myGLAccountDataProvider, glAccId, myJournalPM.Tenant)
                        );
                }
                else
                {
                    


                    var isApproved=myJournalPM.StatusCode=="6";
                    if (isApproved)
                    {
                        var debitDebitControlId = myJournalLinePM.DebitControlAccountId;
                        var glCreditControlAccId = myJournalLinePM.CreditControlAccountId;
                        if (isCreditSide)
                        {
                            if (pmAcc.ControlAccountId != glCreditControlAccId)
                            {
                                errorsList.Add(TranslateMyTextCode(JournalValidator.M_ControlAccountIdIsNotMatch,tenant));
                            }
                        }
                        else
                        {
                            if (pmAcc.ControlAccountId != debitDebitControlId)
                            {
                                errorsList.Add(TranslateMyTextCode(JournalValidator.M_ControlAccountIdIsNotMatch,tenant));
                            }
                        }
                    }
                }
            }

            if (pmAcc.IsControlAccount.GetValueOrDefault())
            {
                errorsList.Add(TranslateIsAccountControl(isCreditSide, GetAccountName(myGLAccountDataProvider, glAccId, myJournalPM.Tenant), tenant));
            }

            //ohad
            if (!String.IsNullOrWhiteSpace(pmAcc.CurrencyId) && jlCurrencyId != pmAcc.CurrencyId)
            {
                //errorsList.Add(
                //    TranslateMyTextCode("Accounting.General.O.CurrentCurrency",0) + " " + GetCurrencyCode(myGLAccountDataProvider, jlCurrencyId, myJournalPM.Tenant) 
                //    + TranslateMyTextCode(JournalValidator.M_ButAccountCurrencyisDifferent/*"Accounting.General.O.ButAccountCurrencyDifferent"*/, 0) + " " + GetCurrencyCode(myGLAccountDataProvider, pmAcc.CurrencyId, myJournalPM.Tenant)
                //    + " ( " + TranslateMyTextCode("Accounting.General.O.GLAccountIs",0) + " " + GetAccountName(myGLAccountDataProvider, pmAcc.Id, myJournalPM.Tenant) + " )");

                // WI:48580
                //if(myJournalPM.AccountingEntityCode != "2")//REM by A. Khitrik--04.Apr.2023--180465-- 
                errorsList.Add(TranslateMyTextCode("Accounting.General.O.PaymentBankAccountCurrencyDifferent", myJournalPM.Tenant));
            }
            if (pmAcc.IsMultiCurrency.GetValueOrDefault())
            {
                
                var list = myGLAccountDataProvider.GetGLAccountCurrencyList(glAccId, pmAcc.Tenant);
                var GlAcc4MyCurrency = list.FirstOrDefault(r => r == jlCurrencyId);
                if (GlAcc4MyCurrency != null)
                {
                    if (SuppressCheckGLAccountIsMultiCurrencyWI40640.GetValueOrDefault())
                    {
                        //B4 streaming its not must 
                    }
                    else
                    {
                        //on streaming its must 
                        errorsList.Add("Using cards that do not match their currency is not allowed =Current MultiCurrencyAccount is  " +
                            GetAccountName(myGLAccountDataProvider, glAccId, myJournalPM.Tenant, showLocal) +
                            " ,for Currency " +

                            GetCurrencyCode(myGLAccountDataProvider, jlCurrencyId, myJournalPM.Tenant)
                            + "  please use GLAccount =" + GlAcc4MyCurrency);
                    }
                }

            }



        }

        private string TranslateSimilarAccountId(bool isCreditAccount, IJournalValidatorContextDataProvider myGLAccountDataProvider, string glAccId, string fullAcountingSetting, int tenant)
        {
            bool useLocal = ToUseLocalText(tenant);
            var accountFieldName = isCreditAccount ? "CreditAccountId": "DebitAccountId";
            string accountTranslation = TranslateTextsClass.Translate($"JournalLine.F.{accountFieldName}", tenant, useLocal) ?? accountFieldName;
            string accountName = GetAccountName(myGLAccountDataProvider, glAccId, tenant);
            if (!string.IsNullOrEmpty(accountName))
            {
                accountTranslation += ": " + accountName;
            }

            string fullAcountingSettingTransalation = TranslateTextsClass.Translate($"FullAccountingSetting.F.{fullAcountingSetting}", tenant, useLocal) ?? fullAcountingSetting;

            string error = TranslateTextsClass.Translate("Journal.O.AccountIdenticalToFullAccountingSetting", tenant, useLocal)
                .Replace("{account}", accountTranslation)
                .Replace("{setting}", fullAcountingSettingTransalation);

            error += " " + _JLineNumberTExt;

            return error;
        }

        private string TranslateIsAccountControl(bool isCreditAccount, string accountName, int tenant)
        {
            bool useLocal = ToUseLocalText(tenant);
            var accountFieldName = isCreditAccount ? "CreditAccountId" : "DebitAccountId";
            string accountTranslation = TranslateTextsClass.Translate($"JournalLine.F.{accountFieldName}", tenant, useLocal) ?? accountFieldName;
            if (!string.IsNullOrEmpty(accountName))
            {
                accountTranslation += ": " + accountName;
            }

            string error = TranslateTextsClass.Translate("Journal.O.AccountIsAccountControl", tenant, useLocal)
                .Replace("{account}", accountTranslation);

            error += " " + _JLineNumberTExt;

            return error;
        }

        private void CheckGLAccountThin(
    bool isCreditSide,
    MyList<string> errorsList,
    IJournalValidatorContextDataProvider myGLAccountDataProvider,
    string glAccId, string jlCurrencyId
    , FullAccountingSettingPM tenantFullAccountingSettingPM,
    JournalPM myJournalPM, JournalLinePM myJournalLinePM,
    bool? SuppressCheckGLAccountIsMultiCurrencyWI40640,
    bool SuppressInactiveCheck
    )
        {

            ContactPM loggedUser = GetLoggedContact(myJournalPM.Tenant);
            bool showLocal = !((bool)loggedUser?.DontShowLocal);

            var tenant = myJournalPM.Tenant;
            if (String.IsNullOrWhiteSpace(glAccId))
            {
                return;
            }
            if (tenantFullAccountingSettingPM != null)
            {

                var pmAcc = myGLAccountDataProvider.GetGLAccount(glAccId, myJournalPM.Tenant);
                if (pmAcc == null)
                {
                    errorsList.Add(TranslateMyTextCode(JournalValidator.M_GetGLAccountReturnNull, tenant) +
                        //glAccId
                        GetAccountName(myGLAccountDataProvider, glAccId, myJournalPM.Tenant)
                        );
                    return;
                }
                //ohad
                if (!String.IsNullOrWhiteSpace(pmAcc.CurrencyId) && jlCurrencyId != pmAcc.CurrencyId)
            {
                errorsList.Add(TranslateMyTextCode("Accounting.General.O.PaymentBankAccountCurrencyDifferent", myJournalPM.Tenant));
            }
   

            }
          }

        private string GetAccountName(IJournalValidatorContextDataProvider myGLAccountDataProvider, string accId, int tenant, bool showLocal = true)
        {
            if (myGLAccountDataProvider == null) return accId;
            var pm = myGLAccountDataProvider.GetGLAccount(accId, tenant);
            if (pm == null)
            {
                return accId;
            }

            return pm.DisplayNumber + "-" + (showLocal ? pm.LocalName : pm.EnglishName);
        }

        private  string GetCurrencyCode(IJournalValidatorContextDataProvider myGLAccountDataProvider,
            string jlCurrencyId,
            int tenant)
        {
            if (myGLAccountDataProvider == null) return jlCurrencyId;
            var pm = myGLAccountDataProvider.GetCurrency(jlCurrencyId, tenant);
            if (pm == null)
            {
                return jlCurrencyId;
            }
            return pm.Code;
        }


        private  void CalculateTotals(JournalLinePM item, ref decimal creditTotal, ref decimal debitTotal)
        {
            if ((item.ActionCode == "3" || item.ActionCode == "4") && (item.LocalAmount != null))
            {
                creditTotal = creditTotal + (decimal)item.LocalAmount;
                debitTotal = debitTotal + (decimal)item.LocalAmount;
            }
            if ((item.ActionCode == "2") && (item.LocalAmount != null))
            {

                debitTotal = debitTotal + (decimal)item.LocalAmount;
            }
            if ((item.ActionCode == "1") && (item.LocalAmount != null))
            {

                creditTotal = creditTotal + (decimal)item.LocalAmount;
            }
        }

        //// server side validations
        //public static bool CheckExternalNoAndSystem(string externalNo, string externalSystem, ref string journalNumber, int tenant)
        //{
        //    if (String.IsNullOrWhiteSpace(externalNo) || String.IsNullOrWhiteSpace(externalSystem))
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
        //        JournalQueryService journalQuery = new JournalQueryService(accountingContext);
        //        return journalQuery.CheckIfExternalNoAndSystemExist(externalNo, externalSystem, ref journalNumber, tenant);
        //    }
        //}



        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }




        public static ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }

            ILoggedContactUtil loggedContactUtil = ContainerAccessor.Container.Resolve(typeof(ILoggedContactUtil), "LoggedContactUtil", new ParameterOverride("", tenant)) as ILoggedContactUtil;
            ContactPM loggedcontact = loggedContactUtil.GetLoggedContact(tenant);

            //ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }


    }

    public class MyList<t>:List<t>
    {

        public new void AddNew(t item)
        {
            base.Add(item);
        }
        public new void Add(t item)
        {
            AddNew(item);
        }
    }

}
