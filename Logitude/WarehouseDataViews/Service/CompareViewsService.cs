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
        string sourceConnection = string.Empty;
        string destinationConnection = string.Empty;
        List<WarehouseView> oldWarehouseView = new List<WarehouseView>();
        public CompareViewsService(string sourceConnection, string destinationConnection) : base(sourceConnection)
        {
            this.sourceConnection = sourceConnection;
            this.destinationConnection = destinationConnection;
            oldWarehouseView = GetOldWarehouseView();

        }

        private List<WarehouseView> GetOldWarehouseView()
        {
            var result = new List<WarehouseView>();
            var oldViews = GetDataTableFromSql(destinationConnection, "select v.name as view_name,   m.definition from sys.views v join sys.sql_modules m  on m.object_id = v.object_id");
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
            return result;
        }

        public string Compare()
        {
            //Phase1 Compare View Name
            StringBuilder results = new StringBuilder("_________Removed Views______________ \n\r");
            foreach (WarehouseView view in oldWarehouseView)
            {
                if (!ChecKIfViewAvailable(view.ViewName)) results.Append(view.ViewName + " is removed \n\r");
            }
            results.Append("______________________________ \n\r");


            //Phase2 Compare Fields Name

            results.Append("_________Removed Fields \n\r");

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
                                    results.Append(fieldName + " was removed in " + view.ViewName + "\n\r");

                                }
                            }
                        }

                    }
                }
                
            }
            results.Append("______________________________ \n\r");



            return results.ToString();

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
            else
            {

            }
         

            return result;
        }

    }
}
