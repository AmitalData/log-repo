using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

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
                case "int":
                    return "int";
                case "decimal":
                    return "decimal";
                case "timestamp":
                    return "timestamp";
                case "varbinary":
                    return "varbinary";
                case "varchar":
                    return "varchar";
                case "datetime":
                    return "datetime";
                case "time":
                    return "time";
                case "float":
                    return "float";
                case "char":
                    return "char";
                case "bigint":
                    return "bigint";
                case "nvarchar":
                    return "nvarchar";
                case "bit":
                    return "bit";
                default:
                    return null;
            }
        }
        
        protected string GetConstraintsScript(ConstraintsDefinition constraints)
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
         
        protected ColumnDefinition SetConstraintForColumnDefinition(ColumnDefinition column, string constraintType, string constraintName)
        {
            switch (constraintType)
            {
                case "PRIMARY KEY":
                    column.Constraints.PrimaryKey = true;
                    column.Constraints.PrimaryKeyConstraintName = constraintName;
                    return column;
                default:
                    return column;
            }
        }

        protected TableMigrations GetTableMigrations(TableDefinition currentTable, TableDefinition dxmlTable)
        {
            List<ColumnMigration> columnsMigrations = new List<ColumnMigration>();
            foreach (var dxmlTableColumn in dxmlTable.Columns)
            {
                if (!IsColumnInCurrentTable(currentTable, dxmlTableColumn.Name, dxmlTableColumn.OldNames))
                {
                    List<ColumnMigration> columnMigrations = GetColumnMigrations(null, dxmlTableColumn);
                    foreach(ColumnMigration columnMigration in columnMigrations)
                    {
                        columnsMigrations.Add(columnMigration);
                    }
                }
                else
                {
                    ColumnDefinition currentTableColumn = GetCurrentTableColumn(currentTable, dxmlTableColumn.Name, dxmlTableColumn.OldNames);
                    List<ColumnMigration> columnMigrations = GetColumnMigrations(currentTableColumn, dxmlTableColumn);
                    foreach (ColumnMigration columnMigration in columnMigrations)
                    {
                        columnsMigrations.Add(columnMigration);
                    }
                }
            }
            columnsMigrations = IncludeDropColumnsMigrations(currentTable, dxmlTable, columnsMigrations);
            TableMigrations tableMigrations = new TableMigrations
            {
                DxmlTableName = dxmlTable.Name,
                CurrentTableName = currentTable.Name,
                ColumnsMigrations = columnsMigrations
            };
            return tableMigrations;
        }
        
        protected List<ColumnMigration> IncludeDropColumnsMigrations(TableDefinition currentTable, TableDefinition dxmlTable, List<ColumnMigration> columnsMigrations)
        {
            List<string> dxmlTableColumnsNames = dxmlTable.Columns.Select(c => c.Name).ToList();
            List<string> dxmlTableColumnsOldNames = dxmlTable.Columns.Where(c => c.OldNames != null).Select(c => c.OldNames).ToList();
            foreach(var name in dxmlTableColumnsOldNames)
            {
                if (!name.Contains(","))
                {
                    dxmlTableColumnsNames.Add(name);
                }
                else
                {
                    dxmlTableColumnsNames = dxmlTableColumnsNames.Concat(name.Split(',').ToList()).ToList();
                }
            }
            
            List<ColumnDefinition> droppedColumns = currentTable.Columns.Where(c => !dxmlTableColumnsNames.Contains(c.Name) && !c.Name.StartsWith("Drop_")).ToList();
            foreach (var droppedColumn in droppedColumns)
            {
                ColumnDefinition currentTableColumn = GetCurrentTableColumn(currentTable, droppedColumn.Name, null);
                List<ColumnMigration> columnMigrations = GetColumnMigrations(currentTableColumn, null);
                foreach (ColumnMigration columnMigration in columnMigrations)
                {
                    columnsMigrations.Add(columnMigration);
                }
            }
            return columnsMigrations;
        }

        protected bool IsColumnInCurrentTable(TableDefinition currentTable, string dxmlColumnName, string dxmlColumnOldNames)
        {
            bool IsColumnInCurrentTable = false;
            if(dxmlColumnOldNames != null)
            {
                if (!dxmlColumnOldNames.Contains(","))
                {
                    IsColumnInCurrentTable = currentTable.Columns.Where(c => c.Name == dxmlColumnOldNames).Any();
                }
                else
                {
                    List<string> oldNames = dxmlColumnOldNames.Split(',').ToList();
                    IsColumnInCurrentTable = currentTable.Columns.Where(c => oldNames.Contains(c.Name)).Any();
                }
            }

            if (!IsColumnInCurrentTable)
            {
                IsColumnInCurrentTable = currentTable.Columns.Where(c => c.Name == dxmlColumnName).Any();
            }
            return IsColumnInCurrentTable;
        }
        
        protected ColumnDefinition GetCurrentTableColumn(TableDefinition currentTable, string dxmlColumnName, string dxmlColumnOldNames)
        {
            string columnName = dxmlColumnOldNames;
            bool IsColumnInCurrentTable = false;
            if(dxmlColumnOldNames != null)
            {
                if (!dxmlColumnOldNames.Contains(','))
                {
                    IsColumnInCurrentTable = currentTable.Columns.Where(c => c.Name == dxmlColumnOldNames).Any();
                }
                else
                {
                    List<string> oldNames = dxmlColumnOldNames.Split(',').ToList();
                    IsColumnInCurrentTable = currentTable.Columns.Where(c => oldNames.Contains(c.Name)).Any();
                }
            }

            if (!IsColumnInCurrentTable)
            {
                columnName = dxmlColumnName;
            }

            if (!columnName.Contains(','))
            {
                return currentTable.Columns.Where(c => c.Name == columnName).First();
            }
            else
            {
                List<string> names = columnName.Split(',').ToList();
                return currentTable.Columns.Where(c => names.Contains(c.Name)).First();
            }
        }

        protected ColumnMigration GetColumnMigration(int migrationType, ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            ColumnMigrationsDefinition currentColumn = currentTableColumn == null ? null : new ColumnMigrationsDefinition
            {
                Name = currentTableColumn.Name,
                Type = currentTableColumn.Type,
                Size = currentTableColumn.Size,
                Constraints = currentTableColumn.Constraints
            };
            ColumnMigrationsDefinition newColumn = dxmlTableColumn == null ? null : new ColumnMigrationsDefinition
            {
                Name = dxmlTableColumn.Name,
                Type = dxmlTableColumn.Type,
                Size = dxmlTableColumn.Size,
                Constraints = dxmlTableColumn.Constraints
            };
            ColumnMigration columnMigration = new ColumnMigration
            {
                MigrationType = migrationType,
                CurrentColumn = currentColumn,
                NewColumn = newColumn
            };
            return columnMigration;
        }

        protected List<ColumnMigration> GetColumnMigrations(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            List<ColumnMigration> columnMigrations = new List<ColumnMigration>();
            if(IsNotInCurrentTable(currentTableColumn))
            {
                ColumnMigration addMigration = GetColumnMigration(MigrationTypes.ADD, currentTableColumn, dxmlTableColumn);
                columnMigrations.Add(addMigration);
            }
            else if(IsNotInDXMLTable(dxmlTableColumn))
            {
                ColumnMigration dropMigration = GetColumnMigration(MigrationTypes.DROP, currentTableColumn, dxmlTableColumn);
                columnMigrations.Add(dropMigration);
            }
            else
            {
                columnMigrations = GetAlterColumnMigrations(currentTableColumn, dxmlTableColumn);
            }
            return columnMigrations;
        }

        protected bool IsNotInCurrentTable(ColumnDefinition currentTableColumn)
        {
            return currentTableColumn == null;
        }

        protected bool IsNotInDXMLTable(ColumnDefinition dxmlTableColumn)
        {
            return dxmlTableColumn == null;
        }

        protected List<ColumnMigration> GetAlterColumnMigrations(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            List<ColumnMigration> columnMigrations = new List<ColumnMigration>();
            bool IncludeUnsetNullableMigration = !(!currentTableColumn.Constraints.PrimaryKey && dxmlTableColumn.Constraints.PrimaryKey && currentTableColumn.Constraints.Nullable);

            List<ColumnMigration> alterTypeColumnMigrations = GetAlterTypeColumnMigrations(currentTableColumn, dxmlTableColumn);
            columnMigrations = columnMigrations.Concat(alterTypeColumnMigrations).ToList();

            List<ColumnMigration> alterSizeColumnMigrations = GetAlterSizeColumnMigrations(currentTableColumn, dxmlTableColumn);
            columnMigrations = columnMigrations.Concat(alterSizeColumnMigrations).ToList();

            List<ColumnMigration> dropPrimaryKeyColumnMigrations = GetDropPrimaryKeyColumnMigrations(currentTableColumn, dxmlTableColumn);
            columnMigrations = columnMigrations.Concat(dropPrimaryKeyColumnMigrations).ToList();

            List<ColumnMigration> addPrimaryKeyColumnMigrations = GetAddPrimaryKeyColumnMigrations(currentTableColumn, dxmlTableColumn);
            columnMigrations = columnMigrations.Concat(addPrimaryKeyColumnMigrations).ToList();

            List<ColumnMigration> unsetNullableColumnMigrations = GetUnsetNullableColumnMigrations(currentTableColumn, dxmlTableColumn, IncludeUnsetNullableMigration);
            columnMigrations = columnMigrations.Concat(unsetNullableColumnMigrations).ToList();

            List<ColumnMigration> setNullableColumnMigrations = GetSetNullableColumnMigrations(currentTableColumn, dxmlTableColumn);
            columnMigrations = columnMigrations.Concat(setNullableColumnMigrations).ToList();

            List<ColumnMigration> renameColumnMigrations = GetRenameColumnMigrations(currentTableColumn, dxmlTableColumn);
            columnMigrations = columnMigrations.Concat(renameColumnMigrations).ToList();

            return columnMigrations;
        }

        protected List<ColumnMigration> GetAlterTypeColumnMigrations(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            List<ColumnMigration> columnMigrations = new List<ColumnMigration>();
            if (currentTableColumn.Type != dxmlTableColumn.Type)
            {
                if (currentTableColumn.Constraints.PrimaryKey)
                {
                    ColumnMigration dropPrimaryKeyMigration = GetColumnMigration(MigrationTypes.DROPPRIMARYKEY, currentTableColumn, dxmlTableColumn);
                    columnMigrations.Add(dropPrimaryKeyMigration);
                }

                ColumnMigration alterTypeMigration = GetColumnMigration(MigrationTypes.ALTERTYPE, currentTableColumn, dxmlTableColumn);
                columnMigrations.Add(alterTypeMigration);

                if (currentTableColumn.Constraints.PrimaryKey)
                {
                    ColumnMigration addPrimaryKeyMigration = GetColumnMigration(MigrationTypes.ADDPRIMARYKEY, currentTableColumn, dxmlTableColumn);
                    columnMigrations.Add(addPrimaryKeyMigration);
                }
            }
            return columnMigrations;
        }

        protected List<ColumnMigration> GetAlterSizeColumnMigrations(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            List<ColumnMigration> columnMigrations = new List<ColumnMigration>();
            if (currentTableColumn.Size != dxmlTableColumn.Size && dxmlTableColumn.Size != 0)
            {
                if (currentTableColumn.Constraints.PrimaryKey)
                {
                    ColumnMigration dropPrimaryKeyMigration = GetColumnMigration(MigrationTypes.DROPPRIMARYKEY, currentTableColumn, dxmlTableColumn);
                    columnMigrations.Add(dropPrimaryKeyMigration);
                }

                ColumnMigration alterTypeMigration = GetColumnMigration(MigrationTypes.ALTERSIZE, currentTableColumn, dxmlTableColumn);
                columnMigrations.Add(alterTypeMigration);

                if (currentTableColumn.Constraints.PrimaryKey)
                {
                    ColumnMigration addPrimaryKeyMigration = GetColumnMigration(MigrationTypes.ADDPRIMARYKEY, currentTableColumn, dxmlTableColumn);
                    columnMigrations.Add(addPrimaryKeyMigration);
                }
            }
            return columnMigrations;
        }

        protected List<ColumnMigration> GetDropPrimaryKeyColumnMigrations(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            List<ColumnMigration> columnMigrations = new List<ColumnMigration>();
            if (currentTableColumn.Constraints.PrimaryKey && !dxmlTableColumn.Constraints.PrimaryKey)
            {
                ColumnMigration dropPrimaryKeyMigration = GetColumnMigration(MigrationTypes.DROPPRIMARYKEY, currentTableColumn, dxmlTableColumn);
                columnMigrations.Add(dropPrimaryKeyMigration);
            }
            return columnMigrations;
        }

        protected List<ColumnMigration> GetAddPrimaryKeyColumnMigrations(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            List<ColumnMigration> columnMigrations = new List<ColumnMigration>();
            if (!currentTableColumn.Constraints.PrimaryKey && dxmlTableColumn.Constraints.PrimaryKey)
            {
                if (currentTableColumn.Constraints.Nullable)
                {
                    ColumnMigration unsetNullableMigration = GetColumnMigration(MigrationTypes.UNSETNULLABLE, currentTableColumn, dxmlTableColumn);
                    columnMigrations.Add(unsetNullableMigration);
                }

                ColumnMigration addPrimaryKeyMigration = GetColumnMigration(MigrationTypes.ADDPRIMARYKEY, currentTableColumn, dxmlTableColumn);
                columnMigrations.Add(addPrimaryKeyMigration);
            }
            return columnMigrations;
        }

        protected List<ColumnMigration> GetUnsetNullableColumnMigrations(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn, bool IncludeUnsetNullableMigration)
        {
            List<ColumnMigration> columnMigrations = new List<ColumnMigration>();
            if (currentTableColumn.Constraints.Nullable && !dxmlTableColumn.Constraints.Nullable && IncludeUnsetNullableMigration)
            {
                ColumnMigration unsetNullableMigration = GetColumnMigration(MigrationTypes.UNSETNULLABLE, currentTableColumn, dxmlTableColumn);
                columnMigrations.Add(unsetNullableMigration);
            }
            return columnMigrations;
        }

        protected List<ColumnMigration> GetSetNullableColumnMigrations(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            List<ColumnMigration> columnMigrations = new List<ColumnMigration>();
            if (!currentTableColumn.Constraints.Nullable && dxmlTableColumn.Constraints.Nullable)
            {
                ColumnMigration setNullableMigration = GetColumnMigration(MigrationTypes.SETNULLABLE, currentTableColumn, dxmlTableColumn);
                columnMigrations.Add(setNullableMigration);
            }
            return columnMigrations;
        }

        protected List<ColumnMigration> GetRenameColumnMigrations(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            List<ColumnMigration> columnMigrations = new List<ColumnMigration>();
            if (currentTableColumn.Name != dxmlTableColumn.Name)
            {
                ColumnMigration renameMigration = GetColumnMigration(MigrationTypes.RENAME, currentTableColumn, dxmlTableColumn);
                columnMigrations.Add(renameMigration);
            }
            return columnMigrations;
        }

        protected string GenerateRandomString()
        {
            return Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "").ToUpper();
        }
        
        // abstract methods
        protected abstract TableDefinition GetCurrentTableDefinitionFromDB();

        protected abstract string GetCreateTableScript();

        protected abstract string GetAlterTableScript(TableDefinition currentTable);

        protected abstract string GetColumnMigrationScript(string tableName, ColumnMigration columnMigration, List<ColumnMigration> columnMigrations);

        protected abstract string GetDataTypeScript(string type, int size);

        protected abstract TableDefinition GetCurrentTableDefinitionFromDB(string tableName);
    }
}