using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace Logitude.DBMigrations.Models
{
    public abstract class DatabaseMigrations
    {
        protected readonly string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        public TableDefinition DXMLTable;
        //public abstract DatabaseMigrations(TableDefinition table)
        //{
        //    DXMLTable = table;
        //}
        
        public string GetScript()
        {
            TableDefinition currentTable = GetCurrentTableDefinitionFromDB();
            if (currentTable == null)
            {
                return GetCreateTableScript();
            }
            else
            {
                return GetAlterTableScript(currentTable);
            }
        }
         
        protected string GetColumnScript(ColumnDefinition column)
        {
            string columnScript = column.Name + " ";
            columnScript += GetDataTypeScript(column.Type, column.Size);
            columnScript += GetConstraintsScript(column.Constraints);
            columnScript += ",";
            return columnScript;
        }
         
        protected string GetDxmlDataType(string type)
        {
            switch (type)
            {
                case "VARCHAR":
                    return "Text";
                case "NVARCHAR":
                    return "nText";
                case "INT":
                    return "Integer";
                case "BIT":
                    return "Boolean";
                case "DATETIME":
                    return "DateTime";
                default:
                    return "Text";
            }
        }

        protected string GetConstraintsScript(ConstraintDefinition constraints)
        {
            if (constraints != null)
            {
                var constraintsScript = "";
                if (constraints.PrimaryKey)
                {
                    constraintsScript += " PRIMARY KEY";
                }
                if (!constraints.Nullable)
                {
                    constraintsScript += " NOT NULL";
                }

                return constraintsScript;
            }
            return null;
        }
         
        protected ColumnDefinition SetConstraintForColumnDefinition(ColumnDefinition column, string constraintType)
        {
            switch (constraintType)
            {
                case "PRIMARY KEY":
                    column.Constraints.PrimaryKey = true;
                    return column;
                default:
                    return column;
            }
        }

        protected TableMigrations GetTableMigrations(TableDefinition currentTable, TableDefinition dxmlTable)
        {
            List<ColumnMigrations> columnsMigrations = new List<ColumnMigrations>();
            
            foreach (var dxmlTableColumn in dxmlTable.Columns)
            {
                if(!IsColumnInTableDefinition(currentTable, dxmlTableColumn.Name))
                {
                    ColumnMigrations columnMigrations = GetColumnMigrations(MigrationTypes.ADDCOLUMN, dxmlTableColumn);
                    columnsMigrations.Add(columnMigrations);
                }
                else
                {
                    ColumnDefinition currentTableColumn = currentTable.Columns.Where(c => c.Name == dxmlTableColumn.Name).First();
                    if(IsThereMigration(currentTableColumn, dxmlTableColumn))
                    {
                        ColumnMigrations columnMigrations = GetColumnMigrations(MigrationTypes.ALTERCOLUMN, dxmlTableColumn);
                        columnsMigrations.Add(columnMigrations);
                    }
                }
            }

            List<string> dxmlTableColumns = dxmlTable.Columns.Select(c => c.Name).ToList();
            List<ColumnDefinition> droppedColumns = currentTable.Columns.Where(c => !dxmlTableColumns.Contains(c.Name)).ToList();

            foreach(var droppedColumn in droppedColumns)
            {
                ColumnDefinition currentTableColumn = currentTable.Columns.Where(c => c.Name == droppedColumn.Name).First();
                ColumnMigrations columnMigrations = GetColumnMigrations(MigrationTypes.DROPCOLUMN, currentTableColumn);
                columnsMigrations.Add(columnMigrations);
            }

            TableMigrations tableMigrations = new TableMigrations
            {
                TableName = dxmlTable.Name,
                ColumnsMigrations = columnsMigrations
            };
            return tableMigrations;
        }

        protected bool IsColumnInTableDefinition(TableDefinition tableDefinition, string columnName)
        {
            return tableDefinition.Columns.Where(c => c.Name == columnName).Any();
        }

        protected ColumnMigrations GetColumnMigrations(int migrationType, ColumnDefinition columnDefinition)
        {
            ColumnMigrations columnMigrations = new ColumnMigrations
            {
                MigrationType = migrationType,
                ColumnName = columnDefinition.Name,
                NewColumnName = columnDefinition.NewName,
                NewColumnType = columnDefinition.Type,
                NewColumnSize = columnDefinition.Size,
                NewConstraints = columnDefinition.Constraints
            };
            return columnMigrations;
        }

        protected bool IsThereMigration(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            if(dxmlTableColumn.Type != currentTableColumn.Type)
            {
                return true;
            }
            if(dxmlTableColumn.Size != currentTableColumn.Size)
            {
                return true;
            }
            if(dxmlTableColumn.Constraints.PrimaryKey != currentTableColumn.Constraints.PrimaryKey)
            {
                return true;
            }
            if(dxmlTableColumn.Constraints.Nullable != currentTableColumn.Constraints.Nullable)
            {
                return true;
            }
            return false;
        }

        //private ColumnMigrations GetAddColumnMigrations()
        //{
        //    ColumnMigrations columnMigrations = GetColumnMigrations(MigrationTypes.ADDCOLUMN, dxmlTableColumn);
        //    columnsMigrations.Add(columnMigrations);
        //}

        // abstract Classes 

        protected abstract TableDefinition GetCurrentTableDefinitionFromDB();

        protected abstract string GetCreateTableScript();

        protected abstract string GetAlterTableScript(TableDefinition currentTable);

        protected abstract string GetColumnMigrationScript(string tableName, ColumnMigrations columnMigrations);

        protected abstract string GetDataTypeScript(string type, int? size);

        protected abstract TableDefinition GetCurrentTableDefinitionFromDB(string tableName);


    }
}
