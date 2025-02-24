using Logitude.Accounting.BL.CoreBL.Mapping;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Logitude.Server.Tools.Helpers;
using System.Diagnostics;
using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using System.Data.Entity.SqlServer;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.Helpers;
using Logitude.Server.Tools;
using Logitude.Accounting.BL.CoreBL.Reports.Aging;
using Logitude.Server.Tools.Utils;
using Logitude.Server.Tools.Helpers;
using System.Reflection;
using System.Data.Entity;

namespace Logitude.Accounting.BL.CoreBL.Reports
{
    public class NewAgingReportService
    {
        private NewAgingReportParam _Param;
        private IAccountingContext _AccountingContext;
        IQueryable<string> /*_MainAccountIdList_withoutAccountThatHave2Aggregate_1RelatedAccCurrencies*/
            _MainAccountIdList_ToFetchThenAggragrate = null;
        //private IQueryable<Logitude.Accounting.Def.EntityPMs.GLAccountPM> _GLAccounts;
        private IQueryable<string> _AccountListId_PleaseTake_ForeignAmount;
        private IQueryable<GLAccountCurrency> _AccountListRelatedCurrenciesAccount_UseToAggregateAsLocalAmount;//Due Param IsAggregate - from GLAccountCurrency get TotalByMonth/Reconcile But Imitate As Parent
        private string _AccountingCurrencyId;
        private IQueryable<GLAccount> _qAllAccAging4AccountTypeCode_CustomerOrVendor;
        private GLAccountQueryService _myGLAccountQueryService;
        private GLAccountRepository _myGLAccountRepository;
        private List<GLAccount> _GLAccountChildren_UseToAggregateAsLocalAmount;
        private bool _TESTIT;
        private List<string> _AccountListRelatedCurrenciesAccount_List2Discard;


        public NewAgingReportService(NewAgingReportParam param)
        {
            _Param = param;
        }

     

        public string RunReport()
        {
            CheckeParams();
           
        }

       



        private void CheckeParams()
        {

            if (_Param.NumberOfmonthsbackwards < 1)
            {
                throw new Exception("NumberOfmonthsbackwards<1");
            }

            if (this._Param.Aging4AccountTypeCode == NewAgingReportParam.Aging4AccountTypeCodeEnum.ControlAccountOnly1)
            {
                if (String.IsNullOrWhiteSpace(_Param.VendorCustomerId))
                {
                    throw new Exception("While Filtering by ControlAccount ,Please set Control Account Id in VendorCustomerId");
                }
                if (_Param.AgingMethod == AgingReportParam.MethodEnum.ReconcileOpenBalanceMethod.ToString())
                {
                    throw new Exception("While Filtering by ControlAccount ,the Reconcile Open Balance Method is providen");
                }
            }
            if (!String.IsNullOrWhiteSpace(_Param.SalesmanId))
            {
                ///throw new Exception("Sorry ,Salesman ?!?!?!  What 2do==" + _Param.SalesmanId);
            }
            if (this._Param.Aging4AccountTypeCode == NewAgingReportParam.Aging4AccountTypeCodeEnum.ControlAccountOnly1)
            {

                var fullPM = FullAccountingSettingQueryService.Get(_Param.Tenant);
                var list = new List<string>()
                {

                        fullPM.AirExportJobControlAccountId ,
                        fullPM.AirImportJobControlAccountId ,
                        fullPM.CustomerControlAccountId ,
                        fullPM.OceanExportJobControlAccountId ,
                        fullPM.OceanImportJobControlAccountId,
                        fullPM.VendorControlAccountId,
                        fullPM.FileControlAccountId,

                };
                if (!list.Contains(_Param.VendorCustomerId))
                {
                    throw new Exception("Filtering by ControlAccount (_Param.VendorCustomerId= " + _Param.VendorCustomerId + " ) ,Please set Control Account Id in VendorCustomerId");
                }
                if (!_Param.SuppressFromGLAccountAgingData)
                {
                    if (
                    !(_Param.AgingMethod == AgingReportParam.MethodEnum.ReconcileOpenBalanceMethod.ToString() &&
                    _Param.GroupByDate == AgingReportParam.DateEnum.DueDate))

                    {
                        Debug.WriteLine("no no NO only if ReconcileOpenBalanceMethod + DueDate !!!");
                        _Param.SuppressFromGLAccountAgingData = true;
                    }
                    if (
                        _Param.AgingForDate.Date.Year != DateTime.Now.Date.Year ||
                        _Param.AgingForDate.Date.Month != DateTime.Now.Date.Month
                        )
                    {
                        Debug.WriteLine("no no NO only if 4 current month  !!!");
                        _Param.SuppressFromGLAccountAgingData = true;

                    }

                    if (_Param.NumberOfmonthsbackwards > 6)
                    {
                        Debug.WriteLine("no no NO only if NumberOfmonthsbackwards<6!!!");
                        _Param.SuppressFromGLAccountAgingData = true;

                    }
                }
                //_Param
            }
            //if (string.IsNullOrWhiteSpace(_Param.VendorCustomerId))
            //{
            //    throw new Exception("string.IsNullOrWhiteSpace(_AgingReportParam.VendorCustomerId) ");
            //}
        }


