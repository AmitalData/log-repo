using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class AccountingAgingDataProvider : BaseDataProvider
    {
        public AccountingAgingDataProvider()
        {
            AgingPeriods = new List<AgingPeriod>();
        }
        public string CustomerFilterValue { get; set; }
        public DateTime? Month { get; set; }
        public string PrintedByUser { get; set; }
        public string TenantCurrencyCode { get; set; }
        public string TenantCurrencySign { get; set; }
        public decimal ReportLocalBalanceTotal { get; set; }
        public bool IsFromGLAccountAgingData { get; set; }

        public List<AgingPeriod> AgingPeriods { get; set; }
    }

    public class AgingPeriod
    {
        public string PeriodName { get; set; }
        public string CreditOrDebit { get; set; } // contains credit/debit labels 
        public decimal? Total { get; set; } = 0; // contains credit/debit total 
        public decimal GrandTotal { get; set; } // used to calculate credit total and debit total from two records
        public int OrderIndex { get; set; }
        public List<AgingPeriodTotal> Totals { get; set; }


        public string AccountEnglishName { get; set; }
        public string AccountLocalName { get; set; }
        public string AccountName { get; set; }
        public string AccountDisplayNumber { get; set; }
        public string AccountCurrencyCode { get; set; }
        public string CurrencyCode { get; set; }
        public string ChartOfAccountLocalName { get; set; }

        public string CustomerVatNumber { get; set; }
        public string CustomerPaymentTerm { get; set; }
        public decimal CustomerCreditLimit { get; set; }
        public double? InsuredCreditLimit { get; set; }
        public decimal GLAccountStandardInterestRate { get; set; }

        public string AccountSalesmanName { get; set; }
        public string AccountSalesmanLocalName { get; set; }

        public string AccountCollectorName { get; set; }
        public string AccountCollectorLocalName { get; set; }
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
    }

    public class AgingPeriodTotal
    {
        public decimal TotalCredit { get; set; }
        public decimal TotalDebit { get; set; }
    }
}