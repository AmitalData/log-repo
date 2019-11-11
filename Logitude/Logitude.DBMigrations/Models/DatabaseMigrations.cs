using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace Logitude.DBMigrations.Models
{
    public class DatabaseMigrations
    {
        readonly string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        public TableDefinition DXMLTable;
        public DatabaseMigrations(TableDefinition table)
        {
            DXMLTable = table;
        }
        
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
            foreach (var column in DXMLTable.Columns)
            {
                createTableScript += GetColumnScript(column) + "\n";
            }
            createTableScript += ")";
            return createTableScript;
        }

        private string GetAlterTableScript(TableDefinition currentTable)
        {
            TableMigrations tableMigrations = GetTableMigrations(currentTable, DXMLTable);
            string alterTableScript = "";
            foreach (var columnMigration in tableMigrations.ColumnsMigrations)
            {
                alterTableScript += GetColumnMigrationScript(tableMigrations.TableName, columnMigration);
            }
            return alterTableScript;
        }

        private string GetColumnScript(ColumnDefinition column)
        {
            string columnScript = column.Name + " ";
            columnScript += GetDataTypeScript(column.Type, column.Size);
            columnScript += GetConstraintsScript(column.Constraints);
            columnScript += ",";
            return columnScript;
        }

        private string GetColumnMigrationScript(string tableName, ColumnMigrations columnMigrations)
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

        private string GetDataTypeScript(string type, int? size)
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

        private string GetDxmlDataType(string type)
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

        private string GetConstraintsScript(ConstraintDefinition constraints)
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

        private TableMigrations GetTableMigrations(TableDefinition currentTable, TableDefinition dxmlTable)
        {
            List<ColumnMigrations> columnsMigrations = new List<ColumnMigrations>();
            
            foreach (var dxmlTableColumn in dxmlTable.Columns)
            {
                if(!IsColumnInTableDefinition(currentTable, dxmlTableColumn.Name))
                {
                    ColumnMigrations columnMigrations = GetColumnMigrations(MigrationTypes.ADDCOLUMN, dxmlTableColumn);
                    columnsMigrations.Add(columnMigrations);
                }
                else
                {
                    ColumnDefinition currentTableColumn = currentTable.Columns.Where(c => c.Name == dxmlTableColumn.Name).First();
                    if(IsThereMigration(currentTableColumn, dxmlTableColumn))
                    {
                        ColumnMigrations columnMigrations = GetColumnMigrations(MigrationTypes.ALTERCOLUMN, dxmlTableColumn);
                        columnsMigrations.Add(columnMigrations);
                    }
                }
            }

            List<string> dxmlTableColumns = dxmlTable.Columns.Select(c => c.Name).ToList();
            List<ColumnDefinition> droppedColumns = currentTable.Columns.Where(c => !dxmlTableColumns.Contains(c.Name)).ToList();

            foreach(var droppedColumn in droppedColumns)
            {
                ColumnDefinition currentTableColumn = currentTable.Columns.Where(c => c.Name == droppedColumn.Name).First();
                ColumnMigrations columnMigrations = GetColumnMigrations(MigrationTypes.DROPCOLUMN, currentTableColumn);
                columnsMigrations.Add(columnMigrations);
            }

            TableMigrations tableMigrations = new TableMigrations
            {
                TableName = dxmlTable.Name,
                ColumnsMigrations = columnsMigrations
            };
            return tableMigrations;
        }

        private bool IsColumnInTableDefinition(TableDefinition tableDefinition, string columnName)
        {
            return tableDefinition.Columns.Where(c => c.Name == columnName).Any();
        }

        private ColumnMigrations GetColumnMigrations(int migrationType, ColumnDefinition columnDefinition)
        {
            ColumnMigrations columnMigrations = new ColumnMigrations
            {
                MigrationType = migrationType,
                ColumnName = columnDefinition.Name,
                NewColumnName = columnDefinition.NewName,
                NewColumnType = columnDefinition.Type,
                NewColumnSize = columnDefinition.Size,
                NewConstraints = columnDefinition.Constraints
            };
            return columnMigrations;
        }

        private bool IsThereMigration(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            if(dxmlTableColumn.Type != currentTableColumn.Type)
            {
                return true;
            }
            if(dxmlTableColumn.Size != currentTableColumn.Size)
            {
                return true;
            }
            if(dxmlTableColumn.Constraints.PrimaryKey != currentTableColumn.Constraints.PrimaryKey)
            {
                return true;
            }
            if(dxmlTableColumn.Constraints.Nullable != currentTableColumn.Constraints.Nullable)
            {
                return true;
            }
            return false;
        }

    }
}
