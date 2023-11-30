
using Logitude.CargoTracking.BL.CargoTrackingServices.Services;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.ServicesHelper;
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
        public static long timeOut = 100000000000000000;

        public bool GetFieldValueFromDBByTableNameAndFieldName(string fieldName, string tableName,string connectionString)
        {
            bool result = false;
            SqlConnection  connection = new SqlConnection(connectionString);
            SqlCommand selectFieldCommand = new SqlCommand("select " + fieldName + " " + "FROM dbo." + tableName + " ;", connection);
            try
            {
                connection.Open();
                using (SqlDataReader reader = selectFieldCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (reader[fieldName] != null)
                            result = (bool)(reader[fieldName]);
                    }
                }
            }
            finally
            {
                connection.Close();
            }
            return result;
        }
 

        public static void AddRecordToCargoTrackingIncrementalStats(string destinationConnectionString)
        {
            var error = !string.IsNullOrEmpty(ApplicationInfo.ErrorLogs)? "'"+ApplicationInfo.ErrorLogs+"'" : "null";
            string[] cargoTrackingTableCopyFields = CargoTrackingTableList.GetCargoTrackingTableList().Select(s => s.DBTableName).ToArray();
            string cargoTrackingTableCopyFieldsAsString = string.Join("," ,cargoTrackingTableCopyFields);
            string cargoTrackingTableCopyFieldsValueAsString = GetCargoTrackingTableCopyFieldsValueAsString(cargoTrackingTableCopyFields);
            string incrementalStatsNewRecoredCommand = "Insert Into [dbo].[CargoTrackingIncrementalStats] (StartDate,EndDate,ErrorLog," 
                                                        + cargoTrackingTableCopyFieldsAsString +") " +
                                                       "values ('" + 
                                                         ApplicationInfo.StartDate?.ToString("MM/dd/yyyy hh:mm:ss.fff tt") + "','"+ 
                                                         ApplicationInfo.EndDate?.ToString("MM/dd/yyyy hh:mm:ss.fff tt") + "',"+
                                                         error +","+
                                                         cargoTrackingTableCopyFieldsValueAsString + 
                                                        ");";
            RunScript(incrementalStatsNewRecoredCommand, destinationConnectionString);
        }

        private static string GetCargoTrackingTableCopyFieldsValueAsString(string[] cargoTrackingTableCopyFields)
        {
            string cargoTrackingTableCopyFieldsValueAsString = "";
            for (int i = 0; i < cargoTrackingTableCopyFields.Length; i++)
            {
                foreach (string table in ApplicationInfo.CargoTrackingRecordsUpdatedDictionary.Keys)
                {
                    if (table == cargoTrackingTableCopyFields[i])
                    {
                        cargoTrackingTableCopyFieldsValueAsString += ApplicationInfo.CargoTrackingRecordsUpdatedDictionary[table] + "";
                        if (i != cargoTrackingTableCopyFields.Length - 1)
                            cargoTrackingTableCopyFieldsValueAsString += ",";
                        break;
                    }
                }

            }
            return cargoTrackingTableCopyFieldsValueAsString;
        }
        public static void RunScript(string command , string connection)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
                sqlCommand.CommandTimeout = (int)timeOut;
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
 

        public string BuildConnectionString(ConnectionStringArguments  connectionStringArguments)
        {
            string result = "Data Source=" + connectionStringArguments.Server + 
                            ";Initial Catalog=" + connectionStringArguments.Catalog + 
                            ";Integrated Security=False;Persist Security Info=True;User ID=" + connectionStringArguments.UserName + 
                            ";Password= " + connectionStringArguments.Password + ";MultipleActiveResultSets=True;Connect Timeout=60";
            return result;
        }
 
 
        public bool CheckIsUpgradingSystem(string sourceConnectionString)
        {
            string connection = sourceConnectionString.Replace("Main", "Global");
            return GetFieldValueFromDBByTableNameAndFieldName("IsUpgrading", "GlobalDBs", connection);

        }
 
    }


}
