using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace Logitude.DBMigrations.Models
{
    public class OracleDatabaseMigrations : DatabaseMigrations
    {
        protected string ConnectionString;
        
        public OracleDatabaseMigrations(TableDefinition table, string connectionString)
        {
            ConnectionString = connectionString;
            DXMLTable = table;
        }

        protected override TableDefinition GetCurrentTableDefinitionFromDB()
        {
            string queryString = @"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @name OR TABLE_NAME = @oldName";

            TableDefinition currentTable = null;

            SqlDataReader reader = null;
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@name", DXMLTable.Name);
            command.Parameters.AddWithValue("@oldName", DXMLTable.OldName ?? DXMLTable.Name);

            try
            {
                connection.Open();
                reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    string tableName = reader["TABLE_NAME"].ToString();
                    currentTable = GetCurrentTableDefinitionFromDB(tableName);
                }

                reader.Close();
                connection.Close();
            }
            catch (Exception exception)
            {
                if (reader != null)
                {
                    reader.Close();
                }
                connection.Close();

                ExitDatabaseMigrations(exception.Message);
            }

            return currentTable;
        }

        protected override TableDefinition GetCurrentTableDefinitionFromDB(string tableName)
        {
            string queryString = @"SELECT Q1.*, Q2.ConstraintType, Q2.ConstraintName " +
                                  "FROM ( " +
                                  "SELECT COL.COLUMN_NAME AS ColumnName, IS_NULLABLE AS Nullable, DATA_TYPE AS DataType, CHARACTER_MAXIMUM_LENGTH AS Size " +
                                  "FROM INFORMATION_SCHEMA.COLUMNS AS COL " +
                                  "WHERE COL.TABLE_NAME = @tableName " +
                                  ") AS Q1 " +
                                  "LEFT JOIN ( " +
                                  "SELECT CON.COLUMN_NAME AS ColumnName, TCON.CONSTRAINT_TYPE AS ConstraintType, TCON.CONSTRAINT_NAME AS ConstraintName " +
                                  "FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TCON " +
                                  "INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS CON ON TCON.CONSTRAINT_NAME = CON.CONSTRAINT_NAME " +
                                  "WHERE TCON.TABLE_NAME = @tableName " +
                                  ") AS Q2 ON Q2.ColumnName = Q1.ColumnName";

            TableDefinition currentTable = null;

            SqlDataReader reader = null;
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@tableName", tableName);

            try
            {
                connection.Open();
                reader = command.ExecuteReader();

                List<ColumnDefinition> currentTableColumns = new List<ColumnDefinition>();

                while (reader.Read())
                {
                    if (!currentTableColumns.Where(c => c.Name == reader["ColumnName"].ToString()).Any())
                    {
                        ColumnDefinition column = new ColumnDefinition
                        {
                            Name = reader["ColumnName"].ToString(),
                            Type = GetDxmlDataType(reader["DataType"].ToString()),
                            Size = !String.IsNullOrEmpty(reader["Size"].ToString()) ? (reader["Size"].ToString() == "-1" ? -1 : Convert.ToInt32(reader["Size"].ToString())) : 0,
                            Constraints = new ConstraintsDefinition
                            {
                                Nullable = (reader["Nullable"].ToString() == "YES")
                            }
                        };

                        if (!String.IsNullOrEmpty(reader["ConstraintType"].ToString()) && !String.IsNullOrEmpty(reader["ConstraintName"].ToString()))
                        {
                            column = SetConstraintForColumnDefinition(column, reader["ConstraintType"].ToString(), reader["ConstraintName"].ToString());
                        }

                        currentTableColumns.Add(column);
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(reader["ConstraintType"].ToString()) && !String.IsNullOrEmpty(reader["ConstraintName"].ToString()))
                        {
                            var column = currentTableColumns.Where(c => c.Name == reader["ColumnName"].ToString()).First();
                            column = SetConstraintForColumnDefinition(column, reader["ConstraintType"].ToString(), reader["ConstraintName"].ToString());
                        }
                    }
                }

                reader.Close();
                connection.Close();

                currentTable = new TableDefinition
                {
                    Name = tableName,
                    Columns = currentTableColumns
                };
            }
            catch (Exception exception)
            {
                if (reader != null)
                {
                    reader.Close();
                }
                connection.Close();

                ExitDatabaseMigrations(exception.Message);
            }

            return currentTable;
        }

        protected override string GetCreateTableScript()
        {
            string createTableScript = "-- Create New Table With Name " + DXMLTable.Name + "\n";
            createTableScript += "CREATE TABLE [" + DXMLTable.Schema + "].[" + DXMLTable.Name + "](" + "\n";
            foreach (var column in DXMLTable.Columns)
            {
                createTableScript += GetCreateColumnScript(column) + "\n";
            }

            if (DXMLTable.Columns.Where(c => c.Constraints.PrimaryKey).Any())
            {
                string primaryKeyColumns = string.Join(",", DXMLTable.Columns.Where(c => c.Constraints.PrimaryKey).Select(c => "[" + c.Name + "]").ToArray());
                createTableScript += "PRIMARY KEY(" + primaryKeyColumns + ")" + "\n";
            }
            createTableScript += ");" + "\n\n";
            return createTableScript;
        }

        protected override string GetCreateColumnScript(ColumnDefinition columnDefinition)
        {
            string columnScript = "[" + columnDefinition.Name + "]" + " ";
            columnScript += GetDataTypeScript(columnDefinition.Type, columnDefinition.Size);
            columnScript += columnDefinition.Constraints.Nullable ? " NULL" : " NOT NULL";
            columnScript += ",";
            return columnScript;
        }

        protected override string GetAlterTableScript()
        {
            TableMigrations = GetTableMigrations();

            string alterTableScript = "";

            alterTableScript += GetRenameTableScript();

            alterTableScript += GetAlterColumnsScript();

            if (AlterPrimaryKeyConstraint || PrimaryKeyColumnAdded)
            {
                alterTableScript += GetPrimaryKeyConstraintScript() + "\n\n";
            }

            return alterTableScript;
        }

        private string GetAlterColumnsScript()
        {
            string alterColumnsScript = "";
            foreach (var columnMigration in TableMigrations.ColumnsMigrations)
            {
                alterColumnsScript += GetAlterColumnScript(columnMigration);
            }
            return alterColumnsScript;
        }

        private bool CheckIfTableRenamed()
        {
            return TableMigrations.DxmlTableName != TableMigrations.CurrentTableName;
        }

        protected override string GetAlterColumnScript(ColumnMigration columnMigration)
        {
            string alterColumnScript = null;
            switch (columnMigration.MigrationType)
            {
                case MigrationTypes.ADD:
                    alterColumnScript = GetAddColumnScript(columnMigration);
                    return alterColumnScript;
                case MigrationTypes.RENAME:
                    alterColumnScript = GetRenameColumnScript(columnMigration);
                    return alterColumnScript;
                case MigrationTypes.DROP:
                    alterColumnScript = GetDropColumnScript(columnMigration);
                    return alterColumnScript;
                case MigrationTypes.ALTERTYPE:
                    alterColumnScript = GetAlterTypeScript(columnMigration);
                    return alterColumnScript;
                case MigrationTypes.ALTERSIZE:
                    alterColumnScript = GetAlterSizeScript(columnMigration);
                    return alterColumnScript;
                case MigrationTypes.ADDPRIMARYKEY:
                    alterColumnScript = GetAddPrimaryKeyScript(columnMigration);
                    return alterColumnScript;
                case MigrationTypes.DROPPRIMARYKEY:
                    alterColumnScript = GetDropPrimaryKeyScript(columnMigration);
                    return alterColumnScript;
                case MigrationTypes.SETNULLABLE:
                    alterColumnScript = GetSetNullableScript(columnMigration);
                    return alterColumnScript;
                case MigrationTypes.UNSETNULLABLE:
                    alterColumnScript = GetUnsetNullableScript(columnMigration);
                    return alterColumnScript;
                default:
                    return alterColumnScript;
            }
        }

        protected override string GetDataTypeScript(string type, int size)
        {
            switch (type)
            {
                case "int":
                    return "INT";
                case "decimal":
                    return "DECIMAL";
                case "timestamp":
                    return "TIMESTAMP";
                case "varbinary":
                    return "VARBINARY(" + (size == -1 ? "MAX" : size.ToString()) + ")";
                case "varchar":
                    return "VARCHAR(" + (size == -1 ? "MAX" : size.ToString()) + ")";
                case "datetime":
                    return "DATETIME";
                case "time":
                    return "TIME";
                case "float":
                    return "FLOAT";
                case "char":
                    return "CHAR(" + (size == -1 ? "MAX" : size.ToString()) + ")";
                case "bigint":
                    return "BIGINT";
                case "nvarchar":
                    return "NVARCHAR(" + (size == -1 ? "MAX" : size.ToString()) + ")";
                case "bit":
                    return "BIT";
                default:
                    return null;
            }
        }

        protected override string GetRenameTableScript()
        {
            string renameTableScript = "";
            if (CheckIfTableRenamed())
            {
                renameTableScript += "-- Rename Table From " + TableMigrations.CurrentTableName + " To " + TableMigrations.DxmlTableName + "\n";
                renameTableScript += "EXEC SP_RENAME '" + TableMigrations.CurrentTableName + "', '" + TableMigrations.DxmlTableName + "'";
                renameTableScript += "\n\n";
            }
            return renameTableScript;
        }

        protected override string GetAddColumnScript(ColumnMigration columnMigration)
        {
            string addScript = "-- Add New Column With Name " + columnMigration.NewColumn.Name + "\n";
            addScript += "ALTER TABLE " + "[" + TableMigrations.DxmlTableName + "]" + " ";
            addScript += "ADD " + "[" + columnMigration.NewColumn.Name + "]" + " ";
            addScript += GetDataTypeScript(columnMigration.NewColumn.Type, columnMigration.NewColumn.Size);
            addScript += columnMigration.NewColumn.Constraints.Nullable ? " NULL" : " NOT NULL";
            return addScript + "\n\n";
        }

        protected override string GetRenameColumnScript(ColumnMigration columnMigration)
        {
            string renameScript = "-- Rename Column From " + columnMigration.CurrentColumn.Name + " To " + columnMigration.NewColumn.Name + "\n";
            renameScript += "EXEC SP_RENAME '" + TableMigrations.DxmlTableName + "." + columnMigration.CurrentColumn.Name + "', '" + columnMigration.NewColumn.Name + "', 'COLUMN'";
            return renameScript + "\n\n";
        }

        protected override string GetDropColumnScript(ColumnMigration columnMigration)
        {
            string dropScript = "-- Drop Column " + columnMigration.CurrentColumn.Name + "\n";
            dropScript += "EXEC SP_RENAME '" + TableMigrations.DxmlTableName + "." + columnMigration.CurrentColumn.Name + "', '" + "Drop_" + columnMigration.CurrentColumn.Name + "', 'COLUMN'";
            return dropScript + "\n\n";
        }

        protected override string GetAlterTypeScript(ColumnMigration columnMigration)
        {
            string alterTypeScript = "-- Change Type From " + columnMigration.CurrentColumn.Type + " To " + columnMigration.NewColumn.Type + " For Column " + columnMigration.CurrentColumn.Name + "\n";
            alterTypeScript += "ALTER TABLE " + "[" + TableMigrations.DxmlTableName + "]" + " ";
            alterTypeScript += "ALTER COLUMN " + "[" + columnMigration.CurrentColumn.Name + "]" + " ";
            alterTypeScript += GetDataTypeScript(columnMigration.NewColumn.Type, columnMigration.CurrentColumn.Size == 0 ? 10 : columnMigration.CurrentColumn.Size);
            if (!columnMigration.CurrentColumn.Constraints.Nullable)
            {
                alterTypeScript += " NOT NULL";
            }
            return alterTypeScript + "\n\n";
        }

        protected override string GetAlterSizeScript(ColumnMigration columnMigration)
        {
            bool IsAlterTypeInMigrationsList = TableMigrations.ColumnsMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERTYPE).Any();
            string alterSizeScript = "-- Change Size From " + columnMigration.CurrentColumn.Size + " To " + columnMigration.NewColumn.Size + " For Column " + columnMigration.CurrentColumn.Name + "\n";
            alterSizeScript += "ALTER TABLE " + "[" + TableMigrations.DxmlTableName + "]" + " ";
            alterSizeScript += "ALTER COLUMN " + "[" + columnMigration.CurrentColumn.Name + "]" + " ";
            alterSizeScript += GetDataTypeScript((IsAlterTypeInMigrationsList ? columnMigration.NewColumn.Type : columnMigration.CurrentColumn.Type), columnMigration.NewColumn.Size);
            if (!columnMigration.CurrentColumn.Constraints.Nullable)
            {
                alterSizeScript += " NOT NULL";
            }
            return alterSizeScript + "\n\n";
        }

        protected override string GetAddPrimaryKeyScript(ColumnMigration columnMigration)
        {
            string primaryKeyColumns = string.Join(",", CurrentTable.Columns.Where(c => c.Constraints.PrimaryKey).Select(c => "[" + c.Name + "]").ToArray());
            string primaryKeyConstraintName = columnMigration.CurrentColumn.Constraints.PrimaryKeyConstraintName;
            string addPrimaryKeyScript = "-- Add The Primary Key Constraint\n";
            addPrimaryKeyScript += "EXEC('ALTER TABLE " + "[" + TableMigrations.DxmlTableName + "]" + " ADD CONSTRAINT " + primaryKeyConstraintName + " PRIMARY KEY (" + primaryKeyColumns + ")')";
            return addPrimaryKeyScript + "\n\n";
        }

        protected override string GetDropPrimaryKeyScript(ColumnMigration columnMigration)
        {
            string dropPrimaryKeyScript = "-- Drop The Primary Key Constraint\n";
            string primaryKeyConstraintName = columnMigration.CurrentColumn.Constraints.PrimaryKeyConstraintName;
            dropPrimaryKeyScript += "EXEC('ALTER TABLE " + "[" + TableMigrations.DxmlTableName + "]" + " DROP CONSTRAINT " + primaryKeyConstraintName + "')";
            return dropPrimaryKeyScript + "\n\n";
        }

        protected override string GetSetNullableScript(ColumnMigration columnMigration)
        {
            bool IsAlterTypeInMigrationsList = TableMigrations.ColumnsMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERTYPE).Any();
            bool IsAlterSizeInMigrationsList = TableMigrations.ColumnsMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERSIZE).Any();
            string setNullableScript = "-- Set Nullable For Column " + columnMigration.CurrentColumn.Name + "\n";
            if (columnMigration.CurrentColumn.Constraints.PrimaryKey)
            {
                setNullableScript += GetPrimaryKeyConstraintScript() + "\n";
            }
            setNullableScript += "ALTER TABLE " + "[" + TableMigrations.DxmlTableName + "]" + " ";
            setNullableScript += "ALTER COLUMN " + "[" + columnMigration.CurrentColumn.Name + "]" + " ";
            setNullableScript += GetDataTypeScript((IsAlterTypeInMigrationsList ? columnMigration.NewColumn.Type : columnMigration.CurrentColumn.Type), (IsAlterSizeInMigrationsList ? columnMigration.NewColumn.Size : columnMigration.CurrentColumn.Size));
            setNullableScript += " NULL";
            return setNullableScript + "\n\n";
        }

        protected override string GetUnsetNullableScript(ColumnMigration columnMigration)
        {
            bool IsAlterTypeInMigrationsList = TableMigrations.ColumnsMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERTYPE).Any();
            bool IsAlterSizeInMigrationsList = TableMigrations.ColumnsMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERSIZE).Any();
            string unsetNullableScript = "-- Unset Nullable For Column " + columnMigration.CurrentColumn.Name + "\n";
            unsetNullableScript += "ALTER TABLE " + "[" + TableMigrations.DxmlTableName + "]" + " ";
            unsetNullableScript += "ALTER COLUMN " + "[" + columnMigration.CurrentColumn.Name + "]" + " ";
            unsetNullableScript += GetDataTypeScript((IsAlterTypeInMigrationsList ? columnMigration.NewColumn.Type : columnMigration.CurrentColumn.Type), (IsAlterSizeInMigrationsList ? columnMigration.NewColumn.Size : columnMigration.CurrentColumn.Size));
            unsetNullableScript += " NOT NULL";
            return unsetNullableScript + "\n\n";
        }

        protected override string GetPrimaryKeyConstraintScript()
        {
            string alterPrimaryKeyScript = "";
            if (CheckIfTableHasPrimaryKeys(CurrentTable))
            {
                alterPrimaryKeyScript += GetAlterPrimaryKeyScript();
            }
            else if (CheckIfTableHasPrimaryKeys(DXMLTable))
            {
                alterPrimaryKeyScript += "-- Add Primary Key Constraint\n";
                string primaryKeyConstraintName = "PK_" + TableMigrations.DxmlTableName + "_" + GenerateRandomString();
                alterPrimaryKeyScript += GetAddPrimaryKeyConstraintScript(primaryKeyConstraintName); 
            }
            return alterPrimaryKeyScript;
        }

        protected override string GetAlterPrimaryKeyScript()
        {
            string alterPrimaryKeyScript = "-- Alter The Primary Key Constraint\n";
            ColumnDefinition columnHasPrimaryKey = CurrentTable.Columns.Where(c => c.Constraints.PrimaryKey).First();
            string primaryKeyConstraintName = columnHasPrimaryKey.Constraints.PrimaryKeyConstraintName;
            alterPrimaryKeyScript += GetDropPrimaryKeyConstraintScript(primaryKeyConstraintName);
            if (CheckIfTableHasPrimaryKeys(DXMLTable))
            {
                alterPrimaryKeyScript += GetAddPrimaryKeyConstraintScript(primaryKeyConstraintName);
            }
            return alterPrimaryKeyScript;
        }

        protected override string GetDropPrimaryKeyConstraintScript(string primaryKeyConstraintName)
        {
            string dropPrimaryKeyConstraintScript = "EXEC('ALTER TABLE " + "[" + TableMigrations.DxmlTableName + "]" + " DROP CONSTRAINT " + primaryKeyConstraintName + "')\n";
            return dropPrimaryKeyConstraintScript;
        }

        protected override string GetAddPrimaryKeyConstraintScript(string primaryKeyConstraintName)
        {
            string primaryKeyColumns = string.Join(",", DXMLTable.Columns.Where(c => c.Constraints.PrimaryKey).Select(c => "[" + c.Name + "]").ToArray());
            string primaryKeyConstraintScript = "EXEC('ALTER TABLE " + "[" + TableMigrations.DxmlTableName + "]" + " ADD CONSTRAINT " + primaryKeyConstraintName + " PRIMARY KEY (" + primaryKeyColumns + ")')";
            return primaryKeyConstraintScript;
        }

        protected override bool CheckIfTableHasPrimaryKeys(TableDefinition table)
        {
            return table.Columns.Where(c => c.Constraints.PrimaryKey).Any();
        }
    }
}