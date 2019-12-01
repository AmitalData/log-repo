using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Logitude.LXMLFixer.Models
{
    public class LXMLFilesFixer
    {
        private readonly string LXMLFilesRoot = ConfigurationManager.AppSettings["LXMLFilesRoot"];
        private readonly string DXMLFilesRoot = ConfigurationManager.AppSettings["DXMLFilesRoot"];

        private string LXMLMistakesData = "";
        private string DXMLFilesThatNotFound = "";

        public void ExtractLXMLFilesMistakes()
        {
            string[] lxmlFiles = GetLXMLFiles();

            if(lxmlFiles != null)
            {
                foreach (var lxmlFile in lxmlFiles)
                {
                    string lxmlFileName = Path.GetFileName(lxmlFile);

                    Console.WriteLine("Extract Mistakes For " + lxmlFileName + " ...");

                    string mistakesData = "";
                    bool appendTableMistakesData = false;
                    string entityName = lxmlFileName.Split('.')[0];

                    TableDefinition lxmlTableDefinition = GetTableDefinitionForLXMLFile(lxmlFile);
                    TableDefinition dxmlTableDefinition = GetTableDefinitionForDXMLFile(entityName);

                    if (dxmlTableDefinition != null && lxmlTableDefinition != null)
                    {
                        mistakesData += "DXML Table, LXML Table\n";
                        mistakesData += dxmlTableDefinition.Name + "," + lxmlTableDefinition.Name;
                        if (dxmlTableDefinition.Name != lxmlTableDefinition.Name)
                        {
                            mistakesData += "," + "Incorrect Table Name In LXML File";
                            appendTableMistakesData = true;
                        }

                        mistakesData += "\n";

                        mistakesData += "DXML Column,LXML Column,Attribute,DXML Value,LXML Value\n";

                        foreach (var dxmlColumn in dxmlTableDefinition.Columns)
                        {
                            ColumnDefinition lxmlColumn = lxmlTableDefinition.Columns.Where(c => c.Name == dxmlColumn.Name).FirstOrDefault();
                            if (lxmlColumn == null)
                            {
                                mistakesData += dxmlColumn.Name + ",Not Found,-,-,-" + "\n";
                                appendTableMistakesData = true;
                            }
                            else
                            {
                                if (dxmlColumn.Type != lxmlColumn.Type)
                                {
                                    mistakesData += dxmlColumn.Name + "," + lxmlColumn.Name + ",Type," + dxmlColumn.Type + "," + lxmlColumn.Type + "\n";
                                    appendTableMistakesData = true;
                                }
                                if (dxmlColumn.Size != lxmlColumn.Size)
                                {
                                    mistakesData += dxmlColumn.Name + "," + lxmlColumn.Name + ",Size," + dxmlColumn.Size + "," + lxmlColumn.Size + "\n";
                                    appendTableMistakesData = true;
                                }
                                if (dxmlColumn.Constraints.PrimaryKey != lxmlColumn.Constraints.PrimaryKey)
                                {
                                    mistakesData += dxmlColumn.Name + "," + lxmlColumn.Name + ",Primary Key," + dxmlColumn.Constraints.PrimaryKey + "," + lxmlColumn.Constraints.PrimaryKey + "\n";
                                    appendTableMistakesData = true;
                                }
                                if (dxmlColumn.Constraints.Nullable != lxmlColumn.Constraints.Nullable)
                                {
                                    mistakesData += dxmlColumn.Name + "," + lxmlColumn.Name + ",Nullable," + dxmlColumn.Constraints.Nullable + "," + lxmlColumn.Constraints.Nullable + "\n";
                                    appendTableMistakesData = true;
                                }
                            }
                        }

                        if (appendTableMistakesData)
                        {
                            LXMLMistakesData += mistakesData + "\n\n";
                        }
                    }
                    else
                    {
                        if (dxmlTableDefinition == null)
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.WriteLine("Cannot Get DXML Table Definition For " + entityName + " Entity");
                            Console.ResetColor();
                            DXMLFilesThatNotFound += entityName + ".dxml" + "\n";
                        }
                        if (lxmlTableDefinition == null)
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.WriteLine("Cannot Get LXML Table Definition For " + entityName + " Entity");
                            Console.ResetColor();
                        }
                    }

                }

                ExportMistakesData();
                ExportDXMLFilesThatNotFound();
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("Cannot Find Any LXML Files");
                Console.ResetColor();
            }
        }

        private void ExportMistakesData()
        {
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string csvFilePath = Path.Combine(projectDirectory, @"Reports\LXMLFilesMistakes.csv");
            File.WriteAllText(csvFilePath, LXMLMistakesData);
            Console.WriteLine("\nLXML Files Mistakes Extracted To /Reports/LXMLFilesMistakes.csv\n");
        }
        
        private void ExportDXMLFilesThatNotFound()
        {
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string csvFilePath = Path.Combine(projectDirectory, @"Reports\DXMLFilesThatNotFound.csv");
            File.WriteAllText(csvFilePath, DXMLFilesThatNotFound);
            Console.WriteLine("DXML Files That Not Found Extracted To /Reports/DXMLFilesThatNotFound.csv\n");
        }

        private string[] GetLXMLFiles()
        {
            try
            {
                string LXMLFilesPath = Path.Combine(LXMLFilesRoot);
                string[] LXMLFiles = Directory.GetFiles(LXMLFilesPath, "*.lxml", SearchOption.AllDirectories);
                if (LXMLFiles.Length > 0)
                {
                    return LXMLFiles;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private TableDefinition GetTableDefinitionForDXMLFile(string entityName)
        {
            try
            {
                string DXMLFilesPath = Path.Combine(DXMLFilesRoot);
                string[] DXMLFiles = Directory.GetFiles(DXMLFilesPath, entityName + ".dxml", SearchOption.AllDirectories);
                if (DXMLFiles.Length > 0)
                {
                    var dxmlFilePath = DXMLFiles[0];
                    string xmlString = File.ReadAllText(dxmlFilePath);
                    TableDefinition tableDefinition = xmlString.ParseXML<TableDefinition>();
                    return tableDefinition;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private TableDefinition GetTableDefinitionForLXMLFile(string lxmlFile)
        {
            try
            {
                List<ColumnDefinition> columnDefinitions = new List<ColumnDefinition>();

                XDocument xmlDocument = XDocument.Load(lxmlFile);
                IEnumerable<XElement> dbFiledXmlElements = xmlDocument.Descendants("field").Where(x => x.Attribute("HasDataBaseField").Value == "true");
                foreach (var field in dbFiledXmlElements)
                {
                    string name = field.Attribute("FieldName") == null ? null : field.Attribute("FieldName").Value.Split('"')[1].Split('"')[0];
                    string dataType = field.Attribute("FieldsDataType") == null ? null : field.Attribute("FieldsDataType").Value.Split('"')[1].Split('"')[0];
                    bool isMaxLength = field.Attribute("IsMaxLength") == null ? false : field.Attribute("IsMaxLength").Value == "true";
                    int size = isMaxLength ? -1 : (field.Attribute("MaxLength") == null ? 0 : Convert.ToInt32(field.Attribute("MaxLength").Value));
                    bool isPrimaryKey = field.Attribute("IsPrimaryKey") == null ? false : field.Attribute("IsPrimaryKey").Value == "true";
                    bool isRequired = field.Attribute("IsRequired") == null ? false : field.Attribute("IsRequired").Value == "true";
                    bool isNullable = field.Attribute("IsNullable") == null ? false : field.Attribute("IsNullable").Value == "true";
                    bool isFixedLength = field.Attribute("IsFixedLength") == null ? false : field.Attribute("IsFixedLength").Value == "true";

                    string columnDefinitionDataType = GetDataTypeForColumnDefinition(dataType, isFixedLength);
                    bool columnDefinitionNullableConstraint = GetIsNullableForConstraintsDefinition(isRequired, isNullable, isPrimaryKey, columnDefinitionDataType);

                    ColumnDefinition columnDefinition = new ColumnDefinition
                    {
                        Name = name,
                        Type = columnDefinitionDataType,
                        Size = size,
                        Constraints = new ConstraintsDefinition
                        {
                            PrimaryKey = isPrimaryKey,
                            Nullable = columnDefinitionNullableConstraint
                        }
                    };
                    
                    columnDefinitions.Add(columnDefinition);
                }

                string dbTableName = xmlDocument.Root.Attribute("DBTableName") == null ? null : xmlDocument.Root.Attribute("DBTableName").Value.Split('"')[1].Split('"')[0];

                TableDefinition lxmlTableDefinition = new TableDefinition
                {
                    Name = dbTableName.Contains("Customs.") ? dbTableName.Split('.')[1] : dbTableName,
                    Columns = columnDefinitions
                };

                return lxmlTableDefinition;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string GetDataTypeForColumnDefinition(string type, bool isFixedLength)
        {
            switch (type)
            {
                case "Boolean":
                    return "bit";
                case "Constant":
                case "PickList":
                case "List":
                case "Emails":
                case "Byte[]":
                case "Text":
                    return "varchar";
                case "Date":
                case "DateTime":
                    return "datetime";
                case "Decimal":
                case "UnsDecimal":
                    return "decimal";
                case "Double":
                case "SigDouble":
                    return "float";
                case "Integer":
                case "UnsInteger":
                    return "int";
                case "nText":
                    return "nvarchar";
                case "LookUp":
                    return isFixedLength ? "char" : "varchar";
                default:
                    return null;
            }
        }

        private bool GetIsNullableForConstraintsDefinition(bool isRequired, bool isNullable, bool isPrimaryKey, string columnDefinitionDataType)
        {
            if (isPrimaryKey)
            {
                return false;
            }

            if (new string[] { "bit", "datetime", "decimal", "float", "int" }.Contains(columnDefinitionDataType))
            {
                if (!isRequired && isNullable)
                {
                    return true;
                }

                return false;
            }

            return !isRequired;
        }

    }
}