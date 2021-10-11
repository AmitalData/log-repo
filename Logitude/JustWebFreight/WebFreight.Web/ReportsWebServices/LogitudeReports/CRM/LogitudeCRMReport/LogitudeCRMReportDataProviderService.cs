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
            //File.WriteAllText(@"D:\path.json", JsonConvert.SerializeObject(logitudeCRMReportDataProvider));
            return bytearray;
        }

        private LogitudeCRMReportDataProvider BuildDataProvider()
        {

            List<OpportunityCRMDetails> opportunities = new LogitudeCRMOpperunityService(tenant, logitudeCRMReportFilter).Build();

            LogitudeCRMReportDataProvider logitudeCRMReportDataProvider = new LogitudeCRMReportDataProvider();
            logitudeCRMReportDataProvider.Customers = opportunities.GroupBy(c => c.ClientId).Select(a => MappOpportunityToCustomer(a)).ToList();

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

            logitudeCRMReportDataProvider.CurrentTotal = logitudeCRMReportDataProvider.Customers.Sum(a => a.CurrentTotal ?? 0);
            logitudeCRMReportDataProvider.TotalNetBeforeYear = logitudeCRMReportDataProvider.Customers.Sum(a => a.TotalNetBeforeYear ?? 0);
        }

        private CustomerItem MappOpportunityToCustomer(IGrouping<string, OpportunityCRMDetails> opportunityCRMDetail)
        {
            return new CustomerItem
            {
                ClientId = opportunityCRMDetail.First().ClientId,
                TenantNumber = opportunityCRMDetail.First().TenantNumber,
                ClientName = opportunityCRMDetail.First().ClientName,
                Reseller = opportunityCRMDetail.First().Reseller,
                CountryName = opportunityCRMDetail.First().CountryName,
                CurrencyCode = opportunityCRMDetail.First().CurrencyCode,
                ResellerCommission = opportunityCRMDetail.First().ResellerCommission,
                NumberOfUsers = opportunityCRMDetail.First().TenantManagementNumberOfUsers,
                AveragePrice = ConvertToUsd(opportunityCRMDetail.First().CurrencyCode, opportunityCRMDetail.First().TenantManagementAveragePrice),
                TotalPrice = ConvertToUsd(opportunityCRMDetail.First().CurrencyCode, opportunityCRMDetail.First().TenantManagementTotalPrice),
                Opportunities = GetOpportunityItems(opportunityCRMDetail.ToList(), opportunityCRMDetail.First().CurrencyCode),
                HasError = HasError(opportunityCRMDetail.ToList()),
                CurrentTotal = ConvertToUsd(opportunityCRMDetail.First().CurrencyCode, opportunityCRMDetail.ToList().Sum(b => b.Total)),
                OpportunityPeriods = GetPeriods(opportunityCRMDetail.ToList(), opportunityCRMDetail.First().CurrencyCode),
                TotalNetBeforeYear = ConvertToUsd(opportunityCRMDetail.First().CurrencyCode, GetTotalNetBeforeYear(opportunityCRMDetail.ToList())),
                InActive = opportunityCRMDetail.First().InActive,
            };
        }

        private decimal? GetTotalNetBeforeYear(IEnumerable<OpportunityCRMDetails> opportunityDetails)
        {
            return opportunityDetails.Where(a => a.ActualClosingDate.Year < DateTime.Now.Year).Sum(a => a.Total);
        }

        private List<OpportunityItem> GetOpportunityItems(IEnumerable<OpportunityCRMDetails> opportunityDetails, string currencyCode)
        {
            return opportunityDetails
                .GroupBy(a => 1)
                .Select(a => new OpportunityItem
                {
                    NumberOfUsers = a.Sum(b => b.NumberOfUsers),
                    Total = ConvertToUsd(currencyCode, a.Sum(b => b.Total)),
                    TotalNet = ConvertToUsd(currencyCode, a.Sum(b => b.TotalNet)),

                }).ToList();
        }

        private List<OpportunityPeriod> GetPeriods(IEnumerable<OpportunityCRMDetails> opportunityDetails, string currencyCode)
        {
            List<OpportunityPeriod> periods = new List<OpportunityPeriod>();
            for (int i = 1; i <= 12; i++)
            {
                OpportunityPeriod opportunityPeriod = new OpportunityPeriod();
                opportunityPeriod.PeriodName = i.ToString();
                opportunityPeriod.OpportunityPeriodSummaries = GetOpportunityPeriodSummaries(opportunityDetails, i, currencyCode);
                opportunityPeriod.Total = ConvertToUsd(currencyCode, opportunityPeriod.OpportunityPeriodSummaries?.Sum(a => a.NewIncome ?? 0) ?? 0).Value;
                periods.Add(opportunityPeriod);
            }

            return periods;
        }

        private List<OpportunityPeriodSummary> GetOpportunityPeriodSummaries(IEnumerable<OpportunityCRMDetails> opportunityDetails, int mounth, string currencyCode)
        {
            return opportunityDetails
                .Where(b => b.ActualClosingDate.Year == DateTime.Now.Year && b.ActualClosingDate.Month == mounth)
                .GroupBy(a => 1)
                .Select(a => new OpportunityPeriodSummary
                {
                    NumberOfUsers = a.Sum(b => b.NumberOfUsers),
                    IsNewCustomer = a.Where(b => b.IsNewCustomer != null).OrderByDescending(b => b.ActualClosingDate).FirstOrDefault()?.IsNewCustomer,
                    NewIncome = ConvertToUsd(currencyCode, a.Sum(b => b.Total)),
                }).ToList();
        }

        private int? HasError(IEnumerable<OpportunityCRMDetails> opportunityDetails)
        {
            if (opportunityDetails.Where(a => a.OpportunityTypeCode == "N").Count() > 1 || opportunityDetails.Any(a => a.IsCancelled))
                return 0;

            return null;
        }

        private decimal? ConvertToUsd(string currencyCode, decimal? price)
        {
            if (currencyCode == "USD" || string.IsNullOrEmpty(currencyCode))
                return price;

            if (price == null)
                return null;

            if (price == 0)
                return 0;

            return price * logitudeCRMReportFilter.ExchangeRate;
        }
    }

}

