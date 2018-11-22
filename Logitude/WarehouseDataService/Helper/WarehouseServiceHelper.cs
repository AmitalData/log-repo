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

        public void ExecuteScript(string scripName, string forderName, string connectionString)
        {
            string path = System.IO.Path.GetDirectoryName(new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);

            if (path.Contains(@"\bin\" + ApplicationInfo.Mode))
            {
                path = path.Replace(@"\bin\" + ApplicationInfo.Mode, string.Empty);
            }

            string fileDirectory = Path.Combine(path, "WarehouseScript\\" + forderName, scripName + ".sql");
            FileInfo file = new FileInfo(fileDirectory);

            string cmd = file.OpenText().ReadToEnd();


            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(cmd, cn);
                sqlCommand.CommandTimeout = (int)timeOut;
                cn.Open();
                sqlCommand.ExecuteNonQuery();
                cn.Close();
            }

        }



    }
}
