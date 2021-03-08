using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDataViews.Service
{
    public class GeneralDataWarehouseViewsService
    {
        public string GetViewName(string fieldName, string viewType)
        {
            string viewName = viewType == "Dim" ? "dim" : viewType == "Fact" ? "fact" : viewType == "Custom" ? "c_dim" : "";
            viewName += fieldName;
            return PluralViewName(viewName);
        }

        public string PluralViewName(string viewName)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(viewName))
            {
                var lastOneCharacter = viewName.ToLower().Substring(viewName.Length - 1);
                var lastSecondCharacter = viewName.ToLower().Substring(viewName.Length - 2, 1);
                var lastTwoCharcter = lastSecondCharacter + lastOneCharacter;

                if (lastOneCharacter == "s" || lastOneCharacter == "x" || lastOneCharacter == "z" || (lastTwoCharcter) == "ch" || lastTwoCharcter == "sh") result = viewName + "es";
                else if (lastOneCharacter == "y" && !"a,3,i,o,u".Split(',').Contains(lastSecondCharacter)) result = viewName.Substring(0, viewName.Length - 1) + "ies";
                else if (lastOneCharacter == "f" && lastTwoCharcter == "fe") result = lastOneCharacter == "f" ? viewName.Substring(0, viewName.Length - 1) + "ves" : viewName.Substring(0, viewName.Length - 2) + "ves";
                else if (lastOneCharacter == "o" && !"a,3,i,o,u".Split(',').Contains(lastSecondCharacter)) result = viewName + "es";
                else result = viewName + "s";
            }

            return result;

        }

        public string GetFieldNameFromCode(string fieldCode)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(fieldCode))
            {
                result = fieldCode.Replace("[", "");
                result = result.Replace("]", "");
                result = result.Replace(" ", "");
            }

            return result;
        }

        public string ConvertStringToCamelCase(string value)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                result = value.Replace(" ", "");
            }
            return result;
        }

        public DataTable GetDataTableFromSql(string connectionString, string sqlString)
        {
            var result = new DataTable();
            using (SqlConnection sourceConnection = new SqlConnection(connectionString))
            {
                sourceConnection.Open();
                SqlCommand commandSourceData = new SqlCommand(sqlString, sourceConnection);
                SqlDataReader reader = commandSourceData.ExecuteReader();
                result.Load(reader);
                reader.Close();

            }
            return result;
        }

        public void RunSql(string connectionString, string sqlString)
        {
            using (SqlConnection sourceConnection = new SqlConnection(connectionString))
            {
                sourceConnection.Open();
                SqlCommand commandSourceData = new SqlCommand(sqlString, sourceConnection);
                SqlDataReader reader = commandSourceData.ExecuteReader();
                reader.Close();
            }
        }

        public string GetSqlFieldDataType(DataRow field)
        {
            string sqlFieldtype = string.Empty;
            string dataTypeCode = field["DataTypeCode"].ToString();
            int maxLength = field["MaxLength"] != null ? int.Parse(field["MaxLength"].ToString()) : 0;
            if ((dataTypeCode == "nText" || dataTypeCode == "Text") && maxLength == 0) maxLength = 100;
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


        public string BuildConnectionString(string catalog, string userName, string password, string server)
        {
            string result = "Data Source=" + server + ";Initial Catalog=" + catalog + ";Integrated Security=False;Persist Security Info=True;User ID=" + userName + ";Password= " + password + ";MultipleActiveResultSets=True;Connect Timeout=60";
            return result;
        }

        public List<string> GetEnvironmentFactTables(string sourceConnection)
        {
            List<string> factTables = new List<string>();
            DataTable dataTable = GetDataTableFromSql(sourceConnection, "select FactCodes from DWHEnvironmentSettings");
            if (dataTable != null && dataTable.Rows != null)
            {
                factTables = dataTable.Rows[0]["FactCodes"].ToString().Split(',').ToList();
            }
            return factTables;
        }


    }
}
