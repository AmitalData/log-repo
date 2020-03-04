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

        public void CreateView(string fieldCode, string dimensionTableCode, string connectionString)
        {
            string viewName = GetViewName(fieldCode);
            string scriptstring = ReadscriptFile(dimensionTableCode);
            // [Key]
            scriptstring = scriptstring.Replace("[Key]", viewName+"key");

        //string sqlstring = "if exists(select 1 from sys.views where name=' " + viewName + "' and type='v') begin drop view " + viewName + ";end";
            string sqlstring = " CREATE VIEW "+ viewName + " AS  ";
            sqlstring += scriptstring;
            ExecuteSql(sqlstring,  connectionString);
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
