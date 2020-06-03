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

        public string GetViewName(string fieldName ,string viewType)
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
                var lastSecondCharacter = viewName.ToLower().Substring(viewName.Length-2 ,1);
                var lastTwoCharcter = lastSecondCharacter + lastOneCharacter;

                if (lastOneCharacter == "s" || lastOneCharacter == "x" || lastOneCharacter == "z" || (lastTwoCharcter) == "ch" || lastTwoCharcter == "sh") result = viewName + "es";
                else if (lastOneCharacter == "y" && !"a,3,i,o,u".Split(',').Contains(lastSecondCharacter)) result = viewName.Substring(0, viewName.Length - 1) + "ies";
                else if (lastOneCharacter == "f" && lastTwoCharcter == "fe") result = lastOneCharacter == "f" ? viewName.Substring(0, viewName.Length - 1) + "ves" : viewName.Substring(0, viewName.Length - 2) + "ves";
                else if (lastOneCharacter == "o" && !"a,3,i,o,u".Split(',').Contains(lastSecondCharacter)) result = viewName + "es";
                else result = viewName + "s";

                if (viewName.ToLower() == "dimcreatedby" || viewName.ToLower() == "dimoperationalclosedby" || viewName.ToLower() == "dimdirecthouse" || viewName.ToLower() == "dimnotifyone" || viewName.ToLower() == "dimnotifytwo" || viewName.ToLower() == "dimspecialservices")
                {
                    result = viewName;
                }
                else if (viewName.ToLower() == "dimsalesman") result = "dimSalesmen";
                else if (viewName.ToLower() == "dimconsigneenotimporter")
                {

                    result = "dimConsigneesNotImportes";
                }


                else if (viewName.ToLower() == "dimshippernotexporter")
                {
                    result = "dimShippersNotExporters";
                }

            }

            return result;

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
                    if (!fieldName.Contains("Key") && !CheckIfContainesAsString(fieldName)  && !fieldName.Contains("@CustomFields"))
                    {
                        string fieldNameCamelCase = fieldName + "as " + ConvertStringToCamelCase(fieldName);
                        result = result.Replace(fieldName, fieldNameCamelCase);
                    }
                }
            }
            return result;
        }

        public string RemoveBowsFromFieldsName(string scriptstring)
        {
            string result = scriptstring;
            string[] sqlArray = result.Split(new string[] { "SELECT" }, StringSplitOptions.None);
            sqlArray = sqlArray[1].Split(new string[] { "FROM" }, StringSplitOptions.None);
            var allScriptLines = sqlArray[0].Split(',');
            foreach (string line in allScriptLines)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    if (line.Contains("(")  && line.Contains(")"))
                    {
                        string fieldName = GetFieldNameFromScriptLine(line);
                        if (!string.IsNullOrEmpty(fieldName))
                        {
                            string fieldNameWithOutBows = fieldName.Replace("(", "In").Replace(")", "");
                            result = result.Replace(fieldName, fieldNameWithOutBows);
                        }
                    }
                }
            }
            return result;
        }

        private string GetFieldNameFromScriptLine(string scriptLine)
        {
            string result = string.Empty;
            var fieldName = scriptLine.Split(new string[] { "as [" }, StringSplitOptions.None);
            if (fieldName.Length == 1) fieldName = scriptLine.Split(new string[] { "as  [" }, StringSplitOptions.None);
            if (fieldName.Length == 1) fieldName = scriptLine.Split(new string[] { "as[" }, StringSplitOptions.None);
            if (fieldName.Length > 1) result = ("[" + fieldName[1]);
            return result;
        }



        private bool CheckIfContainesAsString(string fieldName)
        {
            return (fieldName.Contains("] as") || fieldName.Contains("]  as") || fieldName.Contains("]as")) ? true : false;

        }

       public string ConvertStringToCamelCase(string value)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                //TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
               // result = textInfo.ToTitleCase(value.ToLower());
                result = value.Replace(" ","" );
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

    }
}
