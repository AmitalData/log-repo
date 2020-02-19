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
            stringBuilder.Append(GetSqlInsertNotSpecifiedRecorderToDataWarehouse(table));
            stringBuilder.Append(generalDataWarehouseService.GetDataWarehouseScriptByForderAndScriptName("BuildWarehouse", table.BuildScriptName));
            stringBuilder.Append(generalDataWarehouseService.GetSqlCopyDataFromTempTableToActualTable(table));
            stringBuilder.Append(GetSqlAddConstraintAndIndexToDimensionTable(table));
            generalDataWarehouseService.ExecuteSql(stringBuilder.ToString(), connectionString);
        }

        private string GetSqlInsertNotSpecifiedRecorderToDataWarehouse(TableClass table)
        {
            string result = "insert into #" + table.DWObjectTableCode + "Temp ";
            StringBuilder fieldNamesBuilder = new StringBuilder(" ( ");
            StringBuilder fieldValuesBuilder = new StringBuilder(" values ( ");
            foreach (DWObjectFieldDB field in table.DWObjectFieldDBLists)
            {
                string fieldValue = string.Empty; ;
                if (field.FieldName != "[Id_Number]")
                {
                    fieldNamesBuilder.Append(field.FieldName + ((table.DWObjectFieldDBLists.Last() != field) ? "," : ")"));
                    fieldValuesBuilder.Append(GetNotSpecifiedFieldValue(table, field) + ((table.DWObjectFieldDBLists.Last() != field) ? "," : ")"));
                }
            }
            result = result + fieldNamesBuilder.ToString() + " " + fieldValuesBuilder.ToString();

            if (table.DBTableName == "ShipmentTypes")
            {
                string secondtNotSpecifiedValue = result.Replace("-1", "Air").Replace("Not Specified", "Air");
                result += " " + secondtNotSpecifiedValue;
            }


            return result;
        }

        private static string GetNotSpecifiedFieldValue(TableClass table, DWObjectFieldDB field)
        {
            string fieldValue = string.Empty;
            if (field.DataTypeCode == "Text" || field.DataTypeCode == "nText")
            {
                if (field.FieldName == "[Id]" || (field.FieldName == "[Code]" && table.TableName != "Card")) fieldValue = field.MaxLength > 1 ? "'-1'" : "'1'";
                else fieldValue = "'Not Specified'";

                if (field.MaxLength + 2 < fieldValue.Length) fieldValue = "null";
            }
            else if (field.DataTypeCode == "Boolean" || field.DataTypeCode == "Decimal" || field.DataTypeCode == "Integer")
            {
                fieldValue = field.FieldName == "[Tenant Number]" ? "-1" : "0";
            }

            return fieldValue;
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
