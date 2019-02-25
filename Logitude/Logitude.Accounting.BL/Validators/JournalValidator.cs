
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.Interfaces;
using Logitude.BL.Resolvers;
using Logitude.BL.Security;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
[assembly: InternalsVisibleTo("My1stUnitTestProject")]


namespace Logitude.Accounting.BL.Validators
{
    public partial class JournalValidator
    {
        #region My MessaGE Unit Test Region


        public const string M_YouShouldHaveOneLineAtLeast = "Journal.M.YouShouldHaveOneLineAtLeast";
        public const string M_ClosedMonth = "ARInvoice.M.ClosedMonth"; //"AccountingPeriod.F.ClosedMonth";
        public const string M_ExternalNoAlreadyExists_1 = "Journals.O.ExternalNoAlreadyExists_1";
        public const string M_ExternalNoAlreadyExists_2 = "Journals.O.ExternalNoAlreadyExists_2";
        public const string M_ExternalNoAlreadyExists_3 = "Journals.O.ExternalNoAlreadyExists_3";
        public const string M_ExternalNoAlreadyExists_4 = "Journals.O.ExternalNoAlreadyExists_4";
        public const string M_ExchangeRateEmpty = "Journal.M.ExchangeRateEmpty";
        // ForeignAmount Allowed ...  public const string M_ForeignAmountNotZero = "Journal.M.ForeignAmountNotZero";
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
        public const string M_BlockedGLAccount = "Blocked GLAccounts(Inactive=True)";

        public const string M_GLAccountIsControl = "GLAccount IsControl=True";
        public const string M_ButAccountCurrencyisDifferent =
            ///" But Account Currency is Different ";
            "Accounting.General.O.ButAccountCurrencyDifferent";

        public const string M_JLAccountingDateMustWithinJournalMonth = "Journal.M.JLAccountingDateMustWithinJournalMonth";

        public const string M_AccountingFutureDateForbidden = "Journal.M.FutureDateForbidden";
        
        public const string M_ControlAccountIdIsMust =
            "Journal.M.ControlAccountIdIsMust";

        public const string M_ControlAccountIdIsNotMatch=
            "Journal.M.ControlAccountIdIsNotMatch";


        public const string M_DueDateIsMust = "Journal.M.DueDateIsMust";

        public const string M_FAMltiExchangerateNELA = "Journal.M.FAMltiExchangerateNELA";////Foreign amount ({0}) multiplied by the exchange rate ({1}) does not equal the local amount ({2})
        #endregion

        public const string K_AccountingPeriodsByTypeRegular = "AccountingPeriodsByTypeRegular";
        public const string K_DateTimeUtcNow = "K_DateTimeUtcNow";
        public const string K_FullAccountingSettingPM = "FullAccountingSettingPM";
        public const string K_SuppressCheckGLAccountIsMultiCurrencyWI40640 = "K_SuppressCheckGLAccountIsMultiCurrencyWI40640";//Task 40640: טיפול בסרביס לפקודת יומן - במקרה של כרטיס מפוצל לרשום על הפיצול
        public const string M_AccountingSameOppositeReference = "Journal.M.SameOppositeReference";

        public const string M_AllDateMustInit = "Journal.M.AllDateMustInit";

        public static ValidationResult IsJournalValid(
          JournalPM myJournalPM,
          System.ComponentModel.DataAnnotations.ValidationContext context)
        {
            decimal creditTotal = 0;
            decimal debitTotal = 0;


            List<string> errorsList = new List<string>();

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
                //TranslateTextsClass.Translate("Journal.M.YouShouldHaveOneLineAtLeast", 0, useLocal);
                ;
                errorsList.Add(msg);
            }
            var myDataProvider = context.GetService(typeof(IJournalValidatorContextDataProvider)) as IJournalValidatorContextDataProvider;
            FullAccountingSettingPM tenantFullAccountingSettingPM = null;
            if (context.Items.ContainsKey(K_FullAccountingSettingPM))
            {
                tenantFullAccountingSettingPM = context.Items[K_FullAccountingSettingPM] as FullAccountingSettingPM;
            }
            DateTime? currDateTimeUtcNow = null; ;

