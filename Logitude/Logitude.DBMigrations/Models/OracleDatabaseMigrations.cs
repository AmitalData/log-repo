using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Oracle.DataAccess.Client;

namespace Logitude.DBMigrations.Models
{
    public class OracleDatabaseMigrations : DatabaseMigrations
    {
        protected string ConnectionString;

        public OracleDatabaseMigrations(TableDefinition table, string connectionString)
        {
            ConnectionString = connectionString;
            DXMLTable = FormatCaseSensitiveNames(table);
        }
        
        protected override TableDefinition GetCurrentTableDefinitionFromDB()
        {
            string dxmlTableOldNames = DXMLTable.OldNames;
            string name = "'" + DXMLTable.Name.ToUpper() + "'";
            string shortName = String.IsNullOrEmpty(DXMLTable.ShortName) ? null : "," + "'" + DXMLTable.ShortName.ToUpper() + "'";
            string oldNames = String.IsNullOrEmpty(dxmlTableOldNames) ? null : "," + (dxmlTableOldNames.Contains(",") ? string.Join(",", dxmlTableOldNames.Split(',').Select(oldName => "'" + oldName.ToUpper() + "'").ToArray()) : "'" + dxmlTableOldNames.ToUpper() + "'");

            //tested query
            string queryString = "SELECT TABLE_NAME FROM USER_TABLES WHERE TABLE_NAME IN (" + name + shortName + oldNames + ")";

            TableDefinition currentTable = null;

            OracleDataReader reader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command = new OracleCommand(queryString, connection);

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
            //tested query
            string queryString = "SELECT \"Q1\".*, \"Q2\".\"ConstraintType\", \"Q2\".\"ConstraintName\" " +
                                 "FROM( " +
                                 "SELECT COL.COLUMN_NAME AS \"ColumnName\", COL.NULLABLE AS \"Nullable\", COL.DATA_TYPE AS \"DataType\", COL.CHAR_LENGTH AS \"Size\", COL.DATA_PRECISION AS \"Precision\", COL.DATA_SCALE AS \"Scale\" " +
                                 "FROM USER_TAB_COLUMNS COL " +
                                 "WHERE COL.TABLE_NAME = :tableName " +
                                 ") \"Q1\" " +
                                 "LEFT JOIN( " +
                                 "SELECT CON.COLUMN_NAME AS \"ColumnName\", TCON.CONSTRAINT_TYPE AS \"ConstraintType\", TCON.CONSTRAINT_NAME AS \"ConstraintName\" " +
                                 "FROM USER_CONSTRAINTS TCON " +
                                 "INNER JOIN USER_CONS_COLUMNS CON ON TCON.CONSTRAINT_NAME = CON.CONSTRAINT_NAME " +
                                 "WHERE TCON.TABLE_NAME = :tableName " +
                                 ") \"Q2\" ON \"Q2\".\"ColumnName\" = \"Q1\".\"ColumnName\"";

            TableDefinition currentTable = null;

            OracleDataReader reader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command = new OracleCommand(queryString, connection);
            command.Parameters.Add(new OracleParameter("tableName", tableName));

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
                            Type = GetColumnDefinitionDataType(reader["DataType"].ToString(), reader["Precision"].ToString(), reader["Scale"].ToString()),
                            Size = GetColumnDefinitionSize(reader["DataType"].ToString(), reader["Size"].ToString()),
                            Precision = String.IsNullOrEmpty(reader["Precision"].ToString()) ? 0 : Convert.ToInt32(reader["Precision"].ToString()),
                            Scale = String.IsNullOrEmpty(reader["Scale"].ToString()) ? 0 : Convert.ToInt32(reader["Scale"].ToString()),
                            Constraints = new ConstraintsDefinition
                            {
                                Nullable = (reader["Nullable"].ToString().ToLower() == "yes" || reader["Nullable"].ToString().ToLower() == "y")
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

            return FormatCaseSensitiveNames(currentTable);
        }

        protected override string GetCreateTableScript()
        {
            string createTableScript = "-- Create New Table With Name " + FormatNameLength(DXMLTable.Name, DXMLTable.ShortName).ToUpper() + "\n";
            createTableScript += "CREATE TABLE \"" + FormatNameLength(DXMLTable.Name, DXMLTable.ShortName).ToUpper() + "\"(" + "\n";
            foreach (var column in DXMLTable.Columns)
            {
                createTableScript += GetCreateColumnScript(column) + "\n";
            }

            if (DXMLTable.Columns.Where(c => c.Constraints.PrimaryKey).Any())
            {
                string primaryKeyColumns = string.Join(",", DXMLTable.Columns.Where(c => c.Constraints.PrimaryKey).Select(c => "\"" + FormatNameLength(c.Name, c.ShortName).ToUpper() + "\"").ToArray());
                createTableScript += "PRIMARY KEY(" + primaryKeyColumns + ")" + "\n";
            }
            createTableScript += ");" + "\n\n";
            return createTableScript;
        }

        protected override string GetCreateColumnScript(ColumnDefinition columnDefinition)
        {
            string columnScript = "\"" + FormatNameLength(columnDefinition.Name, columnDefinition.ShortName).ToUpper() + "\"" + " ";
            columnScript += GetDataTypeScript(columnDefinition.Type, columnDefinition.Size, columnDefinition.Precision, columnDefinition.Scale);
            columnScript += columnDefinition.Constraints.Nullable ? " NULL" : " NOT NULL";
            if (DXMLTable.Columns.Last().Name != columnDefinition.Name)
            {
                columnScript += ",";
            }
            else
            {
                if(DXMLTable.Columns.Where(c => c.Constraints.PrimaryKey).Any())
                {
                    columnScript += ",";
                }
            }
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

        protected override string GetAlterColumnsScript()
        {
            string alterColumnsScript = "";
            foreach (var columnMigration in TableMigrations.ColumnsMigrations)
            {
                alterColumnsScript += GetAlterColumnScript(columnMigration);
            }
            return alterColumnsScript;
        }

        protected override bool CheckIfTableRenamed()
        {
            return TableMigrations.CurrentTableName != FormatNameLength(TableMigrations.DxmlTableName, TableMigrations.DxmlTableShortName);
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
                case MigrationTypes.ALTERPRECISIONANDSCALE:
                    alterColumnScript = GetAlterPrecisionAndScaleScript(columnMigration);
                    return alterColumnScript;
                default:
                    return alterColumnScript;
            }
        }

        protected override string GetDataTypeScript(string type, int size, int precision, int scale)
        {
            switch (type)
            {
                case "date":
                    return "DATE";
                case "datetime":
                    return "TIMESTAMP(7)";
                case "time":
                    return "INTERVAL DAY(2) TO SECOND(6)";
                case "varbinary":
                    return "BLOB";
                case "varchar":
                    return size == -1 ? "CLOB" : "VARCHAR2(" + size + " CHAR)";
                case "nvarchar":
                    return size == -1 ? "NCLOB" : "NVARCHAR2(" + size + ")";
                case "timestamp":
                    return "RAW(8)";
                case "char":
                    return "CHAR(" + (size == -1 ? "2000 CHAR" : size.ToString() + " CHAR") + ")";
                case "float":
                    return "NUMBER";
                case "bit":
                    return "NUMBER(1, 0)";
                case "int":
                    return "NUMBER(10, 0)";
                case "bigint":
                    return "NUMBER(18, 0)";
                case "decimal":
                    return "NUMBER(" + precision + ", " + scale + ")";
                default:
                    return null;
            }
        }

        protected override string GetRenameTableScript()
        {
            //SQL Error: ORA-00955: name is already used by an existing object
            //Cannot rename any two tables that have a relationship

            string renameTableScript = "";
            if (CheckIfTableRenamed())
            {
                renameTableScript += "-- Rename Table From " + TableMigrations.CurrentTableName.ToUpper() + " To " + FormatNameLength(TableMigrations.DxmlTableName, TableMigrations.DxmlTableShortName).ToUpper() + "\n";
                renameTableScript += "RENAME \"" + TableMigrations.CurrentTableName.ToUpper() + "\" TO \"" + FormatNameLength(TableMigrations.DxmlTableName, TableMigrations.DxmlTableShortName).ToUpper() + "\"";
                renameTableScript += ";\n\n";
            }
            return renameTableScript;
        }

        protected override string GetAddColumnScript(ColumnMigration columnMigration)
        {
            string addScript = "-- Add New Column With Name " + FormatNameLength(columnMigration.NewColumn.Name, columnMigration.NewColumn.ShortName).ToUpper() + "\n";
            addScript += "ALTER TABLE " + "\"" + FormatNameLength(TableMigrations.DxmlTableName, TableMigrations.DxmlTableShortName).ToUpper() + "\"" + " ";
            addScript += "ADD " + "\"" + FormatNameLength(columnMigration.NewColumn.Name, columnMigration.NewColumn.ShortName).ToUpper() + "\"" + " ";
            addScript += GetDataTypeScript(columnMigration.NewColumn.Type, columnMigration.NewColumn.Size, columnMigration.NewColumn.Precision, columnMigration.NewColumn.Scale);
            addScript += columnMigration.NewColumn.Constraints.Nullable ? " NULL" : " NOT NULL";
            return addScript + ";\n\n";
        }

        protected override string GetRenameColumnScript(ColumnMigration columnMigration)
        {
            string renameScript = "-- Rename Column From " + columnMigration.CurrentColumn.Name.ToUpper() + " To " + FormatNameLength(columnMigration.NewColumn.Name, columnMigration.NewColumn.ShortName).ToUpper() + "\n";
            renameScript += "ALTER TABLE \"" + FormatNameLength(TableMigrations.DxmlTableName, TableMigrations.DxmlTableShortName).ToUpper() + "\" RENAME COLUMN \"" + columnMigration.CurrentColumn.Name.ToUpper() + "\" TO \"" + FormatNameLength(columnMigration.NewColumn.Name, columnMigration.NewColumn.ShortName).ToUpper() + "\"";
            return renameScript + ";\n\n";
        }

        protected override string GetDropColumnScript(ColumnMigration columnMigration)
        {
            string dropScript = "-- Drop Column " + columnMigration.CurrentColumn.Name.ToUpper() + "\n";
            dropScript += "ALTER TABLE \"" + FormatNameLength(TableMigrations.DxmlTableName, TableMigrations.DxmlTableShortName).ToUpper() + "\" RENAME COLUMN \"" + columnMigration.CurrentColumn.Name.ToUpper() + "\" TO \"" + FormatNameLength("Drop_" + columnMigration.CurrentColumn.Name, null).ToUpper() + "\"";

            return dropScript + ";\n\n";
        }

        protected override string GetAlterTypeScript(ColumnMigration columnMigration)
        {
            string alterTypeScript = "-- Change Type From " + columnMigration.CurrentColumn.Type + " To " + columnMigration.NewColumn.Type + " For Column " + columnMigration.CurrentColumn.Name.ToUpper() + "\n";
            alterTypeScript += "ALTER TABLE " + "\"" + FormatNameLength(TableMigrations.DxmlTableName, TableMigrations.DxmlTableShortName).ToUpper() + "\"" + " ";
            alterTypeScript += "MODIFY " + "\"" + columnMigration.CurrentColumn.Name.ToUpper() + "\"" + " ";
            alterTypeScript += GetDataTypeScript(columnMigration.NewColumn.Type, (columnMigration.CurrentColumn.Size == 0 ? 1 : columnMigration.CurrentColumn.Size), columnMigration.NewColumn.Precision, columnMigration.NewColumn.Scale);
            return alterTypeScript + ";\n\n";
        }

        protected override string GetAlterSizeScript(ColumnMigration columnMigration)
        {
            bool IsAlterTypeInMigrationsList = TableMigrations.ColumnsMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERTYPE).Any();
            string alterSizeScript = "-- Change Size From " + columnMigration.CurrentColumn.Size + " To " + columnMigration.NewColumn.Size + " For Column " + columnMigration.CurrentColumn.Name.ToUpper() + "\n";
            alterSizeScript += "ALTER TABLE " + "\"" + FormatNameLength(TableMigrations.DxmlTableName, TableMigrations.DxmlTableShortName).ToUpper() + "\"" + " ";
            alterSizeScript += "MODIFY " + "\"" + columnMigration.CurrentColumn.Name.ToUpper() + "\"" + " ";
            alterSizeScript += GetDataTypeScript((IsAlterTypeInMigrationsList ? columnMigration.NewColumn.Type : columnMigration.CurrentColumn.Type), columnMigration.NewColumn.Size, 0, 0);
            return alterSizeScript + ";\n\n";
        }

