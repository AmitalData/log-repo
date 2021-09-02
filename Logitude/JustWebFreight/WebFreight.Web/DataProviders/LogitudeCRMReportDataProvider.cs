using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class LogitudeCRMReportDataProvider : BaseDataProvider
    {
        public LogitudeCRMReportDataProvider()
        {
            Customers = new List<CustomerItem>();
        }

        public List<CustomerItem> Customers { get; set; }
        public bool ShowNet { get; set; }
        public int ResellerCommission { get; set; }
        public int NumberOfUsers { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal TotalPrice { get; set; }
        public int OpportunitiesNumberOfUsers { get; set; }
        public decimal OpportunitiesTotal { get; set; }
        public decimal OpportunitiesTotalNet { get; set; }
        public decimal TotalNetBeforeYear { get; set; }
        public decimal TotalNet { get; set; }
    }


    public class CustomerItem
    {
        public CustomerItem()
        {
            OpportunityPeriods = new List<OpportunityPeriod>();
        }

        public string ClientId { get; set; }
        public string TenantNumber { get; set; }
        public string ClientName { get; set; }
        public string Reseller { get; set; }
        public string CountryName { get; set; }
        public string CurrencyCode { get; set; }
        public int? ResellerCommission { get; set; }
        public int? NumberOfUsers { get; set; }
        public decimal? AveragePrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public List<OpportunityItem> Opportunities { get; set; }
        public int? HasError { get; set; }
        public decimal? TotalNetBeforeYear { get; set; }
        public List<OpportunityPeriod> OpportunityPeriods { get; set; }
        public decimal? TotalNet { get; set; }

    }

    public class OpportunityItem
    {
        public int? NumberOfUsers { get; set; }
        public decimal? Total { get; set; }
        public decimal? TotalNet { get; set; }
    }

    public class OpportunityPeriod
    {
        public OpportunityPeriod()
        {
            OpportunityPeriodSummaries = new List<OpportunityPeriodSummary>();
        }

        public string PeriodName { get; set; }
        public decimal Total { get; set; }
        public List<OpportunityPeriodSummary> OpportunityPeriodSummaries { get; set; }
    }

    public class OpportunityPeriodSummary
    {
        public string IsNewCustomer { get; set; }
        public int? NumberOfUsers { get; set; }
        public decimal? NewIncome { get; set; }

    }

}