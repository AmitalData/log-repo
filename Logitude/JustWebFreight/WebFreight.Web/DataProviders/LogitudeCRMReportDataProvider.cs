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
        public int NumberOfUsers { get; set; }
        public decimal TotalPrice { get; set; }
        public int OpportunitiesNumberOfUsers { get; set; }
        public decimal OpportunitiesTotal { get; set; }
        public decimal OpportunitiesTotalNet { get; set; }
        public decimal CurrentTotal { get; set; }
        public decimal TotalNetBeforeYear { get; set; }

        public int FirstMonthNumberOfUsersTotal { get; set; }
        public decimal FirstMonthNewIncomeTotal { get; set; }

        public int SecondMonthNumberOfUsersTotal { get; set; }
        public decimal SecondMonthNewIncomeTotal { get; set; }

        public int ThirdMonthNumberOfUsersTotal { get; set; }
        public decimal ThirdMonthNewIncomeTotal { get; set; }

        public int FourthMonthNumberOfUsersTotal { get; set; }
        public decimal FourthMonthNewIncomeTotal { get; set; }

        public int FifthMonthNumberOfUsersTotal { get; set; }
        public decimal FifthMonthNewIncomeTotal { get; set; }

        public int SixthMonthNumberOfUsersTotal { get; set; }
        public decimal SixthMonthNewIncomeTotal { get; set; }

        public int SeventhMonthNumberOfUsersTotal { get; set; }
        public decimal SeventhMonthNewIncomeTotal { get; set; }

        public int EighthMonthNumberOfUsersTotal { get; set; }
        public decimal EighthMonthNewIncomeTotal { get; set; }

        public int NinthMonthNumberOfUsersTotal { get; set; }
        public decimal NinthMonthNewIncomeTotal { get; set; }

        public int TenthMonthNumberOfUsersTotal { get; set; }
        public decimal TenthMonthNewIncomeTotal { get; set; }

        public int EleventhMonthNumberOfUsersTotal { get; set; }
        public decimal EleventhMonthNewIncomeTotal { get; set; }

        public int TwelfthMonthNumberOfUsersTotal { get; set; }
        public decimal TwelfthMonthNewIncomeTotal { get; set; }
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
        public decimal? CurrentTotal { get; set; }
        public List<OpportunityPeriod> OpportunityPeriods { get; set; }
        public decimal? TotalNetBeforeYear { get; set; }
        public bool InActive { get; set; }
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