            if (context.Items.ContainsKey(K_DateTimeUtcNow))
            {
                currDateTimeUtcNow = (DateTime)context.Items[K_DateTimeUtcNow];
            }
            currDateTimeUtcNow = currDateTimeUtcNow ?? DateTime.UtcNow;

            if (context.Items.ContainsKey(K_AccountingPeriodsByTypeRegular))
            {
                var accountingPeriodsByTypeRegular = context.Items[K_AccountingPeriodsByTypeRegular] as List<AccountingPeriodPM>;
                if (accountingPeriodsByTypeRegular != null)
                {
                    string transText = "";
                    transText = TranslateMyTextCode(M_ClosedMonth, myJournalPM.Tenant);
                    if (String.IsNullOrWhiteSpace(transText))
                    {
                        transText = "Closed Month";
                    }

                    if (!IsMonthOpenForAccountingDate(accountingPeriodsByTypeRegular.AsQueryable(), myJournalPM.AccountingDate))
                    {
                        errorsList.Add(transText);
                        valid = false;
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

                if (clientExists == true)
                {
                    string basic_text_ExternalExist = TranslateMyTextCode(M_ExternalNoAlreadyExists_1, myJournalPM.Tenant)
                            + myJournalPM.ExternalNo
                            + TranslateMyTextCode(M_ExternalNoAlreadyExists_2, myJournalPM.Tenant)
                            + myJournalPM.ExternalSystem
                            + TranslateMyTextCode(M_ExternalNoAlreadyExists_3, myJournalPM.Tenant);

                    if (String.IsNullOrWhiteSpace(myJournalPM.JournalNumber))
                    {
                        errorsList.Add(basic_text_ExternalExist);
                    }
                    else
                    {
                        errorsList.Add(basic_text_ExternalExist + journalNumber
                            + TranslateMyTextCode(M_ExternalNoAlreadyExists_4, myJournalPM.Tenant));
                    }
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
                errorsList.Add(TranslateMyTextCode(M_ExchangeRateEmpty, myJournalPM.Tenant));
            }

            
#endif
            if (currDateTimeUtcNow.GetValueOrDefault().Date < myJournalPM.AccountingDate.Date)
            {
                errorsList.Add(TranslateMyTextCode(M_AccountingFutureDateForbidden, myJournalPM.Tenant));
                valid = false;
            }
            if (myJournalPM.JournalLines.Any(l => currDateTimeUtcNow.GetValueOrDefault().Date < l.AccountingDate.Date))
            {
                errorsList.Add(TranslateMyTextCode(M_AccountingFutureDateForbidden, myJournalPM.Tenant));
                valid = false;
            }
            if (myJournalPM.JournalLines.Any(l =>
                l.AccountingDate.Date.Year != myJournalPM.AccountingDate.Date.Year ||
                l.AccountingDate.Date.Month != myJournalPM.AccountingDate.Date.Month
                ))
            {
                errorsList.Add(TranslateMyTextCode(M_JLAccountingDateMustWithinJournalMonth, myJournalPM.Tenant));
            }

            //47045 onRegilarJournalAvoidTheSameReference4DebitOrCredit_DochMaaam(errorsList,myJournalPM);

            if (myJournalPM.JournalLines.Any(l => l.AccountingDate == DateTime.MinValue))
            {
                errorsList.Add(TranslateMyTextCode(M_AllDateMustInit, myJournalPM.Tenant));
            }
            if (myJournalPM.JournalLines.Any(l => l.DueDate == DateTime.MinValue))
            {
                errorsList.Add(TranslateMyTextCode(M_AllDateMustInit, myJournalPM.Tenant));
            }
            if (myJournalPM.JournalLines.Any(l => l.DocumentDate == DateTime.MinValue))
            {
                errorsList.Add(TranslateMyTextCode(M_AllDateMustInit, myJournalPM.Tenant));
            }
            int seq = 0;
            foreach (JournalLinePM currJournalLinePM in myJournalPM.JournalLines
                .Where(jl => jl.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete)
                // i don't know how HTML5 Work But I Soppsed that I Get (postback) All  JournalLines All the Time 
                .OrderBy(r => r.Line))
            {
                seq++;
                if (currJournalLinePM.Line != seq)
                {
                    errorsList.Add(M_LineSequence + seq.ToString() + " !=" + currJournalLinePM.Line.ToString());
                }
                //if (item.ForeignAmount == 0)
                if (currJournalLinePM.ForeignAmount
                    //.GetValueOrDefault()
                    == 0)
                {
                    //errorsList.Add(TranslateMyTextCode(M_ForeignAmountNotZero, myJournalPM.Tenant));
                }
                //if (item.LocalAmount == 0)
                if (currJournalLinePM.LocalAmount
                    //.GetValueOrDefault() 
                    == 0)
                {
                    //errorsList.Add(TranslateMyTextCode(M_LocalAmountNotZero, myJournalPM.Tenant));
                }
                if (string.IsNullOrWhiteSpace(currJournalLinePM.ActionCode) && string.IsNullOrWhiteSpace(currJournalLinePM.ActionTypeCode))
                {
                    errorsList.Add(TranslateMyTextCode(M_ActionCode, myJournalPM.Tenant));
                }
                if (currJournalLinePM.ActionTypeCode == "2" && (currJournalLinePM.DebitAccountId == null))
                {
                    errorsList.Add(TranslateMyTextCode(M_ActionCodeDebit, myJournalPM.Tenant));
                }
                if (currJournalLinePM.ActionTypeCode == "1" && (currJournalLinePM.CreditAccountId == null))
                {
                    errorsList.Add(TranslateMyTextCode(M_ActionCodeCredit, myJournalPM.Tenant));
                }
                if ((currJournalLinePM.ActionTypeCode == "3" || currJournalLinePM.ActionTypeCode == "4") && ((currJournalLinePM.DebitAccountId == null) || (currJournalLinePM.CreditAccountId == null)))
                {
                    errorsList.Add(TranslateMyTextCode(M_ActionCodeCreditAndCreditMeanDebit, myJournalPM.Tenant));
                }
                //if (!currJournalLinePM.DueDate.HasValue)
                //{
                //    errorsList.Add(TranslateMyTextCode(M_DueDateIsMust, myJournalPM.Tenant));
                //}
                bool baselSuppressDocGreaterThenDue = true;
                if (!baselSuppressDocGreaterThenDue)
                {
                    if ((currJournalLinePM.DocumentDate) > (currJournalLinePM.DueDate))
                    {
                        errorsList.Add(TranslateMyTextCode(M_DocumentDateBiggerDueDate, myJournalPM.Tenant));
                    }
                }
                if (currJournalLinePM.ForeignAmount == 0)
                {
                    if (currJournalLinePM.ExchangeRate.GetValueOrDefault() != 0)
                    {
                        var mM_FAMltiExchangerateNELA = TranslateMyTextCode(M_FAMltiExchangerateNELA, myJournalPM.Tenant);
                        //mM_FAMltiExchangerateNELA=mM_FAMltiExchangerateNELA??"Foreign amount ({0}) multiplied by the exchange rate ({1}) does not equal the local amount ({2})";
                        mM_FAMltiExchangerateNELA = String.Format(mM_FAMltiExchangerateNELA, currJournalLinePM.ForeignAmount, currJournalLinePM.ExchangeRate, currJournalLinePM.LocalAmount);
                        errorsList.Add(mM_FAMltiExchangerateNELA);
                    }
                }
                //{{"JournalId":"1-736052","Tenant":1,"Line":1,"ActionCode":"1-1","DebitControlAccountId":null,"DebitAccountId":null,"CreditControlAccountId":"1-5","CreditAccountId":"1-19152","DocumentDate":"2016-11-22T09:35:38.5272647+02:00","AccountingDate":"2017-01-23T00:00:00","DueDate":"2017-01-16T00:00:00","LocalAmount":0.0,"CurrencyId":"1-7","ForeignAmount":1.0,"ExchangeRate":0.0,"Reference1":null,"Reference2":null,"Reference3":null,"ActionName":"Credit","DebitControlAccountName":null,"CreditAccountName":null,"DebitAccountName":null,"CreditControlAccountName":null,"CreditControlAccountNumber":null,"DebitControlAccountNumber":null,"CreditAccountNumber":null,"DebitAccountNumber":null,"CurrencyName":null,"Notes":null,"CurrencyCode":null,"ActionTypeCode":"1","ExternalOpenAmount":null,"IsCreditAccountMulti":null,"IsDebitAccountMulti":null,"ActionTypeCodeEnum":1,"ChangeSetOp":1,"EncodeBase64NVARCHARFieldsBy":null}}
                else if (currJournalLinePM.LocalAmount == 0)
                {
                    if (currJournalLinePM.ExchangeRate.GetValueOrDefault() != 0)
                    {
                        var mM_FAMltiExchangerateNELA = TranslateMyTextCode(M_FAMltiExchangerateNELA, myJournalPM.Tenant);
                        //mM_FAMltiExchangerateNELA=mM_FAMltiExchangerateNELA??"Foreign amount ({0}) multiplied by the exchange rate ({1}) does not equal the local amount ({2})";
                        mM_FAMltiExchangerateNELA = String.Format(mM_FAMltiExchangerateNELA, currJournalLinePM.ForeignAmount, currJournalLinePM.ExchangeRate, currJournalLinePM.LocalAmount);
                        errorsList.Add(mM_FAMltiExchangerateNELA);
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
                                var mM_FAMltiExchangerateNELA = TranslateMyTextCode(M_FAMltiExchangerateNELA, myJournalPM.Tenant);
                                //mM_FAMltiExchangerateNELA=mM_FAMltiExchangerateNELA??"Foreign amount ({0}) multiplied by the exchange rate ({1}) does not equal the local amount ({2})";
                                mM_FAMltiExchangerateNELA = String.Format(mM_FAMltiExchangerateNELA, currJournalLinePM.ForeignAmount, currJournalLinePM.ExchangeRate, currJournalLinePM.LocalAmount);
                                errorsList.Add(mM_FAMltiExchangerateNELA);
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
                        var Mustbegreaterthancurrentdate = TranslateMyTextCode(M_DueDateMustgreaterthancurrent, myJournalPM.Tenant);
                        if (String.IsNullOrWhiteSpace(Mustbegreaterthancurrentdate))
                        {
                            Mustbegreaterthancurrentdate = "Due Date ,Must be greater than current date ";
                        }
                        errorsList.Add(Mustbegreaterthancurrentdate);
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
                            errorsList.Add(TranslateMyTextCode(M_currencydoesnotexist, myJournalPM.Tenant));
                        }
                    }


                    var glCreditAccId = currJournalLinePM.CreditAccountId;


                    var debitAccountId = currJournalLinePM.DebitAccountId;



                    bool? SuppressCheckGLAccountIsMultiCurrencyWI40640 = false;
                    if (context.Items.ContainsKey(K_SuppressCheckGLAccountIsMultiCurrencyWI40640))
                    {
                        SuppressCheckGLAccountIsMultiCurrencyWI40640 = context.Items[K_SuppressCheckGLAccountIsMultiCurrencyWI40640] as bool?;
                    }
                    else
                    {
                        throw new Exception("Dear Programmer U must initialize in context SuppressCheckGLAccountIsMultiCurrencyWI40640");
                    }


                    var jlCurrencyId = currJournalLinePM.CurrencyId;
                    currJournalLinePM.ActionTypeCode = currJournalLinePM.ActionTypeCode ?? string.Empty;
                    switch (currJournalLinePM.ActionTypeCode.ToString())//will be valid on server side only
                    {
                        case "1"://MyJournalActionTypeEnum.Credit:
                            CheckGLAccount(true, errorsList, myDataProvider, glCreditAccId, jlCurrencyId, tenantFullAccountingSettingPM, myJournalPM, currJournalLinePM
                                , SuppressCheckGLAccountIsMultiCurrencyWI40640);
                            break;
                        case "2": //MyJournalActionTypeEnum.Debit:
                            CheckGLAccount(false, errorsList, myDataProvider, debitAccountId, jlCurrencyId, tenantFullAccountingSettingPM, myJournalPM, currJournalLinePM
                                , SuppressCheckGLAccountIsMultiCurrencyWI40640);
                            break;
                        case "3"://MyJournalActionTypeEnum.DebitAndCredit:
                        case "4"://MyJournalActionTypeEnum.DebitCreditAndVatdeduction:
                            CheckGLAccount(true, errorsList, myDataProvider, glCreditAccId, jlCurrencyId, tenantFullAccountingSettingPM, myJournalPM, currJournalLinePM
                                , SuppressCheckGLAccountIsMultiCurrencyWI40640);
                            CheckGLAccount(false, errorsList, myDataProvider, debitAccountId, jlCurrencyId, tenantFullAccountingSettingPM, myJournalPM, currJournalLinePM
                                , SuppressCheckGLAccountIsMultiCurrencyWI40640);
                            break;
                        default:
                            break;
                    }

                }


            }

            if (debitTotal != creditTotal)
            {
                if ((myJournalPM.StatusCode == "1") || (myJournalPM.StatusCode == "2"))//Draft = 0,//WaitingforApprove = 1,//Approved = 2,//Voided = 3
                {
                    errorsList.Add(TranslateMyTextCode(M_JournalAmountNotMatched, myJournalPM.Tenant));
                }
            }

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

        static void onRegilarJournalAvoidTheSameReference4DebitOrCredit_DochMaaam(List<string> errorsList, JournalPM myJournalPM)
        {
            var regular = new JournalTypeDetails() { JournalTypeID = "0", EnglishName = "Regular", LocalName = "רגיל" };
                    if (myJournalPM.TypeCode == regular.Code) //AddClosedTables.AddJournalType(new JournalTypeDetails() { JournalTypeID = "0", EnglishName = "Regular", LocalName = "רגיל" }, journalTypeRepository);
            {
                var creditCardWithTheSameReference1 =
                    (from jl in myJournalPM.JournalLines
                     .Where(r => !string.IsNullOrWhiteSpace(r.Reference1))
                     .Where(r => r.ActionTypeCodeEnum == MyJournalActionTypeEnum.Credit)
                     group jl by new { jl.CreditAccountId, jl.Reference1 } into jlGroup
                     select new { jlGroup.Key, c = jlGroup.Count() });
                var creditCardWithTheSameReference1example = creditCardWithTheSameReference1.FirstOrDefault(r => r.c > 1);

                if (creditCardWithTheSameReference1example!=null)
                {
                    errorsList.Add(TranslateMyTextCode(M_AccountingSameOppositeReference, myJournalPM.Tenant));
                }


                var debitCardWithTheSameReference1 =
                    (from jl in myJournalPM.JournalLines
                     .Where(r => !string.IsNullOrWhiteSpace(r.Reference1))
                     .Where(r => r.ActionTypeCodeEnum == MyJournalActionTypeEnum.Debit)
                     group jl by new { jl.CreditAccountId, jl.Reference1 } into jlGroup
                     select new { jlGroup.Key, c = jlGroup.Count() });
                var debitCardWithTheSameReference1example = debitCardWithTheSameReference1.FirstOrDefault(r => r.c > 1);
                if (debitCardWithTheSameReference1example != null)
                {
                    errorsList.Add(TranslateMyTextCode(M_AccountingSameOppositeReference, myJournalPM.Tenant));
                }
            }
        }

        public static bool IsMonthOpenForAccountingDate(
            IQueryable<AccountingPeriodPM> accountingPeriodsByTypeRegular,
            //JournalPM myJournalPM
            DateTime AccountingDate
            )
        {
            bool valid = true;
            var currentAccountingPeriodPM = accountingPeriodsByTypeRegular.FirstOrDefault(periods =>  
                periods.PeriodTypeCode=="1" && periods.Year == /*myJournalPM.*/AccountingDate.Date.Year);
            if (currentAccountingPeriodPM == null)
            {
                valid = false;
                //errorsList.Add(transText);
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
                    valid = false;
                    //errorsList.Add(transText); //ClosedMonth Must B
                }
                if (accountingDateMonth == currentAccountingPeriodPM.OpenMonth)
                {
                    //valid ... accountingDateMonth can be  equal to OpenMonth
                }
                else if (accountingDateMonth < currentAccountingPeriodPM.OpenMonth)
                {
                    //valid ... accountingDateMonth can be  less than OpenMonth
                }
                else
                {

                    valid = false;
                    //errorsList.Add(transText);
                }
            }
            return valid;
        }



        public static ITextCodeTranslator OverrideITextCodeTranslator { get; set; }
        static string TranslateMyTextCode(string textCodeCode, int tenant)
        {
            string trans = "";
            if (OverrideITextCodeTranslator != null)
            {
                trans = OverrideITextCodeTranslator.Translate(textCodeCode, tenant);
            }
            else
            {
                //bool useLocal = !(GetLoggedContact(tenant).DontShowLocal);

                bool useLocal = true;
                var user = GetLoggedContact(tenant);
                if (user != null) useLocal = !(GetLoggedContact(tenant).DontShowLocal);

                //trans = TextCodesTranslator.TranslateText(textCodeCode, tenant);
                trans = TranslateTextsClass.Translate(textCodeCode, tenant, useLocal);
            }
            if (string.IsNullOrWhiteSpace(trans))
            {
                trans = "$Text(" + textCodeCode + ")";//Our Version Of Uniface Convention
            }
            return trans;
        }

        private static void CheckGLAccountCurrency(List<string> errorsList, IJournalValidatorContextDataProvider myDataProvider, string glAccId, string jlCurrencyId)
        {
            throw new NotImplementedException();
        }

        private static void CheckGLAccount(
            bool isCreditSide,
            List<string> errorsList,
            IJournalValidatorContextDataProvider myGLAccountDataProvider, 
            string glAccId, string jlCurrencyId
            , FullAccountingSettingPM tenantFullAccountingSettingPM,
            JournalPM myJournalPM,JournalLinePM myJournalLinePM,
            bool? SuppressCheckGLAccountIsMultiCurrencyWI40640
            )
        {
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
                    errorsList.Add(TranslateMyTextCode(M_GLAccountIsControl,tenant));
                }

                if (tenantFullAccountingSettingPM.AirImportJobControlAccountId == glAccId)
                {
                    errorsList.Add(TranslateMyTextCode(M_GLAccountIsControl,tenant));
                }
                if (tenantFullAccountingSettingPM.CustomerControlAccountId == glAccId)
                {
                    errorsList.Add(TranslateMyTextCode(M_GLAccountIsControl,tenant));
                }

                if (tenantFullAccountingSettingPM.FileControlAccountId == glAccId)
                {
                    errorsList.Add(TranslateMyTextCode(M_GLAccountIsControl,tenant));
                }

                if (tenantFullAccountingSettingPM.OceanExportJobControlAccountId == glAccId)
                {
                    errorsList.Add(TranslateMyTextCode(M_GLAccountIsControl,tenant));
                }
                if (tenantFullAccountingSettingPM.OceanImportJobControlAccountId == glAccId)
                {
                    errorsList.Add(TranslateMyTextCode(M_GLAccountIsControl,tenant));
                }

                if (tenantFullAccountingSettingPM.VendorControlAccountId == glAccId)
                {
                    errorsList.Add(TranslateMyTextCode(M_GLAccountIsControl,tenant));
                }
            }
            var pmAcc = myGLAccountDataProvider.GetGLAccount(glAccId, myJournalPM.Tenant);
            if (pmAcc == null)
            {
                errorsList.Add(TranslateMyTextCode(M_GetGLAccountReturnNull,tenant) + 
                    //glAccId
                    GetAccountName(myGLAccountDataProvider, glAccId, myJournalPM.Tenant)
                    );
                return;
            }

