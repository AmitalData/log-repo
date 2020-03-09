using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseData.Helper;

namespace WarehouseData.Service
{
    public class WaterMarkDataWarehouseService
    {
        GeneralDataWarehouseService generalDataWarehouseService;
        public WaterMarkDataWarehouseService()
        {
            generalDataWarehouseService = new GeneralDataWarehouseService();
        }


        #region WaterMark
       
  

        public void AddWareMarkRecord(TableClass table, string date, string connectionString, int? tenant)
        {
            string cmd = tenant == null ? "insert into WaterMarks  values('" + table.TableName + "' , '" + date + "')" : "insert into PrivateWaterMarks  values('" + table.TableName + "' , '" + date + "' ," + tenant + " )";
            generalDataWarehouseService.ExecuteSql(cmd, connectionString);

        }

        public string GetAutomaticLastUpdateDate(string tableName, string connectionString)
        {
            string result = null;

            SqlConnection con = new SqlConnection(connectionString);

            SqlCommand com = new SqlCommand(
"select MAX(AutomaticLastUpdateDate) AutomaticLastUpdateDate " +
"FROM dbo." + tableName + " ;", con);

            try
            {
                com.CommandTimeout = (int)this.generalDataWarehouseService.timeOut;
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

        public void UpdateWareMarkTable(TableClass table, string date, string connectionString, int? privateTenant = null)
        {
            string cmd = "update  WaterMarks set LastUpdateDate = '" + date + "' where tableName = '" + table.TableName + "'";
            if (privateTenant != null)
            {
                cmd = cmd.Replace("WaterMarks", "PrivateWaterMarks");
                cmd += (" and PrivateTenant = " + privateTenant);
            }
            generalDataWarehouseService.ExecuteSql(cmd, connectionString);

        }

   
        #endregion


    }
}
