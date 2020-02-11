using System;
using System.Collections.Generic;
using System.Linq;
using Oracle.DataAccess.Client;

namespace Logitude.DBMigrations.Models
{
    public class OracleDatabaseMigrations : DatabaseMigrations
    {
        public OracleDatabaseMigrations(TableDefinition dxmlTable, string connectionString, List<TableDefinition> dxmlTables, string dxmlFileName)
        {
            ConnectionString = connectionString;
            DXMLTable = FormatCaseSensitiveNames(dxmlTable);
            DXMLTables = dxmlTables;
            DXMLFileName = dxmlFileName;
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
            string queryString = "SELECT \"Q1\".*, \"Q2\".\"ConstraintType\", \"Q2\".\"ConstraintName\" " +
                                 "FROM( " +
                                 "SELECT COL.COLUMN_NAME AS \"ColumnName\", COL.NULLABLE AS \"Nullable\", COL.DATA_TYPE AS \"DataType\", COL.CHAR_LENGTH AS \"Size\", COL.DATA_PRECISION AS \"Precision\", COL.DATA_SCALE AS \"Scale\", COL.DATA_DEFAULT AS \"DefaultValue\" " +
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
            command.InitialLONGFetchSize = -1;

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
                            DefaultValue = (String.IsNullOrEmpty(reader["DefaultValue"].ToString()) || reader["DefaultValue"].ToString() == "NULL") ? null : reader["DefaultValue"].ToString().Trim(),
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

            return FormatCaseSensitiveNames(currentTable);
        }

        protected override List<RelationDefinition> GetRelationsForDBTable(string tableName, bool usingParentTable)
        {
            string tableObject = usingParentTable ? "CONS_P" : "CONS_R";

            string queryString = "SELECT CONS_P.TABLE_NAME AS \"ParentTableName\", COLS_P.COLUMN_NAME AS \"ParentColumnName\", CONS_R.TABLE_NAME AS \"ReferencedTableName\", COLS_R.COLUMN_NAME AS \"ReferencedColumnName\", CONS_P.CONSTRAINT_NAME AS \"ForeignKeyConstraintName\", COLS_R.POSITION AS \"ReferencedColumnOrder\" " +
                                 "FROM USER_CONSTRAINTS CONS_P " +
                                 "LEFT JOIN USER_CONS_COLUMNS COLS_P ON COLS_P.CONSTRAINT_NAME = CONS_P.CONSTRAINT_NAME " +
                                 "LEFT JOIN USER_CONSTRAINTS CONS_R ON CONS_R.CONSTRAINT_NAME = CONS_P.R_CONSTRAINT_NAME " +
                                 "LEFT JOIN USER_CONS_COLUMNS COLS_R ON COLS_R.CONSTRAINT_NAME = CONS_P.R_CONSTRAINT_NAME " +
                                 "WHERE " + tableObject + ".TABLE_NAME = :tableName AND CONS_P.CONSTRAINT_TYPE = 'R' AND COLS_P.POSITION = COLS_R.POSITION";

            List<RelationDefinition> relations = new List<RelationDefinition>();

            OracleDataReader reader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command = new OracleCommand(queryString, connection);
            command.Parameters.Add(new OracleParameter("tableName", tableName.ToUpper()));

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
            string queryString = "SELECT IND_COL.COLUMN_NAME AS \"ColumnName\", IND.INDEX_NAME AS \"IndexName\", IND_COL.COLUMN_POSITION AS \"KeyOrder\" " +
                                 "FROM USER_INDEXES IND " +
                                 "INNER JOIN USER_IND_COLUMNS IND_COL ON IND.INDEX_NAME = IND_COL.INDEX_NAME " +
                                 "WHERE IND.UNIQUENESS = 'NONUNIQUE' AND IND.TABLE_NAME = :tableName";

            List<IndexDefinition> indexes = new List<IndexDefinition>();

            OracleDataReader reader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command = new OracleCommand(queryString, connection);
            command.Parameters.Add(new OracleParameter("tableName", tableName.ToUpper()));

            try
            {
                connection.Open();
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    IndexDefinition index = new IndexDefinition
                    {
                        Columns = reader["ColumnName"].ToString(),
                        IndexName = reader["IndexName"].ToString(),
                        KeyOrder = Convert.ToInt32(reader["KeyOrder"].ToString())
                    };
                    indexes.Add(index);
                }

                reader.Close();
                connection.Close();

                indexes = HandlingCompositeIndexes(indexes);
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
            string queryString = "SELECT CONCOL.COLUMN_NAME AS \"ColumnName\", CON.CONSTRAINT_NAME AS \"ConstraintName\", CONCOL.POSITION AS \"KeyOrder\" FROM USER_CONSTRAINTS CON " +
                                 "INNER JOIN USER_CONS_COLUMNS CONCOL ON CON.CONSTRAINT_NAME = CONCOL.CONSTRAINT_NAME " +
                                 "WHERE CON.CONSTRAINT_TYPE = 'U' AND CON.TABLE_NAME = :tableName";

            List<UniqueConstraintDefinition> uniqueConstraints = new List<UniqueConstraintDefinition>();
            
            OracleDataReader reader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command = new OracleCommand(queryString, connection);
            command.Parameters.Add(new OracleParameter("tableName", tableName.ToUpper()));

            try
            {
                connection.Open();
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    UniqueConstraintDefinition uniqueConstraint = new UniqueConstraintDefinition
                    {
                        Columns = reader["ColumnName"].ToString(),
                        ConstraintName = reader["ConstraintName"].ToString(),
                        KeyOrder = Convert.ToInt32(reader["KeyOrder"].ToString())
                    };
                    uniqueConstraints.Add(uniqueConstraint);
                }

                reader.Close();
                connection.Close();

                uniqueConstraints = HandlingCompositeUniqueConstraints(uniqueConstraints);
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
            string tableName = FormatNameLength(DXMLTable.Name, DXMLTable.ShortName).ToUpper();

            string createTableScript = "-- Create New Table With Name " + tableName + "\n";
            createTableScript += "CREATE TABLE \"" + tableName + "\"(" + "\n";
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

            string createTableWithHistoryScript = createTableScript + GetInsertScriptForMigrationsHistory("Create Table", tableName, null, createTableScript);

            return createTableWithHistoryScript;
        }