        public List<NewPeriodM> MyPeriodList { get; set; }
        public List<NewPeriodMExtended> MyPeriodExtendedList { get; set; }
    }
    public class NewPeriodM
    {
        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3}", this.AccountId, this.CurrencyId, PeriodName, this.Total);
        }



        public DateTime OrderDate { get; set; }
        public bool OrderDateB4 { get; set; }
        public bool OrderAfterOpenRecordDueDate { get; set; }

        public string AccountId { get; set; }

        public string CurrencyId { get; set; }

        public decimal Total { get; set; }

        public decimal OpenCredit { get; set; }
        public decimal OpenDebit { get; set; }

        public string PeriodName
        {
            get
            {
                var month = OrderDate.Month.ToString() + "/" + OrderDate.Year.ToString();
                if (OrderDateB4)
                {
                    return "b4 " + month;

                }
                else if (OrderAfterOpenRecordDueDate)
                {
                    return "FutureAmount";// " >= " + month;

                }
                else
                {
                    return month;
                }

            }
        }


        public string AccountAndCurr
        {
            get
            {
                return this.AccountId + " " + this.CurrencyId;
            }
        }

        public string CalcTotalCurrencyId
        {
            get
            {
                //CalcTotalCurrencyId = "0 NIS"
                return Total + " " + CurrencyId;
            }
        }

        public string SplitAccountId { get; set; }
        public int TotalOpenTransactions { get; set; }
    }
    public class NewPeriodMExtended: PeriodM
    {
        public bool? IsMultiCurrency { get; set; }
        public string AccountEnglishName { get; set; }
        public string AccountLocalName { get; set; }


        public string AccountDisplayNumber { get; set; }
        public string ChartOfAccountLocalName { get; set; }
        public string AccountInternalNumber { get; set; }
        public string AccountCurrencyCode { get; set; }
        //accountCardlist.Payment Term: //PaymentTermName = card.PaymentTerm == null ? null : card.PaymentTerm.EnglishName,
        public string AccountTermName { get; set; }
        public string AccountTermLocalName { get; set; }
        public string CurrencyCode { get; set; }
        public string CustomerVatNumber { get; set; }

        public decimal GLAccountStandardInterestRate { get; set; }

        /*
        var percentage = 0;
        if (this.GLAccountMoreData && this.accountCardlist)
        {
            percentage =
                (this.GLAccountMoreData.BalanceInLocalCurrency ? this.GLAccountMoreData.BalanceInLocalCurrency : 0)
            +   (this.GLAccountMoreData.TotalOpenChequesInLocalCur ? this.GLAccountMoreData.TotalOpenChequesInLocalCur : 0)
            +   (this.GLAccountMoreData.TotFutureOpenChequesInLocalCur ? this.GLAccountMoreData.TotFutureOpenChequesInLocalCur : 0)
            + (this.accountCardlist.OpenShipments?this.accountCardlist.OpenShipments:0 );

            this.accountTotal = percentage;

            if(this.accountCardlist.CreditLimitAmount && this.accountCardlist.CreditLimitAmount != 0)
                percentage = percentage / (this.accountCardlist.CreditLimitAmount ? this.accountCardlist.CreditLimitAmount : 0);
            else
                percentage = 0;

            this.creditStatusAmount = (this.accountCardlist.CreditLimitAmount ? this.accountCardlist.CreditLimitAmount : 0) - this.accountTotal;

        }
 
         */


        public string AccountPhone { get; set; }


        //ccountCardlist?accountCardlist.CreditLimitAmount:0>>entityList.CreditLimitAmount = entityPOCO.Customer.CreditLimitAmount;
        public double? CreditLimitAmount { get; set; }
        public double? InsuredCreditLimit { get; set; }
        public decimal? InterestCreditLimit { get; set; }

        //this.creditStatusAmount = (this.accountCardlist.CreditLimitAmount ? this.accountCardlist.CreditLimitAmount : 0) - this.accountTotal;
        public decimal? CreditStatusAmount { get; set; }
        //this.accountCardlist.OpenShipments? this.accountCardlist.OpenShipments:0 
        public decimal? TotalOpenShipments { get; set; }
        //+   (this.GLAccountMoreData.TotFutureOpenChequesInLocalCur ? this.GLAccountMoreData.TotFutureOpenChequesInLocalCur : 0)
        public decimal? TotalFutureOpenCheques { get; set; }
        //+   (this.GLAccountMoreData.TotalOpenChequesInLocalCur ? this.GLAccountMoreData.TotalOpenChequesInLocalCur : 0)
        public decimal? TotalOpenCheques { get; set; }
        public double? CreditStatusAmount_AsIs { get; set; }
        public decimal? BalanceInLocalCurrency { get;  set; }
        public decimal? LocalBalanceInDue { get;  set; }
        public string SplitAccountId { get;  set; }
        public decimal BalanceInLocalAccountingDate { get; set; }
        public decimal BalanceInLocalDueDate { get; set; }
        public decimal BalanceInForeignAccountingDate { get; set; }
        public decimal BalanceInForeignDueDate { get; set; }
        public string AccountSalesmanName { get; set; }
        public string AccountSalesmanLocalName { get; set; }

        public string AccountCollectorName { get; set; }
        public string AccountCollectorLocalName { get; set; }
        public string AccountContactName { get; set; }
        public string AccountContactEmail { get; set; }
        public string AccountContactPhone { get; set; }
        public string Category1Name { get; set; }
        public string Category2Name { get; set; }
        public string Category3Name { get; set; }
        public string Category4Name { get; set; }
        public string Category5Name { get; set; }
        public string Category6Name { get; set; }
        public string Category1LocalName { get; set; }
        public string Category2LocalName { get; set; }
        public string Category3LocalName { get; set; }
        public string Category4LocalName { get; set; }
        public string Category5LocalName { get; set; }
        public string Category6LocalName { get; set; }

        public string ChartOfAccountsLocalName { get; set; }
        public string ChartOfAccountsEnglishName { get; set; }
        public string ChartOfAccountsTypeEnglishName { get; set; }
        public string ChartOfAccountsTypeLocalName { get; set; }
        public int? ChartOfAccountSecurityLevel { get; set; }

    }

    public class NewAgingReportParam
    {
        public NewAgingReportParam()
        {
            _explained_AggregateByGLAccountCurrencies = @"
EYAL:
גיול חובות לכרטיס רב מטבעי מבוצע על מטבע מקומי בלבד  == LOCALAMOUNT
גיול חובות לכרטיס חד מטבעי מבוצע על FOREIGNAMOUNT  + CURRENCY

FALSE=דיפולט של המערכת כל כרטיס מגוייל בפני עצמו – לכל כרטיס שורה אחת  => 
TRUE= כאשר מבקשים עם ריכוז לפי כרטיס אב (פיצול כרטיסים מטבעים ) – יהיה שורה אחת רק עבור כרטיס אב הסוכמת את האב והבנים לפי LOCALAMOUNT  (לא מציגים את כרטיסי הבינים  המטבעים)
";
        }
        private string _explained_AggregateByGLAccountCurrencies;

        public enum MethodEnum
        {
            TotalByMonthMethod,
            TotalByMonthFIFOMethod,
            ReconcileOpenBalanceMethod,
        }
        public enum DateEnum
        {
            DueDate,
            AccountingDate,
        }

        public enum Aging4AccountTypeCodeEnum : int
        {
            ControlAccountOnly1 = 1,
            Customer2 = 2,
            Vendor3,

        }


        public int Tenant { get; set; }

        public DateTime AgingForDate { get; set; }
        public //int
            uint NumberOfmonthsbackwards
        { get; set; }



        public string VendorCustomerId { get; set; }
        public Aging4AccountTypeCodeEnum Aging4AccountTypeCode { get; set; }



        public string Category1Id { get; set; }
        public string Category2Id { get; set; }
        public string Category3Id { get; set; }
        public string Category4Id { get; set; }
        public string Category5Id { get; set; }

        public string CollectorId { get; set; }
        public string SalesmanId { get; set; }

        public string ChartOfAccountsTypeCode { get; set; }

        public string ChartOfAccountsId { get; set; }

        public string CurrencyOriginalLocalValue { get; set; }

        


        //public string ChartOfAccountIdV1NotInUse { get; set; }
        //public bool? CurrenciesDetailedV1NotInUse { get; set; }



        //public bool WithReconciledTransactions { get; set; }


        ///-דיפולט של המערכת כול כרטיס מגויל בפני עצמו – לכול כרטיס שורה אחת(ולא מוסיפים כרטיסי בנים מטבעיים) DetailedByGLAccountCurrencies=false  -- 
        ///DetailedByGLAccountCurrencies= True-כאשר מבקשים עם ריכוז לפי כרטיס אב(פיצול כרטיסים מטבעים ) – יהיה שורה אחת רק עבור כרטיס אב הסוכמת את האב והבנים לפי LOCALAMOUNT(לא מציגים את כרטיסי הבינים המטבעים)
        public bool /*IncludeRelatedCurrenciesAccount*/
        AggregateByGLAccountCurrencies
        { get; set; }


        public bool AggregateByGLAccountChildren { get; set; }


        public string AgingMethod_Options { get; set; }


        public string AgingMethod { get; set; }
        public DateEnum GroupByDate { get; set; }

        public string GroupByDate_Options { get; set; }



        public string Aging4AccountTypeCode_Options { get; set; }

        //public string Explained_AggregateByGLAccountCurrencies => explained_AggregateByGLAccountCurrencies;


        public string Explained_AggregateByGLAccountCurrencies
        {
            get { return _explained_AggregateByGLAccountCurrencies; }
            set { _explained_AggregateByGLAccountCurrencies = value; }
        }

        public bool BuildPivot { get; set; }

        public bool SuppressFromGLAccountAgingData { get; set; }

        public bool FroceFromGLAccountAgingData { get; set; }
    }


    class NewAccountCurrency
    {
        public string AccountId { get; set; }
        public string CurrencyId { get; set; }

    }
}
