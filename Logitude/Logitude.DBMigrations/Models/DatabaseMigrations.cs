using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Logitude.DBMigrations.Models
{
    public abstract class DatabaseMigrations
    {
        protected TableDefinition DXMLTable;
        protected TableDefinition CurrentTable;
        protected TableMigrations TableMigrations;

        protected List<TableDefinition> DXMLTables;
        protected List<ColumnMigration> ColumnsMigrations = new List<ColumnMigration>();

        protected string ConnectionString;
        protected bool AlterPrimaryKeyConstraint = false;
        protected bool PrimaryKeyColumnAdded = false;

        protected string DXMLFileName;

        protected string MissingIndexesWarnings = "";
        
        public string GetScript()
        {
            CurrentTable = GetCurrentTableDefinitionFromDB();
            string script;
            if (CurrentTable == null)
            {
                script = GetCreateTableScript();
            }
            else
            {
                script = GetAlterTableScript();
            }
            return script;
        }

        public string GetRelationsScript()
        {
            string tableRelationsScript = "";

            if(CurrentTable == null)
            {
                foreach (var relation in DXMLTable.Relations)
                {
                    if (!relation.Ignore)
                    {
                        tableRelationsScript += GetCreateRelationScript(relation);
                    }
                }
            }
            else
            {
                foreach (var relation in CurrentTable.Relations)
                {
                    if (!IsRelationInDXMLTable(relation))
                    {
                        tableRelationsScript += GetDropRelationScript(relation);
                    }
                    else
                    {
                        RelationDefinition relationFromDxml = GetRelationFromDXMLTable(relation);
                        if (relationFromDxml.Ignore)
                        {
                            tableRelationsScript += GetDropRelationScript(relation);
                        }
                    }
                }

                foreach (var relation in DXMLTable.Relations)
                {
                    if (!IsRelationInCurrentTable(relation))
                    {
                        if (!relation.Ignore)
                        {
                            tableRelationsScript += GetCreateRelationScript(relation);
                        }
                    }
                }
            }

            return tableRelationsScript;
        }

        public string GetIndexesScript()
        {
            string tableIndexesScript = "";

            if (CurrentTable == null)
            {
                foreach (var index in DXMLTable.Indexes)
                {
                    tableIndexesScript += GetCreateIndexScript(index);
                }
            }
            else
            {
                foreach (var index in CurrentTable.Indexes)
                {
                    if (!IsIndexInDXMLTable(index))
                    {
                        MissingIndexesWarnings += "Warning: Missing Index In DXML File " + DXMLFileName + ", The Found Index On DB Is " + index.IndexName + ", The Index Should Added To The DXML File\n";
                    }
                }

                foreach (var index in DXMLTable.Indexes)
                {
                    if (!IsIndexInCurrentTable(index))
                    {
                        tableIndexesScript += GetCreateIndexScript(index);
                    }
                }
            }

            return tableIndexesScript;
        }

        public string GetMissingIndexesWarnings()
        {
            return MissingIndexesWarnings;
        }

        public string GetUniqueConstraintsScript()
        {
            string tableUniqueConstraintsScript = "";
            
            if (CurrentTable == null)
            {
                foreach (var uniqueConstraint in DXMLTable.UniqueConstraints)
                {
                    tableUniqueConstraintsScript += GetCreateUniqueConstraintScript(uniqueConstraint);
                }
            }
            else
            {
                foreach (var uniqueConstraint in CurrentTable.UniqueConstraints)
                {
                    if (!IsUniqueConstraintInDXMLTable(uniqueConstraint))
                    {
                        tableUniqueConstraintsScript += GetDropUniqueConstraintScript(uniqueConstraint);
                    }
                }

                foreach (var uniqueConstraint in DXMLTable.UniqueConstraints)
                {
                    if (!IsUniqueConstraintInCurrentTable(uniqueConstraint))
                    {
                        tableUniqueConstraintsScript += GetCreateUniqueConstraintScript(uniqueConstraint);
                    }
                }
            }

            return tableUniqueConstraintsScript;
        }

        protected string GetColumnDefinitionDataType(string dataType)
        {
            string formattedDataType = FormatDataTypeString(dataType).ToLower();
            switch (formattedDataType)
            {
                case "int":
                    return "int";
                case "decimal":
                    return "decimal";
                case "timestamp":
                    return "timestamp";
                case "varbinary":
                    return "varbinary";
                case "varchar":
                    return "varchar";
                case "datetime":
                    return "datetime";
                case "date":
                    return "date";
                case "time":
                    return "time";
                case "float":
                    return "float";
                case "char":
                    return "char";
                case "bigint":
                    return "bigint";
                case "nvarchar":
                    return "nvarchar";
                case "bit":
                    return "bit";
                default:
                    return null;
            }
        }

        protected string GetColumnDefinitionDataType(string dataType, string precision, string scale)
        {
            string formattedDataType = FormatDataTypeString(dataType).ToLower();
            switch (formattedDataType)
            {
                case "date":
                    return "date";
                case "timestamp":
                    return "datetime";
                case "interval day to second":
                    return "time";
                case "blob":
                    return "varbinary";
                case "clob":
                case "varchar2":
                    return "varchar";
                case "nclob":
                case "nvarchar2":
                    return "nvarchar";
                case "raw":
                    return "timestamp";
                case "char":
                    return "char";
                case "number":
                    if(String.IsNullOrEmpty(precision) && String.IsNullOrEmpty(scale))
                    {
                        return "float";
                    }
                    else if(Convert.ToInt32(precision) == 1 && Convert.ToInt32(scale) == 0)
                    {
                        return "bit";
                    }
                    else if(Convert.ToInt32(precision) == 10 && Convert.ToInt32(scale) == 0)
                    {
                        return "int";
                    }
                    else if(Convert.ToInt32(precision) == 18 && Convert.ToInt32(scale) == 0)
                    {
                        return "bigint";
                    }
                    else
                    {
                        return "decimal";
                    }
                default:
                    return null;
            }
        }

        protected int GetColumnDefinitionSize(string size)
        {
            if (String.IsNullOrEmpty(size))
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(size);
            }
        }

        protected int GetColumnDefinitionSize(string dataType, string size)
        {
            string formattedDataType = FormatDataTypeString(dataType).ToLower();
            if (new string[] { "blob", "nclob", "clob" }.Contains(formattedDataType))
            {
                return -1;
            }
            else
            {
                if(formattedDataType == "char" && size == "2000")
                {
                    return -1;
                }
                return Convert.ToInt32(size);
            }
        }

        protected string FormatDataTypeString(string dataType)
        {
            Regex regex = new Regex(@"\((.*?)\)");
            string formattedDataType = regex.Replace(dataType, String.Empty);
            return formattedDataType;
        }

        protected ColumnDefinition SetConstraintForColumnDefinition(ColumnDefinition column, string constraintType, string constraintName)
        {
            switch (constraintType)
            {
                case "P":
                case "PRIMARY KEY":
                    column.Constraints.PrimaryKey = true;
                    column.Constraints.PrimaryKeyConstraintName = constraintName;
                    return column;
                default:
                    return column;
            }
        }

        protected TableMigrations GetTableMigrations()
        {
            BuildAddAndAlterMigrations();

            BuildDropMigrations();

            TableMigrations tableMigrations = CreateTableMigrations();

            return tableMigrations;
        }

        private TableMigrations CreateTableMigrations()
        {
            TableMigrations tableMigrations = new TableMigrations
            {
                DxmlTableName = DXMLTable.Name,
                DxmlTableShortName = DXMLTable.ShortName,
                DxmlTableSchema = DXMLTable.Schema,
                CurrentTableName = CurrentTable.Name,
                ColumnsMigrations = ColumnsMigrations
            };

            return tableMigrations;
        }

        private void BuildDropMigrations()
        {
            List<ColumnDefinition> droppedColumns = GetDroppedColumns();

            foreach (var droppedColumn in droppedColumns)
            {
                ColumnDefinition currentTableColumn = GetCurrentTableColumn(droppedColumn.Name, null, null);
                BuildColumnMigrations(currentTableColumn, null);
            }
        }

        private void BuildAddAndAlterMigrations()
        {
            foreach (var dxmlTableColumn in DXMLTable.Columns)
            {
                ColumnDefinition currentTableColumn = null;
                if (IsColumnInCurrentTable(dxmlTableColumn.Name, dxmlTableColumn.ShortName, dxmlTableColumn.OldNames))
                {
                    currentTableColumn = GetCurrentTableColumn(dxmlTableColumn.Name, dxmlTableColumn.ShortName, dxmlTableColumn.OldNames);
                }

                BuildColumnMigrations(currentTableColumn, dxmlTableColumn);
            }
        }

        protected ColumnMigration GetColumnMigration(int migrationType, ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            ColumnMigrationDefinition currentColumn = currentTableColumn == null ? null : new ColumnMigrationDefinition
            {
                Name = currentTableColumn.Name,
                ShortName = currentTableColumn.ShortName,
                Type = currentTableColumn.Type,
                Size = currentTableColumn.Size,
                Precision = currentTableColumn.Precision,
                Scale = currentTableColumn.Scale,
                DefaultValue = currentTableColumn.DefaultValue,
                Constraints = currentTableColumn.Constraints
            };
            ColumnMigrationDefinition newColumn = dxmlTableColumn == null ? null : new ColumnMigrationDefinition
            {
                Name = dxmlTableColumn.Name,
                ShortName = dxmlTableColumn.ShortName,
                Type = dxmlTableColumn.Type,
                Size = dxmlTableColumn.Size,
                Precision = dxmlTableColumn.Precision,
                Scale = dxmlTableColumn.Scale,
                DefaultValue = dxmlTableColumn.DefaultValue,
                Constraints = dxmlTableColumn.Constraints
            };
            ColumnMigration columnMigration = new ColumnMigration
            {
                MigrationType = migrationType,
                CurrentColumn = currentColumn,
                NewColumn = newColumn
            };
            return columnMigration;
        }

        protected bool IsNotInCurrentTable(ColumnDefinition currentTableColumn)
        {
            return currentTableColumn == null;
        }

        protected bool IsNotInDXMLTable(ColumnDefinition dxmlTableColumn)
        {
            return dxmlTableColumn == null;
        }

        protected void BuildColumnMigrations(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            if (IsNotInCurrentTable(currentTableColumn))
            {
                BuildAddColumnMigration(currentTableColumn, dxmlTableColumn);
            }
            else if (IsNotInDXMLTable(dxmlTableColumn))
            {
                BuildDropColumnMigration(currentTableColumn, dxmlTableColumn);
            }
            else
            {
                BuildAlterMigration(currentTableColumn, dxmlTableColumn);
            }
        }

        protected void BuildAddColumnMigration(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            ColumnMigration addMigration = GetColumnMigration(MigrationTypes.ADD, currentTableColumn, dxmlTableColumn);
            ColumnsMigrations.Add(addMigration);
            if (dxmlTableColumn.Constraints.PrimaryKey)
            {
                PrimaryKeyColumnAdded = true;
            }
        }

        protected void BuildDropColumnMigration(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            if (currentTableColumn.Constraints.PrimaryKey)
            {
                ColumnMigration dropPrimaryKeyMigration = GetColumnMigration(MigrationTypes.DROPPRIMARYKEY, currentTableColumn, dxmlTableColumn);
                ColumnsMigrations.Add(dropPrimaryKeyMigration);
            }

            ColumnMigration dropMigration = GetColumnMigration(MigrationTypes.DROP, currentTableColumn, dxmlTableColumn);
            ColumnsMigrations.Add(dropMigration);

            if (currentTableColumn.Constraints.PrimaryKey)
            {
                ColumnMigration addPrimaryKeyMigration = GetColumnMigration(MigrationTypes.ADDPRIMARYKEY, currentTableColumn, dxmlTableColumn);
                ColumnsMigrations.Add(addPrimaryKeyMigration);
            }
        }

        protected void BuildAlterMigration(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            BuildAlterPrecisionAndScaleMigration(currentTableColumn, dxmlTableColumn);

            BuildAlterTypeMigration(currentTableColumn, dxmlTableColumn);

            BuildAlterSizeMigration(currentTableColumn, dxmlTableColumn);

            BuildAlterDefaultMigration(currentTableColumn, dxmlTableColumn);

            BuildUnsetNullableMigration(currentTableColumn, dxmlTableColumn);

            BuildSetNullableMigration(currentTableColumn, dxmlTableColumn);

            BuildRenameMigration(currentTableColumn, dxmlTableColumn);

            BuildAlterPrimaryKeyMigration(currentTableColumn, dxmlTableColumn);
        }

        protected void BuildAlterDefaultMigration(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            currentTableColumn.DefaultValue = GetCurrentColumnDefaultValue(currentTableColumn);
            dxmlTableColumn.DefaultValue = GetDxmlColumnDefaultValue(dxmlTableColumn);

            if (currentTableColumn.DefaultValue != dxmlTableColumn.DefaultValue)
            {
                if (currentTableColumn.DefaultValue == null)
                {
                    ColumnMigration addDefaultMigration = GetColumnMigration(MigrationTypes.ADDDEFAULT, currentTableColumn, dxmlTableColumn);
                    ColumnsMigrations.Add(addDefaultMigration);
                }
                else if (dxmlTableColumn.DefaultValue == null)
                {
                    ColumnMigration dropDefaultMigration = GetColumnMigration(MigrationTypes.DROPDEFAULT, currentTableColumn, dxmlTableColumn);
                    ColumnsMigrations.Add(dropDefaultMigration);
                }
                else
                {
                    ColumnMigration dropDefaultMigration = GetColumnMigration(MigrationTypes.DROPDEFAULT, currentTableColumn, dxmlTableColumn);
                    ColumnsMigrations.Add(dropDefaultMigration);

                    ColumnMigration addDefaultMigration = GetColumnMigration(MigrationTypes.ADDDEFAULT, currentTableColumn, dxmlTableColumn);
                    ColumnsMigrations.Add(addDefaultMigration);
                }
            }
        }

        protected string GetCurrentColumnDefaultValue(ColumnDefinition currentTableColumn)
        {
            if (String.IsNullOrEmpty(currentTableColumn.DefaultValue))
            {
                return null;
            }
            if (currentTableColumn.DefaultValue.ToLower().Contains("getdate()") || currentTableColumn.DefaultValue.ToLower().Contains("sysdate"))
            {
                return "CurrentDate".ToLower();
            }
            if (currentTableColumn.DefaultValue.Contains("'"))
            {
                return "'" + currentTableColumn.DefaultValue.Split('\'')[1] + "'";
            }
            if (currentTableColumn.DefaultValue.ToLower().Contains(".nextval") && currentTableColumn.Constraints.PrimaryKey)
            {
                return null;
            }

            return currentTableColumn.DefaultValue.Replace("(", String.Empty).Replace(")", String.Empty);
        }

        protected string GetDxmlColumnDefaultValue(ColumnDefinition dxmlTableColumn)
        {
            if (!dxmlTableColumn.Constraints.Nullable && String.IsNullOrEmpty(dxmlTableColumn.DefaultValue) && dxmlTableColumn.Type == "bit")
            {
                return "0";
            }
            if (String.IsNullOrEmpty(dxmlTableColumn.DefaultValue))
            {
                return null;
            }
            if (dxmlTableColumn.DefaultValue.ToLower() == "CurrentDate".ToLower())
            {
                return "CurrentDate".ToLower();
            }

            return dxmlTableColumn.DefaultValue;
        }

        protected void BuildAlterPrecisionAndScaleMigration(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            if (currentTableColumn.Type == "decimal" && dxmlTableColumn.Type == "decimal" && (currentTableColumn.Precision != dxmlTableColumn.Precision || currentTableColumn.Scale != dxmlTableColumn.Scale))
            {
                if (currentTableColumn.Constraints.PrimaryKey)
                {
                    ColumnMigration dropPrimaryKeyMigration = GetColumnMigration(MigrationTypes.DROPPRIMARYKEY, currentTableColumn, dxmlTableColumn);
                    ColumnsMigrations.Add(dropPrimaryKeyMigration);
                }

                ColumnMigration alterPrecisionAndScaleMigration = GetColumnMigration(MigrationTypes.ALTERPRECISIONANDSCALE, currentTableColumn, dxmlTableColumn);
                ColumnsMigrations.Add(alterPrecisionAndScaleMigration);

                if (currentTableColumn.Constraints.PrimaryKey)
                {
                    ColumnMigration addPrimaryKeyMigration = GetColumnMigration(MigrationTypes.ADDPRIMARYKEY, currentTableColumn, dxmlTableColumn);
                    ColumnsMigrations.Add(addPrimaryKeyMigration);
                }
            }
        }

        protected void BuildAlterTypeMigration(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            if (currentTableColumn.Type != dxmlTableColumn.Type)
            {
                if (currentTableColumn.Constraints.PrimaryKey)
                {
                    ColumnMigration dropPrimaryKeyMigration = GetColumnMigration(MigrationTypes.DROPPRIMARYKEY, currentTableColumn, dxmlTableColumn);
                    ColumnsMigrations.Add(dropPrimaryKeyMigration);
                }

                ColumnMigration alterTypeMigration = GetColumnMigration(MigrationTypes.ALTERTYPE, currentTableColumn, dxmlTableColumn);
                ColumnsMigrations.Add(alterTypeMigration);

                if (currentTableColumn.Constraints.PrimaryKey)
                {
                    ColumnMigration addPrimaryKeyMigration = GetColumnMigration(MigrationTypes.ADDPRIMARYKEY, currentTableColumn, dxmlTableColumn);
                    ColumnsMigrations.Add(addPrimaryKeyMigration);
                }
            }
        }

        protected void BuildAlterSizeMigration(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            if (currentTableColumn.Size != FormatColumnSize(dxmlTableColumn.Size, dxmlTableColumn.Type) && dxmlTableColumn.Size != 0)
            {
                if (currentTableColumn.Constraints.PrimaryKey)
                {
                    ColumnMigration dropPrimaryKeyMigration = GetColumnMigration(MigrationTypes.DROPPRIMARYKEY, currentTableColumn, dxmlTableColumn);
                    ColumnsMigrations.Add(dropPrimaryKeyMigration);
                }

                ColumnMigration alterTypeMigration = GetColumnMigration(MigrationTypes.ALTERSIZE, currentTableColumn, dxmlTableColumn);
                ColumnsMigrations.Add(alterTypeMigration);

                if (currentTableColumn.Constraints.PrimaryKey)
                {
                    ColumnMigration addPrimaryKeyMigration = GetColumnMigration(MigrationTypes.ADDPRIMARYKEY, currentTableColumn, dxmlTableColumn);
                    ColumnsMigrations.Add(addPrimaryKeyMigration);
                }
            }
        }
        
        protected void BuildAlterPrimaryKeyMigration(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            if (currentTableColumn.Constraints.PrimaryKey != dxmlTableColumn.Constraints.PrimaryKey)
            {
                AlterPrimaryKeyConstraint = true;
            }
        }

        protected void BuildUnsetNullableMigration(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            if (currentTableColumn.Constraints.Nullable && !dxmlTableColumn.Constraints.Nullable)
            {
                ColumnMigration unsetNullableMigration = GetColumnMigration(MigrationTypes.UNSETNULLABLE, currentTableColumn, dxmlTableColumn);
                ColumnsMigrations.Add(unsetNullableMigration);
            }
        }

        protected void BuildSetNullableMigration(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            if (!currentTableColumn.Constraints.Nullable && dxmlTableColumn.Constraints.Nullable)
            {
                ColumnMigration setNullableMigration = GetColumnMigration(MigrationTypes.SETNULLABLE, currentTableColumn, dxmlTableColumn);
                ColumnsMigrations.Add(setNullableMigration);
            }
        }

        protected void BuildRenameMigration(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn)
        {
            if (IsColumnRenamed(currentTableColumn, dxmlTableColumn))
            {
                ColumnMigration renameMigration = GetColumnMigration(MigrationTypes.RENAME, currentTableColumn, dxmlTableColumn);
                ColumnsMigrations.Add(renameMigration);
            }
        }

        protected string GenerateRandomString()
        {
            return Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "").ToUpper();
        }

        protected string FormatNameLength(string name, string shortName)
        {
            int maxLength = 30;

            if(name.Length <= maxLength)
            {
                return name;
            }
            else
            {
                if(!String.IsNullOrEmpty(shortName))
                {
                    return shortName;
                }
                else
                {
                    if (name.ToLower().StartsWith("drop_") || name.ToLower().StartsWith("pk_") || name.ToLower().StartsWith("ix_") || name.ToLower().StartsWith("uq_"))
                    {
                        return name.Substring(0, maxLength);
                    }
                    else
                    {
                        return name;
                    }
                }
            }
        }

        protected TableDefinition FormatCaseSensitiveNames(TableDefinition table)
        {
            if(table != null)
            {
                table.Name = table.Name.ToLower();
                table.ShortName = String.IsNullOrEmpty(table.ShortName) ? null : table.ShortName.ToLower();
                table.OldNames = String.IsNullOrEmpty(table.OldNames) ? null : table.OldNames.ToLower();

                foreach (var column in table.Columns)
                {
                    column.Name = column.Name.ToLower();
                    column.ShortName = String.IsNullOrEmpty(column.ShortName) ? null : column.ShortName.ToLower();
                    column.OldNames = String.IsNullOrEmpty(column.OldNames) ? null : column.OldNames.ToLower();
                }

                foreach (var relation in table.Relations)
                {
                    relation.ForeignKeyColumn = relation.ForeignKeyColumn.ToLower();
                    relation.ParentTable = String.IsNullOrEmpty(relation.ParentTable) ? null : relation.ParentTable.ToLower();
                    relation.ReferencedTable = relation.ReferencedTable.ToLower();
                    relation.ReferencedColumn = relation.ReferencedColumn.ToLower();
                    relation.ForeignKeyConstraintName = String.IsNullOrEmpty(relation.ForeignKeyConstraintName) ? null : relation.ForeignKeyConstraintName.ToLower();
                }

                foreach(var index in table.Indexes)
                {
                    index.Columns = index.Columns.ToLower();
                    index.Include = String.IsNullOrEmpty(index.Include) ? null : index.Include.ToLower();
                }

                foreach (var uniqueConstraint in table.UniqueConstraints)
                {
                    uniqueConstraint.Columns = uniqueConstraint.Columns.ToLower();
                }

                return table;
            }
            else
            {
                return null;
            }
        }

        protected List<RelationDefinition> HandlingCompositeRelations(List<RelationDefinition> relations)
        {
            List<string> processedConstraints = new List<string>();
            List<RelationDefinition> processedRelations = new List<RelationDefinition>();

            foreach (var relation in relations)
            {
                if (!processedConstraints.Contains(relation.ForeignKeyConstraintName))
                {
                    string foreignKeyColumn;
                    string parentTable;
                    string referencedTable;
                    string referencedColumn;
                    string foreignKeyConstraintName;
                    string parentTableSchema;
                    string referencedTableSchema;

                    List<RelationDefinition> relationsWithSameConstraint = relations.Where(r => r.ForeignKeyConstraintName == relation.ForeignKeyConstraintName).OrderBy(r => r.ReferencedColumnOrder).ToList();

                    if (relationsWithSameConstraint.Count() > 1)
                    {
                        foreignKeyColumn = string.Join(",", relationsWithSameConstraint.Select(r => r.ForeignKeyColumn).ToArray());
                        parentTable = relationsWithSameConstraint.First().ParentTable;
                        referencedTable = relationsWithSameConstraint.First().ReferencedTable;
                        referencedColumn = string.Join(",", relationsWithSameConstraint.Select(r => r.ReferencedColumn).ToArray());
                        foreignKeyConstraintName = relationsWithSameConstraint.First().ForeignKeyConstraintName;
                        parentTableSchema = relationsWithSameConstraint.First().ParentTableSchema;
                        referencedTableSchema = relationsWithSameConstraint.First().ReferencedTableSchema;
                    }
                    else
                    {
                        foreignKeyColumn = relation.ForeignKeyColumn;
                        parentTable = relation.ParentTable;
                        referencedTable = relation.ReferencedTable;
                        referencedColumn = relation.ReferencedColumn;
                        foreignKeyConstraintName = relation.ForeignKeyConstraintName;
                        parentTableSchema = relation.ParentTableSchema;
                        referencedTableSchema = relation.ReferencedTableSchema;
                    }

                    RelationDefinition processedRelation = new RelationDefinition
                    {
                        ForeignKeyColumn = foreignKeyColumn,
                        ParentTable = parentTable,
                        ReferencedTable = referencedTable,
                        ReferencedColumn = referencedColumn,
                        ForeignKeyConstraintName = foreignKeyConstraintName,
                        ParentTableSchema = parentTableSchema,
                        ReferencedTableSchema = referencedTableSchema
                    };

                    processedRelations.Add(processedRelation);
                    processedConstraints.Add(relation.ForeignKeyConstraintName);
                }
            }

            return processedRelations;
        }

        protected List<IndexDefinition> HandlingCompositeIndexes(List<IndexDefinition> indexes)
        {
            List<string> processedIndexesNames = new List<string>();
            List<IndexDefinition> processedIndexes = new List<IndexDefinition>();

            foreach (var index in indexes)
            {
                if (!processedIndexesNames.Contains(index.IndexName))
                {
                    string columns;

                    List<IndexDefinition> indexesWithSameName = indexes.Where(i => i.IndexName == index.IndexName).OrderBy(i => i.KeyOrder).ToList();

                    if (indexesWithSameName.Count() > 1)
                    {
                        columns = string.Join(",", indexesWithSameName.Select(i => i.Columns).ToArray());
                    }
                    else
                    {
                        columns = index.Columns;
                    }

                    IndexDefinition processedIndex = new IndexDefinition
                    {
                        Columns = columns,
                        IndexName = index.IndexName
                    };

                    processedIndexes.Add(processedIndex);
                    processedIndexesNames.Add(index.IndexName);
                }
            }

            return processedIndexes;
        }

        protected List<UniqueConstraintDefinition> HandlingCompositeUniqueConstraints(List<UniqueConstraintDefinition> uniqueConstraints)
        {
            List<string> processedConstraintsNames = new List<string>();
            List<UniqueConstraintDefinition> processedConstraints = new List<UniqueConstraintDefinition>();
            
            foreach (var uniqueConstraint in uniqueConstraints)
            {
                if (!processedConstraintsNames.Contains(uniqueConstraint.ConstraintName))
                {
                    string columns;

                    List<UniqueConstraintDefinition> uniqueConstraintsWithSameName = uniqueConstraints.Where(u => u.ConstraintName == uniqueConstraint.ConstraintName).OrderBy(i => i.KeyOrder).ToList();

                    if (uniqueConstraintsWithSameName.Count() > 1)
                    {
                        columns = string.Join(",", uniqueConstraintsWithSameName.Select(u => u.Columns).ToArray());
                    }
                    else
                    {
                        columns = uniqueConstraint.Columns;
                    }

                    UniqueConstraintDefinition processedConstraint = new UniqueConstraintDefinition
                    {
                        Columns = columns,
                        ConstraintName = uniqueConstraint.ConstraintName
                    };

                    processedConstraints.Add(processedConstraint);
                    processedConstraintsNames.Add(uniqueConstraint.ConstraintName);
                }
            }

            return processedConstraints;
        }

        protected string GetAlterTableScript()
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

        protected string GetAlterColumnsScript()
        {
            string alterColumnsScript = "";
            foreach (var columnMigration in TableMigrations.ColumnsMigrations)
            {
                alterColumnsScript += GetAlterColumnScript(columnMigration);
            }
            return alterColumnsScript;
        }

        protected string GetAlterColumnScript(ColumnMigration columnMigration)
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
                case MigrationTypes.ADDDEFAULT:
                    alterColumnScript = GetAddDefaultScript(columnMigration);
                    return alterColumnScript;
                case MigrationTypes.DROPDEFAULT:
                    alterColumnScript = GetDropDefaultScript(columnMigration);
                    return alterColumnScript;
                default:
                    return alterColumnScript;
            }
        }

        protected string GetAlterPrimaryKeyScript()
        {
            string alterPrimaryKeyScript = "-- Alter The Primary Key Constraint\n";
            ColumnDefinition columnHasPrimaryKey = CurrentTable.Columns.Where(c => c.Constraints.PrimaryKey).First();
            string primaryKeyConstraintName = columnHasPrimaryKey.Constraints.PrimaryKeyConstraintName;
            alterPrimaryKeyScript += GetDropPrimaryKeyConstraintScript(primaryKeyConstraintName);
            if (IsTableHasPrimaryKeys(DXMLTable))
            {
                alterPrimaryKeyScript += "\n\n";
                alterPrimaryKeyScript += GetAddPrimaryKeyConstraintScript(primaryKeyConstraintName);
            }
            return alterPrimaryKeyScript;
        }

        protected bool IsTableHasPrimaryKeys(TableDefinition table)
        {
            return table.Columns.Where(c => c.Constraints.PrimaryKey).Any();
        }

        protected void ExitDatabaseMigrations(string message)
        {
            Console.WriteLine(message);
            Environment.Exit(0);
        }


        protected abstract TableDefinition GetCurrentTableDefinitionFromDB();

        protected abstract TableDefinition GetCurrentTableDefinitionFromDB(string tableName);

        protected abstract List<RelationDefinition> GetRelationsForDBTable(string tableName, bool usingParentTable);

        protected abstract List<IndexDefinition> GetIndexesForDBTable(string tableName);

        protected abstract List<UniqueConstraintDefinition> GetUniqueConstraintsForDBTable(string tableName);

        protected abstract string GetCreateRelationScript(RelationDefinition relation);

        protected abstract string GetDropRelationScript(RelationDefinition relation);

        protected abstract string GetCreateTableScript();

        protected abstract string GetCreateColumnScript(ColumnDefinition columnDefinition);

        protected abstract string GetDataTypeScript(string type, int size, int precision, int scale);

        protected abstract string GetRenameTableScript();

        protected abstract string GetAddColumnScript(ColumnMigration columnMigration);

        protected abstract string GetRenameColumnScript(ColumnMigration columnMigration);

        protected abstract string GetDropColumnScript(ColumnMigration columnMigration);

        protected abstract string GetAlterTypeScript(ColumnMigration columnMigration);

        protected abstract string GetAlterSizeScript(ColumnMigration columnMigration);

        protected abstract string GetAddPrimaryKeyScript(ColumnMigration columnMigration);

        protected abstract string GetDropPrimaryKeyScript(ColumnMigration columnMigration);

        protected abstract string GetSetNullableScript(ColumnMigration columnMigration);

        protected abstract string GetUnsetNullableScript(ColumnMigration columnMigration);

        protected abstract string GetAlterPrecisionAndScaleScript(ColumnMigration columnMigration);

        protected abstract string GetAddDefaultScript(ColumnMigration columnMigration);

        protected abstract string GetDropDefaultScript(ColumnMigration columnMigration);

        protected abstract string GetPrimaryKeyConstraintScript();

        protected abstract string GetDropPrimaryKeyConstraintScript(string primaryKeyConstraintName);

        protected abstract string GetAddPrimaryKeyConstraintScript(string primaryKeyConstraintName);

        protected abstract bool IsTableRenamed();

        protected abstract bool IsColumnInCurrentTable(string dxmlColumnName, string dxmlColumnShortName, string dxmlColumnOldNames);

        protected abstract ColumnDefinition GetCurrentTableColumn(string dxmlColumnName, string dxmlColumnShortName, string dxmlColumnOldNames);

        protected abstract List<ColumnDefinition> GetDroppedColumns();

        protected abstract bool IsColumnRenamed(ColumnDefinition currentTableColumn, ColumnDefinition dxmlTableColumn);

        protected abstract int FormatColumnSize(int size, string type);

        protected abstract bool IsRelationInCurrentTable(RelationDefinition relation);

        protected abstract bool IsRelationInDXMLTable(RelationDefinition relation);

        protected abstract bool IsIndexInCurrentTable(IndexDefinition index);

        protected abstract bool IsIndexInDXMLTable(IndexDefinition index);

        protected abstract bool IsUniqueConstraintInCurrentTable(UniqueConstraintDefinition uniqueConstraint);

        protected abstract bool IsUniqueConstraintInDXMLTable(UniqueConstraintDefinition uniqueConstraint);

        protected abstract RelationDefinition GetRelationFromDXMLTable(RelationDefinition relation);

        protected abstract string GetInsertScriptForMigrationsHistory(string migrationType, string tableName, string columnName, string script);

        protected abstract string GetDefaultValueScript(bool nullable, string type, string defaultValue);
        
        protected abstract string GetCreateIndexScript(IndexDefinition index);

        protected abstract string GetDropIndexScript(IndexDefinition index);

        protected abstract string GetCreateUniqueConstraintScript(UniqueConstraintDefinition uniqueConstraint);

        protected abstract string GetDropUniqueConstraintScript(UniqueConstraintDefinition uniqueConstraint);
    }
}