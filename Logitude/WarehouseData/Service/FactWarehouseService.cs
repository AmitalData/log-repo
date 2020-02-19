using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseData.Helper
{

    public class FactWarehouseService
    {
    


        private GeneralDataWarehouseService generalDataWarehouseService;
        public FactWarehouseService(string appName, string mode)
        {
            generalDataWarehouseService = new GeneralDataWarehouseService(appName, mode); 
        }



        public void BuildFactTable(string connectionString, TableClass table)
        {
            StringBuilder buildFactBuilderSqlString = new StringBuilder();
            buildFactBuilderSqlString.Append(generalDataWarehouseService.CreateSqlDataWarehouseTable(table));
            buildFactBuilderSqlString.Append(generalDataWarehouseService.GetDataWarehouseScriptByForderAndScriptName("BuildWarehouse", table.BuildScriptName));
            buildFactBuilderSqlString.Append(generalDataWarehouseService.GetSqlCopyDataFromTempTableToActualTable(table));

            string buildFactSqlString = buildFactBuilderSqlString.ToString();
            if (table.HasCustomFields)
            {
                CustomFieldWarehouseService customFieldWarehouseService = new CustomFieldWarehouseService();
                buildFactSqlString = customFieldWarehouseService.BuildCustomFields(buildFactSqlString, table.CustomFieldsCount);
            }


            generalDataWarehouseService.ExecuteSql(buildFactSqlString, connectionString);
        }



        public void UpdateFactTable(string connectionString, TableClass table)
        {
            string updateFactSqlString = string.Empty;
            RemoveOldRowsFromFactTable(table, connectionString);
            updateFactSqlString = generalDataWarehouseService.GetDataWarehouseScriptByForderAndScriptName("IncrementalWarehouse", table.IncrementalScriptName);

            if (table.HasCustomFields)
            {
                CustomFieldWarehouseService customFieldWarehouseService = new CustomFieldWarehouseService();
                updateFactSqlString = customFieldWarehouseService.BuildCustomFields(updateFactSqlString, table.CustomFieldsCount);
            }
            generalDataWarehouseService.ExecuteSql(updateFactSqlString, connectionString);
        }

        private void RemoveOldRowsFromFactTable(TableClass table, string connectionString)
        {
            using (SqlConnection sourceConnection = new SqlConnection(connectionString))
            {
                sourceConnection.Open();
                SqlCommand commandSourceData = new SqlCommand(
               "SELECT " + table.KeyName +
               " FROM dbo." + table.Dw_TableName + " where AutomaticLastUpdateDate > ( select LastUpdateDate from dw_WaterMarks where TableName = " + "'" + table.TableName + "');", sourceConnection);
                SqlDataReader reader = commandSourceData.ExecuteReader();
                if (reader.HasRows)
                {
                    var dataTable = new DataTable();
                    dataTable.Load(reader);
                    var columns = dataTable.Rows.Cast<DataRow>().Select(r => (string)r[table.KeyName].ToString()).ToList();
                    generalDataWarehouseService.DeleteRowsFromDataWarehouse(new DeleteRowsArgs() { TableName = "Fact_" + table.DBTableName, KeyName = table.KeyName, IdsList = columns, ConnectionString = connectionString });
                }
                reader.Close();
            }
        }

       


    }





}
