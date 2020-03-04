using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Logitude.DBMigrations.Models
{
    public class DXMLValidation
    {
        private string[] DXMLFiles;
        private List<DXMLTable> DXMLTables;
        
        public DXMLValidation(string[] dxmlFiles)
        {
            DXMLFiles = dxmlFiles;
            DXMLTables = GetDXMLTables();
        }

        public void Validate()
        {
            string error;

            error = ValidateDXMLFilesNames();
            
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
            }

            if (!String.IsNullOrEmpty(error))
            {
                ExitTool(error);
            }
        }

        private List<DXMLTable> GetDXMLTables()
        {
            List<DXMLTable> dxmlTables = new List<DXMLTable>();

            foreach (var dxmlFile in DXMLFiles)
            {
                string xmlString = File.ReadAllText(dxmlFile);
                if (xmlString.EndsWith("</Table>"))
                {
                    DXMLTable dxmlTable = CreateDXMLTable(xmlString, dxmlFile);
                    if(dxmlTable == null)
                    {
                        ExitTool("Cannot Create Table Definition For " + Path.GetFileName(dxmlFile));
                    }
                    dxmlTables.Add(dxmlTable);
                }
            }

            return dxmlTables;
        }

        private DXMLTable CreateDXMLTable(string xmlString, string dxmlFile)
        {
            try
            {
                TableDefinition dxmlTableDefinition = xmlString.ParseXML<TableDefinition>();

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

        private string ValidateDXMLFilesNames()
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

        private string ValidateReferencedTables(DXMLTable dxmlTable)
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

        private string ValidateReferencedColumns(DXMLTable dxmlTable)
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

        private string ValidateNumberOfReferencedColumns(DXMLTable dxmlTable)
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

        private string ValidateForeignKeyColumns(DXMLTable dxmlTable)
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

        private string ValidateNumberOfForeignKeyColumns(DXMLTable dxmlTable)
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

        private string ValidateForeignKeyColumnsDataType(DXMLTable dxmlTable)
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

        private string ValidatePrimaryKeys(DXMLTable dxmlTable)
        {
            string error = null;

            List<ColumnDefinition> wrongPrimaryKeyColumns = dxmlTable.TableDefinition.Columns.Where(c => c.Constraints.PrimaryKey == true && c.Constraints.Nullable == true).ToList();

            if (wrongPrimaryKeyColumns.Any())
            {
                error = "Invalid DXML Syntax: Primary Key Column [" + wrongPrimaryKeyColumns.First().Name + "] Cannot Be Nullable In [" + dxmlTable.DXMLFileName + "]";
            }

            return error;
        }

        private string ValidateIndexesColumns(DXMLTable dxmlTable)
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

        private string ValidateUniqueConstraintsColumns(DXMLTable dxmlTable)
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

        private string ValidateDuplicateIndexes(DXMLTable dxmlTable)
        {
            string error = null;

            List<string> duplicatedIndexesColumns = dxmlTable.TableDefinition.Indexes.Select(i => i.Columns).ToList().GroupBy(i => i).SelectMany(g => g.Skip(1)).ToList();
            
            if (duplicatedIndexesColumns.Any())
            {
                error = "Invalid DXML Syntax: Duplicate Index On Columns [" + duplicatedIndexesColumns.First() + "] In [" + dxmlTable.DXMLFileName + "]";
            }

            return error;
        }

        private string ValidateDuplicateUniqueConstraints(DXMLTable dxmlTable)
        {
            string error = null;

            List<string> duplicatedUniqueConstraintsColumns = dxmlTable.TableDefinition.UniqueConstraints.Select(u => u.Columns).ToList().GroupBy(u => u).SelectMany(g => g.Skip(1)).ToList();

            if (duplicatedUniqueConstraintsColumns.Any())
            {
                error = "Invalid DXML Syntax: Duplicate Unique Constraint On Columns [" + duplicatedUniqueConstraintsColumns.First() + "] In [" + dxmlTable.DXMLFileName + "]";
            }

            return error;
        }

        private void ExitTool(string message)
        {
            Console.WriteLine(message);
            Environment.Exit(0);
        }
    }
}