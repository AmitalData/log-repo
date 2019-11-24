using Logitude.DXMLGenerator.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Data.Entity.Design.PluralizationServices;

namespace Logitude.DXMLGenerator
{
    public static class Helper
    {
        private static readonly string Root = ConfigurationManager.AppSettings["Root"];
        private static readonly string ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];

        public static List<string> GetAllTablesFromDB()
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
                }
                catch (Exception)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error While Get All Tables Names From The Database");
                    Console.ResetColor();
                }

                return dbTablesNames.OrderBy(n => n).ToList();
            }
        }

        public static TableDefinition GetTableDefinition(string tableName)
        {
            TableDefinition tableDefinition = null;
            List<ColumnDefinition> columnsDefinitions = new List<ColumnDefinition>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = @"SELECT COL.COLUMN_NAME AS ColumnName, COL.IS_NULLABLE AS Nullable, COL.DATA_TYPE AS DataType, COL.CHARACTER_MAXIMUM_LENGTH AS Size, CON.CONSTRAINT_NAME AS ConstraintName, TCON.CONSTRAINT_TYPE AS ConstraintType " +
                                      "FROM INFORMATION_SCHEMA.COLUMNS COL LEFT OUTER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE CON ON COL.COLUMN_NAME = CON.COLUMN_NAME LEFT OUTER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS TCON ON CON.CONSTRAINT_NAME = TCON.CONSTRAINT_NAME " +
                                      "WHERE COL.TABLE_NAME = @tableName AND(CON.TABLE_NAME = @tableName OR CON.TABLE_NAME IS NULL)";

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
                                Size = !String.IsNullOrEmpty(reader["Size"].ToString()) ? (reader["Size"].ToString() == "-1" ? -1 : Convert.ToInt32(reader["Size"].ToString())) : 0,
                                Constraints = new ConstraintsDefinition
                                {
                                    Nullable = (reader["Nullable"].ToString() == "YES")
                                }
                            };

                            if (!String.IsNullOrEmpty(reader["ConstraintType"].ToString()) && !String.IsNullOrEmpty(reader["ConstraintName"].ToString()))
                            {
                                column = SetConstraintForColumnDefinition(column, reader["ConstraintType"].ToString(), reader["ConstraintName"].ToString());
                            }

                            columnsDefinitions.Add(column);
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(reader["ConstraintType"].ToString()) && !String.IsNullOrEmpty(reader["ConstraintName"].ToString()))
                            {
                                var column = columnsDefinitions.Where(c => c.Name == reader["ColumnName"].ToString()).First();
                                column = SetConstraintForColumnDefinition(column, reader["ConstraintType"].ToString(), reader["ConstraintName"].ToString());
                            }
                        }
                    }

                    reader.Close();
                    connection.Close();

                    tableDefinition = new TableDefinition
                    {
                        Name = tableName,
                        Columns = columnsDefinitions.OrderBy(c => c.Name).ToList()
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

        public static void SerializeAndSaveTableDefinition(TableDefinition tableDefinition)
        {
            //try
            //{
            //    var emptyNamespace = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            //    string path = GetPathForDXMLFile(tableDefinition.Name);
            //    if (!String.IsNullOrEmpty(path))
            //    {
            //        XmlSerializer xmlSerializer = new XmlSerializer(typeof(TableDefinition));
            //        TextWriter textWriter = new StreamWriter(path);
            //        xmlSerializer.Serialize(textWriter, tableDefinition, emptyNamespace);
            //        textWriter.Close();
            //    }
            //    else
            //    {
            //        Console.BackgroundColor = ConsoleColor.Red;
            //        Console.WriteLine("Cannot Find Path For " + tableDefinition.Name + " Table");
            //        Console.ResetColor();
            //    }
            //}
            //catch (Exception)
            //{
            //    Console.BackgroundColor = ConsoleColor.Red;
            //    Console.WriteLine("Error While Serialize And Save Table Definition For " + tableDefinition.Name + " Table");
            //    Console.ResetColor();
            //}
        }

        public static string GetPath(string tableName)
        {
            string path = GetPathForDXMLFile(tableName);
            return path;
        }



        //private methods

        private static string GetDxmlDataType(string type)
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

        private static ColumnDefinition SetConstraintForColumnDefinition(ColumnDefinition column, string constraintType, string constraintName)
        {
            switch (constraintType)
            {
                case "PRIMARY KEY":
                    column.Constraints.PrimaryKey = true;
                    column.Constraints.PrimaryKeyConstraintName = constraintName;
                    return column;
                default:
                    return column;
            }
        }

        private static string GetPathForDXMLFile(string tableName)
        {
            List<string> excludedTables = GetExcludedTables();
            List<string> excludedTablesNames = GetExcludedTables().Select(i => i.Split('/')[0]).ToList();

            string singularTableName;

            if (excludedTablesNames.Contains(tableName))
            {
                string excludedTable = excludedTables.Where(i => i.Contains(tableName)).FirstOrDefault();
                singularTableName = String.IsNullOrEmpty(excludedTable) ? null : excludedTable.Split('/')[1];
            }
            else
            {
                PluralizationService pluralizationService = PluralizationService.CreateService(System.Globalization.CultureInfo.GetCultureInfo("en-us"));
                singularTableName = pluralizationService.Singularize(tableName);
            }

            if (String.IsNullOrEmpty(singularTableName))
            {
                return null;
            }

            string rootPath = Path.Combine(Root);
            string[] lxmlFiles = Directory.GetFiles(rootPath, singularTableName + ".lxml", SearchOption.AllDirectories);

            if (lxmlFiles.Length > 0)
            {
                string lxmlFilePath = lxmlFiles[0];
                if (lxmlFilePath.Contains(@"\EntityFiles\"))
                {
                    string lxmlFileRootPath = lxmlFilePath.Split(new string[] { @"\EntityFiles\" }, StringSplitOptions.None)[0];
                    string test = lxmlFilePath.Split(new string[] { @"\EntityFiles\" }, StringSplitOptions.None)[1];
                    string lxmlFileFolderName;
                    if (test.Contains(".lxml"))
                    {
                        lxmlFileFolderName = null;
                    }
                    else
                    {
                        lxmlFileFolderName = test.Split(new string[] { @"\" + singularTableName + ".lxml" }, StringSplitOptions.None)[0];
                    }
                     
                    string dxmlFilePath = String.IsNullOrEmpty(lxmlFileFolderName) ? lxmlFileRootPath + @"\DBTables" : lxmlFileRootPath + @"\DBTables" + @"\" + lxmlFileFolderName;
                    //if (!Directory.Exists(dxmlFilePath))
                    //{
                    //    Directory.CreateDirectory(dxmlFilePath);
                    //}

                    return dxmlFilePath + @"\" + singularTableName + ".dxml";
                }
                else
                {
                    return null;
                }
            }
            else
            {
                string[] pocoFiles = Directory.GetFiles(rootPath, singularTableName + ".cs", SearchOption.AllDirectories);
                if(pocoFiles.Length > 0)
                {
                    string pocoFilePath = pocoFiles.ToList().Where(a=> a.Contains(@"\EntityPOCOs\")).FirstOrDefault();
                    if (!String.IsNullOrEmpty(pocoFilePath))
                    {
                        string[] pocoFileFolders = pocoFilePath.Split(new string[] { @"\" }, StringSplitOptions.None);
                        string pocoFileFolderName = pocoFileFolders[Array.IndexOf(pocoFileFolders, "EntityPOCOs") - 1].Contains(".Data") ? null : pocoFileFolders[Array.IndexOf(pocoFileFolders, "EntityPOCOs") - 1];
                        string dxmlFilePath = pocoFilePath.Split(new string[] { @"\Logitude\" }, StringSplitOptions.None)[0] + @"\Logitude\Logitude.MetaData\DBTables" + (!String.IsNullOrEmpty(pocoFileFolderName) ? @"\" + pocoFileFolderName : null);
                        //if (!Directory.Exists(dxmlFilePath))
                        //{
                        //    Directory.CreateDirectory(dxmlFilePath);
                        //}

                        return dxmlFilePath + @"\" + singularTableName + ".dxml";
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

            //string path = @"D:\LogitudeMainDXMLFiles\test.dxml";
            //return path;
        }

        private static List<string> GetExcludedTables()
        {
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string filePath = Path.Combine(projectDirectory, @"ExcludedTablesMap.txt");
            return File.ReadLines(filePath).ToList();
        }


    }
}
