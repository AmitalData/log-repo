using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseData.Helper
{
   public class DimensionWarehouseService
    {
       private  GeneralDataWarehouseService generalDataWarehouseService;
        public DimensionWarehouseService(string appName , string mode)
        {
             generalDataWarehouseService = new GeneralDataWarehouseService(appName, mode);
        }


        public void UpdateDimensionTable(string connectionString, TableClass table)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(generalDataWarehouseService.GetDataWarehouseScriptByForderAndScriptName("IncrementalWarehouse", table.IncrementalScriptName));
            generalDataWarehouseService.ExecuteSql(stringBuilder.ToString(), connectionString);
        }

        public void BuildDimensionTable(string connectionString, TableClass table)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(generalDataWarehouseService.CreateSqlDataWarehouseTable(table));
         
            var sqlInsertNotSpecifiedRecordArgs = new SqlInsertNotSpecifiedRecordArgs()
            {
                ObjectFieldDBLists = table.DWObjectFieldDBLists,
                Table = table,
                Tenant = 0,
                TableName = ("#" + table.DWObjectTableCode + "Temp ")
            };

            if(table.Dw_TableName == "dw_Ports")
            {

            }
            stringBuilder.Append(generalDataWarehouseService.GetSqlInsertNotSpecifiedRecorderToDB(sqlInsertNotSpecifiedRecordArgs));

            stringBuilder.Append(generalDataWarehouseService.GetDataWarehouseScriptByForderAndScriptName("BuildWarehouse", table.BuildScriptName));
            stringBuilder.Append(generalDataWarehouseService.GetSqlCopyDataFromTempTableToActualTable(table));
            stringBuilder.Append(GetSqlAddConstraintAndIndexToDimensionTable(table));
            generalDataWarehouseService.ExecuteSql(stringBuilder.ToString(), connectionString);
        }


        private string GetSqlAddConstraintAndIndexToDimensionTable(TableClass table)
        {
            string result = string.Empty;
            var field = table.DWObjectFieldDBLists.Where(d => d.IsPrimaryKey).FirstOrDefault();
            if (field != null)
            {
                string keyName = field.FieldName.Replace("[", "").Replace("]", "");
                result = (" ALTER TABLE New" + table.DWObjectTableCode + " ADD CONSTRAINT PK_New" + table.DWObjectTableCode + "_" + keyName.Replace(" ", "") + " PRIMARY KEY CLUSTERED([" + keyName + "]) \r\n");

            }
            var secondField = table.DWObjectFieldDBLists.Where(d => d.FieldName == "[Id]").FirstOrDefault();
            if (secondField == null) secondField = table.DWObjectFieldDBLists.Where(d => d.FieldName == "[Code]").FirstOrDefault();
            if (secondField != null)
            {
                string secondKeyName = secondField.FieldName.Replace("[", "").Replace("]", "");
                result += (" CREATE NONCLUSTERED INDEX [IX_" + table.DWObjectTableCode + "_" + secondKeyName.Replace(" ", "") + "] ON [dbo].[New" + table.DWObjectTableCode + "]([" + secondKeyName + "]) \r\n");
            }
            return result;
        }
    }
}
