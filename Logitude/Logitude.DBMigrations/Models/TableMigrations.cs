using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace Logitude.DBMigrations.Models
{
    public class TableMigrations
    {
        readonly string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        public TableDefinition DXMLTable;
        public TableMigrations(TableDefinition table)
        {
            DXMLTable = table;
        }

        public string GetScript()
        {
            TableDefinition currentTable = GetCurrentTableDefinitionFromDB();
            Console.WriteLine(currentTable);
            if(currentTable == null)
            {
                return GetCreateTableScript();
            }
            else
            {
                return GetAlterTableScript(currentTable);
            }
        }

        private TableDefinition GetCurrentTableDefinitionFromDB()
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

        private string GetCreateTableScript()
        {
            string createTableScript = "CREATE TABLE " + DXMLTable.Name + "(" + "\n";
            foreach(var column in DXMLTable.Columns)
            {
                createTableScript += GetColumnScript(column) + "\n";
            }
            createTableScript += ")";
            return createTableScript;
        }

        private string GetAlterTableScript(TableDefinition currentTable)
        {
            string script = "";
            return script;
        }

        private string GetColumnScript(ColumnDefinition column)
        {
            string columnScript = column.Name + " ";
            columnScript += GetDbColumnDataType(column.Type, column.Size);

            var constraints = column.Constraints;
            if(constraints != null)
            {
                if (constraints.PrimaryKey)
                {
                    columnScript += " PRIMARY KEY";
                }

                if (!constraints.Nullable)
                {
                    columnScript += " NOT NULL";
                }
            }

            columnScript += ",";
            return columnScript;
        }

        private string GetDbColumnDataType(string type, string size)
        {
            switch (type)
            {
                case "Text":
                    return "VARCHAR(" + size + ")";
                case "nText":
                    return "NVARCHAR(" + size + ")";
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

        private string GetDxmlColumnDataType(string type)
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

        private TableDefinition GetCurrentTableDefinitionFromDB(string tableName)
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
                        if(!currentTableColumns.Where(c => c.Name == reader["ColumnName"].ToString()).Any())
                        {
                            ColumnDefinition column = new ColumnDefinition
                            {
                                Name = reader["ColumnName"].ToString(),
                                Type = GetDxmlColumnDataType(reader["DataType"].ToString().ToUpper()),
                                Size = reader["Size"].ToString() == "-1" ? "MAX" : reader["Size"].ToString(),
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

        private ColumnDefinition SetConstraintForColumnDefinition(ColumnDefinition column, string constraintType)
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







    }
}
