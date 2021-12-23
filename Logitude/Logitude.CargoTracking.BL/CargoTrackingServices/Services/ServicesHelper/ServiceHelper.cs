using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.TableStructure.Helper;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.ServicesHelper
{
    public static class ServiceHelper
    {
        public static long TimeOut = 100000000000000000;

        public static bool GetIsIncrementalRunning(string connectionString)
        {
            bool result = false;

            SqlConnection connection = new SqlConnection(connectionString);

            SqlCommand getIsIncrementalBuildRunningcommand = new SqlCommand(
               "Select  IsIncrementalBuildRunning FROM dbo.Tenants Where Id = 0", connection);
            try
            {
                getIsIncrementalBuildRunningcommand.CommandTimeout = (int)TimeOut;
                connection.Open();
                result = ExecuteIsIncrementalRunningCommand(getIsIncrementalBuildRunningcommand);
                connection.Close();
            }
            catch (Exception ex)
            {
                connection.Close();
                throw ex;
            }
            return result;
        }

        private static bool ExecuteIsIncrementalRunningCommand(SqlCommand isIncrementalBuildRunningcommand)
        {
            bool result = false;
            using (SqlDataReader reader = isIncrementalBuildRunningcommand.ExecuteReader())
            {
                reader.Read();
                bool isRunning = false;
                var value = reader["IsIncrementalBuildRunning"];
                if (value != null)
                {
                    isRunning = (bool)(value);
                    if (isRunning != null) result = isRunning;

                }
            }
            return result;
        }

        public static string GetAutomaticLastUpdateDate(string tableName, string connectionString)
        {
            string result = null;

            SqlConnection connection = new SqlConnection(connectionString);

            SqlCommand getAutomaticLastUpdateDateCommand= new SqlCommand(
               "select MIN(AutomaticLastUpdateDate) -1 AutomaticLastUpdateDate " +
               "FROM dbo." + tableName + " ;", connection);

            try
            {
                getAutomaticLastUpdateDateCommand.CommandTimeout = (int)TimeOut;
                connection.Open();
                result = ExecuteAutomaticLastUpdateDateCommand(getAutomaticLastUpdateDateCommand);
                connection.Close();

            }
            catch (Exception ex)
            {
                connection.Close();
                throw ex;
            }
            return result;
        }

        private static string ExecuteAutomaticLastUpdateDateCommand(SqlCommand automaticLastUpdateDateCommand)
        {
            string result = "";
            using (SqlDataReader reader = automaticLastUpdateDateCommand.ExecuteReader())
            {
                reader.Read();
                DateTime? datetime = null;
                var value = reader["AutomaticLastUpdateDate"];
                if (value != null)
                {
                    if (!string.IsNullOrEmpty(value.ToString()))
                    {
                        datetime = (DateTime?)(value);
                        if (datetime != null) result = datetime.Value.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
                    }

                }
            }
            return result;
        }

        public static string GetTableLastUpdate(string tableName, string connectionString)
        {
            string result = null;
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand getLastUpdateDateCommand = new SqlCommand(
               "Select top 1 LastUpdateDate from CargoTrackingWatermarks where TableName = '" + tableName + "';", connection);
            try
            {
                getLastUpdateDateCommand.CommandTimeout = (int)TimeOut;
                connection.Open();
                result = ExecuteTableLastUpdateCommand(getLastUpdateDateCommand);
            }
            catch (Exception ex)
            {
                connection.Close();
                throw ex;
            }
            finally
            {
                connection.Close();
            }
            return result;
        }


        private static string ExecuteTableLastUpdateCommand(SqlCommand lastUpdateDateCommand)
        {
            string result = "";
            using (SqlDataReader reader = lastUpdateDateCommand.ExecuteReader())
            {
                reader.Read();
                DateTime? datetime = null;
                var value = reader["LastUpdateDate"];
                if (value != null)
                {
                    if (!string.IsNullOrEmpty(value.ToString()))
                    {
                        datetime = (DateTime?)(value);
                        if (datetime != null) result = datetime.Value.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
                    }

                }
            }
            return result;
        }

        public static void ExecuteSql(string sqlString, string connectionString)
        {

            if (!string.IsNullOrEmpty(sqlString))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(sqlString, connection);
                    sqlCommand.CommandTimeout = (int)ServiceHelper.TimeOut;
                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }


        public static string BuildConnectionString(ConnectionStringArguments connectionStringArguments)
        {
            string result = "Data Source=" + connectionStringArguments.Server + 
                ";Initial Catalog="+connectionStringArguments.Catalog + 
                ";Integrated Security=False;Persist Security Info=True;User ID=" + connectionStringArguments.UserName + 
                ";Password= " + connectionStringArguments.Password + ";MultipleActiveResultSets=True;Connect Timeout=60";
            return result;
        }

        public static void CreateCargoTrackingTable(CargoTrackingArgs buildCargoArgs)
        {
            string tableStructureSQLCommand = buildCargoArgs.Table.TableStructure;
            ExecuteSql(tableStructureSQLCommand, buildCargoArgs.DestinationConnectionString);
            if (!string.IsNullOrEmpty(buildCargoArgs.Table.InnerTableStructure))
            {
                tableStructureSQLCommand = buildCargoArgs.Table.InnerTableStructure;
                ExecuteSql(tableStructureSQLCommand, buildCargoArgs.DestinationConnectionString);
            }

        }

        public static ConnectionStringArguments GetConnectionStringArguments(string[] connectionArray)
        {
            ConnectionStringArguments connectionStringArguments = new ConnectionStringArguments()
            {
                Catalog = connectionArray[0],
                UserName = connectionArray[1],
                Password = connectionArray[2],
                Server = connectionArray[3],
            };

            return connectionStringArguments;
        }


        public static string GetInvokeDBTableByTableName(string tableName,string methodName)
        {
            string dmlFile = null;
            if (!string.IsNullOrEmpty(tableName) && !string.IsNullOrEmpty(methodName))
            {
                string dataBaseTableName = tableName;
                Type type = Type.GetType("Logitude.CargoTracking.BL.CargoTrackingServices.DBTablesCopy.Generated." + dataBaseTableName + "Dxml");
                Object obj = Activator.CreateInstance(type);
                MethodInfo methodInfo = type.GetMethod(methodName);
                dmlFile = (string)methodInfo.Invoke(obj, null);
            }
            return dmlFile;
        }


        public static string GetInvokeClassWithMethode(string classPath, string methodName)
        {
            string dxmlFile = null;
            if (!string.IsNullOrEmpty(classPath) && !string.IsNullOrEmpty(methodName))
            {
                Type type = Type.GetType(classPath);
                Object obj = Activator.CreateInstance(type);
                MethodInfo methodInfo = type.GetMethod(methodName);
                if (methodInfo!=null)
                {
                    dxmlFile = (string)methodInfo.Invoke(obj, null);
                }
            }
            return dxmlFile;
        }
        public static void UpdateWaterMarksTable(CargoTrackingTable table, string date, string connectionString)
        {
            var todayDate = TenantServerConfigration.GetCurrentDateTime(0).ToString("MM/dd/yyyy hh:mm:ss.fff tt");
            string setLastUpdateDateCommand = "update  CargoTrackingWatermarks set LastUpdateDate = '" + date + "',LastRun = '" + todayDate + "' where tableName = '" + table.Main_CargoTracking_TableName + "'";
            ExecuteSql(setLastUpdateDateCommand, connectionString);

        }

        public static void AddWaterMarksRecord(CargoTrackingTable table, string connectionString)
        {
            string insertRecoredInWatermarksCommand = "insert into CargoTrackingWatermarks  values('" + table.Main_CargoTracking_TableName + "' , NULL,NULL)";
            ExecuteSql(insertRecoredInWatermarksCommand, connectionString);
        }
        public static void DeleteWatermarks(string connectionString)
        {
            string deleteRecoredFromWatermarksCommand = "Delete From CargoTrackingWatermarks";
            ExecuteSql(deleteRecoredFromWatermarksCommand, connectionString);
        }
        public static void UpdateIsIncrementalRunning(int isRunning, string connectionString)
        {
            string updateIsIncrementalBuildRunningCommandcmd = "Update Tenants set IsIncrementalBuildRunning = " + isRunning + " Where Id = 0";
            ExecuteSql(updateIsIncrementalBuildRunningCommandcmd, connectionString);
        }

        public static DateTime? GetAutomaticLastUpdateDate(DataTable dataTable, bool isClosed)
        {
            DateTime? automaticLastUpdateDate = null;

            if (!isClosed)
            {
                automaticLastUpdateDate = (DateTime)dataTable.Rows
                                                    .Cast<DataRow>()
                                                    .Max(d => d["AutomaticLastUpdateDate"]);
            }


            return automaticLastUpdateDate;
        }



        public static void UpdateWaterMarkAfterFinishCheck(CargoTrackingTable table, DateTime? automaticLastUpdateDate, CargoTrackingArgs buildCargoArgs)
        {
            int ShipmentTable_GetShipmentOrders = 3;
            if (table != null && table.Main_CargoTracking_TableName != "CargoTrackingWatermarks")
            {

                if (automaticLastUpdateDate != null) {
                    var lastUpdateDate = automaticLastUpdateDate.Value.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
                    if (buildCargoArgs.Table.CurrentCondition == ShipmentTable_GetShipmentOrders)
                    {
                        lastUpdateDate = automaticLastUpdateDate.Value.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
                    } 
                    UpdateWaterMarksTable(table, lastUpdateDate, buildCargoArgs.DestinationConnectionString);
                } 
 
                table.IsUpdated = true;

            }
        }
        public static void CheckAndUpdateWaterMark(string dbSourceConnection, string dbDestenationConnection)
        {

            List<CargoTrackingTable> cargoTableLists = CargoTrackingTableList.GetCargoTrackingTableList();
            foreach (CargoTrackingTable table in cargoTableLists)
            {
                if (table.DBTableName != "CargoTrackingWatermarks")
                {
                    ExecuteCheckAndUpdateWaterMarkCommand(dbSourceConnection, dbDestenationConnection, table);
                }
            }

        }

        private static void ExecuteCheckAndUpdateWaterMarkCommand(string dbSourceConnection, string dbDestenationConnection, CargoTrackingTable table)
        {
            using (SqlConnection sourceConnection =
                       new SqlConnection(dbSourceConnection))
            {
                sourceConnection.Open();

                SqlCommand getTableNameFromWatermarksCommand = new SqlCommand(
               "SELECT  TableName" +
               " FROM dbo.CargoTrackingWatermarks WHERE TableName = '" + table.Main_CargoTracking_TableName + "'", sourceConnection);
                getTableNameFromWatermarksCommand.CommandTimeout = (int)ServiceHelper.TimeOut;
                SqlDataReader reader = getTableNameFromWatermarksCommand.ExecuteReader();
                if (!reader.HasRows)
                {
                    string lastUpdateDate = GetAutomaticLastUpdateDate(table.DBTableName, dbDestenationConnection);
                    if (string.IsNullOrEmpty(lastUpdateDate)) lastUpdateDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
                    AddWaterMarksRecord(table,dbSourceConnection);
                    sourceConnection.Close();
                }
                else
                {
                    sourceConnection.Close();

                }
            }
        }

        public static void DropTable(CargoTrackingArgs buildCargoArgs)
        {
            string minTableName = buildCargoArgs.Table.Pre_TableName;
            string innerTableName  = buildCargoArgs.Table.Pre_InnerTableName;
            string dropTableCommand = GetDropTableCommand(minTableName);
            ExecuteSql(dropTableCommand, buildCargoArgs.DestinationConnectionString);
            if (!string.IsNullOrEmpty(innerTableName))
            {
                dropTableCommand = GetDropTableCommand(innerTableName);
                ExecuteSql(dropTableCommand, buildCargoArgs.DestinationConnectionString);
            }
        }

        private static string GetDropTableCommand(string tableName)
        {
            string dropTableCommand = "If exists (select * from sysobjects where name='" + tableName + "' and xtype='U') " +
                                      " BEGIN " +
                                      " Drop Table " + tableName +
                                      " END ";

            return dropTableCommand;
        }

        public static List<int> GetAllTenants(string connectionString)
        {

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand getAllTenantsCommand = new SqlCommand(
               "select id from Tenants", connection);
            try
            {
                getAllTenantsCommand.CommandTimeout = (int)TimeOut;
                connection.Open();
                var result = ExecuteGetAllTenantsCommand(getAllTenantsCommand);
                connection.Close();
                return result;
            }
            catch (Exception ex)
            {
                connection.Close();
                throw ex;
            }
        }

        private static List<int> ExecuteGetAllTenantsCommand(SqlCommand getAllTenantsCommand)
        {
            var tenantsIds = new List<int>();
            using (SqlDataReader reader = getAllTenantsCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    tenantsIds.Add(int.Parse(reader["Id"].ToString()));
                }

            }
            return tenantsIds;
        }
    }


    public class ConnectionStringArguments
    {
        public string Server { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Catalog { get; set; }


    }
}
