using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
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
using WebFreight.Web.Security;

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
            reportQueryOperations = DeserializeQueryOperationFromXml(xmlFilters);

            EnsureSalesmanIdIfSalesmanRestricted(BuildReportParameters());

            AgingReportService agingReportService = new AgingReportService(BuildReportParameters());
            agingReportService.RunReport();
            List<PeriodMExtended> resultedPeriods = agingReportService.MyPeriodExtendedList;

            return BuildDataProvider(resultedPeriods);
        }

        //-----------

        private AccountingAgingDataProvider BuildDataProvider(List<PeriodMExtended> resultedPeriods)
        {
            List<PeriodMExtended> filteredPeriods = FilterPeriods(resultedPeriods);

            AccountingAgingDataProvider dataProvider = new AccountingAgingDataProvider();

            dataProvider.Month = GetFilterValue<DateTime>("AgingForDate");
            dataProvider.PrintedByUser = GetLoggedContactName();
            dataProvider.CustomerFilterValue = GetCustomerFilterTitle();
            dataProvider.AgingPeriods = BuildAgingPeriods(filteredPeriods);

            SetLocalCurrency(dataProvider);
            AddTotalBalancePeriods(filteredPeriods, dataProvider);
            AddTotalLocalBalancePeriods(filteredPeriods, dataProvider);
            CalculateReportLocalBalanceTotal(dataProvider);

            //FilterCustomerPeriodsOnBalance(dataProvider);
            FixSplitAccountData(dataProvider);

            SetOrderForPeriods(filteredPeriods, dataProvider);
            SetPeriodsTotal(filteredPeriods, dataProvider);

            ResharpPeriodsName(dataProvider);

            return dataProvider;
        }
        private void EnsureSalesmanIdIfSalesmanRestricted(AgingReportParam args)
        {
            bool isSalsmanRestrictionsEnabled = SecurityUtility.CheckFeature("GLAccount", "SalesmanAging", tenant);
            UserPM loggedUser = GetLoggerUser();

            if (isSalsmanRestrictionsEnabled && loggedUser?.IsSalesman == true && args.SalesmanId == null)
                throw new ApplicationException(TextCodesTranslator.TranslateText("GLAccount.O.NoSalesman", args.Tenant, LoggedContactResolver.GetLoggedContactShowLocal(tenant)));

        }

        private UserPM GetLoggerUser()
        {
            UserPM loggedUser;
            UserQuery userQuery = new UserQuery(tenant);
            if (AuthenticationUtil.AuthenticatedUserEmail != null)
            { // user set and passed from from WR
                loggedUser = userQuery.GetSinglePMByEmail(AuthenticationUtil.AuthenticatedUserEmail, tenant);
            }
            else
            {
                ContactPM loggedContact = LoggedContactResolver.GetLoggedContact(tenant);
                loggedUser = userQuery.GetSinglePM(loggedContact.Id, tenant);
            }
            return loggedUser;
        }

        private List<PeriodMExtended> FilterPeriods(List<PeriodMExtended> resultedPeriods)
        {
            string groupBy = GetFilterValue<string>("GroupByDate"); // filter_Due, filter_Accounting
            string filterBy = GetFilterValue<string>("BalanceFilter"); // Debtors, DebtAbove, filter_All
            decimal balanceFilterAmount = Convert.ToDecimal(GetFilterValue<decimal>("BalanceFilterValue"));

            if (groupBy == "filter_Due")
            {
                if(filterBy == "Debtors")
                    resultedPeriods = resultedPeriods.Where(d => d.LocalBalanceInDue > 0).ToList();
                else if (filterBy == "DebtAbove")
                    resultedPeriods = resultedPeriods.Where(d => d.LocalBalanceInDue >= balanceFilterAmount).ToList();

            }
            else
            {
                if (filterBy == "Debtors")
                    resultedPeriods = resultedPeriods.Where(d => d.BalanceInLocalCurrency > 0).ToList();
                else if (filterBy == "DebtAbove")
                    resultedPeriods = resultedPeriods.Where(d => d.BalanceInLocalCurrency >= balanceFilterAmount).ToList();

            }

            return resultedPeriods;
        }

        private void FixSplitAccountData(AccountingAgingDataProvider totalData)
        {
            bool showDetailedCurrencyAccounts = GetFilterValue<bool>("Detailed");
            if (showDetailedCurrencyAccounts)
            {
                foreach (var period in totalData.AgingPeriods)
                {
                    if (period.CurrencyCode != null)
                    {
                        if (period.CurrencyCode != period.AccountCurrencyCode)
                            period.AccountDisplayNumber += "/" + period.CurrencyCode;
                        period.AccountCurrencyCode = period.CurrencyCode;
                    }
                }
            }
        }
        private void FilterCustomerPeriodsOnBalance(AccountingAgingDataProvider totalData)
        {
            //if (GetFilterValue<string>("GroupByDate") != "filter_Due")
            //{
            //    List<AgingPeriod> totalBalances = GetTotalBalancePeriods(totalData, showLocals);

            //    var balanceFilterAmount = GetFilterValue<decimal>("BalanceFilterValue");

            //    foreach (var totalBalance in totalBalances)
            //    {
            //        if (GetFilterValue<string>("BalanceFilter") == "Debtors" && !(totalBalance.Total > 0))
            //            RemoveCustomerPeriods(totalData, totalBalance);
            //        else if (GetFilterValue<string>("BalanceFilter") == "DebtAbove" && !(totalBalance.Total >= Convert.ToDecimal(balanceFilterAmount)))
            //            RemoveCustomerPeriods(totalData, totalBalance);
            //    }
            //}

        }

        private void RemoveCustomerPeriods(AccountingAgingDataProvider totalData, AgingPeriod totalBalance)
        {
            var name = totalBalance.AccountLocalName != null ? totalBalance.AccountLocalName : totalBalance.AccountEnglishName;
            List<AgingPeriod> items = totalData.AgingPeriods.Where(d => d.AccountName == name).ToList();
            foreach (var item in items)
                totalData.AgingPeriods.Remove(item);
        }

        private void AddTotalBalancePeriods(List<PeriodMExtended> result, AccountingAgingDataProvider totalData)
        {
            List<AgingPeriod> groupedPeriodsByAccount;

            bool showDetailedCurrencyAccounts = GetFilterValue<bool> ("Detailed");
            if (showDetailedCurrencyAccounts)
                groupedPeriodsByAccount = result.Where(d=> true || d.CurrencyCode != totalData.TenantCurrencyCode).GroupBy(d => d.AccountAndCurr).Select(d => new AgingPeriod()
                {
                    PeriodName = showLocals ? "סיכום תקופות" : "Foreign",
                    Total = d.Sum(x => x.Total),
                    AccountName = (d.First().AccountLocalName != null ? d.First().AccountLocalName : d.First().AccountEnglishName) + " / " + d.First().CurrencyCode,
                    AccountLocalName = d.First().AccountLocalName + " / " + d.First().CurrencyCode,
                    AccountEnglishName = d.First().AccountEnglishName + " / " + d.First().CurrencyCode,
                    AccountCurrencyCode = d.Where(x => x.CurrencyCode != null).First().CurrencyCode,
                    AccountDisplayNumber = d.First().AccountCurrencyCode != d.First().CurrencyCode ?
                                d.First().AccountDisplayNumber + "/" + d.First().CurrencyCode : d.First().AccountDisplayNumber,
                    CustomerCreditLimit = (decimal)d.First().CreditLimitAmount,
                    CustomerVatNumber = d.First().CustomerVatNumber,
                    CustomerPaymentTerm = d.First().AccountTermLocalName,                    
                    GLAccountStandardInterestRate = d.First().GLAccountStandardInterestRate,
                    AccountSalesmanName = d.First().AccountSalesmanName,
                    AccountSalesmanLocalName = d.First().AccountSalesmanLocalName,
                    AccountCollectorName = d.First().AccountCollectorName,
                    AccountCollectorLocalName = d.First().AccountCollectorLocalName,
                    Category1Name = d.First().Category1Name,
                    Category2Name = d.First().Category2Name,
                    Category3Name = d.First().Category3Name,
                    Category4Name = d.First().Category4Name,
                    Category5Name = d.First().Category5Name,
                    Category6Name = d.First().Category6Name,
                    Category1LocalName = d.First().Category1LocalName,
                    Category2LocalName = d.First().Category2LocalName,
                    Category3LocalName = d.First().Category3LocalName,
                    Category4LocalName = d.First().Category4LocalName,
                    Category5LocalName = d.First().Category5LocalName,
                    Category6LocalName = d.First().Category6LocalName,


                    ChartOfAccountsLocalName = d.First().ChartOfAccountsLocalName,
                    ChartOfAccountsEnglishName = d.First().ChartOfAccountsEnglishName,
                    ChartOfAccountsTypeEnglishName = d.First().ChartOfAccountsTypeEnglishName,
                    ChartOfAccountsTypeLocalName = d.First().ChartOfAccountsTypeLocalName,


                }).ToList();
            else
                groupedPeriodsByAccount = result.GroupBy(d => d.AccountId).Select(d => new AgingPeriod()
                {
                    PeriodName = showLocals ? "סיכום תקופות" : "Foreign",
                    Total = d.Sum(x => x.Total),
                    AccountName = (d.First().AccountLocalName != null ? d.First().AccountLocalName : d.First().AccountEnglishName),
                    AccountLocalName = d.First().AccountLocalName,
                    AccountEnglishName = d.First().AccountEnglishName,
                    AccountCurrencyCode = d.First().AccountCurrencyCode,
                    AccountDisplayNumber = d.First().AccountDisplayNumber,
                    CustomerCreditLimit = (decimal)d.First().CreditLimitAmount,
                    CustomerVatNumber = d.First().CustomerVatNumber,
                    CustomerPaymentTerm = d.First().AccountTermLocalName,
                    GLAccountStandardInterestRate = d.First().GLAccountStandardInterestRate,
                    AccountSalesmanName = d.First().AccountSalesmanName,
                    AccountSalesmanLocalName = d.First().AccountSalesmanLocalName,
                    AccountCollectorName = d.First().AccountCollectorName,
                    AccountCollectorLocalName = d.First().AccountCollectorLocalName,
                    Category1Name = d.First().Category1Name,
                    Category2Name = d.First().Category2Name,
                    Category3Name = d.First().Category3Name,
                    Category4Name = d.First().Category4Name,
                    Category5Name = d.First().Category5Name,
                    Category6Name = d.First().Category6Name,
                    Category1LocalName = d.First().Category1LocalName,
                    Category2LocalName = d.First().Category2LocalName,
                    Category3LocalName = d.First().Category3LocalName,
                    Category4LocalName = d.First().Category4LocalName,
                    Category5LocalName = d.First().Category5LocalName,
                    Category6LocalName = d.First().Category6LocalName,



                    ChartOfAccountsLocalName = d.First().ChartOfAccountsLocalName,
                    ChartOfAccountsEnglishName = d.First().ChartOfAccountsEnglishName,
                    ChartOfAccountsTypeEnglishName = d.First().ChartOfAccountsTypeEnglishName,
                    ChartOfAccountsTypeLocalName = d.First().ChartOfAccountsTypeLocalName,

                }).ToList();
            totalData.AgingPeriods.AddRange(groupedPeriodsByAccount);
            
        }

        private void AddTotalLocalBalancePeriods(List<PeriodMExtended> result, AccountingAgingDataProvider totalData)
        {
            //var groupedPeriodsByAccount0 = result.GroupBy(d => d.AccountId);
            List<AgingPeriod> groupedPeriodsByAccount;
            bool showDetailedCurrencyAccounts = GetFilterValue<bool>("Detailed");
            if (showDetailedCurrencyAccounts) { 
                groupedPeriodsByAccount = result.Where(d => d.Total != null && d.CurrencyCode == totalData.TenantCurrencyCode).GroupBy(d => d.AccountAndCurr).Distinct().Select(d => new AgingPeriod()
                {
                    PeriodName = showLocals ? "יתרה בשח להיום" : "Local",
                    Total = d.Sum(x => x.Total),
                    AccountName = (d.First().AccountLocalName != null ? d.First().AccountLocalName : d.First().AccountEnglishName) + " / " + d.First().CurrencyCode,
                    AccountLocalName = d.First().AccountLocalName + " / " + d.First().CurrencyCode,
                    AccountEnglishName = d.First().AccountEnglishName + " / " + d.First().CurrencyCode,
                    AccountCurrencyCode = d.Where(x => x.CurrencyCode != null).First().CurrencyCode,
                    AccountDisplayNumber = d.First().AccountCurrencyCode != d.First().CurrencyCode ?
                                  d.First().AccountDisplayNumber + "/" + d.First().CurrencyCode : d.First().AccountDisplayNumber,
                    CustomerCreditLimit = (decimal)d.First().CreditLimitAmount,
                    CustomerVatNumber = d.First().CustomerVatNumber,
                    CustomerPaymentTerm = d.First().AccountTermLocalName,
                    GLAccountStandardInterestRate = d.First().GLAccountStandardInterestRate,
                    AccountSalesmanName = d.First().AccountSalesmanName,
                    AccountSalesmanLocalName = d.First().AccountSalesmanLocalName,
                    AccountCollectorName = d.First().AccountCollectorName,
                    AccountCollectorLocalName = d.First().AccountCollectorLocalName,
                    Category1Name = d.First().Category1Name,
                    Category2Name = d.First().Category2Name,
                    Category3Name = d.First().Category3Name,
                    Category4Name = d.First().Category4Name,
                    Category5Name = d.First().Category5Name,
                    Category6Name = d.First().Category6Name,
                    Category1LocalName = d.First().Category1LocalName,
                    Category2LocalName = d.First().Category2LocalName,
                    Category3LocalName = d.First().Category3LocalName,
                    Category4LocalName = d.First().Category4LocalName,
                    Category5LocalName = d.First().Category5LocalName,
                    Category6LocalName = d.First().Category6LocalName,

                    ChartOfAccountsLocalName = d.First().ChartOfAccountsLocalName,
                    ChartOfAccountsEnglishName = d.First().ChartOfAccountsEnglishName,
                    ChartOfAccountsTypeEnglishName = d.First().ChartOfAccountsTypeEnglishName,
                    ChartOfAccountsTypeLocalName = d.First().ChartOfAccountsTypeLocalName,



            }).ToList();
            totalData.AgingPeriods.AddRange(groupedPeriodsByAccount);
                groupedPeriodsByAccount = result.Where(d => d.Total != null && d.CurrencyCode != totalData.TenantCurrencyCode).GroupBy(d => d.AccountAndCurr).Distinct().Select(d => new AgingPeriod()
                {
                    PeriodName = showLocals ? "יתרה בשח" : "Local",
                    Total = d.First().BalanceInLocalCurrency,
                    AccountName = (d.First().AccountLocalName != null ? d.First().AccountLocalName : d.First().AccountEnglishName) + " / " + d.First().CurrencyCode,
                    AccountLocalName = d.First().AccountLocalName + " / " + d.First().CurrencyCode,
                    AccountEnglishName = d.First().AccountEnglishName + " / " + d.First().CurrencyCode,
                    AccountCurrencyCode = d.Where(x => x.CurrencyCode != null).First().CurrencyCode,
                    AccountDisplayNumber = d.First().AccountCurrencyCode != d.First().CurrencyCode ?
                                  d.First().AccountDisplayNumber + "/" + d.First().CurrencyCode : d.First().AccountDisplayNumber,
                    CustomerCreditLimit = (decimal)d.First().CreditLimitAmount,
                    CustomerVatNumber = d.First().CustomerVatNumber,
                    CustomerPaymentTerm = d.First().AccountTermLocalName,
                    GLAccountStandardInterestRate = d.First().GLAccountStandardInterestRate,
                    AccountSalesmanName = d.First().AccountSalesmanName,
                    AccountSalesmanLocalName = d.First().AccountSalesmanLocalName,
                    AccountCollectorName = d.First().AccountCollectorName,
                    AccountCollectorLocalName = d.First().AccountCollectorLocalName,
                    Category1Name = d.First().Category1Name,
                    Category2Name = d.First().Category2Name,
                    Category3Name = d.First().Category3Name,
                    Category4Name = d.First().Category4Name,
                    Category5Name = d.First().Category5Name,
                    Category6Name = d.First().Category6Name,
                    Category1LocalName = d.First().Category1LocalName,
                    Category2LocalName = d.First().Category2LocalName,
                    Category3LocalName = d.First().Category3LocalName,
                    Category4LocalName = d.First().Category4LocalName,
                    Category5LocalName = d.First().Category5LocalName,
                    Category6LocalName = d.First().Category6LocalName,

                    ChartOfAccountsLocalName = d.First().ChartOfAccountsLocalName,
                    ChartOfAccountsEnglishName = d.First().ChartOfAccountsEnglishName,
                    ChartOfAccountsTypeEnglishName = d.First().ChartOfAccountsTypeEnglishName,
                    ChartOfAccountsTypeLocalName = d.First().ChartOfAccountsTypeLocalName,

                }).ToList();
            }
            else
                groupedPeriodsByAccount = result.Where(d => d.Total != null).GroupBy(d => d.AccountId).Distinct().Select(d => new AgingPeriod()
                {
                    PeriodName = showLocals ? "יתרה בשח" : "Local",
                    Total = d.FirstOrDefault() == null ? 0 : d.FirstOrDefault().BalanceInLocalCurrency,
                    AccountName = (d.First().AccountLocalName != null ? d.First().AccountLocalName : d.First().AccountEnglishName),
                    AccountLocalName = d.First().AccountLocalName,
                    AccountEnglishName = d.First().AccountEnglishName ,
                    AccountCurrencyCode = d.First().AccountCurrencyCode,
                    AccountDisplayNumber = d.First().AccountDisplayNumber,
                    CustomerCreditLimit = (decimal)d.First().CreditLimitAmount,
                    CustomerVatNumber = d.First().CustomerVatNumber,
                    CustomerPaymentTerm = d.First().AccountTermLocalName,
                    GLAccountStandardInterestRate = d.First().GLAccountStandardInterestRate,
                    AccountSalesmanName = d.First().AccountSalesmanName,
                    AccountSalesmanLocalName = d.First().AccountSalesmanLocalName,
                    AccountCollectorName = d.First().AccountCollectorName,
                    AccountCollectorLocalName = d.First().AccountCollectorLocalName,
                    Category1Name = d.First().Category1Name,
                    Category2Name = d.First().Category2Name,
                    Category3Name = d.First().Category3Name,
                    Category4Name = d.First().Category4Name,
                    Category5Name = d.First().Category5Name,
                    Category6Name = d.First().Category6Name,
                    Category1LocalName = d.First().Category1LocalName,
                    Category2LocalName = d.First().Category2LocalName,
                    Category3LocalName = d.First().Category3LocalName,
                    Category4LocalName = d.First().Category4LocalName,
                    Category5LocalName = d.First().Category5LocalName,
                    Category6LocalName = d.First().Category6LocalName,

                    ChartOfAccountsLocalName = d.First().ChartOfAccountsLocalName,
                    ChartOfAccountsEnglishName = d.First().ChartOfAccountsEnglishName,
                    ChartOfAccountsTypeEnglishName = d.First().ChartOfAccountsTypeEnglishName,
                    ChartOfAccountsTypeLocalName = d.First().ChartOfAccountsTypeLocalName,

                }).ToList();

            totalData.AgingPeriods.AddRange(groupedPeriodsByAccount);

        }
        private void AddTotalSummationFooterPeriod(List<PeriodMExtended> result, AccountingAgingDataProvider totalData)
        {
            string pname = showLocals ? "יתרה בשח" : "Local";
            decimal? summation = totalData.AgingPeriods.Where(d=>d.PeriodName == pname).Sum(d => d.Total);

            string totalLabel = showLocals ? "Local Total" : "Totals";
            totalData.AgingPeriods.Add(new AgingPeriod()
            {
                AccountEnglishName = totalLabel,
                AccountLocalName = totalLabel,
                AccountName = totalLabel,
                Total = summation??0,
                PeriodName = pname,
                OrderIndex = 99999999,
            });

        }
        private void CalculateReportLocalBalanceTotal (AccountingAgingDataProvider totalData)
        {
            string pname = showLocals ? "יתרה בשח" : "Local";
            decimal? summation = totalData.AgingPeriods.Where(d => d.PeriodName == pname).Sum(d => d.Total);

            totalData.ReportLocalBalanceTotal = summation ?? 0;

        }
        private void SetLocalCurrency(AccountingAgingDataProvider totalData)
        {
            TenantQuery tenantQuery = new TenantQuery(tenant);
            var tenantPM = tenantQuery.GetSinglePM(tenant);

            totalData.TenantCurrencyCode = tenantPM.CurrencyCode;
            totalData.TenantCurrencySign = tenantPM.CurrencySign;


        }


        private static List<AgingPeriod> GetTotalBalancePeriods(AccountingAgingDataProvider totalData, bool showLocals)
        {
            var balancePeriod = showLocals ? "סיכום תקופות" : "Foreign";
            List<AgingPeriod> totalBalances = totalData.AgingPeriods.Where(d => d.PeriodName == balancePeriod).ToList();
            return totalBalances;
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

                if (item.PeriodName.Contains("FutureAmount"))
                    item.PeriodName = item.PeriodName.Replace("FutureAmount", showLocals ? "סיכום תקופות עתידיות" : "FutureAmount");
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
            if (GetFilterValue<bool>("Detailed") == true)
            {
                foreach (PeriodMExtended item in result)
                {
                    AgingPeriod record = new AgingPeriod();

                    record.PeriodName = item.PeriodName;
                    record.AccountName = (item.AccountLocalName != null ? item.AccountLocalName : item.AccountEnglishName) + " / " + item.CurrencyCode;
                    record.AccountEnglishName = item.AccountEnglishName + " / " + item.CurrencyCode;
                    record.AccountLocalName = item.AccountLocalName + " / " + item.CurrencyCode;
                    record.CurrencyCode = item.CurrencyCode;
                    record.AccountDisplayNumber = item.AccountDisplayNumber;
                    record.AccountCurrencyCode = item.AccountCurrencyCode;
                    record.CustomerCreditLimit = (decimal)item.CreditLimitAmount;
                    record.CustomerVatNumber = item.CustomerVatNumber;
                    record.CustomerPaymentTerm = item.AccountTermLocalName;
                    record.GLAccountStandardInterestRate = item.GLAccountStandardInterestRate;

                    record.ChartOfAccountsLocalName = item.ChartOfAccountsLocalName;
                    record.ChartOfAccountsEnglishName = item.ChartOfAccountsEnglishName;
                    record.ChartOfAccountsTypeEnglishName = item.ChartOfAccountsTypeEnglishName;
                    record.ChartOfAccountsTypeLocalName = item.ChartOfAccountsTypeLocalName;


                    record.AccountSalesmanName = item.AccountSalesmanName;
                    record.AccountSalesmanLocalName = item.AccountSalesmanLocalName;
                    record.AccountCollectorName = item.AccountCollectorName;
                    record.AccountCollectorLocalName = item.AccountCollectorLocalName;
                    record.Category1Name = item.Category1Name;
                    record.Category2Name = item.Category2Name;
                    record.Category3Name = item.Category3Name;
                    record.Category4Name = item.Category4Name;
                    record.Category5Name = item.Category5Name;
                    record.Category1LocalName = item.Category1LocalName;
                    record.Category2LocalName = item.Category2LocalName;
                    record.Category3LocalName = item.Category3LocalName;
                    record.Category4LocalName = item.Category4LocalName;
                    record.Category5LocalName = item.Category5LocalName;

                    record.Total = item.Total;


                    periods.Add(record);

                }
            }
            else
            {
                foreach (PeriodMExtended item in result)
                {
                    AgingPeriod record = new AgingPeriod();

                    record.PeriodName = item.PeriodName;
                    record.AccountName = item.AccountLocalName != null ? item.AccountLocalName : item.AccountEnglishName;
                    record.AccountEnglishName = item.AccountEnglishName;
                    record.AccountLocalName = item.AccountLocalName;
                    record.AccountDisplayNumber = item.AccountDisplayNumber;
                    record.AccountCurrencyCode = item.AccountCurrencyCode;
                    record.CustomerCreditLimit = (decimal)item.CreditLimitAmount;
                    record.CustomerVatNumber = item.CustomerVatNumber;
                    record.CustomerPaymentTerm = item.AccountTermLocalName;
                    record.GLAccountStandardInterestRate = item.GLAccountStandardInterestRate;

                    record.ChartOfAccountsLocalName = item.ChartOfAccountsLocalName;
                    record.ChartOfAccountsEnglishName = item.ChartOfAccountsEnglishName;
                    record.ChartOfAccountsTypeEnglishName = item.ChartOfAccountsTypeEnglishName;
                    record.ChartOfAccountsTypeLocalName = item.ChartOfAccountsTypeLocalName;

                    record.AccountSalesmanName = item.AccountSalesmanName;
                    record.AccountSalesmanLocalName = item.AccountSalesmanLocalName;
                    record.AccountCollectorName = item.AccountCollectorName;
                    record.AccountCollectorLocalName = item.AccountCollectorLocalName;
                    record.Category1Name = item.Category1Name;
                    record.Category2Name = item.Category2Name;
                    record.Category3Name = item.Category3Name;
                    record.Category4Name = item.Category4Name;
                    record.Category5Name = item.Category5Name;
                    record.Category1LocalName = item.Category1LocalName;
                    record.Category2LocalName = item.Category2LocalName;
                    record.Category3LocalName = item.Category3LocalName;
                    record.Category4LocalName = item.Category4LocalName;
                    record.Category5LocalName = item.Category5LocalName;

                    record.Total = item.Total;

                    periods.Add(record);

                }
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
            reportParameters.CollectorId = GetFilterValue<string>("CollectorId");
            reportParameters.SalesmanId = GetFilterValue<string>("SalesmanId");
            reportParameters.AggregateByGLAccountCurrencies = GetFilterValue<bool>("Detailed");
         
            reportParameters.GroupByDate = GetFilterValue<string>("GroupByDate") == "filter_Due" ? AgingReportParam.DateEnum.DueDate : AgingReportParam.DateEnum.AccountingDate;
            reportParameters.AgingMethod = GetFilterValue<string>("AgingMethod") == "Open Transaction" ? AgingReportParam.MethodEnum.ReconcileOpenBalanceMethod.ToString() : AgingReportParam.MethodEnum.TotalByMonthFIFOMethod.ToString();
            reportParameters.Aging4AccountTypeCode = (GetFilterValue<string>("GLAccountType") == "2") ? AgingReportParam.Aging4AccountTypeCodeEnum.Customer2 : AgingReportParam.Aging4AccountTypeCodeEnum.Vendor3;


            reportParameters.ChartOfAccountsTypeCode = GetFilterValue<string>("ChartOfAccountsTypeCode");
            reportParameters.ChartOfAccountsId = GetFilterValue<string>("ChartOfAccountId");


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