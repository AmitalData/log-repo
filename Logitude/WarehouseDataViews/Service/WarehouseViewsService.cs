using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseDataViews.Service;

namespace WarehouseDataViews
{
    public class WarehouseViewsService: GeneralWarehouseViewsService
    {
       
        private int tenant;
        private DataTable customObjectFields = null;
        DataTable dimensionDWobjectFieldOnFact = null;

        public WarehouseViewsService(int tenant )
        {
            this.tenant = tenant;
        }
        
        public DataTable GetDimensionDWobjectFieldOnFactShipment(string connectionString)
        {
            var dataTable = new DataTable();

            using (SqlConnection sourceConnection = new SqlConnection(connectionString))
            {
                sourceConnection.Open();
                SqlCommand commandSourceData = new SqlCommand(
               "select Code,DimensionTableCode from DWObjectFields where DimensionTableCode is not null AND DataTypeCode = 'Dimension' AND DWObjectTableCode = 'Fact_Shipments'", sourceConnection);

                SqlDataReader reader = commandSourceData.ExecuteReader();

                dataTable.Load(reader);

                reader.Close();
            }

            return dataTable;
        }

     
        
        public void CreateAllDimensionViews(string sourceConnectionString, string destinationConnectionString)
        {
             dimensionDWobjectFieldOnFact = GetDimensionDWobjectFieldOnFactShipment(sourceConnectionString);
            foreach (DataRow row in dimensionDWobjectFieldOnFact.Rows)
            {
                string fieldCode = row["Code"].ToString();
                string tableCode = fieldCode  != "[Customer]" ? row["DimensionTableCode"].ToString(): "DIM_Customer";
                if (!string.IsNullOrEmpty(fieldCode) && fieldCode != "[Parent Tenant]" && !string.IsNullOrEmpty(tableCode))
                {
                    if (tableCode != "DIM_Dates")
                    {
                        if (fieldCode == "[Notify 1]") fieldCode = "[Notify One]";
                        else if (fieldCode == "[Source Tenant]") fieldCode = "[Tenant]";
                        else if (fieldCode == "[Notify 2]") fieldCode = "[Notify Two]";
                        string fieldName = GetFieldNameFromCode(fieldCode);
                        string viewName = GetViewName(fieldName, "Dim");
                        DropView(viewName, destinationConnectionString);



                        string scriptView = GenerateScriptView(viewName, fieldName, tableCode);
                        scriptView = ConvertFieldsNameToCamelCase(scriptView);
                        ExecuteSql(scriptView, destinationConnectionString); 
                        GrantView(viewName, destinationConnectionString);
                    }
                }
            }

            CreateCustomFieldsDimensionViews(sourceConnectionString , destinationConnectionString);
        }
        private void CreateCustomFieldsDimensionViews( string sourceConnectionString, string destinationConnectionString)
        {
            customObjectFields = GetCustomObjectFields(sourceConnectionString);
            var result = customObjectFields.Rows
                                 .Cast<DataRow>()
                                 .Where(r => r["DataTypeCode"] != null && r["DataTypeCode"].ToString() == "PickList")
                                 .ToList();
            foreach (DataRow customField in result)
            {
                string customPickListCode = customField["CustomPickListCode"].ToString();
                string fieldName = GetFieldNameFromCode(customField["DefaultText"].ToString());


                string viewName = GetViewName(fieldName , "Custom");
                DropView(viewName, destinationConnectionString);
                string scriptView = GenerateScriptView(viewName, fieldName, "DIM_CustomPickLists");
                scriptView = scriptView.Replace("@CustomPickListCode", "'" + customPickListCode + "'");
                scriptView = scriptView.Replace("[Value]", "[Value] as ["+ fieldName + "Value]");
                scriptView = ConvertFieldsNameToCamelCase(scriptView);
                ExecuteSql(scriptView, destinationConnectionString);
                GrantView(viewName, destinationConnectionString);

            }

        }

