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

        private List<WarehouseView> GetOldWarehouseView(string connectionString)
        {
          var privateDBConnectionString =  GetPrivateDBConnectionString(connectionString);

            var result = new List<WarehouseView>();
            if (!string.IsNullOrEmpty(privateDBConnectionString))
            {
                var oldViews = GetDataTableFromSql(privateDBConnectionString, "select v.name as view_name,   m.definition from sys.views v join sys.sql_modules m  on m.object_id = v.object_id");
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

       

        public string Compare()
        {
            //Phase1 Compare View Name
            StringBuilder results = new StringBuilder("_________Removed or Rename Views ______________ \n\r");
            foreach (WarehouseView view in oldWarehouseView)
            {
                if (!ChecKIfViewAvailable(view.ViewName)) results.Append(view.ViewName + " is removed \n\r");
            }
            results.Append("______________________________ \n\r");


            //Phase2 Compare Fields Name

            results.Append("_________Removed or Rename Fields___________ \n\r");

            foreach (WarehouseView view in oldWarehouseView)
            {
                if (ChecKIfViewAvailable(view.ViewName))
                {
                    foreach (string item in view.SqlString.Split(new string[] { view.ViewName }, StringSplitOptions.None)[1].Split(','))
                    {
                        if (!item.Contains("CONVERT"))
                        {
                            var fieldName = item.Split(new string[] { "FROM" }, StringSplitOptions.None)[0].Split(new string[] { " as " }, StringSplitOptions.None)[1];
                            fieldName = fieldName.Replace(" ", "");
                            if (!fieldName.Contains("c_"))
                            {
                                if (!string.IsNullOrEmpty(fieldName) && !ChecKIfFieldAvailable(view.ViewName, fieldName))
                                {
                                    results.Append(fieldName + " field was removed from " + view.ViewName + "\n\r");

                                }
                            }
                        }

                    }
                }
                
            }

            results.Append("______________________________ \n\r");


            //Phase3 Compare Data Type Fields 

            results.Append("_________Data Type Fields___________ \n\r");

            foreach (WarehouseView view in oldWarehouseView)
            {
                if (ChecKIfViewAvailable(view.ViewName))
                {
                    foreach (string item in view.SqlString.Split(new string[] { view.ViewName }, StringSplitOptions.None)[1].Split(','))
                    {
                        if (!item.Contains("CONVERT"))
                        {
                            var fieldName = item.Split(new string[] { "FROM" }, StringSplitOptions.None)[0].Split(new string[] { " as " }, StringSplitOptions.None)[1];
                            fieldName = fieldName.Replace(" ", "");
                            if (!fieldName.Contains("c_"))
                            {
                                if (!string.IsNullOrEmpty(fieldName) && ChecKIfFieldAvailable(view.ViewName, fieldName) && ChecKIfFieldDataTypeChange(view.ViewName, fieldName))
                                {
                                    results.Append("Data type changed for "+fieldName + " field in " + view.ViewName + "\n\r");

                                }
                            }
                        }

                    }
                }

            }

            results.Append("______________________________ \n\r");

            return results.ToString();

        }

        private bool ChecKIfFieldDataTypeChange(string viewName, string fieldName)
        {
            bool result = false;
            var warehouseView = DataWarehouseViewLists.Where(d => d.ViewName == viewName).FirstOrDefault();
            if (warehouseView != null && warehouseView.Fields != null)
            {
                var field = warehouseView.Fields.Where(d => d.FieldName == fieldName).FirstOrDefault();
                if (field != null)
                {
                    var x = oldDwObjectFieldLists.Where(d => d.DWObjectTableCode == field.DWObjectTableCode && d.FieldCode == field.FieldCode).FirstOrDefault();
                    if (x != null)
                    {
                        if(x.DataTypeCode != field.DataTypeCode)
                        {
                            result = true;
                        }
                    }
                }

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

    }
}
