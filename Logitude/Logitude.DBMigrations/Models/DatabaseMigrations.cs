using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Logitude.DBMigrations.Models
{
    public abstract class DatabaseMigrations
    {
        protected TableDefinition DXMLTable;
        protected TableDefinition CurrentTable;
        protected TableMigrations TableMigrations;

        private List<ColumnMigration> ColumnMigrations = new List<ColumnMigration>();
        protected bool AlterPrimaryKeyConstraint = false;
        protected bool PrimaryKeyColumnAdded = false;

        public string GetScript()
        {
            CurrentTable = GetCurrentTableDefinitionFromDB();
            string script;
            if (CurrentTable == null)
            {
                script = GetCreateTableScript();
            }
            else
            {
                script = GetAlterTableScript();
            }
            return script;
        }

        protected string GetColumnDefinitionDataType(string dataType)
        {
            string formattedDataType = FormatDataTypeString(dataType).ToLower();
            switch (formattedDataType)
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
                case "date":
                    return "date";
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

        protected string GetColumnDefinitionDataType(string dataType, string precision, string scale)
        {
            string formattedDataType = FormatDataTypeString(dataType).ToLower();
            switch (formattedDataType)
            {
                case "date":
                    return "date";
                case "timestamp":
                    return "datetime";
                case "interval day to second":
                    return "time";
                case "blob":
                    return "varbinary";
                case "clob":
                case "varchar2":
                    return "varchar";
                case "nclob":
                case "nvarchar2":
                    return "nvarchar";
                case "raw":
                    return "timestamp";
                case "char":
                    return "char";
                case "number":
                    if(String.IsNullOrEmpty(precision) && String.IsNullOrEmpty(scale))
                    {
                        return "float";
                    }
                    else if(Convert.ToInt32(precision) == 1 && Convert.ToInt32(scale) == 0)
                    {
                        return "bit";
                    }
                    else if(Convert.ToInt32(precision) == 10 && Convert.ToInt32(scale) == 0)
                    {
                        return "int";
                    }
                    else if(Convert.ToInt32(precision) == 18 && Convert.ToInt32(scale) == 0)
                    {
                        return "bigint";
                    }
                    else
                    {
                        return "decimal";
                    }
                default:
                    return null;
            }
        }

        protected int GetColumnDefinitionSize(string size)
        {
            if (String.IsNullOrEmpty(size))
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(size);
            }
        }

        protected int GetColumnDefinitionSize(string dataType, string size)
        {
            string formattedDataType = FormatDataTypeString(dataType).ToLower();
            if (new string[] { "blob", "nclob", "clob" }.Contains(formattedDataType))
            {
                return -1;
            }
            else
            {
                if(formattedDataType == "char" && size == "2000")
                {
                    return -1;
                }
                return Convert.ToInt32(size);
            }
        }

        protected string FormatDataTypeString(string dataType)
        {
            Regex regex = new Regex(@"\((.*?)\)");
            string formattedDataType = regex.Replace(dataType, String.Empty);
            return formattedDataType;
        }

        protected ColumnDefinition SetConstraintForColumnDefinition(ColumnDefinition column, string constraintType, string constraintName)
        {
            switch (constraintType)
            {
                case "P":
                case "PRIMARY KEY":
                    column.Constraints.PrimaryKey = true;
                    column.Constraints.PrimaryKeyConstraintName = constraintName;
                    return column;
                default:
                    return column;
            }
        }

        protected TableMigrations GetTableMigrations()
        {
            BuildAddAndAlterMigrations();

            BuildDropMigrations();

            TableMigrations tableMigrations = CreateTableMigrations();

            return tableMigrations;
        }

        private TableMigrations CreateTableMigrations()
        {
            TableMigrations tableMigrations = new TableMigrations
            {
                DxmlTableName = DXMLTable.Name,
                DxmlTableShortName = DXMLTable.ShortName,
                CurrentTableName = CurrentTable.Name,
                ColumnsMigrations = ColumnMigrations
            };

            return tableMigrations;
        }

        private void BuildDropMigrations()
        {
            List<ColumnDefinition> droppedColumns = GetDroppedColumns();

            foreach (var droppedColumn in droppedColumns)
            {
                ColumnDefinition currentTableColumn = GetCurrentTableColumn(droppedColumn.Name, null, null);
                BuildColumnMigrations(currentTableColumn, null);
            }
        }

        private void BuildAddAndAlterMigrations()
        {
            foreach (var dxmlTableColumn in DXMLTable.Columns)
            {
                ColumnDefinition currentTableColumn = null;
                if (IsColumnInCurrentTable(dxmlTableColumn.Name, dxmlTableColumn.ShortName, dxmlTableColumn.OldNames))
                {
                    currentTableColumn = GetCurrentTableColumn(dxmlTableColumn.Name, dxmlTableColumn.ShortName, dxmlTableColumn.OldNames);
                }

                BuildColumnMigrations(currentTableColumn, dxmlTableColumn);
            }
        }

        protected bool IsColumnInCurrentTable(string dxmlColumnName, string dxmlColumnShortName, string dxmlColumnOldNames)
        {
            bool isColumnInCurrentTable = false;
            if (dxmlColumnOldNames != null)
            {
                if (!dxmlColumnOldNames.Contains(","))
                {
                    isColumnInCurrentTable = CurrentTable.Columns.Where(c => c.Name == dxmlColumnOldNames).Any();
                }
                else
                {
                    List<string> oldNames = dxmlColumnOldNames.Split(',').ToList();
                    isColumnInCurrentTable = CurrentTable.Columns.Where(c => oldNames.Contains(c.Name)).Any();
                }
            }

            if (!isColumnInCurrentTable)
            {
                isColumnInCurrentTable = CurrentTable.Columns.Where(c => c.Name == dxmlColumnName).Any();
            }

            if (!isColumnInCurrentTable && dxmlColumnShortName != null && dxmlColumnName.Length > 30)
            {
                isColumnInCurrentTable = CurrentTable.Columns.Where(c => c.Name == dxmlColumnShortName).Any();
            }

            return isColumnInCurrentTable;
        }

        protected ColumnDefinition GetCurrentTableColumn(string dxmlColumnName, string dxmlColumnShortName, string dxmlColumnOldNames)
        {
            string columnName = dxmlColumnOldNames;
            bool isColumnInCurrentTable = false;
            if (dxmlColumnOldNames != null)
            {
                if (!dxmlColumnOldNames.Contains(','))
                {
                    isColumnInCurrentTable = CurrentTable.Columns.Where(c => c.Name == dxmlColumnOldNames).Any();
                }
                else
                {
                    List<string> oldNames = dxmlColumnOldNames.Split(',').ToList();
                    isColumnInCurrentTable = CurrentTable.Columns.Where(c => oldNames.Contains(c.Name)).Any();
                }
            }

            if (!isColumnInCurrentTable && CurrentTable.Columns.Where(c => c.Name == dxmlColumnName).Any())
            {
                columnName = dxmlColumnName;
                isColumnInCurrentTable = true;
            }

            if (!isColumnInCurrentTable && dxmlColumnShortName != null && dxmlColumnName.Length > 30 && CurrentTable.Columns.Where(c => c.Name == dxmlColumnShortName).Any())
            {
                columnName = dxmlColumnShortName;
            }

            if (!columnName.Contains(","))
            {
                return CurrentTable.Columns.Where(c => c.Name == columnName).First();
            }
            else
            {
                List<string> names = columnName.Split(',').ToList();
                return CurrentTable.Columns.Where(c => names.Contains(c.Name)).First();
            }
        }

        protected ColumnMigration GetColumnMigration(int migrationType, ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            ColumnMigrationDefinition currentColumn = currentTableColumn == null ? null : new ColumnMigrationDefinition
            {
                Name = currentTableColumn.Name,
                ShortName = currentTableColumn.ShortName,
                Type = currentTableColumn.Type,
                Size = currentTableColumn.Size,
                Precision = currentTableColumn.Precision,
                Scale = currentTableColumn.Scale,
                Constraints = currentTableColumn.Constraints
            };
            ColumnMigrationDefinition newColumn = dxmlTableColumn == null ? null : new ColumnMigrationDefinition
            {
                Name = dxmlTableColumn.Name,
                ShortName = dxmlTableColumn.ShortName,
                Type = dxmlTableColumn.Type,
                Size = dxmlTableColumn.Size,
                Precision = dxmlTableColumn.Precision,
                Scale = dxmlTableColumn.Scale,
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

        protected bool IsNotInCurrentTable(ColumnDefinition currentTableColumn)
        {
            return currentTableColumn == null;
        }

        protected bool IsNotInDXMLTable(ColumnDefinition dxmlTableColumn)
        {
            return dxmlTableColumn == null;
        }

        protected List<ColumnDefinition> GetDroppedColumns()
        {
            List<string> dxmlTableColumnsNames = DXMLTable.Columns.Select(c => c.Name).ToList();
            List<string> dxmlTableColumnsShortNames = DXMLTable.Columns.Where(c => c.ShortName != null).Select(c => c.ShortName).ToList();
            List<string> dxmlTableColumnsOldNames = DXMLTable.Columns.Where(c => c.OldNames != null).Select(c => c.OldNames).ToList();
            foreach (var name in dxmlTableColumnsOldNames)
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

            dxmlTableColumnsNames = dxmlTableColumnsNames.Concat(dxmlTableColumnsShortNames).ToList();

            List<ColumnDefinition> droppedColumns = CurrentTable.Columns.Where(c => !dxmlTableColumnsNames.Contains(c.Name) && !c.Name.StartsWith("Drop_")).ToList();
            return droppedColumns;
        }

        protected void BuildColumnMigrations(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            if (IsNotInCurrentTable(currentTableColumn))
            {
                BuildAddColumnMigration(currentTableColumn, dxmlTableColumn);
            }
            else if (IsNotInDXMLTable(dxmlTableColumn))
            {
                BuildDropColumnMigration(currentTableColumn, dxmlTableColumn);
            }
            else
            {
                BuildAlterMigration(currentTableColumn, dxmlTableColumn);
            }
        }

        protected void BuildAddColumnMigration(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            ColumnMigration addMigration = GetColumnMigration(MigrationTypes.ADD, currentTableColumn, dxmlTableColumn);
            ColumnMigrations.Add(addMigration);
            if (dxmlTableColumn.Constraints.PrimaryKey)
            {
                PrimaryKeyColumnAdded = true;
            }
        }

        protected void BuildDropColumnMigration(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            ColumnMigration dropMigration = GetColumnMigration(MigrationTypes.DROP, currentTableColumn, dxmlTableColumn);
            ColumnMigrations.Add(dropMigration);
        }

        protected void BuildAlterMigration(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            BuildAlterPrecisionAndScaleMigration(currentTableColumn, dxmlTableColumn);

            BuildAlterTypeMigration(currentTableColumn, dxmlTableColumn);

            BuildAlterSizeMigration(currentTableColumn, dxmlTableColumn);

            BuildUnsetNullableMigration(currentTableColumn, dxmlTableColumn);

            BuildSetNullableMigration(currentTableColumn, dxmlTableColumn);

            BuildRenameMigration(currentTableColumn, dxmlTableColumn);

            CheckAlterPrimaryKeyMigration(currentTableColumn, dxmlTableColumn);
        }

        protected void BuildAlterPrecisionAndScaleMigration(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            if (currentTableColumn.Type == "decimal" && dxmlTableColumn.Type == "decimal" && (currentTableColumn.Precision != dxmlTableColumn.Precision || currentTableColumn.Scale != dxmlTableColumn.Scale))
            {
                if (currentTableColumn.Constraints.PrimaryKey)
                {
                    ColumnMigration dropPrimaryKeyMigration = GetColumnMigration(MigrationTypes.DROPPRIMARYKEY, currentTableColumn, dxmlTableColumn);
                    ColumnMigrations.Add(dropPrimaryKeyMigration);
                }

                ColumnMigration alterPrecisionAndScaleMigration = GetColumnMigration(MigrationTypes.ALTERPRECISIONANDSCALE, currentTableColumn, dxmlTableColumn);
                ColumnMigrations.Add(alterPrecisionAndScaleMigration);

                if (currentTableColumn.Constraints.PrimaryKey)
                {
                    ColumnMigration addPrimaryKeyMigration = GetColumnMigration(MigrationTypes.ADDPRIMARYKEY, currentTableColumn, dxmlTableColumn);
                    ColumnMigrations.Add(addPrimaryKeyMigration);
                }
            }
        }

        protected void BuildAlterTypeMigration(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            if (currentTableColumn.Type != dxmlTableColumn.Type)
            {
                if (currentTableColumn.Constraints.PrimaryKey)
                {
                    ColumnMigration dropPrimaryKeyMigration = GetColumnMigration(MigrationTypes.DROPPRIMARYKEY, currentTableColumn, dxmlTableColumn);
                    ColumnMigrations.Add(dropPrimaryKeyMigration);
                }

                ColumnMigration alterTypeMigration = GetColumnMigration(MigrationTypes.ALTERTYPE, currentTableColumn, dxmlTableColumn);
                ColumnMigrations.Add(alterTypeMigration);

                if (currentTableColumn.Constraints.PrimaryKey)
                {
                    ColumnMigration addPrimaryKeyMigration = GetColumnMigration(MigrationTypes.ADDPRIMARYKEY, currentTableColumn, dxmlTableColumn);
                    ColumnMigrations.Add(addPrimaryKeyMigration);
                }
            }
        }

        protected void BuildAlterSizeMigration(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            if (currentTableColumn.Size != dxmlTableColumn.Size && dxmlTableColumn.Size != 0)
            {
                if (currentTableColumn.Constraints.PrimaryKey)
                {
                    ColumnMigration dropPrimaryKeyMigration = GetColumnMigration(MigrationTypes.DROPPRIMARYKEY, currentTableColumn, dxmlTableColumn);
                    ColumnMigrations.Add(dropPrimaryKeyMigration);
                }

                ColumnMigration alterTypeMigration = GetColumnMigration(MigrationTypes.ALTERSIZE, currentTableColumn, dxmlTableColumn);
                ColumnMigrations.Add(alterTypeMigration);

                if (currentTableColumn.Constraints.PrimaryKey)
                {
                    ColumnMigration addPrimaryKeyMigration = GetColumnMigration(MigrationTypes.ADDPRIMARYKEY, currentTableColumn, dxmlTableColumn);
                    ColumnMigrations.Add(addPrimaryKeyMigration);
                }
            }
        }

        protected void CheckAlterPrimaryKeyMigration(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            if (currentTableColumn.Constraints.PrimaryKey != dxmlTableColumn.Constraints.PrimaryKey)
            {
                AlterPrimaryKeyConstraint = true;
            }
        }

        protected void BuildUnsetNullableMigration(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            if (currentTableColumn.Constraints.Nullable && !dxmlTableColumn.Constraints.Nullable)
            {
                ColumnMigration unsetNullableMigration = GetColumnMigration(MigrationTypes.UNSETNULLABLE, currentTableColumn, dxmlTableColumn);
                ColumnMigrations.Add(unsetNullableMigration);
            }
        }

        protected void BuildSetNullableMigration(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            if (!currentTableColumn.Constraints.Nullable && dxmlTableColumn.Constraints.Nullable)
            {
                ColumnMigration setNullableMigration = GetColumnMigration(MigrationTypes.SETNULLABLE, currentTableColumn, dxmlTableColumn);
                ColumnMigrations.Add(setNullableMigration);
            }
        }

        protected void BuildRenameMigration(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            if (currentTableColumn.Name != FormatNameLength(dxmlTableColumn.Name, dxmlTableColumn.ShortName))
            {
                ColumnMigration renameMigration = GetColumnMigration(MigrationTypes.RENAME, currentTableColumn, dxmlTableColumn);
                ColumnMigrations.Add(renameMigration);
            }
        }

        protected string GenerateRandomString()
        {
            return Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "").ToUpper();//21 chars
        }

        protected string FormatNameLength(string name, string shortName)
        {
            if(name.Length <= 30)
            {
                return name;
            }
            else
            {
                if(!String.IsNullOrEmpty(shortName))
                {
                    return shortName;
                }
                else
                {
                    return name;
                }
            }
        }

        protected void ExitDatabaseMigrations(string message)
        {
            Console.WriteLine(message);
            Environment.Exit(0);
        }



        //abstract methods
        protected abstract TableDefinition GetCurrentTableDefinitionFromDB();

        protected abstract TableDefinition GetCurrentTableDefinitionFromDB(string tableName);

        protected abstract string GetCreateTableScript();

        protected abstract string GetCreateColumnScript(ColumnDefinition columnDefinition);

        protected abstract string GetAlterTableScript();

        protected abstract string GetAlterColumnScript(ColumnMigration columnMigration);

        protected abstract string GetDataTypeScript(string type, int size, int precision, int scale);

        protected abstract string GetRenameTableScript();

        protected abstract string GetAddColumnScript(ColumnMigration columnMigration);

        protected abstract string GetRenameColumnScript(ColumnMigration columnMigration);

        protected abstract string GetDropColumnScript(ColumnMigration columnMigration);

        protected abstract string GetAlterTypeScript(ColumnMigration columnMigration);

        protected abstract string GetAlterSizeScript(ColumnMigration columnMigration);

        protected abstract string GetAddPrimaryKeyScript(ColumnMigration columnMigration);

        protected abstract string GetDropPrimaryKeyScript(ColumnMigration columnMigration);

        protected abstract string GetSetNullableScript(ColumnMigration columnMigration);

        protected abstract string GetUnsetNullableScript(ColumnMigration columnMigration);

        protected abstract string GetAlterPrecisionAndScaleScript(ColumnMigration columnMigration);

        protected abstract string GetAlterColumnsScript();

        protected abstract string GetPrimaryKeyConstraintScript();

        protected abstract string GetAlterPrimaryKeyScript();

        protected abstract string GetDropPrimaryKeyConstraintScript(string primaryKeyConstraintName);

        protected abstract string GetAddPrimaryKeyConstraintScript(string primaryKeyConstraintName);

        protected abstract bool CheckIfTableHasPrimaryKeys(TableDefinition table);

        protected abstract bool CheckIfTableRenamed();
    }
}