        protected override string GetCreateColumnScript(ColumnDefinition columnDefinition)
        {
            string columnScript = "\"" + FormatNameLength(columnDefinition.Name, columnDefinition.ShortName).ToUpper() + "\"" + " ";
            columnScript += GetDataTypeScript(columnDefinition.Type, columnDefinition.Size, columnDefinition.Precision, columnDefinition.Scale);
            columnScript += ((columnDefinition.Identity && columnDefinition.Constraints.PrimaryKey) ? " GENERATED BY DEFAULT ON NULL AS IDENTITY" : null);
            columnScript += GetDefaultValueScript(columnDefinition.Constraints.Nullable, columnDefinition.Type, columnDefinition.DefaultValue);
            columnScript += (columnDefinition.Constraints.Nullable ? " NULL" : " NOT NULL");
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

        protected override bool IsTableRenamed()
        {
            return TableMigrations.CurrentTableName != FormatNameLength(TableMigrations.DxmlTableName, TableMigrations.DxmlTableShortName);
        }

        protected override bool IsColumnRenamed(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            return currentTableColumn.Name != FormatNameLength(dxmlTableColumn.Name, dxmlTableColumn.ShortName);
        }

        protected override int FormatColumnSize(int size, string type)
        {
            return size >= 2000 ? 2000 : size;
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
                    return size == -1 ? "CLOB" : "VARCHAR2(" + FormatColumnSize(size, type).ToString() + " CHAR)";
                case "nvarchar":
                    return size == -1 ? "NCLOB" : "NVARCHAR2(" + FormatColumnSize(size, type).ToString() + ")";
                case "timestamp":
                    return "RAW(8)";
                case "char":
                    return "CHAR(" + (size == -1 ? "2000 CHAR" : FormatColumnSize(size, type).ToString() + " CHAR") + ")";
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

            string relationForeignKeyColumn = !relation.ForeignKeyColumn.Contains(",") ? FormatNameLength(relation.ForeignKeyColumn, DXMLTable.Columns.Where(c => c.Name == relation.ForeignKeyColumn).First().ShortName) : string.Join(",", relation.ForeignKeyColumn.Split(',').Select(fc => FormatNameLength(fc, DXMLTable.Columns.Where(c => c.Name == fc).First().ShortName)).ToArray()).ToLower();
            string relationReferencedColumn = !relation.ReferencedColumn.Contains(",") ? FormatNameLength(relation.ReferencedColumn, DXMLTables.Where(t => t.Name.ToLower() == relation.ReferencedTable).First().Columns.Where(c => c.Name.ToLower() == relation.ReferencedColumn).First().ShortName) : string.Join(",", relation.ReferencedColumn.Split(',').Select(rc => FormatNameLength(rc, DXMLTables.Where(t => t.Name.ToLower() == relation.ReferencedTable).First().Columns.Where(c => c.Name.ToLower() == rc).First().ShortName)).ToArray()).ToLower();
            string relationReferencedTable = FormatNameLength(relation.ReferencedTable, DXMLTables.Where(t => t.Name.ToLower() == relation.ReferencedTable).First().ShortName).ToLower();

            bool isRelationInCurrentTable = CurrentTable.Relations.Where(r => r.ForeignKeyColumn == relationForeignKeyColumn && r.ReferencedTable == relationReferencedTable && r.ReferencedColumn == relationReferencedColumn).Any();

            return (!isForeignKeyDataTypeChanged && isRelationInCurrentTable);
        }

        protected override bool IsRelationInDXMLTable(RelationDefinition relation)//db relation//for drop
        {
            return DXMLTable.Relations.Where(r => (!r.ForeignKeyColumn.Contains(",") ? FormatNameLength(r.ForeignKeyColumn, DXMLTable.Columns.Where(c => c.Name == r.ForeignKeyColumn).First().ShortName) : string.Join(",", r.ForeignKeyColumn.Split(',').Select(fc => FormatNameLength(fc, DXMLTable.Columns.Where(c => c.Name == fc).First().ShortName)).ToArray())) == relation.ForeignKeyColumn && FormatNameLength(r.ReferencedTable, DXMLTables.Where(t => t.Name.ToLower() == r.ReferencedTable).First().ShortName).ToLower() == relation.ReferencedTable && (!r.ReferencedColumn.Contains(",") ? FormatNameLength(r.ReferencedColumn, DXMLTables.Where(t => t.Name.ToLower() == r.ReferencedTable).First().Columns.Where(c => c.Name.ToLower() == r.ReferencedColumn).First().ShortName).ToLower() : string.Join(",", r.ReferencedColumn.Split(',').Select(rc => FormatNameLength(rc, DXMLTables.Where(t => t.Name.ToLower() == r.ReferencedTable).First().Columns.Where(c => c.Name.ToLower() == rc).First().ShortName).ToLower()).ToArray())) == relation.ReferencedColumn).Any();
        }

        protected override RelationDefinition GetRelationFromDXMLTable(RelationDefinition relation)
        {
            return DXMLTable.Relations.Where(r => (!r.ForeignKeyColumn.Contains(",") ? FormatNameLength(r.ForeignKeyColumn, DXMLTable.Columns.Where(c => c.Name == r.ForeignKeyColumn).First().ShortName) : string.Join(",", r.ForeignKeyColumn.Split(',').Select(fc => FormatNameLength(fc, DXMLTable.Columns.Where(c => c.Name == fc).First().ShortName)).ToArray())) == relation.ForeignKeyColumn && FormatNameLength(r.ReferencedTable, DXMLTables.Where(t => t.Name.ToLower() == r.ReferencedTable).First().ShortName).ToLower() == relation.ReferencedTable && (!r.ReferencedColumn.Contains(",") ? FormatNameLength(r.ReferencedColumn, DXMLTables.Where(t => t.Name.ToLower() == r.ReferencedTable).First().Columns.Where(c => c.Name.ToLower() == r.ReferencedColumn).First().ShortName).ToLower() : string.Join(",", r.ReferencedColumn.Split(',').Select(rc => FormatNameLength(rc, DXMLTables.Where(t => t.Name.ToLower() == r.ReferencedTable).First().Columns.Where(c => c.Name.ToLower() == rc).First().ShortName).ToLower()).ToArray())) == relation.ReferencedColumn).First();
        }

        protected override bool IsIndexInCurrentTable(IndexDefinition index)
        {
            string indexColumns = !index.Columns.Contains(",") ? FormatNameLength(index.Columns, DXMLTable.Columns.Where(c => c.Name == index.Columns).First().ShortName) : string.Join(",", index.Columns.Split(',').Select(ic => FormatNameLength(ic, DXMLTable.Columns.Where(c => c.Name == ic).First().ShortName)).ToArray()).ToLower();
            return CurrentTable.Indexes.Where(i => i.Columns == indexColumns).Any();
        }

        protected override bool IsIndexInDXMLTable(IndexDefinition index)
        {
            return DXMLTable.Indexes.Where(i => (!i.Columns.Contains(",") ? FormatNameLength(i.Columns, DXMLTable.Columns.Where(c => c.Name == i.Columns).First().ShortName) : string.Join(",", i.Columns.Split(',').Select(ic => FormatNameLength(ic, DXMLTable.Columns.Where(c => c.Name == ic).First().ShortName)).ToArray())) == index.Columns).Any();
        }

        protected override bool IsUniqueConstraintInCurrentTable(UniqueConstraintDefinition uniqueConstraint)
        {
            string uniqueConstraintColumns = !uniqueConstraint.Columns.Contains(",") ? FormatNameLength(uniqueConstraint.Columns, DXMLTable.Columns.Where(c => c.Name == uniqueConstraint.Columns).First().ShortName) : string.Join(",", uniqueConstraint.Columns.Split(',').Select(uc => FormatNameLength(uc, DXMLTable.Columns.Where(c => c.Name == uc).First().ShortName)).ToArray()).ToLower();
            return CurrentTable.UniqueConstraints.Where(u => u.Columns == uniqueConstraintColumns).Any();
        }

        protected override bool IsUniqueConstraintInDXMLTable(UniqueConstraintDefinition uniqueConstraint)
        {
            return DXMLTable.UniqueConstraints.Where(u => (!u.Columns.Contains(",") ? FormatNameLength(u.Columns, DXMLTable.Columns.Where(c => c.Name == u.Columns).First().ShortName) : string.Join(",", u.Columns.Split(',').Select(uc => FormatNameLength(uc, DXMLTable.Columns.Where(c => c.Name == uc).First().ShortName)).ToArray())) == uniqueConstraint.Columns).Any();
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

            if (!isColumnInCurrentTable && dxmlColumnShortName != null && dxmlColumnName.Length > 30)
            {
                isColumnInCurrentTable = CurrentTable.Columns.Where(c => c.Name == dxmlColumnShortName).Any();
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

            if (!isColumnInCurrentTable && dxmlColumnShortName != null && dxmlColumnName.Length > 30 && CurrentTable.Columns.Where(c => c.Name == dxmlColumnShortName).Any())
            {
                columnName = dxmlColumnShortName;
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
            List<string> dxmlTableColumnsShortNames = DXMLTable.Columns.Where(c => c.ShortName != null).Select(c => c.ShortName).ToList();
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

            dxmlTableColumnsNames = dxmlTableColumnsNames.Concat(dxmlTableColumnsShortNames).ToList();

            List<ColumnDefinition> droppedColumns = CurrentTable.Columns.Where(c => !dxmlTableColumnsNames.Contains(c.Name) && !c.Name.StartsWith("drop_")).ToList();
            return droppedColumns;
        }

        protected override string GetRenameTableScript()
        {
            string renameTableScript = "";
            string dropRelationsScript = "";

            if (IsTableRenamed())
            {
                List<RelationDefinition> relations = GetRelationsForDBTable(CurrentTable.Name, false);
                foreach (var relation in relations)
                {
                    dropRelationsScript += GetDropRelationScript(relation);
                }

                string newTableName = FormatNameLength(TableMigrations.DxmlTableName, TableMigrations.DxmlTableShortName).ToUpper();

                renameTableScript += "-- Rename Table From " + TableMigrations.CurrentTableName.ToUpper() + " To " + newTableName + "\n";
                renameTableScript += "RENAME \"" + TableMigrations.CurrentTableName.ToUpper() + "\" TO \"" + newTableName + "\"";
                renameTableScript += ";\n\n";

                string renameTableWithHistoryScript = dropRelationsScript + renameTableScript + GetInsertScriptForMigrationsHistory("Rename Table", TableMigrations.CurrentTableName.ToUpper(), null, renameTableScript);

                return renameTableWithHistoryScript;
            }

            return null;
        }

        protected override string GetAddColumnScript(ColumnMigration columnMigration)
        {
            string tableName = FormatNameLength(TableMigrations.DxmlTableName, TableMigrations.DxmlTableShortName).ToUpper();
            string columnName = FormatNameLength(columnMigration.NewColumn.Name, columnMigration.NewColumn.ShortName).ToUpper();

            string addScript = "-- Add New Column With Name " + columnName + "\n";
            addScript += "ALTER TABLE " + "\"" + tableName + "\"" + " ";
            addScript += "ADD " + "\"" + columnName + "\"" + " ";
            addScript += GetDataTypeScript(columnMigration.NewColumn.Type, columnMigration.NewColumn.Size, columnMigration.NewColumn.Precision, columnMigration.NewColumn.Scale);
            addScript += GetDefaultValueScript(columnMigration.NewColumn.Constraints.Nullable, columnMigration.NewColumn.Type, columnMigration.NewColumn.DefaultValue);
            addScript += columnMigration.NewColumn.Constraints.Nullable ? " NULL" : " NOT NULL";
            addScript += ";\n\n";

            string addWithHistoryScript = addScript + GetInsertScriptForMigrationsHistory("Add Column", tableName, columnName, addScript);

            return addWithHistoryScript;
        }

        protected override string GetRenameColumnScript(ColumnMigration columnMigration)
        {
            string tableName = FormatNameLength(TableMigrations.DxmlTableName, TableMigrations.DxmlTableShortName).ToUpper();
            string newColumnName = FormatNameLength(columnMigration.NewColumn.Name, columnMigration.NewColumn.ShortName).ToUpper();

            string renameScript = "-- Rename Column From " + columnMigration.CurrentColumn.Name.ToUpper() + " To " + newColumnName + "\n";
            renameScript += "ALTER TABLE \"" + tableName + "\" RENAME COLUMN \"" + columnMigration.CurrentColumn.Name.ToUpper() + "\" TO \"" + newColumnName + "\"";
            renameScript += ";\n\n";

            string renameWithHistoryScript = renameScript + GetInsertScriptForMigrationsHistory("Rename Column", tableName, columnMigration.CurrentColumn.Name.ToUpper(), renameScript);

            return renameWithHistoryScript;
        }

        protected override string GetDropColumnScript(ColumnMigration columnMigration)
        {
            string tableName = FormatNameLength(TableMigrations.DxmlTableName, TableMigrations.DxmlTableShortName).ToUpper();

            string dropScript = "-- Drop Column " + columnMigration.CurrentColumn.Name.ToUpper() + "\n";
            dropScript += "ALTER TABLE \"" + tableName + "\" RENAME COLUMN \"" + columnMigration.CurrentColumn.Name.ToUpper() + "\" TO \"" + FormatNameLength("Drop_" + columnMigration.CurrentColumn.Name, null).ToUpper() + "\"";
            dropScript += ";\n\n";

            string dropWithHistoryScript = dropScript + GetInsertScriptForMigrationsHistory("Drop Column", tableName, columnMigration.CurrentColumn.Name.ToUpper(), dropScript);

            return dropWithHistoryScript;
        }

        protected override string GetAlterTypeScript(ColumnMigration columnMigration)
        {
            string alterTypeScript = "";
            string dropRelationsScript = "";

            List<RelationDefinition> relations = CurrentTable.Relations;
            RelationDefinition foreignKeyRelation = relations.Where(r => (!r.ForeignKeyColumn.Contains(",") && r.ForeignKeyColumn == columnMigration.CurrentColumn.Name) || (r.ForeignKeyColumn.Contains(",") && r.ForeignKeyColumn.Contains(columnMigration.CurrentColumn.Name))).FirstOrDefault();
            if (foreignKeyRelation != null)
            {
                dropRelationsScript += GetDropRelationScript(foreignKeyRelation);
            }

            string tableName = FormatNameLength(TableMigrations.DxmlTableName, TableMigrations.DxmlTableShortName).ToUpper();

            alterTypeScript += "-- Change Type From " + columnMigration.CurrentColumn.Type + " To " + columnMigration.NewColumn.Type + " For Column " + columnMigration.CurrentColumn.Name.ToUpper() + "\n";
            alterTypeScript += "ALTER TABLE " + "\"" + tableName + "\"" + " ";
            alterTypeScript += "MODIFY " + "\"" + columnMigration.CurrentColumn.Name.ToUpper() + "\"" + " ";
            alterTypeScript += GetDataTypeScript(columnMigration.NewColumn.Type, (columnMigration.CurrentColumn.Size == 0 ? 1 : columnMigration.CurrentColumn.Size), columnMigration.NewColumn.Precision, columnMigration.NewColumn.Scale);
            alterTypeScript += ";\n\n";

            string alterTypeWithHistoryScript = dropRelationsScript + alterTypeScript + GetInsertScriptForMigrationsHistory("Alter Column Type", tableName, columnMigration.CurrentColumn.Name.ToUpper(), alterTypeScript);

            return alterTypeWithHistoryScript;
        }

        protected override string GetAlterSizeScript(ColumnMigration columnMigration)
        {
            string alterSizeScript = "";
            string dropRelationsScript = "";

            List<RelationDefinition> relations = CurrentTable.Relations;
            RelationDefinition foreignKeyRelation = relations.Where(r => (!r.ForeignKeyColumn.Contains(",") && r.ForeignKeyColumn == columnMigration.CurrentColumn.Name) || (r.ForeignKeyColumn.Contains(",") && r.ForeignKeyColumn.Contains(columnMigration.CurrentColumn.Name))).FirstOrDefault();
            if (foreignKeyRelation != null)
            {
                dropRelationsScript += GetDropRelationScript(foreignKeyRelation);
            }

            string tableName = FormatNameLength(TableMigrations.DxmlTableName, TableMigrations.DxmlTableShortName).ToUpper();

            bool IsAlterTypeInMigrationsList = TableMigrations.ColumnsMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERTYPE).Any();
            alterSizeScript += "-- Change Size From " + columnMigration.CurrentColumn.Size + " To " + columnMigration.NewColumn.Size + " For Column " + columnMigration.CurrentColumn.Name.ToUpper() + "\n";
            alterSizeScript += "ALTER TABLE " + "\"" + tableName + "\"" + " ";
            alterSizeScript += "MODIFY " + "\"" + columnMigration.CurrentColumn.Name.ToUpper() + "\"" + " ";
            alterSizeScript += GetDataTypeScript((IsAlterTypeInMigrationsList ? columnMigration.NewColumn.Type : columnMigration.CurrentColumn.Type), columnMigration.NewColumn.Size, 0, 0);
            alterSizeScript += ";\n\n";

            string alterSizeWithHistoryScript = dropRelationsScript + alterSizeScript + GetInsertScriptForMigrationsHistory("Alter Column Size", tableName, columnMigration.CurrentColumn.Name.ToUpper(), alterSizeScript);

            return alterSizeWithHistoryScript;
        }

        protected override string GetAddPrimaryKeyScript(ColumnMigration columnMigration)
        {
            List<string> droppedColumnsNames = GetDroppedColumns().Select(c => c.Name).ToList();
            if (CurrentTable.Columns.Where(c => c.Constraints.PrimaryKey && !droppedColumnsNames.Contains(c.Name)).Any())
            {
                string tableName = FormatNameLength(TableMigrations.DxmlTableName, TableMigrations.DxmlTableShortName).ToUpper();
                string primaryKeyColumns = string.Join(",", CurrentTable.Columns.Where(c => c.Constraints.PrimaryKey && !droppedColumnsNames.Contains(c.Name)).Select(c => "\"" + c.Name.ToUpper() + "\"").ToArray());
                string primaryKeyConstraintName = columnMigration.CurrentColumn.Constraints.PrimaryKeyConstraintName;
                string addPrimaryKeyScript = "-- Add Primary Key Constraint\n";
                addPrimaryKeyScript += "ALTER TABLE \"" + tableName + "\" ADD CONSTRAINT \"" + primaryKeyConstraintName + "\" PRIMARY KEY (" + primaryKeyColumns + ")";
                addPrimaryKeyScript += ";\n\n";

                string addPrimaryKeyWithHistoryScript = addPrimaryKeyScript + GetInsertScriptForMigrationsHistory("Add Primary Key", tableName, primaryKeyColumns.Replace("\"", String.Empty), addPrimaryKeyScript);

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

            string tableName = FormatNameLength(TableMigrations.DxmlTableName, TableMigrations.DxmlTableShortName).ToUpper();
            dropPrimaryKeyScript += "-- Drop Primary Key Constraint\n";
            string primaryKeyConstraintName = columnMigration.CurrentColumn.Constraints.PrimaryKeyConstraintName;
            dropPrimaryKeyScript += "DECLARE ConstraintCount NUMBER; BEGIN SELECT COUNT(*) INTO ConstraintCount FROM USER_CONSTRAINTS WHERE CONSTRAINT_NAME = '" + primaryKeyConstraintName + "'; IF (ConstraintCount <> 0) THEN EXECUTE IMMEDIATE 'ALTER TABLE \"" + tableName + "\" DROP CONSTRAINT \"" + primaryKeyConstraintName + "\"'; END IF; END";
            dropPrimaryKeyScript += ";\n\n";

            string dropPrimaryKeyWithHistoryScript = dropRelationsScript + dropPrimaryKeyScript + GetInsertScriptForMigrationsHistory("Drop Primary Key", tableName, null, dropPrimaryKeyScript);

            return dropPrimaryKeyWithHistoryScript;
        }
        
        protected override string GetSetNullableScript(ColumnMigration columnMigration)
        {
            string tableName = FormatNameLength(TableMigrations.DxmlTableName, TableMigrations.DxmlTableShortName).ToUpper();

            bool isAlterTypeInMigrationsList = TableMigrations.ColumnsMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERTYPE).Any();
            bool isAlterSizeInMigrationsList = TableMigrations.ColumnsMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERSIZE).Any();
            bool isAlterPrecisionAndScaleInMigrationsList = TableMigrations.ColumnsMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERPRECISIONANDSCALE).Any();
            string setNullableScript = "-- Set Nullable For Column " + columnMigration.CurrentColumn.Name.ToUpper() + "\n";
            setNullableScript += "ALTER TABLE " + "\"" + tableName + "\"" + " ";
            setNullableScript += "MODIFY " + "\"" + columnMigration.CurrentColumn.Name.ToUpper() + "\"" + " ";
            setNullableScript += GetDataTypeScript((isAlterTypeInMigrationsList ? columnMigration.NewColumn.Type : columnMigration.CurrentColumn.Type), (isAlterSizeInMigrationsList ? columnMigration.NewColumn.Size : columnMigration.CurrentColumn.Size), (isAlterPrecisionAndScaleInMigrationsList ? columnMigration.NewColumn.Precision : columnMigration.CurrentColumn.Precision), (isAlterPrecisionAndScaleInMigrationsList ? columnMigration.NewColumn.Scale : columnMigration.CurrentColumn.Scale));
            setNullableScript += " NULL";
            setNullableScript += ";\n\n";

            string setNullableWithHistoryScript = setNullableScript + GetInsertScriptForMigrationsHistory("Set Column Nullable", tableName, columnMigration.CurrentColumn.Name.ToUpper(), setNullableScript);

            return setNullableWithHistoryScript;
        }

