using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class NewAccountingAgingDataProvider : BaseDataProvider
    {
        public NewAccountingAgingDataProvider()
        {
            AgingPeriods = new List<NewAgingPeriod>();
        }
        public DateTime? Month { get; set; }
        public string PrintedByUser { get; set; }
        public string TenantCurrencyCode { get; set; }
        public string TenantCurrencySign { get; set; }
        public decimal ReportLocalBalanceTotal { get; set; }
        public bool IsFromGLAccountAgingData { get; set; }

        public List<NewAgingPeriod> AgingPeriods { get; set; }
    }

    public class NewAgingPeriod
    {
      

        public decimal? Rate { get; set; }
        public string AccountEnglishName { get; set; }
        public string AccountLocalName { get; set; }
        public string AccountDisplayNumber { get; set; }
        public string AccountVatNumber { get; set; }

        public string AccountCurrencyCode { get; set; }
        public decimal? InsuredCreditLimit { get; set; }
        public decimal? ExternalTransactionsTotal { get; set; }
        public decimal? FutureChequesTotal { get { return TotalFutureOpenCheques + ExternalTransactionsTotal; } }
        public decimal? Obligo { get { return TotalOpenShipments + BalanceInLocalCurrency+ TotalFutureOpenCheques + TotalPastOpenCheques; } }
         public decimal? CreditUsed { get { return (CreditLimit - Obligo)*-1; } }
         public decimal? TotalLocal { get; set; } 
        public decimal? TotalForeign { get; set; } 
       public decimal? TotalFutureOpenCheques { get; set; }
        public decimal? TotalPastOpenCheques { get; set; }

        public decimal? TotalOpenShipments { get; set; }
        public decimal? BalanceInLocalCurrency { get; set; }

        public string AccountSalesmanName { get; set; }
        public string AccountSalesmanLocalName { get; set; }
         public string AccountCollectorName { get; set; }
        public string AccountCollectorLocalName { get; set; }
        public string ContactPhoneOrEmail { get; set; }
        public string ContactLocalName { get; set; }
        public string ContactEnglishName { get; set; }
        public string MinimumInterestInvoiceBilling { get; set; }
        public string CreditAllotmentPercentage { get; set; }
        public decimal? CreditLimit { get; set; }
         public decimal? TotalToCollect { get; set; }
        public decimal? AccountingBalance { get; set; }
        public decimal? Minus30Days { get; set; }
        public decimal? Minus60Days { get; set; }
        public decimal? Minus90Days { get; set; }
        public decimal? Minus120Days { get; set; }
        public decimal? Minus150Days { get; set; }
        public decimal? Minus180Days { get; set; }
        public decimal? Past { get; set; }
        public decimal? Plus30Days { get; set; }
        public decimal? Plus60Days { get; set; }
        public decimal? Plus90Days { get; set; }
        public decimal? Future { get; set; }

    }

  
}