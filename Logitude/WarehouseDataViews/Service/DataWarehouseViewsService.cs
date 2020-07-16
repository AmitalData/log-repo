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
        public List<WarehouseView> DataWarehouseViewLists = new List<WarehouseView>();
        public List<DWObjectFieldDB> DwObjectFieldLists = new List<DWObjectFieldDB>();
        private string connectionString = string.Empty;
        public DataWarehouseViewsService(string connectionString)
        {
            this.connectionString = connectionString;
            DwObjectFieldLists = GetDwObjectFieldLists();
            BuildDataWarehouseViewLists();
        }

        public void BuildDataWarehouseViewLists()
        {
            var factTables = GetDataTableFromSql(connectionString, "select Code,DataViewName from DWObjectTables where TypeCode = 'Fact'");
            foreach (DataRow row in factTables.AsEnumerable())
            {
                string factCode = row["Code"]!=null ? row["Code"].ToString() : "";
                string viewName = row["DataViewName"] != null ? row["DataViewName"].ToString() : "";
                if (!string.IsNullOrEmpty(viewName))
                {
                    foreach (DWObjectFieldDB field in DwObjectFieldLists.Where(d => d.DWObjectTableCode == factCode && d.DataTypeCode == "Dimension" && d.DimensionTableCode != "DIM_Dates").ToList())
                    {
                        CreateDimensionDataView(field);
                    }

                    CreateFactDataView(factCode, viewName);
                }
            }
        }
       
        private void CreateFactDataView(string factCode, string viewName)
        {
            var warehouseView = new WarehouseView() { ViewName = viewName, IsFactView = true,SqlString = " CREATE VIEW " + viewName + " AS SELECT " };
            foreach (DWObjectFieldDB dwObjectFieldDB in DwObjectFieldLists.Where(d => d.DWObjectTableCode == factCode).ToList())
            {
                if (dwObjectFieldDB.DimensionTableCode == "DIM_Dates")
                {
                    warehouseView.SqlString += " " + "CASE WHEN " + dwObjectFieldDB.FieldCode + " ='1-1-1' or  " + dwObjectFieldDB.FieldCode + " ='2-2-2' or  " + dwObjectFieldDB.FieldCode + " ='3-3-3'  THEN null ELSE " + dwObjectFieldDB.FieldCode + " END " + " as ";
                }
                else warehouseView.SqlString += " " + dwObjectFieldDB.FieldCode + " as ";

                string displayName = !string.IsNullOrEmpty(dwObjectFieldDB.ViewFieldDisplayName) ? dwObjectFieldDB.ViewFieldDisplayName : ConvertStringToCamelCase(GetFieldNameFromCode(dwObjectFieldDB.FieldCode)).Replace("(", "In").Replace(")", "");
                warehouseView.SqlString += (!string.IsNullOrEmpty(dwObjectFieldDB.DimensionTableCode) && dwObjectFieldDB.DimensionTableCode != "DIM_Dates" ? displayName + "Key" : displayName);
                warehouseView.SqlString += ",";
            }
            warehouseView.IsHaveCustomFields = CheckIfFactHaveCustomField(factCode);
            warehouseView.SqlString = warehouseView.SqlString.Remove(warehouseView.SqlString.Length - 1);
            warehouseView.SqlString +=((warehouseView.IsHaveCustomFields ? ",@CustomFields":"") +  " FROM " + factCode);
            DataWarehouseViewLists.Add(warehouseView);
        }

        private void CreateDimensionDataView(DWObjectFieldDB field)
        {
            string parentfieldName = !string.IsNullOrEmpty(field.ViewFieldDisplayName) ? field.ViewFieldDisplayName : GetFieldNameFromCode(field.FieldCode);
            string viewName = !string.IsNullOrEmpty(field.DimensionDataViewName) ?  field.DimensionDataViewName: GetViewName(parentfieldName, "Dim");
            if (DataWarehouseViewLists.Where(d => d.ViewName == viewName).FirstOrDefault() == null)
            {
                var warehouseView = new WarehouseView() {ViewName = viewName , SqlString  = " CREATE VIEW " + viewName + " AS SELECT " };
                foreach (DWObjectFieldDB dwObjectFieldDB in DwObjectFieldLists.Where(d => d.DWObjectTableCode == field.DimensionTableCode).ToList())
                {
                    warehouseView.SqlString += " " + dwObjectFieldDB.FieldCode + " as ";
                    if (dwObjectFieldDB.IsPrimaryKey) warehouseView.SqlString += (parentfieldName + "Key");
                    else if (dwObjectFieldDB.FieldCode == "[Code]" || dwObjectFieldDB.FieldCode == "[Email]" ||  dwObjectFieldDB.FieldCode == "[Source Tenant]") warehouseView.SqlString += parentfieldName + (!string.IsNullOrEmpty(dwObjectFieldDB.ViewFieldDisplayName) ? dwObjectFieldDB.ViewFieldDisplayName : GetFieldNameFromCode(dwObjectFieldDB.FieldCode));
                    else if (!string.IsNullOrEmpty(dwObjectFieldDB.ViewFieldDisplayName)) warehouseView.SqlString += dwObjectFieldDB.ViewFieldDisplayName;
                    else warehouseView.SqlString += ConvertStringToCamelCase(GetFieldNameFromCode(dwObjectFieldDB.FieldCode)).Replace("(", "In").Replace(")", "");
                    warehouseView.SqlString += ",";
                }
                warehouseView.SqlString  = warehouseView.SqlString.Remove(warehouseView.SqlString.Length - 1);
                warehouseView.SqlString += (" FROM " + field.DimensionTableCode);
                DataWarehouseViewLists.Add(warehouseView);
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

        private List<DWObjectFieldDB> GetDwObjectFieldLists()
        {
            var result = new List<DWObjectFieldDB>();
            DataTable dWObjectFieldsMetaData = GetDataTableFromSql(connectionString, "SELECT  Code,IsCustom, IsPrimaryKey, DWObjectTableCode,ViewFieldDisplayName,DimensionDataViewName, DataTypeCode ,DimensionTableCode from DWObjectFields where DontDisplayInView =0 and IsCustom=0");
            foreach (DataRow row in dWObjectFieldsMetaData.AsEnumerable())
            {
                var fieldDB = new DWObjectFieldDB();
                fieldDB.FieldCode = row.Table.Columns.Contains("Code") ? row["Code"].ToString() : "";
                fieldDB.DataTypeCode = row.Table.Columns.Contains("DataTypeCode") ? row["DataTypeCode"].ToString() : "";
                fieldDB.DimensionTableCode = row.Table.Columns.Contains("DimensionTableCode") ? row["DimensionTableCode"].ToString() : "";
                fieldDB.DimensionDataViewName = row.Table.Columns.Contains("DimensionDataViewName") ? row["DimensionDataViewName"].ToString() : "";
                fieldDB.ViewFieldDisplayName = row.Table.Columns.Contains("ViewFieldDisplayName") ? row["ViewFieldDisplayName"].ToString() : "";
                fieldDB.DWObjectTableCode = row.Table.Columns.Contains("DWObjectTableCode") ? row["DWObjectTableCode"].ToString() : "";
                fieldDB.IsCustom = row.Table.Columns.Contains("IsCustom") ? bool.Parse(row["IsCustom"].ToString()) : false;
                fieldDB.IsPrimaryKey = row.Table.Columns.Contains("IsPrimaryKey") ? bool.Parse(row["IsPrimaryKey"].ToString()) : false;
                result.Add(fieldDB);
            }
            return result;
        }
    }

    public class WarehouseView
    {
        public string ViewName { get; set; }
        public string SqlString { get; set; }
        public bool IsFactView { get; set; }
        public bool IsHaveCustomFields { get; set; }

    }

    public class DWObjectFieldDB
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

        

    }


}
