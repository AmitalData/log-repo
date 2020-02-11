using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Logitude.DBMigrations.Models
{
    public class SQLDatabaseMigrations : DatabaseMigrations
    {
        public SQLDatabaseMigrations(TableDefinition dxmlTable, string connectionString, List<TableDefinition> dxmlTables, string dxmlFileName)
        {
            ConnectionString = connectionString;
            DXMLTable = dxmlTable;
            DXMLTables = dxmlTables;
            DXMLFileName = dxmlFileName;
        }

        protected override TableDefinition GetCurrentTableDefinitionFromDB()
        {
            string dxmlTableOldNames = DXMLTable.OldNames;
            string name = "'" + DXMLTable.Name + "'";
            string oldNames = String.IsNullOrEmpty(dxmlTableOldNames) ? null : "," + (dxmlTableOldNames.Contains(",") ? string.Join(",", dxmlTableOldNames.Split(',').Select(n => "'" + n + "'").ToArray()) : "'" + dxmlTableOldNames + "'");
            
            string queryString = @"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME IN (" + name + oldNames + ")";

            TableDefinition currentTable = null;

            SqlDataReader reader = null;
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand(queryString, connection);

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
            string queryString = @"SELECT Q1.*, Q2.ConstraintType, Q2.ConstraintName, Q3.DefaultValue, Q3.DefaultConstraintName " +
                                  "FROM ( " +
                                  "SELECT COL.COLUMN_NAME AS ColumnName, COL.IS_NULLABLE AS Nullable, COL.DATA_TYPE AS DataType, COL.CHARACTER_MAXIMUM_LENGTH AS Size, COL.NUMERIC_PRECISION AS Precision, COL.NUMERIC_SCALE AS Scale " +
                                  "FROM INFORMATION_SCHEMA.COLUMNS AS COL " +
                                  "WHERE COL.TABLE_NAME = @tableName " +
                                  ") AS Q1 " +
                                  "LEFT JOIN ( " +
                                  "SELECT CON.COLUMN_NAME AS ColumnName, TCON.CONSTRAINT_TYPE AS ConstraintType, TCON.CONSTRAINT_NAME AS ConstraintName " +
                                  "FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TCON " +
                                  "INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS CON ON TCON.CONSTRAINT_NAME = CON.CONSTRAINT_NAME " +
                                  "WHERE TCON.TABLE_NAME = @tableName " +
                                  ") AS Q2 ON Q2.ColumnName = Q1.ColumnName " +
                                  "LEFT JOIN (" +
                                  "SELECT COL.name AS ColumnName, DEFCON.definition AS DefaultValue, DEFCON.name AS DefaultConstraintName " +
                                  "FROM SYS.DEFAULT_CONSTRAINTS DEFCON " +
                                  "LEFT OUTER JOIN SYS.OBJECTS TAB ON DEFCON.parent_object_id = TAB.object_id " +
                                  "LEFT OUTER JOIN SYS.ALL_COLUMNS COL " +
                                  "ON DEFCON.parent_column_id = COL.column_id AND DEFCON.parent_object_id = COL.object_id " +
                                  "WHERE COL.object_id = (SELECT object_id FROM SYS.TABLES WHERE name = @tableName) " +
                                  ") AS Q3 ON Q3.ColumnName = Q1.ColumnName";

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
                            Type = GetColumnDefinitionDataType(reader["DataType"].ToString()),
                            Size = GetColumnDefinitionSize(reader["Size"].ToString()),
                            Precision = String.IsNullOrEmpty(reader["Precision"].ToString()) ? 0 : Convert.ToInt32(reader["Precision"].ToString()),
                            Scale = String.IsNullOrEmpty(reader["Scale"].ToString()) ? 0 : Convert.ToInt32(reader["Scale"].ToString()),
                            DefaultValue = String.IsNullOrEmpty(reader["DefaultValue"].ToString()) ? null : reader["DefaultValue"].ToString(),
                            Constraints = new ConstraintsDefinition
                            {
                                Nullable = (reader["Nullable"].ToString().ToLower() == "yes"),
                                DefaultConstraintName = String.IsNullOrEmpty(reader["DefaultConstraintName"].ToString()) ? null : reader["DefaultConstraintName"].ToString()
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

                List<RelationDefinition> tableRelations = GetRelationsForDBTable(tableName, true);
                List<IndexDefinition> tableIndexes = GetIndexesForDBTable(tableName);

                currentTable = new TableDefinition
                {
                    Name = tableName,
                    Columns = currentTableColumns,
                    Relations = tableRelations,
                    Indexes = tableIndexes.Where(i => !tableRelations.Select(r => r.ForeignKeyColumn).Contains(i.Columns)).ToList(),
                    UniqueConstraints = GetUniqueConstraintsForDBTable(tableName),
                    AllIndexes = tableIndexes
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

        protected override List<RelationDefinition> GetRelationsForDBTable(string tableName, bool usingParentTable)
        {
            string tableObjectId = usingParentTable ? "parent_object_id" : "referenced_object_id";
            
            string queryString = @"SELECT ParentTable.name AS ParentTableName, ParentColumn.name AS ParentColumnName, ReferencedTable.name AS ReferencedTableName, ReferencedColumn.name AS ReferencedColumnName, SysObject.name AS ForeignKeyConstraintName, ParentTableSchema.name AS ParentTableSchemaName, ReferencedTableSchema.name AS ReferencedTableSchemaName, ReferencedColumn.column_id AS ReferencedColumnOrder " +
                      "FROM SYS.FOREIGN_KEY_COLUMNS ForeignKeyColumns " +
                      "INNER JOIN SYS.TABLES ParentTable ON ParentTable.object_id = ForeignKeyColumns.parent_object_id " +
                      "INNER JOIN SYS.SCHEMAS ParentTableSchema ON ParentTable.schema_id = ParentTableSchema.schema_id " +
                      "INNER JOIN SYS.COLUMNS ParentColumn ON ParentColumn.column_id = ForeignKeyColumns.parent_column_id AND ParentColumn.object_id = ParentTable.object_id " +
                      "INNER JOIN SYS.TABLES ReferencedTable ON ReferencedTable.object_id = ForeignKeyColumns.referenced_object_id " +
                      "INNER JOIN SYS.SCHEMAS ReferencedTableSchema ON ReferencedTable.schema_id = ReferencedTableSchema.schema_id " +
                      "INNER JOIN SYS.COLUMNS ReferencedColumn ON ReferencedColumn.column_id = ForeignKeyColumns.referenced_column_id AND ReferencedColumn.object_id = ReferencedTable.object_id " +
                      "INNER JOIN SYS.OBJECTS SysObject ON SysObject.object_id = ForeignKeyColumns.constraint_object_id " +
                      "WHERE ForeignKeyColumns." + tableObjectId + " = (SELECT object_id FROM SYS.TABLES WHERE name = @tableName)";

            List<RelationDefinition> relations = new List<RelationDefinition>();

            SqlDataReader reader = null;
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@tableName", tableName);

            try
            {
                connection.Open();
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    RelationDefinition relation = new RelationDefinition
                    {
                        ForeignKeyColumn = reader["ParentColumnName"].ToString(),
                        ParentTable = reader["ParentTableName"].ToString(),
                        ReferencedTable = reader["ReferencedTableName"].ToString(),
                        ReferencedColumn = reader["ReferencedColumnName"].ToString(),
                        ForeignKeyConstraintName = reader["ForeignKeyConstraintName"].ToString(),
                        ParentTableSchema = reader["ParentTableSchemaName"].ToString(),
                        ReferencedTableSchema = reader["ReferencedTableSchemaName"].ToString(),
                        ReferencedColumnOrder = Convert.ToInt32(reader["ReferencedColumnOrder"].ToString())
                    };
                    relations.Add(relation);
                }

                reader.Close();
                connection.Close();

                relations = HandlingCompositeRelations(relations);
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

            return relations;
        }

        protected override List<IndexDefinition> GetIndexesForDBTable(string tableName)
        {
            string queryString = @"SELECT " +
                                  "STUFF(REPLACE(REPLACE(( " +
                                  "SELECT c.name + CASE WHEN ic.is_descending_key = 1 THEN '' ELSE '' END AS [data()] " +
                                  "FROM sys.index_columns AS ic " +
                                  "INNER JOIN sys.columns AS c ON ic.object_id = c.object_id AND ic.column_id = c.column_id " +
                                  "WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id AND ic.is_included_column = 0 " +
                                  "ORDER BY ic.key_ordinal " +
                                  "FOR XML PATH " +
                                  "), '<row>', ', '), '</row>', ''), 1, 2, '') AS [Columns], " +
                                  "STUFF(REPLACE(REPLACE(( " +
                                  "SELECT c.name AS [data()] " +
                                  "FROM sys.index_columns AS ic " +
                                  "INNER JOIN sys.columns AS c ON ic.object_id = c.object_id AND ic.column_id = c.column_id " +
                                  "WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id AND ic.is_included_column = 1 " +
                                  "ORDER BY ic.index_column_id " +
                                  "FOR XML PATH " +
                                  "), '<row>', ', '), '</row>', ''), 1, 2, '') AS [Include], " +
                                  "i.name AS [IndexName] " +
                                  "FROM sys.tables AS t " +
                                  "INNER JOIN sys.indexes AS i ON t.object_id = i.object_id " +
                                  "WHERE t.is_ms_shipped = 0 " +
                                  "AND i.type <> 0 " +
                                  "AND i.is_primary_key = 0 " +
                                  "AND i.is_unique_constraint = 0 " +
                                  "AND i.object_id = (SELECT object_id FROM SYS.TABLES WHERE name = @tableName)";

            List<IndexDefinition> indexes = new List<IndexDefinition>();

            SqlDataReader reader = null;
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@tableName", tableName);

            try
            {
                connection.Open();
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    IndexDefinition index = new IndexDefinition
                    {
                        Columns = reader["Columns"].ToString().Replace(", ", ","),
                        Include = String.IsNullOrEmpty(reader["Include"].ToString()) ? null : reader["Include"].ToString().Replace(", ", ","),
                        IndexName = reader["IndexName"].ToString()
                    };
                    indexes.Add(index);
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

            return indexes;
        }

        protected override List<UniqueConstraintDefinition> GetUniqueConstraintsForDBTable(string tableName)
        {
            string queryString = @"SELECT " +
                                  "STUFF(REPLACE(REPLACE(( " +
                                  "SELECT c.name + CASE WHEN ic.is_descending_key = 1 THEN '' ELSE '' END AS [data()] " +
                                  "FROM sys.index_columns AS ic " +
                                  "INNER JOIN sys.columns AS c ON ic.object_id = c.object_id AND ic.column_id = c.column_id " +
                                  "WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id AND ic.is_included_column = 0 " +
                                  "ORDER BY ic.key_ordinal " +
                                  "FOR XML PATH " +
                                  "), '<row>', ', '), '</row>', ''), 1, 2, '') AS [Columns], " +
                                  "i.name AS [ConstraintName] " +
                                  "FROM sys.tables AS t " +
                                  "INNER JOIN sys.indexes AS i ON t.object_id = i.object_id " +
                                  "WHERE t.is_ms_shipped = 0 " +
                                  "AND i.type <> 0 " +
                                  "AND i.is_primary_key = 0 " +
                                  "AND i.is_unique_constraint = 1 " +
                                  "AND i.object_id = (SELECT object_id FROM SYS.TABLES WHERE name = @tableName)";

            List<UniqueConstraintDefinition> uniqueConstraints = new List<UniqueConstraintDefinition>();

            SqlDataReader reader = null;
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@tableName", tableName);

            try
            {
                connection.Open();
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    UniqueConstraintDefinition uniqueConstraint = new UniqueConstraintDefinition
                    {
                        Columns = reader["Columns"].ToString().Replace(", ", ","),
                        ConstraintName = reader["ConstraintName"].ToString()
                    };
                    uniqueConstraints.Add(uniqueConstraint);
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

            return uniqueConstraints;
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

            string createTableWithHistoryScript = createTableScript + GetInsertScriptForMigrationsHistory("Create Table", DXMLTable.Name, null, createTableScript);

            return createTableWithHistoryScript;
        }

        protected override string GetCreateColumnScript(ColumnDefinition columnDefinition)
        {
            string columnScript = "[" + columnDefinition.Name + "]" + " ";
            columnScript += GetDataTypeScript(columnDefinition.Type, columnDefinition.Size, columnDefinition.Precision, columnDefinition.Scale);
            columnScript += ((columnDefinition.Identity && columnDefinition.Constraints.PrimaryKey) ? " IDENTITY(1,1)" : null);
            columnScript += GetDefaultValueScript(columnDefinition.Constraints.Nullable, columnDefinition.Type, columnDefinition.DefaultValue);
            columnScript += (columnDefinition.Constraints.Nullable ? " NULL" : " NOT NULL");
            columnScript += ",";
            return columnScript;
        }

        protected override bool IsTableRenamed()
        {
            return TableMigrations.DxmlTableName.ToLower() != TableMigrations.CurrentTableName.ToLower();
        }

        protected override bool IsColumnRenamed(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            return currentTableColumn.Name.ToLower() != dxmlTableColumn.Name.ToLower();
        }

        protected override int FormatColumnSize(int size, string type)
        {
            if(type == "varchar" || type == "char" || type == "varbinary")
            {
                return size >= 8000 ? 8000 : size;
            }
            else
            {
                return size >= 4000 ? 4000 : size;
            }
        }

        protected override string GetDataTypeScript(string type, int size, int precision, int scale)
        {
            switch (type)
            {
                case "int":
                    return "INT";
                case "decimal":
                    return "DECIMAL(" + precision + ", " + scale + ")";
                case "timestamp":
                    return "TIMESTAMP";
                case "varbinary":
                    return "VARBINARY(" + (size == -1 ? "MAX" : FormatColumnSize(size, type).ToString()) + ")";
                case "varchar":
                    return "VARCHAR(" + (size == -1 ? "MAX" : FormatColumnSize(size, type).ToString()) + ")";
                case "datetime":
                    return "DATETIME";
                case "date":
                    return "DATE";
                case "time":
                    return "TIME";
                case "float":
                    return "FLOAT";
                case "char":
                    return "CHAR(" + (size == -1 ? "8000" : FormatColumnSize(size, type).ToString()) + ")";
                case "bigint":
                    return "BIGINT";
                case "nvarchar":
                    return "NVARCHAR(" + (size == -1 ? "MAX" : FormatColumnSize(size, type).ToString()) + ")";
                case "bit":
                    return "BIT";
                default:
                    return null;
            }
        }

        protected override bool IsRelationInCurrentTable(RelationDefinition relation)
        {
            bool isForeignKeyDataTypeChanged = false;

            if (!relation.ForeignKeyColumn.Contains(","))
            {
                ColumnDefinition dxmlForeignKeyColumn = DXMLTable.Columns.Where(c => c.Name == relation.ForeignKeyColumn).First();

                if (IsColumnInCurrentTable(dxmlForeignKeyColumn.Name, dxmlForeignKeyColumn.ShortName, dxmlForeignKeyColumn.OldNames))
                {
                    ColumnDefinition dbForeignKeyColumn = GetCurrentTableColumn(dxmlForeignKeyColumn.Name, dxmlForeignKeyColumn.ShortName, dxmlForeignKeyColumn.OldNames);
                    isForeignKeyDataTypeChanged = (dbForeignKeyColumn.Type != dxmlForeignKeyColumn.Type) || (dbForeignKeyColumn.Size != FormatColumnSize(dxmlForeignKeyColumn.Size, dxmlForeignKeyColumn.Type) && dxmlForeignKeyColumn.Size != 0) || (dbForeignKeyColumn.Type == "decimal" && dxmlForeignKeyColumn.Type == "decimal" && (dbForeignKeyColumn.Precision != dxmlForeignKeyColumn.Precision || dbForeignKeyColumn.Scale != dxmlForeignKeyColumn.Scale));
                }
            }
            else
            {
                List<ColumnDefinition> dxmlForeignKeyColumns = DXMLTable.Columns.Where(c => relation.ForeignKeyColumn.Split(',').Contains(c.Name)).ToList();

                foreach (var dxmlForeignKeyColumn in dxmlForeignKeyColumns)
                {
                    if (IsColumnInCurrentTable(dxmlForeignKeyColumn.Name, dxmlForeignKeyColumn.ShortName, dxmlForeignKeyColumn.OldNames))
                    {
                        ColumnDefinition dbForeignKeyColumn = GetCurrentTableColumn(dxmlForeignKeyColumn.Name, dxmlForeignKeyColumn.ShortName, dxmlForeignKeyColumn.OldNames);
                        if ((dbForeignKeyColumn.Type != dxmlForeignKeyColumn.Type) || (dbForeignKeyColumn.Size != FormatColumnSize(dxmlForeignKeyColumn.Size, dxmlForeignKeyColumn.Type) && dxmlForeignKeyColumn.Size != 0) || (dbForeignKeyColumn.Type == "decimal" && dxmlForeignKeyColumn.Type == "decimal" && (dbForeignKeyColumn.Precision != dxmlForeignKeyColumn.Precision || dbForeignKeyColumn.Scale != dxmlForeignKeyColumn.Scale)))
                        {
                            isForeignKeyDataTypeChanged = true;
                        }
                    }
                }
            }

            bool isRelationInCurrentTable = CurrentTable.Relations.Where(r => r.ForeignKeyColumn.ToLower() == relation.ForeignKeyColumn.ToLower() && r.ReferencedTable.ToLower() == relation.ReferencedTable.ToLower() && r.ReferencedColumn.ToLower() == relation.ReferencedColumn.ToLower()).Any();

            return (!isForeignKeyDataTypeChanged && isRelationInCurrentTable);
        }

        protected override bool IsRelationInDXMLTable(RelationDefinition relation)
        {
            return DXMLTable.Relations.Where(r => r.ForeignKeyColumn.ToLower() == relation.ForeignKeyColumn.ToLower() && r.ReferencedTable.ToLower() == relation.ReferencedTable.ToLower() && r.ReferencedColumn.ToLower() == relation.ReferencedColumn.ToLower()).Any();
        }

        protected override RelationDefinition GetRelationFromDXMLTable(RelationDefinition relation)
        {
            return DXMLTable.Relations.Where(r => r.ForeignKeyColumn.ToLower() == relation.ForeignKeyColumn.ToLower() && r.ReferencedTable.ToLower() == relation.ReferencedTable.ToLower() && r.ReferencedColumn.ToLower() == relation.ReferencedColumn.ToLower()).First();
        }

        protected override bool IsIndexInCurrentTable(IndexDefinition index)
        {
            bool isIndexColumnDataTypeChanged = false;
            
            if (!index.Columns.Contains(","))
            {
                ColumnDefinition dxmlColumn = DXMLTable.Columns.Where(c => c.Name == index.Columns).First();

                if (IsColumnInCurrentTable(dxmlColumn.Name, dxmlColumn.ShortName, dxmlColumn.OldNames))
                {
                    ColumnDefinition dbColumn = GetCurrentTableColumn(dxmlColumn.Name, dxmlColumn.ShortName, dxmlColumn.OldNames);
                    isIndexColumnDataTypeChanged = (dbColumn.Type != dxmlColumn.Type) || (dbColumn.Size != FormatColumnSize(dxmlColumn.Size, dxmlColumn.Type) && dxmlColumn.Size != 0) || (dbColumn.Type == "decimal" && dxmlColumn.Type == "decimal" && (dbColumn.Precision != dxmlColumn.Precision || dbColumn.Scale != dxmlColumn.Scale));
                }
            }
            else
            {
                List<ColumnDefinition> dxmlColumns = DXMLTable.Columns.Where(c => index.Columns.Split(',').Contains(c.Name)).ToList();

                foreach (var dxmlColumn in dxmlColumns)
                {
                    if (IsColumnInCurrentTable(dxmlColumn.Name, dxmlColumn.ShortName, dxmlColumn.OldNames))
                    {
                        ColumnDefinition dbColumn = GetCurrentTableColumn(dxmlColumn.Name, dxmlColumn.ShortName, dxmlColumn.OldNames);
                        if ((dbColumn.Type != dxmlColumn.Type) || (dbColumn.Size != FormatColumnSize(dxmlColumn.Size, dxmlColumn.Type) && dxmlColumn.Size != 0) || (dbColumn.Type == "decimal" && dxmlColumn.Type == "decimal" && (dbColumn.Precision != dxmlColumn.Precision || dbColumn.Scale != dxmlColumn.Scale)))
                        {
                            isIndexColumnDataTypeChanged = true;
                        }
                    }
                }
            }

            bool isIndexInCurrentTable = CurrentTable.Indexes.Where(i => i.Columns.ToLower() == index.Columns.ToLower()).Any();

            return (!isIndexColumnDataTypeChanged && isIndexInCurrentTable);

            //return CurrentTable.Indexes.Where(i => i.Columns.ToLower() == index.Columns.ToLower()).Any();
        }

        protected override bool IsIndexInDXMLTable(IndexDefinition index)
        {
            return DXMLTable.Indexes.Where(i => i.Columns.ToLower() == index.Columns.ToLower()).Any();
        }

        protected override bool IsUniqueConstraintInCurrentTable(UniqueConstraintDefinition uniqueConstraint)
        {
            bool isUniqueConstraintColumnDataTypeChanged = false;

            if (!uniqueConstraint.Columns.Contains(","))
            {
                ColumnDefinition dxmlColumn = DXMLTable.Columns.Where(c => c.Name == uniqueConstraint.Columns).First();

                if (IsColumnInCurrentTable(dxmlColumn.Name, dxmlColumn.ShortName, dxmlColumn.OldNames))
                {
                    ColumnDefinition dbColumn = GetCurrentTableColumn(dxmlColumn.Name, dxmlColumn.ShortName, dxmlColumn.OldNames);
                    isUniqueConstraintColumnDataTypeChanged = (dbColumn.Type != dxmlColumn.Type) || (dbColumn.Size != FormatColumnSize(dxmlColumn.Size, dxmlColumn.Type) && dxmlColumn.Size != 0) || (dbColumn.Type == "decimal" && dxmlColumn.Type == "decimal" && (dbColumn.Precision != dxmlColumn.Precision || dbColumn.Scale != dxmlColumn.Scale));
                }
            }
            else
            {
                List<ColumnDefinition> dxmlColumns = DXMLTable.Columns.Where(c => uniqueConstraint.Columns.Split(',').Contains(c.Name)).ToList();

                foreach (var dxmlColumn in dxmlColumns)
                {
                    if (IsColumnInCurrentTable(dxmlColumn.Name, dxmlColumn.ShortName, dxmlColumn.OldNames))
                    {
                        ColumnDefinition dbColumn = GetCurrentTableColumn(dxmlColumn.Name, dxmlColumn.ShortName, dxmlColumn.OldNames);
                        if ((dbColumn.Type != dxmlColumn.Type) || (dbColumn.Size != FormatColumnSize(dxmlColumn.Size, dxmlColumn.Type) && dxmlColumn.Size != 0) || (dbColumn.Type == "decimal" && dxmlColumn.Type == "decimal" && (dbColumn.Precision != dxmlColumn.Precision || dbColumn.Scale != dxmlColumn.Scale)))
                        {
                            isUniqueConstraintColumnDataTypeChanged = true;
                        }
                    }
                }
            }

            bool isUniqueConstraintInCurrentTable = CurrentTable.UniqueConstraints.Where(u => u.Columns.ToLower() == uniqueConstraint.Columns.ToLower()).Any();

            return (!isUniqueConstraintColumnDataTypeChanged && isUniqueConstraintInCurrentTable);

            //return CurrentTable.UniqueConstraints.Where(u => u.Columns.ToLower() == uniqueConstraint.Columns.ToLower()).Any();
        }

        protected override bool IsUniqueConstraintInDXMLTable(UniqueConstraintDefinition uniqueConstraint)
        {
            return DXMLTable.UniqueConstraints.Where(u => u.Columns.ToLower() == uniqueConstraint.Columns.ToLower()).Any();
        }

        protected override bool IsColumnInCurrentTable(string dxmlColumnName, string dxmlColumnShortName, string dxmlColumnOldNames)
        {
            dxmlColumnName = dxmlColumnName.ToLower();
            dxmlColumnOldNames = dxmlColumnOldNames?.ToLower();

            bool isColumnInCurrentTable = false;
            if (dxmlColumnOldNames != null)
            {
                if (!dxmlColumnOldNames.Contains(","))
                {
                    isColumnInCurrentTable = CurrentTable.Columns.Where(c => c.Name.ToLower() == dxmlColumnOldNames).Any();
                }
                else
                {
                    List<string> oldNames = dxmlColumnOldNames.Split(',').ToList();
                    isColumnInCurrentTable = CurrentTable.Columns.Where(c => oldNames.Contains(c.Name.ToLower())).Any();
                }
            }

            if (!isColumnInCurrentTable)
            {
                isColumnInCurrentTable = CurrentTable.Columns.Where(c => c.Name.ToLower() == dxmlColumnName).Any();
            }

            return isColumnInCurrentTable;
        }

        protected override ColumnDefinition GetCurrentTableColumn(string dxmlColumnName, string dxmlColumnShortName, string dxmlColumnOldNames)
        {
            dxmlColumnName = dxmlColumnName.ToLower();
            dxmlColumnOldNames = dxmlColumnOldNames?.ToLower();

            string columnName = dxmlColumnOldNames;
            bool isColumnInCurrentTable = false;
            if (dxmlColumnOldNames != null)
            {
                if (!dxmlColumnOldNames.Contains(','))
                {
                    isColumnInCurrentTable = CurrentTable.Columns.Where(c => c.Name.ToLower() == dxmlColumnOldNames).Any();
                }
                else
                {
                    List<string> oldNames = dxmlColumnOldNames.Split(',').ToList();
                    isColumnInCurrentTable = CurrentTable.Columns.Where(c => oldNames.Contains(c.Name.ToLower())).Any();
                }
            }

            if (!isColumnInCurrentTable && CurrentTable.Columns.Where(c => c.Name.ToLower() == dxmlColumnName).Any())
            {
                columnName = dxmlColumnName;
                isColumnInCurrentTable = true;
            }

            if (!columnName.Contains(","))
            {
                return CurrentTable.Columns.Where(c => c.Name.ToLower() == columnName).First();
            }
            else
            {
                List<string> names = columnName.Split(',').ToList();
                return CurrentTable.Columns.Where(c => names.Contains(c.Name.ToLower())).First();
            }
        }

        protected override List<ColumnDefinition> GetDroppedColumns()
        {
            List<string> dxmlTableColumnsNames = DXMLTable.Columns.Select(c => c.Name.ToLower()).ToList();
            List<string> dxmlTableColumnsOldNames = DXMLTable.Columns.Where(c => c.OldNames != null).Select(c => c.OldNames.ToLower()).ToList();
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

            List<ColumnDefinition> droppedColumns = CurrentTable.Columns.Where(c => !dxmlTableColumnsNames.Contains(c.Name.ToLower()) && !c.Name.ToLower().StartsWith("drop_")).ToList();
            return droppedColumns;
        }

        protected override string GetRenameTableScript()
        {
            string renameTableScript = "";
            string dropRelationsScript = "";

            if (IsTableRenamed())
            {
                List<RelationDefinition> relations = GetRelationsForDBTable(CurrentTable.Name, false);
                foreach(var relation in relations)
                {
                    dropRelationsScript += GetDropRelationScript(relation);
                }
                renameTableScript += "-- Rename Table From " + TableMigrations.CurrentTableName + " To " + TableMigrations.DxmlTableName + "\n";
                renameTableScript += "EXEC SP_RENAME '" + TableMigrations.DxmlTableSchema + "." + TableMigrations.CurrentTableName + "', '" + TableMigrations.DxmlTableName + "'";
                renameTableScript += ";\n\n";

                string renameTableWithHistoryScript = dropRelationsScript + renameTableScript + GetInsertScriptForMigrationsHistory("Rename Table", TableMigrations.CurrentTableName, null, renameTableScript);

                return renameTableWithHistoryScript;
            }

            return null;
        }

        protected override string GetAddColumnScript(ColumnMigration columnMigration)
        {
            string addScript = "-- Add New Column With Name " + columnMigration.NewColumn.Name + "\n";
            addScript += "ALTER TABLE " + "[" + TableMigrations.DxmlTableSchema + "].[" + TableMigrations.DxmlTableName + "]" + " ";
            addScript += "ADD " + "[" + columnMigration.NewColumn.Name + "]" + " ";
            addScript += GetDataTypeScript(columnMigration.NewColumn.Type, columnMigration.NewColumn.Size, columnMigration.NewColumn.Precision, columnMigration.NewColumn.Scale);
            addScript += GetDefaultValueScript(columnMigration.NewColumn.Constraints.Nullable, columnMigration.NewColumn.Type, columnMigration.NewColumn.DefaultValue);
            addScript += columnMigration.NewColumn.Constraints.Nullable ? " NULL" : " NOT NULL";
            addScript += ";\n\n";

            string addWithHistoryScript = addScript + GetInsertScriptForMigrationsHistory("Add Column", TableMigrations.DxmlTableName, columnMigration.NewColumn.Name, addScript);

            return addWithHistoryScript;
        }

        protected override string GetRenameColumnScript(ColumnMigration columnMigration)
        {
            string renameScript = "-- Rename Column From " + columnMigration.CurrentColumn.Name + " To " + columnMigration.NewColumn.Name + "\n";
            renameScript += "EXEC SP_RENAME '" + TableMigrations.DxmlTableSchema + "." + TableMigrations.DxmlTableName + "." + columnMigration.CurrentColumn.Name + "', '" + columnMigration.NewColumn.Name + "', 'COLUMN'";
            renameScript += ";\n\n";

            string renameWithHistoryScript = renameScript + GetInsertScriptForMigrationsHistory("Rename Column", TableMigrations.DxmlTableName, columnMigration.CurrentColumn.Name, renameScript);

            return renameWithHistoryScript;
        }

        protected override string GetDropColumnScript(ColumnMigration columnMigration)
        {
            string dropScript = "-- Drop Column " + columnMigration.CurrentColumn.Name + "\n";
            dropScript += "EXEC SP_RENAME '" + TableMigrations.DxmlTableSchema + "." + TableMigrations.DxmlTableName + "." + columnMigration.CurrentColumn.Name + "', '" + "Drop_" + columnMigration.CurrentColumn.Name + "', 'COLUMN'";
            dropScript += ";\n\n";

            string dropWithHistoryScript = dropScript + GetInsertScriptForMigrationsHistory("Drop Column", TableMigrations.DxmlTableName, columnMigration.CurrentColumn.Name, dropScript);

            return dropWithHistoryScript;
        }

        protected override string GetAlterTypeScript(ColumnMigration columnMigration)////
        {
            string alterTypeScript = "";
            string dropRelationsScript = "";
            string dropIndexsScript = "";
            string dropUniqueConstraintsScript = "";

            RelationDefinition foreignKeyRelation = CurrentTable.Relations.Where(r => (!r.ForeignKeyColumn.Contains(",") && r.ForeignKeyColumn == columnMigration.CurrentColumn.Name) || (r.ForeignKeyColumn.Contains(",") && r.ForeignKeyColumn.Contains(columnMigration.CurrentColumn.Name))).FirstOrDefault();
            if (foreignKeyRelation != null)
            {
                dropRelationsScript += GetDropRelationScript(foreignKeyRelation);
            }

            List<IndexDefinition> indexes = CurrentTable.Indexes.Where(i => (!i.Columns.Contains(",") && i.Columns == columnMigration.CurrentColumn.Name) || (i.Columns.Contains(",") && i.Columns.Contains(columnMigration.CurrentColumn.Name))).ToList();
            foreach(var index in indexes)
            {
                dropIndexsScript += GetDropIndexScript(index);
            }

            List<UniqueConstraintDefinition> uniqueConstraints = CurrentTable.UniqueConstraints.Where(u => (!u.Columns.Contains(",") && u.Columns == columnMigration.CurrentColumn.Name) || (u.Columns.Contains(",") && u.Columns.Contains(columnMigration.CurrentColumn.Name))).ToList();
            foreach (var uniqueConstraint in uniqueConstraints)
            {
                dropUniqueConstraintsScript += GetDropUniqueConstraintScript(uniqueConstraint);
            }

            alterTypeScript += "-- Change Type From " + columnMigration.CurrentColumn.Type + " To " + columnMigration.NewColumn.Type + " For Column " + columnMigration.CurrentColumn.Name + "\n";
            alterTypeScript += "ALTER TABLE " + "[" + TableMigrations.DxmlTableSchema + "].[" + TableMigrations.DxmlTableName + "]" + " ";
            alterTypeScript += "ALTER COLUMN " + "[" + columnMigration.CurrentColumn.Name + "]" + " ";
            alterTypeScript += GetDataTypeScript(columnMigration.NewColumn.Type, (columnMigration.CurrentColumn.Size == 0 ? 1 : columnMigration.CurrentColumn.Size), columnMigration.NewColumn.Precision, columnMigration.NewColumn.Scale);
            if (!columnMigration.CurrentColumn.Constraints.Nullable)
            {
                alterTypeScript += " NOT NULL";
            }
            alterTypeScript += ";\n\n";

            string alterTypeWithHistoryScript = dropRelationsScript + dropIndexsScript + dropUniqueConstraintsScript + alterTypeScript + GetInsertScriptForMigrationsHistory("Alter Column Type", TableMigrations.DxmlTableName, columnMigration.CurrentColumn.Name, alterTypeScript);

            return alterTypeWithHistoryScript;
        }

        protected override string GetAlterSizeScript(ColumnMigration columnMigration)////
        {
            string alterSizeScript = "";
            string dropRelationsScript = "";
            string dropIndexsScript = "";
            string dropUniqueConstraintsScript = "";

            List<RelationDefinition> relations = CurrentTable.Relations;
            RelationDefinition foreignKeyRelation = relations.Where(r => (!r.ForeignKeyColumn.Contains(",") && r.ForeignKeyColumn == columnMigration.CurrentColumn.Name) || (r.ForeignKeyColumn.Contains(",") && r.ForeignKeyColumn.Contains(columnMigration.CurrentColumn.Name))).FirstOrDefault();
            if (foreignKeyRelation != null)
            {
                dropRelationsScript += GetDropRelationScript(foreignKeyRelation);
            }

            List<IndexDefinition> indexes = CurrentTable.Indexes.Where(i => (!i.Columns.Contains(",") && i.Columns == columnMigration.CurrentColumn.Name) || (i.Columns.Contains(",") && i.Columns.Contains(columnMigration.CurrentColumn.Name))).ToList();
            foreach (var index in indexes)
            {
                dropIndexsScript += GetDropIndexScript(index);
            }

            List<UniqueConstraintDefinition> uniqueConstraints = CurrentTable.UniqueConstraints.Where(u => (!u.Columns.Contains(",") && u.Columns == columnMigration.CurrentColumn.Name) || (u.Columns.Contains(",") && u.Columns.Contains(columnMigration.CurrentColumn.Name))).ToList();
            foreach (var uniqueConstraint in uniqueConstraints)
            {
                dropUniqueConstraintsScript += GetDropUniqueConstraintScript(uniqueConstraint);
            }

            bool IsAlterTypeInMigrationsList = TableMigrations.ColumnsMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERTYPE).Any();
            alterSizeScript += "-- Change Size From " + columnMigration.CurrentColumn.Size + " To " + columnMigration.NewColumn.Size + " For Column " + columnMigration.CurrentColumn.Name + "\n";
            alterSizeScript += "ALTER TABLE " + "[" + TableMigrations.DxmlTableSchema + "].[" + TableMigrations.DxmlTableName + "]" + " ";
            alterSizeScript += "ALTER COLUMN " + "[" + columnMigration.CurrentColumn.Name + "]" + " ";
            alterSizeScript += GetDataTypeScript((IsAlterTypeInMigrationsList ? columnMigration.NewColumn.Type : columnMigration.CurrentColumn.Type), columnMigration.NewColumn.Size, 0, 0);
            if (!columnMigration.CurrentColumn.Constraints.Nullable)
            {
                alterSizeScript += " NOT NULL";
            }
            alterSizeScript += ";\n\n";

            string alterSizeWithHistoryScript = dropRelationsScript + dropIndexsScript + dropUniqueConstraintsScript + alterSizeScript + GetInsertScriptForMigrationsHistory("Alter Column Size", TableMigrations.DxmlTableName, columnMigration.CurrentColumn.Name, alterSizeScript);

            return alterSizeWithHistoryScript;
        }

        protected override string GetAddPrimaryKeyScript(ColumnMigration columnMigration)
        {
            List<string> droppedColumnsNames = GetDroppedColumns().Select(c => c.Name.ToLower()).ToList();
            if(CurrentTable.Columns.Where(c => c.Constraints.PrimaryKey && !droppedColumnsNames.Contains(c.Name.ToLower())).Any())
            {
                string primaryKeyColumns = string.Join(",", CurrentTable.Columns.Where(c => c.Constraints.PrimaryKey && !droppedColumnsNames.Contains(c.Name.ToLower())).Select(c => "[" + c.Name + "]").ToArray());
                string primaryKeyConstraintName = columnMigration.CurrentColumn.Constraints.PrimaryKeyConstraintName;
                string addPrimaryKeyScript = "-- Add Primary Key Constraint\n";
                addPrimaryKeyScript += "EXEC('ALTER TABLE " + "[" + TableMigrations.DxmlTableSchema + "].[" + TableMigrations.DxmlTableName + "]" + " ADD CONSTRAINT " + "[" + primaryKeyConstraintName + "]" + " PRIMARY KEY (" + primaryKeyColumns + ")')";
                addPrimaryKeyScript += ";\n\n";

                string addPrimaryKeyWithHistoryScript = addPrimaryKeyScript + GetInsertScriptForMigrationsHistory("Add Primary Key", TableMigrations.DxmlTableName, primaryKeyColumns.Replace("[", String.Empty).Replace("]", String.Empty), addPrimaryKeyScript);

                return addPrimaryKeyWithHistoryScript;
            }
            else
            {
                return null;
            }
        }

        protected override string GetDropPrimaryKeyScript(ColumnMigration columnMigration)
        {
            string dropPrimaryKeyScript = "";
            string dropRelationsScript = "";

            List<RelationDefinition> relations = GetRelationsForDBTable(CurrentTable.Name, false);
            foreach (var relation in relations)
            {
                dropRelationsScript += GetDropRelationScript(relation);
            }
            dropPrimaryKeyScript += "-- Drop Primary Key Constraint\n";
            string primaryKeyConstraintName = columnMigration.CurrentColumn.Constraints.PrimaryKeyConstraintName;
            dropPrimaryKeyScript += "EXEC('IF (OBJECT_ID(''" + TableMigrations.DxmlTableSchema + "." + primaryKeyConstraintName + "'', ''PK'') IS NOT NULL) BEGIN ALTER TABLE " + "[" + TableMigrations.DxmlTableSchema + "].[" + TableMigrations.DxmlTableName + "]" + " DROP CONSTRAINT " + "[" + primaryKeyConstraintName + "]" + " END" + "')";
            dropPrimaryKeyScript += ";\n\n";

            string dropPrimaryKeyWithHistoryScript = dropRelationsScript + dropPrimaryKeyScript + GetInsertScriptForMigrationsHistory("Drop Primary Key", TableMigrations.DxmlTableName, null, dropPrimaryKeyScript);

            return dropPrimaryKeyWithHistoryScript;
        }
        
        protected override string GetSetNullableScript(ColumnMigration columnMigration)
        {
            bool isAlterTypeInMigrationsList = TableMigrations.ColumnsMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERTYPE).Any();
            bool isAlterSizeInMigrationsList = TableMigrations.ColumnsMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERSIZE).Any();
            bool isAlterPrecisionAndScaleInMigrationsList = TableMigrations.ColumnsMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERPRECISIONANDSCALE).Any();
            string setNullableScript = "-- Set Nullable For Column " + columnMigration.CurrentColumn.Name + "\n";
            setNullableScript += "ALTER TABLE " + "[" + TableMigrations.DxmlTableSchema + "].[" + TableMigrations.DxmlTableName + "]" + " ";
            setNullableScript += "ALTER COLUMN " + "[" + columnMigration.CurrentColumn.Name + "]" + " ";
            setNullableScript += GetDataTypeScript((isAlterTypeInMigrationsList ? columnMigration.NewColumn.Type : columnMigration.CurrentColumn.Type), (isAlterSizeInMigrationsList ? columnMigration.NewColumn.Size : columnMigration.CurrentColumn.Size), (isAlterPrecisionAndScaleInMigrationsList ? columnMigration.NewColumn.Precision : columnMigration.CurrentColumn.Precision), (isAlterPrecisionAndScaleInMigrationsList ? columnMigration.NewColumn.Scale : columnMigration.CurrentColumn.Scale));
            setNullableScript += " NULL";
            setNullableScript += ";\n\n";

            string setNullableWithHistoryScript = setNullableScript + GetInsertScriptForMigrationsHistory("Set Column Nullable", TableMigrations.DxmlTableName, columnMigration.CurrentColumn.Name, setNullableScript);

            return setNullableWithHistoryScript;
        }

        protected override string GetUnsetNullableScript(ColumnMigration columnMigration)////
        {
            string dropIndexsScript = "";
            string dropUniqueConstraintsScript = "";

            List<IndexDefinition> indexes = CurrentTable.Indexes.Where(i => (!i.Columns.Contains(",") && i.Columns == columnMigration.CurrentColumn.Name) || (i.Columns.Contains(",") && i.Columns.Contains(columnMigration.CurrentColumn.Name))).ToList();
            foreach (var index in indexes)
            {
                dropIndexsScript += GetDropIndexScript(index);
            }

            List<UniqueConstraintDefinition> uniqueConstraints = CurrentTable.UniqueConstraints.Where(u => (!u.Columns.Contains(",") && u.Columns == columnMigration.CurrentColumn.Name) || (u.Columns.Contains(",") && u.Columns.Contains(columnMigration.CurrentColumn.Name))).ToList();
            foreach (var uniqueConstraint in uniqueConstraints)
            {
                dropUniqueConstraintsScript += GetDropUniqueConstraintScript(uniqueConstraint);
            }

            bool isAlterTypeInMigrationsList = TableMigrations.ColumnsMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERTYPE).Any();
            bool isAlterSizeInMigrationsList = TableMigrations.ColumnsMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERSIZE).Any();
            bool isAlterPrecisionAndScaleInMigrationsList = TableMigrations.ColumnsMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERPRECISIONANDSCALE).Any();
            string unsetNullableScript = "-- Unset Nullable For Column " + columnMigration.CurrentColumn.Name + "\n";
            unsetNullableScript += "ALTER TABLE " + "[" + TableMigrations.DxmlTableSchema + "].[" + TableMigrations.DxmlTableName + "]" + " ";
            unsetNullableScript += "ALTER COLUMN " + "[" + columnMigration.CurrentColumn.Name + "]" + " ";
            unsetNullableScript += GetDataTypeScript((isAlterTypeInMigrationsList ? columnMigration.NewColumn.Type : columnMigration.CurrentColumn.Type), (isAlterSizeInMigrationsList ? columnMigration.NewColumn.Size : columnMigration.CurrentColumn.Size), (isAlterPrecisionAndScaleInMigrationsList ? columnMigration.NewColumn.Precision : columnMigration.CurrentColumn.Precision), (isAlterPrecisionAndScaleInMigrationsList ? columnMigration.NewColumn.Scale : columnMigration.CurrentColumn.Scale));
            unsetNullableScript += " NOT NULL";
            unsetNullableScript += ";\n\n";

            string unsetNullableWithHistoryScript = unsetNullableScript + GetInsertScriptForMigrationsHistory("Unset Column Nullable", TableMigrations.DxmlTableName, columnMigration.CurrentColumn.Name, unsetNullableScript);

            return unsetNullableWithHistoryScript;
        }

        protected override string GetAlterPrecisionAndScaleScript(ColumnMigration columnMigration)////
        {
            string alterPrecisionAndScaleScript = "";
            string dropRelationsScript = "";
            string dropIndexsScript = "";
            string dropUniqueConstraintsScript = "";
            
            List<RelationDefinition> relations = CurrentTable.Relations;
            RelationDefinition foreignKeyRelation = relations.Where(r => (!r.ForeignKeyColumn.Contains(",") && r.ForeignKeyColumn == columnMigration.CurrentColumn.Name) || (r.ForeignKeyColumn.Contains(",") && r.ForeignKeyColumn.Contains(columnMigration.CurrentColumn.Name))).FirstOrDefault();
            if (foreignKeyRelation != null)
            {
                dropRelationsScript += GetDropRelationScript(foreignKeyRelation);
            }

            List<IndexDefinition> indexes = CurrentTable.Indexes.Where(i => (!i.Columns.Contains(",") && i.Columns == columnMigration.CurrentColumn.Name) || (i.Columns.Contains(",") && i.Columns.Contains(columnMigration.CurrentColumn.Name))).ToList();
            foreach (var index in indexes)
            {
                dropIndexsScript += GetDropIndexScript(index);
            }

            List<UniqueConstraintDefinition> uniqueConstraints = CurrentTable.UniqueConstraints.Where(u => (!u.Columns.Contains(",") && u.Columns == columnMigration.CurrentColumn.Name) || (u.Columns.Contains(",") && u.Columns.Contains(columnMigration.CurrentColumn.Name))).ToList();
            foreach (var uniqueConstraint in uniqueConstraints)
            {
                dropUniqueConstraintsScript += GetDropUniqueConstraintScript(uniqueConstraint);
            }

            alterPrecisionAndScaleScript += "-- Change Precision And Scale From " + "(" + columnMigration.CurrentColumn.Precision + ", " + columnMigration.CurrentColumn.Scale + ")" + " To " + "(" + columnMigration.NewColumn.Precision + ", " + columnMigration.NewColumn.Scale + ")" + " For Column " + columnMigration.CurrentColumn.Name + "\n";
            alterPrecisionAndScaleScript += "ALTER TABLE " + "[" + TableMigrations.DxmlTableSchema + "].[" + TableMigrations.DxmlTableName + "]" + " ";
            alterPrecisionAndScaleScript += "ALTER COLUMN " + "[" + columnMigration.CurrentColumn.Name + "]" + " ";
            alterPrecisionAndScaleScript += GetDataTypeScript(columnMigration.CurrentColumn.Type, columnMigration.CurrentColumn.Size, columnMigration.NewColumn.Precision, columnMigration.NewColumn.Scale);
            if (!columnMigration.CurrentColumn.Constraints.Nullable)
            {
                alterPrecisionAndScaleScript += " NOT NULL";
            }
            alterPrecisionAndScaleScript += ";\n\n";

            string alterPrecisionAndScaleWithHistoryScript = dropRelationsScript + dropIndexsScript + dropUniqueConstraintsScript + alterPrecisionAndScaleScript + GetInsertScriptForMigrationsHistory("Alter Column Precision And Scale", TableMigrations.DxmlTableName, columnMigration.CurrentColumn.Name, alterPrecisionAndScaleScript);

            return alterPrecisionAndScaleWithHistoryScript;
        }

        protected override string GetPrimaryKeyConstraintScript()
        {
            string alterPrimaryKeyScript = "";
            if (IsTableHasPrimaryKeys(CurrentTable))
            {
                alterPrimaryKeyScript += GetAlterPrimaryKeyScript();
            }
            else if (IsTableHasPrimaryKeys(DXMLTable))
            {
                string primaryKeyConstraintName = "PK_" + TableMigrations.DxmlTableName + "_" + GenerateRandomString();
                alterPrimaryKeyScript += GetAddPrimaryKeyConstraintScript(primaryKeyConstraintName); 
            }
            return alterPrimaryKeyScript;
        }

        protected override string GetDropPrimaryKeyConstraintScript(string primaryKeyConstraintName)
        {
            string dropRelationsScript = "";

            List<RelationDefinition> relations = GetRelationsForDBTable(CurrentTable.Name, false);
            foreach (var relation in relations)
            {
                dropRelationsScript += GetDropRelationScript(relation);
            }

            string dropPrimaryKeyScript = "-- Drop Primary Key Constraint\n";
            dropPrimaryKeyScript += "EXEC('IF (OBJECT_ID(''" + TableMigrations.DxmlTableSchema + "." + primaryKeyConstraintName + "'', ''PK'') IS NOT NULL) BEGIN ALTER TABLE " + "[" + TableMigrations.DxmlTableSchema + "].[" + TableMigrations.DxmlTableName + "]" + " DROP CONSTRAINT " + "[" + primaryKeyConstraintName + "]" + " END" + "');";
            return dropRelationsScript + dropPrimaryKeyScript + "\n\n" + GetInsertScriptForMigrationsHistory("Drop Primary Key Constraint", TableMigrations.DxmlTableName, null, dropPrimaryKeyScript);
        }
        
        protected override string GetAddPrimaryKeyConstraintScript(string primaryKeyConstraintName)
        {
            string addPrimaryKeyConstraintScript = "-- Add Primary Key Constraint\n";
            string primaryKeyColumns = string.Join(",", DXMLTable.Columns.Where(c => c.Constraints.PrimaryKey).Select(c => "[" + c.Name + "]").ToArray());
            addPrimaryKeyConstraintScript += "EXEC('ALTER TABLE " + "[" + TableMigrations.DxmlTableSchema + "].[" + TableMigrations.DxmlTableName + "]" + " ADD CONSTRAINT " + "[" + primaryKeyConstraintName + "]" + " PRIMARY KEY (" + primaryKeyColumns + ")');";
            return addPrimaryKeyConstraintScript + "\n\n" + GetInsertScriptForMigrationsHistory("Add Primary Key Constraint", TableMigrations.DxmlTableName, primaryKeyColumns.Replace("[", String.Empty).Replace("]", String.Empty), addPrimaryKeyConstraintScript);
        }

        protected override string GetCreateRelationScript(RelationDefinition relation)
        {
            string createRelationScript = "-- Add Foreign Key Constraint For Column " + relation.ForeignKeyColumn + " In Table " + DXMLTable.Name + " As Reference To Column " + relation.ReferencedColumn + " In Table " + relation.ReferencedTable + "\n";
            string foreignKeyColumns = relation.ForeignKeyColumn.Contains(",") ? string.Join(",", relation.ForeignKeyColumn.Split(',').Select(c => "[" + c + "]").ToArray()) : "[" + relation.ForeignKeyColumn + "]";
            string referencedColumns = relation.ReferencedColumn.Contains(",") ? string.Join(",", relation.ReferencedColumn.Split(',').Select(c => "[" + c + "]").ToArray()) : "[" + relation.ReferencedColumn + "]";
            createRelationScript += "EXEC('ALTER TABLE " + "[" + DXMLTable.Schema + "].[" + DXMLTable.Name + "]" + " ADD FOREIGN KEY(" + foreignKeyColumns + ") REFERENCES " + "[" + relation.ReferencedTableSchema + "]." + "[" + relation.ReferencedTable + "]" + "(" + referencedColumns + ")')";
            createRelationScript += ";\n\n";

            string createRelationWithHistoryScript = createRelationScript + GetInsertScriptForMigrationsHistory("Create Relation", DXMLTable.Name, relation.ForeignKeyColumn, createRelationScript);

            IndexDefinition relationIndex = new IndexDefinition
            {
                Columns = relation.ForeignKeyColumn
            };

            string createIndexScript = GetCreateIndexScript(relationIndex);

            return createRelationWithHistoryScript + createIndexScript;
        }
        
        protected override string GetDropRelationScript(RelationDefinition relation)
        {
            string dropRelationScript = "-- Drop Foreign Key Constraint For Column " + relation.ForeignKeyColumn + " In Table " + relation.ParentTable + " That Reference To Column " + relation.ReferencedColumn + " In Table " + relation.ReferencedTable + "\n";
            dropRelationScript += "EXEC('IF (OBJECT_ID(''" + relation.ParentTableSchema + "." + relation.ForeignKeyConstraintName + "'', ''F'') IS NOT NULL) BEGIN ALTER TABLE " + "[" + relation.ParentTableSchema + "].[" + relation.ParentTable + "]" + " DROP CONSTRAINT " + "[" + relation.ForeignKeyConstraintName + "]" + " END" + "')";
            dropRelationScript += ";\n\n";

            string dropRelationWithHistoryScript = dropRelationScript + GetInsertScriptForMigrationsHistory("Drop Relation", relation.ParentTable, null, dropRelationScript);

            string dropIndexScript = null;

            if(relation.ParentTable == CurrentTable.Name)
            {
                IndexDefinition relationIndex = CurrentTable.AllIndexes.Where(i => i.Columns == relation.ForeignKeyColumn).FirstOrDefault();
                if (relationIndex != null)
                {
                    dropIndexScript = GetDropIndexScript(relationIndex);
                }
            }

            return dropRelationWithHistoryScript + dropIndexScript;
        }

        protected override string GetInsertScriptForMigrationsHistory(string migrationType, string tableName, string columnName, string script)
        {
            if (!String.IsNullOrEmpty(script))
            {
                string insertScript = "INSERT INTO [dbo].[DBMigrationsHistory]([DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('" + DXMLFileName + "', '" + tableName + "', " + (columnName == null ? "NULL" : "'" + columnName + "'") + ", '" + migrationType + "', GETDATE(), '" + script.Replace("'", "''").TrimEnd(new char[] { '\r', '\n' }) + "');" + "\n\n";
                return insertScript;
            }

            return "";
        }

        protected override string GetDefaultValueScript(bool nullable, string type, string defaultValue)
        {
            if (nullable)
            {
                return null;
            }

            if(type == "bit" && String.IsNullOrEmpty(defaultValue))
            {
                return " DEFAULT(0)";
            }

            if (!String.IsNullOrEmpty(defaultValue))
            {
                if (defaultValue.ToLower() == "CurrentDate".ToLower())
                {
                    return " DEFAULT(GETDATE())";
                }

                return " DEFAULT(" + defaultValue + ")";
            }

            return null;
        }

        protected override string GetAddDefaultScript(ColumnMigration columnMigration)
        {
            string addDefaultScript = "-- Add Default Value For Column " + columnMigration.CurrentColumn.Name + "\n";
            addDefaultScript += "ALTER TABLE " + "[" + TableMigrations.DxmlTableSchema + "].[" + TableMigrations.DxmlTableName + "]" + " ADD DEFAULT " + (columnMigration.NewColumn.DefaultValue.ToLower() == "CurrentDate".ToLower() ? "GETDATE()" : columnMigration.NewColumn.DefaultValue) + " FOR [" + columnMigration.CurrentColumn.Name + "]";
            addDefaultScript += ";\n\n";

            string addDefaultWithHistoryScript = addDefaultScript + GetInsertScriptForMigrationsHistory("Add Default Value", TableMigrations.DxmlTableName, columnMigration.CurrentColumn.Name, addDefaultScript);

            return addDefaultWithHistoryScript;
        }

        protected override string GetDropDefaultScript(ColumnMigration columnMigration)
        {
            string dropDefaultScript = "-- Drop Default Value For Column " + columnMigration.CurrentColumn.Name + "\n";
            dropDefaultScript += "EXEC('IF (OBJECT_ID(''" + TableMigrations.DxmlTableSchema + "." + columnMigration.CurrentColumn.Constraints.DefaultConstraintName + "'', ''D'') IS NOT NULL) BEGIN ALTER TABLE " + "[" + TableMigrations.DxmlTableSchema + "].[" + TableMigrations.DxmlTableName + "]" + " DROP CONSTRAINT " + "[" + columnMigration.CurrentColumn.Constraints.DefaultConstraintName + "]" + " END" + "')";
            dropDefaultScript += ";\n\n";

            string dropDefaultWithHistoryScript = dropDefaultScript + GetInsertScriptForMigrationsHistory("Drop Default Value", TableMigrations.DxmlTableName, columnMigration.CurrentColumn.Name, dropDefaultScript);

            return dropDefaultWithHistoryScript;
        }

        protected override string GetCreateIndexScript(IndexDefinition index)
        {
            string indexColumns = !index.Columns.Contains(",") ? "[" + index.Columns + "]" : string.Join(",", index.Columns.Split(',').Select(c => "[" + c + "]").ToArray());
            string includeColumns = String.IsNullOrEmpty(index.Include) ? null : (!index.Include.Contains(",") ? "[" + index.Include + "]" : string.Join(",", index.Include.Split(',').Select(c => "[" + c + "]").ToArray()));
            string tableName = "[" + DXMLTable.Schema + "].[" + DXMLTable.Name + "]";
            string createIndexScript = "-- Create Index On " + DXMLTable.Name + " Table\n";
            string indexName = "IX_" + DXMLTable.Name + "_" + (!indexColumns.Contains(",") ? indexColumns : string.Join("_", indexColumns.Split(',').ToArray())).Replace("[", String.Empty).Replace("]", String.Empty);
            if (includeColumns != null)
            {
                createIndexScript += "EXEC('CREATE NONCLUSTERED INDEX " + "[" + indexName + "]" + " ON " + tableName + "(" + indexColumns + ") INCLUDE(" + includeColumns + ")')";
            }
            else
            {
                createIndexScript += "EXEC('CREATE NONCLUSTERED INDEX " + "[" + indexName + "]" + " ON " + tableName + "(" + indexColumns + ")')";
            }
            
            createIndexScript += ";\n\n";

            string addIndexWithHistoryScript = createIndexScript + GetInsertScriptForMigrationsHistory("Create Index", DXMLTable.Name, indexColumns.Replace("[", String.Empty).Replace("]", String.Empty), createIndexScript);

            return addIndexWithHistoryScript;
        }

        protected override string GetCreateUniqueConstraintScript(UniqueConstraintDefinition uniqueConstraint)
        {
            string uniqueConstraintColumns = !uniqueConstraint.Columns.Contains(",") ? "[" + uniqueConstraint.Columns + "]" : string.Join(",", uniqueConstraint.Columns.Split(',').Select(c => "[" + c + "]").ToArray());
            string tableName = "[" + DXMLTable.Schema + "].[" + DXMLTable.Name + "]";
            string createUniqueConstraintScript = "-- Create Unique Constraint On " + DXMLTable.Name + " Table\n";
            string uniqueConstraintName = "UQ_" + DXMLTable.Name + "_" + (!uniqueConstraintColumns.Contains(",") ? uniqueConstraintColumns : string.Join("_", uniqueConstraintColumns.Split(',').ToArray())).Replace("[", String.Empty).Replace("]", String.Empty);
            createUniqueConstraintScript += "EXEC('ALTER TABLE " + tableName + " ADD CONSTRAINT " + "[" + uniqueConstraintName + "]" + " UNIQUE(" + uniqueConstraintColumns + ")')";

            createUniqueConstraintScript += ";\n\n";

            string addIndexWithHistoryScript = createUniqueConstraintScript + GetInsertScriptForMigrationsHistory("Create Unique Constraint", DXMLTable.Name, uniqueConstraintColumns.Replace("[", String.Empty).Replace("]", String.Empty), createUniqueConstraintScript);

            return addIndexWithHistoryScript;
        }

        protected override string GetDropUniqueConstraintScript(UniqueConstraintDefinition uniqueConstraint)
        {
            string tableName = "[" + DXMLTable.Schema + "].[" + DXMLTable.Name + "]";
            string dropUniqueConstraintScript = "-- Drop Unique Constraint " + uniqueConstraint.ConstraintName + " From Table " + DXMLTable.Name + "\n";
            dropUniqueConstraintScript += "EXEC('IF (OBJECT_ID(''" + DXMLTable.Schema + "." + uniqueConstraint.ConstraintName + "'', ''UQ'') IS NOT NULL) BEGIN ALTER TABLE " + tableName + " DROP CONSTRAINT " + "[" + uniqueConstraint.ConstraintName + "]" + " END" + "')";
            dropUniqueConstraintScript += ";\n\n";

            string dropUniqueConstraintWithHistoryScript = dropUniqueConstraintScript + GetInsertScriptForMigrationsHistory("Drop Unique Constraint", DXMLTable.Name, null, dropUniqueConstraintScript);

            return dropUniqueConstraintWithHistoryScript;
        }

        protected override string GetDropIndexScript(IndexDefinition index)
        {
            string tableName = "[" + DXMLTable.Schema + "].[" + DXMLTable.Name + "]";
            string dropIndexScript = "-- Drop Index " + index.IndexName + " From Table " + DXMLTable.Name + "\n";
            dropIndexScript += "EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''" + index.IndexName + "'' AND object_id = OBJECT_ID(''" + tableName + "'', ''U'')) BEGIN DROP INDEX " + "[" + index.IndexName + "]" + " ON " + tableName + " End')";
            dropIndexScript += ";\n\n";

            string dropIndexWithHistoryScript = dropIndexScript + GetInsertScriptForMigrationsHistory("Drop Index", DXMLTable.Name, null, dropIndexScript);

            return dropIndexWithHistoryScript;
        }
    }
}