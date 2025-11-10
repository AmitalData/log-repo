using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class CustomerStatusDataProvider : BaseDataProvider
    {
        public CustomerStatusDataProvider()
        {
        }
        public string PrintedByUser { get; set; }

        public List<CustomerStatus> CustomersStatuses { get; set; } = new List<CustomerStatus>();
    }

    public class CustomerStatus
    {
        public CustomerStatus()
        {
            Periods = new List<StatusPeriod>();
        }
        public int Tenant { get; set; }

        public string CustomerName { get; set; }
        public string CustomerLocalName { get; set; }
        public string CustomerDisplayNumber { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerPaymentTerm { get; set; }
        public string CustomerLocalPaymentTerm { get; set; }
        public decimal CreditStatus { get; set; }
        public decimal TotalOpenShipments { get; set; }
        public decimal TotalFutureOpenCheques { get; set; }
        public decimal TotalOpenCheques { get; set; }
        public decimal AccountingBalance { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal InsuredCreditLimit { get; set; }

        public string AccountSalesmanName { get; set; }
        public string AccountSalesmanLocalName { get; set; }
        public string AccountCollectorName { get; set; }
        public string AccountCollectorLocalName { get; set; }
		public string CustomerVatNumber { get; set; }
		public string ChartOfAccountLocalName { get; set; }
        public string CurrencyCode { get; set; }
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

        public decimal ExternalTransactionsTotal { get; set; }

        public decimal TotalToCollect { get { return AccountingBalance + TotalOpenShipments; } }
        public decimal FutureChequesTotal { get { return TotalFutureOpenCheques + ExternalTransactionsTotal; } }
        public decimal Obligo { get { return TotalToCollect + FutureChequesTotal + (FeatureToggleHelper.HasFeatureToggle("CTPC", Tenant) ? (TotalOpenCheques) : 0); } }
        public decimal CreditUsed { get { return CreditLimit - Obligo; } }

        public decimal TotalLocal { get; set; } = 0;
        public decimal TotalForeign { get; set; } = 0;
		public decimal SumTotalCredit { get; set; } = 0;

		public int IsSplitAccount { get; set; }
        public List<StatusPeriod> Periods { get; set; } = new List<StatusPeriod>();


    }

    public class StatusPeriod
    {
        public string PeriodName { get; set; }
        public decimal PeriodTotal { get; set; }
        public List<PeriodCurrencySummary> PeriodCurrenciesSummaries { get; set; } = new List<PeriodCurrencySummary>();
    }

    public class PeriodCurrencySummary
    {
        public string CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public decimal TotalDebit { get; set; }
        public decimal TotalCredit { get; set; }
    }
}