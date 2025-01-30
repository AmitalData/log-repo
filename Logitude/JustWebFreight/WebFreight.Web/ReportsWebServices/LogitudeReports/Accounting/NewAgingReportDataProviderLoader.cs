using CHAMP17;
using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.DataProviders;
using WebFreight.Web.Security;
using WebFreight.Web.Services;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.Accounting
{
    public class NewAgingReportDataProviderLoader
    {
        private const string BalanceInLocalCurrencyString = "Balance In Local Currency";
        private const string BalanceInLocalCurrencyHebrewString = "יתרה במטבע מקומי";
        private const string BalanceInForeignCurrencyString = "Balance In Foreign Currency";
        private const string BalanceInForeignCurrencyHebrewString = "יתרה במטבע זר";
        private const string SummaryPeriodsString = "Summary Periods";
        private const string SummaryPeriodsHebrewString = "סיכום תקופות";
        private QueryOperations reportQueryOperations;
        private bool showLocals = false;
        private bool isFromGLAccountAgingData = false;
        private int tenant;
        string filterReportByLocalCurrency = "filter_LocalCurr";

        public NewAgingReportDataProviderLoader(byte[] xmlFilters, int _tenant)
        {
            tenant = _tenant;
            showLocals = LoggedContactResolver.GetLoggedContactShowLocal(_tenant);
            reportQueryOperations = DeserializeQueryOperationFromXml(xmlFilters);

        }
        
        private NewAccountingAgingDataProvider BuildDataProvider()
        {
            NewAccountingAgingDataProvider dataProvider = new NewAccountingAgingDataProvider();

            return SetAccountingAgingDataLine(dataProvider);

        }
        private NewAccountingAgingDataProvider SetAccountingAgingDataLine(NewAccountingAgingDataProvider dataProvider)
        {
            List<AgingPeriod> AgingDataLine = GetAccountingAgingDataLineByFilter();
            dataProvider.ChartOfAccountLine = new List<ChartOfAccountLine>();
            for (int i = 0; i < chartOfAccountList?.Count(); i++)
            {
                QueryFilterItem DetailedForJobs = reportQueryOperations.QueryFilterItems.Where(d => d.FieldName == "DetailedForJobs").FirstOrDefault();
                List<MonthlyBalancesLine> monthlyBalancesLineOfChartOfAccount = monthlyBalancesLine.Where(a => a.ChartOfAccount == chartOfAccountList[i].Id).ToList();

                ChartOfAccountLine chartOfAccountLine =
                                new ChartOfAccountLine()
                                {
                                    QuantityForJanuary = monthlyBalancesLineOfChartOfAccount != null ? monthlyBalancesLineOfChartOfAccount.Sum(a => a.QuantityForJanuary) : 0,
                                    QuantityForFebruary = monthlyBalancesLineOfChartOfAccount != null ? monthlyBalancesLineOfChartOfAccount.Sum(a => a.QuantityForFebruary) : 0,
                                    QuantityForMarch = monthlyBalancesLineOfChartOfAccount != null ? monthlyBalancesLineOfChartOfAccount.Sum(a => a.QuantityForMarch) : 0,
                                    QuantityForApril = monthlyBalancesLineOfChartOfAccount != null ? monthlyBalancesLineOfChartOfAccount.Sum(a => a.QuantityForApril) : 0,
                                    QuantityForMay = monthlyBalancesLineOfChartOfAccount != null ? monthlyBalancesLineOfChartOfAccount.Sum(a => a.QuantityForMay) : 0,
                                    QuantityForJune = monthlyBalancesLineOfChartOfAccount != null ? monthlyBalancesLineOfChartOfAccount.Sum(a => a.QuantityForJune) : 0,
                                    QuantityForJuly = monthlyBalancesLineOfChartOfAccount != null ? monthlyBalancesLineOfChartOfAccount.Sum(a => a.QuantityForJuly) : 0,
                                    QuantityForAugust = monthlyBalancesLineOfChartOfAccount != null ? monthlyBalancesLineOfChartOfAccount.Sum(a => a.QuantityForAugust) : 0,
                                    QuantityForSeptember = monthlyBalancesLineOfChartOfAccount != null ? monthlyBalancesLineOfChartOfAccount.Sum(a => a.QuantityForSeptember) : 0,
                                    QuantityForOctober = monthlyBalancesLineOfChartOfAccount != null ? monthlyBalancesLineOfChartOfAccount.Sum(a => a.QuantityForOctober) : 0,
                                    QuantityForNovember = monthlyBalancesLineOfChartOfAccount != null ? monthlyBalancesLineOfChartOfAccount.Sum(a => a.QuantityForNovember) : 0,
                                    QuantityForDecember = monthlyBalancesLineOfChartOfAccount != null ? monthlyBalancesLineOfChartOfAccount.Sum(a => a.QuantityForDecember) : 0,
                                    TotalReport = monthlyBalancesLineOfChartOfAccount != null ? monthlyBalancesLineOfChartOfAccount.Sum(a => a.TotalReport) : 0,
                                    GLAcountLocalName = chartOfAccountList[i].LocalName,
                                    GLAcountNumber = chartOfAccountList[i].Code,
                                    GLAcountEnglishName = chartOfAccountList[i].EnglishName,
                                    MonthlyBalancesLine = (bool)DetailedForJobs?.FieldValue ? monthlyBalancesLineOfChartOfAccount : new List<MonthlyBalancesLine>()


                                };
                dataProvider.ChartOfAccountLine.Add(chartOfAccountLine);

            }

        }

        private List<NewAgingPeriod> GetAccountingAgingDataLineByFilter()
        {
            try
            {
                 List<NewAgingPeriod> results = new List<NewAgingPeriod>();
                string strConnString = GetConnection(0);
                using (SqlConnection connection = new SqlConnection(strConnString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "usp_NewAgingReport";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                   


                    command.Parameters.AddWithValue("@AccountID", GetFilterValue<string>( "CustomerId"));
                    command.Parameters.AddWithValue("@InputDate", GetFilterValue<DateTime>("AgingForDate"));
                    command.Parameters.AddWithValue("@ChartOfAccountsTypeCode", GetFilterValue<string>("ChartOfAccountsTypeCode"));
                    command.Parameters.AddWithValue("@AccountTypeCode", GetFilterValue<string>("GLAccountType"));
                    command.Parameters.AddWithValue("@CollectorId", GetFilterValue<string>("CollectorId"));
                    command.Parameters.AddWithValue("@SalesmanId", GetFilterValue<string>("SalesmanId"));
                    command.Parameters.AddWithValue("@ChartOfAccountsId", GetFilterValue<string>("ChartOfAccountsId"));
                    command.Parameters.AddWithValue("@CategoryName", GetFilterValue<string>("CategoryIndex"));
                    command.Parameters.AddWithValue("@CategoryValue", GetFilterValue<string>("CategoryValue"));
                    command.Parameters.AddWithValue("@CurrencyFilterSelectedValue", GetFilterValue<string>("CurrencyOriginalLocalValue"));
                    command.Parameters.AddWithValue("@CurrencyId", GetFilterValue<string>("CurrencyId"));

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            NewAgingPeriod result = new NewAgingPeriod
                            {
                                SalesmanId = reader["SalesmanUserId"] != DBNull.Value ? (string)reader["SalesmanUserId"] :null,
                                CollectorId = reader["CollectorId"] != DBNull.Value ? (string)reader["CollectorId"] : null,
                                CurrencyId = reader["CurrencyId"] != DBNull.Value ? (string)reader["CurrencyId"] : null,
                                AccountEnglishName = reader["AccountEnglishName"] != DBNull.Value ? (string)reader["AccountEnglishName"] : null,
                                AccountLocalName = reader["AccountLocalName"] != DBNull.Value ? (string)reader["AccountLocalName"] : null,
                                AccountDisplayNumber = reader["AccountDisplayNumber"] != DBNull.Value ? (string)reader["AccountDisplayNumber"] : null,

                            };
                            results.Add(result);
                        }
                    }
                    connection.Close();
                }

                return results;
            }

            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        private static string GetConnection(int tenant)
        {
            GlobalDB currentDb;

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
            {
                currentDb = GlobalDBRepository.GetGlobalDBByTenant(tenant);
                scope.Complete();
            }

            string dbConnectionInfo = currentDb.DBConnection;
            string dbSeconderyConnectionInfo = currentDb.SecondaryAzureDBConnection;

            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo, dbSeconderyConnectionInfo);
            WebFreightContext context = new WebFreightContext(connection);

            return context.Database.Connection.ConnectionString;
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
                if (filterItem.FieldDataType == "decimal" || (filterItem.FieldValue2 != null && filterItem.FieldValue2.ToString() == "decimal"))
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
        public decimal? GetNullableDecimalFilterValue(string FieldName)
        {
            QueryFilterItem filterItem = reportQueryOperations.QueryFilterItems
                .Where(d => d.FieldName == FieldName).FirstOrDefault();



            if (filterItem != null && filterItem.FieldValue != null)
            {
                    return Convert.ToDecimal(filterItem.FieldValue);
            }

            return null;
        }

    }

}