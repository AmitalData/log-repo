using Logitude.DBMigrations.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Logitude.DBMigrations.Models
{
    public class DxmlValidation
    {
        private string[] DXMLFiles;
        private List<DxmlTable> DXMLTables;
        
        public DxmlValidation(string[] dxmlFiles)
        {
            DXMLFiles = dxmlFiles;
            DXMLTables = GetDxmlTableDefinitions();
        }

        public void Validate()
        {
            Console.WriteLine("Validating DXML Files ...");

            string error = null;

            foreach (var dxmlTable in DXMLTables)
            {
                error = ValidatePrimaryKeys(dxmlTable);
                if(error != null) break;
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
            }

            if (!String.IsNullOrEmpty(error))
            {
                ExitTool(error);
            }
        }

        private List<DxmlTable> GetDxmlTableDefinitions()
        {
            List<DxmlTable> dxmlTableDefinitions = new List<DxmlTable>();

            foreach (var dxmlFile in DXMLFiles)
            {
                string xmlString = File.ReadAllText(dxmlFile);
                TableDefinition dxmlTableDefinition = xmlString.ParseXML<TableDefinition>();
                DxmlTable dxmlTable = new DxmlTable
                {
                    DxmlFileName = Path.GetFileName(dxmlFile),
                    TableDefinition = dxmlTableDefinition
                };
                dxmlTableDefinitions.Add(dxmlTable);
            }

            return dxmlTableDefinitions;
        }

        private string ValidatePrimaryKeys(DxmlTable dxmlTable)
        {
            string error = null;

            List<ColumnDefinition> wrongPrimaryKeyColumns = dxmlTable.TableDefinition.Columns.Where(c => c.Constraints.PrimaryKey == true && c.Constraints.Nullable == true).ToList();

            if (wrongPrimaryKeyColumns.Any())
            {
                error = "Invalid DXML Syntax: Primary Key Column [" + wrongPrimaryKeyColumns.First().Name + "] Cannot Be Nullable In [" + dxmlTable.DxmlFileName + "]";
            }

            return error;
        }

        private string ValidateReferencedTables(DxmlTable dxmlTable)
        {
            string error = null;

            List<string> dxmlTablesNames = DXMLTables.Select(t => t.TableDefinition.Name).ToList();

            List<RelationDefinition> relationsWithWrongReferencedTableName = dxmlTable.TableDefinition.Relations.Where(r => !dxmlTablesNames.Contains(r.ReferencedTable)).ToList();

            if (relationsWithWrongReferencedTableName.Any())
            {
                error = "Invalid DXML Syntax: Invalid Referenced Table [" + relationsWithWrongReferencedTableName.First().ReferencedTable + "] For Relation In [" + dxmlTable.DxmlFileName + "]";
                return error;
            }

            List<RelationDefinition> relationsWithWrongReferencedTableSchema = dxmlTable.TableDefinition.Relations.Where(r => r.ReferencedTableSchema != DXMLTables.Where(t => t.TableDefinition.Name == r.ReferencedTable).First().TableDefinition.Schema).ToList();

            if (relationsWithWrongReferencedTableSchema.Any())
            {
                error = "Invalid DXML Syntax: Invalid Referenced Table Schema [" + relationsWithWrongReferencedTableSchema.First().ReferencedTableSchema + "] For Relation In [" + dxmlTable.DxmlFileName + "]";
                return error;
            }

            return error;
        }

        private string ValidateReferencedColumns(DxmlTable dxmlTable)
        {
            string error = null;

            List<RelationDefinition> relationsWithWrongReferencedColumnName = dxmlTable.TableDefinition.Relations.Where(r => (!DXMLTables.Where(t => t.TableDefinition.Name == r.ReferencedTable).First().TableDefinition.Columns.Select(c => c.Name).Contains(r.ReferencedColumn) && !r.ReferencedColumn.Contains(",")) || (r.ReferencedColumn.Split(',').Where(cc => DXMLTables.Where(t => t.TableDefinition.Name == r.ReferencedTable).First().TableDefinition.Columns.Select(c => c.Name).All(c => c != cc)).Any() && r.ReferencedColumn.Contains(","))).ToList();


            if (relationsWithWrongReferencedColumnName.Any())
            {
                error = "Invalid DXML Syntax: Invalid Referenced Column [" + relationsWithWrongReferencedColumnName.First().ReferencedColumn + "] For Relation In [" + dxmlTable.DxmlFileName + "]";
            }

            return error;
        }

        private string ValidateNumberOfReferencedColumns(DxmlTable dxmlTable)
        {
            string error = null;

            List<RelationDefinition> relationsWithWrongReferencedColumnsNumber = dxmlTable.TableDefinition.Relations.Where(r => DXMLTables.Where(t => t.TableDefinition.Name == r.ReferencedTable).First().TableDefinition.Columns.Where(c => c.Constraints.PrimaryKey).Count() != r.ReferencedColumn.Split(',').Count()).ToList();

            if (relationsWithWrongReferencedColumnsNumber.Any())
            {
                error = "Invalid DXML Syntax: Invalid Number Of Referenced Columns [" + relationsWithWrongReferencedColumnsNumber.First().ReferencedColumn + "] For Relation In [" + dxmlTable.DxmlFileName + "]";
            }

            return error;
        }

        private string ValidateForeignKeyColumns(DxmlTable dxmlTable)
        {
            string error = null;

            List<string> dxmlTableColumnsNames = dxmlTable.TableDefinition.Columns.Select(c => c.Name).ToList();

            List<RelationDefinition> relationsWithWrongForeignKeyColumnName = dxmlTable.TableDefinition.Relations.Where(r => (!dxmlTableColumnsNames.Contains(r.ForeignKeyColumn) && !r.ReferencedColumn.Contains(",")) || (r.ForeignKeyColumn.Split(',').Where(cc => dxmlTableColumnsNames.All(c => c != cc)).Any() && r.ForeignKeyColumn.Contains(","))).ToList();

            if (relationsWithWrongForeignKeyColumnName.Any())
            {
                error = "Invalid DXML Syntax: Invalid Foreign Key Column [" + relationsWithWrongForeignKeyColumnName.First().ForeignKeyColumn + "] For Relation In [" + dxmlTable.DxmlFileName + "]";
            }

            return error;
        }

        private string ValidateNumberOfForeignKeyColumns(DxmlTable dxmlTable)
        {
            string error = null;

            List<RelationDefinition> relationsWithWrongForeignKeyColumnsNumber = dxmlTable.TableDefinition.Relations.Where(r => r.ForeignKeyColumn.Split(',').Count() != r.ReferencedColumn.Split(',').Count()).ToList();

            if (relationsWithWrongForeignKeyColumnsNumber.Any())
            {
                error = "Invalid DXML Syntax: Invalid Number Of Foreign Key Columns [" + relationsWithWrongForeignKeyColumnsNumber.First().ForeignKeyColumn + "] For Relation In [" + dxmlTable.DxmlFileName + "]";
            }

            return error;
        }

        private string ValidateForeignKeyColumnsDataType(DxmlTable dxmlTable)
        {
            string error = null;

            foreach (var relation in dxmlTable.TableDefinition.Relations)
            {
                string[] foreignKeyColumns = relation.ForeignKeyColumn.Split(',');

                for(int i = 0; i < foreignKeyColumns.Length; i++)
                {
                    bool dataTypeNotSame = false;
                    string referencedColumn = relation.ReferencedColumn.Split(',')[i];

                    string foreignKeyColumnType = dxmlTable.TableDefinition.Columns.Where(c => c.Name == foreignKeyColumns[i]).First().Type;
                    string referencedColumnType = DXMLTables.Where(t => t.TableDefinition.Name == relation.ReferencedTable).First().TableDefinition.Columns.Where(c => c.Name == referencedColumn).First().Type;
                    if (foreignKeyColumnType != referencedColumnType)
                    {
                        dataTypeNotSame = true;
                    }

                    int foreignKeyColumnSize = dxmlTable.TableDefinition.Columns.Where(c => c.Name == foreignKeyColumns[i]).First().Size;
                    int referencedColumnSize = DXMLTables.Where(t => t.TableDefinition.Name == relation.ReferencedTable).First().TableDefinition.Columns.Where(c => c.Name == referencedColumn).First().Size;
                    if (foreignKeyColumnSize != referencedColumnSize)
                    {
                        dataTypeNotSame = true;
                    }

                    int foreignKeyColumnPrecision = dxmlTable.TableDefinition.Columns.Where(c => c.Name == foreignKeyColumns[i]).First().Precision;
                    int referencedColumnPrecision = DXMLTables.Where(t => t.TableDefinition.Name == relation.ReferencedTable).First().TableDefinition.Columns.Where(c => c.Name == referencedColumn).First().Precision;
                    if (foreignKeyColumnPrecision != referencedColumnPrecision)
                    {
                        dataTypeNotSame = true;
                    }

                    int foreignKeyColumnScale = dxmlTable.TableDefinition.Columns.Where(c => c.Name == foreignKeyColumns[i]).First().Scale;
                    int referencedColumnScale = DXMLTables.Where(t => t.TableDefinition.Name == relation.ReferencedTable).First().TableDefinition.Columns.Where(c => c.Name == referencedColumn).First().Scale;
                    if (foreignKeyColumnScale != referencedColumnScale)
                    {
                        dataTypeNotSame = true;
                    }

                    if (dataTypeNotSame)
                    {
                        error = "Invalid DXML Syntax: Invalid Data Type For Foreign Key Column [" + relation.ForeignKeyColumn + "] For Relation In [" + dxmlTable.DxmlFileName + "]";
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

        private void ExitTool(string message)
        {
            Console.WriteLine(message);
            Environment.Exit(0);
        }
    }
}