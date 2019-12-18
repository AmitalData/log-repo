using LXMLFixer.Models;
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
        private string LXMLFixedMistakesData = "";

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

                        if (dxmlTableDefinition.DBType != lxmlTableDefinition.DBType)
                        {
                            mistakesData += "DXML DBType, LXML DBType\n";
                            mistakesData += dxmlTableDefinition.DBType + "," + lxmlTableDefinition.DBType + "\n";
                            appendTableMistakesData = true;
                        }

                        if (dxmlTableDefinition.Schema != lxmlTableDefinition.Schema)
                        {
                            mistakesData += "DXML Schema, LXML Schema\n";
                            mistakesData += dxmlTableDefinition.Schema + "," + lxmlTableDefinition.Schema + "\n";
                            appendTableMistakesData = true;
                        }

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
                                if (dxmlColumn.Type == "decimal" && lxmlColumn.Type == "decimal" && dxmlColumn.Precision != lxmlColumn.Precision)
                                {
                                    mistakesData += dxmlColumn.Name + "," + lxmlColumn.Name + ",Precision," + dxmlColumn.Precision + "," + lxmlColumn.Precision + "\n";
                                    appendTableMistakesData = true;
                                }
                                if (dxmlColumn.Type == "decimal" && lxmlColumn.Type == "decimal" && dxmlColumn.Scale != lxmlColumn.Scale)
                                {
                                    mistakesData += dxmlColumn.Name + "," + lxmlColumn.Name + ",Scale," + dxmlColumn.Scale + "," + lxmlColumn.Scale + "\n";
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

        public void FixLXMLFilesMistakes()
        {
            string[] lxmlFiles = GetLXMLFiles();

            if (lxmlFiles != null)
            {
                foreach (var lxmlFile in lxmlFiles)
                {
                    string lxmlFileName = Path.GetFileName(lxmlFile);

                    Console.WriteLine("Fix Mistakes For " + lxmlFileName + " ...");

                    string entityName = lxmlFileName.Split('.')[0];

                    TableDefinition lxmlTableDefinition = GetTableDefinitionForLXMLFile(lxmlFile);
                    TableDefinition dxmlTableDefinition = GetTableDefinitionForDXMLFile(entityName);

                    List<LXMLAttribute> attributes = new List<LXMLAttribute>();

                    if (dxmlTableDefinition != null && lxmlTableDefinition != null)
                    {
                        if (dxmlTableDefinition.Name != lxmlTableDefinition.Name)
                        {
                            LXMLAttribute attribute = new LXMLAttribute
                            {
                                ElementName = "entity",
                                AttributeName = "DBTableName",
                                AttributeValue = GetStringValue(dxmlTableDefinition.Schema.ToLower() == "customs" ? "Customs." + dxmlTableDefinition.Name : dxmlTableDefinition.Name),
                                OldAttributeValue = lxmlTableDefinition.Name,
                                AttributeFilter = null
                            };

                            attributes.Add(attribute);
                        }

                        if(dxmlTableDefinition.DBType != lxmlTableDefinition.DBType)
                        {
                            LXMLAttribute attribute = new LXMLAttribute
                            {
                                ElementName = "entity",
                                AttributeName = "DxmlDatabaseTypeCode",
                                AttributeValue = dxmlTableDefinition.DBType,
                                OldAttributeValue = lxmlTableDefinition.DBType,
                                AttributeFilter = null
                            };

                            attributes.Add(attribute);
                        }

                        if (dxmlTableDefinition.Schema != lxmlTableDefinition.Schema)
                        {
                            LXMLAttribute attribute = new LXMLAttribute
                            {
                                ElementName = "entity",
                                AttributeName = "DxmlDatabaseSchemaCode",
                                AttributeValue = dxmlTableDefinition.Schema,
                                OldAttributeValue = lxmlTableDefinition.Schema,
                                AttributeFilter = null
                            };

                            attributes.Add(attribute);
                        }

                        foreach (var dxmlColumn in dxmlTableDefinition.Columns)
                        {
                            ColumnDefinition lxmlColumn = lxmlTableDefinition.Columns.Where(c => c.Name == dxmlColumn.Name).FirstOrDefault();

                            if (lxmlColumn == null)
                            {

                            }
                            else
                            {
                                if (dxmlColumn.Type != lxmlColumn.Type)
                                {
                                    LXMLAttribute attribute = new LXMLAttribute
                                    {
                                        ElementName = "field",
                                        AttributeName = "FieldsDataType",
                                        AttributeValue = GetStringValue(GetLXMLDataType(dxmlColumn.Type)),
                                        OldAttributeValue = lxmlColumn.Type,
                                        AttributeFilter = new LXMLAttributeFilter
                                        {
                                            Name = "FieldName",
                                            Value = GetStringValue(dxmlColumn.Name)
                                        }
                                    };

                                    attributes.Add(attribute);
                                }

                                if (dxmlColumn.Type == "decimal" && lxmlColumn.Type == "decimal" && dxmlColumn.Precision != lxmlColumn.Precision)
                                {
                                    LXMLAttribute attribute = new LXMLAttribute
                                    {
                                        ElementName = "field",
                                        AttributeName = "NumberOfDigits",
                                        AttributeValue = dxmlColumn.Precision.ToString(),
                                        OldAttributeValue = lxmlColumn.Precision.ToString(),
                                        AttributeFilter = new LXMLAttributeFilter
                                        {
                                            Name = "FieldName",
                                            Value = GetStringValue(lxmlColumn.Name)
                                        }
                                    };

                                    attributes.Add(attribute);
                                }

                                if (dxmlColumn.Type == "decimal" && lxmlColumn.Type == "decimal" && dxmlColumn.Scale != lxmlColumn.Scale)
                                {
                                    LXMLAttribute attribute = new LXMLAttribute
                                    {
                                        ElementName = "field",
                                        AttributeName = "DigitsAfterPoint",
                                        AttributeValue = dxmlColumn.Scale.ToString(),
                                        OldAttributeValue = lxmlColumn.Scale.ToString(),
                                        AttributeFilter = new LXMLAttributeFilter
                                        {
                                            Name = "FieldName",
                                            Value = GetStringValue(lxmlColumn.Name)
                                        }
                                    };

                                    attributes.Add(attribute);
                                }

                                if (dxmlColumn.Size != lxmlColumn.Size)
                                {
                                    if (dxmlColumn.Size == -1 || lxmlColumn.Size == -1)
                                    {
                                        LXMLAttribute attribute = new LXMLAttribute
                                        {
                                            ElementName = "field",
                                            AttributeName = "IsMaxLength",
                                            AttributeValue = dxmlColumn.Size == -1 ? "true" : "false",
                                            OldAttributeValue = lxmlColumn.Size == -1 ? "true" : "false",
                                            AttributeFilter = new LXMLAttributeFilter
                                            {
                                                Name = "FieldName",
                                                Value = GetStringValue(lxmlColumn.Name)
                                            }
                                        };

                                        attributes.Add(attribute);
                                    }
                                    else
                                    {
                                        LXMLAttribute attribute = new LXMLAttribute
                                        {
                                            ElementName = "field",
                                            AttributeName = "MaxLength",
                                            AttributeValue = dxmlColumn.Size.ToString(),
                                            OldAttributeValue = lxmlColumn.Size.ToString(),
                                            AttributeFilter = new LXMLAttributeFilter
                                            {
                                                Name = "FieldName",
                                                Value = GetStringValue(lxmlColumn.Name)
                                            }
                                        };

                                        attributes.Add(attribute);
                                    }
                                }

                                if (dxmlColumn.Constraints.PrimaryKey != lxmlColumn.Constraints.PrimaryKey)
                                {
                                    LXMLAttribute attribute = new LXMLAttribute
                                    {
                                        ElementName = "field",
                                        AttributeName = "IsPrimaryKey",
                                        AttributeValue = dxmlColumn.Constraints.PrimaryKey ? "true" : "false",
                                        OldAttributeValue = lxmlColumn.Constraints.PrimaryKey ? "true" : "false",
                                        AttributeFilter = new LXMLAttributeFilter
                                        {
                                            Name = "FieldName",
                                            Value = GetStringValue(lxmlColumn.Name)
                                        }
                                    };

                                    attributes.Add(attribute);
                                }

                                if (dxmlColumn.Constraints.Nullable != lxmlColumn.Constraints.Nullable)
                                {
                                    if (dxmlColumn.Constraints.Nullable)
                                    {
                                        if (lxmlColumn.Constraints.PrimaryKey)
                                        {
                                            LXMLAttribute attribute = new LXMLAttribute
                                            {
                                                ElementName = "field",
                                                AttributeName = "IsPrimaryKey",
                                                AttributeValue = "false",
                                                OldAttributeValue = "true",
                                                AttributeFilter = new LXMLAttributeFilter
                                                {
                                                    Name = "FieldName",
                                                    Value = GetStringValue(lxmlColumn.Name)
                                                }
                                            };

                                            attributes.Add(attribute);
                                        }
                                        else if (new string[] { "bit", "datetime", "decimal", "float", "int" }.Contains(lxmlColumn.Type))
                                        {
                                            LXMLAttribute attribute = new LXMLAttribute
                                            {
                                                ElementName = "field",
                                                AttributeName = "IsRequired",
                                                AttributeValue = "false",
                                                OldAttributeValue = "true",
                                                AttributeFilter = new LXMLAttributeFilter
                                                {
                                                    Name = "FieldName",
                                                    Value = GetStringValue(lxmlColumn.Name)
                                                }
                                            };

                                            attributes.Add(attribute);

                                            LXMLAttribute attribute2 = new LXMLAttribute
                                            {
                                                ElementName = "field",
                                                AttributeName = "IsNullable",
                                                AttributeValue = "true",
                                                OldAttributeValue = "false",
                                                AttributeFilter = new LXMLAttributeFilter
                                                {
                                                    Name = "FieldName",
                                                    Value = GetStringValue(lxmlColumn.Name)
                                                }
                                            };

                                            attributes.Add(attribute2);
                                        }
                                        else
                                        {
                                            LXMLAttribute attribute = new LXMLAttribute
                                            {
                                                ElementName = "field",
                                                AttributeName = "IsRequired",
                                                AttributeValue = "false",
                                                OldAttributeValue = "true",
                                                AttributeFilter = new LXMLAttributeFilter
                                                {
                                                    Name = "FieldName",
                                                    Value = GetStringValue(lxmlColumn.Name)
                                                }
                                            };

                                            attributes.Add(attribute);
                                        }
                                    }
                                    else
                                    {
                                        if (new string[] { "bit", "datetime", "decimal", "float", "int" }.Contains(lxmlColumn.Type))
                                        {
                                            LXMLAttribute attribute = new LXMLAttribute
                                            {
                                                ElementName = "field",
                                                AttributeName = "IsRequired",
                                                AttributeValue = "true",
                                                OldAttributeValue = "false",
                                                AttributeFilter = new LXMLAttributeFilter
                                                {
                                                    Name = "FieldName",
                                                    Value = GetStringValue(lxmlColumn.Name)
                                                }
                                            };

                                            attributes.Add(attribute);

                                            LXMLAttribute attribute2 = new LXMLAttribute
                                            {
                                                ElementName = "field",
                                                AttributeName = "IsNullable",
                                                AttributeValue = "false",
                                                OldAttributeValue = "true",
                                                AttributeFilter = new LXMLAttributeFilter
                                                {
                                                    Name = "FieldName",
                                                    Value = GetStringValue(lxmlColumn.Name)
                                                }
                                            };

                                            attributes.Add(attribute2);
                                        }
                                        else
                                        {
                                            LXMLAttribute attribute = new LXMLAttribute
                                            {
                                                ElementName = "field",
                                                AttributeName = "IsRequired",
                                                AttributeValue = "true",
                                                OldAttributeValue = "false",
                                                AttributeFilter = new LXMLAttributeFilter
                                                {
                                                    Name = "FieldName",
                                                    Value = GetStringValue(lxmlColumn.Name)
                                                }
                                            };

                                            attributes.Add(attribute);
                                        }
                                    }
                                }
                            }
                        }

                        LXMLFileFixer lxmlFileFixer = new LXMLFileFixer
                        {
                            FilePath = lxmlFile,
                            Attributes = attributes
                        };

                        FixLXMLFile(lxmlFileFixer);
                    }
                    else
                    {
                        if (dxmlTableDefinition == null)
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.WriteLine("Cannot Get DXML Table Definition For " + entityName + " Entity");
                            Console.ResetColor();
                        }
                        if (lxmlTableDefinition == null)
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.WriteLine("Cannot Get LXML Table Definition For " + entityName + " Entity");
                            Console.ResetColor();
                        }
                    }
                }

                ExportFixedMistakesData();
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

        private void ExportFixedMistakesData()
        {
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string csvFilePath = Path.Combine(projectDirectory, @"Reports\LXMLFilesFixedMistakes.csv");
            File.WriteAllText(csvFilePath, LXMLFixedMistakesData);
            Console.WriteLine("\nFixed LXML Files Mistakes Extracted To /Reports/LXMLFilesFixedMistakes.csv\n");
        }

        private string[] GetLXMLFiles()
        {
            try
            {
                string LXMLFilesPath = Path.Combine(LXMLFilesRoot);
                string[] LXMLFiles = Directory.GetFiles(LXMLFilesPath, "*.lxml", SearchOption.AllDirectories);

                LXMLFiles = LXMLFiles.Where(x => !x.ToLower().Contains("logitude.customs.metadata")).ToArray();

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
                    int numberOfDigits = field.Attribute("NumberOfDigits") == null ? 0 : Convert.ToInt32(field.Attribute("NumberOfDigits").Value);
                    int digitsAfterPoint = field.Attribute("DigitsAfterPoint") == null ? 0 : Convert.ToInt32(field.Attribute("DigitsAfterPoint").Value);

                    string columnDefinitionDataType = GetColumnDefinitionDataType(dataType, isFixedLength);
                    bool columnDefinitionNullableConstraint = GetNullableForConstraintsDefinition(isRequired, isNullable, isPrimaryKey, columnDefinitionDataType);
                    
                    ColumnDefinition columnDefinition = new ColumnDefinition
                    {
                        Name = name,
                        Type = columnDefinitionDataType,
                        Size = size,
                        Precision = numberOfDigits,
                        Scale = digitsAfterPoint,
                        Constraints = new ConstraintsDefinition
                        {
                            PrimaryKey = isPrimaryKey,
                            Nullable = columnDefinitionNullableConstraint
                        }
                    };
                    
                    columnDefinitions.Add(columnDefinition);
                }

                string dbTableName = xmlDocument.Root.Attribute("DBTableName") == null ? null : xmlDocument.Root.Attribute("DBTableName").Value.Split('"')[1].Split('"')[0];
                string dbType = xmlDocument.Root.Attribute("DxmlDatabaseTypeCode") == null ? null : xmlDocument.Root.Attribute("DxmlDatabaseTypeCode").Value;
                string dbSchema = xmlDocument.Root.Attribute("DxmlDatabaseSchemaCode") == null ? null : xmlDocument.Root.Attribute("DxmlDatabaseSchemaCode").Value;

                TableDefinition lxmlTableDefinition = new TableDefinition
                {
                    Name = dbTableName.Contains("Customs.") ? dbTableName.Split('.')[1] : dbTableName,
                    Schema = dbSchema,
                    DBType = dbType,
                    Columns = columnDefinitions
                };

                return lxmlTableDefinition;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string GetColumnDefinitionDataType(string type, bool isFixedLength)
        {
            switch (type)
            {
                case "Boolean":
                    return "bit";
                case "Constant":
                case "PickList":
                case "List":
                case "Byte[]":
                    return "varchar";
                case "Date":
                    return "date";
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
                case "Emails":
                case "Text":
                    return isFixedLength ? "char" : "varchar";
                default:
                    return null;
            }
        }

        private string GetLXMLDataType(string type)//should handle any new data types that added into the lxml tool
        {
            switch (type)
            {
                case "int":
                    return "Integer";
                case "decimal":
                    return "Decimal";
                case "timestamp":
                    return "Text";
                case "varbinary":
                    return "Text";
                case "varchar":
                    return "Text";
                case "datetime":
                    return "DateTime";
                case "time":
                    return "DateTime";
                case "float":
                    return "Double";
                case "char":
                    return "LookUp";
                case "bigint":
                    return "Integer";
                case "nvarchar":
                    return "nText";
                case "bit":
                    return "Boolean";
                default:
                    return null;
            }
        }

        private bool GetNullableForConstraintsDefinition(bool isRequired, bool isNullable, bool isPrimaryKey, string columnDefinitionDataType)
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

        private void FixLXMLFile(LXMLFileFixer lxmlFileFixer)
        {
            if(lxmlFileFixer.Attributes.Count() > 0)
            {
                XDocument doc = XDocument.Load(lxmlFileFixer.FilePath);

                string fixedMistakesData = "LXML File: " + Path.GetFileName(lxmlFileFixer.FilePath) + "\n";
                fixedMistakesData += "Element,Attribute,Old Value,New Value\n";

                foreach (var attr in lxmlFileFixer.Attributes)
                {
                    if (!String.IsNullOrEmpty(attr.AttributeValue))
                    {
                        XElement element;

                        if (attr.AttributeFilter == null)
                        {
                            element = doc.Descendants(attr.ElementName).Single();
                        }
                        else
                        {
                            element = doc.Descendants(attr.ElementName).Where(x => x.Attribute(attr.AttributeFilter.Name).Value == attr.AttributeFilter.Value).Single();
                        }

                        element.SetAttributeValue(attr.AttributeName, attr.AttributeValue);

                        string elementText = attr.ElementName;
                        if (attr.AttributeFilter != null)
                        {
                            elementText += "[" + attr.AttributeFilter.Name + "='" + attr.AttributeFilter.Value + "']";
                        }

                        fixedMistakesData += elementText + "," + attr.AttributeName + "," + attr.OldAttributeValue + "," + attr.AttributeValue + "\n";
                    }
                }

                if(lxmlFileFixer.Attributes.Where(a => !String.IsNullOrEmpty(a.AttributeValue)).Any())
                {
                    LXMLFixedMistakesData += fixedMistakesData;
                }

                doc.Save(lxmlFileFixer.FilePath);
            }
        }

        private string GetStringValue(object value)
        {
            if(value != null)
            {
                return "\"" + value.ToString().Replace("\"", "\u0022") + "\"";
            }
            else
            {
                return null;
            }
        }
    }
}