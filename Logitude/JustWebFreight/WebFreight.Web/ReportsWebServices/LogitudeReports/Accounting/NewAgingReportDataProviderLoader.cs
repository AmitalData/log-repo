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
     
        private QueryOperations reportQueryOperations;
        private bool showLocals = false;
        private int tenant;

        public NewAgingReportDataProviderLoader(byte[] xmlFilters, int _tenant)
        {
            tenant = _tenant;
            showLocals = LoggedContactResolver.GetLoggedContactShowLocal(_tenant);
            reportQueryOperations = DeserializeQueryOperationFromXml(xmlFilters);

        }
        
        public NewAccountingAgingDataProvider BuildDataProvider()
        {
            NewAccountingAgingDataProvider dataProvider = new NewAccountingAgingDataProvider();

            return SetAccountingAgingDataLine(dataProvider);

        }
        private NewAccountingAgingDataProvider SetAccountingAgingDataLine(NewAccountingAgingDataProvider dataProvider)
        {
            List<NewAgingPeriod> AgingDataLine = GetAccountingAgingDataLineByFilter();
            FilterByObligo(GetFilterValue<string>("Obligo"), AgingDataLine);
            FilterByBalance(GetFilterValue<string>("BalanceFilter"), AgingDataLine);

            dataProvider.AgingPeriods = SortAccountingAgingDataLines(AgingDataLine); 
           
            return dataProvider;
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
                    command.Parameters.AddWithValue("@Tenant", tenant);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            NewAgingPeriod result = new NewAgingPeriod
                            {
                                SalesmanId = reader["SalesmanUserId"] != DBNull.Value ? (string)reader["SalesmanUserId"] :null,
                                CollectorId = reader["CollectorId"] != DBNull.Value ? (string)reader["CollectorId"] : null,
                                CurrencyId = reader["CurrencyId"] != DBNull.Value ? (string)reader["CurrencyId"] : null,
                                AccountEnglishName = reader["EnglishName"] != DBNull.Value ? (string)reader["EnglishName"] : null,
                                AccountLocalName = reader["LocalName"] != DBNull.Value ? (string)reader["LocalName"] : null,
                                AccountDisplayNumber = reader["DisplayNumber"] != DBNull.Value ? (string)reader["DisplayNumber"] : null,
                                BalanceInLocalCurrency = reader["BalanceInLocalCurrency"] != DBNull.Value ? (decimal?)reader["BalanceInLocalCurrency"] : null,
                                CreditLimit = reader["CreditLimit"] != DBNull.Value ? (decimal?)reader["CreditLimit"] : null,
                                InsuredCreditLimit = reader["InsuredCreditLimit"] != DBNull.Value ? (double?)reader["InsuredCreditLimit"] : null,
                               // AccountingBalance = reader[ "AccountingBalance"] != DBNull.Value ? (decimal?)reader["AccountingBalance"] : null,
                                TotalOpenShipments = reader[ "TotalOpenShipments"] != DBNull.Value ? (decimal?)reader["TotalOpenShipments"] : null,
                                TotalFutureOpenCheques = reader["TotFutureOpenChequesInLocalCur"] != DBNull.Value ? (decimal?)reader["TotFutureOpenChequesInLocalCur"] : null,
                               ExternalTransactionsTotal= reader["ExternalTransactionsTotal"] != DBNull.Value ? (decimal?)reader["ExternalTransactionsTotal"] : null,
                                TotalLocal= reader["BalanceInLocalCurrency"] != DBNull.Value ? (decimal?)reader["BalanceInLocalCurrency"] : null,
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


        private void FilterByObligo(string obligoOperator, List<NewAgingPeriod> agingDataLine)
        {
            switch (obligoOperator)
            {
                case "GreaterThan":
                    agingDataLine = agingDataLine.Where(line => line.Obligo > 0).ToList();
                    break;
                case "LessThan":
                    agingDataLine = agingDataLine.Where(line => line.Obligo < 0).ToList();
                    break;
                case "NotEqual":
                    agingDataLine = agingDataLine.Where(line => line.Obligo != 0).ToList();
                    break;
                default:
                    break;
            }
        }
        private void FilterByBalance(string balanceFilter, List<NewAgingPeriod> agingDataLine)
        {
           
        }

        private List<NewAgingPeriod> SortAccountingAgingDataLines(List<NewAgingPeriod> agingDataLine)
        {
            string sortField = GetFilterValue<string>("SortField");
            string sortDirection = GetFilterValue<string>("SortDirection");

            if (sortField == "balance")
               return SortByBalance(sortDirection,agingDataLine);
            else if (sortField == "customer")
                return SortByCustoemrName(sortDirection, agingDataLine);
            else if (sortField == "TotalToCollect")
                return  SortByTotalToCollectAmount(sortDirection, agingDataLine);
            else if (sortField == "Obligo")
                return SortByObligoField(sortDirection, agingDataLine);
            else if (sortField == "CreditUsed")
                return SortByUsedCreditAmount(sortDirection, agingDataLine);
            else
                return DefaultSort(agingDataLine);
        }

        private List<NewAgingPeriod> DefaultSort(List<NewAgingPeriod> agingDataLine)
        {
            return agingDataLine.OrderBy(d => d.AccountEnglishName).ToList();
        }

        private List<NewAgingPeriod> SortByUsedCreditAmount(string sortDirection, List<NewAgingPeriod> agingDataLine)
        {
            if (sortDirection == "Descending")
                return agingDataLine.OrderByDescending(d => d.CreditUsed).ToList();
            else
                return agingDataLine.OrderBy(d => d.CreditUsed).ToList();
        }

        private List<NewAgingPeriod> SortByObligoField(string sortDirection, List<NewAgingPeriod> agingDataLine)
        {
            if (sortDirection == "Descending")
                return agingDataLine.OrderByDescending(d => d.Obligo).ToList();
            else
                return agingDataLine.OrderBy(d => d.Obligo).ToList();
        }

        private List<NewAgingPeriod> SortByTotalToCollectAmount(string sortDirection, List<NewAgingPeriod> agingDataLine)
        {
            if (sortDirection == "Descending")
                return agingDataLine.OrderByDescending(d => d.TotalToCollect).ToList();
            else
                return agingDataLine.OrderBy(d => d.TotalToCollect).ToList();
        }

        private List<NewAgingPeriod> SortByCustoemrName(string sortDirection, List<NewAgingPeriod> agingDataLine)
        {
            if (sortDirection == "Descending")
                return agingDataLine.OrderByDescending(d => d.AccountEnglishName).ToList();
            else
                return agingDataLine.OrderBy(d => d.AccountEnglishName).ToList();
        }

        private List<NewAgingPeriod> SortByBalance(string sortDirection, List<NewAgingPeriod> agingDataLine)
        {
            if (sortDirection == "Descending")
                return agingDataLine.OrderByDescending(d => d.AccountingBalance).ToList();
            else
                return agingDataLine.OrderBy(d => d.AccountingBalance).ToList();
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