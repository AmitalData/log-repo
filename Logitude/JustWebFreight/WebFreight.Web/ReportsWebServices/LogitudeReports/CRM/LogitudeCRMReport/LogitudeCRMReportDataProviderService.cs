using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using WebFreight.Web.DataProviders;
using Logitude.CRM.BL.DataContracts;
using Newtonsoft.Json;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.CRM.LogitudeCRMReport
{
    public class LogitudeCRMReportDataProviderService
    {
        private readonly int tenant;
        private readonly LogitudeCRMReportFilter logitudeCRMReportFilter;

        public LogitudeCRMReportDataProviderService(byte[] xmlFilters, int tenant)
        {
            this.tenant = tenant;
            logitudeCRMReportFilter = new LogitudeCRMReportFilterService(xmlFilters, tenant).Build();
        }

        public byte[] Load()
        {
            LogitudeCRMReportDataProvider logitudeCRMReportDataProvider = BuildDataProvider();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(LogitudeCRMReportDataProvider));
            MemoryStream memoryStream = new MemoryStream();
            xmlSerializer.Serialize(memoryStream, logitudeCRMReportDataProvider);
            memoryStream.Seek(0, SeekOrigin.Begin);
            byte[] bytearray = memoryStream.ToArray();
            File.WriteAllText(@"D:\path.json", JsonConvert.SerializeObject(logitudeCRMReportDataProvider));
            return bytearray;
        }

        private LogitudeCRMReportDataProvider BuildDataProvider()
        {
            LogitudeCRMReportDataProvider logitudeCRMReportDataProvider = new LogitudeCRMReportDataProvider();
            List<OpportunityDetails> opportunities = new LogitudeCRMOpperunityService(tenant, logitudeCRMReportFilter).Build();

            logitudeCRMReportDataProvider.ShowNet = logitudeCRMReportFilter.ShowNet;
            logitudeCRMReportDataProvider.Customers = opportunities.GroupBy(c => c.ClientId).Select(a => MappOpportunityToCustomer(a, opportunities)).ToList();

            SetSums(logitudeCRMReportDataProvider);
            return logitudeCRMReportDataProvider;
        }

        private static void SetSums(LogitudeCRMReportDataProvider logitudeCRMReportDataProvider)
        {
            logitudeCRMReportDataProvider.ResellerCommission = logitudeCRMReportDataProvider.Customers.Where(a => a.ResellerCommission != null).Count() != 0 ? logitudeCRMReportDataProvider.Customers.Sum(a => a.ResellerCommission ?? 0) / (logitudeCRMReportDataProvider.Customers.Where(a => a.ResellerCommission != null).Count()) : 0;
            logitudeCRMReportDataProvider.NumberOfUsers = logitudeCRMReportDataProvider.Customers.Sum(a => a.NumberOfUsers ?? 0);
            logitudeCRMReportDataProvider.AveragePrice = logitudeCRMReportDataProvider.Customers.Sum(a => a.AveragePrice ?? 0);
            logitudeCRMReportDataProvider.TotalPrice = logitudeCRMReportDataProvider.Customers.Sum(a => a.TotalPrice ?? 0);

            logitudeCRMReportDataProvider.OpportunitiesNumberOfUsers = logitudeCRMReportDataProvider.Customers.Sum(a => a.Opportunities.Sum(b => b.NumberOfUsers ?? 0));
            logitudeCRMReportDataProvider.OpportunitiesTotal = logitudeCRMReportDataProvider.Customers.Sum(a => a.Opportunities.Sum(b => b.Total ?? 0));
            logitudeCRMReportDataProvider.OpportunitiesTotalNet = logitudeCRMReportDataProvider.Customers.Sum(a => a.Opportunities.Sum(b => b.TotalNet ?? 0));

            logitudeCRMReportDataProvider.TotalNetBeforeYear = logitudeCRMReportDataProvider.Customers.Sum(a => a.TotalNetBeforeYear ?? 0);
            logitudeCRMReportDataProvider.TotalNet = logitudeCRMReportDataProvider.Customers.Sum(a => a.TotalNet ?? 0);
        }

        private CustomerItem MappOpportunityToCustomer(IGrouping<string, OpportunityDetails> a, List<OpportunityDetails> opportunityDetails)
        {
            return new CustomerItem
            {
                ClientId = a.First().ClientId,
                TenantNumber = a.First().TenantNumber,
                ClientName = a.First().ClientName,
                Reseller = a.First().Reseller,
                CountryName = a.First().CountryName,
                CurrencyCode = a.First().CurrencyCode,
                ResellerCommission = a.First().ResellerCommission,
                NumberOfUsers = a.First().TenantManagementNumberOfUsers,
                AveragePrice = ToUsd(a.First().CurrencyCode, a.First().TenantManagementAveragePrice),
                TotalPrice = ToUsd(a.First().CurrencyCode, a.First().TenantManagementTotalPrice),
                Opportunities = GetOpportunityItems(opportunityDetails.Where(b => b.ClientId == a.First().ClientId), a.First().CurrencyCode),
                HasError = HasError(opportunityDetails.Where(b => b.ClientId == a.First().ClientId)),
                TotalNetBeforeYear = GetTotalNetBeforeYear(opportunityDetails.Where(b => b.ClientId == a.First().ClientId), a.First().CurrencyCode),
                OpportunityPeriods = GetPeriods(opportunityDetails.Where(b => b.ClientId == a.First().ClientId && b.CreateDate.Year == DateTime.Now.Year), a.First().CurrencyCode),
                TotalNet = ToUsd(a.First().CurrencyCode, opportunityDetails.Where(b => b.ClientId == a.First().ClientId).Sum(b => b.Total))
            };
        }

        private decimal? GetTotalNetBeforeYear(IEnumerable<OpportunityDetails> opportunityDetails, string currencyCode)
        {
            return ToUsd(currencyCode, opportunityDetails.Where(a => a.CreateDate.Year < DateTime.Now.Year).Sum(a => a.Total));
        }

        private List<OpportunityItem> GetOpportunityItems(IEnumerable<OpportunityDetails> opportunityDetails, string currencyCode)
        {
            return opportunityDetails.Select(a => new OpportunityItem
            {
                NumberOfUsers = a.NumberOfUsers,
                Total = ToUsd(currencyCode, a.Total),
                TotalNet = ToUsd(currencyCode, a.TotalNet),

            }).ToList();
        }

        private List<OpportunityPeriod> GetPeriods(IEnumerable<OpportunityDetails> opportunityDetails, string currencyCode)
        {
            List<OpportunityPeriod> periods = new List<OpportunityPeriod>();
            for (int i = 1; i <= 12; i++)
            {
                OpportunityPeriod opportunityPeriod = new OpportunityPeriod();
                opportunityPeriod.PeriodName = i.ToString();
                opportunityPeriod.OpportunityPeriodSummaries = GetOpportunityPeriodSummaries(opportunityDetails.Where(a => a.CreateDate.Month == i), currencyCode);
                opportunityPeriod.Total = opportunityPeriod.OpportunityPeriodSummaries?.Sum(a => a.NewIncome ?? 0) ?? 0;
                periods.Add(opportunityPeriod);
            }

            return periods;
        }

        private List<OpportunityPeriodSummary> GetOpportunityPeriodSummaries(IEnumerable<OpportunityDetails> opportunityDetails, string currencyCode)
        {
            if (opportunityDetails.Count() == 0)
                return null;

            return opportunityDetails.Select(a => new OpportunityPeriodSummary
            {
                NumberOfUsers = a.NumberOfUsers,
                IsNewCustomer = a.IsNewCustomer,
                NewIncome = ToUsd(currencyCode, a.Total),

            }).ToList();
        }

        private int? HasError(IEnumerable<OpportunityDetails> opportunityDetails)
        {
            if (opportunityDetails.Where(a => a.OpportunityTypeCode == "N").Count() > 1 || opportunityDetails.Any(a => a.IsCancelled))
                return 0;

            return null;
        }

        private decimal? ToUsd(string currencyCode, decimal? price)
        {
            if (currencyCode == "USD" || currencyCode == null)
                return price;

            if (price == null)
                return null;

            if (price == 0)
                return 0;

            return price * logitudeCRMReportFilter.ExchangeRate;
        }
    }

}

