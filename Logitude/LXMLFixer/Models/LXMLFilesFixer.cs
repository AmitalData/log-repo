using Logitude.LXMLFixer.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Logitude.LXMLFixer.Models
{
    public class LXMLFilesFixer
    {
        private readonly string LXMLFilesRoot = ConfigurationManager.AppSettings["LXMLFilesRoot"];
        private readonly string DXMLFilesRoot = ConfigurationManager.AppSettings["DXMLFilesRoot"];
        private string LXMLMistakesData = "";

        public void ExtractMistakes()
        {
            string[] lxmlFiles = GetLXMLFiles();
            foreach (var lxmlFile in lxmlFiles)
            {
                string lxmlFileName = Path.GetFileName(lxmlFile);

                Console.WriteLine("Extract Mistakes For " + lxmlFileName + " ...");

                string data = "";
                string entityName = lxmlFileName.Split('.')[0];

                TableDefinition dxmlTableDefinition = GetTableDefinitionForDXMLFile(entityName);
                TableDefinition lxmlTableDefinition = GetTableDefinitionForLXMLFile(lxmlFile);

                if (dxmlTableDefinition != null && lxmlTableDefinition != null)
                {
                    data += "DXML Table, LXML Table\n";
                    data += dxmlTableDefinition.Name + "," + lxmlTableDefinition.Name;
                    if (dxmlTableDefinition.Name != lxmlTableDefinition.Name)
                    {
                        data += "," + "Incorrect Table Name In LXML File";
                    }

                    data += "\n";

                    data += "DXML Column,LXML Column,Attribute,DXML Value,LXML Value\n";

                    foreach (var dxmlColumn in dxmlTableDefinition.Columns)
                    {
                        ColumnDefinition lxmlColumn = lxmlTableDefinition.Columns.Where(c => c.Name == dxmlColumn.Name).FirstOrDefault();
                        if (lxmlColumn == null)
                        {
                            data += dxmlColumn.Name + ",Not Found,-,-,-" + "\n";
                        }
                        else
                        {
                            if (dxmlColumn.Type != lxmlColumn.Type)
                            {
                                data += dxmlColumn.Name + "," + lxmlColumn.Name + ",Type," + dxmlColumn.Type + "," + lxmlColumn.Type + "\n";
                            }
                            if (dxmlColumn.Size != lxmlColumn.Size)
                            {
                                data += dxmlColumn.Name + "," + lxmlColumn.Name + ",Size," + dxmlColumn.Size + "," + lxmlColumn.Size + "\n";
                            }
                            if (dxmlColumn.Constraints.PrimaryKey != lxmlColumn.Constraints.PrimaryKey)
                            {
                                data += dxmlColumn.Name + "," + lxmlColumn.Name + ",Primary Key," + dxmlColumn.Constraints.PrimaryKey + "," + lxmlColumn.Constraints.PrimaryKey + "\n";
                            }
                            if (dxmlColumn.Constraints.Nullable != lxmlColumn.Constraints.Nullable)
                            {
                                data += dxmlColumn.Name + "," + lxmlColumn.Name + ",Nullable," + dxmlColumn.Constraints.Nullable + "," + lxmlColumn.Constraints.Nullable + "\n";
                            }
                        }
                    }

                    LXMLMistakesData += data + "\n\n";
                }
                else
                {
                    if(dxmlTableDefinition == null)
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

            WriteMistakesDataToCSVFile();
        }

        private void WriteMistakesDataToCSVFile()
        {
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string csvFilePath = Path.Combine(projectDirectory, @"Reports\LXMLFilesMistakes.csv");
            File.WriteAllText(csvFilePath, LXMLMistakesData);
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
                    string fieldName = field.Attribute("FieldName") == null ? null : field.Attribute("FieldName").Value.Split('"')[1].Split('"')[0];
                    string type = field.Attribute("FieldsDataType") == null ? null : field.Attribute("FieldsDataType").Value.Split('"')[1].Split('"')[0];
                    int size = field.Attribute("MaxLength") == null ? 0 : Convert.ToInt32(field.Attribute("MaxLength").Value);
                    bool primaryKey = field.Attribute("IsPrimaryKey") == null ? false : field.Attribute("IsPrimaryKey").Value == "true";
                    bool nullable = field.Attribute("IsNullable") == null ? false : field.Attribute("IsNullable").Value == "true";

                    ColumnDefinition columnDefinition = new ColumnDefinition
                    {
                        Name = fieldName,
                        Type = GetDataTypeForLXMLColumn(type),
                        Size = size,
                        Constraints = new ConstraintsDefinition
                        {
                            PrimaryKey = primaryKey,
                            Nullable = nullable
                        }
                    };
                    
                    columnDefinitions.Add(columnDefinition);
                }

                TableDefinition lxmlTableDefinition = new TableDefinition
                {
                    Name = String.IsNullOrEmpty(xmlDocument.Root.Attribute("DBTableName").Value) ? null : xmlDocument.Root.Attribute("DBTableName").Value.Split('"')[1].Split('"')[0],
                    Columns = columnDefinitions
                };

                return lxmlTableDefinition;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string GetDataTypeForLXMLColumn(string type)
        {
            switch (type)
            {
                default:
                    return null;
            }
        }
    }
}
