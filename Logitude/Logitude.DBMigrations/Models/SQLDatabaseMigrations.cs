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
                alterTableScript += GetColumnMigrationScript(tableMigrations.TableName, columnMigration);
            }
            return alterTableScript;
        }

        protected override string GetColumnMigrationScript(string tableName, ColumnMigrations columnMigrations)
        {
            string columnMigrationScript = "ALTER TABLE " + tableName + " ";
            if(columnMigrations.MigrationType == MigrationTypes.ADDCOLUMN)
            {
                columnMigrationScript += "ADD " + columnMigrations.ColumnName + " ";
                columnMigrationScript += GetDataTypeScript(columnMigrations.NewColumnType, columnMigrations.NewColumnSize);
                columnMigrationScript += GetConstraintsScript(columnMigrations.NewConstraints);
            }
            else if(columnMigrations.MigrationType == MigrationTypes.ALTERCOLUMN)
            {
                columnMigrationScript += "ALTER COLUMN " + columnMigrations.ColumnName + " ";
                columnMigrationScript += GetDataTypeScript(columnMigrations.NewColumnType, columnMigrations.NewColumnSize);
                columnMigrationScript += GetConstraintsScript(columnMigrations.NewConstraints);
            }
            else
            {
                columnMigrationScript += "DROP COLUMN " + columnMigrations.ColumnName;
            }
            columnMigrationScript += "\n";
            return columnMigrationScript;
        }


        protected override string GetDataTypeScript(string type, int? size)
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
                                Constraints = new ConstraintDefinition
                                {
                                    Nullable = (reader["Nullable"].ToString() == "YES")
                                }
                            };

                            if (!String.IsNullOrEmpty(reader["ConstraintType"].ToString()))
                            {
                                column = SetConstraintForColumnDefinition(column, reader["ConstraintType"].ToString());
                            }

                            currentTableColumns.Add(column);
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(reader["ConstraintType"].ToString()))
                            {
                                var column = currentTableColumns.Where(c => c.Name == reader["ColumnName"].ToString()).First();
                                column = SetConstraintForColumnDefinition(column, reader["ConstraintType"].ToString());
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
