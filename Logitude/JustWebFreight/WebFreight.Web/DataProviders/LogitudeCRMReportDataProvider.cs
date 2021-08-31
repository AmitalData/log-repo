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
            LogitudeCRMReports = new List<LogitudeCRMReport>();
        }

        public List<LogitudeCRMReport> LogitudeCRMReports { get; set; }

    }


    public class LogitudeCRMReport
    {
        public LogitudeCRMReport()
        {
            Periods = new List<OpportunityPeriod>();
        }

        public string ClientId { get; set; }
        public string TenantNumber { get; set; }
        public string ClientName { get; set; }
        public string Reseller { get; set; }
        public string CountryName { get; set; }

        public string CurrencyCode { get; set; }
        public int? ResellerCommission { get; set; }
        public int? TenantManagementNumberOfUsers { get; set; }
        public decimal? TenantManagementAveragePrice { get; set; }
        public decimal? TenantManagementTotalPrice { get; set; }
        public List<LogitudeCRMReportOpportunity> Opportunities { get; set; }
        public int? HasError { get; set; }
        public decimal? TotalNetBeforeYear { get; set; }
        public List<OpportunityPeriod> Periods { get; set; }
        public decimal? TotalNet { get; set; }

    }

    public class LogitudeCRMReportOpportunity
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
        public List<OpportunityPeriodSummary> OpportunityPeriodSummaries { get; set; }
    }

    public class OpportunityPeriodSummary
    {
        public string IsNewCustomer { get; set; }
        public int? NumberOfUsers { get; set; }
        public decimal? NewIncome { get; set; }

    }


}