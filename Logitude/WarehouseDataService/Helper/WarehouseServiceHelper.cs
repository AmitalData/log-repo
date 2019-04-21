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

            return BuildConnectionString(result) ;
        }

    }
}
