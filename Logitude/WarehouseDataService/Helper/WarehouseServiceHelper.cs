using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDataService.Helper
{
    class WarehouseServiceHelper
    {
        int timeOut = 0;
        public string GetCount(string tableName, string connectionString)
        {
            string countStart = string.Empty;

            using (SqlConnection sourceConnection =
                       new SqlConnection(connectionString))
            {
                sourceConnection.Open();
                SqlCommand commandRowCount = new SqlCommand(
                "SELECT COUNT(*) FROM " +
                "dbo." + tableName + ";",
                sourceConnection);

                try
                {
                    countStart = commandRowCount.ExecuteScalar().ToString();


                }
                catch (Exception ex)
                {

                    // MessageBox.Show(ex.Message);
                }

            }
            return countStart;
        }




        public bool GetFieldValueFromDBByTableNameAndFieldName(string fieldName, string tableName,string connectionString)
        {
            bool result = false;
            
            SqlConnection con = new SqlConnection(connectionString);

            SqlCommand com = new SqlCommand(
"select " + fieldName + " " +
"FROM dbo." + tableName + " ;", con);

            try
            {
                con.Open();

                using (SqlDataReader reader = com.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (reader[fieldName] != null)
                        {
                            result = (bool)(reader[fieldName]);
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

        public DateTime? GetDWNextRunTime(string connectionString)
        {
            DateTime? result = null;

            SqlConnection con = new SqlConnection(connectionString);

            SqlCommand com = new SqlCommand(
"select DWNextRunTime " +
"FROM dbo.DWHBuildStatus" + " ;", con);

            try
            {
                con.Open();

                using (SqlDataReader reader = com.ExecuteReader())
                {
                    reader.Read();

                    if (reader["DWNextRunTime"] != null && !string.IsNullOrEmpty(reader["DWNextRunTime"].ToString()))
                    {
                        result = (DateTime?)(reader["DWNextRunTime"]);
                    }

                }
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public void UpdateDWNextRunTime(string connectionString, DateTime? datetime)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("update  DWHBuildStatus set DWNextRunTime= '" + datetime + "' ;", cn);
                sqlCommand.CommandTimeout = (int)timeOut;
                cn.Open();
                sqlCommand.ExecuteNonQuery();
                cn.Close();
            }

        }


        public void UpdateDWHBuildStatus(string fieldName, bool value, string connectionString)
        {
            string sql = "update  dbo.DWHBuildStatus set " + fieldName + "= " + (value ? 1 : 0) + " ";
            RunScript(sql, connectionString);
        }


        private void RunScript(string sql , string connection)
        {
            using (SqlConnection cn = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand(sql, cn);
                sqlCommand.CommandTimeout = (int)timeOut;
                cn.Open();
                sqlCommand.ExecuteNonQuery();
                cn.Close();
            }
        }

        public string BuildConnectionString(string catalog, string userName, string password, string server)
        {
            string result = "Data Source=" + server + ";Initial Catalog=" + catalog + ";Integrated Security=False;Persist Security Info=True;User ID=" + userName + ";Password= " + password + ";MultipleActiveResultSets=True;Connect Timeout=60";
            return result;
        }

        public void UpdateLastIncrementalDWUpdateDate(string sourceConnectionString)
        {
            string sql = "update  dbo.DWHBuildStatus set LastIncrementalDWUpdateDate = " + "'" + DateTime.Now + "'";
            RunScript(sql, sourceConnectionString);
        }

        public string BuildConnectionString(string dbSourceConnection)
        {
            string result = string.Empty;

            string[] sourceConnectionArray = dbSourceConnection.Split(',');
            if (sourceConnectionArray.Length == 4)
            {
                result = BuildConnectionString(sourceConnectionArray[0], sourceConnectionArray[1], sourceConnectionArray[2], sourceConnectionArray[3]);
            }

            return result;

        }

        public string GetMainDBConnectionString(string connectionString)
        {
            string result = null;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand com = new SqlCommand("select DBConnection,SecondaryAzureDBConnection from dbo.GlobalDBs where Id =0;", con);
            try
            {
                con.Open();

                using (SqlDataReader reader = com.ExecuteReader())
                {
                    reader.Read();

                    var dbConnectionString = reader["DBConnection"];
                    if (dbConnectionString != null && !string.IsNullOrEmpty(dbConnectionString.ToString()))
                    {
                        result = dbConnectionString.ToString();
                    }
                    else
                    {
                        dbConnectionString = reader["SecondaryAzureDBConnection"];
                        if (dbConnectionString != null && !string.IsNullOrEmpty(dbConnectionString.ToString())) result = dbConnectionString.ToString();
                    }

                }
            }
            finally
            {
                con.Close();
            }

            return result;
        }



        public DateTime? CalculateDWNextRunTime(DateTime todayDate)
        {


            DateTime? warehouseDate = null;

            DayOfWeekClass dayToday = ApplicationInfo.Days.Where(d => d.DayOfWeek == todayDate.DayOfWeek).FirstOrDefault();
            DayOfWeekClass warehouseday = ApplicationInfo.WarehouseBuildDays.Where(d => d.NumberOfDay == dayToday.NumberOfDay).FirstOrDefault();

            if (warehouseday != null)
            {
                warehouseDate = DateTime.Parse((todayDate.Date.ToShortDateString() + " " + ApplicationInfo.WarehouseBuildHours.ToString()));
                if (warehouseDate < todayDate)
                {
                    warehouseDate = null;
                    warehouseday = null;
                }
            }

            if (warehouseday == null)
            {
                warehouseday = ApplicationInfo.WarehouseBuildDays.Where(d => d.NumberOfDay > dayToday.NumberOfDay).OrderBy(a => a.NumberOfDay).FirstOrDefault();
                if (warehouseday != null)
                {
                    int dayBetwwenDate = warehouseday.NumberOfDay - dayToday.NumberOfDay;
                    warehouseDate = DateTime.Parse((todayDate.AddDays(dayBetwwenDate).Date.ToShortDateString() + " " + ApplicationInfo.WarehouseBuildHours.ToString()));
                }
            }

            if (warehouseday == null)
            {
                warehouseday = ApplicationInfo.WarehouseBuildDays.Where(d => d.NumberOfDay < dayToday.NumberOfDay).OrderBy(a => a.NumberOfDay).FirstOrDefault();
                if (warehouseday != null)
                {
                    int dayBetwwenDate = (7 - dayToday.NumberOfDay) + warehouseday.NumberOfDay;
                    warehouseDate = DateTime.Parse((todayDate.AddDays(dayBetwwenDate).Date.ToShortDateString() + " " + ApplicationInfo.WarehouseBuildHours.ToString()));
                }
            }

            return warehouseDate;
        }


        public bool CheckIsUpgradingSystem(string sourceConnectionString)
        {
            string connection = sourceConnectionString.Replace("Main", "Global");
            return GetFieldValueFromDBByTableNameAndFieldName("IsUpgrading", "GlobalDBs", connection);

        }




        public void FillDaysList()
        {
            ApplicationInfo.Days = new List<DayOfWeekClass>();
            ApplicationInfo.Days.Add(new DayOfWeekClass(DayOfWeek.Sunday, 0));
            ApplicationInfo.Days.Add(new DayOfWeekClass(DayOfWeek.Monday, 1));
            ApplicationInfo.Days.Add(new DayOfWeekClass(DayOfWeek.Tuesday, 2));
            ApplicationInfo.Days.Add(new DayOfWeekClass(DayOfWeek.Wednesday, 3));
            ApplicationInfo.Days.Add(new DayOfWeekClass(DayOfWeek.Thursday, 4));
            ApplicationInfo.Days.Add(new DayOfWeekClass(DayOfWeek.Friday, 5));
            ApplicationInfo.Days.Add(new DayOfWeekClass(DayOfWeek.Saturday, 6));
        }


    }
}
