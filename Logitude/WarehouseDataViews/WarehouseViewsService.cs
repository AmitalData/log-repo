using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDataViews
{
    public class WarehouseViewsService
    {
        long timeOut = 10000000000000000;


        public string BuildConnectionString(string dbSourceConnection)
        {
            var connectionStringArray =  dbSourceConnection.Split(',');
            string catalog = connectionStringArray[0];
            string userName = connectionStringArray[1];
            string password = connectionStringArray[2];
            string server = connectionStringArray[3];
            string result = "Data Source=" + server + ";Initial Catalog=" + catalog + ";Integrated Security=False;Persist Security Info=True;User ID=" + userName + ";Password= " + password + ";MultipleActiveResultSets=True;Connect Timeout=60";
            return result;
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

        public string ReadscriptFile(string fileName)
        {
            string result = string.Empty;
            string path = System.IO.Path.GetDirectoryName(new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            if (path.Contains(@"\bin\Debug")) path = path.Replace(@"\bin\Debug", string.Empty);

            string fileDirectory = Path.Combine(path, "WarehouseViewsScript\\", fileName + ".sql");
            FileInfo file = new FileInfo(fileDirectory);
            result = file.OpenText().ReadToEnd();
            return result;
        }

        public void ExecuteSql(string sqlString, string connectionString)
        {

            if (!string.IsNullOrEmpty(sqlString))
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(sqlString, cn);
                    sqlCommand.CommandTimeout = (int)timeOut;
                    cn.Open();
                    sqlCommand.ExecuteNonQuery();
                    cn.Close();
                }
            }
        }

        public void CreateDimensionView(string fieldCode, string tableCode, string connectionString)
        {
            string scriptView = GenerateScriptView(fieldCode, tableCode);
            ExecuteSql(scriptView,  connectionString);
        }


        public void CreateFactView(string tableCode, string connectionString , string destinationConnectionString)
        {
            string scriptView = GenerateScriptView("Shipment" , tableCode);
            scriptView = ConvertFieldsNameToCamelCase(scriptView);
            DataTable customFields = GetCustomObjectFields(connectionString);
            string customFieldScript = string.Empty;
            foreach (var customField in customFields.AsEnumerable().ToList())
            {
                string fieldDisplay = customField["DefaultText"].ToString();
                string fieldName = customField["FieldName"].ToString();
                if (customField["DataTypeCode"].ToString() != "PickList" && customField["DataTypeCode"].ToString() != "LookUp" && customField["DataTypeCode"].ToString() != "Date")
                {
                    customFieldScript += ",CONVERT(" + GetDataWarehouseSqlFieldType(customField) + ",[" + fieldName + "]) as " + "[" + fieldDisplay + "]";
                }
            }
            scriptView = scriptView.Replace(",@CustomFields", customFieldScript);
            ExecuteSql(scriptView, destinationConnectionString);
        }



        private string GetDataWarehouseSqlFieldType(DataRow field)
        {
            string sqlFieldtype = string.Empty;
            string dataTypeCode = field["DataTypeCode"].ToString();
            int maxLength = field["MaxLength"]!=null ? int.Parse(field["MaxLength"].ToString()) :0;
            if (dataTypeCode == "nText") sqlFieldtype += "nvarchar(" + maxLength + ")";
            else if (dataTypeCode == "Text") sqlFieldtype += "varchar(" + maxLength + ")";
            else if (dataTypeCode == "Boolean") sqlFieldtype += " bit";
            else if (dataTypeCode == "Decimal" || dataTypeCode == "Double") sqlFieldtype += " float";
            else if (dataTypeCode == "Integer") sqlFieldtype += " int";
            else if (dataTypeCode == "DateTime") sqlFieldtype += " dateTime";
            else if (dataTypeCode == "Date") sqlFieldtype += " date";
            return sqlFieldtype;
        }


        private DataTable GetCustomObjectFields(string connectionString)
        {
            var customObjectFields = new DataTable();
            int tenant = 1;
            using (SqlConnection sourceConnection = new SqlConnection(connectionString))
            {
                sourceConnection.Open();
                string sql = "SELECT  MaxLength ,FieldName,  DataTypeCode,TextCodes.DefaultText from  ObjectFields inner join TextCodes on ObjectFields.FullNameTextCodeCode = TextCodes.Code where ObjectFields.IsCustom = 1 and ObjectFields.Tenant =" + tenant + " and ObjectFields.ObjectTableId =(select id from ObjectTables where Name = 'Shipment')";
                SqlCommand commandSourceData = new SqlCommand(sql, sourceConnection);
                SqlDataReader reader = commandSourceData.ExecuteReader();
                customObjectFields.Load(reader);
                reader.Close();
            }

            return customObjectFields;
        }

        public string GenerateScriptView(string fieldCode  , string tableCode)
        {
            string scriptView = string.Empty;
            string viewName = GetViewName(fieldCode);
            string scriptstring = ReadscriptFile(tableCode);

            scriptstring = scriptstring.Replace("[Key]", viewName + "key");
            scriptstring = scriptstring.Replace("[Code]", "[Code] as [" + viewName.Replace("View", "") + " Code]");
            scriptView = " CREATE VIEW " + viewName + " AS  " + scriptstring;

            return scriptView;
        }

        private string ConvertFieldsNameToCamelCase(string scriptstring)
        {
            string result = scriptstring;
            string[] sqlArray = result.Split(new string[] { "SELECT" }, StringSplitOptions.None);
            sqlArray = sqlArray[1].Split(new string[] { "FROM" }, StringSplitOptions.None);
            var allFields = sqlArray[0].Split(',');
            foreach (string fieldName in allFields)
            {
                if (!string.IsNullOrEmpty(fieldName))
                {
                    if (!fieldName.Contains("Key") && !fieldName.Contains("Id_Number") && !fieldName.Contains("@CustomFields"))
                    {
                        string fieldNameCamelCase = fieldName + "as " + fieldName.Replace(" ", "");
                        result = result.Replace(fieldName, fieldNameCamelCase);
                    }
                }
            }
            return result;
        }

        public void GrantView(string fieldCode,  string destinationConnectionString)
        {
            string viewName = GetViewName(fieldCode);
            string sqlstring = "GRANT SELECT  ON [UnicargoDW].[dbo].["+ viewName + "] TO [UnicargoDBUser]";
            ExecuteSql(sqlstring, destinationConnectionString);
        }

        public void DropView(string fieldCode, string connectionString)
        {
            string viewName = GetViewName(fieldCode);
            string sqlstring = "if exists(select 1 from sys.views where name='" + viewName + "' and type='v') begin drop view " + viewName + ";end";
            ExecuteSql(sqlstring, connectionString);
        }

        public string GetViewName(string fieldCode)
        {
            string viewName = fieldCode;
            viewName = viewName.Replace("[", "");
            viewName = viewName.Replace("]", "");
            viewName = viewName.Replace(" ", "");
            viewName += "View";
            return viewName;
        }
    }
}
