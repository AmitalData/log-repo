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
            DeleteDataWarehouseViews(privateViewArgs);

            var dataWarehouseViews = GetDataWarehouseViewsListsByTenant(privateViewArgs.Tenant);
            
            if (!privateViewArgs.IsParentTenant)
            {
                List<WarehouseView> customFieldViewLists = GetCustomFieldViewsLists(privateViewArgs.Tenant, dataWarehouseViews);
                customFieldViewLists = customFieldViewLists.GroupBy(d => d.ViewName).Select(d => d.First()).ToList();

                dataWarehouseViews = dataWarehouseViews.Concat(customFieldViewLists).ToList();
            }

            foreach (WarehouseView view in dataWarehouseViews)
            {
                string viewscript = view.SqlString;
                if (view.IsFactView && view.HasCustomFields)
                {
                    viewscript = view.SqlString.Replace(",@CustomFields", view.CustomFieldScriptSQL);
                }
                CreateView(privateViewArgs.ConnectionString, viewscript);
                if (privateViewArgs.ApplyGrantOnViews) GrantView(view.ViewName, privateViewArgs);
            }
        }

        private List<WarehouseView> GetCustomFieldViewsLists(int tenant, List<WarehouseView> dataWarehouseViews)
        {
            List<WarehouseView> customFieldViewsLists = new List<WarehouseView>();

            foreach (WarehouseView view in dataWarehouseViews.Where(d=>d.HasCustomFields))
            {
                CustomFieldDataWarehouseArgs customFieldDataWarehouseArgs = new CustomFieldDataWarehouseArgs()
                {
                    ConnectionString = sourceConnectionString,
                    DWObjectFieldLists = DwObjectFieldLists,
                    Tenant = tenant,
                    WarehouseView = view

                };
                CustomFieldDataWarehouseViewService customFieldViewDataWarehouseService = new CustomFieldDataWarehouseViewService(customFieldDataWarehouseArgs);
                List<WarehouseView> customFieldViews = customFieldViewDataWarehouseService.GetCustomFieldViewLists();
                customFieldViewsLists = customFieldViewsLists.Concat(customFieldViews).ToList();
                view.CustomFieldScriptSQL = customFieldViewDataWarehouseService.GetCustomFieldsAsSqlString();
            }

            return customFieldViewsLists;
        }


        private List<WarehouseView> GetDataWarehouseViewsListsByTenant(int tenant)
        {
            FeaturePrivateDataWarehouseService featurePrivateDataWarehouseService = new FeaturePrivateDataWarehouseService(sourceConnectionString.Replace("Main", "Global"), sourceConnectionString , tenant);
            List<WarehouseView> dataWarehouseViews = new List<WarehouseView>();
            foreach (WarehouseView factView in DataWarehouseViewLists.Where(d => d.IsFactView).ToList())
            {
                if (featurePrivateDataWarehouseService.CheckFeature("BIReport." + factView.ViewCode))
                {
                    dataWarehouseViews = dataWarehouseViews.Concat(DataWarehouseViewLists.Where(d => !d.IsFactView && d.FactConnectedCodeLists.Contains(factView.ViewCode)).ToList()).ToList();
                    dataWarehouseViews.Add(factView);

                }
            }

            return dataWarehouseViews.GroupBy(d => d.ViewName).Select(d => d.FirstOrDefault()).ToList();
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


        private void DeleteDataWarehouseViews(PrivateViewArgs privateViewArgs)
        {
            string deleteViewsSql = "DECLARE @sql VARCHAR(MAX) = '', @crlf VARCHAR(2) = CHAR(13) + CHAR(10); SELECT @sql = @sql + 'DROP VIEW ' + QUOTENAME(SCHEMA_NAME(schema_id)) + '.' + QUOTENAME(v.name) + ';' + @crlf FROM sys.views v where  v.name !='database_firewall_rules'  PRINT @sql;EXEC(@sql);";
            RunSql(privateViewArgs.ConnectionString, deleteViewsSql);
        }
    }


    public class PrivateViewArgs
    {
        public int Tenant { get; set; }
        public string ConnectionString { get; set; }
        public string UserName { get; set; }
        public string Catalog { get; set; }
        public bool ApplyGrantOnViews { get; set; }
        public bool IsParentTenant { get; set; }

        

    }

}
