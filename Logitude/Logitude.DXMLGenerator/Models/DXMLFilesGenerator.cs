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
        private readonly string Root = ConfigurationManager.AppSettings["LogitudeRoot"];
        private readonly string ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];

        private List<string> ExcludedTables;
        private List<string> ExcludedTablesNames;
        private int GeneratedDXMLFilesCounter = 0;
        private int GeneratedPathsCounter = 0;
        private int DeleteCounter = 0;
        private string ErrorsData = "";

        public DXMLFilesGenerator()
        {
            BuildExcludedTablesList();
        }

        public void GenerateDXMLFiles()
        {
            List<string> dbTablesNames = GetAllTablesNamesFromDB();
            if(dbTablesNames != null)
            {
                foreach (string tableName in dbTablesNames)
                {
                    Console.WriteLine("Generating DXML File For " + tableName + " Table ...");
                    SerializeAndSaveTable(tableName);
                }
                Console.WriteLine("\n" + GeneratedDXMLFilesCounter + " DXML Files Generated Successfully\n");
                ExportErrorsData();
            }
        }

        public void GeneratePathsForDXMLFiles()
        {
            List<string> dbTablesNames = GetAllTablesNamesFromDB();
            if (dbTablesNames != null)
            {
                Console.WriteLine("Generating DXML Files Paths For " + dbTablesNames.Count() + " Tables ...");
                foreach (string tableName in dbTablesNames)
                {
                    string path = GetPathForDXMLFile(tableName, false);
                    if (!String.IsNullOrEmpty(path))
                    {
                        Console.WriteLine(tableName + " DXML File Path: " + path);
                        GeneratedPathsCounter++;
                    }
                    else
                    {
                        ErrorsData += "Cannot Find Path For " + tableName + " Table" + "\n";
                    }
                }
                Console.WriteLine("\n" + GeneratedPathsCounter + " Paths Generated Successfully\n");
                ExportErrorsData();
            }
        }
        
        public void DeleteDXMLFiles()
        {
            string rootPath = Path.Combine(Root);
            string[] dxmlFiles = Directory.GetFiles(rootPath, "*.dxml", SearchOption.AllDirectories);
            foreach (string dxmlFile in dxmlFiles)
            {
                string dxmlFileName = Path.GetFileName(dxmlFile);
                try
                {
                    Console.WriteLine("Deleting " + dxmlFileName + " ...");
                    File.Delete(dxmlFile);
                    DeleteCounter++;
                }
                catch (Exception)
                {
                    Console.WriteLine("Error While Delete " + dxmlFileName);
                }
            }
            Console.WriteLine("\n" + DeleteCounter + " DXML Files Deleted Successfully\n");
        }

        public void DeleteDBTablesFolders()
        {
            string rootPath = Path.Combine(Root);
            string[] folders = Directory.GetDirectories(rootPath, "DBTables", SearchOption.AllDirectories);
            foreach (string folder in folders)
            {
                try
                {
                    Console.WriteLine("Deleting " + folder + " ...");
                    Directory.Delete(folder, true);
                    DeleteCounter++;
                }
                catch (Exception)
                {
                    Console.WriteLine("Error While Delete " + folder);
                }
            }
            Console.WriteLine("\n" + DeleteCounter + " DBTables Folders Deleted Successfully\n");
        }

        private List<string> GetAllTablesNamesFromDB()
        {
            List<string> dbTablesNames = new List<string>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES";
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = null;

                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string tableName = reader["TABLE_NAME"].ToString();
                        if (tableName != "__MigrationHistory")
                        {
                            dbTablesNames.Add(tableName);
                        }
                    }

                    reader.Close();
                    connection.Close();

                    return dbTablesNames.OrderBy(n => n).ToList();
                }
                catch (Exception)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error While Get All Tables Names From The Database");
                    Console.ResetColor();
                    return null;
                }
            }
        }

        private TableDefinition GetTableDefinition(string tableName)
        {
            TableDefinition tableDefinition = null;
            List<ColumnDefinition> columnsDefinitions = new List<ColumnDefinition>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = @"SELECT Q1.*, Q2.ConstraintType, Q2.ConstraintName " +
                                      "FROM ( " +
                                      "SELECT COL.COLUMN_NAME AS ColumnName, IS_NULLABLE AS Nullable, DATA_TYPE AS DataType, CHARACTER_MAXIMUM_LENGTH AS Size " +
                                      "FROM INFORMATION_SCHEMA.COLUMNS AS COL " +
                                      "WHERE COL.TABLE_NAME = @tableName " +
                                      ") AS Q1 " +
                                      "LEFT JOIN ( " +
                                      "SELECT CON.COLUMN_NAME AS ColumnName, TCON.CONSTRAINT_TYPE AS ConstraintType, TCON.CONSTRAINT_NAME AS ConstraintName " +
                                      "FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TCON " +
                                      "INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS CON ON TCON.CONSTRAINT_NAME = CON.CONSTRAINT_NAME " +
                                      "WHERE TCON.TABLE_NAME = @tableName " +
                                      ") AS Q2 ON Q2.ColumnName = Q1.ColumnName";

                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = null;
                command.Parameters.AddWithValue("@tableName", tableName);

                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        if (!columnsDefinitions.Where(c => c.Name == reader["ColumnName"].ToString()).Any())
                        {
                            ColumnDefinition column = new ColumnDefinition
                            {
                                Name = reader["ColumnName"].ToString(),
                                Type = GetDxmlDataType(reader["DataType"].ToString()),
                                Size = !String.IsNullOrEmpty(reader["Size"].ToString()) ? Convert.ToInt32(reader["Size"].ToString()) : 0,
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
                        Name = tableName,
                        Columns = columnsDefinitions.OrderBy(c => c.Name).ToList(),
                        Relations = GetTableRelations(tableName)
                    };
                }
                catch (Exception)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error While Get Table Definition For " + tableName + " Table");
                    Console.ResetColor();
                }

                return tableDefinition;
            }
        }

        private List<RelationDefinition> GetTableRelations(string tableName)
        {
            List<RelationDefinition> relations = new List<RelationDefinition>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = "SELECT ParentTable.name AS ParentTableName, ParentColumn.name AS ParentColumnName, ReferencedTable.name AS ReferencedTableName, ReferencedColumn.name AS ReferencedColumnName, SysObject.name AS ForeignKeyConstraintName " +
                                     "FROM SYS.FOREIGN_KEY_COLUMNS ForeignKeyColumns " +
                                     "INNER JOIN SYS.TABLES ParentTable ON ParentTable.object_id = ForeignKeyColumns.parent_object_id " +
                                     "INNER JOIN SYS.COLUMNS ParentColumn ON ParentColumn.column_id = ForeignKeyColumns.parent_column_id AND ParentColumn.object_id = ParentTable.object_id " +
                                     "INNER JOIN SYS.TABLES ReferencedTable ON ReferencedTable.object_id = ForeignKeyColumns.referenced_object_id " +
                                     "INNER JOIN SYS.COLUMNS ReferencedColumn ON ReferencedColumn.column_id = ForeignKeyColumns.referenced_column_id AND ReferencedColumn.object_id = ReferencedTable.object_id " +
                                     "INNER JOIN SYS.OBJECTS SysObject ON SysObject.object_id = ForeignKeyColumns.constraint_object_id " +
                                     "WHERE ForeignKeyColumns.referenced_object_id = (SELECT object_id FROM SYS.TABLES WHERE name = @tableName)";

                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = null;
                command.Parameters.AddWithValue("@tableName", tableName);

                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        RelationDefinition relation = new RelationDefinition
                        {
                            ParentTableName = reader["ParentTableName"].ToString(),
                            ParentColumnName = reader["ParentColumnName"].ToString(),
                            ReferencedTableName = reader["ReferencedTableName"].ToString(),
                            ReferencedColumnName = reader["ReferencedColumnName"].ToString(),
                            ForeignKeyConstraintName = reader["ForeignKeyConstraintName"].ToString()
                        };
                        relations.Add(relation);
                    }

                    reader.Close();
                    connection.Close();
                }
                catch (Exception)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error While Get Relations For " + tableName + " Table");
                    Console.ResetColor();
                }

                return relations;
            }
        }

        private void SerializeAndSaveTable(string tableName)
        {
            TableDefinition tableDefinition = GetTableDefinition(tableName);
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

        private string GetDxmlDataType(string type)
        {
            switch (type)
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

                //string path = @"D:\LogitudeMainDXMLFiles\" + entityName + ".dxml";
                //return path;
            }
            catch (Exception)
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

        private void ExportErrorsData()
        {
            if (!String.IsNullOrEmpty(ErrorsData))
            {
                string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                string filePath = Path.Combine(projectDirectory, "Errors.txt");
                File.WriteAllText(filePath, ErrorsData);
                Console.WriteLine("\n" + "All Errors Are Exported To /Errors.txt\n");
            }
        }
    }
}