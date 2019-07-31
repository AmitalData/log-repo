using DW_Editor_Tool.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace DW_Editor_Tool.Helpers
{
    public class XmlGenerator
    {
        public static bool GenerateXmlToFile(DWObjectTableViewModel tableViewModel)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(xmlDeclaration);
                XmlElement entityElement = (XmlElement)doc.AppendChild(doc.CreateElement("entity"));
                
                if (string.IsNullOrEmpty(tableViewModel.Id))
                {
                    SetAttribute("Id", GetStringValue(Guid.NewGuid()), entityElement);
                }
                else
                {
                    SetAttribute("Id", GetStringValue(tableViewModel.Id), entityElement);
                }

                SetAttribute("Code", GetStringValue(tableViewModel.Code), entityElement);
                SetAttribute("Name", GetStringValue(tableViewModel.Name), entityElement);
                SetAttribute("TypeCode", GetStringValue(tableViewModel.TypeCode), entityElement);
                SetAttribute("IsClosed", tableViewModel.IsClosed.ToString().ToLower(), entityElement);
                SetAttribute("DefaultFilterBy", GetStringValue(tableViewModel.DefaultFilterBy), entityElement);
                XmlElement fieldsTagElement = doc.CreateElement("fields");
                entityElement.AppendChild(fieldsTagElement);
                BuildFieldTags(tableViewModel, doc, fieldsTagElement);

                if (!string.IsNullOrEmpty(App.CurrentFilePath))
                {
                    doc.Save(App.CurrentFilePath);
                   
                    return true;
                }
                else
                {
                    tableViewModel.ErrorMessages = "File path in not valid!";
                    tableViewModel.ErrorsVisibility = Visibility.Visible;

                    return false;
                    //MessageBox.Show("file path in not valid!");
                }
            }
            catch(Exception ex)
            {
                tableViewModel.ErrorMessages = ex.Message;
                tableViewModel.ErrorsVisibility = Visibility.Visible;

                return false;
            }
        }

        private static void BuildFieldTags(DWObjectTableViewModel tableViewModel, XmlDocument doc, XmlElement fieldsTagElement)
        {
            foreach (DWObjectFieldViewModel fieldViewModel in tableViewModel.DWObjectFieldsList)
            {
                XmlElement fieldElement = doc.CreateElement("field");
                fieldsTagElement.AppendChild(fieldElement);
                if (string.IsNullOrEmpty(fieldViewModel.Id))
                {
                    SetAttribute("Id", GetStringValue(Guid.NewGuid()), fieldElement);
                }
                else
                {
                    SetAttribute("Id", GetStringValue(fieldViewModel.Id), fieldElement);
                }

                SetAttribute("Code", GetStringValue(fieldViewModel.Code), fieldElement);
                SetAttribute("Name", GetStringValue(fieldViewModel.Name), fieldElement);
                SetAttribute("DataTypeCode", GetStringValue(fieldViewModel.DataTypeCode), fieldElement);
                SetAttribute("IsRequired", fieldViewModel.IsRequired.ToString().ToLower(), fieldElement);
                SetAttribute("MinLength", fieldViewModel.MinLength.ToString(), fieldElement);
                SetAttribute("MaxLength", fieldViewModel.MaxLength.ToString(), fieldElement);
                SetAttribute("DimensionTableCode", GetStringValue(fieldViewModel.DimensionTableCode), fieldElement);
                SetAttribute("IsPrimaryKey", fieldViewModel.IsPrimaryKey.ToString().ToLower(), fieldElement);
                SetAttribute("IsMeasurement", fieldViewModel.IsMeasurement.ToString().ToLower(), fieldElement);
                SetAttribute("AggregationTypeCode", GetStringValue(fieldViewModel.AggregationTypeCode), fieldElement);
                SetAttribute("DisplayInQueryBuilder", fieldViewModel.DisplayInQueryBuilder.ToString().ToLower(), fieldElement);
                SetAttribute("Category1", GetStringValue(fieldViewModel.Category1), fieldElement);
                SetAttribute("Category2", GetStringValue(fieldViewModel.Category2), fieldElement);
                SetAttribute("LOVAdditionalColumns", GetStringValue(fieldViewModel.LOVAdditionalColumns), fieldElement);
				SetAttribute("HideTree", fieldViewModel.HideTree.ToString().ToLower(), fieldElement);
                SetAttribute("CannotFilter", fieldViewModel.CannotFilter.ToString().ToLower(), fieldElement);
                SetAttribute("HelpText", GetStringValue(fieldViewModel.HelpText), fieldElement);
                SetAttribute("IsCustom", fieldViewModel.IsCustom.ToString().ToLower(), fieldElement);


            }

        }



        public static DWObjectTableViewModel GetDWViewModelFromFile()
        {
            DWObjectTableViewModel tableViewModel = new DWObjectTableViewModel();

            try
            {
                if (!string.IsNullOrEmpty(App.CurrentFilePath))
                {
                    FileStream stream = new FileStream(App.CurrentFilePath, FileMode.Open);
                    XmlDocument document = new XmlDocument();
                    document.Load(stream);

                    XmlElement entity = document["entity"];
                    if (entity != null)
                    {
                        tableViewModel.Id = GetAttributeStringValue(entity.Attributes["Id"]);
                        tableViewModel.Code = GetAttributeStringValue(entity.Attributes["Code"]);
                        tableViewModel.Name = GetAttributeStringValue(entity.Attributes["Name"]);
                        tableViewModel.TypeCode = GetAttributeStringValue(entity.Attributes["TypeCode"]);
                        tableViewModel.IsClosed = GetAttributeBoolValue(entity.Attributes["IsClosed"]);
                        tableViewModel.DefaultFilterBy = GetAttributeStringValue(entity.Attributes["DefaultFilterBy"]);

                        List<DWObjectFieldViewModel> fieldsList = new List<DWObjectFieldViewModel>();
                        foreach (XmlNode childNode in entity.ChildNodes)
                        {
                            if (childNode.Name == "fields")
                            {
                                foreach (XmlNode fieldNode in childNode.ChildNodes)
                                {
                                    if (fieldNode.Name == "field")
                                    {
                                        fieldsList.Add(BuildObjectFieldViewModel(fieldNode, tableViewModel));
                                    }
                                }
                            }
                        }

                        tableViewModel.BuildObsList(fieldsList);
                    }
                    else
                    {
                        var FileName = Path.GetFileName(App.CurrentFilePath).Replace(".dwml", "");
                        tableViewModel.Code = FileName;
                        tableViewModel.Name = FileName;
                    }

                    stream.Dispose();

                }

                return tableViewModel;
            }
            catch (Exception ex)
            {
                var FileName = Path.GetFileName(App.CurrentFilePath).Replace(".dwml", "");
                tableViewModel.Code = FileName;
                
                return tableViewModel;
            }
        }


        private static DWObjectFieldViewModel BuildObjectFieldViewModel(XmlNode fieldNode, DWObjectTableViewModel tableViewModel)
        {
            DWObjectFieldViewModel fieldViewModel = new DWObjectFieldViewModel(tableViewModel, false);

            fieldViewModel.Id = GetAttributeStringValue(fieldNode.Attributes["Id"]);
            fieldViewModel.Code = GetAttributeStringValue(fieldNode.Attributes["Code"]);
            fieldViewModel.Name = GetAttributeStringValue(fieldNode.Attributes["Name"]);
            fieldViewModel.DataTypeCode = GetAttributeStringValue(fieldNode.Attributes["DataTypeCode"]);
            fieldViewModel.DimensionTableCode = GetAttributeStringValue(fieldNode.Attributes["DimensionTableCode"]);
            fieldViewModel.IsRequired = GetAttributeBoolValue(fieldNode.Attributes["IsRequired"]);
            fieldViewModel.MinLength = GetAttributeIntegerValue(fieldNode.Attributes["MinLength"]);
            fieldViewModel.MaxLength = GetAttributeIntegerValue(fieldNode.Attributes["MaxLength"]);
            fieldViewModel.IsPrimaryKey = GetAttributeBoolValue(fieldNode.Attributes["IsPrimaryKey"]);
            fieldViewModel.IsMeasurement = GetAttributeBoolValue(fieldNode.Attributes["IsMeasurement"]);
            fieldViewModel.AggregationTypeCode = GetAttributeStringValue(fieldNode.Attributes["AggregationTypeCode"]);
            fieldViewModel.DisplayInQueryBuilder = GetAttributeBoolValueDefaultTrue(fieldNode.Attributes["DisplayInQueryBuilder"]);
            fieldViewModel.Category1 = GetAttributeStringValue(fieldNode.Attributes["Category1"]);
            fieldViewModel.Category2 = GetAttributeStringValue(fieldNode.Attributes["Category2"]);
            fieldViewModel.LOVAdditionalColumns = GetAttributeStringValue(fieldNode.Attributes["LOVAdditionalColumns"]);
			fieldViewModel.HideTree = GetAttributeBoolValue(fieldNode.Attributes["HideTree"]);
            fieldViewModel.CannotFilter = GetAttributeBoolValue(fieldNode.Attributes["CannotFilter"]);
            fieldViewModel.HelpText = GetAttributeStringValue(fieldNode.Attributes["HelpText"]);
            fieldViewModel.IsCustom = GetAttributeBoolValue(fieldNode.Attributes["IsCustom"]);


            return fieldViewModel;
        }

    










        private static void SetAttribute(string atrrName, string attrValue, XmlElement fieldElement)
        {
            if (attrValue != null)
            {
                fieldElement.SetAttribute(atrrName, attrValue);
            }
        }

        private static string GetStringValue(object value)
        {
            if (value != null)
            {
                if (string.IsNullOrEmpty(value.ToString())) value = null;
            }


            if (value != null)
            {
                return "\"" + value.ToString().Replace("\"", "\u0022") + "\""; 
            }
            else
            {
                return null;
            }
        }


        public static bool GetAttributeBoolValue(XmlAttribute att)
        {
            bool result = false;
            if (att != null)
            {
                if (!string.IsNullOrEmpty(att.Value))
                {
                    result = bool.Parse(att.Value);
                }
            }

            return result;
        }

        public static bool GetAttributeBoolValueDefaultTrue(XmlAttribute att)
        {
            bool result = true;
            if (att != null)
            {
                if (!string.IsNullOrEmpty(att.Value))
                {
                    result = bool.Parse(att.Value);
                }
            }

            return result;
        }

        public static int GetAttributeIntegerValue(XmlAttribute att)
        {
            int result = 0;
            if (att != null)
            {
                if (!string.IsNullOrEmpty(att.Value))
                {
                    result = int.Parse(att.Value);
                }
            }

            return result;
        }

        public static int? GetAttributeNullableIntegerValue(XmlAttribute att)
        {
            int? result = null;
            if (att != null)
            {
                if (!string.IsNullOrEmpty(att.Value))
                {
                    result = int.Parse(att.Value);
                }
            }

            return result;
        }


        public static double GetAttributeDoubleValue(XmlAttribute att)
        {
            double result = 0.0;
            if (att != null)
            {
                if (!string.IsNullOrEmpty(att.Value))
                {
                    result = double.Parse(att.Value);
                }
            }

            return result;
        }

        public static string GetAttributeStringValue(XmlAttribute att)
        {
            string result = null;
            if (att != null)
            {
                if (!string.IsNullOrEmpty(att.Value))
                {
                    if (att.Value != null)
                    {
                        result = att.Value.Trim('"');
                    }
                }
            }
            return result;
        }
    }
}
