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

        public bool GetWarehouseFieldFromSettings(string fieldName, string connectionString)
        {
            string connection = connectionString.Replace("Main", "Global");

            bool result = false;

            SqlConnection con = new SqlConnection(connection);

            SqlCommand com = new SqlCommand(
"select " + fieldName + " " +
"FROM dbo.Settings" + " ;", con);

            try
            {
                con.Open();

                using (SqlDataReader reader = com.ExecuteReader())
                {
                    reader.Read();
                    result = (bool)(reader[fieldName]);
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

            string connection = connectionString.Replace("Main", "Global");

            DateTime? result = null;

            SqlConnection con = new SqlConnection(connection);

            SqlCommand com = new SqlCommand(
"select DWNextRunTime " +
"FROM dbo.Settings" + " ;", con);

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
            string connection = connectionString.Replace("Main", "Global");
            using (SqlConnection cn = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("update  dbo.Settings set DWNextRunTime= '" + datetime + "' ;", cn);
                sqlCommand.CommandTimeout = (int)timeOut;
                cn.Open();
                sqlCommand.ExecuteNonQuery();
                cn.Close();
            }

        }


        public void UpdateWarehouseFieldSettings(string fieldName, bool value, string connectionString)
        {
            string connection = connectionString.Replace("Main", "Global");
            using (SqlConnection cn = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("update  dbo.Settings set " + fieldName + "= " + (value ? 1 : 0) + " ;", cn);
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

                    var dbConnectionString = reader["SecondaryAzureDBConnection"];
                    if (dbConnectionString != null && !string.IsNullOrEmpty(dbConnectionString.ToString())) result = dbConnectionString.ToString();
                    else
                    {
                        dbConnectionString = reader["DBConnection"];
                        if (dbConnectionString != null) result = dbConnectionString.ToString();
                    }

                }
            }
            finally
            {
                con.Close();
            }

            return result;
        }



        public DateTime? GetWarehouseRunDate(DateTime todayDate)
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



        public void FillDaysList()
        {
            ApplicationInfo.Days = new List<DayOfWeekClass>();
            ApplicationInfo.Days.Add(new DayOfWeekClass(DayOfWeek.Sunday, 0));
            ApplicationInfo.Days.Add(new DayOfWeekClass(DayOfWeek.Monday, 1));
            ApplicationInfo.Days.Add(new DayOfWeekClass(DayOfWeek.Thursday, 2));
            ApplicationInfo.Days.Add(new DayOfWeekClass(DayOfWeek.Wednesday, 3));
            ApplicationInfo.Days.Add(new DayOfWeekClass(DayOfWeek.Tuesday, 4));
            ApplicationInfo.Days.Add(new DayOfWeekClass(DayOfWeek.Friday, 5));
            ApplicationInfo.Days.Add(new DayOfWeekClass(DayOfWeek.Saturday, 6));
        }


    }
}