        public void CreateFactShipmentView(string sourceConnection, string destinationConnectionString)
        {
            string viewName = "factShipments";
            DropView(viewName, destinationConnectionString);
            string scriptView = GenerateScriptView(viewName, "Shipment", "Fact_Shipments");
            scriptView = ConvertFieldsNameToCamelCase(scriptView);
            string customFieldScript = GetCustomFieldsSql();
            scriptView = scriptView.Replace(",@CustomFields", customFieldScript);
            scriptView = AppendDatesFieldToFactTable(scriptView);
            scriptView = RemoveBowsFromFieldsName(scriptView);
            ExecuteSql(scriptView, destinationConnectionString);
            GrantView(viewName, destinationConnectionString);
        }

        private string AppendDatesFieldToFactTable(string scriptView)
        {
            var dwObjectFieldDate = dimensionDWobjectFieldOnFact.Rows
                          .Cast<DataRow>()
                          .Where(r => r["DimensionTableCode"] != null && r["DimensionTableCode"].ToString() == "DIM_Dates")
                          .ToList();
            foreach (DataRow dateField in dwObjectFieldDate)
            {
                string fieldCode = dateField["Code"].ToString();
                if (!string.IsNullOrEmpty(fieldCode))
                {
                    string x = "CASE WHEN " + fieldCode + " ='1-1-1' or  " + fieldCode + " ='2-2-2' or  " + fieldCode + " ='3-3-3'  THEN null ELSE " + fieldCode + " END ";
                    scriptView = scriptView.Replace(fieldCode, x);
                }
            }

            return scriptView;
        }

