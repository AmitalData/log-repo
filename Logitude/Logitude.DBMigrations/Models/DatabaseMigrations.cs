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
            List<ColumnMigrations> columnsMigrations = new List<ColumnMigrations>();
            
            foreach (var dxmlTableColumn in dxmlTable.Columns)
            {
                var result = IsColumnInCurrentTable(currentTable, dxmlTableColumn.Name, dxmlTableColumn.OldName);
                if (!IsColumnInCurrentTable(currentTable, dxmlTableColumn.Name, dxmlTableColumn.OldName))
                {
                    List<ColumnMigrations> columnMigrations = GetColumnMigrations(null, dxmlTableColumn);
                    foreach(ColumnMigrations columnMigration in columnMigrations)
                    {
                        columnsMigrations.Add(columnMigration);
                    }
                }
                else
                {
                    ColumnDefinition currentTableColumn = GetCurrentTableColumn(currentTable, dxmlTableColumn.Name, dxmlTableColumn.OldName);
                    List<ColumnMigrations> columnMigrations = GetColumnMigrations(currentTableColumn, dxmlTableColumn);
                    foreach (ColumnMigrations columnMigration in columnMigrations)
                    {
                        columnsMigrations.Add(columnMigration);
                    }
                }
            }

            columnsMigrations = IncludeDropColumnsMigrations(currentTable, dxmlTable, columnsMigrations);

            TableMigrations tableMigrations = new TableMigrations
            {
                TableName = dxmlTable.Name,
                ColumnsMigrations = columnsMigrations
            };
            return tableMigrations;
        }
        
        protected List<ColumnMigrations> IncludeDropColumnsMigrations(TableDefinition currentTable, TableDefinition dxmlTable, List<ColumnMigrations> columnsMigrations)
        {
            List<string> dxmlTableColumnsNames = dxmlTable.Columns.Select(c => (c.Name)).ToList();
            dxmlTableColumnsNames = dxmlTableColumnsNames.Concat(dxmlTable.Columns.Where(a => a.OldName != null).Select(c => (c.OldName)).ToList()).ToList();
            List<ColumnDefinition> droppedColumns = currentTable.Columns.Where(c => !dxmlTableColumnsNames.Contains(c.Name) && !c.Name.StartsWith("Drop_")).ToList();

            foreach (var droppedColumn in droppedColumns)
            {
                //ColumnDefinition currentTableColumn = currentTable.Columns.Where(c => c.Name == droppedColumn.Name).First();
                ColumnDefinition currentTableColumn = GetCurrentTableColumn(currentTable, droppedColumn.Name, null);
                List<ColumnMigrations> columnMigrations = GetColumnMigrations(currentTableColumn, null);
                foreach (ColumnMigrations columnMigration in columnMigrations)
                {
                    columnsMigrations.Add(columnMigration);
                }
            }

            return columnsMigrations;
        }

        protected bool IsColumnInCurrentTable(TableDefinition currentTable, string dxmlColumnName, string dxmlColumnOldName)
        {
            bool IsColumnInCurrentTable = currentTable.Columns.Where(c => c.Name == dxmlColumnOldName).Any();

            if (!IsColumnInCurrentTable)
            {
                IsColumnInCurrentTable = currentTable.Columns.Where(c => c.Name == dxmlColumnName).Any();
            }

            return IsColumnInCurrentTable;
        }
        
        protected ColumnDefinition GetCurrentTableColumn(TableDefinition currentTable, string dxmlColumnName, string dxmlColumnOldName)
        {
            string name = dxmlColumnOldName;
            bool IsColumnInCurrentTable = currentTable.Columns.Where(c => c.Name == dxmlColumnOldName).Any();

            if (!IsColumnInCurrentTable)
            {
                IsColumnInCurrentTable = currentTable.Columns.Where(c => c.Name == dxmlColumnName).Any();
                name = dxmlColumnName;
            }

            //return IsColumnInCurrentTable;
            //string name = dxmlColumnOldName == null ? dxmlColumnName : dxmlColumnOldName;
            return currentTable.Columns.Where(c => c.Name == name).First();
        }

        protected List<ColumnMigrations> GetColumnMigrations(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            bool IncludeUnsetNullableMigration = true;
            List<ColumnMigrations> columnMigrations = new List<ColumnMigrations>();
            ColumnMigrationsDefinition currentColumn = null;
            ColumnMigrationsDefinition newColumn = null;

            if(currentTableColumn != null)
            {
                currentColumn = new ColumnMigrationsDefinition
                {
                    Name = currentTableColumn.Name,
                    Type = currentTableColumn.Type,
                    Size = currentTableColumn.Size,
                    Constraints = currentTableColumn.Constraints
                };//from db
            }
            else
            {
                newColumn = new ColumnMigrationsDefinition
                {
                    Name = dxmlTableColumn.Name,
                    Type = dxmlTableColumn.Type,
                    Size = dxmlTableColumn.Size,
                    Constraints = dxmlTableColumn.Constraints
                };//from dxml
                columnMigrations.Add(new ColumnMigrations
                {
                    MigrationType = MigrationTypes.ADD,
                    CurrentColumn = currentColumn,
                    NewColumn = newColumn
                });
                return columnMigrations;
            }

            if(dxmlTableColumn != null)
            {
                newColumn = new ColumnMigrationsDefinition
                {
                    Name = dxmlTableColumn.Name,
                    Type = dxmlTableColumn.Type,
                    Size = dxmlTableColumn.Size,
                    Constraints = dxmlTableColumn.Constraints
                };//from dxml
            }
            else
            {
                columnMigrations.Add(new ColumnMigrations
                {
                    MigrationType = MigrationTypes.DROP,
                    CurrentColumn = currentColumn,
                    NewColumn = newColumn
                });
               return columnMigrations;
            }

            if (currentTableColumn.Type != dxmlTableColumn.Type)
            {
                if (currentTableColumn.Constraints.PrimaryKey)
                {
                    columnMigrations.Add(new ColumnMigrations
                    {
                        MigrationType = MigrationTypes.DROPPRIMARYKEY,
                        CurrentColumn = currentColumn,
                        NewColumn = newColumn
                    });
                }
                columnMigrations.Add(new ColumnMigrations
                {
                    MigrationType = MigrationTypes.ALTERTYPE,
                    CurrentColumn = currentColumn,
                    NewColumn = newColumn
                });
                if (currentTableColumn.Constraints.PrimaryKey)
                {
                    columnMigrations.Add(new ColumnMigrations
                    {
                        MigrationType = MigrationTypes.ADDPRIMARYKEY,
                        CurrentColumn = currentColumn,
                        NewColumn = newColumn
                    });
                }
            }

            if (currentTableColumn.Size != dxmlTableColumn.Size)
            {
                if (currentTableColumn.Constraints.PrimaryKey)
                {
                    columnMigrations.Add(new ColumnMigrations
                    {
                        MigrationType = MigrationTypes.DROPPRIMARYKEY,
                        CurrentColumn = currentColumn,
                        NewColumn = newColumn
                    });
                }
                columnMigrations.Add(new ColumnMigrations
                {
                    MigrationType = MigrationTypes.ALTERSIZE,
                    CurrentColumn = currentColumn,
                    NewColumn = newColumn
                });
                if (currentTableColumn.Constraints.PrimaryKey)
                {
                    columnMigrations.Add(new ColumnMigrations
                    {
                        MigrationType = MigrationTypes.ADDPRIMARYKEY,
                        CurrentColumn = currentColumn,
                        NewColumn = newColumn
                    });
                }
            }





            if (currentTableColumn.Constraints.PrimaryKey && !dxmlTableColumn.Constraints.PrimaryKey)
            {
                columnMigrations.Add(new ColumnMigrations
                {
                    MigrationType = MigrationTypes.DROPPRIMARYKEY,
                    CurrentColumn = currentColumn,
                    NewColumn = newColumn
                });
            }

            if (!currentTableColumn.Constraints.PrimaryKey && dxmlTableColumn.Constraints.PrimaryKey)
            {
                if (currentTableColumn.Constraints.Nullable)
                {
                    columnMigrations.Add(new ColumnMigrations
                    {
                        MigrationType = MigrationTypes.UNSETNULLABLE,
                        CurrentColumn = currentColumn,
                        NewColumn = newColumn
                    });

                    IncludeUnsetNullableMigration = false;
                }
                columnMigrations.Add(new ColumnMigrations
                {
                    MigrationType = MigrationTypes.ADDPRIMARYKEY,
                    CurrentColumn = currentColumn,
                    NewColumn = newColumn
                });
            }

            if (currentTableColumn.Constraints.Nullable && !dxmlTableColumn.Constraints.Nullable && IncludeUnsetNullableMigration)
            {
                columnMigrations.Add(new ColumnMigrations
                {
                    MigrationType = MigrationTypes.UNSETNULLABLE,
                    CurrentColumn = currentColumn,
                    NewColumn = newColumn
                });
            }

            if (!currentTableColumn.Constraints.Nullable && dxmlTableColumn.Constraints.Nullable)
            {
                columnMigrations.Add(new ColumnMigrations
                {
                    MigrationType = MigrationTypes.SETNULLABLE,
                    CurrentColumn = currentColumn,
                    NewColumn = newColumn
                });
            }

            if (currentTableColumn.Name != dxmlTableColumn.Name)
            {
                columnMigrations.Add(new ColumnMigrations
                {
                    MigrationType = MigrationTypes.RENAME,
                    CurrentColumn = currentColumn,
                    NewColumn = newColumn
                });
            }

            return columnMigrations;
        }


        protected string GenerateIdForConstraint()
        {
            return Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
        }

        //protected bool IsThereMigration(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        //{
        //    if(dxmlTableColumn.Type != currentTableColumn.Type)
        //    {
        //        return true;
        //    }
        //    if(dxmlTableColumn.Size != currentTableColumn.Size)
        //    {
        //        return true;
        //    }
        //    if(dxmlTableColumn.Constraints.PrimaryKey != currentTableColumn.Constraints.PrimaryKey)
        //    {
        //        return true;
        //    }
        //    if(dxmlTableColumn.Constraints.Nullable != currentTableColumn.Constraints.Nullable)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        //private ColumnMigrations GetAddColumnMigrations()
        //{
        //    ColumnMigrations columnMigrations = GetColumnMigrations(MigrationTypes.ADDCOLUMN, dxmlTableColumn);
        //    columnsMigrations.Add(columnMigrations);
        //}

        // abstract Classes 

        protected abstract TableDefinition GetCurrentTableDefinitionFromDB();

        protected abstract string GetCreateTableScript();

        protected abstract string GetAlterTableScript(TableDefinition currentTable);

        protected abstract string GetColumnMigrationScript(string tableName, ColumnMigrations columnMigration, List<ColumnMigrations> columnMigrations);

        protected abstract string GetDataTypeScript(string type, int size);

        protected abstract TableDefinition GetCurrentTableDefinitionFromDB(string tableName);

    }
}
