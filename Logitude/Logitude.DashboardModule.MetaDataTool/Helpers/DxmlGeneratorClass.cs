using Logitude.DashboardModule.MetaDataTool.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml;
using System.Xml.Linq;

namespace Logitude.DashboardModule.MetaDataTool.Helpers
{
    public class DxmlGeneratorClass
    {
        //public static void GenerateDXMLFileFromTool(AnalyticsFactsMetaDataViewModel analyticsFactsMetaDataView)
        //{
        //    AnalyticsFactsMetaData table = JsonHelper.CleanUnwantedProp(analyticsFactsMetaDataView);

        //    if (!table.AnalyticsFactsFieldsMetaDatas.Any()) return;
        //    if (string.IsNullOrEmpty(App.DirectOpenPath))
        //    {
        //        MessageBox.Show("DXML File Path Is Not Valid");
        //        return;
        //    }

        //    string entityName = Path.GetFileName(App.DirectOpenPath).Replace(".ljson", string.Empty);
        //    string dxmlFilePath = App.DirectOpenPath.Replace("EntityFiles", "DBTables").Replace("lxml", "dxml");
        //    List<XElement> indexElements = new List<XElement>();
        //    List<XElement> uniqueConstraintElements = new List<XElement>();
        //    List<XElement> columnWithDefaultValueElements = new List<XElement>();
        //    List<XElement> columnWithIdentityElements = new List<XElement>();
        //    List<XElement> columnWithInitialValueScriptElements = new List<XElement>();

        //    if (File.Exists(dxmlFilePath))
        //    {
        //        XDocument oldDoc = XDocument.Load(dxmlFilePath);

        //        indexElements = oldDoc.Descendants("Index").ToList();
        //        uniqueConstraintElements = oldDoc.Descendants("UniqueConstraint").ToList();
        //        columnWithDefaultValueElements = oldDoc.Descendants("Column").Where(x => x.Attribute("DefaultValue") != null).ToList();
        //        columnWithIdentityElements = oldDoc.Descendants("Column").Where(x => x.Attribute("Identity") != null).ToList();
        //        columnWithInitialValueScriptElements = oldDoc.Descendants("Column").Where(x => x.Attribute("InitialValueScript") != null).ToList();
        //    }

        //    XmlDocument doc = new XmlDocument();
        //    XmlElement tableElement = (XmlElement)doc.AppendChild(doc.CreateElement("Table"));

        //    string tableName = table.TableName.ToLower().StartsWith("customs.") ? table.TableName.Split('.')[1] : table.TableName;
        //    tableElement.SetAttribute("Name", tableName);

        //    tableElement.SetAttribute("Schema", "dbo");
        //    tableElement.SetAttribute("DBType", "Main");
        //    tableElement.SetAttribute("Module", "Dashboard");

        //    foreach (var field in table.AnalyticsFactsFieldsMetaDatas)
        //    {
        //        tableElement.AppendChild(BuildField(entityName, columnWithDefaultValueElements, columnWithIdentityElements, columnWithInitialValueScriptElements, doc, field));
        //    }

        //    List<string> processedForeignKeyFields = new List<string>();

        //    foreach (var indexElement in indexElements)
        //    {
        //        XmlElement indexXmlElement = doc.CreateElement("Index");

        //        indexElement.Attributes().ToList().ForEach(element =>
        //        {
        //            indexXmlElement.SetAttribute(element.Name.LocalName, element.Value);
        //        });

        //        tableElement.AppendChild(indexXmlElement);
        //    }

        //    foreach (var uniqueConstraintElement in uniqueConstraintElements)
        //    {
        //        XmlElement uniqueConstraintXmlElement = doc.CreateElement("UniqueConstraint");

        //        uniqueConstraintElement.Attributes().ToList().ForEach(element =>
        //        {
        //            uniqueConstraintXmlElement.SetAttribute(element.Name.LocalName, element.Value);
        //        });

        //        tableElement.AppendChild(uniqueConstraintXmlElement);
        //    }

        //    try
        //    {
        //        //doc.Save(dxmlFilePath);
        //        FileStream fileStream;
        //        if (File.Exists(dxmlFilePath))
        //        {
        //            fileStream = new FileStream(dxmlFilePath, FileMode.Truncate, FileAccess.Write);
        //        }
        //        else
        //        {
        //            fileStream = new FileStream(dxmlFilePath, FileMode.CreateNew, FileAccess.Write);
        //        }

