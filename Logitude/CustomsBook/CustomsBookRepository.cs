using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using System.Xml.Schema;

namespace CustomsBook
{
    internal class CustomsBookRepository
    {
        static string sqlConnectionString = ConfigurationManager.ConnectionStrings["LogitudeStr"].ConnectionString;
        static readonly Logger logger = Program.logger;

        public static void TruncateTables()
        {
            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
            {
                try
                {
                    sqlConnection.Open();

                    using (SqlCommand command = new SqlCommand("dbo.TruncateCustomsBookTables", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.ExecuteNonQuery();
                    }

                    logger.Debug($"Tables truncated successfully.");
                }
                catch (Exception ex)
                {
                    logger.Debug($"Error occurred: {ex.Message}");
                    throw ex;
                }
                finally
                {
                    sqlConnection.Close();
                }
            }

        }

        public static List<int> GetCustomsItemIdWithRules()
        {

            string query = "SELECT CustomsItemId FROM [dbo].[NewCustomsBookMainView] " +
                     "WHERE (ItemHierarchicLocationID = 1 OR ItemHierarchicLocationID = 2) AND Rules = 1";

            List<int> customsItemIds = new List<int>();

            using (SqlConnection connection = new SqlConnection(sqlConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customsItemIds.Add(reader.GetInt32(0));
                    }
                }
            }
            return customsItemIds;
        }

        public static int GetTenantFromCustomsSettings()
        {
            string query = "select top 1 TENANT from customs.CUSTOMSSETTINGS where CUSTOMSAGENTID is not null";
            int tenant = 0;
            using (SqlConnection connection = new SqlConnection(sqlConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tenant = reader.GetInt32(0);
                    }
                }
            }
            return tenant;
        }


        public static void SwapTempToMainTable(string tempTableName)
        {

            string sqlTableName = tempTableName.Replace("TEMP_", "");

            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
            {
                sqlConnection.Open();

                string swapQuery = $@"
                        BEGIN TRANSACTION;

                        IF OBJECT_ID('customs.{sqlTableName}') IS NOT NULL
                        BEGIN
                            EXEC sp_rename 'customs.{sqlTableName}', '{sqlTableName}_Old';
                        END

                        IF OBJECT_ID('customs.{tempTableName}') IS NOT NULL
                        BEGIN
                            EXEC sp_rename 'customs.{tempTableName}', '{sqlTableName}';
                        END

                        IF OBJECT_ID('customs.{sqlTableName}_Old') IS NOT NULL
                        BEGIN
                            EXEC sp_rename 'customs.{sqlTableName}_old', '{tempTableName}';
                        END

                        COMMIT TRANSACTION;";

                try
                {
                    using (SqlCommand swapCommand = new SqlCommand(swapQuery, sqlConnection))
                    {
                        swapCommand.ExecuteNonQuery();

                    }
                    logger.Debug($"Table {tempTableName} Swap to {sqlTableName} successfully.");

                }
                catch (Exception ex)
                {
                    logger.Debug($"Error occurred: {ex.Message}");
                    throw ex;
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }

        public static List<string> GetColumnNames(string tableName)
        {
            List<string> columns = new List<string>();
            string tableNameAfterDot = tableName.Substring(tableName.LastIndexOf('.') + 1);

            using (SqlConnection con = new SqlConnection(sqlConnectionString))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand(@"SELECT COLUMN_NAME 
                                 FROM INFORMATION_SCHEMA.COLUMNS 
                                 WHERE TABLE_NAME = @yourtableName", con))
                {

                    com.Parameters.AddWithValue("@yourtableName", tableNameAfterDot);
                    using (SqlDataReader reader = com.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            string columnName = reader.GetString(0);
                            columns.Add(columnName);
                        }
                    }
                }
            }
            return columns;
        }


        public static void InsertDataToTempTable(DataTable dataTable, string tempTableName, List<string> sqlSchema, string sqlTableName)
        {
            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
            {
                try
                {
                    sqlConnection.Open();

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConnectionString))
                    {

                        bulkCopy.DestinationTableName = $"customs.{tempTableName}";
                        bulkCopy.BatchSize = 1000;
                        bulkCopy.BulkCopyTimeout = 600;

                        foreach (DataColumn column in dataTable.Columns)
                        {
                            string columnName = column.ColumnName;
                            if (columnName == "ID")
                            {
                                bulkCopy.ColumnMappings.Add(columnName, "CB_ID");
                            }
                            if (sqlSchema.Contains(columnName))
                            {
                                bulkCopy.ColumnMappings.Add(columnName, columnName);
                            }
                            else if (columnName == "Connected_CustomsItemDetailsHistoryID")
                            {
                                bulkCopy.ColumnMappings.Add(columnName, "Connect_CustItemDetailsHistID");
                            }
                            else if (columnName == "Valid_CustomsItemDetailsHistoryID")
                            {
                                bulkCopy.ColumnMappings.Add(columnName, "CustomsItemDetailsHistoryID");
                            }
                            else if (columnName == "Valid_PropertiesDetailsHistoryID")
                            {
                                bulkCopy.ColumnMappings.Add(columnName, "PropertiesDetailsHistoryID");
                            }
                            else if (columnName == "CI_CustomsItemHierarchicLocationIDNum")
                            {
                                bulkCopy.ColumnMappings.Add(columnName, "ItemHierarchicLocationID");
                            }
                            else if (columnName == "CIH_CustomsItemEntityStatusIDNum")
                            {
                                bulkCopy.ColumnMappings.Add(columnName, "CustomsItemEntityStatusIDNum");
                            }
                            else if (columnName == "ValidQuotaDetailsHistoryID")
                            {
                                bulkCopy.ColumnMappings.Add(columnName, "ValidQuotaDetailsHistoryID");
                            }
                            if (columnName == "IsVoluntaryOrImporterInBreachOfTrust")
                            {
                                bulkCopy.ColumnMappings.Add(columnName, "IsVoluntaryOrImporterOfTrust");
                            }
                            if (sqlTableName == "Customs.CB_TariffComputedDatas")
                            {
                                if (columnName == "WithoutQuota_ComputationMethodDataID")
                                {
                                    bulkCopy.ColumnMappings.Add(columnName, "WithoutQuota_ComputationID");
                                }
                                else if (columnName == "WithinQuota_ComputationMethodDataID")
                                {
                                    bulkCopy.ColumnMappings.Add(columnName, "WithinQuota_ComputationID");
                                }
                            }
                            else
                            {
                                if (columnName == "WithoutQuota_ComputationMethodDataID")
                                {
                                    bulkCopy.ColumnMappings.Add(columnName, "WithoutQuota_ComputMethDataID");
                                }
                                else if (columnName == "WithinQuota_ComputationMethodDataID")
                                {
                                    bulkCopy.ColumnMappings.Add(columnName, "WithinQuota_ComputMethDataID");
                                }
                            }
                        }

                        bulkCopy.WriteToServer(dataTable);
                    }
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    if (sqlConnection != null && sqlConnection.State == ConnectionState.Open)
                    {
                        sqlConnection.Close();
                    }
                }
            }
        }
    }
}
