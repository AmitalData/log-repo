using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.ServicesHelper
{
    public static class ServiceHelper
    {
        public static long timeOut = 100000000000000000;

        public static bool GetIsIncrementalRunning(string connectionString)
        {
            bool Result = false;

            SqlConnection con = new SqlConnection(connectionString);

            SqlCommand com = new SqlCommand(
               "Select  IsIncrementalBuildRunning FROM dbo.Tenants Where Id = 0", con);
            try
            {
                com.CommandTimeout = (int)timeOut;
                con.Open();
                using (SqlDataReader reader = com.ExecuteReader())
                {
                    reader.Read();
                    bool IsRunning = false;
                    var value = reader["IsIncrementalBuildRunning"];
                    if (value != null)
                    {
                        IsRunning = (bool)(value);
                        if (IsRunning != null) Result = IsRunning;


                    }
                }
            }
            finally
            {
                con.Close();
            }
            return Result;
        }


        public static string GetAutomaticLastUpdateDate(string tableName, string connectionString)
        {
            string result = null;

            SqlConnection con = new SqlConnection(connectionString);

            SqlCommand com = new SqlCommand(
               "select MIN(AutomaticLastUpdateDate) -1 AutomaticLastUpdateDate " +
               "FROM dbo." + tableName + " ;", con);

            try
            {
                com.CommandTimeout = (int)timeOut;
                con.Open();
                using (SqlDataReader reader = com.ExecuteReader())
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
            }
            finally
            {
                con.Close();
            }
            return result;
        }



        public static string GetTableLastUpdate(string tableName, string connectionString)
        {


            string result = null;

            SqlConnection con = new SqlConnection(connectionString);

            SqlCommand com = new SqlCommand(
               "Select top 1 LastUpdateDate from CargoTrackingWatermarks where TableName = '" + tableName + "';", con);
            try
            {
                com.CommandTimeout = (int)timeOut;
                con.Open();
                using (SqlDataReader reader = com.ExecuteReader())
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
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public static void ExecuteSql(string sqlString, string connectionString)
        {

            if (!string.IsNullOrEmpty(sqlString))
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(sqlString, cn);
                    sqlCommand.CommandTimeout = (int)ServiceHelper.timeOut;
                    cn.Open();
                    sqlCommand.ExecuteNonQuery();
                    cn.Close();
                }
            }
        }


        public static string BuildConnectionString(string catalog, string userName, string password, string server)
        {
            string result = "Data Source=" + server + ";Initial Catalog=" + catalog + ";Integrated Security=False;Persist Security Info=True;User ID=" + userName + ";Password= " + password + ";MultipleActiveResultSets=True;Connect Timeout=60";
            return result;
        }

        public static void CreateCargoTrackingTable(CargoArgs buildCargoArgs)
        {
            string TableName = buildCargoArgs.Table.Pre_TableName;
            string TableName2 = buildCargoArgs.Table.Pre2_TableName;
            string SQL = BuildTablesStructure.GetTableStructure(TableName);
            ExecuteSql(SQL, buildCargoArgs.DestinationConnectionString);
            if (!string.IsNullOrEmpty(TableName2))
            {
                SQL = BuildTablesStructure.GetTableStructure(TableName2);
                ExecuteSql(SQL, buildCargoArgs.DestinationConnectionString);
            }

        }


        public static void UpdateWaterMarksTable(CargoTable table, string date, string connectionString)
        {
            var TodayDate = TenantServerConfigration.GetCurrentDateTime(0);
            string cmd = "update  CargoTrackingWatermarks set LastUpdateDate = '" + date + "',LastRun = '" + TodayDate + "' where tableName = '" + table.Main_CT_TableName + "'";
            ExecuteSql(cmd, connectionString);

        }

        public static void AddWaterMarksRecord(CargoTable table, string date, string connectionString)
        {
            string cmd = "insert into CargoTrackingWatermarks  values('" + table.CT_TableName + "' , NULL,NULL)";
            ExecuteSql(cmd, connectionString);
        }
        public static void DeleteWatermarks(string connectionString)
        {
            string cmd = "Delete From CargoTrackingWatermarks";
            ExecuteSql(cmd, connectionString);
        }
        public static void UpdateIsIncrementalRunning(int IsRunning, string connectionString)
        {
            string cmd = "Update Tenants set IsIncrementalBuildRunning = " + IsRunning + " Where Id = 0";
            ExecuteSql(cmd, connectionString);
        }

        public static DateTime? GetAutomaticLastUpdateDate(DateTime? automaticLastUpdateDate, DataTable dataTable, bool IsClosed)
        {

            if (!IsClosed)
            {

                var MaxUpdate = (DateTime)dataTable.Rows
                                                .Cast<DataRow>()
                                                .Max(d => d["AutomaticLastUpdateDate"]);

                if (MaxUpdate > automaticLastUpdateDate || automaticLastUpdateDate == null)
                {
                    automaticLastUpdateDate = (DateTime)dataTable.Rows
                   .Cast<DataRow>()
                   .Max(d => d["AutomaticLastUpdateDate"]);
                }


            }


            return automaticLastUpdateDate;
        }



        public static void UpdateWaterMarkAfterFinishCheck(CargoTable table, DateTime? automaticLastUpdateDate, CargoArgs buildCargoArgs)
        {
            if (table != null && table.DBTableName != "CargoTrackingWatermarks")
            {

                var lastUpdateDate = string.Empty;
                if (automaticLastUpdateDate != null) lastUpdateDate = automaticLastUpdateDate.Value.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
                else lastUpdateDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
                UpdateWaterMarksTable(table, lastUpdateDate, buildCargoArgs.DestinationConnectionString);
                table.IsUpdated = true;

            }
        }
        public static void CheckAndUpdateWaterMark(string dbSourceConnection, string dbDestenationConnection)
        {

            List<CargoTable> CargoTableLists = CargoTrackingTableList.FillCargoTableList();
            foreach (CargoTable table in CargoTableLists)
            {
                if (table.DBTableName != "CargoTrackingWatermarks")
                {
                    using (SqlConnection SourceConnection =
                         new SqlConnection(dbSourceConnection))
                    {
                        SourceConnection.Open();

                        SqlCommand commandSourceData = new SqlCommand(
                       "SELECT  TableName" +
                       " FROM dbo.CargoTrackingWatermarks WHERE TableName = '" + table.CT_TableName + "'", SourceConnection);
                        commandSourceData.CommandTimeout = (int)ServiceHelper.timeOut;
                        SqlDataReader reader = commandSourceData.ExecuteReader();
                        if (!reader.HasRows)
                        {
                            string lastUpdateDate = GetAutomaticLastUpdateDate(table.DBTableName, dbDestenationConnection);
                            if (string.IsNullOrEmpty(lastUpdateDate)) lastUpdateDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
                            AddWaterMarksRecord(table, lastUpdateDate, dbSourceConnection);
                            SourceConnection.Close();
                        }
                        else
                        {
                            SourceConnection.Close();

                        }
                    }
                }
            }

        }

        public static void DropTable(CargoArgs buildCargoArgs)
        {
            string TableName = buildCargoArgs.Table.Pre_TableName;
            string TableName2 = buildCargoArgs.Table.Pre2_TableName;

            string cmd = "If exists (select * from sysobjects where name='" + TableName + "' and xtype='U') " +
                              " BEGIN " +
                              " Drop Table " + TableName +
                              " END ";


            ExecuteSql(cmd, buildCargoArgs.DestinationConnectionString);

            if (!string.IsNullOrEmpty(TableName2))
            {
                cmd = "If exists (select * from sysobjects where name='" + TableName2 + "' and xtype='U') " +
                        " BEGIN " +
                        " Drop Table " + TableName2 +
                        " END ";


                ExecuteSql(cmd, buildCargoArgs.DestinationConnectionString);
            }
        }
    }
}
