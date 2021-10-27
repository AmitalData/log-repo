using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDataViews.Service
{
    public class DataWarehouseViewsService: GeneralDataWarehouseViewsService
    {
        public List<WarehouseView> DataWarehouseViewLists = null;
        public List<DWObjectFieldItem> DwObjectFieldLists = null;
        private string connectionString = string.Empty;
        public DataWarehouseViewsService(string connectionString)
        {
            this.connectionString = connectionString;
            DataWarehouseViewLists = new List<WarehouseView>();
            DwObjectFieldLists = GetDwObjectFieldLists(connectionString);

            BuildDataWarehouseViewLists();
        }


        private bool CheckIfDataRowHaveColumnValue(DataRow dataRow , string columnName)
        {
            return dataRow[columnName] != null && !string.IsNullOrEmpty(dataRow[columnName].ToString()) ? true : false;
        }


        public void BuildDataWarehouseViewLists()
        {
            List<string> environmentFactTableCodes = GetEnvironmentFactTables(connectionString);
            var factTables = GetDataTableFromSql(connectionString, "select Code,DataViewName,RecordType ,HasCustomFields ,MaxNumberOfCustomFields ,ObjectTableName,AdditionalConditions,ParentFactCode from DWObjectTables where TypeCode = 'Fact' ");
            foreach (DataRow row in factTables.AsEnumerable())
            {
                string factCode = CheckIfDataRowHaveColumnValue(row, "Code") ? row["Code"].ToString() : "";
                string parentFactCode = CheckIfDataRowHaveColumnValue(row, "ParentFactCode") ? row["ParentFactCode"].ToString() : "";

                if (environmentFactTableCodes.Contains(factCode) || environmentFactTableCodes.Contains(parentFactCode) )
               {
                    string viewName = CheckIfDataRowHaveColumnValue(row, "DataViewName") ? row["DataViewName"].ToString() : "";
                    string recordType = CheckIfDataRowHaveColumnValue(row, "RecordType") ? row["RecordType"].ToString() : "";
                    string objectTableName = CheckIfDataRowHaveColumnValue(row, "ObjectTableName") ? row["ObjectTableName"].ToString() : "";
                    bool hasCustomFields = CheckIfDataRowHaveColumnValue(row, "HasCustomFields") ? bool.Parse(row["HasCustomFields"].ToString()) : false;
                    int maxNumberOfCustomFields = CheckIfDataRowHaveColumnValue(row, "MaxNumberOfCustomFields") ? int.Parse(row["MaxNumberOfCustomFields"].ToString()) : 0;
                    string additionalConditions = CheckIfDataRowHaveColumnValue(row, "AdditionalConditions") ? row["AdditionalConditions"].ToString() : "";

                    if (!string.IsNullOrEmpty(viewName))
                    {

                        foreach (DWObjectFieldItem field in DwObjectFieldLists.Where(d => (d.DWObjectTableCode == factCode || d.DWObjectTableCode == parentFactCode) && (string.IsNullOrEmpty(d.RecordType) || (!string.IsNullOrEmpty(d.RecordType) && d.RecordType.Split(',').Contains(recordType))) && d.DataTypeCode == "Dimension" && d.DimensionTableCode != "DIM_Dates").ToList())
                        {
                            CreateDimensionDataView(field, factCode);
                        }
                        CreateDataWarehouseFactViewArgs createDataWarehouseFactViewArgs = new CreateDataWarehouseFactViewArgs()
                        {
                            FactCode = factCode,
                            RecordType = recordType,
                            ViewName = viewName,
                            ObjectTableName = objectTableName,
                            MaxNumberOfCustomFields =!string.IsNullOrEmpty(parentFactCode) ? factTables.AsEnumerable().Where(d => d.Field<string>("Code") == parentFactCode).Select(d => d.Field<int>("MaxNumberOfCustomFields")).FirstOrDefault() : maxNumberOfCustomFields,
                            HasCustomFields = !string.IsNullOrEmpty(parentFactCode) ? factTables.AsEnumerable().Where(d => d.Field<string>("Code") == parentFactCode).Select(d=>d.Field<bool>("HasCustomFields")).FirstOrDefault() : hasCustomFields,
                            AdditionalConditions = additionalConditions,
                            ParentFactCode = parentFactCode,
                        };

                        CreateFactDataView(createDataWarehouseFactViewArgs);
                    }
               }
            }
        }
       
        private void CreateFactDataView(CreateDataWarehouseFactViewArgs createDataWarehouseFactViewArgs)
        {
            var warehouseView = new WarehouseView() { Fields = new List<DWObjectFieldItem>(), ViewCode = createDataWarehouseFactViewArgs.FactCode, ViewName = createDataWarehouseFactViewArgs.ViewName, IsFactView = true,SqlString = " CREATE VIEW " + createDataWarehouseFactViewArgs.ViewName + " AS SELECT " };


            foreach (DWObjectFieldItem dwObjectFieldDB in DwObjectFieldLists.Where(d => (d.DWObjectTableCode == createDataWarehouseFactViewArgs.FactCode || d.DWObjectTableCode == createDataWarehouseFactViewArgs.ParentFactCode) && (string.IsNullOrEmpty(d.RecordType) || (!string.IsNullOrEmpty(d.RecordType) && d.RecordType.Split(',').Contains(createDataWarehouseFactViewArgs.RecordType)))).ToList())
            {
                if (dwObjectFieldDB.DimensionTableCode == "DIM_Dates")
                {
                    warehouseView.SqlString += " " + "CASE WHEN " + dwObjectFieldDB.FieldCode + " ='1-1-1' or  " + dwObjectFieldDB.FieldCode + " ='2-2-2' or  " + dwObjectFieldDB.FieldCode + " ='3-3-3'  THEN null ELSE " + dwObjectFieldDB.FieldCode + " END " + " as ";
                }
                else warehouseView.SqlString += " " + dwObjectFieldDB.FieldCode + " as ";

                string displayName = !string.IsNullOrEmpty(dwObjectFieldDB.ViewFieldDisplayName) ? dwObjectFieldDB.ViewFieldDisplayName : ConvertStringToCamelCase(GetFieldNameFromCode(dwObjectFieldDB.FieldCode)).Replace("(", "In").Replace(")", "");
                string fieldName = (!string.IsNullOrEmpty(dwObjectFieldDB.DimensionTableCode) && dwObjectFieldDB.DimensionTableCode != "DIM_Dates" ? displayName + "Key" : displayName);


                warehouseView.SqlString += fieldName;
                warehouseView.SqlString += ",";

                warehouseView.Fields.Add(new DWObjectFieldItem() { DataTypeCode = dwObjectFieldDB.DataTypeCode, FieldName = fieldName, FieldCode = dwObjectFieldDB.FieldCode, DWObjectTableCode = dwObjectFieldDB.DWObjectTableCode });

            }
            warehouseView.HasCustomFields = createDataWarehouseFactViewArgs.HasCustomFields;
            warehouseView.MaxNumberOfCustomFields = createDataWarehouseFactViewArgs.MaxNumberOfCustomFields;
            warehouseView.ObjectTableName = createDataWarehouseFactViewArgs.ObjectTableName;

            warehouseView.SqlString = warehouseView.SqlString.Remove(warehouseView.SqlString.Length - 1);
            warehouseView.SqlString +=((warehouseView.HasCustomFields ? ",@CustomFields":"") +  " FROM " + (!string.IsNullOrEmpty( createDataWarehouseFactViewArgs.ParentFactCode) ? createDataWarehouseFactViewArgs.ParentFactCode : createDataWarehouseFactViewArgs.FactCode));


            List<string> shipmentLevelLists = GetShipmentLevelListsByRecordType(createDataWarehouseFactViewArgs.RecordType);
            string additionalCondition = new FactAdditionalConditionService(createDataWarehouseFactViewArgs , connectionString).Get();
            string recordTypeCondation = string.Empty;
            if (shipmentLevelLists.Count() > 0)
            {
                recordTypeCondation = "  [DirectHouse] in ( ";
                foreach (string shipmentType in shipmentLevelLists)
                {
                    recordTypeCondation += "'" + shipmentType + "' ,";
                }
                recordTypeCondation = recordTypeCondation.Remove(recordTypeCondation.Length - 1);
                recordTypeCondation += ") ";
            }

            warehouseView.SqlString += (!string.IsNullOrEmpty(additionalCondition) || !string.IsNullOrEmpty(recordTypeCondation)) ? " where " : "";
            warehouseView.SqlString += additionalCondition;
            warehouseView.SqlString += (!string.IsNullOrEmpty(additionalCondition) && !string.IsNullOrEmpty(recordTypeCondation)) ? " and ":"";
            warehouseView.SqlString += recordTypeCondation;



            DataWarehouseViewLists.Add(warehouseView);
        }

        private List<string> GetShipmentLevelListsByRecordType(string recordType)
        {
            var result = new List<string>();
            if (!string.IsNullOrEmpty(recordType))
            {
                if (recordType == "Master")
                {
                    result.Add("Consol");
                    result.Add("Direct");
                }
                else if (recordType == "Shipment")
                {
                    result.Add("Direct");
                    result.Add("House");
                }
            }
            return result;
        }


        private void CreateDimensionDataView(DWObjectFieldItem field , string factCode)
        {
            string parentfieldName = !string.IsNullOrEmpty(field.ViewFieldDisplayName) ? field.ViewFieldDisplayName : GetFieldNameFromCode(field.FieldCode);
            string viewName = !string.IsNullOrEmpty(field.DimensionDataViewName) ?  field.DimensionDataViewName: GetViewName(parentfieldName, "Dim");
            if (DataWarehouseViewLists.Where(d => d.ViewName == viewName).FirstOrDefault() == null)
            {
                var warehouseView = new WarehouseView() { ViewName = viewName, SqlString = " CREATE VIEW " + viewName + " AS SELECT ", Fields = new List<DWObjectFieldItem>(), FactConnectedCodeLists = new List<string>() { factCode } };

                foreach (DWObjectFieldItem dwObjectFieldDB in DwObjectFieldLists.Where(d => d.DWObjectTableCode == field.DimensionTableCode && (string.IsNullOrEmpty(d.RecordType) || (!string.IsNullOrEmpty(d.RecordType) && d.RecordType.Split(',').Contains(parentfieldName)))).ToList())
                {
                    warehouseView.SqlString += " " + dwObjectFieldDB.FieldCode + " as ";
                    string fieldDisplayName = string.Empty;
                    if (dwObjectFieldDB.IsPrimaryKey) fieldDisplayName = (parentfieldName + "Key");
                    else if (dwObjectFieldDB.FieldCode == "[Code]" || dwObjectFieldDB.FieldCode == "[Email]" ||  dwObjectFieldDB.FieldCode == "[Source Tenant]") fieldDisplayName = parentfieldName + (!string.IsNullOrEmpty(dwObjectFieldDB.ViewFieldDisplayName) ? dwObjectFieldDB.ViewFieldDisplayName : GetFieldNameFromCode(dwObjectFieldDB.FieldCode));
                    else if (!string.IsNullOrEmpty(dwObjectFieldDB.ViewFieldDisplayName)) fieldDisplayName = dwObjectFieldDB.ViewFieldDisplayName;
                    else fieldDisplayName = ConvertStringToCamelCase(GetFieldNameFromCode(dwObjectFieldDB.FieldCode)).Replace("(", "In").Replace(")", "");
                    warehouseView.SqlString += (fieldDisplayName +  ",");

                    warehouseView.Fields.Add(new DWObjectFieldItem() {DataTypeCode = dwObjectFieldDB.DataTypeCode, FieldName = fieldDisplayName, FieldCode = dwObjectFieldDB.FieldCode , DWObjectTableCode = dwObjectFieldDB.DWObjectTableCode });

                }
                warehouseView.SqlString  = warehouseView.SqlString.Remove(warehouseView.SqlString.Length - 1);
                warehouseView.SqlString += (" FROM " + field.DimensionTableCode);
                DataWarehouseViewLists.Add(warehouseView);
            }
            else
            {
                DataWarehouseViewLists.Where(d => d.ViewName == viewName).FirstOrDefault().FactConnectedCodeLists.Add(factCode);
            }
        }

        private bool CheckIfFactHaveCustomField(string factCode)
        {
            var result = false;

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand com = new SqlCommand("select top(1) IsCustom FROM dbo.DWObjectFields where IsCustom =1 and DWObjectTableCode = '" + factCode + "'", con);
            try
            {
                con.Open();

                using (SqlDataReader reader = com.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (reader["IsCustom"] != null)
                        {
                            result = true;
                        }

                    }
                }
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public List<DWObjectFieldItem> GetDwObjectFieldLists(string connectionString)
        {
            var result = new List<DWObjectFieldItem>();
            DataTable dWObjectFieldsMetaData = GetDataTableFromSql(connectionString, "SELECT  Code,IsCustom, IsPrimaryKey, DWObjectTableCode,ViewFieldDisplayName,DimensionDataViewName, DataTypeCode ,DimensionTableCode,RecordType from DWObjectFields where DontDisplayInView =0 and IsCustom=0");
            foreach (DataRow row in dWObjectFieldsMetaData.AsEnumerable())
            {
                var fieldDB = new DWObjectFieldItem();
                fieldDB.FieldCode = row.Table.Columns.Contains("Code") ? row["Code"].ToString() : "";
                fieldDB.DataTypeCode = row.Table.Columns.Contains("DataTypeCode") ? row["DataTypeCode"].ToString() : "";
                fieldDB.DimensionTableCode = row.Table.Columns.Contains("DimensionTableCode") ? row["DimensionTableCode"].ToString() : "";
                fieldDB.DimensionDataViewName = row.Table.Columns.Contains("DimensionDataViewName") ? row["DimensionDataViewName"].ToString() : "";
                fieldDB.ViewFieldDisplayName = row.Table.Columns.Contains("ViewFieldDisplayName") ? row["ViewFieldDisplayName"].ToString() : "";
                fieldDB.DWObjectTableCode = row.Table.Columns.Contains("DWObjectTableCode") ? row["DWObjectTableCode"].ToString() : "";
                fieldDB.IsCustom = row.Table.Columns.Contains("IsCustom") ? bool.Parse(row["IsCustom"].ToString()) : false;
                fieldDB.IsPrimaryKey = row.Table.Columns.Contains("IsPrimaryKey") ? bool.Parse(row["IsPrimaryKey"].ToString()) : false;
                fieldDB.RecordType = row.Table.Columns.Contains("RecordType") ? row["RecordType"].ToString() : "";
                result.Add(fieldDB);
            }
            return result;
        }
    }

    public class WarehouseView
    {
        public string ViewCode { get; set; }
        public string ViewName { get; set; }
        public string SqlString { get; set; }
        public bool IsFactView { get; set; }
        public bool HasCustomFields { get; set; }
        public List<DWObjectFieldItem> Fields { get; set; }
        public List<string> FactConnectedCodeLists { get; set; }
        public int MaxNumberOfCustomFields { get; set; }
        public string ObjectTableName { get; set; }
        public string CustomFieldScriptSQL { get; set; }

        

    }

   
    public class CreateDataWarehouseFactViewArgs
    {
        public string FactCode { get; set; }
        public string ViewName { get; set; }
        public string RecordType { get; set; }
        public bool HasCustomFields { get; set; }
        public int MaxNumberOfCustomFields { get; set; }
        public string ObjectTableName { get; set; }
        public string AdditionalConditions { get; set; }
        public string ParentFactCode { get; set; }

    }


    public class DWObjectFieldItem
    {
        public string FieldCode { get; set; }
        public string DataTypeCode { get; set; }
        public string DimensionTableCode { get; set; }
        public string DWObjectTableCode { get; set; }
        public string ViewFieldDisplayName { get; set; }
        public bool DontDisplayInView { get; set; }
        public string DimensionDataViewName { get; set; }
        public bool IsCustom { get; set; }
        public bool IsPrimaryKey { get; set; }
        public string RecordType { get; set; }
        public string FieldName { get; set; }
    }


}
