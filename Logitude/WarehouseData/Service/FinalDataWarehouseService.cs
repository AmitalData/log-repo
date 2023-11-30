using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseData.Service;

namespace WarehouseData.Helper
{
    public class FinalDataWarehouseService
    {
        GeneralDataWarehouseService generalDataWarehouseService;
        EnvironmentDWTableUnUseSqlProvider environmentDWTableUnUseSqlProvider;
        public FinalDataWarehouseService(string applicationName = "WarehouseData", string applicationMode = "Debug")
        {
            generalDataWarehouseService = new GeneralDataWarehouseService(applicationName, applicationMode);

        }


        public void FinishBuildingDataWarehouse(string connectionString, string destinationConnectionString, List<TableClass> tableLists)
        {
            generalDataWarehouseService.ExecuteSql(new EnvironmentDWTableUnUseSqlProvider(connectionString).GetUnUseDataWarehouseSQL(), destinationConnectionString);
            generalDataWarehouseService.ExecuteSqlTransaction(RenameAllDataWarehouseTables(tableLists), destinationConnectionString);
            AddRelationsBetweenFactAndDimensionTables(tableLists, destinationConnectionString);
            AddNonClusteredIndexs(tableLists, destinationConnectionString);
            AddAutomaticDWObjectIndex(tableLists, destinationConnectionString);
            generalDataWarehouseService.ExecuteScript("Others", "AddAdditionalIndexesToFactTables", destinationConnectionString);

        }



        #region Rename Data Warehouse Tables
        public string RenameAllDataWarehouseTables(List<TableClass> tables)
        {
            StringBuilder resultBuilder = new StringBuilder();
            foreach (TableClass table in tables.Where(d => d.HasFactTable))
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
            StringBuilder result = new StringBuilder();
            string fieldName = GetTablePrimaryKey(table);
            if (!string.IsNullOrWhiteSpace(fieldName))
            {
                result.Append("IF  EXISTS (SELECT * FROM sys.key_constraints WHERE [type] = 'PK' and   [parent_object_id] = Object_id('dbo." + table.DWObjectTableCode + "') and name = 'PK_New" + table.DWObjectTableCode + "_" + fieldName.Replace(" ", "") + "')  begin ALTER TABLE " + table.DWObjectTableCode + " DROP CONSTRAINT PK_New" + table.DWObjectTableCode + "_" + fieldName.Replace(" ", "") + " end \r\n");
                result.Append("IF  NOT EXISTS (SELECT * FROM sys.key_constraints WHERE [type] = 'PK' and   [parent_object_id] = Object_id('dbo." + table.DWObjectTableCode + "') and name = 'PK_" + table.DWObjectTableCode + "_" + fieldName.Replace(" ", "") + "')  begin ");
                result.Append("ALTER TABLE " + table.DWObjectTableCode + " ADD CONSTRAINT PK_" + table.DWObjectTableCode + "_" + fieldName.Replace(" ", "") + " PRIMARY KEY CLUSTERED([" + fieldName + "]) \r\n end \r\n");
            }
            return result.ToString();
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

        public void AddRelationsBetweenFactAndDimensionTables(List<TableClass> tableLists, string connectionString)
        {

            Parallel.ForEach(tableLists.Where(d => d.HasFactTable).ToList(), (table) =>
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
                        sqlStringBuilder.Append("IF  not EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE TABLE_NAME='" + table.DWObjectTableCode + "'" + " and CONSTRAINT_NAME = '" + "FK_" + table.DWObjectTableCode + "_" + objectFieldDB.DimensionTableCode + "_" + field.Replace(" ", "") + "') begin ");
                        sqlStringBuilder.Append((" ALTER TABLE " + table.DWObjectTableCode + " ADD CONSTRAINT FK_" + table.DWObjectTableCode + "_" + objectFieldDB.DimensionTableCode + "_" + field.Replace(" ", "") + " FOREIGN KEY ([" + field + "]) REFERENCES " + objectFieldDB.DimensionTableCode + "([" + dimensionTableKey + "]) End \r\n"));
                    }
                }

                sqlStringBuilder.Append(("\r\n end \r\n"));

                generalDataWarehouseService.ExecuteSql(sqlStringBuilder.ToString(), connectionString);


            });
        }


        public void AddNonClusteredIndexs(List<TableClass> tableLists, string connectionString)
        {
            Parallel.ForEach(tableLists.Where(d => d.HasFactTable).ToList(), (table) =>
            {
                StringBuilder stringBuilder = new StringBuilder();

                if (!string.IsNullOrEmpty(table.FieldIndexes))
                {
                    string[] fieldNames = table.FieldIndexes.Split(',');
                    if (fieldNames.Length > 0)
                    {
                        foreach (string fieldName in fieldNames)
                        {

                            stringBuilder.Append("IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_" + table.DWObjectTableCode + "_" + fieldName + "' AND object_id = OBJECT_ID('" + table.DWObjectTableCode + "')) begin");
                            stringBuilder.Append(" CREATE NONCLUSTERED INDEX [IX_" + table.DWObjectTableCode + "_" + fieldName + "] ON [dbo].[" + table.DWObjectTableCode + "]([" + fieldName + "]) end\r\n ");
                        }
                    }
                }
                generalDataWarehouseService.ExecuteSql(stringBuilder.ToString(), connectionString);

            });

        }

        public void FinishUpdatingDataWarehouse(string destinationConnectionString)
        {
            generalDataWarehouseService.ExecuteScript("Others", "UpdateSharedFactDWWaterMark", destinationConnectionString);
        }

        private void AddAutomaticDWObjectIndex(List<TableClass> tableLists, string connectionString)
        {
            Parallel.ForEach(tableLists.Where(d => d.Indexes != null && d.Indexes.Count() > 0).ToList(), (table) =>
            {
                StringBuilder result = new StringBuilder();

                foreach (IndexItem index in table.Indexes)
                {
                    List<string> indexColumns = index.Columns.Split(',').ToList();
                    string indexName = "INDEX[IX_" + table.DWObjectTableCode;
                    foreach (string column in indexColumns)
                    {
                        indexName += ("_" + column.Replace("[", "").Replace("]", "").Replace(" ", ""));
                    }
                    result.Append("IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = '" + indexName.Replace("INDEX[", "").Replace("]", "") + "' AND object_id = OBJECT_ID('" + table.DWObjectTableCode + "')) begin" + " CREATE NONCLUSTERED " + indexName + "] ON [dbo].[" + table.DWObjectTableCode + "](" + index.Columns + ") end  \r\n ");
                }

                generalDataWarehouseService.ExecuteSql(result.ToString(), connectionString);
            });

        }
    }
}
