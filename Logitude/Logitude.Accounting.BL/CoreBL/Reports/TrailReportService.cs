using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data;
using Logitude.Accounting.BL.EntityQueryServices;
using System.Threading;

using Logitude.Accounting.Def.EntityPMs;

namespace Logitude.Accounting.BL.CoreBL.Reports
{
    public class TrailReportService : IDisposable
    {
        TrailReportParam _TrailReportParam = null;
        public TrailReportService(TrailReportParam trailReportParam)
        {
            _TrailReportParam = trailReportParam;
            //var trailReportChartofaccountType = new TrailReportChartofaccountType();
            //var trailReportChartofaccount= new TrailReportChartofaccount();
            //var trailReportGLAccount = new TrailReportGLAccount();
        }
        int __TimeOutInMinutes = 129;
        protected IQueryable<ChartOfAccount5LevelM> _QAllChartOfAccountFlattenBy5LevelofHierarchy;
        protected FullAccountingSettingPM _FullAccountingSetting;
        protected IQueryable<ChartOfAccount5LevelM> _QAllCardsAndDetialsAccTypeBy5LevelHierarchy;
        protected IQueryable<TrailReportTemp> _QAccumulateTotalsFrom0BCTilNotIncludeStartOfMonthFromDate;
        protected IQueryable<TrailReportTemp> _QAccumulateTotalsFromStartOfMonthFromTilStartOfMonthTo;
        protected IQueryable<TrailReportTemp> _QAccumulateTranactionBeginOfMonthFromTillFromDateNotInclude;
        protected IQueryable<TrailReportTemp> _QAccumulateTranactionBeginOfMonthToDateTillToDateInculde;
        protected IQueryable<TrailReportM> _QTrailReportFull;
        protected IQueryable<TrailReportM> _QTrailReportCOALevel;
        protected IQueryable<TrailReportM> _QTrailReportCOALevelACurrenciesDetailed;
        private System.Transactions.TransactionScope _TransactionScope;
        private IQueryable<Data.EntityPOCOs.GLAccount> _QAllCardsAndDetialsAccType;
        private IAccountingContext _AccountingContext;
        public void ParamValidation()
        {
        }




        public void Dispose()
        {
            if (_TransactionScope != null)
            {
                _TransactionScope.Dispose();
                _TransactionScope = null;
            }

        }
    }
    public class TrailReportTemp
    {


        public string AccountId_COAType { get; set; }
        public string CurrencyId { get; set; }

        public decimal? ForeignAmountCreditTotalStart { get; set; }
        public decimal? ForeignAmountDebitTotalStart { get; set; }
        public decimal? LocalAmountCreditTotalStart { get; set; }
        public decimal? LocalAmountDebitTotalStart { get; set; }


        public decimal? ForeignAmountCreditTransStart { get; set; }
        public decimal? ForeignAmountDebitTransStart { get; set; }
        public decimal? LocalAmountCreditTransStart { get; set; }
        public decimal? LocalAmountDebitTransStart { get; set; }//// תנעות מכולל תחילת החודש  של מתאריך עד למתאריך -לא כולל    


        public decimal? ForeignAmountCreditTotalDelta2End { get; set; }
        public decimal? ForeignAmountDebitTotalDelta2End { get; set; }
        public decimal? LocalAmountCreditTotalDelta2End { get; set; }
        public decimal? LocalAmountDebitTotalDelta2End { get; set; }//// מצטברים מחודש כולל ועד חודש לא כולל


        public decimal? ForeignAmountCreditTransEnd { get; set; }
        public decimal? ForeignAmountDebitTransEnd { get; set; }
        public decimal? LocalAmountCreditTransEnd { get; set; }
        public decimal? LocalAmountDebitTransEnd { get; set; }// תנעות מתחילת חודש אחרון כולל עד  תאריך הסיום + 1 לא כולל


