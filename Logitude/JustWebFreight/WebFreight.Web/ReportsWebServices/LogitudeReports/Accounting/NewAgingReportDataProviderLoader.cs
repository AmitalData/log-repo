using CHAMP17;
using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools.Helpers;
using NPOI.SS.Formula.Functions;
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
            AgingDataLine= FilterByObligo(GetFilterValue<string>("Obligo"), AgingDataLine);
            AgingDataLine =FilterByBalance(GetFilterValue<string>("BalanceFilter"), AgingDataLine);

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
                    command.Parameters.AddWithValue("@GroupMultiAccounts", GetFilterValue<bool>("IsGroupMultiAccounts"));


                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            NewAgingPeriod result = new NewAgingPeriod
                            {
                                
                                AccountEnglishName = reader["EnglishName"] != DBNull.Value ? (string)reader["EnglishName"] : null,
                                AccountLocalName = reader["LocalName"] != DBNull.Value ? (string)reader["LocalName"] : null,
                               AccountDisplayNumber = reader["DisplayNumber"] != DBNull.Value ? (string)reader["DisplayNumber"] : null,
                                AccountVatNumber = reader["VatNumber"] != DBNull.Value ? (string)reader["VatNumber"] : null,
                                CreditLimit = reader["CreditLimit"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["CreditLimit"]) : 0,
                                InsuredCreditLimit = reader["InsuredCreditLimit"] != DBNull.Value ? (decimal?)reader["InsuredCreditLimit"] : 0,
                                TotalOpenShipments = reader[ "TotalOpenShipments"] != DBNull.Value ? (decimal?)reader["TotalOpenShipments"] : 0,
                              TotalFutureOpenCheques = reader["TotFutureOpenChequesInLocalCur"] != DBNull.Value ? (decimal?)reader["TotFutureOpenChequesInLocalCur"] : 0,
                                TotalPastOpenCheques = reader["TotPastOpenChequesInLocalCur"] != DBNull.Value ? (decimal?)reader["TotPastOpenChequesInLocalCur"] : 0,

                                ExternalTransactionsTotal = reader["ExternalTransactionsTotal"] != DBNull.Value ? (decimal?)reader["ExternalTransactionsTotal"] : 0,
                               AccountSalesmanName = reader["AccountSalesmanName"] != DBNull.Value ? (string)reader["AccountSalesmanName"] : null,
                              AccountSalesmanLocalName = reader["AccountSalesmanLocalName"] != DBNull.Value ? (string)reader["AccountSalesmanLocalName"] : null,
                               AccountCollectorName = reader["AccountCollectorName"] != DBNull.Value ? (string)reader["AccountCollectorName"] : null,
                                AccountCollectorLocalName = reader["AccountCollectorLocalName"] != DBNull.Value ? (string)reader["AccountCollectorLocalName"] : null,
                                ContactPhoneOrEmail = reader["ContactPhoneOrEmail"] != DBNull.Value ? (string)reader["ContactPhoneOrEmail"] : null,
                                ContactLocalName = reader["ContactLocalName"] != DBNull.Value ? (string)reader["ContactLocalName"] : null,
                                ContactEnglishName = reader["ContactEnglishName"] != DBNull.Value ? (string)reader["ContactEnglishName"] : null,
                                MinimumInterestInvoiceBilling = reader["MinimumInterestInvoiceBilling"] != DBNull.Value ? (string)reader["MinimumInterestInvoiceBilling"] : null,
                                CreditAllotmentPercentage = reader["CreditAllotmentPercentage"] != DBNull.Value ? (string)reader["CreditAllotmentPercentage"] : null,
                                PaymentTermLocalName = reader["PaymnetTermLocalName"] != DBNull.Value ? (string)reader["PaymnetTermLocalName"] : null,
                                PaymentTermEnglishName = reader["PaymnetTermEnglishName"] != DBNull.Value ? (string)reader["PaymnetTermEnglishName"] : null,
                                StandardInterestRateBaseLocalName = reader["StandardInterestRateBaseLocalName"] != DBNull.Value ? (string)reader["StandardInterestRateBaseLocalName"] : null,
                                StandardAddInterestPercent = reader["StandardAddInterestPercent"] != DBNull.Value ? (decimal?)reader["StandardAddInterestPercent"] : null,
                                AccountCurrencyCode = reader["AccountCurrencyCode"] != DBNull.Value ? (string)reader["AccountCurrencyCode"] : null,
                               Minus30Days = reader["Minus30Days"] != DBNull.Value ? (decimal?)reader["Minus30Days"] : 0,
                                  Minus60Days = reader["Minus60Days"] != DBNull.Value ? (decimal?)reader["Minus60Days"] : 0,
                              Minus90Days = reader["Minus90Days"] != DBNull.Value ? (decimal?)reader["Minus90Days"] : 0,
                                  Minus120Days = reader["Minus120Days"] != DBNull.Value ? (decimal?)reader["Minus120Days"] : 0,
                                Minus150Days = reader["Minus150Days"] != DBNull.Value ? (decimal?)reader["Minus150Days"] : 0,
                                Minus180Days = reader["Minus180Days"] != DBNull.Value ? (decimal?)reader["Minus180Days"] : 0,
                                 Past = reader["Past"] != DBNull.Value ? (decimal?)reader["Past"] : 0,
                                Plus30Days = reader["Plus30Days"] != DBNull.Value ? (decimal?)reader["Plus30Days"] : 0,
                                Plus60Days = reader["Plus60Days"] != DBNull.Value ? (decimal?)reader["Plus60Days"] : 0,
                                  Plus90Days = reader["Plus90Days"] != DBNull.Value ? (decimal?)reader["Plus90Days"] : 0,
                                Future = reader["Future"] != DBNull.Value ? (decimal?)reader["Future"] : 0,
                                BalanceInLocalCurrency = reader["BalanceInLocalCurrency"] != DBNull.Value ? (decimal?)reader["BalanceInLocalCurrency"] : 0,
                                AccountingBalance = reader["BalanceInForeignCurrency"] != DBNull.Value ? (decimal?)reader["BalanceInForeignCurrency"] : 0,
                                TotalForeign = reader["TotalForeign"] != DBNull.Value ? (decimal?)reader["TotalForeign"] : 0,
                                TotalToCollect = reader["TotalToCollect"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["TotalToCollect"]) : 0,

                            };
                            result.TotalLocal = result.BalanceInLocalCurrency;
                            results.Add(result);
                        }
                    }
                    connection.Close();
                }

                return results;
            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError($"Failed to execute usp_NewAgingReport: {ex.Message}");
                throw ex;
            }
        }


        private List<NewAgingPeriod> FilterByObligo(string obligoOperator, List<NewAgingPeriod> agingDataLine)
        {
            switch (obligoOperator)
            {
                case "LargerThan":
                    return agingDataLine.Where(line => line.Obligo > 0).ToList();
                case "LessThan":
                    return agingDataLine.Where(line => line.Obligo < 0).ToList();
                   
                case "NotEqual":
                    return agingDataLine.Where(line => line.Obligo != 0).ToList();
                default:
                    return agingDataLine;
            }
        }
        private List<NewAgingPeriod> FilterByBalance(string balanceFilter, List<NewAgingPeriod> agingDataLine)
        {
            var ToBalanceFilterValue=GetFilterValue<decimal>("ToBalanceFilterValue");
            var FromBalanceFilterValue = GetFilterValue<decimal>("FromBalanceFilterValue");

            switch (balanceFilter)
            {
               
                case "Debtors":
                    return agingDataLine.Where(line =>SumOfPastBalance(line) > 0).ToList();
                case "DebtBetween":
                 return agingDataLine.Where(line => SumOfPastBalance(line) > FromBalanceFilterValue && SumOfPastBalance(line) < ToBalanceFilterValue).ToList();
                case "BalanceDiffersFrom0":
                    return agingDataLine.Where(line => SumOfPastBalance(line) != 0).ToList();
                default:
                    return agingDataLine;
            }

        }
        public decimal? SumOfPastBalance(NewAgingPeriod agingDataLine)
        {
            return (agingDataLine.Minus180Days ?? 0) +
                   (agingDataLine.Minus150Days ?? 0) +
                   (agingDataLine.Minus120Days ?? 0) +
                   (agingDataLine.Minus90Days ?? 0) +
                   (agingDataLine.Minus60Days ?? 0) +
                   (agingDataLine.Minus30Days ?? 0) +
                   (agingDataLine.Past ?? 0);
        }

        public decimal? SumOfBalance(NewAgingPeriod agingDataLine)
        {
            return (agingDataLine.Future ?? 0) +
                   (agingDataLine.Plus90Days ?? 0) +
                   (agingDataLine.Plus60Days ?? 0) +
                   (agingDataLine.Plus30Days ?? 0) +
                   (agingDataLine.Past ?? 0) +
                   (agingDataLine.Minus180Days ?? 0) +
                   (agingDataLine.Minus150Days ?? 0) +
                   (agingDataLine.Minus120Days ?? 0) +
                   (agingDataLine.Minus90Days ?? 0) +
                   (agingDataLine.Minus60Days ?? 0) +
                   (agingDataLine.Minus30Days ?? 0);
        }
        private List<NewAgingPeriod> SortAccountingAgingDataLines(List<NewAgingPeriod> agingDataLine)
        {
            string sortField = GetFilterValue<string>("SortField");
            string sortDirection = GetFilterValue<string>("SortDirection");

            switch (sortField)
            {
                case "balance":
                    return SortByField(agingDataLine, sortDirection, d => d.AccountingBalance);
                case "customer":
                    return SortByField(agingDataLine, sortDirection, d => d.AccountEnglishName);
                case "TotalToCollect":
                    return SortByField(agingDataLine, sortDirection, d => d.TotalToCollect);
                case "Obligo":
                    return SortByField(agingDataLine, sortDirection, d => d.Obligo);
                case "CreditUsed":
                    return SortByField(agingDataLine, sortDirection, d => d.CreditUsed);
                default:
                    return DefaultSort(agingDataLine);
            }
        }

        private List<NewAgingPeriod> DefaultSort(List<NewAgingPeriod> agingDataLine)
        {
            return agingDataLine.OrderBy(d => d.AccountEnglishName).ToList();
        }
             
        private List<NewAgingPeriod> SortByField<TKey>(List<NewAgingPeriod> agingDataLine, string sortDirection, Func<NewAgingPeriod, TKey> keySelector)
        {
            return sortDirection == "Descending"
            ? agingDataLine.OrderByDescending(keySelector).ToList()
            : agingDataLine.OrderBy(keySelector).ToList();
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