        //        XmlWriterSettings xmlWriterSettings = new XmlWriterSettings() { Indent = true, NewLineOnAttributes = true, OmitXmlDeclaration = false, WriteEndDocumentOnClose = false };
        //        XmlWriter xmlWriter = XmlWriter.Create(fileStream, xmlWriterSettings);

        //        doc.Save(xmlWriter);
        //        xmlWriter.Close();
        //        xmlWriter.Dispose();
        //        fileStream.Close();
        //    }
        //    catch (Exception exception)
        //    {
        //        MessageBox.Show("Error: " + exception.Message);
        //    }
        //}

        //private static XmlElement BuildField(string entityName, List<XElement> columnWithDefaultValueElements, List<XElement> columnWithIdentityElements, List<XElement> columnWithInitialValueScriptElements, XmlDocument doc, Models.FieldModels.AnalyticsFactsFieldsMetaData field)
        //{
        //    XmlElement columnElement = doc.CreateElement("Column");


        //    string fieldName = field.FieldCode;
        //    string fieldDataType = field.DataTypeCode;
        //    int fieldMaxLength = field.MaxLength;
        //    bool fieldIsMaxLength = field.IsMaxLength;
        //    bool fieldIsPrimaryKey = field.IsPrimaryKey;
        //    bool fieldIsRequired = field.IsRequired;
        //    bool fieldIsNullable = field.IsNullable;
        //    bool fieldIsFixedLength = field.IsFixedLength;
        //    int? fieldNumberOfDigits = field.NumberOfDigits;
        //    int? fieldDigitsAfterPoint = field.DigitsAfterPoint;

        //    string dxmlColumnDataType = GetDataTypeForDXMLColumn(fieldDataType, fieldIsFixedLength);
        //    bool dxmlColumnNullable;

        //    if (entityName.ToLower() == "Address".ToLower() && (fieldName.ToLower() == "City".ToLower() || fieldName.ToLower() == "CountryId".ToLower()))
        //    {
        //        dxmlColumnNullable = true;
        //    }
        //    else
        //    {
        //        dxmlColumnNullable = GetNullableForDXMLColumn(fieldIsRequired, fieldIsNullable, fieldIsPrimaryKey, dxmlColumnDataType);
        //    }


        //    int dxmlColumnSize = new string[] { "bit", "datetime", "decimal", "float", "int" }.Contains(dxmlColumnDataType) ? 0 : (fieldIsMaxLength ? -1 : fieldMaxLength);

        //    columnElement.SetAttribute("Name", fieldName);


        //    columnElement.SetAttribute("Type", dxmlColumnDataType);

        //    if (dxmlColumnSize != 0)
        //    {
        //        columnElement.SetAttribute("Size", dxmlColumnSize.ToString());
        //    }

        //    if (dxmlColumnDataType == "decimal" && fieldNumberOfDigits != null)
        //    {
        //        columnElement.SetAttribute("Precision", fieldNumberOfDigits.ToString());
        //    }

        //    if (dxmlColumnDataType == "decimal" && fieldDigitsAfterPoint != null)
        //    {
        //        columnElement.SetAttribute("Scale", fieldDigitsAfterPoint.ToString());
        //    }

        //    XmlElement constraintsElement = doc.CreateElement("Constraints");
        //    columnElement.AppendChild(constraintsElement);

        //    if (fieldIsPrimaryKey)
        //    {
        //        constraintsElement.SetAttribute("PrimaryKey", "true");
        //    }

        //    constraintsElement.SetAttribute("Nullable", dxmlColumnNullable ? "true" : "false");

        //    return columnElement;
        //}

        private static string GetDataTypeForDXMLColumn(string type, bool isFixedLength)
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

        private static bool GetNullableForDXMLColumn(bool fieldIsRequired, bool fieldIsNullable, bool fieldIsPrimaryKey, string dxmlColumnDataType)
        {
            if (fieldIsPrimaryKey) return false;
            if (new string[] { "bit", "datetime", "decimal", "float", "int" }.Contains(dxmlColumnDataType)) return !fieldIsRequired && fieldIsNullable;
            return !fieldIsRequired;
        }

    }
}