        public bool? TotalDelta2End_AnyActivity { get; set; }
        public bool? TransEnd_AnyActivity { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("accid_COAT:" + this.AccountId_COAType);
            sb.Append("Cur:" + this.CurrencyId);




            sb.Append("TotalStart:");
            sb.Append("Local:");
            sb.Append("D" + this.LocalAmountDebitTotalStart.GetValueOrDefault().ToString());
            sb.Append("C" + this.LocalAmountCreditTotalStart.GetValueOrDefault().ToString());
            sb.Append("Foreign:");
            sb.Append("D" +this.ForeignAmountDebitTotalStart.GetValueOrDefault().ToString());
            sb.Append("C" + this.ForeignAmountCreditTotalStart.GetValueOrDefault().ToString());

            sb.Append("TransStart:");
            sb.Append("Local:");
            sb.Append("D" + this.LocalAmountDebitTransStart.GetValueOrDefault().ToString());
            sb.Append("C" + this.LocalAmountCreditTransStart.GetValueOrDefault().ToString());
            sb.Append("Foreign:");
            sb.Append("D" + this.ForeignAmountDebitTransStart.GetValueOrDefault().ToString());
            sb.Append("C" + this.ForeignAmountCreditTransStart.GetValueOrDefault().ToString());

            sb.Append("TotalDelta2End:");
            sb.Append("Local:");
            sb.Append("D" + this.LocalAmountDebitTotalDelta2End.GetValueOrDefault().ToString());
            sb.Append("C" + this.LocalAmountCreditTotalDelta2End.GetValueOrDefault().ToString());
            sb.Append("Foreign:");
            sb.Append("D" + this.ForeignAmountDebitTotalDelta2End.GetValueOrDefault().ToString());
            sb.Append("C" + this.ForeignAmountCreditTotalDelta2End.GetValueOrDefault().ToString());


            sb.Append("TransEnd:");
            sb.Append("Local:");
            sb.Append("D" + this.LocalAmountDebitTransEnd.GetValueOrDefault().ToString());
            sb.Append("C" + this.LocalAmountCreditTransEnd.GetValueOrDefault().ToString());
            sb.Append("Foreign:");
            sb.Append("D" + this.ForeignAmountDebitTransEnd.GetValueOrDefault().ToString());
            sb.Append("C" + this.ForeignAmountCreditTransEnd.GetValueOrDefault().ToString());

            return sb.ToString();
        }

    }
    public class TrailReportM
    {

        public string ChartOfAccountTypeName { get; set; }
        public string ChartOfAcountType { get; set; }
        public int? ChartOfAcountTypeOrder { get; set; }
        public string ChartOfAcount1 { get; set; }
        public string ChartOfAcount2 { get; set; }
        public string ChartOfAcount3 { get; set; }
        public string ChartOfAcount4 { get; set; }
        public string ChartOfAcount5 { get; set; }
        public string ChartOfAcountName1 { get; set; }
        public string ChartOfAcountName2 { get; set; }
        public string ChartOfAcountName3 { get; set; }
        public string ChartOfAcountName4 { get; set; }
        public string ChartOfAcountName5 { get; set; }
        public string ChartOfAcountName1English { get; set; }
        public string ChartOfAcountName2English { get; set; }
        public string ChartOfAcountName3English { get; set; }
        public string ChartOfAcountName4English { get; set; }
        public string ChartOfAcountName5English { get; set; }
        public string ChartOfAcountCode1 { get; set; }
        public string ChartOfAcountCode2 { get; set; }
        public string ChartOfAcountCode3 { get; set; }
        public string ChartOfAcountCode4 { get; set; }
        public string ChartOfAcountCode5 { get; set; }
        public string ChartOfAccountsTypeEnglish { get; set; }
        public string ChartOfAccountsEnglish { get; set; }

        public string GLAccountName { get; set; }
        public string GLAccountEnglish { get; set; }
        public string GLAccountNumber { get; set; }

        public string GLAccountId { get; set; }
        public string CurrencyId { get; set; }
        public string ChartOfAccountId { get; set; }

        public decimal? LocalOpenBalance { get; set; }
        public decimal? LocalDebit { get; set; }
        public decimal? LocalCredit { get; set; }
        public decimal? LocalCloseBalance { get; set; }



        public decimal? ForeignOpenBalance { get; set; }

        public decimal? ForeignDebit { get; set; }

