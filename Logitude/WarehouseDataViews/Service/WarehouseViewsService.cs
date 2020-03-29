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
            DataTable dimensionDWobjectFieldOnFact = GetDimensionDWobjectFieldOnFactShipment(sourceConnectionString);
            foreach (DataRow row in dimensionDWobjectFieldOnFact.Rows)
            {
                string fieldCode = row["Code"].ToString();
                string tableCode = row["DimensionTableCode"].ToString();
                if (!string.IsNullOrEmpty(fieldCode) && fieldCode != "[Parent Tenant]" && !string.IsNullOrEmpty(tableCode))
                {
                    if (tableCode != "DIM_Dates")
                    {
                        if (fieldCode == "[Notify 1]") fieldCode = "[Notify One]";
                        else if (fieldCode == "[Notify 2]") fieldCode = "[Notify Two]";

                        string viewName = GetViewName(fieldCode);
                        DropView(viewName, destinationConnectionString);
                        string scriptView = GenerateScriptView(viewName, tableCode);
                        ExecuteSql(scriptView, destinationConnectionString); 
                        // GrantView(viewName, destinationConnectionString);
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

                string fieldCode = customField["DefaultText"].ToString().Replace(" ", "");
                string viewName = GetViewName(fieldCode);
                DropView(viewName, destinationConnectionString);
                string scriptView = GenerateScriptView(viewName, "DIM_CustomPickLists");
                scriptView = scriptView.Replace("@CustomPickListCode", "'" + customPickListCode + "'");
                ExecuteSql(scriptView, destinationConnectionString);
                //  GrantView(viewName, destinationConnectionString);

            }

        }

        public void CreateFactShipmentView(string sourceConnection, string destinationConnectionString)
        {
            DropView("ShipmentView", destinationConnectionString);
            string scriptView = GenerateScriptView("ShipmentView", "Fact_Shipments");
            scriptView = ConvertFieldsNameToCamelCase(scriptView);
            string customFieldScript = GetCustomFieldsSql();
            scriptView = scriptView.Replace(",@CustomFields", customFieldScript);
            ExecuteSql(scriptView, destinationConnectionString);
            // warehouseViewsService.GrantView("ShipmentView", destinationConnectionString);
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
                    string viewName = GetViewName(fieldCode);
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
                string fieldCode = customField["DefaultText"] != null ? customField["DefaultText"].ToString().Replace(" ", "") : "";
                if (!string.IsNullOrEmpty(fieldCode)) DropView(GetViewName(fieldCode), destinationConnectionString);
            }
        }

    
        private string GetCustomFieldsSql()
        {
            
            string result = string.Empty;
            foreach (var customField in customObjectFields.AsEnumerable().ToList())
            {
                string fieldDisplay = customField["DefaultText"].ToString();
                string fieldName = customField["FieldName"].ToString();
                string dataTypeCode = customField["DataTypeCode"].ToString();
                if (dataTypeCode != "LookUp" && dataTypeCode != "Date")
                {
                    if(dataTypeCode == "PickList") fieldDisplay = fieldDisplay.Replace(" ", "") + "ViewKey";
                    result += ",CONVERT(" + GetDataWarehouseSqlFieldType(customField) + ",[" + fieldName + "]) as " + "[" + fieldDisplay + "]";
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

        public string GenerateScriptView(string viewName, string tableCode)
        {
            string scriptView = string.Empty;
            string scriptstring = ReadScriptFile(tableCode);
            scriptstring = scriptstring.Replace("[Key]", viewName + "key");
            if (tableCode != "DIM_CustomPickLists")
            {
                scriptstring = scriptstring.Replace("[Code]", "[Code] as [" + viewName.Replace("View", "") + " Code]");
            }
            scriptView = " CREATE VIEW " + viewName + " AS  " + scriptstring;

            return scriptView;
        }
        
        public void GrantView(string viewName,  string destinationConnectionString)
        {
            string sqlstring = "GRANT SELECT  ON [UnicargoDW].[dbo].["+ viewName + "] TO [UnicargoDBUser]";
            ExecuteSql(sqlstring, destinationConnectionString);
        }
        public void DropView(string viewName, string connectionString)
        {
            string sqlstring = "if exists(select 1 from sys.views where name='" + viewName + "' and type='v') begin drop view " + viewName + ";end";
            ExecuteSql(sqlstring, connectionString);
        }
     
    }
}
