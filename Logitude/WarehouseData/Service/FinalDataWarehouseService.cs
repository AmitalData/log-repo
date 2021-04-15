using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseData.Service;

namespace WarehouseData.Helper
{
  public  class FinalDataWarehouseService
    {
        GeneralDataWarehouseService generalDataWarehouseService;
        EnvironmentDWTableUnUseSqlProvider environmentDWTableUnUseSqlProvider;
        public FinalDataWarehouseService()
        {
            generalDataWarehouseService = new GeneralDataWarehouseService();

        }


        public void FinishBuildingDataWarehouse(string connectionString ,string destinationConnectionString, List<TableClass> tableLists)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(new EnvironmentDWTableUnUseSqlProvider(connectionString).GetUnUseDataWarehouseSQL());
            stringBuilder.Append(RenameAllDataWarehouseTables(tableLists));
            stringBuilder.Append(AddRelationsBetweenFactAndDimensionTables(tableLists));
            stringBuilder.Append(AddNonClusteredIndexs(tableLists));
            generalDataWarehouseService.ExecuteSql(stringBuilder.ToString(), destinationConnectionString);
        }


        #region Rename Data Warehouse Tables
        public string RenameAllDataWarehouseTables(List<TableClass> tables)
        {
            StringBuilder resultBuilder = new StringBuilder();
            foreach (TableClass table in tables.Where(d=>d.HasFactTable))
            {
                resultBuilder.Append(RenameDataWarehouseTable(table));
                resultBuilder.Append(RenameDataWarehouseConstraint(table));
            }

            foreach (TableClass table in tables.Where(d => d.HasDimensionTable))
            {
                resultBuilder.Append(RenameDataWarehouseTable(table));
                resultBuilder.Append(RenameDataWarehouseConstraint(table));
            }

            return resultBuilder.ToString();
        }
        private string RenameDataWarehouseTable(TableClass table)
        {
            string sql = "IF OBJECT_ID('" + table.DWObjectTableCode + "', 'U')  IS NOT NULL and OBJECT_ID('New" + table.DWObjectTableCode + "', 'U')  IS NOT NULL begin EXEC sp_rename '" + table.DWObjectTableCode + "', 'Old" + table.DWObjectTableCode + "' end \r\n";
            sql += "IF OBJECT_ID('New" + table.DWObjectTableCode + "', 'U')  IS NOT NULL begin EXEC sp_rename 'New" + table.DWObjectTableCode + "', '" + table.DWObjectTableCode + "' end \r\n";
            sql += "IF OBJECT_ID('Old" + table.DWObjectTableCode + "', 'U')  IS NOT NULL begin drop table Old" + table.DWObjectTableCode + " end \r\n";

            if (table.HasFactTable) sql += "\r\n";

            return sql;
        }
        private string RenameDataWarehouseConstraint(TableClass table)
        {
            string result = string.Empty;
            string fieldName = GetTablePrimaryKey(table);
            if (!string.IsNullOrWhiteSpace(fieldName))
            {
                result += "IF  EXISTS (SELECT * FROM sys.key_constraints WHERE [type] = 'PK' and   [parent_object_id] = Object_id('dbo." + table.DWObjectTableCode + "') and name = 'PK_New" + table.DWObjectTableCode + "_" + fieldName.Replace(" ", "") + "')  begin ALTER TABLE " + table.DWObjectTableCode + " DROP CONSTRAINT PK_New" + table.DWObjectTableCode + "_" + fieldName.Replace(" ", "") + " end \r\n";
                result += ("ALTER TABLE " + table.DWObjectTableCode + " ADD CONSTRAINT PK_" + table.DWObjectTableCode + "_" + fieldName.Replace(" ", "") + " PRIMARY KEY CLUSTERED([" + fieldName + "]) \r\n\r\n");

            }


            return result;
        }

        #endregion

        private string GetTablePrimaryKey(TableClass table)
        {
            string result = "";
            if (table.DWObjectFieldDBLists != null)
            {
                var field = table.DWObjectFieldDBLists.Where(d => d.IsPrimaryKey).FirstOrDefault();
                if (field != null)
                {
                    result = field.FieldName.Replace("[", "").Replace("]", "");
                }
            }
            return result;
        }
   
        public string AddRelationsBetweenFactAndDimensionTables(List<TableClass> tableLists)
        {

            StringBuilder resultBuilder = new StringBuilder();
            foreach (TableClass table in tableLists.Where(d => d.HasFactTable))
            {
                StringBuilder sqlStringBuilder = new StringBuilder();
                sqlStringBuilder.Append("IF OBJECT_ID ('" + table.DWObjectTableCode + "', 'U')  IS NOT NULL \r\n begin \r\n");
                foreach (DWObjectFieldDB objectFieldDB in table.DWObjectFieldDBLists.Where(d => !string.IsNullOrWhiteSpace(d.DimensionTableCode) && d.DimensionTableCode != "DIM_Dates"))
                {
                    TableClass dimensionTable = tableLists.Where(d => d.DWObjectTableCode == objectFieldDB.DimensionTableCode).FirstOrDefault();
                    string field = objectFieldDB.FieldName.Replace("[", "").Replace("]", "");

                    if (dimensionTable != null)
                    {
                        string dimensionTableKey = GetTablePrimaryKey(dimensionTable);
                        sqlStringBuilder.Append((" ALTER TABLE " + table.DWObjectTableCode + " ADD CONSTRAINT FK_" + table.DWObjectTableCode + "_" + objectFieldDB.DimensionTableCode + "_" + field.Replace(" ", "") + " FOREIGN KEY ([" + field + "]) REFERENCES " + objectFieldDB.DimensionTableCode + "([" + dimensionTableKey + "])\r\n"));
                    }
                }
                sqlStringBuilder.Append(("\r\n end \r\n"));
                resultBuilder.Append(sqlStringBuilder.ToString());
            }
            return resultBuilder.ToString();
        }


        public string AddNonClusteredIndexs(List<TableClass> tableLists)
        {
            string result = string.Empty;
            foreach (TableClass table in tableLists.Where(d => d.HasFactTable))
            {
                if (!string.IsNullOrEmpty(table.FieldIndexes))
                {
                    string[] fieldNames = table.FieldIndexes.Split(',');
                    if (fieldNames.Length > 0)
                    {
                        foreach (string fieldName in fieldNames)
                        {
                            result += (" CREATE NONCLUSTERED INDEX [IX_" + table.DWObjectTableCode + "_" + fieldName + "] ON [dbo].[" + table.DWObjectTableCode + "]([" + fieldName + "]) \r\n ");
                        }
                    }
                }
            }
            result  += GetAutomaticDWObjectIndex(tableLists);

            return result;
        }

        private  string GetAutomaticDWObjectIndex(List<TableClass> tableLists)
        {
            string result = string.Empty;
            foreach (TableClass table in tableLists.Where(d => d.Indexes != null && d.Indexes.Count() > 0))
            {
                foreach (IndexItem index in table.Indexes)
                {
                    List<string> indexColumns = index.Columns.Split(',').ToList();
                    string indexName = "INDEX[IX_" + table.DWObjectTableCode;
                    foreach (string column in indexColumns)
                    {
                        indexName += ("_" + column.Replace("[", "").Replace("]", "").Replace(" ", ""));
                    }
                    result += (" CREATE NONCLUSTERED " + indexName + "] ON [dbo].[" + table.DWObjectTableCode + "](" + index.Columns + ") \r\n ");
                }
            }

            return result;
        }
    }
}