        public decimal? ForeignCredit { get; set; }

        public decimal? ForeignCloseBalance { get; set; }


        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("COAT:" + this.ChartOfAcountType);
            sb.Append("COA:" + this.ChartOfAcount1 +
        this.ChartOfAcount2 +
        this.ChartOfAcount3 +
        this.ChartOfAcount4 +
        this.ChartOfAcount5);
            if (!String.IsNullOrWhiteSpace(this.GLAccountName + this.GLAccountId))
            {
                sb.Append("ACC:" + this.GLAccountName + "," + this.GLAccountId);
            }


            sb.Append("Cur:" + this.CurrencyId);




            sb.Append("Local:");
            sb.Append("OB:").Append(this.LocalOpenBalance.GetValueOrDefault().ToString());

            sb.Append("D:").Append(this.LocalDebit.GetValueOrDefault().ToString());
            sb.Append("C:").Append(this.LocalCredit.GetValueOrDefault().ToString());

            sb.Append("CB:").Append(this.LocalCloseBalance.GetValueOrDefault().ToString());



            sb.Append("Foreign:");
            sb.Append("OB:").Append(this.ForeignOpenBalance.GetValueOrDefault().ToString());

            sb.Append("D:").Append(this.ForeignDebit.GetValueOrDefault().ToString());
            sb.Append("C:").Append(this.ForeignCredit.GetValueOrDefault().ToString());

            sb.Append("CB:").Append(this.ForeignCloseBalance.GetValueOrDefault().ToString());


