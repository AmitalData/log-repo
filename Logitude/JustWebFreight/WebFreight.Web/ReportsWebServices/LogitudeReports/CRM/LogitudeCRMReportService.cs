using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using WebFreight.Web.DataProviders;
using Logitude.CRM.Data.Repsitories;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.EntityQueryServices;
using Logitude.CRM.BL.DataContracts;
using Newtonsoft.Json;
using Logitude.BL.GlobalModel.EntityQueries;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.CRM
{
    public class LogitudeCRMReportService
    {
        private readonly int tenant;
        private LogitudeCRMReportDataProvider iDataProvider;
        private readonly LogitudeCRMReportFilter logitudeCRMReportFilter;
        public LogitudeCRMReportService(byte[] xmlFilters, int tenant)
        {
            this.tenant = tenant;
            logitudeCRMReportFilter = new LogitudeCRMReportFilter();
            this.BuildFilters(xmlFilters);
        }
        private void BuildFilters(byte[] xmlFilters)
        {
            QueryOperations iQueryOperations = BuildQueryOperation(xmlFilters);
            SetOppotunityFilter(iQueryOperations);
            SetCustomerStatusFilter(iQueryOperations);
            SetResellerFilter(iQueryOperations);
            SetShowNetFilter(iQueryOperations);
            SetExchangeRateFilter(iQueryOperations);
        }
        private QueryOperations BuildQueryOperation(byte[] xmlFilters)
        {
            MemoryStream memoryStream = new MemoryStream(xmlFilters);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations iQueryOperations = (QueryOperations)xmlSerializer.Deserialize(memoryStream);
            return iQueryOperations;
        }

        private void SetOppotunityFilter(QueryOperations iQueryOperations)
        {
            QueryFilterItem queryFilterItem = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "OpportunityTypes").FirstOrDefault();
            string opportunityTypes = null;
            if (queryFilterItem != null)
            {
                if (queryFilterItem.FieldValue != null)
                {
                    opportunityTypes = queryFilterItem.FieldValue.ToString();
                }
            }


            if (string.IsNullOrEmpty(opportunityTypes) || opportunityTypes.ToLower() == "all")
            {
                logitudeCRMReportFilter.OpportunityTypes = GetOpportunityTypes();
            }
            else
            {
                logitudeCRMReportFilter.OpportunityTypes = opportunityTypes.Trim(',').Split(',').ToList();
            }

        }

        private List<string> GetOpportunityTypes()
        {
            OpportunityTypeRepository additionalServiceRepository = new OpportunityTypeRepository(tenant);
            IQueryable<OpportunityType> opportunityTypes = additionalServiceRepository.GetAll(tenant);
            return opportunityTypes.Select(s => s.Id).ToList();
        }

        private void SetCustomerStatusFilter(QueryOperations iQueryOperations)
        {
            QueryFilterItem queryFilterItem = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "CustomerStatus").FirstOrDefault();
            if (queryFilterItem != null && queryFilterItem.FieldValue != null)
            {
                logitudeCRMReportFilter.CustomerStatus = queryFilterItem.FieldValue.ToString();
            }
        }

        private void SetResellerFilter(QueryOperations iQueryOperations)
        {
            QueryFilterItem queryFilterItem = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ResellerId").FirstOrDefault();

            if (queryFilterItem != null && queryFilterItem.FieldValue != null)
            {
                logitudeCRMReportFilter.ResellerId = queryFilterItem.FieldValue.ToString();
            }
        }

        private void SetShowNetFilter(QueryOperations iQueryOperations)
        {
            QueryFilterItem queryFilterItem = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ShowAllRecurringTenants").FirstOrDefault();

            if (queryFilterItem != null && queryFilterItem.FieldValue != null)
            {
                logitudeCRMReportFilter.ShowNet = Convert.ToBoolean(queryFilterItem.FieldValue);
            }
        }

        private void SetExchangeRateFilter(QueryOperations iQueryOperations)
        {
            QueryFilterItem queryFilterItem = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ExchangeRate").FirstOrDefault();

            if (queryFilterItem != null && queryFilterItem.FieldValue != null)
            {
                logitudeCRMReportFilter.ExchangeRate = decimal.Parse(queryFilterItem.FieldValue.ToString());
            }
        }

        public byte[] GetData()
        {
            this.LoadDataProvider();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(LogitudeCRMReportDataProvider));
            MemoryStream memoryStream = new MemoryStream();
            xmlSerializer.Serialize(memoryStream, iDataProvider);
            memoryStream.Seek(0, SeekOrigin.Begin);
            byte[] bytearray = memoryStream.ToArray();
            return bytearray;
        }

        private void LoadDataProvider()
        {
            this.iDataProvider = new LogitudeCRMReportDataProvider();
            this.BuildReportData();
            File.WriteAllText(@"D:\path.json", JsonConvert.SerializeObject(iDataProvider));

        }

        private void BuildReportData()
        {
            OpportunityQueryService opportunityQueryService = new OpportunityQueryService(tenant);
            List<OpportunityDetails> opportunityDetails = opportunityQueryService.GetLogitudeOpportunities(tenant, logitudeCRMReportFilter);
            GetTenantManagements(opportunityDetails);
            this.iDataProvider.LogitudeCRMReports = opportunityDetails.GroupBy(c => c.ClientId).Select(a => MappOpportunityToReport(a, opportunityDetails)).ToList();
        }

        private List<OpportunityDetails> GetTenantManagements(List<OpportunityDetails> opportunityDetails)
        {
            List<string> tenantNumbers = opportunityDetails.Select(b => b.TenantNumber).ToList();
    
            List<TenantManagement> tenantManagements = new TenantManagementQuery().GetForTenantNumbers(tenantNumbers);
            List<TenantManagementLicense> tenantManagementLicenses = new TenantManagementLicenseQuery().GetForTenantNumbers(tenantNumbers);

            foreach (OpportunityDetails opportunity in opportunityDetails)
            {
                opportunity.Total = (decimal.TryParse(opportunity.Field4, out decimal temp2) ? temp2 : 0);
                TenantManagement tenantManagement = tenantManagements.Where(a => a.Id.ToString() == opportunity.TenantNumber).FirstOrDefault();
                if (tenantManagement != null)
                {
                    opportunity.CurrencyCode = tenantManagement.PaymentCurrencyCode;
                    opportunity.ResellerCommission = tenantManagement.ResellerCommission;
                    opportunity.TenantManagementNumberOfUsers = GetTenantManagementNumberOfUsers(tenantManagement, tenantManagementLicenses.Sum(a => a.NumberOfUsers));
                    opportunity.TenantManagementTotalPrice = (decimal?)GetTenantManagementTotalPrice(tenantManagement, tenantManagementLicenses.Sum(a => a.TotalPrice));
                    opportunity.TenantManagementAveragePrice = (opportunity.TenantManagementNumberOfUsers == null || opportunity.TenantManagementNumberOfUsers == 0) ? 0 : opportunity.TenantManagementTotalPrice / opportunity.TenantManagementNumberOfUsers;
                    opportunity.TotalNet = opportunity.Total * (100 - tenantManagement.ResellerCommission ?? 0) / 100;
                }

            }

            return opportunityDetails;
        }

        private double? GetTenantManagementTotalPrice(TenantManagement tenantManagement, double? totalPrice)
        {
            if (tenantManagement.MainAdditionalPackageApplied)
            {
                if (tenantManagement.IsMultiPackage)
                {
                    return (totalPrice ?? 0) + (tenantManagement.TotalPrice ?? 0);
                }
                else
                {
                    return tenantManagement.TotalPrice ?? 0;
                }
            }
            else
            {
                if (tenantManagement.IsMultiPackage)
                {
                    return totalPrice ?? 0;
                }
                else
                {
                    return tenantManagement.TotalPrice;
                }

            }

        }

        private int? GetTenantManagementNumberOfUsers(TenantManagement tenantManagement, int? numberOfUsers)
        {
            if (tenantManagement.MainAdditionalPackageApplied)
            {
                if (tenantManagement.IsMultiPackage)
                {
                    return (numberOfUsers ?? 0) + (tenantManagement.NumberOfUsers ?? 0);
                }
                else
                {
                    return tenantManagement.NumberOfUsers ?? 0;
                }
            }
            else
            {
                if (tenantManagement.IsMultiPackage)
                {
                    return numberOfUsers ?? 0;
                }
                else
                {
                    return tenantManagement.NumberOfUsers;
                }

            }

        }

        private LogitudeCRMReport MappOpportunityToReport(IGrouping<string, OpportunityDetails> a, List<OpportunityDetails> opportunityDetails)
        {
            return new LogitudeCRMReport
            {
                ClientId = a.First().ClientId,
                TenantNumber = a.First().TenantNumber,
                ClientName = a.First().ClientName,
                Reseller = a.First().Reseller,
                CountryName = a.First().CountryName,
                CurrencyCode = a.First().CurrencyCode,
                ResellerCommission = a.First().ResellerCommission,
                TenantManagementNumberOfUsers = a.First().TenantManagementNumberOfUsers,
                TenantManagementAveragePrice = ToUsd(a.First().CurrencyCode, a.First().TenantManagementAveragePrice),
                TenantManagementTotalPrice = ToUsd(a.First().CurrencyCode, a.First().TenantManagementTotalPrice),
                Opportunities = GetOpportunities(opportunityDetails.Where(b => b.ClientId == a.First().ClientId), a.First().CurrencyCode),
                HasError = HasError(opportunityDetails.Where(b => b.ClientId == a.First().ClientId)),
                TotalNetBeforeYear = GetTotalNetBeforeYear(opportunityDetails.Where(b => b.ClientId == a.First().ClientId), a.First().CurrencyCode),
                Periods = GetPeriods(opportunityDetails.Where(b => b.ClientId == a.First().ClientId && b.CreateDate.Year == DateTime.Now.Year), a.First().CurrencyCode),
                TotalNet = ToUsd(a.First().CurrencyCode, opportunityDetails.Where(b => b.ClientId == a.First().ClientId).Sum(b => b.Total))
            };
        }

        private decimal? GetTotalNetBeforeYear(IEnumerable<OpportunityDetails> opportunityDetails, string currencyCode)
        {
            return ToUsd(currencyCode, opportunityDetails.Where(a => a.CreateDate.Year < DateTime.Now.Year).Sum(a => a.Total));
        }

        private int? HasError(IEnumerable<OpportunityDetails> opportunityDetails)
        {
            if (opportunityDetails.Where(a => a.OpportunityTypeCode == "N").Count() > 1 || opportunityDetails.Any(a => a.IsCancelled))
                return 0;

            return null;
        }

        private List<LogitudeCRMReportOpportunity> GetOpportunities(IEnumerable<OpportunityDetails> opportunityDetails, string currencyCode)
        {
            return opportunityDetails.Select(a => new LogitudeCRMReportOpportunity
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
                periods.Add(new OpportunityPeriod
                {
                    PeriodName = i.ToString(),
                    OpportunityPeriodSummaries = GetOpportunityPeriodSummaries(opportunityDetails.Where(a => a.CreateDate.Month == i), currencyCode)
                });
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

