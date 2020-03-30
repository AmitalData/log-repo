using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDataViews.Service
{
  public  class GeneralWarehouseViewsService
    {
        long timeOut = 10000000000000000;
        public string BuildConnectionString(string dbSourceConnection)
        {
            var connectionStringArray = dbSourceConnection.Split(',');
            string catalog = connectionStringArray[0];
            string userName = connectionStringArray[1];
            string password = connectionStringArray[2];
            string server = connectionStringArray[3];
            string result = "Data Source=" + server + ";Initial Catalog=" + catalog + ";Integrated Security=False;Persist Security Info=True;User ID=" + userName + ";Password= " + password + ";MultipleActiveResultSets=True;Connect Timeout=60";
            return result;
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

        public string ReadScriptFile(string fileName)
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

        public string ConvertFieldsNameToCamelCase(string scriptstring)
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
                        string fieldNameCamelCase = fieldName + "as " + ConvertStringToCamelCase(fieldName);
                        result = result.Replace(fieldName, fieldNameCamelCase);
                    }
                }
            }
            return result;
        }

       public string ConvertStringToCamelCase(string value)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                result = textInfo.ToTitleCase(value.ToLower());
                result = result.Replace(" ","" );
            }
            return result;
        }

    }
}
