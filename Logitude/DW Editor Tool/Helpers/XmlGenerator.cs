using DW_Editor_Tool.ViewModels; 
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

        public static XmlNode indexesXmlNode = null;
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
                SetAttribute("DataViewName", GetStringValue(tableViewModel.DataViewName), entityElement);
                SetAttribute("HasPivotColumn", tableViewModel.HasPivotColumn.ToString().ToLower(), entityElement);
                SetAttribute("PivotFieldCode", GetStringValue(tableViewModel.PivotFieldCode), entityElement);

                SetAttribute("AdditionalFactCode", GetStringValue(tableViewModel.AdditionalFactCode), entityElement);
                SetAttribute("AdditionalFactForeignKey", GetStringValue(tableViewModel.AdditionalFactForeignKey), entityElement);

                SetAttribute("ParentFactCode", GetStringValue(tableViewModel.ParentFactCode), entityElement);
                SetAttribute("RecordType", GetStringValue(tableViewModel.RecordType), entityElement);
                SetAttribute("DisplayName", GetStringValue(tableViewModel.DisplayName), entityElement);

                SetAttribute("ObjectTableName", GetStringValue(tableViewModel.ObjectTableName), entityElement);
                SetAttribute("HasCustomFields", tableViewModel.HasCustomFields.ToString().ToLower(), entityElement);
                SetAttribute("MaxNumberOfCustomFields", tableViewModel.MaxNumberOfCustomFields.ToString(), entityElement);
                SetAttribute("AdditionalFactRelationType", GetStringValue(tableViewModel.AdditionalFactRelationType), entityElement);
                SetAttribute("AdditionalConditions", GetStringValue(tableViewModel.AdditionalConditions), entityElement);
                SetAttribute("Description", GetStringValue(tableViewModel.Description), entityElement);


                XmlElement fieldsTagElement = doc.CreateElement("fields");
                entityElement.AppendChild(fieldsTagElement);
                BuildFieldTags(tableViewModel, doc, fieldsTagElement);

                entityElement.AppendChild(fieldsTagElement);

                if (indexesXmlNode != null)
                {
                    XmlNode newBook = doc.ImportNode(indexesXmlNode, true);
                    entityElement.AppendChild(newBook);
                }



                ////entityElement.AppendChild();



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
            catch (Exception ex)
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
                SetAttribute("OriginalObjectFieldCode", GetStringValue(fieldViewModel.OriginalObjectFieldCode), fieldElement);
                SetAttribute("ViewFieldDisplayName", GetStringValue(fieldViewModel.ViewFieldDisplayName), fieldElement);
                SetAttribute("DontDisplayInView", fieldViewModel.DontDisplayInView.ToString().ToLower(), fieldElement);
                SetAttribute("IsMultipleSelection", fieldViewModel.IsMultipleSelection.ToString().ToLower(), fieldElement);
                SetAttribute("DimensionDataViewName", GetStringValue(fieldViewModel.DimensionDataViewName), fieldElement);
                SetAttribute("RecordType", GetStringValue(fieldViewModel.RecordType), fieldElement);



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
                        tableViewModel.DataViewName = GetAttributeStringValue(entity.Attributes["DataViewName"]);
                        tableViewModel.HasPivotColumn = GetAttributeBoolValue(entity.Attributes["HasPivotColumn"]);
                        tableViewModel.PivotFieldCode = GetAttributeStringValue(entity.Attributes["PivotFieldCode"]);

                        tableViewModel.AdditionalFactCode = GetAttributeStringValue(entity.Attributes["AdditionalFactCode"]);
                        tableViewModel.AdditionalFactForeignKey = GetAttributeStringValue(entity.Attributes["AdditionalFactForeignKey"]);

                        tableViewModel.ParentFactCode = GetAttributeStringValue(entity.Attributes["ParentFactCode"]);
                        tableViewModel.RecordType = GetAttributeStringValue(entity.Attributes["RecordType"]);
                        tableViewModel.DisplayName = GetAttributeStringValue(entity.Attributes["DisplayName"]);

                        tableViewModel.ObjectTableName = GetAttributeStringValue(entity.Attributes["ObjectTableName"]);
                        tableViewModel.HasCustomFields = GetAttributeBoolValue(entity.Attributes["HasCustomFields"]);
                        tableViewModel.MaxNumberOfCustomFields = GetAttributeIntegerValue(entity.Attributes["MaxNumberOfCustomFields"]);
                        tableViewModel.AdditionalFactRelationType = GetAttributeStringValue(entity.Attributes["AdditionalFactRelationType"]);
                        tableViewModel.AdditionalConditions = GetAttributeStringValue(entity.Attributes["AdditionalConditions"]);
                        tableViewModel.Description = GetAttributeStringValue(entity.Attributes["Description"]);

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

                            if (childNode.Name == "Indexes") indexesXmlNode = childNode;

                            // Generate Object Table Fields From List
                            BuilDWObjectFields(tableViewModel, fieldsList);
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

        private static void BuilDWObjectFields(DWObjectTableViewModel tableViewModel, List<DWObjectFieldViewModel> fieldsList)
        {
            string connectionString = "Data Source=.;Initial Catalog=Logbox_Main;Integrated Security=False;Persist Security Info=True;User ID=sa;Password= Saas256;MultipleActiveResultSets=True;Connect Timeout=60";
            string objectTableName = "Container";
            var objectTableLists = GetObjectFields(connectionString);

            var fieldListString = "ActualEmptyPickupDate,EmptyPickupLocation,EmptyPickupLocationPortId,EstimatedEmptyPickupDate,PreCarriageLocation,PreCarriageLocationPortId,POLLocation," +
"POLLocationPortId,EstimatedPOLArrival,ActualPOLArrival,EstimatedPOLLoaded,ActualPOLLoaded,EstimatedPOLVesselDeparture,ActualPOLVesselDeparture,Transshipment1Location,Transshipment1LocationPortId,EstimatedTrans1VesselArrival," +
"ActualTransshipment1VesselArrival,EstimatedTransshipment1Discharge,ActualTransshipment1Discharge,EstimatedTransshipment1Loaded,ActualTransshipment1Loaded,EstimatedTrans1VesselDeparture," +
"ActualTrans1VesselDeparture,Transshipment2Location,Transshipment2LocationPortId,EstimatedTrans2VesselArrival,ActualTransshipment2VesselArrival,ActualTransshipment2VesselArrival,EstimatedTransshipment2Discharge," +
"ActualTransshipment2Discharge,EstimatedTransshipment2Loaded,ActualTransshipment2Loaded,EstimatedTrans2VesselDeparture,ActualTrans2VesselDeparture,Transshipment3Location,Transshipment3LocationPortId,EstimatedTrans3VesselArrival," +
"ActualTransshipment3VesselArrival,EstimatedTransshipment3Discharge,ActualTransshipment3Discharge,EstimatedTransshipment3Loaded,ActualTransshipment3Loaded,EstimatedTrans3VesselDeparture,ActualTrans3VesselDeparture," +
"Transshipment4Location,Transshipment4LocationPortId,EstimatedTrans4VesselArrival,ActualTransshipment4VesselArrival,EstimatedTransshipment4Discharge,ActualTransshipment4Discharge,EstimatedTransshipment4Loaded," +
"ActualTransshipment4Loaded,EstimatedTrans4VesselDeparture,ActualTrans4VesselDeparture,PODLocation,PODLocationPortId,EstimatedPODVesselArrival,EstimatedPODDischarge,ActualPODVesselArrival," +
"ActualPODDischarge,EstimatedPODDeparture,ActualPODDeparture,AvailabilityLocation,EmptyReturnLocationPortId,EmptyReturnLocation,EstimatedEmptyReturn,ActualEmptyReturn";
             
            string[] fieldss = fieldListString.Split(','); 

            foreach (string field in fieldss)
            {
                var item = (from rowfield in objectTableLists.AsEnumerable() 
                            where rowfield.Field<string>("FieldName") == field
                            select rowfield).FirstOrDefault();

                if (item != null)
                {
                    DWObjectFieldViewModel fieldViewModel = new DWObjectFieldViewModel(tableViewModel, false);
                    fieldViewModel.Id = item["Id"].ToString();
                    fieldViewModel.Name = item["DisplayName"].ToString();
                    fieldViewModel.Code = "[" + item["DisplayName"].ToString() + "]";
                    fieldViewModel.DataTypeCode = GetDataType(item["DataTypeCode"].ToString());
                    fieldViewModel.DimensionTableCode = GeDimensionTableCode(item["LookUpName"].ToString(), item["DataTypeCode"].ToString());
                    fieldViewModel.IsRequired = (bool)item["IsRequiered"];
                    fieldViewModel.MinLength = (int)item["MinLength"];
                    fieldViewModel.MaxLength = (int)item["MaxLength"];
                    fieldViewModel.OriginalObjectFieldCode = objectTableName + "." + item["FieldName"].ToString();
                    fieldsList.Add(fieldViewModel);
                }
                else
                { // print to check
                    Console.WriteLine(field + " Not Found!");
                }
            }

        }

        private static string GeDimensionTableCode(string tableName, string dataType)
        { 

            if (dataType == "DateTime") return "DIM_Dates";

            if (string.IsNullOrEmpty(tableName)) return null;
            string DimensionTableCode;
            switch (tableName)
            {
                case "Port":
                    DimensionTableCode = "DIM_Ports";
                    break;
                case "Card":
                    DimensionTableCode = "DIM_Partners";
                    break;
                case "User":
                    DimensionTableCode = "DIM_Users";
                    break; 
                default:
                    DimensionTableCode = null;
                    break;
            }
            return DimensionTableCode;
        }

        private static string GetDataType(string dataType)
        {
            string type;
            switch (dataType)
            {
                case "LookUp":
                    type = "Dimension";
                    break;
                case "DateTime":
                    type = "Dimension";
                    break;
                default:
                    type = dataType;
                    break;
            }
            return type;
        }

        private static DataTable GetObjectFields(string connectionString)
        {
            string objectTableName = "Container";

            DataTable objectFieldsTable = new DataTable();
            using (SqlConnection sourceConnection = new SqlConnection(connectionString))
            {
                sourceConnection.Open();
                SqlCommand commandSourceData = new SqlCommand("select objectTables.Id as Id,ObjectFields.FieldName as FieldName, objectTables.Name as LookUpName,TextCodes.DefaultText as DisplayName, DataTypeCode , MaxLength, MinLength, IsRequiered  from ObjectFields " +
                    "left JOIN objectTables on ObjectFields.LookupTableId = objectTables.id" +
                    " left JOIN TextCodes on ObjectFields.FullNameTextCodeId = TextCodes.id " +
                    "where ObjectFields.ObjectTableId = (select id from ObjectTables where name = " +
                    "'"+ objectTableName + "')" + " and ObjectFields.FieldName  in" +
                    " ('ActualEmptyPickupDate', 'EmptyPickupLocation', 'EmptyPickupLocationPortId', 'EstimatedEmptyPickupDate', 'PreCarriageLocation', 'PreCarriageLocationPortId', 'POLLocation', 'POLLocationPortId', 'EstimatedPOLArrival', 'ActualPOLArrival', 'EstimatedPOLLoaded', 'ActualPOLLoaded', 'EstimatedPOLVesselDeparture', 'ActualPOLVesselDeparture', 'Transshipment1Location', 'Transshipment1LocationPortId', 'EstimatedTrans1VesselArrival', 'ActualTransshipment1VesselArrival', 'EstimatedTransshipment1Discharge', 'ActualTransshipment1Discharge', 'EstimatedTransshipment1Loaded', 'ActualTransshipment1Loaded', 'EstimatedTrans1VesselDeparture', 'ActualTrans1VesselDeparture', 'Transshipment2Location', 'Transshipment2LocationPortId', 'EstimatedTrans2VesselArrival', 'ActualTransshipment2VesselArrival', 'ActualTransshipment2VesselArrival', 'EstimatedTransshipment2Discharge', 'ActualTransshipment2Discharge', 'EstimatedTransshipment2Loaded', 'ActualTransshipment2Loaded', 'EstimatedTrans2VesselDeparture', 'ActualTrans2VesselDeparture', 'Transshipment3Location', 'Transshipment3LocationPortId', 'EstimatedTrans3VesselArrival', 'ActualTransshipment3VesselArrival', 'EstimatedTransshipment3Discharge', 'ActualTransshipment3Discharge', 'EstimatedTransshipment3Loaded', 'ActualTransshipment3Loaded', 'EstimatedTrans3VesselDeparture', 'ActualTrans3VesselDeparture', 'Transshipment4Location', 'Transshipment4LocationPortId', 'EstimatedTrans4VesselArrival', 'ActualTransshipment4VesselArrival', 'EstimatedTransshipment4Discharge', 'ActualTransshipment4Discharge', 'EstimatedTransshipment4Loaded', 'ActualTransshipment4Loaded', 'EstimatedTrans4VesselDeparture', 'ActualTrans4VesselDeparture', 'PODLocation', 'PODLocationPortId', 'EstimatedPODVesselArrival', 'EstimatedPODDischarge', 'ActualPODVesselArrival', 'ActualPODDischarge', 'EstimatedPODDeparture', 'ActualPODDeparture', 'AvailabilityLocation', 'EmptyReturnLocationPortId', 'EmptyReturnLocation', 'EstimatedEmptyReturn', 'ActualEmptyReturn')", sourceConnection);

                SqlDataReader reader = commandSourceData.ExecuteReader();
                Console.WriteLine("added succsigully");
                objectFieldsTable.Load(reader);
                reader.Close();
            }
             
            return objectFieldsTable;
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
            fieldViewModel.OriginalObjectFieldCode = GetAttributeStringValue(fieldNode.Attributes["OriginalObjectFieldCode"]);
            fieldViewModel.ViewFieldDisplayName = GetAttributeStringValue(fieldNode.Attributes["ViewFieldDisplayName"]);
            fieldViewModel.DontDisplayInView = GetAttributeBoolValue(fieldNode.Attributes["DontDisplayInView"]);
            fieldViewModel.IsMultipleSelection = GetAttributeBoolValue(fieldNode.Attributes["IsMultipleSelection"]);
            fieldViewModel.DimensionDataViewName = GetAttributeStringValue(fieldNode.Attributes["DimensionDataViewName"]);
            fieldViewModel.RecordType = GetAttributeStringValue(fieldNode.Attributes["RecordType"]);





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