        protected override string GetAddPrimaryKeyScript(ColumnMigration columnMigration)
        {
            string primaryKeyColumns = string.Join(",", CurrentTable.Columns.Where(c => c.Constraints.PrimaryKey).Select(c => "\"" + c.Name.ToUpper() + "\"").ToArray());
            string primaryKeyConstraintName = columnMigration.CurrentColumn.Constraints.PrimaryKeyConstraintName;
            string addPrimaryKeyScript = "-- Add The Primary Key Constraint\n";
            addPrimaryKeyScript += "ALTER TABLE \"" + FormatNameLength(TableMigrations.DxmlTableName, TableMigrations.DxmlTableShortName).ToUpper() + "\" ADD CONSTRAINT \"" + primaryKeyConstraintName + "\" PRIMARY KEY (" + primaryKeyColumns + ")";
            return addPrimaryKeyScript + ";\n\n";
        }

        protected override string GetDropPrimaryKeyScript(ColumnMigration columnMigration)
        {
            string dropPrimaryKeyScript = "-- Drop The Primary Key Constraint\n";
            string primaryKeyConstraintName = columnMigration.CurrentColumn.Constraints.PrimaryKeyConstraintName;
            dropPrimaryKeyScript += "ALTER TABLE \"" + FormatNameLength(TableMigrations.DxmlTableName, TableMigrations.DxmlTableShortName).ToUpper() + "\" DROP CONSTRAINT \"" + primaryKeyConstraintName + "\"";
            return dropPrimaryKeyScript + ";\n\n";
        }

