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

        protected TableMigrations GetTableMigrations()
        {
            BuildAddAndAlterMigrations();

            BuildDropMigrations();

            TableMigrations tableMigrations = CreateNewTableMigrations();

            return tableMigrations;
        }

        private TableMigrations CreateNewTableMigrations()
        {
            TableMigrations tableMigrations = new TableMigrations
            {
                DxmlTableName = DXMLTable.Name,
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
                ColumnDefinition currentTableColumn = GetCurrentTableColumn(droppedColumn.Name, null);
                BuildColumnMigrations(currentTableColumn, null);
            }
        }

        private void BuildAddAndAlterMigrations()
        {
            foreach (var dxmlTableColumn in DXMLTable.Columns)
            {
                ColumnDefinition currentTableColumn = null;
                if (IsColumnInCurrentTable(dxmlTableColumn.Name, dxmlTableColumn.OldNames))
                {
                    currentTableColumn = GetCurrentTableColumn(dxmlTableColumn.Name, dxmlTableColumn.OldNames);
                }

                BuildColumnMigrations(currentTableColumn, dxmlTableColumn);
            }
        }

        protected bool IsColumnInCurrentTable(string dxmlColumnName, string dxmlColumnOldNames)
        {
            bool IsColumnInCurrentTable = false;
            if (dxmlColumnOldNames != null)
            {
                if (!dxmlColumnOldNames.Contains(","))
                {
                    IsColumnInCurrentTable = CurrentTable.Columns.Where(c => c.Name == dxmlColumnOldNames).Any();
                }
                else
                {
                    List<string> oldNames = dxmlColumnOldNames.Split(',').ToList();
                    IsColumnInCurrentTable = CurrentTable.Columns.Where(c => oldNames.Contains(c.Name)).Any();
                }
            }

            if (!IsColumnInCurrentTable)
            {
                IsColumnInCurrentTable = CurrentTable.Columns.Where(c => c.Name == dxmlColumnName).Any();
            }
            return IsColumnInCurrentTable;
        }

        protected ColumnDefinition GetCurrentTableColumn(string dxmlColumnName, string dxmlColumnOldNames)
        {
            string columnName = dxmlColumnOldNames;
            bool IsColumnInCurrentTable = false;
            if (dxmlColumnOldNames != null)
            {
                if (!dxmlColumnOldNames.Contains(','))
                {
                    IsColumnInCurrentTable = CurrentTable.Columns.Where(c => c.Name == dxmlColumnOldNames).Any();
                }
                else
                {
                    List<string> oldNames = dxmlColumnOldNames.Split(',').ToList();
                    IsColumnInCurrentTable = CurrentTable.Columns.Where(c => oldNames.Contains(c.Name)).Any();
                }
            }

            if (!IsColumnInCurrentTable)
            {
                columnName = dxmlColumnName;
            }

            if (!columnName.Contains(','))
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
                Type = currentTableColumn.Type,
                Size = currentTableColumn.Size,
                Constraints = currentTableColumn.Constraints
            };
            ColumnMigrationDefinition newColumn = dxmlTableColumn == null ? null : new ColumnMigrationDefinition
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
            BuildAlterTypeMigration(currentTableColumn, dxmlTableColumn);

            BuildAlterSizeMigration(currentTableColumn, dxmlTableColumn);

            BuildUnsetNullableMigration(currentTableColumn, dxmlTableColumn);

            BuildSetNullableMigration(currentTableColumn, dxmlTableColumn);

            BuildRenameMigration(currentTableColumn, dxmlTableColumn);

            CheckAlterPrimaryKeyMigration(currentTableColumn, dxmlTableColumn);
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
            if (currentTableColumn.Name != dxmlTableColumn.Name)
            {
                ColumnMigration renameMigration = GetColumnMigration(MigrationTypes.RENAME, currentTableColumn, dxmlTableColumn);
                ColumnMigrations.Add(renameMigration);
            }
        }

        protected string GenerateRandomString()
        {
            return Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "").ToUpper();
        }



        // abstract methods
        protected abstract string GetCreateTableScript();

        protected abstract string GetCreateColumnScript(ColumnDefinition columnDefinition);

        protected abstract string GetAlterTableScript();

        protected abstract string GetAlterColumnScript(ColumnMigration columnMigration);

        protected abstract string GetDataTypeScript(string type, int size);

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

        protected abstract TableDefinition GetCurrentTableDefinitionFromDB();

        protected abstract TableDefinition GetCurrentTableDefinitionFromDB(string tableName);

        protected abstract string GetPrimaryKeyConstraintScript();

        protected abstract string GetAlterPrimaryKeyScript();

        protected abstract string GetDropPrimaryKeyConstraintScript(string primaryKeyConstraintName);

        protected abstract string GetAddPrimaryKeyConstraintScript(string primaryKeyConstraintName);

        protected abstract bool CheckIfTableHasPrimaryKeys(TableDefinition table);
    }
}