        public void DeleteDimensionViews(string sourceConnectionString, string destinationConnectionString)
        {
            DataTable dimensionDWobjectFieldOnFact = GetDimensionDWobjectFieldOnFactShipment(sourceConnectionString);
            foreach (DataRow row in dimensionDWobjectFieldOnFact.Rows)
            {
                string fieldCode = row["Code"].ToString();
                if (!string.IsNullOrEmpty(fieldCode))
                {
                    if (fieldCode == "[Notify 1]") fieldCode = "[Notify One]";
                    else if (fieldCode == "[Notify 2]") fieldCode = "[Notify Two]";
                    else if (fieldCode == "[Source Tenant]") fieldCode = "[Tenant]";

                    string fieldName = GetFieldNameFromCode(fieldCode);
                    string viewName = GetViewName(fieldName, "Dim");
                    DropView(viewName, destinationConnectionString);

                }
            }

            DeleteCustomFieldsDimensionViews(sourceConnectionString, destinationConnectionString);
        }
        private void DeleteCustomFieldsDimensionViews(string sourceConnectionString, string destinationConnectionString)
        {
            customObjectFields = GetCustomObjectFields(sourceConnectionString);

            var result = customObjectFields.Rows
                                 .Cast<DataRow>()
                                 .Where(r => r["DataTypeCode"] != null && r["DataTypeCode"].ToString() == "PickList")
                                 .ToList();
            foreach (DataRow customField in result)
            {
                string fieldCode = customField["DefaultText"] != null ? ConvertStringToCamelCase(customField["DefaultText"].ToString()) : "";
                if (!string.IsNullOrEmpty(fieldCode)) DropView(GetViewName(fieldCode,"Custom"), destinationConnectionString);
            }
        }

    
        private string GetCustomFieldsSql()
        {
            string result = string.Empty;
            foreach (var customField in customObjectFields.AsEnumerable().ToList())
            {
                string fieldDisplay = ConvertStringToCamelCase (customField["DefaultText"].ToString());
                string fieldCode ="["+ customField["FieldName"].ToString() + "]";

                string dataTypeCode = customField["DataTypeCode"].ToString();
                if (dataTypeCode != "LookUp")
                {
                    if (dataTypeCode == "PickList") fieldDisplay = fieldDisplay + "Key";
                    if (dataTypeCode == "Date")
                    {
                        result += ",CASE WHEN CONVERT(date," + fieldCode + ")  ='1-1-1' or  CONVERT(date," + fieldCode + ") ='2-2-2' or  CONVERT(date," + fieldCode + ") ='3-3-3'  THEN null ELSE CONVERT(" + GetDataWarehouseSqlFieldType(customField) + "," + fieldCode + ")" + " END as " + "[c_" + fieldDisplay + "]";
                    }
                    else
                    {
                        result += ",CONVERT(" + GetDataWarehouseSqlFieldType(customField) + "," + fieldCode + ") as " + "[c_" + fieldDisplay + "]";
                    }
                }
         

            }
            return result;
        }
        private string GetDataWarehouseSqlFieldType(DataRow field)
        {
            string sqlFieldtype = string.Empty;
            string dataTypeCode = field["DataTypeCode"].ToString();
            int maxLength = field["MaxLength"]!=null ? int.Parse(field["MaxLength"].ToString()) :0;

            if((dataTypeCode == "nText" || dataTypeCode == "Text") && maxLength == 0)
            {
                maxLength = 100;
            }

            if (dataTypeCode == "nText") sqlFieldtype += "nvarchar(" + maxLength + ")";
            else if (dataTypeCode == "Text") sqlFieldtype += "varchar(" + maxLength + ")";
            else if (dataTypeCode == "Boolean") sqlFieldtype += " bit";
            else if (dataTypeCode == "Decimal" || dataTypeCode == "Double") sqlFieldtype += " float";
            else if (dataTypeCode == "Integer") sqlFieldtype += " int";
            else if (dataTypeCode == "DateTime") sqlFieldtype += " dateTime";
            else if (dataTypeCode == "Date") sqlFieldtype += " date";
            else if (dataTypeCode == "PickList") sqlFieldtype += " varchar(15)";
            return sqlFieldtype;
        }
        public DataTable GetCustomObjectFields(string connectionString)
        {
            DataTable customFields = new DataTable();
            using (SqlConnection sourceConnection = new SqlConnection(connectionString))
            {
                sourceConnection.Open();
                string sql = "SELECT  MaxLength ,FieldName,  DataTypeCode,TextCodes.DefaultText,CustomPickListCode from  ObjectFields inner join TextCodes on ObjectFields.FullNameTextCodeCode = TextCodes.Code and ObjectFields.tenant = TextCodes.Tenant where ObjectFields.IsCustom = 1 and ObjectFields.Tenant =" + tenant + " and ObjectFields.ObjectTableId =(select id from ObjectTables where Name = 'Shipment')";
                SqlCommand commandSourceData = new SqlCommand(sql, sourceConnection);
                SqlDataReader reader = commandSourceData.ExecuteReader();
                customFields.Load(reader);
                reader.Close();
            }

            return customFields;

        }

        public string GenerateScriptView(string viewName, string fieldCode,string tableCode)
        {
            string scriptView = string.Empty;
            string scriptstring = ReadScriptFile(tableCode);
            scriptstring = scriptstring.Replace("[Key]", fieldCode + "Key");
            if (tableCode != "DIM_CustomPickLists")
            {
                scriptstring = scriptstring.Replace("[Code]", "[Code] as [" + fieldCode + "Code]");
            }
            scriptView = " CREATE VIEW " + viewName + " AS  " + scriptstring;

            return scriptView;
        }
        
        public void GrantView(string viewName,  string destinationConnectionString)
        {
            string sqlstring = "GRANT SELECT  ON [UnicargoDW].[dbo].[" + viewName + "] TO [UnicargoDBUser]"; // Pre

           // string sqlstring = "GRANT SELECT  ON [T570Unicargo].[dbo].[" + viewName + "] TO [U570gmxaU]";   //Online 
            ExecuteSql(sqlstring, destinationConnectionString);
        }
        public void DropView(string viewName, string connectionString)
        {
            string sqlstring = "if exists(select 1 from sys.views where name='" + viewName + "' and type='v') begin drop view " + viewName + ";end";
            ExecuteSql(sqlstring, connectionString);
        }
     
    }
}