        protected override string GetSetNullableScript(ColumnMigration columnMigration)
        {
            bool isAlterTypeInMigrationsList = TableMigrations.ColumnsMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERTYPE).Any();
            bool isAlterSizeInMigrationsList = TableMigrations.ColumnsMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERSIZE).Any();
            bool isAlterPrecisionAndScaleInMigrationsList = TableMigrations.ColumnsMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERPRECISIONANDSCALE).Any();
            string setNullableScript = "-- Set Nullable For Column " + columnMigration.CurrentColumn.Name.ToUpper() + "\n";
            if (columnMigration.CurrentColumn.Constraints.PrimaryKey)
            {
                setNullableScript += GetPrimaryKeyConstraintScript() + "\n";
            }
            setNullableScript += "ALTER TABLE " + "\"" + FormatNameLength(TableMigrations.DxmlTableName, TableMigrations.DxmlTableShortName).ToUpper() + "\"" + " ";
            setNullableScript += "MODIFY " + "\"" + columnMigration.CurrentColumn.Name.ToUpper() + "\"" + " ";
            setNullableScript += GetDataTypeScript((isAlterTypeInMigrationsList ? columnMigration.NewColumn.Type : columnMigration.CurrentColumn.Type), (isAlterSizeInMigrationsList ? columnMigration.NewColumn.Size : columnMigration.CurrentColumn.Size), (isAlterPrecisionAndScaleInMigrationsList ? columnMigration.NewColumn.Precision : columnMigration.CurrentColumn.Precision), (isAlterPrecisionAndScaleInMigrationsList ? columnMigration.NewColumn.Scale : columnMigration.CurrentColumn.Scale));
            setNullableScript += " NULL";
            return setNullableScript + ";\n\n";
        }

        protected override string GetUnsetNullableScript(ColumnMigration columnMigration)
        {
            bool isAlterTypeInMigrationsList = TableMigrations.ColumnsMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERTYPE).Any();
            bool isAlterSizeInMigrationsList = TableMigrations.ColumnsMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERSIZE).Any();
            bool isAlterPrecisionAndScaleInMigrationsList = TableMigrations.ColumnsMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERPRECISIONANDSCALE).Any();
            string unsetNullableScript = "-- Unset Nullable For Column " + columnMigration.CurrentColumn.Name.ToUpper() + "\n";
            unsetNullableScript += "ALTER TABLE " + "\"" + FormatNameLength(TableMigrations.DxmlTableName, TableMigrations.DxmlTableShortName).ToUpper() + "\"" + " ";
            unsetNullableScript += "MODIFY " + "\"" + columnMigration.CurrentColumn.Name.ToUpper() + "\"" + " ";
            unsetNullableScript += GetDataTypeScript((isAlterTypeInMigrationsList ? columnMigration.NewColumn.Type : columnMigration.CurrentColumn.Type), (isAlterSizeInMigrationsList ? columnMigration.NewColumn.Size : columnMigration.CurrentColumn.Size), (isAlterPrecisionAndScaleInMigrationsList ? columnMigration.NewColumn.Precision : columnMigration.CurrentColumn.Precision), (isAlterPrecisionAndScaleInMigrationsList ? columnMigration.NewColumn.Scale : columnMigration.CurrentColumn.Scale));
            unsetNullableScript += " NOT NULL";
            return unsetNullableScript + ";\n\n";
        }

        protected override string GetAlterPrecisionAndScaleScript(ColumnMigration columnMigration)
        {
            string alterPrecisionAndScaleScript = "-- Change Precision And Scale From " + "(" + columnMigration.CurrentColumn.Precision + ", " + columnMigration.CurrentColumn.Scale + ")" + " To " + "(" + columnMigration.NewColumn.Precision + ", " + columnMigration.NewColumn.Scale + ")" + " For Column " + columnMigration.CurrentColumn.Name.ToUpper() + "\n";
            alterPrecisionAndScaleScript += "ALTER TABLE " + "\"" + FormatNameLength(TableMigrations.DxmlTableName, TableMigrations.DxmlTableShortName).ToUpper() + "\"" + " ";
            alterPrecisionAndScaleScript += "MODIFY " + "\"" + columnMigration.CurrentColumn.Name.ToUpper() + "\"" + " ";
            alterPrecisionAndScaleScript += GetDataTypeScript(columnMigration.CurrentColumn.Type, columnMigration.CurrentColumn.Size, columnMigration.NewColumn.Precision, columnMigration.NewColumn.Scale);
            return alterPrecisionAndScaleScript + ";\n\n";
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
                string primaryKeyConstraintName = "PK_" + GenerateRandomString();
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
                alterPrimaryKeyScript += "\n";
                alterPrimaryKeyScript += GetAddPrimaryKeyConstraintScript(primaryKeyConstraintName);
            }
            return alterPrimaryKeyScript;
        }

        protected override string GetDropPrimaryKeyConstraintScript(string primaryKeyConstraintName)
        {
            string dropPrimaryKeyConstraintScript = "ALTER TABLE \"" + FormatNameLength(TableMigrations.DxmlTableName, TableMigrations.DxmlTableShortName).ToUpper() + "\" DROP CONSTRAINT \"" + primaryKeyConstraintName + "\"" + ";";
            return dropPrimaryKeyConstraintScript;
        }

        protected override string GetAddPrimaryKeyConstraintScript(string primaryKeyConstraintName)
        {
            string primaryKeyColumns = string.Join(",", DXMLTable.Columns.Where(c => c.Constraints.PrimaryKey).Select(c => "\"" + FormatNameLength(c.Name, c.ShortName).ToUpper() + "\"").ToArray());
            string primaryKeyConstraintScript = "ALTER TABLE \"" + FormatNameLength(TableMigrations.DxmlTableName, TableMigrations.DxmlTableShortName).ToUpper() + "\" ADD CONSTRAINT \"" + primaryKeyConstraintName + "\" PRIMARY KEY (" + primaryKeyColumns + ");";
            return primaryKeyConstraintScript;
        }

        protected override bool CheckIfTableHasPrimaryKeys(TableDefinition table)
        {
            return table.Columns.Where(c => c.Constraints.PrimaryKey).Any();
        }
    }
}