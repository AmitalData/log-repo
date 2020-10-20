
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrackingWinService.Helper
{
    public class CargoTrackingServiceHelper
    {
        public static int timeOut = 0;
 



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
 

        public static void AddRecordToCargoTrackingIncrementalStats(string destinationConnectionString)
        {
            var Error = ApplicationInfo.ErrorLogs!=null? ApplicationInfo.ErrorLogs : "null";
            string cmd = "Insert Into [dbo].[CargoTrackingIncrementalStats] (StartDate,EndDate,Shipments,Cards,Ports,Countries,TransportModes,ShipmentComputedFields,ShipmentMasterDatas,ErrorLog) values ('" + ApplicationInfo.StartDate+ "','"+ ApplicationInfo.EndDate+ "',"+ ApplicationInfo.Shipments+ ","+ ApplicationInfo .Cards+ ","+ ApplicationInfo .Ports+ ","+ ApplicationInfo .Countries+ ","+ ApplicationInfo .TransportModes+","+ ApplicationInfo.ShipmentComputedFields + ","+ ApplicationInfo.ShipmentMasterDatas+ "," + Error + ");";
            RunScript(cmd, destinationConnectionString);
        }


        public static void RunScript(string sql , string connection)
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
 
      
         

        public bool CheckIsUpgradingSystem(string sourceConnectionString)
        {
            string connection = sourceConnectionString.Replace("Main", "Global");
            return GetFieldValueFromDBByTableNameAndFieldName("IsUpgrading", "GlobalDBs", connection);

        }
 
    }
}
