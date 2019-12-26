using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;

namespace Logitude.DXMLGenerator.Models
{
    public class DXMLFilesGenerator
    {
        private readonly string Root = ConfigurationManager.AppSettings["Root"];

        private string ConnectionString;
        private string ErrorsFileName;
        private List<string> ExcludedTables;
        private List<string> ExcludedTablesNames;
        private int GeneratedDXMLFilesCounter = 0;
        private int GeneratedPathsCounter = 0;
        private string ErrorsData = "";

        public DXMLFilesGenerator(string connectionString, string errorsFileName)
        {
            ConnectionString = connectionString;
            ErrorsFileName = errorsFileName;
            BuildExcludedTablesList();
        }

        public void GenerateDXMLFiles()
        {
            List<DBTable> dbTables = GetTablesFromDB();
            if(dbTables != null)
            {
                foreach (var table in dbTables)
                {
                    Console.WriteLine("Generating DXML File For " + table.Name + " Table ...");
                    SerializeAndSaveTable(table);
                }
                Console.WriteLine("\n" + GeneratedDXMLFilesCounter + " DXML Files Generated Successfully\n");
            }

            ExportErrorsData();
        }

        public void GeneratePathsForDXMLFiles()
        {
            List<DBTable> dbTables = GetTablesFromDB();
            if (dbTables != null)
            {
                Console.WriteLine("Generating DXML Files Paths For " + dbTables.Count() + " Tables ...");
                foreach (var table in dbTables)
                {
                    string path = GetPathForDXMLFile(table.Name, false);
                    if (!String.IsNullOrEmpty(path))
                    {
                        Console.WriteLine(table.Name + " DXML File Path: " + path);
                        GeneratedPathsCounter++;
                    }
                    else
                    {
                        ErrorsData += "Cannot Find Path For " + table.Name + " Table" + "\n";
                    }
                }
                Console.WriteLine("\n" + GeneratedPathsCounter + " Paths Generated Successfully\n");
            }

            ExportErrorsData();
        }

        private List<DBTable> GetTablesFromDB()
        {
            string queryString = @"SELECT TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME FROM INFORMATION_SCHEMA.TABLES";

            SqlDataReader reader = null;
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand(queryString, connection);

            try
            {
                connection.Open();
                reader = command.ExecuteReader();

                List<DBTable> dbTables = new List<DBTable>();

                while (reader.Read())
                {
                    string tableName = reader["TABLE_NAME"].ToString();

                    if (tableName != "__MigrationHistory")
                    {
                        string schema = reader["TABLE_SCHEMA"].ToString();
                        string dbName = reader["TABLE_CATALOG"].ToString();

                        DBTable dbTable = new DBTable
                        {
                            Name = tableName,
                            Schema = schema,
                            DBName = dbName
                        };

                        dbTables.Add(dbTable);
                    }
                }

                reader.Close();
                connection.Close();

                return dbTables;
            }
            catch (Exception)
            {
                if (reader != null)
                {
                    reader.Close();
                }
                connection.Close();

                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("Error While Get Tables From The Database");
                Console.ResetColor();
                ErrorsData += "Error While Get Tables From The Database" + "\n";
                return null;
            }
        }

        private TableDefinition GetTableDefinition(DBTable table)
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

            TableDefinition tableDefinition = null;

            SqlDataReader reader = null;
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@tableName", table.Name);

            try
            {
                connection.Open();
                reader = command.ExecuteReader();

                List<ColumnDefinition> columnsDefinitions = new List<ColumnDefinition>();

                while (reader.Read())
                {
                    if (!columnsDefinitions.Where(c => c.Name == reader["ColumnName"].ToString()).Any())
                    {
                        ColumnDefinition column = new ColumnDefinition
                        {
                            Name = reader["ColumnName"].ToString(),
                            Type = GetColumnDefinitionDataType(reader["DataType"].ToString()),
                            Size = !String.IsNullOrEmpty(reader["Size"].ToString()) ? Convert.ToInt32(reader["Size"].ToString()) : 0,
                            Precision = !String.IsNullOrEmpty(reader["Precision"].ToString()) ? Convert.ToInt32(reader["Precision"].ToString()) : 0,
                            Scale = !String.IsNullOrEmpty(reader["Scale"].ToString()) ? Convert.ToInt32(reader["Scale"].ToString()) : 0,
                            Constraints = new ConstraintsDefinition
                            {
                                Nullable = (reader["Nullable"].ToString() == "YES")
                            }
                        };

                        if (!String.IsNullOrEmpty(reader["ConstraintType"].ToString()) && !String.IsNullOrEmpty(reader["ConstraintName"].ToString()))
                        {
                            column = SetConstraintForColumnDefinition(column, reader["ConstraintType"].ToString());
                        }

                        columnsDefinitions.Add(column);
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(reader["ConstraintType"].ToString()) && !String.IsNullOrEmpty(reader["ConstraintName"].ToString()))
                        {
                            var column = columnsDefinitions.Where(c => c.Name == reader["ColumnName"].ToString()).First();
                            column = SetConstraintForColumnDefinition(column, reader["ConstraintType"].ToString());
                        }
                    }
                }

