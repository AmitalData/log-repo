using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools.Helpers;
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
        private CustomerStatusDataProvider dataProvider;
        private List<LedgerTransactionList> ExternalTransactions;

        public CustomerStatusDataProviderLoader(int _tenant)
        {
            tenant = _tenant;
            showLocals = LoggedContactResolver.GetLoggedContactShowLocal(_tenant);
            dataProvider = new CustomerStatusDataProvider();
        }
        public CustomerStatusDataProvider LoadFromXML(byte[] xmlFilters)
        {
            reportQueryOperations = DeserializeQueryOperationFromXml(xmlFilters);

            AgingReportService agingReportService = new AgingReportService(BuildReportParameters());
            agingReportService.RunReport();

            GetExternalTransactionsForPeriodsAccounts(agingReportService.MyPeriodExtendedList);

            return BuildDataProvider(agingReportService.MyPeriodExtendedList);
        }








        //-----------

        private CustomerStatusDataProvider BuildDataProvider(List<PeriodMExtended> agingPeriods)
        {
            CreateCustomerStatusesByAgingPeriods(agingPeriods);

            SortCustomerStatuses();

            return dataProvider;
        }

        private void GetExternalTransactionsForPeriodsAccounts(List<PeriodMExtended> agingPeriods)
        {
            List<string> accountsIds = agingPeriods.Select(p => p.AccountId).Distinct().ToList();

            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            LedgerTransactionListQueryService ledgerQuery = new LedgerTransactionListQueryService(accountingContext);
            ExternalTransactions = ledgerQuery.GetExternalTransactionsForAccounts(accountsIds, tenant).ToList();
        }

        private void CreateCustomerStatusesByAgingPeriods(List<PeriodMExtended> resultedPeriods)
        {
            var customersPeriods = GroupPeriodsByCustomers(resultedPeriods);
            customersPeriods = FilterPeriods(customersPeriods);

            foreach (var customerPeriods in customersPeriods)
                CreateCustomerStatusByPeriods(customerPeriods);
        }

        private IEnumerable<IGrouping<string, PeriodMExtended>> FilterPeriods(IEnumerable<IGrouping<string, PeriodMExtended>> customersPeriods)
        {
            bool filterByDeptors = CheckIfFilterByDeptorsEnabled();
            bool filterByDept = CheckIfFilterByDeptEnabled();
            bool noFilterSelected = CheckIfNoFilterSelected();

            if (filterByDeptors)
                customersPeriods = customersPeriods.Where(customerPeriods => customerPeriods.First().BalanceInLocalCurrency > 0).ToList();
            else if (filterByDept)
            {
                decimal balanceFilterValue = GetFilterValue<decimal>("BalanceFilterValue");
                customersPeriods = customersPeriods.Where(customerPeriods => customerPeriods.First().BalanceInLocalCurrency > balanceFilterValue).ToList();
            }
            else
                customersPeriods = customersPeriods.ToList();


            return customersPeriods;
        }

        private static IEnumerable<IGrouping<string, PeriodMExtended>> GroupPeriodsByCustomers(List<PeriodMExtended> resultedPeriods)
        {
            return resultedPeriods.GroupBy(periods => periods.AccountId);
        }

        private void CreateCustomerStatusByPeriods(IGrouping<string, PeriodMExtended> customerPeriods)
        {
            CustomerStatus customerStatus = CreateNewCustomerStatus(customerPeriods);

            bool isCreditLimitSetAndCustomerHasNoCredit = CheckIfCustomerHasNoCreditAndIsCreditLimitSet(customerStatus);
            if (!isCreditLimitSetAndCustomerHasNoCredit)
                dataProvider.CustomersStatuses.Add(customerStatus);
        }

        private bool CheckIfCustomerHasNoCreditAndIsCreditLimitSet(CustomerStatus customerStatus)
        {
            var creditLimitSet = GetFilterValue<bool>("IsCreditLimitSet") == true;
            var customerHasNoCreditLimit = customerStatus.CreditLimit == 0;
            bool isCreditLimitSetAndCustomerHasNoCredit = (creditLimitSet && customerHasNoCreditLimit);
            return isCreditLimitSetAndCustomerHasNoCredit;
        }

        private bool CheckIfNoFilterSelected()
        {
            string balanceFilter = GetFilterValue<string>("BalanceFilter");
            bool noFilter = balanceFilter == "all" || balanceFilter == null;
            return noFilter;
        }

        private bool CheckIfFilterByDeptEnabled()
        {
            string balanceFilter = GetFilterValue<string>("BalanceFilter");
            var filterByDept = balanceFilter == "debt";
            return filterByDept;
        }

        private bool CheckIfFilterByDeptorsEnabled()
        {
            string balanceFilter = GetFilterValue<string>("BalanceFilter");
            var filterByDeptors = balanceFilter == "debtors";
            return filterByDeptors;
        }

        private void SortCustomerStatuses()
        {
            string sortField = GetFilterValue<string>("SortField");
            string sortDirection = GetFilterValue<string>("SortDirection");

            if (sortField == "balance")
            {
                if (sortDirection == "Descending")
                    dataProvider.CustomersStatuses = dataProvider.CustomersStatuses.OrderByDescending(d => d.AccountingBalance).ToList();
                else
                    dataProvider.CustomersStatuses = dataProvider.CustomersStatuses.OrderBy(d => d.AccountingBalance).ToList();
            }
            else if (sortField == "customer")
            {
                if (sortDirection == "Descending")
                    dataProvider.CustomersStatuses = dataProvider.CustomersStatuses.OrderByDescending(d => d.CustomerName).ToList();
                else
                    dataProvider.CustomersStatuses = dataProvider.CustomersStatuses.OrderBy(d => d.CustomerName).ToList();
            }
            else if (sortField == "TotalToCollect")
            {
                if (sortDirection == "Descending")
                    dataProvider.CustomersStatuses = dataProvider.CustomersStatuses.OrderByDescending(d => d.TotalToCollect).ToList();
                else
                    dataProvider.CustomersStatuses = dataProvider.CustomersStatuses.OrderBy(d => d.TotalToCollect).ToList();
            }
            else if (sortField == "Obligo")
            {
                if (sortDirection == "Descending")
                    dataProvider.CustomersStatuses = dataProvider.CustomersStatuses.OrderByDescending(d => d.Obligo).ToList();
                else
                    dataProvider.CustomersStatuses = dataProvider.CustomersStatuses.OrderBy(d => d.Obligo).ToList();
            }
            else if (sortField == "CreditUsed")
            {
                if (sortDirection == "Descending")
                    dataProvider.CustomersStatuses = dataProvider.CustomersStatuses.OrderByDescending(d => d.CreditUsed).ToList();
                else
                    dataProvider.CustomersStatuses = dataProvider.CustomersStatuses.OrderBy(d => d.CreditUsed).ToList();
            }
            else
            {
                dataProvider.CustomersStatuses = dataProvider.CustomersStatuses.OrderBy(d => d.CustomerName).ToList();
            }
        }

        private CustomerStatus CreateNewCustomerStatus(IGrouping<string, PeriodMExtended> customerPeriods)
        {
            CustomerStatus customerStatus = new CustomerStatus()
            {
                // account details
                CustomerName = customerPeriods.First().AccountEnglishName,
                CustomerLocalName = customerPeriods.First().AccountLocalName,
                CustomerDisplayNumber = customerPeriods.First().AccountDisplayNumber,
                CustomerPaymentTerm = customerPeriods.First().AccountTermName,
                CustomerLocalPaymentTerm = customerPeriods.First().AccountTermLocalName,
                CustomerPhone = customerPeriods.First().AccountPhone,

                //credit details
                CreditLimit = (decimal)customerPeriods.First().CreditLimitAmount,
                CreditStatus = customerPeriods.First().CreditStatusAmount ?? 0,
                TotalFutureOpenCheques = customerPeriods.First().TotalFutureOpenCheques ?? 0,
                TotalOpenCheques = customerPeriods.First().TotalOpenCheques ?? 0,
                TotalOpenShipments = customerPeriods.First().TotalOpenShipments ?? 0,
                ExternalTransactionsTotal = ExternalTransactions.Where(d => d.AccountId == customerPeriods.First().AccountId).Sum(d => d.LocalAmountCredit),

                AccountingBalance = GetBalanceSummationForSpliitedAccounts(customerPeriods) ?? 0,
                Periods = GetStatusPeriods(customerPeriods)
            };
            return customerStatus;
        }

        private static decimal? GetBalanceSummationForSpliitedAccounts(IGrouping<string, PeriodMExtended> customerPeriods)
        {
            return customerPeriods
                            .GroupBy(d => new { d.CurrencyId, d.SplitAccountId })
                            .Sum(d => d.First().BalanceInLocalCurrency);
        }

        private List<StatusPeriod> GetStatusPeriods(IGrouping<string, PeriodMExtended> customerPeriods)
        {
            var customerDatePeriods = customerPeriods.GroupBy(r => r.PeriodName).ToList();

            List<StatusPeriod> statusPeriods = new List<StatusPeriod>();
            foreach (var datePeriod in customerDatePeriods)
            {
                StatusPeriod statusPeriod = CreateNewStatusPeriods(datePeriod);
                statusPeriods.Add(statusPeriod);
            }

            return statusPeriods;
        }

        private StatusPeriod CreateNewStatusPeriods(IGrouping<string, PeriodMExtended> datePeriod)
        {
            List<PeriodMExtended> currencyPeriods = datePeriod.ToList();
            StatusPeriod statusPeriod = new StatusPeriod()
            {
                PeriodName = ResharpPeriodName(currencyPeriods.First().PeriodName),
                PeriodTotal = currencyPeriods.Sum(d => d.Total),
                PeriodCurrenciesSummaries = GetCurrencyPeriodsSummaries(currencyPeriods)
            };
            return statusPeriod;
        }

        private List<PeriodCurrencySummary> GetCurrencyPeriodsSummaries(List<PeriodMExtended> currencyPeriods)
        {
            var ssss = new List<PeriodCurrencySummary>();

            foreach (PeriodMExtended currencyPeriod in currencyPeriods)
            {
                var showCurrencyDetails = GetFilterValue<bool>("Detailed");
                var customerId = GetFilterValue<string>("CustomerId");
                PeriodCurrencySummary summary = new PeriodCurrencySummary()
                {
                    CurrencyId = currencyPeriod.CurrencyId,
                    TotalCredit = currencyPeriod.OpenCredit,
                    TotalDebit = currencyPeriod.OpenDebit,
                    CurrencyCode = currencyPeriod.CurrencyCode
                };

                //if ((showCurrencyDetails && customerId != null && currencyPeriod.CurrencyId != null)||!showCurrencyDetails)
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
            reportParameters.CollectorId = GetFilterValue<string>("CollectorId");
            reportParameters.SalesmanId = GetFilterValue<string>("SalesmanId");
            reportParameters.AggregateByGLAccountCurrencies = GetFilterValue<bool>("Detailed");

            reportParameters.GroupByDate = AgingReportParam.DateEnum.DueDate;
            reportParameters.AgingMethod = AgingReportParam.MethodEnum.ReconcileOpenBalanceMethod.ToString();
            reportParameters.Aging4AccountTypeCode = (GetFilterValue<string>("GLAccountType") == "2") ? AgingReportParam.Aging4AccountTypeCodeEnum.Customer2 : AgingReportParam.Aging4AccountTypeCodeEnum.Vendor3;

            SetReportCategoryParameters(reportParameters);

            return reportParameters;
        }

        private uint GetAgingReportMonthsBackwards()
        {
            FullAccountingSetting settings = GetFullAccountingSettings();

            if (settings.NumberOfAgingMonths == null)
                throw new ApplicationException(TextCodesTranslator.TranslateText("LedgerTransaction.O.AgingMonthNotSet", tenant, LoggedContactResolver.GetLoggedContactShowLocal(tenant)));

            var monthsBackwards = settings.NumberOfAgingMonths ?? 0;
            return (uint)monthsBackwards;
        }

        private FullAccountingSetting GetFullAccountingSettings()
        {
            FullAccountingSettingRepository settingRepository = new FullAccountingSettingRepository(tenant);
            FullAccountingSetting settings = settingRepository.GetSingleFullAccountingSetting(tenant);
            return settings;
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
                if (filterItem.FieldDataType == "decimal")
                {
                    decimal value = Convert.ToDecimal(filterItem.FieldValue);
                    object x = value;

                    return (T)x;
                }
                else
                {
                    return (T)filterItem.FieldValue;
                }
            }

            return default(T);
        }

    }

}