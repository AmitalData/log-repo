using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDataViews.Service
{
  public  class CustomFieldDataWarehouseViewService: GeneralDataWarehouseViewsService
    {

        private string connectionString;
        private List<DWObjectFieldItem> dwObjectFieldLists;
        private DataTable customObjectFieldLists;
        private WarehouseView warehouseView;
        int tenant;

        public CustomFieldDataWarehouseViewService(CustomFieldDataWarehouseArgs customFieldDataWarehouseArgs)
        {
            this.connectionString = customFieldDataWarehouseArgs.ConnectionString;
            this.dwObjectFieldLists = customFieldDataWarehouseArgs.DWObjectFieldLists;
            this.tenant = customFieldDataWarehouseArgs.Tenant;
            this.warehouseView = customFieldDataWarehouseArgs.WarehouseView;

            
            customObjectFieldLists = GetCustomObjectFields(tenant , warehouseView.CustomFieldObjectTableName);

        }

        public List<WarehouseView> GetCustomFieldViewLists()
        {
            var customFieldViewLists = new List<WarehouseView>();
            var pickListCustomFields = customObjectFieldLists.Rows
                                 .Cast<DataRow>()
                                 .Where(r => r["DataTypeCode"] != null && r["DataTypeCode"].ToString() == "PickList")
                                 .ToList();
            foreach (DataRow customField in pickListCustomFields)
            {
                string customPickListCode = customField["CustomPickListCode"].ToString();
                string fieldName = GetFieldNameFromCode(customField["DefaultText"].ToString());
                string viewName = GetViewName(fieldName, "Custom");
                if (customFieldViewLists.Where(d => d.ViewName == viewName).FirstOrDefault() == null)
                {
                    customFieldViewLists.Add(CreateCustomFieldPickListDataView(customPickListCode, fieldName));
                }

            }
            return customFieldViewLists;
        }
        private WarehouseView CreateCustomFieldPickListDataView(string customPickListCode, string fieldName)
        {
            string viewName = GetViewName(fieldName, "Custom");

            var warehouseView = new WarehouseView() { ViewName = viewName, SqlString = " CREATE VIEW " + viewName + " AS SELECT " };
            foreach (DWObjectFieldItem dwObjectFieldDB in dwObjectFieldLists.Where(d => d.DWObjectTableCode == "DIM_CustomPickLists").ToList())
            {
                warehouseView.SqlString += " " + dwObjectFieldDB.FieldCode + " as ";
                if (dwObjectFieldDB.IsPrimaryKey) warehouseView.SqlString += ("c_" + fieldName + "Key");
                else if (dwObjectFieldDB.FieldCode == "[Value]") warehouseView.SqlString += (fieldName + "Value");
                else if (!string.IsNullOrEmpty(dwObjectFieldDB.ViewFieldDisplayName)) warehouseView.SqlString += dwObjectFieldDB.ViewFieldDisplayName;
                else warehouseView.SqlString += ConvertStringToCamelCase(GetFieldNameFromCode(dwObjectFieldDB.FieldCode)).Replace("(", "In").Replace(")", "");

                warehouseView.SqlString += ",";
            }
            warehouseView.SqlString = warehouseView.SqlString.Remove(warehouseView.SqlString.Length - 1);
            warehouseView.SqlString += (" FROM DIM_CustomPickLists where [Code] ='" + customPickListCode + "' or [Code] = '-1'");
            return warehouseView;
        }

        public string GetCustomFieldsAsSqlString()
        {
            string result = string.Empty;
            foreach (var customField in customObjectFieldLists.AsEnumerable().ToList())
            {
                string fieldDisplay = ConvertStringToCamelCase(customField["DefaultText"].ToString());
                string fieldCode = "[" + customField["FieldName"].ToString() + "]";

                string dataTypeCode = customField["DataTypeCode"].ToString();
                if (dataTypeCode != "LookUp")
                {
                    if (dataTypeCode == "PickList") fieldDisplay = fieldDisplay + "Key";
                    if (dataTypeCode == "Date")
                    {
                        result += ",CASE WHEN CONVERT(date," + fieldCode + ")  ='1-1-1' or  CONVERT(date," + fieldCode + ") ='2-2-2' or  CONVERT(date," + fieldCode + ") ='3-3-3'  THEN null ELSE CONVERT(" + GetSqlFieldDataType(customField) + "," + fieldCode + ")" + " END as " + "[c_" + fieldDisplay + "]";
                    }
                    else
                    {
                        result += ",CONVERT(" + GetSqlFieldDataType(customField) + "," + fieldCode + ") as " + "[c_" + fieldDisplay + "]";
                    }
                }
            }
            return result;
        }

        private DataTable GetCustomObjectFields(int tenant  , string objectTableName)
        {
            string sql = "SELECT  MaxLength ,FieldName,  DataTypeCode,TextCodes.DefaultText,CustomPickListCode from  ObjectFields inner join TextCodes on ObjectFields.FullNameTextCodeCode = TextCodes.Code and ObjectFields.tenant = TextCodes.Tenant where ObjectFields.IsCustom = 1 and ObjectFields.Tenant =" + tenant + " and ObjectFields.ObjectTableId =(select id from ObjectTables where Name = '" + objectTableName  + "')";
            return GetDataTableFromSql(connectionString, sql);
        }

    }

    public class CustomFieldDataWarehouseArgs
    {
        public List<DWObjectFieldItem> DWObjectFieldLists { get; set; }
        public int Tenant { get; set; }
        public string ConnectionString { get; set; }
        public WarehouseView WarehouseView { get; set; }

        

    }
}
