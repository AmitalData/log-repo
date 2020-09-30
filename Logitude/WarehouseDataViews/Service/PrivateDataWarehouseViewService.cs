using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDataViews.Service
{
    public class PrivateDataWarehouseViewService : DataWarehouseViewsService
    {

        private string sourceConnectionString = string.Empty;
        public PrivateDataWarehouseViewService(string sourceConnectionString) : base(sourceConnectionString)
        {
            this.sourceConnectionString = sourceConnectionString;
        }

        public void GeneratePrivateViews(PrivateViewArgs privateViewArgs)
        {
            var  customFieldViewDataWarehouseService = new CustomFieldDataWarehouseViewService(sourceConnectionString, DwObjectFieldLists, privateViewArgs.Tenant);
            List<WarehouseView> customFieldViewLists = customFieldViewDataWarehouseService.GetCustomFieldViewLists();
            List<WarehouseView> allDataWarehouseViews = DataWarehouseViewLists.Concat(customFieldViewLists).ToList();
            foreach (WarehouseView view in allDataWarehouseViews)
            {
                string viewscript = view.SqlString;
                if (view.IsFactView && view.IsHaveCustomFields)
                {
                    string customFieldScript = customFieldViewDataWarehouseService.GetCustomFieldsAsSqlString();
                    viewscript = view.SqlString.Replace(",@CustomFields", customFieldScript);
                }
                DropView(view.ViewName, privateViewArgs.ConnectionString);
                CreateView(privateViewArgs.ConnectionString, viewscript);
                if (privateViewArgs.ApplyGrantOnViews) GrantView(view.ViewName, privateViewArgs);
            }

        }

        private void DropView(string viewName, string connectionString)
        {
            string sqlstring = "if exists(select 1 from sys.views where name='" + viewName + "' and type='v') begin drop view " + viewName + ";end";
            RunSql(connectionString, sqlstring);
        }
        private void CreateView(string connectionString, string sqlString)
        {
            RunSql(connectionString, sqlString);

        }
        private void GrantView(string viewName, PrivateViewArgs privateViewArgs)
        {
            //string sqlstring = "GRANT SELECT  ON [UnicargoDW].[dbo].[" + viewName + "] TO [UnicargoDBUser]"; // Pre
            // string sqlstring = "GRANT SELECT  ON [T570Unicargo].[dbo].[" + viewName + "] TO [U570gmxaU]";   //Online 
            string sqlstring = "GRANT SELECT  ON [" + privateViewArgs.Catalog + "].[dbo].[" + viewName + "] TO [" + privateViewArgs.UserName + "]"; // Pre
            RunSql(privateViewArgs.ConnectionString, sqlstring);
        }

    }


    public class PrivateViewArgs
    {
        public int Tenant { get; set; }
        public string ConnectionString { get; set; }
        public string UserName { get; set; }
        public string Catalog { get; set; }
        public bool ApplyGrantOnViews { get; set; }

        
    }

}
