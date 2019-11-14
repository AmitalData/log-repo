using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace Logitude.DBMigrations.Models
{
    public class SQLDatabaseMigrations : DatabaseMigrations
    {
        public SQLDatabaseMigrations(TableDefinition table)
        { 
            DXMLTable = table;
        }

        protected override TableDefinition GetCurrentTableDefinitionFromDB()
        {
            string queryString = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @tableName";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                TableDefinition currentTable;
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@tableName", DXMLTable.Name);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        currentTable = null;
                    }
                    else
                    {
                        currentTable = GetCurrentTableDefinitionFromDB(DXMLTable.Name);
                    }

                    reader.Close();
                    connection.Close();
                }
                catch (Exception)
                {
                    connection.Close();
                    currentTable = null;//throw an exception to console window
                }

                return currentTable;
            }
        }

        protected override string GetCreateTableScript()
        {
            string createTableScript = "CREATE TABLE " + DXMLTable.Name + "(" + "\n";
            foreach (var column in DXMLTable.Columns)
            {
                createTableScript += GetColumnScript(column) + "\n";
            }
            createTableScript += ")";
            return createTableScript;
        }

        protected override string GetAlterTableScript(TableDefinition currentTable)
        {
            TableMigrations tableMigrations = GetTableMigrations(currentTable, DXMLTable);
            string alterTableScript = "";
            foreach (var columnMigration in tableMigrations.ColumnsMigrations)
            {
                alterTableScript += GetColumnMigrationScript(tableMigrations.TableName, columnMigration, tableMigrations.ColumnsMigrations) + "\n";
            }
            return alterTableScript;
        }

        protected override string GetColumnMigrationScript(string tableName, ColumnMigrations columnMigration, List<ColumnMigrations> columnMigrations)
        {
            switch (columnMigration.MigrationType)
            {
                case MigrationTypes.ADD:
                    string addScript = "ALTER TABLE " + tableName + " ";
                    addScript += "ADD " + columnMigration.NewColumn.Name + " ";
                    addScript += GetDataTypeScript(columnMigration.NewColumn.Type, columnMigration.NewColumn.Size);
                    addScript += GetConstraintsScript(columnMigration.NewColumn.Constraints);
                    return addScript;
                case MigrationTypes.RENAME:
                    string renameScript = "EXEC SP_RENAME '" + tableName + "." + columnMigration.CurrentColumn.Name + "', '" + columnMigration.NewColumn.Name + "', 'COLUMN'";
                    return renameScript;
                case MigrationTypes.DROP:
                    string dropScript = "EXEC SP_RENAME '" + tableName + "." + columnMigration.CurrentColumn.Name + "', '" + "Drop_" + columnMigration.CurrentColumn.Name + "', 'COLUMN'";
                    return dropScript;
                case MigrationTypes.ALTERTYPE:
                    string alterTypeScript = "ALTER TABLE " + tableName + " ";
                    alterTypeScript += "ALTER COLUMN " + columnMigration.CurrentColumn.Name + " ";
                    alterTypeScript += GetDataTypeScript(columnMigration.NewColumn.Type, columnMigration.CurrentColumn.Size);
                    if (!columnMigration.CurrentColumn.Constraints.Nullable)
                    {
                        alterTypeScript += " NOT NULL";
                    }
                    return alterTypeScript;
                case MigrationTypes.ALTERSIZE:
                    string alterSizeScript = "ALTER TABLE " + tableName + " ";
                    alterSizeScript += "ALTER COLUMN " + columnMigration.CurrentColumn.Name + " ";
                    alterSizeScript += GetDataTypeScript((columnMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERTYPE).Any() ? columnMigration.NewColumn.Type : columnMigration.CurrentColumn.Type), columnMigration.NewColumn.Size);
                    if (!columnMigration.CurrentColumn.Constraints.Nullable)
                    {
                        alterSizeScript += " NOT NULL";
                    }
                    return alterSizeScript;
                case MigrationTypes.ADDPRIMARYKEY:
                    string primaryKeyConstraintName = columnMigration.CurrentColumn.Constraints.PrimaryKeyConstraintName ?? "PK_" + tableName + "_" + GenerateIdForConstraint();
                    string addPrimaryKeyScript = "ALTER TABLE " + tableName + " ADD CONSTRAINT " + primaryKeyConstraintName + " PRIMARY KEY (" + columnMigration.CurrentColumn.Name + ")";
                    return addPrimaryKeyScript;
                case MigrationTypes.DROPPRIMARYKEY:
                    string dropPrimaryKeyScript = "ALTER TABLE " + tableName + " DROP CONSTRAINT " + columnMigration.CurrentColumn.Constraints.PrimaryKeyConstraintName;
                    return dropPrimaryKeyScript;
                case MigrationTypes.SETNULLABLE:
                    string setNullableScript = "ALTER TABLE " + tableName + " ";
                    setNullableScript += "ALTER COLUMN " + columnMigration.CurrentColumn.Name + " ";
                    setNullableScript += GetDataTypeScript((columnMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERTYPE).Any() ? columnMigration.NewColumn.Type : columnMigration.CurrentColumn.Type), (columnMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERSIZE).Any() ? columnMigration.NewColumn.Size : columnMigration.CurrentColumn.Size));
                    return setNullableScript;
                case MigrationTypes.UNSETNULLABLE:
                    string UnsetNullableScript = "ALTER TABLE " + tableName + " ";
                    UnsetNullableScript += "ALTER COLUMN " + columnMigration.CurrentColumn.Name + " ";
                    UnsetNullableScript += GetDataTypeScript((columnMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERTYPE).Any() ? columnMigration.NewColumn.Type : columnMigration.CurrentColumn.Type), (columnMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERSIZE).Any() ? columnMigration.NewColumn.Size : columnMigration.CurrentColumn.Size));
                    UnsetNullableScript += " NOT NULL";
                    return UnsetNullableScript;
                default:
                    return null;
            }

            //string columnMigrationScript = "ALTER TABLE " + tableName + " ";
            //if(columnMigrations.MigrationType == MigrationTypes.ADDCOLUMN)
            //{
            //    columnMigrationScript += "ADD " + columnMigrations.ColumnName + " ";
            //    columnMigrationScript += GetDataTypeScript(columnMigrations.NewColumnType, columnMigrations.NewColumnSize);
            //    columnMigrationScript += GetConstraintsScript(columnMigrations.NewConstraints);
            //}
            //else if(columnMigrations.MigrationType == MigrationTypes.ALTERCOLUMN)
            //{
            //    columnMigrationScript += "ALTER COLUMN " + columnMigrations.ColumnName + " ";
            //    columnMigrationScript += GetDataTypeScript(columnMigrations.NewColumnType, columnMigrations.NewColumnSize);
            //    columnMigrationScript += GetConstraintsScript(columnMigrations.NewConstraints);
            //}
            //else
            //{
            //    columnMigrationScript += "DROP COLUMN " + columnMigrations.ColumnName;
            //}
            //columnMigrationScript += "\n";
            //return columnMigrationScript;
        }


        protected override string GetDataTypeScript(string type, int size)
        {
            switch (type)
            {
                case "Text":
                    return "VARCHAR(" + (size == -1 ? "MAX" : size.ToString()) + ")";
                case "nText":
                    return "NVARCHAR(" + (size == -1 ? "MAX" : size.ToString()) + ")";
                case "Integer":
                    return "INT";
                case "Boolean":
                    return "BIT";
                case "DateTime":
                    return "DATETIME";
                default:
                    return "VARCHAR(10)";
            }
        }
         
        protected override TableDefinition GetCurrentTableDefinitionFromDB(string tableName)
        {
            string queryString = @"SELECT COL.COLUMN_NAME AS ColumnName, COL.IS_NULLABLE AS Nullable, COL.DATA_TYPE AS DataType, COL.CHARACTER_MAXIMUM_LENGTH AS Size, CON.CONSTRAINT_NAME AS ConstraintName, TCON.CONSTRAINT_TYPE AS ConstraintType " +
                                  "FROM INFORMATION_SCHEMA.COLUMNS COL LEFT OUTER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE CON ON COL.COLUMN_NAME = CON.COLUMN_NAME LEFT OUTER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS TCON ON CON.CONSTRAINT_NAME = TCON.CONSTRAINT_NAME " +
                                  "WHERE COL.TABLE_NAME = @tableName AND(CON.TABLE_NAME = @tableName OR CON.TABLE_NAME IS NULL)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                List<ColumnDefinition> currentTableColumns = new List<ColumnDefinition>();

                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@tableName", DXMLTable.Name);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        if (!currentTableColumns.Where(c => c.Name == reader["ColumnName"].ToString()).Any())
                        {
                            ColumnDefinition column = new ColumnDefinition
                            {
                                Name = reader["ColumnName"].ToString(),
                                Type = GetDxmlDataType(reader["DataType"].ToString().ToUpper()),
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

                    var currentTable = new TableDefinition
                    {
                        Name = tableName,
                        Columns = currentTableColumns
                    };

                    return currentTable;
                }
                catch (Exception e)
                {
                    var error = e.ToString();
                    return null;
                }
            }
        }
          
    }
}
