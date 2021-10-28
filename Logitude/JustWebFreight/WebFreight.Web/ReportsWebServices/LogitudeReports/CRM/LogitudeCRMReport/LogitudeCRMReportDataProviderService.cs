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
        private readonly LogitudeCRMOpperunityService logitudeCRMOpperunityService;

        public LogitudeCRMReportDataProviderService(byte[] xmlFilters, int tenant)
        {
            this.tenant = tenant;
            logitudeCRMReportFilter = new LogitudeCRMReportFilterService(xmlFilters, tenant).Build();
            logitudeCRMOpperunityService = new LogitudeCRMOpperunityService(tenant, logitudeCRMReportFilter);
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

            List<OpportunityCRMDetails> opportunities = logitudeCRMOpperunityService.Build();

            LogitudeCRMReportDataProvider logitudeCRMReportDataProvider = new LogitudeCRMReportDataProvider();
            logitudeCRMReportDataProvider.Customers = opportunities.GroupBy(c => c.ClientId).Select(a => MappOpportunityToCustomer(a)).OrderBy(a => a.ClientId).ToList();

            SetSums(logitudeCRMReportDataProvider);
            return logitudeCRMReportDataProvider;
        }

        private static void SetSums(LogitudeCRMReportDataProvider logitudeCRMReportDataProvider)
        {
            logitudeCRMReportDataProvider.NumberOfUsers = logitudeCRMReportDataProvider.Customers.Sum(a => a.NumberOfUsers ?? 0);
            logitudeCRMReportDataProvider.TotalPrice = logitudeCRMReportDataProvider.Customers.Sum(a => a.TotalPrice ?? 0);

            logitudeCRMReportDataProvider.OpportunitiesNumberOfUsers = logitudeCRMReportDataProvider.Customers.Sum(a => a.Opportunities.Sum(b => b.NumberOfUsers ?? 0));
            logitudeCRMReportDataProvider.OpportunitiesTotal = logitudeCRMReportDataProvider.Customers.Sum(a => a.Opportunities.Sum(b => b.Total ?? 0));
            logitudeCRMReportDataProvider.OpportunitiesTotalNet = logitudeCRMReportDataProvider.Customers.Sum(a => a.Opportunities.Sum(b => b.TotalNet ?? 0));

            logitudeCRMReportDataProvider.CurrentTotal = logitudeCRMReportDataProvider.Customers.Sum(a => a.CurrentTotal ?? 0);
            logitudeCRMReportDataProvider.TotalNetBeforeYear = logitudeCRMReportDataProvider.Customers.Sum(a => a.TotalNetBeforeYear ?? 0);


            logitudeCRMReportDataProvider.FirstMonthNumberOfUsersTotal = logitudeCRMReportDataProvider.Customers.Sum(a => a.OpportunityPeriods.Where(b => b.PeriodName == "1").Sum(b => b.OpportunityPeriodSummaries.Sum(c => c.NumberOfUsers ?? 0)));
            logitudeCRMReportDataProvider.FirstMonthNewIncomeTotal = logitudeCRMReportDataProvider.Customers.Sum(a => a.OpportunityPeriods.Where(b => b.PeriodName == "1").Sum(b => b.OpportunityPeriodSummaries.Sum(c => c.NewIncome ?? 0)));

            logitudeCRMReportDataProvider.SecondMonthNumberOfUsersTotal = logitudeCRMReportDataProvider.Customers.Sum(a => a.OpportunityPeriods.Where(b => b.PeriodName == "2").Sum(b => b.OpportunityPeriodSummaries.Sum(c => c.NumberOfUsers ?? 0)));
            logitudeCRMReportDataProvider.SecondMonthNewIncomeTotal = logitudeCRMReportDataProvider.Customers.Sum(a => a.OpportunityPeriods.Where(b => b.PeriodName == "2").Sum(b => b.OpportunityPeriodSummaries.Sum(c => c.NewIncome ?? 0)));

            logitudeCRMReportDataProvider.ThirdMonthNumberOfUsersTotal = logitudeCRMReportDataProvider.Customers.Sum(a => a.OpportunityPeriods.Where(b => b.PeriodName == "3").Sum(b => b.OpportunityPeriodSummaries.Sum(c => c.NumberOfUsers ?? 0)));
            logitudeCRMReportDataProvider.ThirdMonthNewIncomeTotal = logitudeCRMReportDataProvider.Customers.Sum(a => a.OpportunityPeriods.Where(b => b.PeriodName == "3").Sum(b => b.OpportunityPeriodSummaries.Sum(c => c.NewIncome ?? 0)));

            logitudeCRMReportDataProvider.FourthMonthNumberOfUsersTotal = logitudeCRMReportDataProvider.Customers.Sum(a => a.OpportunityPeriods.Where(b => b.PeriodName == "4").Sum(b => b.OpportunityPeriodSummaries.Sum(c => c.NumberOfUsers ?? 0)));
            logitudeCRMReportDataProvider.FourthMonthNewIncomeTotal = logitudeCRMReportDataProvider.Customers.Sum(a => a.OpportunityPeriods.Where(b => b.PeriodName == "4").Sum(b => b.OpportunityPeriodSummaries.Sum(c => c.NewIncome ?? 0)));

            logitudeCRMReportDataProvider.FifthMonthNumberOfUsersTotal = logitudeCRMReportDataProvider.Customers.Sum(a => a.OpportunityPeriods.Where(b => b.PeriodName == "5").Sum(b => b.OpportunityPeriodSummaries.Sum(c => c.NumberOfUsers ?? 0)));
            logitudeCRMReportDataProvider.FifthMonthNewIncomeTotal = logitudeCRMReportDataProvider.Customers.Sum(a => a.OpportunityPeriods.Where(b => b.PeriodName == "5").Sum(b => b.OpportunityPeriodSummaries.Sum(c => c.NewIncome ?? 0)));

            logitudeCRMReportDataProvider.SixthMonthNumberOfUsersTotal = logitudeCRMReportDataProvider.Customers.Sum(a => a.OpportunityPeriods.Where(b => b.PeriodName == "6").Sum(b => b.OpportunityPeriodSummaries.Sum(c => c.NumberOfUsers ?? 0)));
            logitudeCRMReportDataProvider.SixthMonthNewIncomeTotal = logitudeCRMReportDataProvider.Customers.Sum(a => a.OpportunityPeriods.Where(b => b.PeriodName == "6").Sum(b => b.OpportunityPeriodSummaries.Sum(c => c.NewIncome ?? 0)));

            logitudeCRMReportDataProvider.SeventhMonthNumberOfUsersTotal = logitudeCRMReportDataProvider.Customers.Sum(a => a.OpportunityPeriods.Where(b => b.PeriodName == "7").Sum(b => b.OpportunityPeriodSummaries.Sum(c => c.NumberOfUsers ?? 0)));
            logitudeCRMReportDataProvider.SeventhMonthNewIncomeTotal = logitudeCRMReportDataProvider.Customers.Sum(a => a.OpportunityPeriods.Where(b => b.PeriodName == "7").Sum(b => b.OpportunityPeriodSummaries.Sum(c => c.NewIncome ?? 0)));

            logitudeCRMReportDataProvider.EighthMonthNumberOfUsersTotal = logitudeCRMReportDataProvider.Customers.Sum(a => a.OpportunityPeriods.Where(b => b.PeriodName == "8").Sum(b => b.OpportunityPeriodSummaries.Sum(c => c.NumberOfUsers ?? 0)));
            logitudeCRMReportDataProvider.EighthMonthNewIncomeTotal = logitudeCRMReportDataProvider.Customers.Sum(a => a.OpportunityPeriods.Where(b => b.PeriodName == "8").Sum(b => b.OpportunityPeriodSummaries.Sum(c => c.NewIncome ?? 0)));

            logitudeCRMReportDataProvider.NinthMonthNumberOfUsersTotal = logitudeCRMReportDataProvider.Customers.Sum(a => a.OpportunityPeriods.Where(b => b.PeriodName == "9").Sum(b => b.OpportunityPeriodSummaries.Sum(c => c.NumberOfUsers ?? 0)));
            logitudeCRMReportDataProvider.NinthMonthNewIncomeTotal = logitudeCRMReportDataProvider.Customers.Sum(a => a.OpportunityPeriods.Where(b => b.PeriodName == "9").Sum(b => b.OpportunityPeriodSummaries.Sum(c => c.NewIncome ?? 0)));

            logitudeCRMReportDataProvider.TenthMonthNumberOfUsersTotal = logitudeCRMReportDataProvider.Customers.Sum(a => a.OpportunityPeriods.Where(b => b.PeriodName == "10").Sum(b => b.OpportunityPeriodSummaries.Sum(c => c.NumberOfUsers ?? 0)));
            logitudeCRMReportDataProvider.TenthMonthNewIncomeTotal = logitudeCRMReportDataProvider.Customers.Sum(a => a.OpportunityPeriods.Where(b => b.PeriodName == "10").Sum(b => b.OpportunityPeriodSummaries.Sum(c => c.NewIncome ?? 0)));

            logitudeCRMReportDataProvider.EleventhMonthNumberOfUsersTotal = logitudeCRMReportDataProvider.Customers.Sum(a => a.OpportunityPeriods.Where(b => b.PeriodName == "11").Sum(b => b.OpportunityPeriodSummaries.Sum(c => c.NumberOfUsers ?? 0)));
            logitudeCRMReportDataProvider.EleventhMonthNewIncomeTotal = logitudeCRMReportDataProvider.Customers.Sum(a => a.OpportunityPeriods.Where(b => b.PeriodName == "11").Sum(b => b.OpportunityPeriodSummaries.Sum(c => c.NewIncome ?? 0)));

            logitudeCRMReportDataProvider.TwelfthMonthNumberOfUsersTotal = logitudeCRMReportDataProvider.Customers.Sum(a => a.OpportunityPeriods.Where(b => b.PeriodName == "12").Sum(b => b.OpportunityPeriodSummaries.Sum(c => c.NumberOfUsers ?? 0)));
            logitudeCRMReportDataProvider.TwelfthMonthNewIncomeTotal = logitudeCRMReportDataProvider.Customers.Sum(a => a.OpportunityPeriods.Where(b => b.PeriodName == "12").Sum(b => b.OpportunityPeriodSummaries.Sum(c => c.NewIncome ?? 0)));
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
                CurrentTotal = opportunityCRMDetail.ToList().Sum(b => b.Total),
                OpportunityPeriods = GetPeriods(opportunityCRMDetail.ToList()),
                TotalNetBeforeYear = GetTotalNetBeforeYear(opportunityCRMDetail.ToList()),
                InActive = opportunityCRMDetail.First().InActive,
            };
        }

        private decimal? GetTotalNetBeforeYear(IEnumerable<OpportunityCRMDetails> opportunityDetails)
        {
            return opportunityDetails.Where(a => a.ActualClosingDate.Year < DateTime.Now.Year).Sum(a => a.Total);
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

        private List<OpportunityPeriod> GetPeriods(IEnumerable<OpportunityCRMDetails> opportunityDetails)
        {
            List<OpportunityPeriod> periods = new List<OpportunityPeriod>();
            for (int i = 1; i <= 12; i++)
            {
                OpportunityPeriod opportunityPeriod = new OpportunityPeriod();
                opportunityPeriod.PeriodName = i.ToString();
                opportunityPeriod.OpportunityPeriodSummaries = GetOpportunityPeriodSummaries(opportunityDetails, i);
                opportunityPeriod.Total = opportunityPeriod.OpportunityPeriodSummaries?.Sum(a => a.NewIncome ?? 0) ?? 0;
                periods.Add(opportunityPeriod);
            }

            return periods;
        }

        private List<OpportunityPeriodSummary> GetOpportunityPeriodSummaries(IEnumerable<OpportunityCRMDetails> opportunityDetails, int mounth)
        {
            return opportunityDetails
                .Where(b => b.ActualClosingDate.Year == DateTime.Now.Year && b.ActualClosingDate.Month == mounth)
                .GroupBy(a => 1)
                .Select(a => new OpportunityPeriodSummary
                {
                    NumberOfUsers = a.Sum(b => b.NumberOfUsers),
                    IsNewCustomer = a.Where(b => b.IsNewCustomer != null).OrderByDescending(b => b.ActualClosingDate).FirstOrDefault()?.IsNewCustomer,
                    NewIncome = a.Sum(b => b.Total),
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

