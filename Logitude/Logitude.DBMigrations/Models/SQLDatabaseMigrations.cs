using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Logitude.DBMigrations.Models
{
    public class SQLDatabaseMigrations : DatabaseMigrations
    {
        public SQLDatabaseMigrations(TableDefinition dxmlTable, string connectionString, List<TableDefinition> dxmlTables)
        {
            ConnectionString = connectionString;
            DXMLTable = dxmlTable;
            DXMLTables = dxmlTables;
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
            string queryString = @"SELECT Q1.*, Q2.ConstraintType, Q2.ConstraintName " +
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
                            Type = GetColumnDefinitionDataType(reader["DataType"].ToString()),
                            Size = GetColumnDefinitionSize(reader["Size"].ToString()),
                            Precision = String.IsNullOrEmpty(reader["Precision"].ToString()) ? 0 : Convert.ToInt32(reader["Precision"].ToString()),
                            Scale = String.IsNullOrEmpty(reader["Scale"].ToString()) ? 0 : Convert.ToInt32(reader["Scale"].ToString()),
                            Constraints = new ConstraintsDefinition
                            {
                                Nullable = (reader["Nullable"].ToString().ToLower() == "yes")
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
                    Columns = currentTableColumns,
                    Relations = GetRelationsForDBTable(tableName, true)
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

            List<RelationDefinition> relations = null;

            SqlDataReader reader = null;
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@tableName", tableName);

            try
            {
                connection.Open();
                reader = command.ExecuteReader();

                relations = new List<RelationDefinition>();

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
            columnScript += GetDataTypeScript(columnDefinition.Type, columnDefinition.Size, columnDefinition.Precision, columnDefinition.Scale);
            columnScript += (columnDefinition.Constraints.Nullable ? " NULL" : " NOT NULL");
            columnScript += ",";
            return columnScript;
        }

        protected override bool IsTableRenamed()
        {
            return TableMigrations.DxmlTableName != TableMigrations.CurrentTableName;
        }

        protected override bool IsColumnRenamed(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            return currentTableColumn.Name != dxmlTableColumn.Name;
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

        protected override bool IsRelationInCurrentTable(RelationDefinition relation)//dxml relation
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

            bool isRelationInCurrentTable = CurrentTable.Relations.Where(r => r.ForeignKeyColumn == relation.ForeignKeyColumn && r.ReferencedTable == relation.ReferencedTable && r.ReferencedColumn == relation.ReferencedColumn).Any();

            return (!isForeignKeyDataTypeChanged && isRelationInCurrentTable);
        }

        protected override bool IsRelationInDXMLTable(RelationDefinition relation)//db relation
        {
            return DXMLTable.Relations.Where(r => r.ForeignKeyColumn == relation.ForeignKeyColumn && r.ReferencedTable == relation.ReferencedTable && r.ReferencedColumn == relation.ReferencedColumn).Any();
        }

        protected override bool IsColumnInCurrentTable(string dxmlColumnName, string dxmlColumnShortName, string dxmlColumnOldNames)
        {
            bool isColumnInCurrentTable = false;
            if (dxmlColumnOldNames != null)
            {
                if (!dxmlColumnOldNames.Contains(","))
                {
                    isColumnInCurrentTable = CurrentTable.Columns.Where(c => c.Name == dxmlColumnOldNames).Any();
                }
                else
                {
                    List<string> oldNames = dxmlColumnOldNames.Split(',').ToList();
                    isColumnInCurrentTable = CurrentTable.Columns.Where(c => oldNames.Contains(c.Name)).Any();
                }
            }

            if (!isColumnInCurrentTable)
            {
                isColumnInCurrentTable = CurrentTable.Columns.Where(c => c.Name == dxmlColumnName).Any();
            }

            return isColumnInCurrentTable;
        }

        protected override ColumnDefinition GetCurrentTableColumn(string dxmlColumnName, string dxmlColumnShortName, string dxmlColumnOldNames)
        {
            string columnName = dxmlColumnOldNames;
            bool isColumnInCurrentTable = false;
            if (dxmlColumnOldNames != null)
            {
                if (!dxmlColumnOldNames.Contains(','))
                {
                    isColumnInCurrentTable = CurrentTable.Columns.Where(c => c.Name == dxmlColumnOldNames).Any();
                }
                else
                {
                    List<string> oldNames = dxmlColumnOldNames.Split(',').ToList();
                    isColumnInCurrentTable = CurrentTable.Columns.Where(c => oldNames.Contains(c.Name)).Any();
                }
            }

            if (!isColumnInCurrentTable && CurrentTable.Columns.Where(c => c.Name == dxmlColumnName).Any())
            {
                columnName = dxmlColumnName;
                isColumnInCurrentTable = true;
            }

            if (!columnName.Contains(","))
            {
                return CurrentTable.Columns.Where(c => c.Name == columnName).First();
            }
            else
            {
                List<string> names = columnName.Split(',').ToList();
                return CurrentTable.Columns.Where(c => names.Contains(c.Name)).First();
            }
        }

        protected override List<ColumnDefinition> GetDroppedColumns()
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

        protected override string GetRenameTableScript()
        {
            string renameTableScript = "";
            if (IsTableRenamed())
            {
                List<RelationDefinition> relations = GetRelationsForDBTable(CurrentTable.Name, false);
                foreach(var relation in relations)
                {
                    renameTableScript += GetDropRelationScript(relation);
                }
                renameTableScript += "-- Rename Table From " + TableMigrations.CurrentTableName + " To " + TableMigrations.DxmlTableName + "\n";
                renameTableScript += "EXEC SP_RENAME '" + TableMigrations.DxmlTableSchema + "." + TableMigrations.CurrentTableName + "', '" + TableMigrations.DxmlTableName + "'";
                renameTableScript += ";\n\n";
            }
            return renameTableScript;
        }

        protected override string GetAddColumnScript(ColumnMigration columnMigration)
        {
            string addScript = "-- Add New Column With Name " + columnMigration.NewColumn.Name + "\n";
            addScript += "ALTER TABLE " + "[" + TableMigrations.DxmlTableSchema + "].[" + TableMigrations.DxmlTableName + "]" + " ";
            addScript += "ADD " + "[" + columnMigration.NewColumn.Name + "]" + " ";
            addScript += GetDataTypeScript(columnMigration.NewColumn.Type, columnMigration.NewColumn.Size, columnMigration.NewColumn.Precision, columnMigration.NewColumn.Scale);
            addScript += columnMigration.NewColumn.Constraints.Nullable ? " NULL" : " NOT NULL";
            return addScript + ";\n\n";
        }

        protected override string GetRenameColumnScript(ColumnMigration columnMigration)
        {
            string renameScript = "-- Rename Column From " + columnMigration.CurrentColumn.Name + " To " + columnMigration.NewColumn.Name + "\n";
            renameScript += "EXEC SP_RENAME '" + TableMigrations.DxmlTableSchema + "." + TableMigrations.DxmlTableName + "." + columnMigration.CurrentColumn.Name + "', '" + columnMigration.NewColumn.Name + "', 'COLUMN'";
            return renameScript + ";\n\n";
        }

        protected override string GetDropColumnScript(ColumnMigration columnMigration)
        {
            string dropScript = "-- Drop Column " + columnMigration.CurrentColumn.Name + "\n";
            dropScript += "EXEC SP_RENAME '" + TableMigrations.DxmlTableSchema + "." + TableMigrations.DxmlTableName + "." + columnMigration.CurrentColumn.Name + "', '" + "Drop_" + columnMigration.CurrentColumn.Name + "', 'COLUMN'";
            return dropScript + ";\n\n";
        }

        protected override string GetAlterTypeScript(ColumnMigration columnMigration)//
        {
            string alterTypeScript = "";
            List<RelationDefinition> relations = CurrentTable.Relations;
            RelationDefinition foreignKeyRelation = relations.Where(r => (!r.ForeignKeyColumn.Contains(",") && r.ForeignKeyColumn == columnMigration.CurrentColumn.Name) || (r.ForeignKeyColumn.Contains(",") && r.ForeignKeyColumn.Contains(columnMigration.CurrentColumn.Name))).FirstOrDefault();
            if (foreignKeyRelation != null)
            {
                alterTypeScript += GetDropRelationScript(foreignKeyRelation);
            }
            alterTypeScript += "-- Change Type From " + columnMigration.CurrentColumn.Type + " To " + columnMigration.NewColumn.Type + " For Column " + columnMigration.CurrentColumn.Name + "\n";
            alterTypeScript += "ALTER TABLE " + "[" + TableMigrations.DxmlTableSchema + "].[" + TableMigrations.DxmlTableName + "]" + " ";
            alterTypeScript += "ALTER COLUMN " + "[" + columnMigration.CurrentColumn.Name + "]" + " ";
            alterTypeScript += GetDataTypeScript(columnMigration.NewColumn.Type, (columnMigration.CurrentColumn.Size == 0 ? 1 : columnMigration.CurrentColumn.Size), columnMigration.NewColumn.Precision, columnMigration.NewColumn.Scale);
            if (!columnMigration.CurrentColumn.Constraints.Nullable)
            {
                alterTypeScript += " NOT NULL";
            }
            return alterTypeScript + ";\n\n";
        }

        protected override string GetAlterSizeScript(ColumnMigration columnMigration)//
        {
            string alterSizeScript = "";
            List<RelationDefinition> relations = CurrentTable.Relations;
            RelationDefinition foreignKeyRelation = relations.Where(r => (!r.ForeignKeyColumn.Contains(",") && r.ForeignKeyColumn == columnMigration.CurrentColumn.Name) || (r.ForeignKeyColumn.Contains(",") && r.ForeignKeyColumn.Contains(columnMigration.CurrentColumn.Name))).FirstOrDefault();
            if (foreignKeyRelation != null)
            {
                alterSizeScript += GetDropRelationScript(foreignKeyRelation);
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
            return alterSizeScript + ";\n\n";
        }

        protected override string GetAddPrimaryKeyScript(ColumnMigration columnMigration)
        {
            List<string> droppedColumnsNames = GetDroppedColumns().Select(c => c.Name).ToList();
            if(CurrentTable.Columns.Where(c => c.Constraints.PrimaryKey && !droppedColumnsNames.Contains(c.Name)).Any())
            {
                string primaryKeyColumns = string.Join(",", CurrentTable.Columns.Where(c => c.Constraints.PrimaryKey && !droppedColumnsNames.Contains(c.Name)).Select(c => "[" + c.Name + "]").ToArray());
                string primaryKeyConstraintName = columnMigration.CurrentColumn.Constraints.PrimaryKeyConstraintName;
                string addPrimaryKeyScript = "-- Add The Primary Key Constraint\n";
                addPrimaryKeyScript += "EXEC('ALTER TABLE " + "[" + TableMigrations.DxmlTableSchema + "].[" + TableMigrations.DxmlTableName + "]" + " ADD CONSTRAINT " + primaryKeyConstraintName + " PRIMARY KEY (" + primaryKeyColumns + ")')";
                return addPrimaryKeyScript + ";\n\n";
            }
            else
            {
                return null;
            }
        }

        protected override string GetDropPrimaryKeyScript(ColumnMigration columnMigration)//
        {
            string dropPrimaryKeyScript = "";
            List<RelationDefinition> relations = GetRelationsForDBTable(CurrentTable.Name, false);
            foreach (var relation in relations)
            {
                dropPrimaryKeyScript += GetDropRelationScript(relation);
            }
            dropPrimaryKeyScript += "-- Drop The Primary Key Constraint\n";
            string primaryKeyConstraintName = columnMigration.CurrentColumn.Constraints.PrimaryKeyConstraintName;
            dropPrimaryKeyScript += "EXEC('IF (OBJECT_ID(''" + TableMigrations.DxmlTableSchema + "." + primaryKeyConstraintName + "'', ''PK'') IS NOT NULL) BEGIN ALTER TABLE " + "[" + TableMigrations.DxmlTableSchema + "].[" + TableMigrations.DxmlTableName + "]" + " DROP CONSTRAINT " + primaryKeyConstraintName + " END" + "')";
            return dropPrimaryKeyScript + ";\n\n";
        }
        
        protected override string GetSetNullableScript(ColumnMigration columnMigration)
        {
            bool isAlterTypeInMigrationsList = TableMigrations.ColumnsMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERTYPE).Any();
            bool isAlterSizeInMigrationsList = TableMigrations.ColumnsMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERSIZE).Any();
            bool isAlterPrecisionAndScaleInMigrationsList = TableMigrations.ColumnsMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERPRECISIONANDSCALE).Any();
            string setNullableScript = "-- Set Nullable For Column " + columnMigration.CurrentColumn.Name + "\n";
            if (columnMigration.CurrentColumn.Constraints.PrimaryKey)
            {
                setNullableScript += GetPrimaryKeyConstraintScript() + "\n";
            }
            setNullableScript += "ALTER TABLE " + "[" + TableMigrations.DxmlTableSchema + "].[" + TableMigrations.DxmlTableName + "]" + " ";
            setNullableScript += "ALTER COLUMN " + "[" + columnMigration.CurrentColumn.Name + "]" + " ";
            setNullableScript += GetDataTypeScript((isAlterTypeInMigrationsList ? columnMigration.NewColumn.Type : columnMigration.CurrentColumn.Type), (isAlterSizeInMigrationsList ? columnMigration.NewColumn.Size : columnMigration.CurrentColumn.Size), (isAlterPrecisionAndScaleInMigrationsList ? columnMigration.NewColumn.Precision : columnMigration.CurrentColumn.Precision), (isAlterPrecisionAndScaleInMigrationsList ? columnMigration.NewColumn.Scale : columnMigration.CurrentColumn.Scale));
            setNullableScript += " NULL";
            return setNullableScript + ";\n\n";
        }

        protected override string GetUnsetNullableScript(ColumnMigration columnMigration)
        {
            bool isAlterTypeInMigrationsList = TableMigrations.ColumnsMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERTYPE).Any();
            bool isAlterSizeInMigrationsList = TableMigrations.ColumnsMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERSIZE).Any();
            bool isAlterPrecisionAndScaleInMigrationsList = TableMigrations.ColumnsMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERPRECISIONANDSCALE).Any();
            string unsetNullableScript = "-- Unset Nullable For Column " + columnMigration.CurrentColumn.Name + "\n";
            unsetNullableScript += "ALTER TABLE " + "[" + TableMigrations.DxmlTableSchema + "].[" + TableMigrations.DxmlTableName + "]" + " ";
            unsetNullableScript += "ALTER COLUMN " + "[" + columnMigration.CurrentColumn.Name + "]" + " ";
            unsetNullableScript += GetDataTypeScript((isAlterTypeInMigrationsList ? columnMigration.NewColumn.Type : columnMigration.CurrentColumn.Type), (isAlterSizeInMigrationsList ? columnMigration.NewColumn.Size : columnMigration.CurrentColumn.Size), (isAlterPrecisionAndScaleInMigrationsList ? columnMigration.NewColumn.Precision : columnMigration.CurrentColumn.Precision), (isAlterPrecisionAndScaleInMigrationsList ? columnMigration.NewColumn.Scale : columnMigration.CurrentColumn.Scale));
            unsetNullableScript += " NOT NULL";
            return unsetNullableScript + ";\n\n";
        }

        protected override string GetAlterPrecisionAndScaleScript(ColumnMigration columnMigration)//
        {
            string alterPrecisionAndScaleScript = "";
            List<RelationDefinition> relations = CurrentTable.Relations;
            RelationDefinition foreignKeyRelation = relations.Where(r => (!r.ForeignKeyColumn.Contains(",") && r.ForeignKeyColumn == columnMigration.CurrentColumn.Name) || (r.ForeignKeyColumn.Contains(",") && r.ForeignKeyColumn.Contains(columnMigration.CurrentColumn.Name))).FirstOrDefault();
            if (foreignKeyRelation != null)
            {
                alterPrecisionAndScaleScript += GetDropRelationScript(foreignKeyRelation);
            }
            alterPrecisionAndScaleScript += "-- Change Precision And Scale From " + "(" + columnMigration.CurrentColumn.Precision + ", " + columnMigration.CurrentColumn.Scale + ")" + " To " + "(" + columnMigration.NewColumn.Precision + ", " + columnMigration.NewColumn.Scale + ")" + " For Column " + columnMigration.CurrentColumn.Name + "\n";
            alterPrecisionAndScaleScript += "ALTER TABLE " + "[" + TableMigrations.DxmlTableSchema + "].[" + TableMigrations.DxmlTableName + "]" + " ";
            alterPrecisionAndScaleScript += "ALTER COLUMN " + "[" + columnMigration.CurrentColumn.Name + "]" + " ";
            alterPrecisionAndScaleScript += GetDataTypeScript(columnMigration.CurrentColumn.Type, columnMigration.CurrentColumn.Size, columnMigration.NewColumn.Precision, columnMigration.NewColumn.Scale);
            if (!columnMigration.CurrentColumn.Constraints.Nullable)
            {
                alterPrecisionAndScaleScript += " NOT NULL";
            }
            return alterPrecisionAndScaleScript + ";\n\n";
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
                alterPrimaryKeyScript += "-- Add Primary Key Constraint\n";
                string primaryKeyConstraintName = "PK_" + TableMigrations.DxmlTableName + "_" + GenerateRandomString();
                alterPrimaryKeyScript += GetAddPrimaryKeyConstraintScript(primaryKeyConstraintName); 
            }
            return alterPrimaryKeyScript;
        }

        protected override string GetDropPrimaryKeyConstraintScript(string primaryKeyConstraintName)
        {
            string dropPrimaryKeyConstraintScript = "EXEC('IF (OBJECT_ID(''" + TableMigrations.DxmlTableSchema + "." + primaryKeyConstraintName + "'', ''PK'') IS NOT NULL) BEGIN ALTER TABLE " + "[" + TableMigrations.DxmlTableSchema + "].[" + TableMigrations.DxmlTableName + "]" + " DROP CONSTRAINT " + primaryKeyConstraintName + " END" + "');";
            return dropPrimaryKeyConstraintScript;
        }
        
        protected override string GetAddPrimaryKeyConstraintScript(string primaryKeyConstraintName)
        {
            string primaryKeyColumns = string.Join(",", DXMLTable.Columns.Where(c => c.Constraints.PrimaryKey).Select(c => "[" + c.Name + "]").ToArray());
            string addPrimaryKeyConstraintScript = "EXEC('ALTER TABLE " + "[" + TableMigrations.DxmlTableSchema + "].[" + TableMigrations.DxmlTableName + "]" + " ADD CONSTRAINT " + primaryKeyConstraintName + " PRIMARY KEY (" + primaryKeyColumns + ")');";
            return addPrimaryKeyConstraintScript;
        }

        protected override string GetCreateRelationScript(RelationDefinition relation)//relation from DXMLTable
        {
            string createRelationScript = "-- Add Foreign Key Constraint For Column " + relation.ForeignKeyColumn + " In Table " + DXMLTable.Name + " As Reference To Column " + relation.ReferencedColumn + " In Table " + relation.ReferencedTable + "\n";
            string foreignKeyColumns = relation.ForeignKeyColumn.Contains(",") ? string.Join(",", relation.ForeignKeyColumn.Split(',').Select(c => "[" + c + "]").ToArray()) : "[" + relation.ForeignKeyColumn + "]";
            string referencedColumns = relation.ReferencedColumn.Contains(",") ? string.Join(",", relation.ReferencedColumn.Split(',').Select(c => "[" + c + "]").ToArray()) : "[" + relation.ReferencedColumn + "]";
            createRelationScript += "EXEC('ALTER TABLE " + "[" + DXMLTable.Schema + "].[" + DXMLTable.Name + "]" + " ADD FOREIGN KEY(" + foreignKeyColumns + ") REFERENCES " + "[" + relation.ReferencedTableSchema + "]." + "[" + relation.ReferencedTable + "]" + "(" + referencedColumns + ")')";
            return createRelationScript + ";\n\n";
        }
        
        protected override string GetDropRelationScript(RelationDefinition relation)//relation from DBTable
        {
            string dropRelationScript = "-- Drop Foreign Key Constraint For Column " + relation.ForeignKeyColumn + " In Table " + relation.ParentTable + " That Reference To Column " + relation.ReferencedColumn + " In Table " + relation.ReferencedTable + "\n";
            dropRelationScript += "EXEC('IF (OBJECT_ID(''" + relation.ParentTableSchema + "." + relation.ForeignKeyConstraintName + "'', ''F'') IS NOT NULL) BEGIN ALTER TABLE " + "[" + relation.ParentTableSchema + "].[" + relation.ParentTable + "]" + " DROP CONSTRAINT " + relation.ForeignKeyConstraintName + " END" + "')";
            return dropRelationScript + ";\n\n";
        }
    }
}