            if (pmAcc.Inactive.GetValueOrDefault())
            {
                errorsList.Add(TranslateMyTextCode(M_BlockedGLAccount,tenant) + 
                    //glAccId
                    GetAccountName(myGLAccountDataProvider, glAccId, myJournalPM.Tenant)
                    );
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
                    errorsList.Add(TranslateMyTextCode(M_ControlAccountIdIsMust,tenant) + 
                        //glAccId
                        GetAccountName(myGLAccountDataProvider, glAccId, myJournalPM.Tenant)
                        );
                }
                else
                {
                    


                    var isApproved=myJournalPM.StatusCode=="2";
                    if (isApproved)
                    {
                        var debitDebitControlId = myJournalLinePM.DebitControlAccountId;
                        var glCreditControlAccId = myJournalLinePM.CreditControlAccountId;
                        if (isCreditSide)
                        {
                            if (pmAcc.ControlAccountId != glCreditControlAccId)
                            {
                                errorsList.Add(TranslateMyTextCode(M_ControlAccountIdIsNotMatch,tenant));
                            }
                        }
                        else
                        {
                            if (pmAcc.ControlAccountId != debitDebitControlId)
                            {
                                errorsList.Add(TranslateMyTextCode(M_ControlAccountIdIsNotMatch,tenant));
                            }
                        }
                    }
                }
            }
            if (pmAcc.IsControlAccount.GetValueOrDefault())
            {
                errorsList.Add(TranslateMyTextCode(M_GLAccountIsControl,tenant) + 
                    //glAccId
                    GetAccountName(myGLAccountDataProvider, glAccId, myJournalPM.Tenant)
                    );
            }



