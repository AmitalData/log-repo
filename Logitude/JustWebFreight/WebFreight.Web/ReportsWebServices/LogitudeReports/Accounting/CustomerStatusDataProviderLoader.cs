using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Resolvers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.DataProviders;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.Accounting
{
    public class CustomerStatusDataProviderLoader
    {
        private QueryOperations reportQueryOperations;
        private bool showLocals = false;
        private int tenant;


        public CustomerStatusDataProviderLoader(int _tenant)
        {
            tenant = _tenant;
            showLocals = LoggedContactResolver.GetLoggedContactShowLocal(_tenant);

        }
        public CustomerStatusDataProvider LoadFromXML(byte[] xmlFilters)
        {
            reportQueryOperations = DeserializeQueryOperationFromXml(xmlFilters);

            AgingReportService agingReportService = new AgingReportService(BuildReportParameters());
            agingReportService.RunReport();

            List<PeriodMExtended> resultedPeriods = agingReportService.MyPeriodExtendedList;

            return BuildDataProvider(resultedPeriods);
        }

        //-----------

        private CustomerStatusDataProvider BuildDataProvider(List<PeriodMExtended> resultedPeriods)
        {
            CustomerStatusDataProvider dataProvider = new CustomerStatusDataProvider();

            List<IGrouping<string, PeriodMExtended>> customersPeriod = resultedPeriods.GroupBy(d => d.AccountId).ToList();


            foreach (IGrouping<string, PeriodMExtended> customer in customersPeriod)
            {
                var periodsByDate = customer.GroupBy(r => r.PeriodName).ToList();

                CustomerStatus customerStatus = new CustomerStatus()
                {
                    // account details
                    CustomerName = customer.First().AccountEnglishName,
                    CustomerDisplayNumber = customer.First().AccountDisplayNumber,
                    CustomerPaymentTerm = customer.First().AccountPaymentTermName,
                    CustomerPhone = customer.First().AccountPhone,

                    // credit details
                    CreditLimit = customer.First().CreditLimit,
                    CreditStatus = customer.First().CreditStatus,
                    TotalFutureOpenCheques = customer.First().TotalFutureOpenCheques,
                    TotalOpenCheques = customer.First().TotalOpenCheques,
                    TotalOpenShipments = customer.First().TotalOpenShipments,

                    AccountingBalance = customer.Sum(d => d.Total),
                    Periods = GetStatusPeriods(periodsByDate)
                };

                dataProvider.CustomersStatuses.Add(customerStatus);
            }

            return dataProvider;
        }
        private List<StatusPeriod> GetStatusPeriods(List<IGrouping<string, PeriodMExtended>> customerDatePeriods)
        {
            List<StatusPeriod> ssss = new List<StatusPeriod>();
            foreach (var datePeriod in customerDatePeriods)
            {
                List<PeriodMExtended> currencyPeriods = datePeriod.ToList();
                StatusPeriod statusPeriod = new StatusPeriod()
                {
                    PeriodName = ResharpPeriodName(currencyPeriods.First().PeriodName),
                    PeriodTotal = currencyPeriods.Sum(d => d.Total),
                    PeriodCurrenciesSummaries = GetCurrencyPeriodsSummaries(currencyPeriods)
                };

                ssss.Add(statusPeriod);
            }

            return ssss;
        }

        private static List<PeriodCurrencySummary> GetCurrencyPeriodsSummaries(List<PeriodMExtended> currencyPeriods)
        {
            var ssss = new List<PeriodCurrencySummary>();

            foreach (PeriodMExtended currencyPeriod in currencyPeriods)
            {
                PeriodCurrencySummary summary = new PeriodCurrencySummary()
                {
                    CurrencyId = currencyPeriod.CurrencyId,
                    TotalCredit = currencyPeriod.OpenCredit,
                    TotalDebit = currencyPeriod.OpenDebit,
                    CurrencyCode = currencyPeriod.CurrencyCode
                };
                ssss.Add(summary);
            }

            return ssss;
        }
        private string ResharpPeriodName(string name)
        {
            if (name.Contains("b4"))
                name = name.Replace("b4", showLocals ? "לפני" : "Before");
            return name;
        }
        private void SetReportCategoryParameters(AgingReportParam reportParameters)
        {
            string category1Id = null;
            string category2Id = null;
            string category3Id = null;
            string category4Id = null;
            string category5Id = null;

            string categoryIndex = GetFilterValue<string>("CategoryIndex");
            string categoryValue = GetFilterValue<string>("CategoryValue");

            if (!string.IsNullOrEmpty(categoryIndex))
            {

                switch (categoryIndex)
                {
                    case "Category1": { category1Id = categoryValue; break; }
                    case "Category2": { category2Id = categoryValue; break; }
                    case "Category3": { category3Id = categoryValue; break; }
                    case "Category4": { category4Id = categoryValue; break; }
                    case "Category5": { category5Id = categoryValue; break; }
                }
            }

            reportParameters.Category1Id = category1Id;
            reportParameters.Category2Id = category2Id;
            reportParameters.Category3Id = category3Id;
            reportParameters.Category4Id = category4Id;
            reportParameters.Category5Id = category5Id;
        }

        private AgingReportParam BuildReportParameters()
        {
            AgingReportParam reportParameters = InitiateAgingReportParameters();
            uint monthsBackwards = GetAgingReportMonthsBackwards();

            reportParameters.Tenant = tenant;
            reportParameters.AgingForDate = GetFilterValue<DateTime>("AgingForDate");
            reportParameters.NumberOfmonthsbackwards = monthsBackwards;
            reportParameters.VendorCustomerId = GetFilterValue<string>("CustomerId");
            reportParameters.CollectorId = GetFilterValue<string>("CollectoId");
            reportParameters.SalesmanId = GetFilterValue<string>("SalesmanId");
            reportParameters.AggregateByGLAccountCurrencies = GetFilterValue<bool>("Detailed");

            reportParameters.GroupByDate = AgingReportParam.DateEnum.DueDate;
            reportParameters.AgingMethod = AgingReportParam.MethodEnum.TotalByMonthMethod.ToString();
            reportParameters.Aging4AccountTypeCode = (GetFilterValue<string>("GLAccountType") == "2") ? AgingReportParam.Aging4AccountTypeCodeEnum.Customer2 : AgingReportParam.Aging4AccountTypeCodeEnum.Vendor3;

            SetReportCategoryParameters(reportParameters);

            return reportParameters;
        }

        private uint GetAgingReportMonthsBackwards()
        {
            FullAccountingSettingRepository settingRepository = new FullAccountingSettingRepository(tenant);
            FullAccountingSetting settings = settingRepository.GetSingleFullAccountingSetting(tenant);
            var monthsBackwards = settings.NumberOfAgingMonths ?? 0;
            return (uint)monthsBackwards;
        }

        private AgingReportParam InitiateAgingReportParameters()
        {
            return new AgingReportParam()
            {

                AgingMethod_Options = Enum.GetNames(typeof(AgingReportParam.MethodEnum)).ToList().Aggregate((b4, aftr) => string.Concat(b4, ";", aftr)),
                GroupByDate_Options = Enum.GetNames(typeof(AgingReportParam.DateEnum)).ToList().Aggregate((b4, aftr) => string.Concat(b4, ";", aftr)),
                Aging4AccountTypeCode_Options = Enum.GetNames(typeof(AgingReportParam.Aging4AccountTypeCodeEnum)).ToList().Aggregate((b4, aftr) => string.Concat(b4, ";", aftr)),
            };
        }

        private QueryOperations DeserializeQueryOperationFromXml(byte[] xmlFilters)
        {
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            return queryOperations;
        }
        public T GetFilterValue<T>(string FieldName)
        {
            QueryFilterItem filterItem = reportQueryOperations.QueryFilterItems
                .Where(d => d.FieldName == FieldName).FirstOrDefault();

            if (filterItem != null && filterItem.FieldValue != null)
            {
                return (T)filterItem.FieldValue;
            }

            return default(T);
        }

    }

}