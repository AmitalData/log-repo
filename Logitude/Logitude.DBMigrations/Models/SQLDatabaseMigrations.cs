using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace Logitude.DBMigrations.Models
{
    public class SQLDatabaseMigrations : DatabaseMigrations
    {
        protected readonly string connectionString = ConfigurationManager.AppSettings["ConnectionString"];

        public SQLDatabaseMigrations(TableDefinition table)
        {
            DXMLTable = table;
        }

        protected override TableDefinition GetCurrentTableDefinitionFromDB()
        {
            string queryString = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @name OR TABLE_NAME = @oldName";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                TableDefinition currentTable = null;
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = null;
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
                catch (Exception)
                {
                    reader.Close();
                    connection.Close();
                }

                return currentTable;
            }
        }

        protected override TableDefinition GetCurrentTableDefinitionFromDB(string tableName)
        {
            string queryString = @"SELECT COL.COLUMN_NAME AS ColumnName, COL.IS_NULLABLE AS Nullable, COL.DATA_TYPE AS DataType, COL.CHARACTER_MAXIMUM_LENGTH AS Size, CON.CONSTRAINT_NAME AS ConstraintName, TCON.CONSTRAINT_TYPE AS ConstraintType " +
                                  "FROM INFORMATION_SCHEMA.COLUMNS COL LEFT OUTER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE CON ON COL.COLUMN_NAME = CON.COLUMN_NAME LEFT OUTER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS TCON ON CON.CONSTRAINT_NAME = TCON.CONSTRAINT_NAME " +
                                  "WHERE COL.TABLE_NAME = @tableName AND(CON.TABLE_NAME = @tableName OR CON.TABLE_NAME IS NULL)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                TableDefinition currentTable = null;
                List<ColumnDefinition> currentTableColumns = new List<ColumnDefinition>();
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = null;
                command.Parameters.AddWithValue("@tableName", tableName);

                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();

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
                catch (Exception)
                {
                    reader.Close();
                    connection.Close();
                }

                return currentTable;
            }
        }

        protected override string GetCreateTableScript()
        {
            string createTableScript = "-- Create New Table With Name " + DXMLTable.Name + "\n";
            createTableScript += "CREATE TABLE " + DXMLTable.Name + "(" + "\n";
            foreach (var column in DXMLTable.Columns)
            {
                createTableScript += GetCreateColumnScript(column) + "\n";
            }

            if(DXMLTable.Columns.Where(c => c.Constraints.PrimaryKey).Any())
            {
                string primaryKeyColumns = string.Join(",", DXMLTable.Columns.Where(c => c.Constraints.PrimaryKey).Select(c => c.Name).ToArray());
                createTableScript += "PRIMARY KEY(" + primaryKeyColumns + ")" + "\n";
            }
            createTableScript += ")" + "\n\n";
            return createTableScript;
        }

        protected override string GetCreateColumnScript(ColumnDefinition columnDefinition)
        {
            string columnScript = columnDefinition.Name + " ";
            columnScript += GetDataTypeScript(columnDefinition.Type, columnDefinition.Size);
            columnScript += columnDefinition.Constraints.Nullable ? " NULL" : " NOT NULL";
            columnScript += ",";
            return columnScript;
        }

        protected override string GetAlterTableScript()
        {
            TableMigrations = GetTableMigrations();

            string alterTableScript = "";

            if(TableMigrations.DxmlTableName != TableMigrations.CurrentTableName)
            {
                alterTableScript += GetRenameTableScript() + "\n\n";
            }

            foreach (var columnMigration in TableMigrations.ColumnsMigrations)
            {
                alterTableScript += GetAlterColumnScript(columnMigration) + "\n\n";
            }

            return alterTableScript;
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
            string renameTableScript = "-- Rename Table From " + TableMigrations.CurrentTableName + " To " + TableMigrations.DxmlTableName + "\n";
            renameTableScript += "EXEC SP_RENAME '" + TableMigrations.CurrentTableName + "', '" + TableMigrations.DxmlTableName + "'";
            return renameTableScript;
        }

        protected override string GetAddColumnScript(ColumnMigration columnMigration)
        {
            string addScript = "-- Add New Column With Name " + columnMigration.NewColumn.Name + "\n";
            addScript += "ALTER TABLE " + TableMigrations.DxmlTableName + " ";
            addScript += "ADD " + columnMigration.NewColumn.Name + " ";
            addScript += GetDataTypeScript(columnMigration.NewColumn.Type, columnMigration.NewColumn.Size);
            //addScript += columnMigration.NewColumn.Constraints.Nullable ? " NULL" : " NOT NULL";
            addScript += GetConstraintsScript(columnMigration.NewColumn.Constraints);
            return addScript;
        }
        
        protected override string GetRenameColumnScript(ColumnMigration columnMigration)
        {
            string renameScript = "-- Rename Column From " + columnMigration.CurrentColumn.Name + " To " + columnMigration.NewColumn.Name + "\n";
            renameScript += "EXEC SP_RENAME '" + TableMigrations.DxmlTableName + "." + columnMigration.CurrentColumn.Name + "', '" + columnMigration.NewColumn.Name + "', 'COLUMN'";
            return renameScript;
        }
        
        protected override string GetDropColumnScript(ColumnMigration columnMigration)
        {
            string dropScript = "-- Drop Column " + columnMigration.CurrentColumn.Name + "\n";
            dropScript += "EXEC SP_RENAME '" + TableMigrations.DxmlTableName + "." + columnMigration.CurrentColumn.Name + "', '" + "Drop_" + columnMigration.CurrentColumn.Name + "', 'COLUMN'";
            return dropScript;
        }

        protected override string GetAlterTypeScript(ColumnMigration columnMigration)
        {
            string alterTypeScript = "-- Change Type From " + columnMigration.CurrentColumn.Type + " To " + columnMigration.NewColumn.Type + " For Column " + columnMigration.CurrentColumn.Name + "\n";
            alterTypeScript += "ALTER TABLE " + TableMigrations.DxmlTableName + " ";
            alterTypeScript += "ALTER COLUMN " + columnMigration.CurrentColumn.Name + " ";
            alterTypeScript += GetDataTypeScript(columnMigration.NewColumn.Type, columnMigration.CurrentColumn.Size);
            if (!columnMigration.CurrentColumn.Constraints.Nullable)
            {
                alterTypeScript += " NOT NULL";
            }
            return !alterTypeScript.Contains("(0)") ? alterTypeScript : null;
        }

        protected override string GetAlterSizeScript(ColumnMigration columnMigration)
        {
            bool IsAlterTypeInMigrationsList = TableMigrations.ColumnsMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERTYPE).Any();
            string alterSizeScript = "-- Change Size From " + columnMigration.CurrentColumn.Size + " To " + columnMigration.NewColumn.Size + " For Column " + columnMigration.CurrentColumn.Name + "\n";
            alterSizeScript += "ALTER TABLE " + TableMigrations.DxmlTableName + " ";
            alterSizeScript += "ALTER COLUMN " + columnMigration.CurrentColumn.Name + " ";
            alterSizeScript += GetDataTypeScript((IsAlterTypeInMigrationsList ? columnMigration.NewColumn.Type : columnMigration.CurrentColumn.Type), columnMigration.NewColumn.Size);
            if (!columnMigration.CurrentColumn.Constraints.Nullable)
            {
                alterSizeScript += " NOT NULL";
            }
            return alterSizeScript;
        }

        protected override string GetAddPrimaryKeyScript(ColumnMigration columnMigration)
        {
            string primaryKeyConstraintName = columnMigration.CurrentColumn.Constraints.PrimaryKeyConstraintName ?? "PK_" + TableMigrations.DxmlTableName + "_" + GenerateRandomString();
            string addPrimaryKeyScript = "-- Add Primary Key To Column " + columnMigration.CurrentColumn.Name + "\n";
            addPrimaryKeyScript += "ALTER TABLE " + TableMigrations.DxmlTableName + " ADD CONSTRAINT " + primaryKeyConstraintName + " PRIMARY KEY (" + columnMigration.CurrentColumn.Name + ")";
            return addPrimaryKeyScript;
        }

        protected override string GetDropPrimaryKeyScript(ColumnMigration columnMigration)
        {
            string dropPrimaryKeyScript = "-- Drop Primary Key From Column " + columnMigration.CurrentColumn.Name + "\n";
            dropPrimaryKeyScript += "ALTER TABLE " + TableMigrations.DxmlTableName + " DROP CONSTRAINT " + columnMigration.CurrentColumn.Constraints.PrimaryKeyConstraintName;
            return dropPrimaryKeyScript;
        }

        protected override string GetSetNullableScript(ColumnMigration columnMigration)
        {
            bool IsAlterTypeInMigrationsList = TableMigrations.ColumnsMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERTYPE).Any();
            bool IsAlterSizeInMigrationsList = TableMigrations.ColumnsMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERSIZE).Any();
            string setNullableScript = "-- Set Nullable For Column " + columnMigration.CurrentColumn.Name + "\n";
            setNullableScript += "ALTER TABLE " + TableMigrations.DxmlTableName + " ";
            setNullableScript += "ALTER COLUMN " + columnMigration.CurrentColumn.Name + " ";
            setNullableScript += GetDataTypeScript((IsAlterTypeInMigrationsList ? columnMigration.NewColumn.Type : columnMigration.CurrentColumn.Type), (IsAlterSizeInMigrationsList ? columnMigration.NewColumn.Size : columnMigration.CurrentColumn.Size));
            return setNullableScript;
        }
        
        protected override string GetUnsetNullableScript(ColumnMigration columnMigration)
        {
            bool IsAlterTypeInMigrationsList = TableMigrations.ColumnsMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERTYPE).Any();
            bool IsAlterSizeInMigrationsList = TableMigrations.ColumnsMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERSIZE).Any();
            string unsetNullableScript = "-- Unset Nullable For Column " + columnMigration.CurrentColumn.Name + "\n";
            unsetNullableScript += "ALTER TABLE " + TableMigrations.DxmlTableName + " ";
            unsetNullableScript += "ALTER COLUMN " + columnMigration.CurrentColumn.Name + " ";
            unsetNullableScript += GetDataTypeScript((IsAlterTypeInMigrationsList ? columnMigration.NewColumn.Type : columnMigration.CurrentColumn.Type), (IsAlterSizeInMigrationsList ? columnMigration.NewColumn.Size : columnMigration.CurrentColumn.Size));
            unsetNullableScript += " NOT NULL";
            return unsetNullableScript;
        }
    }
}