                reader.Close();
                connection.Close();

                tableDefinition = new TableDefinition
                {
                    Name = table.Name,
                    Schema = table.Schema,
                    DBType = GetDatabaseType(table.DBName),
                    Columns = columnsDefinitions,
                    Relations = GetTableRelations(table.Name)
                };
            }
            catch (Exception)
            {
                if(reader != null)
                {
                    reader.Close();
                }
                connection.Close();

                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("Error While Get Table Definition For " + table.Name + " Table");
                Console.ResetColor();
                ErrorsData += "Error While Get Table Definition For " + table.Name + " Table" + "\n";
            }

            return tableDefinition;
        }

        private List<RelationDefinition> GetTableRelations(string tableName)
        {
            string queryString = @"SELECT ParentTable.name AS ParentTableName, ParentColumn.name AS ParentColumnName, ReferencedTable.name AS ReferencedTableName, ReferencedColumn.name AS ReferencedColumnName, SysObject.name AS ForeignKeyConstraintName, ParentTableSchema.name AS ParentTableSchemaName, ReferencedTableSchema.name AS ReferencedTableSchemaName, ReferencedColumn.column_id AS ReferencedColumnOrder " +
                                  "FROM SYS.FOREIGN_KEY_COLUMNS ForeignKeyColumns " +
                                  "INNER JOIN SYS.TABLES ParentTable ON ParentTable.object_id = ForeignKeyColumns.parent_object_id " +
                                  "INNER JOIN SYS.SCHEMAS ParentTableSchema ON ParentTable.schema_id = ParentTableSchema.schema_id " +
                                  "INNER JOIN SYS.COLUMNS ParentColumn ON ParentColumn.column_id = ForeignKeyColumns.parent_column_id AND ParentColumn.object_id = ParentTable.object_id " +
                                  "INNER JOIN SYS.TABLES ReferencedTable ON ReferencedTable.object_id = ForeignKeyColumns.referenced_object_id " +
                                  "INNER JOIN SYS.SCHEMAS ReferencedTableSchema ON ReferencedTable.schema_id = ReferencedTableSchema.schema_id " +
                                  "INNER JOIN SYS.COLUMNS ReferencedColumn ON ReferencedColumn.column_id = ForeignKeyColumns.referenced_column_id AND ReferencedColumn.object_id = ReferencedTable.object_id " +
                                  "INNER JOIN SYS.OBJECTS SysObject ON SysObject.object_id = ForeignKeyColumns.constraint_object_id " +
                                  "WHERE ForeignKeyColumns.parent_object_id = (SELECT object_id FROM SYS.TABLES WHERE name = @tableName)";

            SqlDataReader reader = null;
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@tableName", tableName);

            try
            {
                connection.Open();
                reader = command.ExecuteReader();

                List<RelationDefinition> relations = new List<RelationDefinition>();

                while (reader.Read())
                {
                    RelationDefinition relation = new RelationDefinition
                    {
                        ForeignKeyColumn = reader["ParentColumnName"].ToString(),
                        ReferencedTable = reader["ReferencedTableName"].ToString(),
                        ReferencedColumn = reader["ReferencedColumnName"].ToString(),
                        ForeignKeyConstraintName = reader["ForeignKeyConstraintName"].ToString(),
                        ReferencedTableSchema = reader["ReferencedTableSchemaName"].ToString(),
                        ReferencedColumnOrder = Convert.ToInt32(reader["ReferencedColumnOrder"].ToString())
                    };
                    relations.Add(relation);
                }

                reader.Close();
                connection.Close();

                List<RelationDefinition> processedRelations = HandlingCompositeRelations(relations);

                return processedRelations;
            }
            catch (Exception)
            {
                if (reader != null)
                {
                    reader.Close();
                }
                connection.Close();

                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("Error While Get Relations For " + tableName + " Table");
                Console.ResetColor();
                ErrorsData += "Error While Get Relations For " + tableName + " Table" + "\n";
                return null;
            }
        }

        private void SerializeAndSaveTable(DBTable table)
        {
            TableDefinition tableDefinition = GetTableDefinition(table);
            if(tableDefinition != null)
            {
                try
                {
                    string path = GetPathForDXMLFile(tableDefinition.Name, true);
                    if (!String.IsNullOrEmpty(path))
                    {
                        XmlSerializerNamespaces emptyNamespace = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(TableDefinition));
                        TextWriter textWriter = new StreamWriter(path);
                        xmlSerializer.Serialize(textWriter, tableDefinition, emptyNamespace);
                        textWriter.Close();
                        GeneratedDXMLFilesCounter++;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.WriteLine("Cannot Find Path For " + tableDefinition.Name + " Table");
                        Console.ResetColor();
                        ErrorsData += "Cannot Find Path For " + tableDefinition.Name + " Table" + "\n";
                    }
                }
                catch (Exception)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error While Serialize And Save Table Definition For " + tableDefinition.Name + " Table");
                    Console.ResetColor();
                    ErrorsData += "Error While Serialize And Save Table Definition For " + tableDefinition.Name + " Table" + "\n";
                }
            }
        }

        private string GetColumnDefinitionDataType(string type)
        {
            switch (type.ToLower())
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

        private string GetPathForDXMLFile(string tableName, bool createDirectory)
        {
            try
            {
                string entityName;

                if (ExcludedTables != null && ExcludedTablesNames != null && ExcludedTablesNames.Contains(tableName))
                {
                    string excludedTable = ExcludedTables.Where(t => t.Split('/')[0] == tableName).FirstOrDefault();
                    entityName = String.IsNullOrEmpty(excludedTable) ? null : excludedTable.Split('/')[1];
                }
                else
                {
                    PluralizationService pluralizationService = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en-us"));
                    entityName = pluralizationService.Singularize(tableName);
                }

                if (String.IsNullOrEmpty(entityName))
                {
                    return null;
                }

                string rootPath = Path.Combine(Root);
                string[] lxmlFiles = Directory.GetFiles(rootPath, entityName + ".lxml", SearchOption.AllDirectories);

                if (lxmlFiles.Length > 0)
                {
                    string lxmlFilePath = lxmlFiles[0];
                    string lxmlFileName = Path.GetFileName(lxmlFilePath);

                    if (lxmlFilePath.Contains(@"\EntityFiles\"))
                    {
                        string lxmlFileRootPath = lxmlFilePath.Split(new string[] { @"\EntityFiles\" }, StringSplitOptions.None)[0];
                        string lxmlFileFolderName;
                        if (!lxmlFilePath.Split(new string[] { @"\EntityFiles\" }, StringSplitOptions.None)[1].Contains(@"\"))
                        {
                            lxmlFileFolderName = null;
                        }
                        else
                        {
                            lxmlFileFolderName = lxmlFilePath.Split(new string[] { @"\EntityFiles\" }, StringSplitOptions.None)[1].Split(new string[] { @"\" + lxmlFileName }, StringSplitOptions.None)[0];
                        }

                        string dxmlFilePath = String.IsNullOrEmpty(lxmlFileFolderName) ? lxmlFileRootPath + @"\DBTables" : lxmlFileRootPath + @"\DBTables" + @"\" + lxmlFileFolderName;
                        if (!Directory.Exists(dxmlFilePath) && createDirectory)
                        {
                            Directory.CreateDirectory(dxmlFilePath);
                        }

                        return dxmlFilePath + @"\" + entityName + ".dxml";
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    string[] pocoFiles = Directory.GetFiles(rootPath, entityName + ".cs", SearchOption.AllDirectories);
                    if (pocoFiles.Length > 0)
                    {
                        string pocoFilePath = pocoFiles.ToList().Where(a => a.Contains(@"\EntityPOCOs\") || a.Contains(@"\POCOs\")).FirstOrDefault();

                        if (!String.IsNullOrEmpty(pocoFilePath))
                        {
                            string pocoFolderName = pocoFilePath.Contains(@"\EntityPOCOs\") ? "EntityPOCOs" : "POCOs";
                            string[] pocoFileFolders = pocoFilePath.Split(new string[] { @"\" }, StringSplitOptions.None);
                            string pocoFileFolderName = pocoFileFolders[Array.IndexOf(pocoFileFolders, pocoFolderName) - 1].Contains(".Data") ? null : pocoFileFolders[Array.IndexOf(pocoFileFolders, pocoFolderName) - 1];
                            pocoFileFolderName = pocoFileFolders[Array.IndexOf(pocoFileFolders, pocoFolderName) - 1].Contains(".SystemLogs") ? "SystemLogsModel" : pocoFileFolderName;

                            string dxmlFilePath = pocoFilePath.Split(new string[] { @"\Logitude\" }, StringSplitOptions.None)[0] + @"\Logitude\Logitude.MetaData\DBTables" + (!String.IsNullOrEmpty(pocoFileFolderName) ? @"\" + pocoFileFolderName : null);
                            if (!Directory.Exists(dxmlFilePath))
                            {
                                Directory.CreateDirectory(dxmlFilePath);
                            }

                            return dxmlFilePath + @"\" + entityName + ".dxml";
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string GetDatabaseType(string dbName)
        {
            if (dbName.ToLower().Contains("main"))
            {
                return "Main";
            }
            else if (dbName.ToLower().Contains("global"))
            {
                return "Global";
            }
            else if (dbName.ToLower().Contains("systemlogs"))
            {
                return "SystemLogs";
            }
            else
            {
                return null;
            }
        }

        private void BuildExcludedTablesList()
        {
            try
            {
                string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                string filePath = Path.Combine(projectDirectory, @"ExcludedTablesMap.txt");
                List<string> excludedTablesList = File.ReadLines(filePath).ToList();
                if (excludedTablesList.Count() > 0)
                {
                    ExcludedTables = excludedTablesList;
                    ExcludedTablesNames = excludedTablesList.Select(t => t.Split('/')[0]).ToList();
                }
                else
                {
                    ExcludedTables = new List<string>();
                    ExcludedTablesNames = new List<string>();
                }
            }
            catch(Exception)
            {
                ExcludedTables = null;
                ExcludedTablesNames = null;
            }
        }

        private List<RelationDefinition> HandlingCompositeRelations(List<RelationDefinition> relations)
        {
            List<string> processedConstraints = new List<string>();
            List<RelationDefinition> processedRelations = new List<RelationDefinition>();

            foreach(var relation in relations)
            {
                if (!processedConstraints.Contains(relation.ForeignKeyConstraintName))
                {
                    string foreignKeyColumn;
                    string referencedColumn;
                    string referencedTable;
                    string referencedTableSchema;

                    List<RelationDefinition> relationsWithSameConstraint = relations.Where(r => r.ForeignKeyConstraintName == relation.ForeignKeyConstraintName).OrderBy(r => r.ReferencedColumnOrder).ToList();

                    if (relationsWithSameConstraint.Count() > 1)
                    {
                        foreignKeyColumn = string.Join(",", relationsWithSameConstraint.Select(r => r.ForeignKeyColumn).ToArray());
                        referencedColumn = string.Join(",", relationsWithSameConstraint.Select(r => r.ReferencedColumn).ToArray());
                        referencedTable = relationsWithSameConstraint.First().ReferencedTable;
                        referencedTableSchema = relationsWithSameConstraint.First().ReferencedTableSchema;
                    }
                    else
                    {
                        foreignKeyColumn = relation.ForeignKeyColumn;
                        referencedColumn = relation.ReferencedColumn;
                        referencedTable = relation.ReferencedTable;
                        referencedTableSchema = relation.ReferencedTableSchema;
                    }

                    RelationDefinition processedRelation = new RelationDefinition
                    {
                        ForeignKeyColumn = foreignKeyColumn,
                        ReferencedColumn = referencedColumn,
                        ReferencedTable = referencedTable,
                        ReferencedTableSchema = referencedTableSchema
                    };

                    processedRelations.Add(processedRelation);
                    processedConstraints.Add(relation.ForeignKeyConstraintName);
                }
            }

            return processedRelations;
        }

        private void ExportErrorsData()
        {
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string filePath = Path.Combine(projectDirectory, @"Errors\" + ErrorsFileName);
            File.WriteAllText(filePath, ErrorsData);

            if (!String.IsNullOrEmpty(ErrorsData))
            {
                Console.WriteLine("\n" + "Errors Are Exported To /Errors/" + ErrorsFileName + "\n");
            }
        }
    }
}