            if (!String.IsNullOrWhiteSpace(pmAcc.CurrencyId) && jlCurrencyId != pmAcc.CurrencyId)
            {
                //errorsList.Add(
                //    TranslateMyTextCode("Accounting.General.O.CurrentCurrency",0) + " " + GetCurrencyCode(myGLAccountDataProvider, jlCurrencyId, myJournalPM.Tenant) 
                //    + TranslateMyTextCode(M_ButAccountCurrencyisDifferent/*"Accounting.General.O.ButAccountCurrencyDifferent"*/, 0) + " " + GetCurrencyCode(myGLAccountDataProvider, pmAcc.CurrencyId, myJournalPM.Tenant)
                //    + " ( " + TranslateMyTextCode("Accounting.General.O.GLAccountIs",0) + " " + GetAccountName(myGLAccountDataProvider, pmAcc.Id, myJournalPM.Tenant) + " )");

                // WI:48580
                errorsList.Add(TranslateMyTextCode("Accounting.General.O.PaymentBankAccountCurrencyDifferent", 0));
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
                            GetAccountName(myGLAccountDataProvider, glAccId, myJournalPM.Tenant) +
                            " ,for Currency " +

                            GetCurrencyCode(myGLAccountDataProvider, jlCurrencyId, myJournalPM.Tenant)
                            + "  please use GLAccount =" + GlAcc4MyCurrency);
                    }
                }

            }



        }

        private static string GetAccountName(IJournalValidatorContextDataProvider myGLAccountDataProvider, string accId, int tenant)
        {
            if (myGLAccountDataProvider == null) return accId;
            var pm = myGLAccountDataProvider.GetGLAccount(accId, tenant);
            if (pm == null)
            {
                return accId;
            }
            return pm.DisplayNumber + "-" + pm.LocalName;
        }

        private static string GetCurrencyCode(IJournalValidatorContextDataProvider myGLAccountDataProvider,
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


        private static void CalculateTotals(JournalLinePM item, ref decimal creditTotal, ref decimal debitTotal)
        {
            if ((item.ActionTypeCode == "3" || item.ActionTypeCode == "4") && (item.LocalAmount != null))
            {
                creditTotal = creditTotal + (decimal)item.LocalAmount;
                debitTotal = debitTotal + (decimal)item.LocalAmount;
            }
            if ((item.ActionTypeCode == "2") && (item.LocalAmount != null))
            {

                debitTotal = debitTotal + (decimal)item.LocalAmount;
            }
            if ((item.ActionTypeCode == "1") && (item.LocalAmount != null))
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


}
