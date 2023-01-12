using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using WebFreight.Web.DataProviders;
using Logitude.CRM.BL.DataContracts;
using Newtonsoft.Json;
using WebFreight.Web.Services;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.CRM.LogitudeCRMReport
{
    public class LogitudeCRMReportDataProviderService
    {
        private readonly int tenant;
        private readonly LogitudeCRMReportFilter logitudeCRMReportFilter;
        private readonly LogitudeCRMOpperunityService logitudeCRMOpperunityService;

        private enum Month
        {
            January = 1,
            February = 2,
            March = 3,
            April = 4,
            May = 5,
            June = 6,
            July = 7,
            August = 8,
            September = 9,
            October = 10,
            November = 11,
            December = 12
        }

        public LogitudeCRMReportDataProviderService(byte[] xmlFilters, int tenant)
        {
            this.tenant = tenant;
            logitudeCRMReportFilter = new LogitudeCRMReportFilterService(xmlFilters, tenant).Build();
            logitudeCRMOpperunityService = new LogitudeCRMOpperunityService(tenant, logitudeCRMReportFilter);
        }

        public byte[] Load()
        {
            LogitudeCRMReportDataProvider logitudeCRMReportDataProvider = BuildDataProvider();
            return new ReportMemoryStreamService().Convert(logitudeCRMReportDataProvider, typeof(LogitudeCRMReportDataProvider), tenant);
        }

        private LogitudeCRMReportDataProvider BuildDataProvider()
        {

            List<OpportunityCRMDetails> opportunities = logitudeCRMOpperunityService.Build();

            LogitudeCRMReportDataProvider logitudeCRMReportDataProvider = new LogitudeCRMReportDataProvider();
            logitudeCRMReportDataProvider.Customers = opportunities.GroupBy(c => c.ClientId).Select(a => MappOpportunityToCustomer(a)).OrderBy(a => a.ClientId).ToList();

            SetSums(logitudeCRMReportDataProvider);
            return logitudeCRMReportDataProvider;
        }

        private void SetSums(LogitudeCRMReportDataProvider logitudeCRMReportDataProvider)
        {
            logitudeCRMReportDataProvider.NumberOfUsers = logitudeCRMReportDataProvider.Customers.Sum(a => a.NumberOfUsers ?? 0);
            logitudeCRMReportDataProvider.TotalPrice = logitudeCRMReportDataProvider.Customers.Sum(a => a.TotalPrice ?? 0);

            logitudeCRMReportDataProvider.OpportunitiesNumberOfUsers = logitudeCRMReportDataProvider.Customers.Sum(a => a.Opportunities.Sum(b => b.NumberOfUsers ?? 0));
            logitudeCRMReportDataProvider.OpportunitiesTotal = logitudeCRMReportDataProvider.Customers.Sum(a => a.Opportunities.Sum(b => b.Total ?? 0));
            logitudeCRMReportDataProvider.OpportunitiesTotalNet = logitudeCRMReportDataProvider.Customers.Sum(a => a.Opportunities.Sum(b => b.TotalNet ?? 0));

            logitudeCRMReportDataProvider.CurrentTotal = logitudeCRMReportDataProvider.Customers.Sum(a => a.CurrentTotal ?? 0);
            logitudeCRMReportDataProvider.TotalNetBeforeYear = logitudeCRMReportDataProvider.Customers.Sum(a => a.TotalNetBeforeYear ?? 0);
            logitudeCRMReportDataProvider.TotalNetAfterYear = logitudeCRMReportDataProvider.Customers.Sum(a => a.TotalNetAfterYear ?? 0);

            SetNumberOfUsersTotals(logitudeCRMReportDataProvider);
            SetNewIncomeTotals(logitudeCRMReportDataProvider);
            SetNewCustomerTotals(logitudeCRMReportDataProvider);
        }

        private void SetNewCustomerTotals(LogitudeCRMReportDataProvider logitudeCRMReportDataProvider)
        {
            logitudeCRMReportDataProvider.FirstMonthNewCustomerTotal = GetOpportunityPeriodsNewCustomerTotal(logitudeCRMReportDataProvider, Month.January);
            logitudeCRMReportDataProvider.SecondMonthNewCustomerTotal = GetOpportunityPeriodsNewCustomerTotal(logitudeCRMReportDataProvider, Month.February);
            logitudeCRMReportDataProvider.ThirdMonthNewCustomerTotal = GetOpportunityPeriodsNewCustomerTotal(logitudeCRMReportDataProvider, Month.March);
            logitudeCRMReportDataProvider.FourthMonthNewCustomerTotal = GetOpportunityPeriodsNewCustomerTotal(logitudeCRMReportDataProvider, Month.April);
            logitudeCRMReportDataProvider.FifthMonthNewCustomerTotal = GetOpportunityPeriodsNewCustomerTotal(logitudeCRMReportDataProvider, Month.May);
            logitudeCRMReportDataProvider.SixthMonthNewCustomerTotal = GetOpportunityPeriodsNewCustomerTotal(logitudeCRMReportDataProvider, Month.June);
            logitudeCRMReportDataProvider.SeventhMonthNewCustomerTotal = GetOpportunityPeriodsNewCustomerTotal(logitudeCRMReportDataProvider, Month.July);
            logitudeCRMReportDataProvider.EighthMonthNewCustomerTotal = GetOpportunityPeriodsNewCustomerTotal(logitudeCRMReportDataProvider, Month.August);
            logitudeCRMReportDataProvider.NinthMonthNewCustomerTotal = GetOpportunityPeriodsNewCustomerTotal(logitudeCRMReportDataProvider, Month.September);
            logitudeCRMReportDataProvider.TenthMonthNewCustomerTotal = GetOpportunityPeriodsNewCustomerTotal(logitudeCRMReportDataProvider, Month.October);
            logitudeCRMReportDataProvider.EleventhMonthNewCustomerTotal = GetOpportunityPeriodsNewCustomerTotal(logitudeCRMReportDataProvider, Month.November);
            logitudeCRMReportDataProvider.TwelfthMonthNewCustomerTotal = GetOpportunityPeriodsNewCustomerTotal(logitudeCRMReportDataProvider, Month.December);
        }

        private void SetNewIncomeTotals(LogitudeCRMReportDataProvider logitudeCRMReportDataProvider)
        {
            logitudeCRMReportDataProvider.FirstMonthNewIncomeTotal = GetOpportunityPeriodsNewIncomeTotal(logitudeCRMReportDataProvider, Month.January);
            logitudeCRMReportDataProvider.SecondMonthNewIncomeTotal = GetOpportunityPeriodsNewIncomeTotal(logitudeCRMReportDataProvider, Month.February);
            logitudeCRMReportDataProvider.ThirdMonthNewIncomeTotal = GetOpportunityPeriodsNewIncomeTotal(logitudeCRMReportDataProvider, Month.March);
            logitudeCRMReportDataProvider.FourthMonthNewIncomeTotal = GetOpportunityPeriodsNewIncomeTotal(logitudeCRMReportDataProvider, Month.April);
            logitudeCRMReportDataProvider.FifthMonthNewIncomeTotal = GetOpportunityPeriodsNewIncomeTotal(logitudeCRMReportDataProvider, Month.May);
            logitudeCRMReportDataProvider.SixthMonthNewIncomeTotal = GetOpportunityPeriodsNewIncomeTotal(logitudeCRMReportDataProvider, Month.June);
            logitudeCRMReportDataProvider.SeventhMonthNewIncomeTotal = GetOpportunityPeriodsNewIncomeTotal(logitudeCRMReportDataProvider, Month.July);
            logitudeCRMReportDataProvider.EighthMonthNewIncomeTotal = GetOpportunityPeriodsNewIncomeTotal(logitudeCRMReportDataProvider, Month.August);
            logitudeCRMReportDataProvider.NinthMonthNewIncomeTotal = GetOpportunityPeriodsNewIncomeTotal(logitudeCRMReportDataProvider, Month.September);
            logitudeCRMReportDataProvider.TenthMonthNewIncomeTotal = GetOpportunityPeriodsNewIncomeTotal(logitudeCRMReportDataProvider, Month.October);
            logitudeCRMReportDataProvider.EleventhMonthNewIncomeTotal = GetOpportunityPeriodsNewIncomeTotal(logitudeCRMReportDataProvider, Month.November);
            logitudeCRMReportDataProvider.TwelfthMonthNewIncomeTotal = GetOpportunityPeriodsNewIncomeTotal(logitudeCRMReportDataProvider, Month.December);
        }

        private void SetNumberOfUsersTotals(LogitudeCRMReportDataProvider logitudeCRMReportDataProvider)
        {
            logitudeCRMReportDataProvider.FirstMonthNumberOfUsersTotal = GetOpportunityPeriodsNumberOfUsersTotal(logitudeCRMReportDataProvider, Month.January);
            logitudeCRMReportDataProvider.SecondMonthNumberOfUsersTotal = GetOpportunityPeriodsNumberOfUsersTotal(logitudeCRMReportDataProvider, Month.February);
            logitudeCRMReportDataProvider.ThirdMonthNumberOfUsersTotal = GetOpportunityPeriodsNumberOfUsersTotal(logitudeCRMReportDataProvider, Month.March);
            logitudeCRMReportDataProvider.FourthMonthNumberOfUsersTotal = GetOpportunityPeriodsNumberOfUsersTotal(logitudeCRMReportDataProvider, Month.April);
            logitudeCRMReportDataProvider.FifthMonthNumberOfUsersTotal = GetOpportunityPeriodsNumberOfUsersTotal(logitudeCRMReportDataProvider, Month.May);
            logitudeCRMReportDataProvider.SixthMonthNumberOfUsersTotal = GetOpportunityPeriodsNumberOfUsersTotal(logitudeCRMReportDataProvider, Month.June);
            logitudeCRMReportDataProvider.SeventhMonthNumberOfUsersTotal = GetOpportunityPeriodsNumberOfUsersTotal(logitudeCRMReportDataProvider, Month.July);
            logitudeCRMReportDataProvider.EighthMonthNumberOfUsersTotal = GetOpportunityPeriodsNumberOfUsersTotal(logitudeCRMReportDataProvider, Month.August);
            logitudeCRMReportDataProvider.NinthMonthNumberOfUsersTotal = GetOpportunityPeriodsNumberOfUsersTotal(logitudeCRMReportDataProvider, Month.September);
            logitudeCRMReportDataProvider.EleventhMonthNumberOfUsersTotal = GetOpportunityPeriodsNumberOfUsersTotal(logitudeCRMReportDataProvider, Month.November);
            logitudeCRMReportDataProvider.TenthMonthNumberOfUsersTotal = GetOpportunityPeriodsNumberOfUsersTotal(logitudeCRMReportDataProvider, Month.October);
            logitudeCRMReportDataProvider.TwelfthMonthNumberOfUsersTotal = GetOpportunityPeriodsNumberOfUsersTotal(logitudeCRMReportDataProvider, Month.December);
        }

        private int GetOpportunityPeriodsNumberOfUsersTotal(LogitudeCRMReportDataProvider logitudeCRMReportDataProvider, Month month)
        {
            return logitudeCRMReportDataProvider.Customers.Sum(a => a.OpportunityPeriods.Where(b => b.Period == ((int)month)).Sum(b => b.OpportunityPeriodSummaries.Sum(c => c.NumberOfUsers ?? 0)));
        }

        private decimal GetOpportunityPeriodsNewIncomeTotal(LogitudeCRMReportDataProvider logitudeCRMReportDataProvider, Month month)
        {
            return logitudeCRMReportDataProvider.Customers.Sum(a => a.OpportunityPeriods.Where(b => b.Period == ((int)month)).Sum(b => b.OpportunityPeriodSummaries.Sum(c => c.NewIncome ?? 0)));
        }
        private int GetOpportunityPeriodsNewCustomerTotal(LogitudeCRMReportDataProvider logitudeCRMReportDataProvider, Month month)
        {
            return logitudeCRMReportDataProvider.Customers.Sum(a => a.OpportunityPeriods.Where(b => b.Period == ((int)month)).Sum(b => b.OpportunityPeriodSummaries.Sum(c => c.IsNewCustomer ?? 0)));
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
                AveragePrice = logitudeCRMOpperunityService.ConvertToUsd(opportunityCRMDetail.First().CurrencyCode, opportunityCRMDetail.First().TenantManagementAveragePrice),
                TotalPrice = logitudeCRMOpperunityService.ConvertToUsd(opportunityCRMDetail.First().CurrencyCode, opportunityCRMDetail.First().TenantManagementTotalPrice),

                Opportunities = GetOpportunityItems(opportunityCRMDetail.ToList()),
                HasError = HasError(opportunityCRMDetail.ToList()),
                CurrentTotal = CalculateNetValue(opportunityCRMDetail.ToList().Sum(b => b.Total), opportunityCRMDetail.First().ResellerCommission),
                OpportunityPeriods = GetPeriods(opportunityCRMDetail.ToList(), opportunityCRMDetail.First().ResellerCommission),
                TotalNetBeforeYear = CalculateNetValue(GetTotalNetBeforeYear(opportunityCRMDetail.ToList()), opportunityCRMDetail.First().ResellerCommission),
                TotalNetAfterYear = CalculateNetValue(GetTotalNetAfterYear(opportunityCRMDetail.ToList()), opportunityCRMDetail.First().ResellerCommission),
                InActive = opportunityCRMDetail.First().InActive,
            };
        }

        private decimal CalculateNetValue(decimal value, int? resellerCommission)
        {
            if (!logitudeCRMReportFilter.ShowNet)
                return value;

            return value * (100 - (resellerCommission ?? 0)) / 100;
        }

        private decimal GetTotalNetBeforeYear(IEnumerable<OpportunityCRMDetails> opportunityDetails)
        {
            return opportunityDetails.Where(a => a.ActualClosingDate.Year < logitudeCRMReportFilter.Year).Sum(a => a.Total);
        }

        private decimal GetTotalNetAfterYear(List<OpportunityCRMDetails> opportunityDetails)
        {
            return opportunityDetails.Where(a => a.ActualClosingDate.Year > logitudeCRMReportFilter.Year).Sum(a => a.Total);
        }


        private List<OpportunityItem> GetOpportunityItems(IEnumerable<OpportunityCRMDetails> opportunityDetails)
        {
            return opportunityDetails
                .GroupBy(a => 1)
                .Select(a => new OpportunityItem
                {
                    NumberOfUsers = a.Sum(b => b.NumberOfUsers),
                    Total = a.Sum(b => b.Total),
                    TotalNet = a.Sum(b => b.TotalNet),
                }).ToList();
        }

        private List<OpportunityPeriod> GetPeriods(IEnumerable<OpportunityCRMDetails> opportunityDetails, int? resellerCommission)
        {
            List<OpportunityPeriod> periods = new List<OpportunityPeriod>();
            foreach (Month month in (Month[])Enum.GetValues(typeof(Month)))
            {
                periods.Add(GetOpportunityPeriod(opportunityDetails, resellerCommission, month));
            }
            return periods;
        }

        private OpportunityPeriod GetOpportunityPeriod(IEnumerable<OpportunityCRMDetails> opportunityDetails, int? resellerCommission, Month month)
        {
            OpportunityPeriod opportunityPeriod = new OpportunityPeriod();
            opportunityPeriod.Period = ((int)month);
            opportunityPeriod.PeriodName = Enum.GetName(typeof(Month), month);
            opportunityPeriod.OpportunityPeriodSummaries = GetOpportunityPeriodSummaries(opportunityDetails, ((int)month), resellerCommission);
            opportunityPeriod.Total = opportunityPeriod.OpportunityPeriodSummaries?.Sum(a => a.NewIncome ?? 0) ?? 0;
            return opportunityPeriod;
        }

        private List<OpportunityPeriodSummary> GetOpportunityPeriodSummaries(IEnumerable<OpportunityCRMDetails> opportunityDetails, int mounth, int? resellerCommission)
        {
            return opportunityDetails
                .Where(b => b.ActualClosingDate.Year == logitudeCRMReportFilter.Year && b.ActualClosingDate.Month == mounth)
                .GroupBy(a => 1)
                .Select(a => new OpportunityPeriodSummary
                {
                    NumberOfUsers = a.Sum(b => b.NumberOfUsers),
                    IsNewCustomer = a.Where(b => b.IsNewCustomer != null).OrderBy(b => b.IsNewCustomer).FirstOrDefault()?.IsNewCustomer,
                    NewIncome = CalculateNetValue(a.Sum(b => b.Total), resellerCommission),
                }).ToList();
        }

        private int? HasError(IEnumerable<OpportunityCRMDetails> opportunityDetails)
        {
            if (opportunityDetails.Where(a => a.OpportunityTypeCode == "N").Count() > 1 || opportunityDetails.Any(a => a.IsCancelled))
                return 0;

            return null;
        }

    }

}