        protected override string GetUnsetNullableScript(ColumnMigration columnMigration)
        {
            string tableName = FormatNameLength(TableMigrations.DxmlTableName, TableMigrations.DxmlTableShortName).ToUpper();

            bool isAlterTypeInMigrationsList = TableMigrations.ColumnsMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERTYPE).Any();
            bool isAlterSizeInMigrationsList = TableMigrations.ColumnsMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERSIZE).Any();
            bool isAlterPrecisionAndScaleInMigrationsList = TableMigrations.ColumnsMigrations.Where(m => m.MigrationType == MigrationTypes.ALTERPRECISIONANDSCALE).Any();
            string unsetNullableScript = "-- Unset Nullable For Column " + columnMigration.CurrentColumn.Name.ToUpper() + "\n";
            unsetNullableScript += "ALTER TABLE " + "\"" + tableName + "\"" + " ";
            unsetNullableScript += "MODIFY " + "\"" + columnMigration.CurrentColumn.Name.ToUpper() + "\"" + " ";
            unsetNullableScript += GetDataTypeScript((isAlterTypeInMigrationsList ? columnMigration.NewColumn.Type : columnMigration.CurrentColumn.Type), (isAlterSizeInMigrationsList ? columnMigration.NewColumn.Size : columnMigration.CurrentColumn.Size), (isAlterPrecisionAndScaleInMigrationsList ? columnMigration.NewColumn.Precision : columnMigration.CurrentColumn.Precision), (isAlterPrecisionAndScaleInMigrationsList ? columnMigration.NewColumn.Scale : columnMigration.CurrentColumn.Scale));
            unsetNullableScript += " NOT NULL";
            unsetNullableScript += ";\n\n";

