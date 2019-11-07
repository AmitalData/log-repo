using System;
using System.Configuration;
using System.Data.SqlClient;

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
            TableDefinition currentTable = GetCurrentTableFromDB();
            if(currentTable == null)
            {
                return GetCreateTableScript();
            }
            else
            {
                return GetAlterTableScript(currentTable);
            }
        }

        private TableDefinition GetCurrentTableFromDB()
        {
            string queryString = "SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @tableName";
            
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
                        currentTable = new TableDefinition();//should return TableDefinition here
                    }
                    reader.Close();
                }
                catch (Exception)
                {
                    currentTable = null;
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
            columnScript += GetColumnDbDataType(column.Type, column.Size);
            foreach(var constraint in column.Constraints)
            {
                if (constraint.PrimaryKey)
                {
                    columnScript += " PRIMARY KEY";
                }

                if (!constraint.Nullable)
                {
                    columnScript += " NOT NULL";
                }
            }
            columnScript += ",";
            return columnScript;
        }

        private string GetColumnDbDataType(string type, string size)
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

    }
}