            return sb.ToString();
        }
    }
    public class TrailReportM2
    {

        public string ChartOfAccountTypeName { get; set; }
        public string ChartOfAcountType { get; set; }
        public int? ChartOfAcountTypeOrder { get; set; }
        public string ChartOfAcount1 { get; set; }
        public string ChartOfAcount2 { get; set; }
        public string ChartOfAcount3 { get; set; }
        public string ChartOfAcount4 { get; set; }
        public string ChartOfAcount5 { get; set; }
        public string ChartOfAcountName1 { get; set; }
        public string ChartOfAcountName2 { get; set; }
        public string ChartOfAcountName3 { get; set; }
        public string ChartOfAcountName4 { get; set; }
        public string ChartOfAcountName5 { get; set; }
        public string ChartOfAcountName1English { get; set; }
        public string ChartOfAcountName2English { get; set; }
        public string ChartOfAcountName3English { get; set; }
        public string ChartOfAcountName4English { get; set; }
        public string ChartOfAcountName5English { get; set; }
        public string ChartOfAcountCode1 { get; set; }
        public string ChartOfAcountCode2 { get; set; }
        public string ChartOfAcountCode3 { get; set; }
        public string ChartOfAcountCode4 { get; set; }
        public string ChartOfAcountCode5 { get; set; }
        public string ChartOfAccountsTypeEnglish { get; set; }
        public string ChartOfAccountsEnglish { get; set; }

        public string GLAccountName { get; set; }
        public string GLAccountEnglish { get; set; }
        public string GLAccountNumber { get; set; }

        public string GLAccountId { get; set; }
        public string CurrencyId { get; set; }
        public string ChartOfAccountId { get; set; }

        public decimal? LocalOpenBalance { get; set; }
        public decimal? LocalDebit { get; set; }
        public decimal? LocalCredit { get; set; }
        public decimal? LocalCloseBalance { get; set; }



        public decimal? ForeignOpenBalance { get; set; }

        public decimal? ForeignDebit { get; set; }

        public decimal? ForeignCredit { get; set; }

        public decimal? ForeignCloseBalance { get; set; }


        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("COAT:" + this.ChartOfAcountType);
            sb.Append("COA:" + this.ChartOfAcount1 +
        this.ChartOfAcount2 +
        this.ChartOfAcount3 +
        this.ChartOfAcount4 +
        this.ChartOfAcount5);
            if (!String.IsNullOrWhiteSpace(this.GLAccountName + this.GLAccountId))
            {
                sb.Append("ACC:" + this.GLAccountName + "," + this.GLAccountId);
            }


            sb.Append("Cur:" + this.CurrencyId);




            sb.Append("Local:");
            sb.Append("OB:").Append(this.LocalOpenBalance.GetValueOrDefault().ToString());

            sb.Append("D:").Append(this.LocalDebit.GetValueOrDefault().ToString());
            sb.Append("C:").Append(this.LocalCredit.GetValueOrDefault().ToString());

            sb.Append("CB:").Append(this.LocalCloseBalance.GetValueOrDefault().ToString());



            sb.Append("Foreign:");
            sb.Append("OB:").Append(this.ForeignOpenBalance.GetValueOrDefault().ToString());

            sb.Append("D:").Append(this.ForeignDebit.GetValueOrDefault().ToString());
            sb.Append("C:").Append(this.ForeignCredit.GetValueOrDefault().ToString());

            sb.Append("CB:").Append(this.ForeignCloseBalance.GetValueOrDefault().ToString());


            return sb.ToString();
        }
    }
    public class TrailReportParam
    {
        //filter for alll!!!
        public int Tenant { get; set; }

        //filter for Money!!!
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        //filter the GLAccount ?!?!?
        public string Category1 { get; set; }
        public string Category2 { get; set; }
        public string Category3 { get; set; }
        public string Category4 { get; set; }
        public string Category5 { get; set; }

        //filter the GLAccount ?!?!?
        public bool DetailedControlClients { get; set; }
        public bool DetailedControlVendors { get; set; }
        public bool DetailedControlJob { get; set; }
        public bool DetailedControlFile { get; set; }
        public bool Skip { get; set; }
        //filter the GLAccount (if COATypeLevel) + Group by 
        public ReportLevel MyTrailReportLevel { get; set; }

        //Group by 
        public bool CurrenciesDetailed { get; set; }

        /// <summary>
        /// לאחר שיחה הבהרה עם אייל 
        ///דוח מאזן בוחן
        ///1ביררת המחדל -לא יצגו כרטיסים לא פעילים .
        ///2לפי חיתוך\בחירה "הצג כרטיסים לא פעילים" – יצגו !
        //*** כרטיסים לא פעילים (יתרת פתיחה לתקופה 0 וללא תנועות  לטווח הדוח )
        /// </summary>
        public bool Suppress_DoNotShowCardWithoutActivity { get; set; }
        public bool Suppress_ControlAccount { get; set; }

        public bool IsRevenueExpenseReport { get; set; }
        public bool IsTrialBalanceReport { get; set; }

        #region Task 60992: Trial Balance- New filters + multiple choice
        /// <summary>
        /// new filter Chart Of Account type with multiple choice option.
        /// </summary>
        List<string> _GLAccountLevel_ChartOfAccountsTypeCodeList;
        public List<string> ChartOfAccountsTypeCodeList {
            get { 
                _GLAccountLevel_ChartOfAccountsTypeCodeList = _GLAccountLevel_ChartOfAccountsTypeCodeList ?? new List<string>();
                return _GLAccountLevel_ChartOfAccountsTypeCodeList;
            }
            set
            {
                _GLAccountLevel_ChartOfAccountsTypeCodeList = value;
            }
        }
        /// <summary>
        /// change the filter Chart Of Account to  multiple choice option. 
        /// </summary>
        List<string> _GLAccountLevel_ChartOfAccountsIdList;
        public List<string> ChartOfAccountsIdList 
        {
            get {
                _GLAccountLevel_ChartOfAccountsIdList = _GLAccountLevel_ChartOfAccountsIdList ?? new List<string>();
                return _GLAccountLevel_ChartOfAccountsIdList;
            }
            set { _GLAccountLevel_ChartOfAccountsIdList = value; }
        }

        public bool DoNotShowCardWithLocalCloseBalanceEqualZero { get;  set; }
        #endregion

        public string SelectedBalance { get; set; }

        public const string A_OPTION = "A_OPTION";
        public const string B_OPTION = "B_OPTION";
        public const string C_OPTION = "C_OPTION";
    }


    public enum ReportLevel
    {
        ChartofaccountType=1,
        Chartofaccount=2,
        GLAccount=3
    }
}