            string unsetNullableWithHistoryScript = unsetNullableScript + GetInsertScriptForMigrationsHistory("Unset Column Nullable", tableName, columnMigration.CurrentColumn.Name.ToUpper(), unsetNullableScript);

            return unsetNullableWithHistoryScript;
        }

        protected override string GetAlterPrecisionAndScaleScript(ColumnMigration columnMigration)
        {
            string alterPrecisionAndScaleScript = "";
            string dropRelationsScript = "";

            List<RelationDefinition> relations = CurrentTable.Relations;
            RelationDefinition foreignKeyRelation = relations.Where(r => (!r.ForeignKeyColumn.Contains(",") && r.ForeignKeyColumn == columnMigration.CurrentColumn.Name) || (r.ForeignKeyColumn.Contains(",") && r.ForeignKeyColumn.Contains(columnMigration.CurrentColumn.Name))).FirstOrDefault();
            if (foreignKeyRelation != null)
            {
                dropRelationsScript += GetDropRelationScript(foreignKeyRelation);
            }

            string tableName = FormatNameLength(TableMigrations.DxmlTableName, TableMigrations.DxmlTableShortName).ToUpper();

            alterPrecisionAndScaleScript += "-- Change Precision And Scale From " + "(" + columnMigration.CurrentColumn.Precision + ", " + columnMigration.CurrentColumn.Scale + ")" + " To " + "(" + columnMigration.NewColumn.Precision + ", " + columnMigration.NewColumn.Scale + ")" + " For Column " + columnMigration.CurrentColumn.Name.ToUpper() + "\n";
            alterPrecisionAndScaleScript += "ALTER TABLE " + "\"" + tableName + "\"" + " ";
            alterPrecisionAndScaleScript += "MODIFY " + "\"" + columnMigration.CurrentColumn.Name.ToUpper() + "\"" + " ";
            alterPrecisionAndScaleScript += GetDataTypeScript(columnMigration.CurrentColumn.Type, columnMigration.CurrentColumn.Size, columnMigration.NewColumn.Precision, columnMigration.NewColumn.Scale);
            alterPrecisionAndScaleScript += ";\n\n";

            string alterPrecisionAndScaleWithHistoryScript = dropRelationsScript + alterPrecisionAndScaleScript + GetInsertScriptForMigrationsHistory("Alter Column Precision And Scale", tableName, columnMigration.CurrentColumn.Name.ToUpper(), alterPrecisionAndScaleScript);

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
                string primaryKeyConstraintName = "PK_" + FormatNameLength(TableMigrations.DxmlTableName, TableMigrations.DxmlTableShortName).ToUpper() + "_" + GenerateRandomString().ToUpper();
                primaryKeyConstraintName = FormatNameLength(primaryKeyConstraintName, null);
                alterPrimaryKeyScript += GetAddPrimaryKeyConstraintScript(primaryKeyConstraintName);
            }
            return alterPrimaryKeyScript;
        }

        protected override string GetDropPrimaryKeyConstraintScript(string primaryKeyConstraintName)
        {
            string tableName = FormatNameLength(TableMigrations.DxmlTableName, TableMigrations.DxmlTableShortName).ToUpper();

            string dropRelationsScript = "";

            List<RelationDefinition> relations = GetRelationsForDBTable(CurrentTable.Name, false);
            foreach (var relation in relations)
            {
                dropRelationsScript += GetDropRelationScript(relation);
            }

            string dropPrimaryKeyScript = "-- Drop Primary Key Constraint\n";
            dropPrimaryKeyScript += "DECLARE ConstraintCount NUMBER; BEGIN SELECT COUNT(*) INTO ConstraintCount FROM USER_CONSTRAINTS WHERE CONSTRAINT_NAME = '" + primaryKeyConstraintName + "'; IF (ConstraintCount <> 0) THEN EXECUTE IMMEDIATE 'ALTER TABLE \"" + tableName + "\" DROP CONSTRAINT \"" + primaryKeyConstraintName + "\"'; END IF; END;";
            return dropRelationsScript + dropPrimaryKeyScript + "\n\n" + GetInsertScriptForMigrationsHistory("Drop Primary Key Constraint", tableName, null, dropPrimaryKeyScript);
        }
        
        protected override string GetAddPrimaryKeyConstraintScript(string primaryKeyConstraintName)
        {
            string tableName = FormatNameLength(TableMigrations.DxmlTableName, TableMigrations.DxmlTableShortName).ToUpper();
            string addPrimaryKeyConstraintScript = "-- Add Primary Key Constraint\n";
            string primaryKeyColumns = string.Join(",", DXMLTable.Columns.Where(c => c.Constraints.PrimaryKey).Select(c => "\"" + FormatNameLength(c.Name, c.ShortName).ToUpper() + "\"").ToArray());
            addPrimaryKeyConstraintScript += "ALTER TABLE \"" + tableName + "\" ADD CONSTRAINT \"" + primaryKeyConstraintName + "\" PRIMARY KEY (" + primaryKeyColumns + ");";
            return addPrimaryKeyConstraintScript + "\n\n" + GetInsertScriptForMigrationsHistory("Add Primary Key Constraint", tableName, primaryKeyColumns.Replace("\"", String.Empty), addPrimaryKeyConstraintScript);
        }

        protected override string GetCreateRelationScript(RelationDefinition relation)
        {
            string parentTable = FormatNameLength(DXMLTable.Name, DXMLTable.ShortName).ToUpper();
            string referencedTable = FormatNameLength(relation.ReferencedTable, DXMLTables.Where(t => t.Name.ToLower() == relation.ReferencedTable).First().ShortName).ToUpper();
            string foreignKeyColumns = relation.ForeignKeyColumn.Contains(",") ? string.Join(",", relation.ForeignKeyColumn.Split(',').Select(c => "\"" + FormatNameLength(c, DXMLTable.Columns.Where(fc => fc.Name == c).First().ShortName).ToUpper() + "\"").ToArray()) : "\"" + FormatNameLength(relation.ForeignKeyColumn, DXMLTable.Columns.Where(c => c.Name == relation.ForeignKeyColumn).First().ShortName).ToUpper() + "\"";
            string referencedColumns = relation.ReferencedColumn.Contains(",") ? string.Join(",", relation.ReferencedColumn.Split(',').Select(c => "\"" + FormatNameLength(c, DXMLTables.Where(t => t.Name.ToLower() == relation.ReferencedTable).First().Columns.Where(rc => rc.Name.ToLower() == c).First().ShortName).ToUpper() + "\"").ToArray()) : "\"" + FormatNameLength(relation.ReferencedColumn, DXMLTables.Where(t => t.Name.ToLower() == relation.ReferencedTable).First().Columns.Where(c => c.Name.ToLower() == relation.ReferencedColumn).First().ShortName).ToUpper() + "\"";
            string createRelationScript = "-- Add Foreign Key Constraint For Column " + foreignKeyColumns.Replace("\"", String.Empty) + " In Table " + parentTable + " As Reference To Column " + referencedColumns.Replace("\"", String.Empty) + " In Table " + referencedTable + "\n";
            createRelationScript += "ALTER TABLE " + "\"" + parentTable + "\"" + " ADD FOREIGN KEY(" + foreignKeyColumns + ") REFERENCES " + "\"" + referencedTable + "\"" + "(" + referencedColumns + ")";
            createRelationScript += ";\n\n";

            string createRelationWithHistoryScript = createRelationScript + GetInsertScriptForMigrationsHistory("Create Relation", parentTable, foreignKeyColumns.Replace("\"", String.Empty), createRelationScript);

            IndexDefinition relationIndex = new IndexDefinition
            {
                Columns = relation.ForeignKeyColumn
            };

            string createIndexScript = GetCreateIndexScript(relationIndex);

            return createRelationWithHistoryScript + createIndexScript;
        }

        protected override string GetDropRelationScript(RelationDefinition relation)
        {
            string dropRelationScript = "-- Drop Foreign Key Constraint For Column " + relation.ForeignKeyColumn.ToUpper() + " In Table " + relation.ParentTable.ToUpper() + " That Reference To Column " + relation.ReferencedColumn.ToUpper() + " In Table " + relation.ReferencedTable.ToUpper() + "\n";
            dropRelationScript += "DECLARE ConstraintCount NUMBER; BEGIN SELECT COUNT(*) INTO ConstraintCount FROM USER_CONSTRAINTS WHERE CONSTRAINT_NAME = '" + relation.ForeignKeyConstraintName.ToUpper() + "'; IF (ConstraintCount <> 0) THEN EXECUTE IMMEDIATE 'ALTER TABLE \"" + relation.ParentTable.ToUpper() + "\" DROP CONSTRAINT \"" + relation.ForeignKeyConstraintName.ToUpper() + "\"'; END IF; END";
            dropRelationScript += ";\n\n";

            string dropRelationWithHistoryScript = dropRelationScript + GetInsertScriptForMigrationsHistory("Drop Relation", relation.ParentTable.ToUpper(), null, dropRelationScript);

            string dropIndexScript = null;

            if (relation.ParentTable.ToLower() == CurrentTable.Name.ToLower())
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
                string insertScript = "DECLARE ScriptText NCLOB; BEGIN ScriptText := '" + script.Replace("'", "''").TrimEnd(new char[] { '\r', '\n' }) + "'; INSERT INTO \"DBMIGRATIONSHISTORY\"(\"DXMLFILENAME\", \"TABLENAME\", \"COLUMNNAME\", \"MIGRATIONTYPE\", \"EXECUTIONDATE\", \"MIGRATIONSCRIPT\")VALUES('" + DXMLFileName + "', '" + tableName + "', " + (columnName == null ? "NULL" : "'" + columnName + "'") + ", '" + migrationType + "', SYSDATE, ScriptText); END;" + "\n\n";
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

            if (type == "bit" && String.IsNullOrEmpty(defaultValue))
            {
                return " DEFAULT 0";
            }

            if (!String.IsNullOrEmpty(defaultValue))
            {
                if (defaultValue.ToLower() == "CurrentDate".ToLower())
                {
                    return " DEFAULT SYSDATE";
                }

                return " DEFAULT " + defaultValue;
            }

            return null;
        }

        protected override string GetAddDefaultScript(ColumnMigration columnMigration)
        {
            string tableName = FormatNameLength(TableMigrations.DxmlTableName, TableMigrations.DxmlTableShortName).ToUpper();

            string addDefaultScript = "-- Add Default Value For Column " + columnMigration.CurrentColumn.Name.ToUpper() + "\n";
            addDefaultScript += "ALTER TABLE " + "\"" + tableName + "\"" + " MODIFY " + "\"" + columnMigration.CurrentColumn.Name.ToUpper() + "\"" + " DEFAULT " + (columnMigration.NewColumn.DefaultValue.ToLower() == "CurrentDate".ToLower() ? "SYSDATE" : columnMigration.NewColumn.DefaultValue);
            addDefaultScript += ";\n\n";

            string addDefaultWithHistoryScript = addDefaultScript + GetInsertScriptForMigrationsHistory("Add Default Value", tableName, columnMigration.CurrentColumn.Name.ToUpper(), addDefaultScript);

            return addDefaultWithHistoryScript;
        }

        protected override string GetDropDefaultScript(ColumnMigration columnMigration)
        {
            string tableName = FormatNameLength(TableMigrations.DxmlTableName, TableMigrations.DxmlTableShortName).ToUpper();

            string dropDefaultScript = "-- Drop Default Value For Column " + columnMigration.CurrentColumn.Name.ToUpper() + "\n";
            dropDefaultScript += "ALTER TABLE " + "\"" + tableName + "\"" + " MODIFY " + "\"" + columnMigration.CurrentColumn.Name.ToUpper() + "\"" + " DEFAULT NULL";
            dropDefaultScript += ";\n\n";

            string dropDefaultWithHistoryScript = dropDefaultScript + GetInsertScriptForMigrationsHistory("Drop Default Value", tableName, columnMigration.CurrentColumn.Name.ToUpper(), dropDefaultScript);

            return dropDefaultWithHistoryScript;
        }


        protected override string GetCreateIndexScript(IndexDefinition index)
        {
            string tableName = FormatNameLength(DXMLTable.Name, DXMLTable.ShortName).ToUpper();
            string indexColumns = (!index.Columns.Contains(",") ? "\"" + index.Columns + "\"" : string.Join(",", index.Columns.Split(',').Select(c => "\"" + c + "\"").ToArray())).ToUpper();
            string createIndexScript = "-- Create Index On " + tableName + " Table\n";
            string indexName = FormatNameLength("IX_" + tableName + "_" + (!indexColumns.Contains(",") ? indexColumns : string.Join("_", indexColumns.Split(',').ToArray())).Replace("\"", String.Empty), null).ToUpper();

            createIndexScript += "CREATE INDEX " + "\"" + indexName + "\"" + " ON " + "\"" + tableName + "\"" + "(" + indexColumns + ")";
            createIndexScript += ";\n\n";

            string addIndexWithHistoryScript = createIndexScript + GetInsertScriptForMigrationsHistory("Create Index", tableName, indexColumns.Replace("\"", String.Empty), createIndexScript);

            return addIndexWithHistoryScript;
        }

        protected override string GetCreateUniqueConstraintScript(UniqueConstraintDefinition uniqueConstraint)
        {
            string tableName = FormatNameLength(DXMLTable.Name, DXMLTable.ShortName).ToUpper();
            string uniqueConstraintColumns = (!uniqueConstraint.Columns.Contains(",") ? "\"" + uniqueConstraint.Columns + "\"" : string.Join(",", uniqueConstraint.Columns.Split(',').Select(c => "\"" + c + "\"").ToArray())).ToUpper();
            string createUniqueConstraintScript = "-- Create Unique Constraint On " + tableName + " Table\n";
            string uniqueConstraintName = FormatNameLength("UQ_" + tableName + "_" + (!uniqueConstraintColumns.Contains(",") ? uniqueConstraintColumns : string.Join("_", uniqueConstraintColumns.Split(',').ToArray())).Replace("\"", String.Empty), null).ToUpper();

            createUniqueConstraintScript += "ALTER TABLE \"" + tableName + "\" ADD CONSTRAINT \"" + uniqueConstraintName + "\" UNIQUE(" + uniqueConstraintColumns + ")";
            createUniqueConstraintScript += ";\n\n";

            string createUniqueConstraintWithHistoryScript = createUniqueConstraintScript + GetInsertScriptForMigrationsHistory("Create Unique Constraint", tableName, uniqueConstraintColumns.Replace("\"", String.Empty), createUniqueConstraintScript);

            return createUniqueConstraintWithHistoryScript;
        }

        protected override string GetDropUniqueConstraintScript(UniqueConstraintDefinition uniqueConstraint)
        {
            string tableName = FormatNameLength(DXMLTable.Name, DXMLTable.ShortName).ToUpper();
            string dropUniqueConstraintScript = "-- Drop Unique Constraint " + uniqueConstraint.ConstraintName + " From Table " + tableName + "\n";
            dropUniqueConstraintScript += "DECLARE ConstraintCount NUMBER; BEGIN SELECT COUNT(*) INTO ConstraintCount FROM USER_CONSTRAINTS WHERE CONSTRAINT_NAME = '" + uniqueConstraint.ConstraintName + "'; IF (ConstraintCount <> 0) THEN EXECUTE IMMEDIATE 'ALTER TABLE \"" + tableName + "\" DROP CONSTRAINT \"" + uniqueConstraint.ConstraintName + "\"'; END IF; END";
            dropUniqueConstraintScript += ";\n\n";

            string dropUniqueConstraintWithHistoryScript = dropUniqueConstraintScript + GetInsertScriptForMigrationsHistory("Drop Unique Constraint", tableName, null, dropUniqueConstraintScript);

            return dropUniqueConstraintWithHistoryScript;
        }

        protected override string GetDropIndexScript(IndexDefinition index)
        {
            string tableName = FormatNameLength(DXMLTable.Name, DXMLTable.ShortName).ToUpper();
            string dropIndexScript = "-- Drop Index " + index.IndexName + " From Table " + tableName + "\n";
            dropIndexScript += "DECLARE IndexCount NUMBER; BEGIN SELECT COUNT(*) INTO IndexCount FROM USER_INDEXES WHERE INDEX_NAME = '" + index.IndexName + "'; IF (IndexCount <> 0) THEN EXECUTE IMMEDIATE 'DROP INDEX \"" + index.IndexName + "\"'; END IF; END";
            dropIndexScript += ";\n\n";

            string dropIndexWithHistoryScript = dropIndexScript + GetInsertScriptForMigrationsHistory("Drop Index", tableName, null, dropIndexScript);

            return dropIndexWithHistoryScript;
        }
    }
}