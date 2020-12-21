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
        public string CustomerName { get; set; }
        public string CustomerLocalName { get; set; }
        public string CustomerDisplayNumber { get; set; }
        public string CustomerPhone{ get; set; }
        public string CustomerPaymentTerm { get; set; }
        public decimal CreditStatus { get; set; }
        public decimal TotalOpenShipments { get; set; }
        public decimal TotalFutureOpenCheques { get; set; }
        public decimal TotalOpenCheques { get; set; }
        public decimal AccountingBalance { get; set; }
        public decimal CreditLimit { get; set; }
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