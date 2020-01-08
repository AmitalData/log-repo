using LXMLFixer.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Logitude.LXMLFixer.Models
{
    public class LXMLFilesFixer
    {
        private string LXMLFilesRoot;
        private string DXMLFilesRoot;
        private string ModuleName;

        private string LXMLMistakesData = "DXML File,LXML File,Element Type,DXML Element Name,LXML Element Name,Attribute Type,DXML Attribute Value,LXML Attribute Value\n";
        private string LXMLIgnoredMistakesData = "DXML File,LXML File,Element Type,DXML Element Name,LXML Element Name,Attribute Type,DXML Attribute Value,LXML Attribute Value\n";
        private string DXMLFilesThatNotFound = "";
        private string LXMLFixedMistakesData = "";

        private List<string> ExcludedTables;
        private List<string> ExcludedTablesNames;

        public LXMLFilesFixer(int moduleNumber)
        {
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            ModuleName = GetModuleName(moduleNumber);
            if(ModuleName == "AllModules")
            {
                LXMLFilesRoot = projectDirectory.Split(new string[] { @"\Logitude\" }, StringSplitOptions.None)[0] + @"\Logitude";
                DXMLFilesRoot = projectDirectory.Split(new string[] { @"\Logitude\" }, StringSplitOptions.None)[0] + @"\Logitude";
            }
            else
            {
                LXMLFilesRoot = projectDirectory.Split(new string[] { @"\Logitude\" }, StringSplitOptions.None)[0] + ConfigurationManager.AppSettings[ModuleName + "_LXMLFilesRoot"];
                DXMLFilesRoot = projectDirectory.Split(new string[] { @"\Logitude\" }, StringSplitOptions.None)[0] + ConfigurationManager.AppSettings[ModuleName + "_DXMLFilesRoot"];
            }
        }

        public void ExtractLXMLFilesMistakes()
        {
            string[] lxmlFiles = GetLXMLFiles(false);

            if(lxmlFiles != null)
            {
                foreach (var lxmlFile in lxmlFiles)
                {
                    string lxmlFileName = Path.GetFileName(lxmlFile);
                    string entityName = lxmlFileName.Split('.')[0];
                    string dxmlFileName = entityName + ".dxml";

                    Console.WriteLine("Extract Mistakes For " + lxmlFileName + " ...");

                    TableDefinition lxmlTableDefinition = GetTableDefinitionForLXMLFile(lxmlFile);
                    TableDefinition dxmlTableDefinition = GetTableDefinitionForDXMLFile(entityName);

                    if (dxmlTableDefinition != null && lxmlTableDefinition != null)
                    {
                        if (dxmlTableDefinition.Name != lxmlTableDefinition.Name)
                        {
                            LXMLMistakesData += dxmlFileName + "," + lxmlFileName + ",Table," + dxmlTableDefinition.Name + "," + lxmlTableDefinition.Name + ",Name," + dxmlTableDefinition.Name + "," + lxmlTableDefinition.Name + "\n";
                        }

                        if (dxmlTableDefinition.DBType != lxmlTableDefinition.DBType)
                        {
                            LXMLMistakesData += dxmlFileName + "," + lxmlFileName + ",Table," + dxmlTableDefinition.Name + "," + lxmlTableDefinition.Name + ",DBType," + dxmlTableDefinition.DBType + "," + lxmlTableDefinition.DBType + "\n";
                        }

                        if (dxmlTableDefinition.Schema != lxmlTableDefinition.Schema)
                        {
                            LXMLMistakesData += dxmlFileName + "," + lxmlFileName + ",Table," + dxmlTableDefinition.Name + "," + lxmlTableDefinition.Name + ",Schema," + dxmlTableDefinition.Schema + "," + lxmlTableDefinition.Schema + "\n";
                        }

                        foreach (var dxmlColumn in dxmlTableDefinition.Columns)
                        {
                            ColumnDefinition lxmlColumn = lxmlTableDefinition.Columns.Where(c => c.Name == dxmlColumn.Name).FirstOrDefault();

                            if (lxmlColumn == null)
                            {
                                LXMLMistakesData += dxmlFileName + "," + lxmlFileName + ",Column," + dxmlColumn.Name + "," + "NULL" + ",Name," + dxmlColumn.Name + "," + "NULL" + "\n";

                                if (dxmlTableDefinition.Relations.Where(r => (r.ForeignKeyColumn == dxmlColumn.Name && !r.ForeignKeyColumn.Contains(",")) || (r.ForeignKeyColumn.Split(',').Contains(dxmlColumn.Name) && r.ForeignKeyColumn.Contains(","))).Any())
                                {
                                    LXMLMistakesData += dxmlFileName + "," + lxmlFileName + ",Column," + dxmlColumn.Name + "," + "NULL" + ",Foreign Key," + "TRUE" + "," + "NULL" + "\n";
                                }
                            }
                            else
                            {
                                if (dxmlColumn.Type != lxmlColumn.Type)
                                {
                                    LXMLMistakesData += dxmlFileName + "," + lxmlFileName + ",Column," + dxmlColumn.Name + "," + lxmlColumn.Name + ",Type," + dxmlColumn.Type + "," + lxmlColumn.Type + "\n";
                                }

                                if (dxmlColumn.Type == "decimal" && lxmlColumn.Type == "decimal" && dxmlColumn.Precision != lxmlColumn.Precision)
                                {
                                    LXMLMistakesData += dxmlFileName + "," + lxmlFileName + ",Column," + dxmlColumn.Name + "," + lxmlColumn.Name + ",Precision," + dxmlColumn.Precision + "," + lxmlColumn.Precision + "\n";
                                }

                                if (dxmlColumn.Type == "decimal" && lxmlColumn.Type == "decimal" && dxmlColumn.Scale != lxmlColumn.Scale)
                                {
                                    LXMLMistakesData += dxmlFileName + "," + lxmlFileName + ",Column," + dxmlColumn.Name + "," + lxmlColumn.Name + ",Scale," + dxmlColumn.Scale + "," + lxmlColumn.Scale + "\n";
                                }

                                if (dxmlColumn.Size != lxmlColumn.Size)
                                {
                                    LXMLMistakesData += dxmlFileName + "," + lxmlFileName + ",Column," + dxmlColumn.Name + "," + lxmlColumn.Name + ",Size," + dxmlColumn.Size + "," + lxmlColumn.Size + "\n";
                                }

                                if (dxmlColumn.Constraints.PrimaryKey != lxmlColumn.Constraints.PrimaryKey)
                                {
                                    LXMLMistakesData += dxmlFileName + "," + lxmlFileName + ",Column," + dxmlColumn.Name + "," + lxmlColumn.Name + ",Primary Key," + dxmlColumn.Constraints.PrimaryKey + "," + lxmlColumn.Constraints.PrimaryKey + "\n";
                                }

                                if (dxmlColumn.Constraints.Nullable != lxmlColumn.Constraints.Nullable)
                                {
                                    LXMLMistakesData += dxmlFileName + "," + lxmlFileName + ",Column," + dxmlColumn.Name + "," + lxmlColumn.Name + ",Nullable," + dxmlColumn.Constraints.Nullable + "," + lxmlColumn.Constraints.Nullable + "\n";
                                }

                                if(dxmlTableDefinition.Relations.Where(r => (r.ForeignKeyColumn == dxmlColumn.Name && !r.ForeignKeyColumn.Contains(",")) || (r.ForeignKeyColumn.Split(',').Contains(dxmlColumn.Name) && r.ForeignKeyColumn.Contains(","))).Any() && !lxmlColumn.Constraints.ForeignKey)
                                {
                                    LXMLMistakesData += dxmlFileName + "," + lxmlFileName + ",Column," + dxmlColumn.Name + "," + lxmlColumn.Name + ",Foreign Key," + "TRUE" + "," + "FALSE" + "\n";
                                }

                                if (!dxmlTableDefinition.Relations.Where(r => (r.ForeignKeyColumn == dxmlColumn.Name && !r.ForeignKeyColumn.Contains(",")) || (r.ForeignKeyColumn.Split(',').Contains(dxmlColumn.Name) && r.ForeignKeyColumn.Contains(","))).Any() && lxmlColumn.Constraints.ForeignKey)
                                {
                                    LXMLMistakesData += dxmlFileName + "," + lxmlFileName + ",Column," + dxmlColumn.Name + "," + lxmlColumn.Name + ",Foreign Key," + "FALSE" + "," + "TRUE" + "\n";
                                }
                            }
                        }

                        foreach(var dxmlRelation in dxmlTableDefinition.Relations)
                        {
                            RelationDefinition lxmlRelation = lxmlTableDefinition.Relations.Where(r => r.ForeignKeyColumn == dxmlRelation.ForeignKeyColumn).FirstOrDefault();

                            if(lxmlRelation == null)
                            {
                                LXMLMistakesData += dxmlFileName + "," + lxmlFileName + ",Relation," + dxmlRelation.ForeignKeyColumn.Replace(",","/") + "," + "NULL" + ",Foreign Key Column," + dxmlRelation.ForeignKeyColumn.Replace(",", "/") + "," + "NULL" + "\n";
                            }
                            else
                            {
                                if(dxmlRelation.ReferencedTable != lxmlRelation.ReferencedTable)
                                {
                                    LXMLMistakesData += dxmlFileName + "," + lxmlFileName + ",Relation," + dxmlRelation.ForeignKeyColumn.Replace(",", "/") + "," + lxmlRelation.ForeignKeyColumn.Replace(",", "/") + ",Referenced Table," + dxmlRelation.ReferencedTable + "," + lxmlRelation.ReferencedTable + "\n";
                                }

                                if (dxmlRelation.ReferencedTableSchema != lxmlRelation.ReferencedTableSchema)
                                {
                                    LXMLMistakesData += dxmlFileName + "," + lxmlFileName + ",Relation," + dxmlRelation.ForeignKeyColumn.Replace(",", "/") + "," + lxmlRelation.ForeignKeyColumn.Replace(",", "/") + ",Referenced Table Schema," + dxmlRelation.ReferencedTableSchema + "," + lxmlRelation.ReferencedTableSchema + "\n";
                                }

                                if (dxmlRelation.ReferencedColumn != lxmlRelation.ReferencedColumn)
                                {
                                    LXMLMistakesData += dxmlFileName + "," + lxmlFileName + ",Relation," + dxmlRelation.ForeignKeyColumn.Replace(",", "/") + "," + lxmlRelation.ForeignKeyColumn.Replace(",", "/") + ",Referenced Column," + dxmlRelation.ReferencedColumn.Replace(",", "/") + "," + lxmlRelation.ReferencedColumn.Replace(",", "/") + "\n";
                                }
                            }
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
            BuildExcludedTablesList();

            string[] lxmlFiles = GetLXMLFiles(true);

            if (lxmlFiles != null)
            {
                foreach (var lxmlFile in lxmlFiles)
                {
                    string lxmlFileName = Path.GetFileName(lxmlFile);
                    string entityName = lxmlFileName.Split('.')[0];
                    string dxmlFileName = entityName + ".dxml";

                    Console.WriteLine("Fix Mistakes For " + lxmlFileName + " ...");

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
                                LXMLIgnoredMistakesData += dxmlFileName + "," + lxmlFileName + "," + "Column" + "," + dxmlColumn.Name + "," + "NULL" + "," + "Name" + "," + dxmlColumn.Name + "," + "Null" + "\n";
                            }
                            else
                            {
                                if (dxmlColumn.Type != lxmlColumn.Type)
                                {
                                    if(dxmlColumn.Type == "char" || dxmlColumn.Type == "varchar")
                                    {
                                        LXMLIgnoredMistakesData += dxmlFileName + "," + lxmlFileName + "," + "Column" + "," + dxmlColumn.Name + "," + lxmlColumn.Name + "," + "Type" + "," + dxmlColumn.Type + "," + lxmlColumn.Type + "\n";
                                    }
                                    else
                                    {
                                        LXMLAttribute attribute = new LXMLAttribute
                                        {
                                            ElementName = "field",
                                            AttributeName = "FieldsDataType",
                                            AttributeValue = GetStringValue(GetLXMLDataType(dxmlColumn.Type)),
                                            OldAttributeValue = GetStringValue(lxmlColumn.Type),
                                            AttributeFilter = new LXMLAttributeFilter
                                            {
                                                Name = "FieldName",
                                                Value = GetStringValue(dxmlColumn.Name)
                                            }
                                        };

                                        attributes.Add(attribute);
                                    }
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

                                var dxmlRelations = dxmlTableDefinition.Relations.Where(r => (r.ForeignKeyColumn == dxmlColumn.Name && !r.ForeignKeyColumn.Contains(",")) || (r.ForeignKeyColumn.Split(',').Contains(dxmlColumn.Name) && r.ForeignKeyColumn.Contains(",")));
                                
                                if (dxmlRelations.Any() && !lxmlColumn.Constraints.ForeignKey)
                                {
                                    LXMLAttribute attribute = new LXMLAttribute
                                    {
                                        ElementName = "field",
                                        AttributeName = "IsForeignKey",
                                        AttributeValue = "true",
                                        OldAttributeValue = "false",
                                        AttributeFilter = new LXMLAttributeFilter
                                        {
                                            Name = "FieldName",
                                            Value = GetStringValue(lxmlColumn.Name)
                                        }
                                    };

                                    attributes.Add(attribute);

                                    string foreignEntity = GetForeignEntityFromDBTable(dxmlRelations.First().ReferencedTable);
                                    string navigationPropertyName = GetNavigationPropertyNameFromDBTable(dxmlTableDefinition.Name, foreignEntity, lxmlColumn.Name);

                                    if(foreignEntity != null)
                                    {
                                        LXMLAttribute attribute2 = new LXMLAttribute
                                        {
                                            ElementName = "field",
                                            AttributeName = "ForeignEntity",
                                            AttributeValue = foreignEntity,
                                            OldAttributeValue = "NULL",
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
                                        LXMLIgnoredMistakesData += dxmlFileName + "," + lxmlFileName + "," + "Column" + "," + dxmlColumn.Name + "," + lxmlColumn.Name + "," + "Foreign Entity" + "," + "From Table " + dxmlRelations.First().ReferencedTable + "," + "NULL" + "\n";
                                    }

                                    if(navigationPropertyName != null)
                                    {
                                        LXMLAttribute attribute3 = new LXMLAttribute
                                        {
                                            ElementName = "field",
                                            AttributeName = "NavigationPropertyName",
                                            AttributeValue = navigationPropertyName,
                                            OldAttributeValue = "NULL",
                                            AttributeFilter = new LXMLAttributeFilter
                                            {
                                                Name = "FieldName",
                                                Value = GetStringValue(lxmlColumn.Name)
                                            }
                                        };

                                        attributes.Add(attribute3);
                                    }
                                    else
                                    {
                                        LXMLIgnoredMistakesData += dxmlFileName + "," + lxmlFileName + "," + "Column" + "," + dxmlColumn.Name + "," + lxmlColumn.Name + "," + "Navigation Property Name" + "," + "From Table " + dxmlTableDefinition.Name + "," + "NULL" + "\n";
                                    }
                                }

                                if (!dxmlRelations.Any() && lxmlColumn.Constraints.ForeignKey)
                                {
                                    LXMLAttribute attribute = new LXMLAttribute
                                    {
                                        ElementName = "field",
                                        AttributeName = "IsForeignKey",
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
                                        AttributeName = "ForeignEntity",
                                        AttributeValue = null,
                                        OldAttributeValue = "Not Determined",
                                        AttributeFilter = new LXMLAttributeFilter
                                        {
                                            Name = "FieldName",
                                            Value = GetStringValue(lxmlColumn.Name)
                                        }
                                    };

                                    attributes.Add(attribute2);

                                    LXMLAttribute attribute3 = new LXMLAttribute
                                    {
                                        ElementName = "field",
                                        AttributeName = "NavigationPropertyName",
                                        AttributeValue = null,
                                        OldAttributeValue = "Not Determined",
                                        AttributeFilter = new LXMLAttributeFilter
                                        {
                                            Name = "FieldName",
                                            Value = GetStringValue(lxmlColumn.Name)
                                        }
                                    };

                                    attributes.Add(attribute3);
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
                ExportIgnoredMistakesData();
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("Cannot Find Any LXML Files");
                Console.ResetColor();
            }
        }

        private string[] GetLXMLFiles(bool exceptCustomsModule)
        {
            try
            {
                string lxmlFilesPath = Path.Combine(LXMLFilesRoot);
                string[] lxmlFiles = Directory.GetFiles(lxmlFilesPath, "*.lxml", SearchOption.AllDirectories);
                
                if (exceptCustomsModule)
                {
                    lxmlFiles = lxmlFiles.Where(x => !x.ToLower().Contains("logitude.customs.metadata")).ToArray();
                }

                if (lxmlFiles.Length > 0)
                {
                    return lxmlFiles;
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
                List<RelationDefinition> relationDefinitions = new List<RelationDefinition>();

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

                    bool isForeignKey = field.Attribute("IsForeignKey") == null ? false : field.Attribute("IsForeignKey").Value == "true";

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
                            ForeignKey = isForeignKey,
                            Nullable = columnDefinitionNullableConstraint
                        }
                    };
                    
                    columnDefinitions.Add(columnDefinition);
                }

                List<string> processedForeignKeyFields = new List<string>();
                IEnumerable<XElement> dbForeignKeyFiledXmlElements = xmlDocument.Descendants("field").Where(x => x.Attribute("HasDataBaseField").Value == "true" && x.Attribute("IsForeignKey") != null && x.Attribute("IsForeignKey").Value == "true");
                foreach(var field in dbForeignKeyFiledXmlElements)
                {
                    if (!processedForeignKeyFields.Contains(field.Attribute("FieldName").Value.Split('"')[1].Split('"')[0]))
                    {
                        string fieldNavigationPropertyName = field.Attribute("NavigationPropertyName") == null ? null : field.Attribute("NavigationPropertyName").Value;

                        string[] filedsWithSameNavigationPropertyName = dbForeignKeyFiledXmlElements.Where(x => x.Attribute("NavigationPropertyName") != null && x.Attribute("NavigationPropertyName").Value == fieldNavigationPropertyName && fieldNavigationPropertyName != null).Select(x => x.Attribute("FieldName").Value.Split('"')[1].Split('"')[0]).ToArray();

                        string foreignKeyColumn = filedsWithSameNavigationPropertyName.Length == 1 ? filedsWithSameNavigationPropertyName[0] : string.Join(",", filedsWithSameNavigationPropertyName);

                        string foreignEntity = field.Attribute("ForeignEntity") == null ? null : field.Attribute("ForeignEntity").Value;

                        ForeignEntityData foreignEntityData = GetForeignEntityData(foreignEntity);

                        if (foreignEntityData != null)
                        {
                            RelationDefinition relationDefinition = new RelationDefinition
                            {
                                ForeignKeyColumn = foreignKeyColumn,
                                ReferencedTable = foreignEntityData.ReferencedTable,
                                ReferencedColumn = foreignEntityData.ReferencedColumn,
                                ReferencedTableSchema = foreignEntityData.ReferencedTableSchema
                            };

                            relationDefinitions.Add(relationDefinition);
                        }

                        foreach (string fieldName in filedsWithSameNavigationPropertyName)
                        {
                            processedForeignKeyFields.Add(fieldName);
                        }
                    }
                }
                
                string dbTableName = xmlDocument.Root.Attribute("DBTableName") == null ? null : xmlDocument.Root.Attribute("DBTableName").Value.Split('"')[1].Split('"')[0];
                string dbType = xmlDocument.Root.Attribute("DxmlDatabaseTypeCode") == null ? null : xmlDocument.Root.Attribute("DxmlDatabaseTypeCode").Value;
                string dbSchema = xmlDocument.Root.Attribute("DxmlDatabaseSchemaCode") == null ? null : xmlDocument.Root.Attribute("DxmlDatabaseSchemaCode").Value;

                TableDefinition lxmlTableDefinition = new TableDefinition
                {
                    Name = dbTableName.Contains("Customs.") ? dbTableName.Split('.')[1] : dbTableName,
                    Schema = dbSchema,
                    DBType = dbType,
                    Columns = columnDefinitions,
                    Relations = relationDefinitions
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
                case "Raw":
                    return "timestamp";
                case "Binary":
                    return "varbinary";
                case "Time":
                    return "time";
                case "BigInteger":
                    return "bigint";
                default:
                    return null;
            }
        }

        private string GetLXMLDataType(string type)
        {
            switch (type)
            {
                case "int":
                    return "Integer";
                case "decimal":
                    return "Decimal";
                case "timestamp":
                    return "Raw";
                case "varbinary":
                    return "Binary";
                case "varchar":
                    return "Text";
                case "date":
                    return "Date";
                case "datetime":
                    return "DateTime";
                case "time":
                    return "Time";
                case "float":
                    return "Double";
                case "char":
                    return "LookUp";
                case "bigint":
                    return "BigInteger";
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
                    XElement element;

                    if (attr.AttributeFilter == null)
                    {
                        element = doc.Descendants(attr.ElementName).Single();
                    }
                    else
                    {
                        element = doc.Descendants(attr.ElementName).Where(x => x.Attribute(attr.AttributeFilter.Name).Value == attr.AttributeFilter.Value).Single();
                    }

                    if (attr.AttributeValue == null)
                    {
                        XAttribute elementAttribute = element.Attribute(attr.AttributeName);
                        if (elementAttribute != null && !String.IsNullOrEmpty(elementAttribute.Value))
                        {
                            elementAttribute.Remove();
                        }
                    }
                    else
                    {
                        element.SetAttributeValue(attr.AttributeName, attr.AttributeValue);
                    }

                    string elementText = attr.ElementName;
                    if (attr.AttributeFilter != null)
                    {
                        elementText += "[" + attr.AttributeFilter.Name + "='" + attr.AttributeFilter.Value + "']";
                    }

                    fixedMistakesData += elementText + "," + attr.AttributeName + "," + (!String.IsNullOrEmpty(attr.OldAttributeValue) ? attr.OldAttributeValue : "NULL") + "," + (!String.IsNullOrEmpty(attr.AttributeValue) ? attr.AttributeValue : "NULL") + "\n";
                }

                if(lxmlFileFixer.Attributes.Where(a => !String.IsNullOrEmpty(a.AttributeValue)).Any())
                {
                    LXMLFixedMistakesData += fixedMistakesData + "\n\n";
                }

                FileStream fileStream = new FileStream(lxmlFileFixer.FilePath, FileMode.Truncate, FileAccess.Write);
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings() { Indent = true, NewLineOnAttributes = true, OmitXmlDeclaration = false, WriteEndDocumentOnClose = false };
                XmlWriter xmlWriter = XmlWriter.Create(fileStream, xmlWriterSettings);

                doc.Save(xmlWriter);
                xmlWriter.Close();
                xmlWriter.Dispose();
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

        private void ExportMistakesData()
        {
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string csvFilePath = Path.Combine(projectDirectory, @"Reports\" + ModuleName + @"\LXMLFilesMistakes.csv");
            File.WriteAllText(csvFilePath, LXMLMistakesData);
            Console.WriteLine("\nLXML Files Mistakes Extracted To /Reports/" + ModuleName + "/LXMLFilesMistakes.csv\n");
        }

        private void ExportDXMLFilesThatNotFound()
        {
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string csvFilePath = Path.Combine(projectDirectory, @"Reports\" + ModuleName + @"\DXMLFilesThatNotFound.csv");
            File.WriteAllText(csvFilePath, DXMLFilesThatNotFound);
            Console.WriteLine("DXML Files That Not Found Extracted To /Reports/" + ModuleName + "/DXMLFilesThatNotFound.csv\n");
        }

        private void ExportFixedMistakesData()
        {
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string csvFilePath = Path.Combine(projectDirectory, @"Reports\" + ModuleName + @"\LXMLFilesFixedMistakes.csv");
            File.WriteAllText(csvFilePath, LXMLFixedMistakesData);
            Console.WriteLine("\nFixed LXML Files Mistakes Extracted To /Reports/" + ModuleName + "/LXMLFilesFixedMistakes.csv\n");
        }

        private void ExportIgnoredMistakesData()
        {
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string csvFilePath = Path.Combine(projectDirectory, @"Reports\" + ModuleName + @"\LXMLFilesIgnoredMistakes.csv");
            File.WriteAllText(csvFilePath, LXMLIgnoredMistakesData);
            Console.WriteLine("Ignored LXML Files Mistakes Extracted  To /Reports/" + ModuleName + "/LXMLFilesIgnoredMistakes.csv\n");
        }

        private string GetModuleName(int moduleNumber)
        {
            switch (moduleNumber)
            {
                case 1:
                    return "Accounting";
                case 2:
                    return "Booking";
                case 3:
                    return "CRM";
                case 4:
                    return "Customs";
                case 5:
                    return "OldModules";
                case 6:
                    return "Social";
                case 7:
                    return "Tarrifs";
                case 8:
                    return "TimeManagement";
                case 9:
                    return "Warehouse";
                case 10:
                    return "Infrastructure";
                case 11:
                    return "AllModules";
                default:
                    return "AllModules";
            }
        }

        private static ForeignEntityData GetForeignEntityData(string foreignEntity)
        {
            string foreignEntityLXMLFilePath = GetForeignEntityLXMLFilePath(foreignEntity);

            if (foreignEntityLXMLFilePath != null)
            {
                XDocument xmlDocument = XDocument.Load(foreignEntityLXMLFilePath);

                string referencedTable = xmlDocument.Root.Attribute("DBTableName") == null ? null : xmlDocument.Root.Attribute("DBTableName").Value.Split('"')[1].Split('"')[0];
                string referencedTableSchema = xmlDocument.Root.Attribute("DxmlDatabaseSchemaCode") == null ? null : xmlDocument.Root.Attribute("DxmlDatabaseSchemaCode").Value;

                string[] primaryKeyFields = xmlDocument.Descendants("field").Where(x => x.Attribute("HasDataBaseField").Value == "true" && x.Attribute("IsPrimaryKey") != null && x.Attribute("IsPrimaryKey").Value == "true" && x.Attribute("FieldName") != null).Select(x => x.Attribute("FieldName").Value.Split('"')[1].Split('"')[0]).ToArray();
                string referencedColumn = primaryKeyFields.Length > 0 ? string.Join(",", primaryKeyFields) : null;

                return new ForeignEntityData
                {
                    ReferencedTable = referencedTable.Contains("Customs.") ? referencedTable.Split('.')[1] : referencedTable,
                    ReferencedTableSchema = referencedTableSchema,
                    ReferencedColumn = referencedColumn
                };
            }

            return null;
        }

        private static string GetForeignEntityLXMLFilePath(string foreignEntity)
        {
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string logitudePath = projectDirectory.Split(new string[] { @"\Logitude\" }, StringSplitOptions.None)[0];

            string[] modulesPaths = new string[]
            {
                @"\Logitude\Logitude.Accounting.MetaData\EntityFiles\",
                @"\Logitude\Logitude.BookingLib.MetaData\EntityFiles\",
                @"\Logitude\Logitude.CRM.MetaData\EntityFiles\",
                @"\Logitude\Logitude.Customs.MetaData\EntityFiles\",
                @"\Logitude\Logitude.Social.MetaData\EntityFiles\",
                @"\Logitude\Logitude.TariffModule.MetaData\EntityFiles\",
                @"\Logitude\Logitude.TimeManagement.MetaData\EntityFiles\",
                @"\Logitude\Logitude.WarehouseLib.MetaData\EntityFiles\",
                @"\Logitude\Logitude.Infrastructure.MetaData\EntityFiles\",
                @"\Logitude\Logitude.MetaData\EntityFiles\CommonDataModel\",
                @"\Logitude\Logitude.MetaData\EntityFiles\GlobalModel\",
                @"\Logitude\Logitude.MetaData\EntityFiles\InfrastructureModel\",
                @"\Logitude\Logitude.MetaData\EntityFiles\InvoiceModel\",
                @"\Logitude\Logitude.MetaData\EntityFiles\QuoteModel\",
                @"\Logitude\Logitude.MetaData\EntityFiles\ShipmentsModel\",
                @"\Logitude\Logitude.MetaData\EntityFiles\SystemLogsModel\"
            };

            foreach (var modulePath in modulesPaths)
            {
                string path = logitudePath + modulePath + foreignEntity + ".lxml";
                if (File.Exists(path))
                {
                    return path;
                }
            }

            string[] lxmlFilesUnderRoot = Directory.GetFiles(logitudePath + @"\Logitude\", foreignEntity + ".lxml", SearchOption.AllDirectories);

            if (lxmlFilesUnderRoot.Length > 0)
            {
                return lxmlFilesUnderRoot[0];
            }

            return null;
        }

        private static string GetEntityPOCOFilePath(string entityName)
        {
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string logitudePath = projectDirectory.Split(new string[] { @"\Logitude\" }, StringSplitOptions.None)[0];

            string[] modulesPaths = new string[]
            {
                @"\Logitude\Logitude.Accounting.Data\EntityPOCOs\",
                @"\Logitude\Logitude.BookingLib.Data\EntityPOCOs\",
                @"\Logitude\Logitude.CRM.Data\EntityPOCOs\",
                @"\Logitude\Logitude.Customs.Data\EntityPOCOs\",
                @"\Logitude\Logitude.Social.Data\EntityPOCOs\",
                @"\Logitude\Logitude.TariffModule.Data\EntityPOCOs\",
                @"\Logitude\Logitude.TimeManagement.Data\EntityPOCOs\",
                @"\Logitude\Logitude.WarehouseLib.Data\EntityPOCOs\",
                @"\Logitude\Logitude.Infrastructure.Data\EntityPOCOs\",
                @"\Logitude\Simplog.Global.Data\GlobalModel\EntityPOCOs\",
                @"\Logitude\Simplog.Data\CommonDataModel\EntityPOCOs\",
                @"\Logitude\Simplog.Data\InfrastructureModel\EntityPOCOs\",
                @"\Logitude\Simplog.Data\InvoiceModel\EntityPOCOs\",
                @"\Logitude\Simplog.Data\QuoteModel\EntityPOCOs\",
                @"\Logitude\Simplog.Data\ShipmentsModel\EntityPOCOs\",
                @"\Logitude\Logitude.SystemLogs\POCOs\"
            };

            foreach (var modulePath in modulesPaths)
            {
                string path = logitudePath + modulePath + entityName + ".cs";
                if (File.Exists(path))
                {
                    return path;
                }
            }

            string[] lxmlFilesUnderRoot = Directory.GetFiles(logitudePath + @"\Logitude\", entityName + ".cs", SearchOption.AllDirectories);

            if (lxmlFilesUnderRoot.Length > 0)
            {
                return lxmlFilesUnderRoot[0];
            }

            return null;
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
            catch (Exception)
            {
                ExcludedTables = null;
                ExcludedTablesNames = null;
            }
        }

        private string GetForeignEntityFromDBTable(string tableName)
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

            string entityPOCOFilePath = GetEntityPOCOFilePath(entityName);

            if(entityPOCOFilePath == null)
            {
                return null;
            }

            List<string> pocoFileLines = File.ReadAllLines(entityPOCOFilePath).ToList();

            string foreignEntity = pocoFileLines.Where(l => l.Replace(" ", string.Empty).Contains("publicclass")).First().Replace(" ", string.Empty).Split(new string[] { "publicclass" }, StringSplitOptions.None)[1].Trim();
            
            return foreignEntity;
        }

        private string GetNavigationPropertyNameFromDBTable(string tableName, string foreignEntity, string columnName)
        {
            if(foreignEntity == null)
            {
                return null;
            }

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

            string entityPOCOFilePath = GetEntityPOCOFilePath(entityName);

            if (entityPOCOFilePath == null)
            {
                return null;
            }

            List<string> pocoFileLines = File.ReadAllLines(entityPOCOFilePath).ToList();

            List<string> navigationProperties = pocoFileLines.Where(l => l.Replace(" ", string.Empty).Contains("publicvirtual" + foreignEntity) || l.Contains("public" + foreignEntity)).ToList();

            navigationProperties = navigationProperties.Select(p => p.Replace(" ", string.Empty).Contains("publicvirtual") ? p.Replace(" ", string.Empty).Split(new string[] { "publicvirtual" + foreignEntity }, StringSplitOptions.None)[1].Split('{')[0].Trim() : p.Replace(" ", string.Empty).Split(new string[] { "public" + foreignEntity }, StringSplitOptions.None)[1].Split('{')[0].Trim()).ToList();

            string navigationPropertyName = navigationProperties.Where(p => columnName.ToLower().Contains(p.ToLower())).FirstOrDefault();
            
            if (navigationPropertyName == null)
            {
                return null;
            }

            return navigationPropertyName;
        }
    }
}