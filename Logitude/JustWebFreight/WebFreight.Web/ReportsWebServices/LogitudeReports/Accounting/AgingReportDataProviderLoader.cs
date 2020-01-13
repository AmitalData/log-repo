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
    public class AgingReportDataProviderLoader
    {
        private QueryOperations reportQueryOperations;
        private bool showLocals = false;
        private int tenant;


        public AgingReportDataProviderLoader(int _tenant)
        {
            tenant = _tenant;
            showLocals = LoggedContactResolver.GetLoggedContactShowLocal(_tenant);

        }
        public AccountingAgingDataProvider LoadFromXML(byte[] xmlFilters)
        {
            reportQueryOperations = BuildQueryOperations(xmlFilters);

            AgingReportService agingReportService = new AgingReportService(BuildReportParameters());
            agingReportService.RunReport();
            List<PeriodMExtended> resultedPeriods = agingReportService.MyPeriodExtendedList;

            return BuildDataProvider(resultedPeriods);
        }

        //-----------

        private AccountingAgingDataProvider BuildDataProvider(List<PeriodMExtended> resultedPeriods)
        {
            AccountingAgingDataProvider dataProvider = new AccountingAgingDataProvider();

            dataProvider.Month = GetFilterValue<DateTime>("AgingForDate");
            dataProvider.PrintedByUser = GetLoggedContactName();
            dataProvider.CustomerFilterValue = GetCustomerFilterTitle();
            dataProvider.AgingPeriods = BuildAgingPeriods(resultedPeriods);

            CreateAndAppendTotalBalanceColumn(resultedPeriods, dataProvider);

            SetOrderForPeriods(resultedPeriods, dataProvider);

            SetPeriodsTotal(resultedPeriods, dataProvider);

            ResharpPeriodsName(dataProvider);

            //SetPeriodSummery(dataProvider);
            return dataProvider;
        }

        private void SetPeriodSummery(AccountingAgingDataProvider dataProvider)
        {
            if (dataProvider.AgingPeriods.Count > 0)
                dataProvider.AgingPeriods[0].Totals = new List<AgingPeriodTotal>();

            //totalData.AgingPeriods[0].Totals.Add(new AgingPeriodTotal() { TotalCredit = 111, TotalDebit = 222 });

            //totalData.AgingPeriods.Add(new AgingPeriod() { PeriodName = showLocals ? "סה''כ יתרה" : "Total Balance", Total = sum });
            //totalData.AgingPeriods.Add(new AgingPeriod() { PeriodName = showLocals ? "סה''כ יתרה" : "Total Balance", Total = sum, AccountEnglishName ="USD" });

            //fill credit / debit labels
            //bool even = true;
            //foreach (var item in totalData.AgingPeriods)
            //{
            //    if (item.PeriodName.Contains("Before") || item.PeriodName.Contains("Total Balance"))
            //        continue;

            //    if (even)
            //    {
            //        item.CreditOrDebit = "Credit";
            //        even = false;
            //    }
            //    else
            //    {
            //        item.CreditOrDebit = "Debit";
            //        even = true;
            //    }

            //}
        }

        private void ResharpPeriodsName(AccountingAgingDataProvider dataProvider)
        {
            foreach (var item in dataProvider.AgingPeriods)
            {
                if (item.PeriodName.Contains("b4"))
                    item.PeriodName = item.PeriodName.Replace("b4", showLocals ? "לפני" : "Before");
            }
        }

        private void SetPeriodsTotal(List<PeriodMExtended> resultedPeriods, AccountingAgingDataProvider dataProvider)
        {
            List<GroupedPeriodM> groupedArray =
                            resultedPeriods.GroupBy(l => l.PeriodName)
                            .Select(cl => new GroupedPeriodM { PeriodName = cl.First().PeriodName, GrandTotal = cl.Sum(c => c.Total), }).ToList();

            foreach (var period in groupedArray)
            {
                List<AgingPeriod> items = dataProvider.AgingPeriods.FindAll(a => a.PeriodName == period.PeriodName);
                items.ForEach((item) =>
                {
                    item.GrandTotal = period.GrandTotal;
                });
            }
        }

        private void SetOrderForPeriods(List<PeriodMExtended> resultedPeriods, AccountingAgingDataProvider dataProvider)
        {
            int i = 0;
            decimal sum = 0;
            foreach (var period in resultedPeriods.OrderBy(d => d.OrderDate).ToList())
            {
                i++;
                sum += period.Total;
                List<AgingPeriod> items = dataProvider.AgingPeriods.FindAll(a => a.PeriodName == period.PeriodName);
                items.ForEach((item) =>
                {
                    item.OrderIndex = i;
                });

            }
        }

        private void CreateAndAppendTotalBalanceColumn(List<PeriodMExtended> resultedPeriods, AccountingAgingDataProvider dataProvider)
        {
            Dictionary<PeriodMExtended, decimal> totalsDictionary = new Dictionary<PeriodMExtended, decimal>();
            foreach (var period in resultedPeriods)
            {
                totalsDictionary.Add(period, 0);
            }
            foreach (var period in resultedPeriods)
            {
                totalsDictionary[period] += period.Total;
            }
            foreach (var sumValue in totalsDictionary)
            {
                dataProvider.AgingPeriods.Add(new AgingPeriod()
                {
                    PeriodName = showLocals ? "סה''כ יתרה" : "Total Balance",
                    Total = sumValue.Value,
                    AccountName = sumValue.Key.AccountLocalName != null ? sumValue.Key.AccountLocalName : sumValue.Key.AccountEnglishName,

                });
            }
        }

        private List<AgingPeriod> BuildAgingPeriods(List<PeriodMExtended> result)
        {
            List<AgingPeriod> periods = new List<AgingPeriod>();
            foreach (PeriodMExtended item in result)
            {
                AgingPeriod record = new AgingPeriod();

                record.PeriodName = item.PeriodName;
                record.AccountName = item.AccountLocalName != null ? item.AccountLocalName : item.AccountEnglishName;

                record.Total = item.Total;

                periods.Add(record);
            }

            return periods;
        }

        private string GetCustomerFilterTitle()
        {
            var x = "";
            x = showLocals ? "לקוחות" : "All Customers";
            if (!string.IsNullOrEmpty(GetFilterValue<string>("CustomerId")))
            {
                GLAccountRepository glaccountRepository = new GLAccountRepository(tenant);
                GLAccount glaccount = glaccountRepository.GetSingle(GetFilterValue<string>("CustomerId"), tenant);
                x = glaccount.LocalName;
            }

            return x;
        }

        private string GetLoggedContactName()
        {
            ContactPM loggedContact = LoggedContactResolver.GetLoggedContact(tenant);
            var x = showLocals ? loggedContact.LocalName : loggedContact.EnglishName;
            return x;
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

            reportParameters.Tenant = tenant;
            reportParameters.AgingForDate = GetFilterValue<DateTime>("AgingForDate");
            reportParameters.NumberOfmonthsbackwards = Convert.ToUInt32(GetFilterValue<Int64>("NumberOfMonths"));
            reportParameters.VendorCustomerId = GetFilterValue<string>("CustomerId");
            reportParameters.CollectorId = GetFilterValue<string>("CollectoId");
            reportParameters.SalesmanId = GetFilterValue<string>("SalesmanId");
            reportParameters.AggregateByGLAccountCurrencies = GetFilterValue<bool>("Detailed");

            reportParameters.GroupByDate = GetFilterValue<string>("GroupByDate") == "filter_Due" ? AgingReportParam.DateEnum.DueDate : AgingReportParam.DateEnum.AccountingDate;
            reportParameters.AgingMethod = AgingReportParam.MethodEnum.ReconcileOpenBalanceMethod.ToString();
            reportParameters.Aging4AccountTypeCode = (GetFilterValue<string>("GLAccountType") == "2") ? AgingReportParam.Aging4AccountTypeCodeEnum.Customer2 : AgingReportParam.Aging4AccountTypeCodeEnum.Vendor3;

            SetReportCategoryParameters(reportParameters);

            return reportParameters;
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