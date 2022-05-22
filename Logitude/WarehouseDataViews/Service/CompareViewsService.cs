using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WarehouseDataViews.Service
{
   public class CompareViewsService: DataWarehouseViewsService
    {
        private  List<WarehouseView> oldWarehouseView = new List<WarehouseView>();
        private List<DWObjectFieldItem> oldDwObjectFieldLists = new List<DWObjectFieldItem>();


        public CompareViewsService(string sourceConnection, string destinationConnection) : base(sourceConnection)
        {
            oldDwObjectFieldLists = GetDwObjectFieldLists(destinationConnection);
            oldWarehouseView = GetOldWarehouseView(destinationConnection);

        }


       

        public string Compare()
        {
            StringBuilder results = new StringBuilder();
            results.Append(CompareViewName());
            results.Append(CompareFieldsName());
            results.Append(CompareDataTypeFields());
            return results.ToString();

        }

        private string CompareViewName()
        {
            var result = new StringBuilder("_________Removed or Rename Views ______________ \n\r");
            foreach (WarehouseView view in oldWarehouseView)
            {
                if (!ChecKIfViewAvailable(view.ViewName)) result.Append(view.ViewName + " was removed \n\r");
            }
            result.Append("______________________________ \n\r");
            return result.ToString();
        }

        private string CompareFieldsName()
        {
            var result = new StringBuilder("_________Removed or Rename Fields___________ \n\r");

            foreach (WarehouseView view in oldWarehouseView)
            {
                if (ChecKIfViewAvailable(view.ViewName))
                {
                    foreach (string item in view.SqlString.Split(new string[] { view.ViewName }, StringSplitOptions.None)[1].Split(','))
                    {
                        if (!item.Contains("CONVERT"))
                        {
                            var fieldName  = GetFieldName(item);
                            if (!fieldName.Contains("c_"))
                            {
                                if (!string.IsNullOrEmpty(fieldName) && !ChecKIfFieldAvailable(view.ViewName, fieldName))
                                {
                                    result.Append(fieldName + " field was rename or removed from " + view.ViewName + "\n\r");
                                }
                            }
                        }

                    }
                }

            }

            result.Append("______________________________ \n\r");
            return result.ToString();
        }

        private  string GetFieldName(string item)
        {
            string result = string.Empty;
            var x = item.Split(new string[] { "FROM" }, StringSplitOptions.None)[0];
            if (x.Contains(" as ")) result = x.Split(new string[] { " as " }, StringSplitOptions.None)[1];
            else if (x.Contains("]as ")) result = x.Split(new string[] { "]as" }, StringSplitOptions.None)[1];
            return result.Replace(" ", "").Replace("[","").Replace("]","").Replace("\r\n","").Replace("\t","");
        }

        private string CompareDataTypeFields()
        {
            var result = new StringBuilder("_________Data Type Fields___________ \n\r");
            foreach (WarehouseView view in oldWarehouseView)
            {
                if (ChecKIfViewAvailable(view.ViewName))
                {
                    foreach (string item in view.SqlString.Split(new string[] { view.ViewName }, StringSplitOptions.None)[1].Split(','))
                    {
                        if (!item.Contains("CONVERT"))
                        {
                            var fieldName = GetFieldName(item);
                            if (!fieldName.Contains("c_"))
                            {
                                if (!string.IsNullOrEmpty(fieldName) && ChecKIfFieldAvailable(view.ViewName, fieldName) && ChecKIfFieldDataTypeChange(view.ViewName, fieldName))
                                {
                                    result.Append("Data type changed for " + fieldName + " field in " + view.ViewName + "\n\r");

                                }
                            }
                        }

                    }
                }

            }

            result.Append("______________________________ \n\r");
            return result.ToString();
        }


        private bool ChecKIfFieldDataTypeChange(string viewName, string fieldName)
        {
            bool result = false;
            var warehouseView = DataWarehouseViewLists.Where(d => d.ViewName == viewName).FirstOrDefault();
            if (warehouseView != null && warehouseView.Fields != null)
            {
                var dwObjectfield = warehouseView.Fields.Where(d => d.FieldName == fieldName).FirstOrDefault();
                if (dwObjectfield != null)
                {
                    var oldDwObjectfield = oldDwObjectFieldLists.Where(d => d.DWObjectTableCode == dwObjectfield.DWObjectTableCode && d.FieldCode == dwObjectfield.FieldCode).FirstOrDefault();
                    if (oldDwObjectfield != null)
                    {
                        if(oldDwObjectfield.DataTypeCode != dwObjectfield.DataTypeCode) result = true;
                    }
                }

            }
            return result;
        }

        private bool ChecKIfViewAvailable(string viewName)
        {
            return DataWarehouseViewLists.Where(d => d.ViewName == viewName).Any();
        }

        private bool ChecKIfFieldAvailable(string viewName , string fieldName)
        {
            bool result = false;
            var warehouseView = DataWarehouseViewLists.Where(d => d.ViewName == viewName).FirstOrDefault();
            if (warehouseView != null && warehouseView.Fields!=null)
            {
                result = warehouseView.Fields.Where(d => d.FieldName == fieldName).Any();

            }
           
         

            return result;
        }


        private string GetPrivateDBConnectionString(string connectionString)
        {
            string result = string.Empty;
            var dWHSettingsTable = GetDataTableFromSql(connectionString, "SELECT top(1) * from  DWHSettings where Catalog is not null");
            foreach (DataRow row in dWHSettingsTable.Rows)
            {
                int tenant = Int32.Parse(row["Tenant"].ToString());
                string catalog = row["Catalog"].ToString();
                string userName = row["UserName"].ToString();
                string password = row["Password"].ToString();
                string server = row["Server"].ToString();
                string privateUserName = row["PrivateUserName"].ToString();
                result = BuildConnectionString(catalog, userName, password, server);

            }
            return result;
        }

        private List<WarehouseView> GetOldWarehouseView(string connectionString)
        {
            var privateDBConnectionString = GetPrivateDBConnectionString(connectionString);

            var result = new List<WarehouseView>();
            if (!string.IsNullOrEmpty(privateDBConnectionString))
            {
                var oldViews = GetDataTableFromSql(privateDBConnectionString, "select v.name as view_name,   m.definition from sys.views v join sys.sql_modules m  on m.object_id = v.object_id where v.name !='database_firewall_rules' and v.name !='ipv6_database_firewall_rules' ");
                foreach (DataRow row in oldViews.AsEnumerable())
                {
                    string viewName = row["view_name"] != null ? row["view_name"].ToString() : "";
                    string viewScript = row["definition"] != null ? row["definition"].ToString() : "";
                    if (!viewName.Contains("c_"))
                    {
                        var view = new WarehouseView() { ViewName = viewName, SqlString = viewScript };
                        result.Add(view);
                    }
                }
            }
            return result;
        }

    }
}
