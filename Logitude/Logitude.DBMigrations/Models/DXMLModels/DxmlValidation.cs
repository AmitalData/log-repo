using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Logitude.DBMigrations.Models
{
    public class DXMLValidation
    {
        protected string[] DXMLFiles;
        protected List<DXMLTable> DXMLTables;
        protected List<DXMLView> DXMLViews;
        protected List<DXMLProcedure> DXMLProcedures;
        protected List<DXMLTrigger> DXMLTriggers;
        
        public DXMLValidation(string[] dxmlFiles)
        {
            DXMLFiles = dxmlFiles;
            DXMLDefinitions dxmlDefinitions = GetDXMLDefinitions();
            DXMLTables = dxmlDefinitions.DXMLTables;
            DXMLViews = dxmlDefinitions.DXMLViews;
            DXMLProcedures = dxmlDefinitions.DXMLProcedures;
            DXMLTriggers = dxmlDefinitions.DXMLTriggers;
        }

        public void Validate()
        {
            string error;

            error = ValidateDXMLFilesNames();
            if (!String.IsNullOrEmpty(error))
            {
                ExitTool(error);
            }

            error = ValidateDXMLViewsNames();
            if (!String.IsNullOrEmpty(error))
            {
                ExitTool(error);
            }

            error = ValidateDXMLProceduresNames();
            if (!String.IsNullOrEmpty(error))
            {
                ExitTool(error);
            }

            error = ValidateDXMLTriggersNames();
            if (!String.IsNullOrEmpty(error))
            {
                ExitTool(error);
            }

            foreach (var dxmlTable in DXMLTables)
            {
                error = ValidateReferencedTables(dxmlTable);
                if (error != null) break;
                error = ValidateReferencedColumns(dxmlTable);
                if (error != null) break;
                error = ValidateNumberOfReferencedColumns(dxmlTable);
                if (error != null) break;
                error = ValidateForeignKeyColumns(dxmlTable);
                if (error != null) break;
                error = ValidateNumberOfForeignKeyColumns(dxmlTable);
                if (error != null) break;
                error = ValidateForeignKeyColumnsDataType(dxmlTable);
                if (error != null) break;
                error = ValidatePrimaryKeys(dxmlTable);
                if (error != null) break;
                error = ValidateIndexesColumns(dxmlTable);
                if (error != null) break;
                error = ValidateUniqueConstraintsColumns(dxmlTable);
                if (error != null) break;
                error = ValidateDuplicateIndexes(dxmlTable);
                if (error != null) break;
                error = ValidateDuplicateUniqueConstraints(dxmlTable);
                if (error != null) break;
                error = ValidateIndexesAgainstRelations(dxmlTable);
                if (error != null) break;
            }

            if (!String.IsNullOrEmpty(error))
            {
                ExitTool(error);
            }
        }

        protected DXMLDefinitions GetDXMLDefinitions()
        {
            List<DXMLTable> dxmlTables = new List<DXMLTable>();
            List<DXMLView> dxmlViews = new List<DXMLView>();
            List<DXMLProcedure> dxmlProcedures = new List<DXMLProcedure>();
            List<DXMLTrigger> dxmlTriggers = new List<DXMLTrigger>();

            foreach (var dxmlFile in DXMLFiles)
            {
                string dxmlString = File.ReadAllText(dxmlFile);
                if (dxmlString.EndsWith("</Table>"))
                {
                    DXMLTable dxmlTable = CreateDXMLTable(dxmlString, dxmlFile);
                    if(dxmlTable == null)
                    {
                        ExitTool("Cannot Create Table Definition For " + Path.GetFileName(dxmlFile));
                    }
                    dxmlTables.Add(dxmlTable);
                }
                else if (dxmlString.EndsWith("</View>"))
                {
                    DXMLView dxmlView = CreateDXMLView(dxmlString, dxmlFile);
                    if (dxmlView == null)
                    {
                        ExitTool("Error: Cannot Create View Definition For " + Path.GetFileName(dxmlFile));
                    }
                    dxmlViews.Add(dxmlView);
                }
                else if (dxmlString.EndsWith("</Procedure>"))
                {
                    DXMLProcedure dxmlProcedure = CreateDXMLProcedure(dxmlString, dxmlFile);
                    if (dxmlProcedure == null)
                    {
                        ExitTool("Error: Cannot Create Procedure Definition For " + Path.GetFileName(dxmlFile));
                    }
                    dxmlProcedures.Add(dxmlProcedure);
                }
                else if (dxmlString.EndsWith("</Trigger>"))
                {
                    DXMLTrigger dxmlTrigger = CreateDXMLTrigger(dxmlString, dxmlFile);
                    if (dxmlTrigger == null)
                    {
                        ExitTool("Error: Cannot Create Trigger Definition For " + Path.GetFileName(dxmlFile));
                    }
                    dxmlTriggers.Add(dxmlTrigger);
                }
                else
                {
                    ExitTool("Error: Cannot Create Class Definition For " + Path.GetFileName(dxmlFile));
                }
            }

            return new DXMLDefinitions
            {
                DXMLTables = dxmlTables,
                DXMLViews = dxmlViews,
                DXMLProcedures = dxmlProcedures,
                DXMLTriggers = dxmlTriggers
            };
        }

        protected DXMLTable CreateDXMLTable(string dxmlString, string dxmlFile)
        {
            try
            {
                TableDefinition dxmlTableDefinition = dxmlString.ParseXML<TableDefinition>();

                DXMLTable dxmlTable = new DXMLTable
                {
                    DXMLFileName = Path.GetFileName(dxmlFile),
                    TableDefinition = dxmlTableDefinition
                };

                return dxmlTable;
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected DXMLView CreateDXMLView(string dxmlString, string dxmlFile)
        {
            try
            {
                ViewDefinition dxmlViewDefinition = dxmlString.ParseXML<ViewDefinition>();

                DXMLView dxmlView = new DXMLView
                {
                    DXMLFileName = Path.GetFileName(dxmlFile),
                    ViewDefinition = dxmlViewDefinition
                };

                return dxmlView;
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected DXMLProcedure CreateDXMLProcedure(string dxmlString, string dxmlFile)
        {
            try
            {
                ProcedureDefinition dxmlProcedureDefinition = dxmlString.ParseXML<ProcedureDefinition>();

                DXMLProcedure dxmlProcedure = new DXMLProcedure
                {
                    DXMLFileName = Path.GetFileName(dxmlFile),
                    ProcedureDefinition = dxmlProcedureDefinition
                };

                return dxmlProcedure;
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected DXMLTrigger CreateDXMLTrigger(string dxmlString, string dxmlFile)
        {
            try
            {
                TriggerDefinition dxmlTriggerDefinition = dxmlString.ParseXML<TriggerDefinition>();

                DXMLTrigger dxmlTrigger = new DXMLTrigger
                {
                    DXMLFileName = Path.GetFileName(dxmlFile),
                    TriggerDefinition = dxmlTriggerDefinition
                };

                return dxmlTrigger;
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected string ValidateDXMLFilesNames()
        {
            string error = null;

            List<string> duplicatedDxmlFiles = DXMLFiles.Select(d => Path.GetFileName(d)).ToList().GroupBy(d => d).SelectMany(g => g.Skip(1)).ToList();

            if (duplicatedDxmlFiles.Any())
            {
                error = "Error: Duplicate DXML Files:\n";
                foreach(var dxmlFile in DXMLFiles.Where(d => d.Contains(@"\" + duplicatedDxmlFiles.First())).ToList())
                {
                    error += dxmlFile + "\n";
                }
                return error.TrimEnd('\n');
            }

            return error;
        }

        protected string ValidateDXMLViewsNames()
        {
            string error = null;

            List<ViewDefinition> duplicatedDxmlViews = DXMLViews.Select(d => d.ViewDefinition).ToList().GroupBy(d => new { Name = d.Name.ToLower(), DBType = d.DBType.ToLower() }).SelectMany(g => g.Skip(1)).ToList();

            if (duplicatedDxmlViews.Any())
            {
                error = "Error: Duplicate DXML Views With Same Database Type:\n";
                foreach (var dxmlView in DXMLViews.Where(d => d.ViewDefinition.Name.ToLower() == duplicatedDxmlViews.First().Name.ToLower()).ToList())
                {
                    error += DXMLFiles.Where(d => d.Contains(@"\" + dxmlView.DXMLFileName)).First() + "\n";
                }
                return error.TrimEnd('\n');
            }

            return error;
        }

        protected string ValidateDXMLProceduresNames()
        {
            string error = null;

            List<ProcedureDefinition> duplicatedDxmlProcedures = DXMLProcedures.Select(d => d.ProcedureDefinition).ToList().GroupBy(d => new { Name = d.Name.ToLower(), DBType = d.DBType.ToLower() }).SelectMany(g => g.Skip(1)).ToList();

            if (duplicatedDxmlProcedures.Any())
            {
                error = "Error: Duplicate DXML Procedures With Same Database Type:\n";
                foreach (var dxmlProcedure in DXMLProcedures.Where(d => d.ProcedureDefinition.Name.ToLower() == duplicatedDxmlProcedures.First().Name.ToLower()).ToList())
                {
                    error += DXMLFiles.Where(d => d.Contains(@"\" + dxmlProcedure.DXMLFileName)).First() + "\n";
                }
                return error.TrimEnd('\n');
            }

            return error;
        }

        protected string ValidateDXMLTriggersNames()
        {
            string error = null;

            List<TriggerDefinition> duplicatedDxmlTriggers = DXMLTriggers.Select(d => d.TriggerDefinition).ToList().GroupBy(d => new { Name = d.Name.ToLower(), DBType = d.DBType.ToLower() }).SelectMany(g => g.Skip(1)).ToList();

            if (duplicatedDxmlTriggers.Any())
            {
                error = "Error: Duplicate DXML Triggers With Same Database Type:\n";
                foreach (var dxmlTrigger in DXMLTriggers.Where(d => d.TriggerDefinition.Name.ToLower() == duplicatedDxmlTriggers.First().Name.ToLower()).ToList())
                {
                    error += DXMLFiles.Where(d => d.Contains(@"\" + dxmlTrigger.DXMLFileName)).First() + "\n";
                }
                return error.TrimEnd('\n');
            }

            return error;
        }

        protected string ValidateReferencedTables(DXMLTable dxmlTable)
        {
            string error = null;

            List<string> dxmlTablesNames = DXMLTables.Select(t => t.TableDefinition.Name).ToList();

            List<RelationDefinition> relationsWithWrongReferencedTableName = dxmlTable.TableDefinition.Relations
                .Where(r => !dxmlTablesNames.Contains(r.ReferencedTable)).ToList();

            if (relationsWithWrongReferencedTableName.Any())
            {
                error = "Invalid DXML Syntax: There Is No Table With Name [" + relationsWithWrongReferencedTableName.First().ReferencedTable + "] To Use It As Referenced Table For Relation In [" + dxmlTable.DXMLFileName + "]";
                return error;
            }

            List<RelationDefinition> relationsWithWrongReferencedTableSchema = dxmlTable.TableDefinition.Relations
                .Where(r => r.ReferencedTableSchema != DXMLTables.Where(t => t.TableDefinition.Name == r.ReferencedTable).First().TableDefinition.Schema).ToList();

            if (relationsWithWrongReferencedTableSchema.Any())
            {
                error = "Invalid DXML Syntax: The Referenced Table [" + relationsWithWrongReferencedTableSchema.First().ReferencedTable + "] Has Schema Not Equal To [" + relationsWithWrongReferencedTableSchema.First().ReferencedTableSchema + "] For Relation In [" + dxmlTable.DXMLFileName + "]";
                return error;
            }

            return error;
        }

        protected string ValidateReferencedColumns(DXMLTable dxmlTable)
        {
            string error = null;

            List<RelationDefinition> relationsWithWrongReferencedColumnName = dxmlTable.TableDefinition.Relations
                .Where(r => (!DXMLTables.Where(t => t.TableDefinition.Name == r.ReferencedTable)
                .First().TableDefinition.Columns.Select(c => c.Name).Contains(r.ReferencedColumn) && !r.ReferencedColumn.Contains(",")) || (r.ReferencedColumn.Split(',')
                .Where(cc => DXMLTables.Where(t => t.TableDefinition.Name == r.ReferencedTable)
                .First().TableDefinition.Columns.Select(c => c.Name).All(c => c != cc)).Any() && r.ReferencedColumn.Contains(","))).ToList();

            if (relationsWithWrongReferencedColumnName.Any())
            {
                error = "Invalid DXML Syntax: All Or Some Of Referenced Columns [" + relationsWithWrongReferencedColumnName.First().ReferencedColumn + "] Not In The Referenced Table [" + relationsWithWrongReferencedColumnName.First().ReferencedTable + "] For Relation In [" + dxmlTable.DXMLFileName + "]";
            }

            return error;
        }

        protected string ValidateNumberOfReferencedColumns(DXMLTable dxmlTable)
        {
            string error = null;

            List<RelationDefinition> relationsWithWrongReferencedColumnsNumber = dxmlTable.TableDefinition.Relations
                .Where(r => DXMLTables.Where(t => t.TableDefinition.Name == r.ReferencedTable)
                .First().TableDefinition.Columns.Where(c => c.Constraints.PrimaryKey).Count() != r.ReferencedColumn.Split(',').Count()).ToList();

            if (relationsWithWrongReferencedColumnsNumber.Any())
            {
                error = "Invalid DXML Syntax: Number Of Referenced Columns [" + relationsWithWrongReferencedColumnsNumber.First().ReferencedColumn + "] Not Equal To Number Of Primary Key Columns In The Referenced Table [" + relationsWithWrongReferencedColumnsNumber.First().ReferencedTable + "] For Relation In [" + dxmlTable.DXMLFileName + "]";
            }

            return error;
        }

        protected string ValidateForeignKeyColumns(DXMLTable dxmlTable)
        {
            string error = null;

            List<string> dxmlTableColumnsNames = dxmlTable.TableDefinition.Columns.Select(c => c.Name).ToList();

            List<RelationDefinition> relationsWithWrongForeignKeyColumnName = dxmlTable.TableDefinition.Relations
                .Where(r => (!dxmlTableColumnsNames.Contains(r.ForeignKeyColumn) && !r.ForeignKeyColumn.Contains(",")) || (r.ForeignKeyColumn.Split(',')
                .Where(cc => dxmlTableColumnsNames.All(c => c != cc)).Any() && r.ForeignKeyColumn.Contains(","))).ToList();

            if (relationsWithWrongForeignKeyColumnName.Any())
            {
                error = "Invalid DXML Syntax: All Or Some Of Foreign Key Columns [" + relationsWithWrongForeignKeyColumnName.First().ForeignKeyColumn + "] Not In The Parent Table [" + dxmlTable.TableDefinition.Name + "] For Relation In [" + dxmlTable.DXMLFileName + "]";
            }

            return error;
        }

        protected string ValidateNumberOfForeignKeyColumns(DXMLTable dxmlTable)
        {
            string error = null;

            List<RelationDefinition> relationsWithWrongForeignKeyColumnsNumber = dxmlTable.TableDefinition.Relations
                .Where(r => r.ForeignKeyColumn.Split(',').Count() != r.ReferencedColumn.Split(',').Count()).ToList();

            if (relationsWithWrongForeignKeyColumnsNumber.Any())
            {
                error = "Invalid DXML Syntax: Number Of Foreign Key Columns [" + relationsWithWrongForeignKeyColumnsNumber.First().ForeignKeyColumn + "] Not Equal To Number Of Referenced Columns [" + relationsWithWrongForeignKeyColumnsNumber.First().ReferencedColumn + "] For Relation In [" + dxmlTable.DXMLFileName + "]";
            }

            return error;
        }

        protected string ValidateForeignKeyColumnsDataType(DXMLTable dxmlTable)
        {
            string error = null;

            foreach (var relation in dxmlTable.TableDefinition.Relations)
            {
                string[] foreignKeyColumns = relation.ForeignKeyColumn.Split(',');

                for(int i = 0; i < foreignKeyColumns.Length; i++)
                {
                    bool dataTypeNotSame = false;
                    string referencedColumnName = relation.ReferencedColumn.Split(',')[i];

                    ColumnDefinition referencedColumn = DXMLTables.Where(t => t.TableDefinition.Name == relation.ReferencedTable).First().TableDefinition.Columns
                        .Where(c => c.Name == referencedColumnName).First();

                    string foreignKeyColumnType = dxmlTable.TableDefinition.Columns.Where(c => c.Name == foreignKeyColumns[i]).First().Type;
                    string referencedColumnType = referencedColumn.Type;
                    
                    if (foreignKeyColumnType != referencedColumnType)
                    {
                        dataTypeNotSame = true;
                    }

                    int foreignKeyColumnSize = dxmlTable.TableDefinition.Columns.Where(c => c.Name == foreignKeyColumns[i]).First().Size;
                    int referencedColumnSize = referencedColumn.Size;
                    if (foreignKeyColumnSize != referencedColumnSize)
                    {
                        dataTypeNotSame = true;
                    }

                    int foreignKeyColumnPrecision = dxmlTable.TableDefinition.Columns.Where(c => c.Name == foreignKeyColumns[i]).First().Precision;
                    int referencedColumnPrecision = referencedColumn.Precision;
                    if (foreignKeyColumnPrecision != referencedColumnPrecision)
                    {
                        dataTypeNotSame = true;
                    }

                    int foreignKeyColumnScale = dxmlTable.TableDefinition.Columns.Where(c => c.Name == foreignKeyColumns[i]).First().Scale;
                    int referencedColumnScale = referencedColumn.Scale;
                    if (foreignKeyColumnScale != referencedColumnScale)
                    {
                        dataTypeNotSame = true;
                    }

                    if (dataTypeNotSame)
                    {
                        error = "Invalid DXML Syntax: Data Type Of Foreign Key Column [" + foreignKeyColumns[i] + "] In The Parent Table [" + dxmlTable.TableDefinition.Name + "] Different From Data Type Of Primary Key Column [" + referencedColumnName + "] In The Referenced Table [" + relation.ReferencedTable + "] For Relation In [" + dxmlTable.DXMLFileName + "]";
                        break;
                    }
                }

                if(error != null)
                {
                    break;
                }
            }

            return error;
        }

        protected string ValidatePrimaryKeys(DXMLTable dxmlTable)
        {
            string error = null;

            List<ColumnDefinition> wrongPrimaryKeyColumns = dxmlTable.TableDefinition.Columns.Where(c => c.Constraints.PrimaryKey == true && c.Constraints.Nullable == true).ToList();

            if (wrongPrimaryKeyColumns.Any())
            {
                error = "Invalid DXML Syntax: Primary Key Column [" + wrongPrimaryKeyColumns.First().Name + "] Cannot Be Nullable In [" + dxmlTable.DXMLFileName + "]";
            }

            return error;
        }

        protected string ValidateIndexesColumns(DXMLTable dxmlTable)
        {
            string error = null;

            List<string> dxmlTableColumnsNames = dxmlTable.TableDefinition.Columns.Select(c => c.Name).ToList();
            
            List<IndexDefinition> indexesWithWrongColumnsNames = dxmlTable.TableDefinition.Indexes
                .Where(i => (!dxmlTableColumnsNames.Contains(i.Columns) && !i.Columns.Contains(",")) || (i.Columns.Split(',')
                .Where(cc => dxmlTableColumnsNames.All(c => c != cc)).Any() && i.Columns.Contains(","))).ToList();

            if (indexesWithWrongColumnsNames.Any())
            {
                error = "Invalid DXML Syntax: All Or Some Of Columns [" + indexesWithWrongColumnsNames.First().Columns + "] Not In The Table [" + dxmlTable.TableDefinition.Name + "] For Index In [" + dxmlTable.DXMLFileName + "]";
            }

            List<IndexDefinition> indexesWithWrongIncludeColumnsNames = dxmlTable.TableDefinition.Indexes
                .Where(i => (i.Include != null && !dxmlTableColumnsNames.Contains(i.Include) && !i.Include.Contains(",")) || (i.Include != null && i.Include.Split(',')
                .Where(cc => dxmlTableColumnsNames.All(c => c != cc)).Any() && i.Include.Contains(","))).ToList();

            if (indexesWithWrongIncludeColumnsNames.Any())
            {
                error = "Invalid DXML Syntax: All Or Some Of Included Columns [" + indexesWithWrongIncludeColumnsNames.First().Include + "] Not In The Table [" + dxmlTable.TableDefinition.Name + "] For Index In [" + dxmlTable.DXMLFileName + "]";
            }

            return error;
        }

        protected string ValidateUniqueConstraintsColumns(DXMLTable dxmlTable)
        {
            string error = null;

            List<string> dxmlTableColumnsNames = dxmlTable.TableDefinition.Columns.Select(c => c.Name).ToList();

            List<UniqueConstraintDefinition> uniqueConstraintsWithWrongColumnsNames = dxmlTable.TableDefinition.UniqueConstraints
                .Where(u => (!dxmlTableColumnsNames.Contains(u.Columns) && !u.Columns.Contains(",")) || (u.Columns.Split(',')
                .Where(cc => dxmlTableColumnsNames.All(c => c != cc)).Any() && u.Columns.Contains(","))).ToList();

            if (uniqueConstraintsWithWrongColumnsNames.Any())
            {
                error = "Invalid DXML Syntax: All Or Some Of Columns [" + uniqueConstraintsWithWrongColumnsNames.First().Columns + "] Not In The Table [" + dxmlTable.TableDefinition.Name + "] For Unique Constraint In [" + dxmlTable.DXMLFileName + "]";
            }

            return error;
        }

        protected string ValidateDuplicateIndexes(DXMLTable dxmlTable)
        {
            string error = null;

            List<string> duplicatedIndexesColumns = dxmlTable.TableDefinition.Indexes.Select(i => i.Columns).ToList().GroupBy(i => i).SelectMany(g => g.Skip(1)).ToList();
            
            if (duplicatedIndexesColumns.Any())
            {
                error = "Invalid DXML Syntax: Duplicate Index On Columns [" + duplicatedIndexesColumns.First() + "] In [" + dxmlTable.DXMLFileName + "]";
            }

            return error;
        }

        protected string ValidateDuplicateUniqueConstraints(DXMLTable dxmlTable)
        {
            string error = null;

            List<string> duplicatedUniqueConstraintsColumns = dxmlTable.TableDefinition.UniqueConstraints.Select(u => u.Columns).ToList().GroupBy(u => u).SelectMany(g => g.Skip(1)).ToList();

            if (duplicatedUniqueConstraintsColumns.Any())
            {
                error = "Invalid DXML Syntax: Duplicate Unique Constraint On Columns [" + duplicatedUniqueConstraintsColumns.First() + "] In [" + dxmlTable.DXMLFileName + "]";
            }

            return error;
        }

        protected string ValidateIndexesAgainstRelations(DXMLTable dxmlTable)
        {
            string error = null;

            List<string> unnecessaryIndexesColumns = dxmlTable.TableDefinition.Indexes.Select(i => i.Columns)
                .Where(c => dxmlTable.TableDefinition.Relations.Select(r => r.ForeignKeyColumn).Contains(c)).ToList();

            if (unnecessaryIndexesColumns.Any())
            {
                error = "Invalid DXML Syntax: Unnecessary Index On Columns [" + unnecessaryIndexesColumns.First() + "] In [" + dxmlTable.DXMLFileName + "]";
            }

            return error;
        }

        protected void ExitTool(string message)
        {
            Console.WriteLine(message);
#if DEBUG

            Console.ReadLine();
#endif
            Environment.Exit(1);
        }
    }
}