using Logitude.BL.Helpers;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.TreeFilterQuery;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Helpers;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFreight.Web.GlobalModel;
using WebFreight.Web.MetaDataUpdate;

namespace Logitude.MetadataUpdate
{
    public class MetadataUpdateService
    {

        private Dictionary<int, string> _globalDBs = new Dictionary<int, string>();

        public void RunModulesUpdate(string moduleName, int? tenantId = null)
        {
            try
            {
                Console.WriteLine("Initializing Settings ...");
                InitializeSettings();
                Dictionary<int, string> processedDBConnections = new Dictionary<int, string>();
                bool multiDB = false;
                string firstDifferentDB = null;

                if (_globalDBs.Count > 1)
                    multiDB = true;
               
 
                foreach (var db in _globalDBs)
                {
                    int currentTenantId =   db.Key;
                    string globalDbId = db.Value;
            
                    if (tenantId.HasValue && currentTenantId != tenantId.Value)
                    {
                        continue;
                    }

                    if (processedDBConnections.ContainsKey(currentTenantId))
                    {
                        continue;
                    }

                    var dbConnection = db.Value;
              
                    if (processedDBConnections.ContainsValue(dbConnection))
                    {
                        continue;
                    }

                    var moduleToIncule = GetIncludeModules(DatabaseInitializer.GetConnection(dbConnection, dbConnection).ConnectionString);
                    if ((moduleToIncule.Modules.Contains("customs") && moduleToIncule.Include) && (!moduleToIncule.Modules.Contains("shipment") && moduleToIncule.Include))
                    {
                        Console.WriteLine($"Updating module '{moduleName}' for Tenant {currentTenantId}, DB Connection: {dbConnection}");
                        TenantsUpdateClass.UpdateDataForTenant(currentTenantId, "Customs", false, multiDB);
                    }
                    if ((moduleToIncule.Modules.Contains("shipment") && moduleToIncule.Include) && (moduleToIncule.Modules.Contains("customs") && moduleToIncule.Include) )
                    {
                        Console.WriteLine($"Updating module '{moduleName}' for Tenant {currentTenantId}, DB Connection: {dbConnection}");
                        TenantsUpdateClass.UpdateDataForTenant(currentTenantId, moduleName, false, multiDB);
                    }
                    if ((moduleToIncule.Modules.Contains("shipment") && moduleToIncule.Include) || (moduleToIncule.Modules.Contains("customs") && !moduleToIncule.Include) )
                    {
                        Console.WriteLine($"Updating module '{moduleName}' for Tenant {currentTenantId}, DB Connection: {dbConnection}");
                        TenantsUpdateClass.UpdateDataForTenant(currentTenantId, "UpdateTenantZeroNew", false, multiDB);
                    }
                     

                    Console.WriteLine($"Building Object table zip files for DB Connection: {dbConnection} ...");
                    bool isCustomsTenant = moduleToIncule.Modules.Contains("customs") && moduleToIncule.Include;
                    if (isCustomsTenant)
                    {
                        Console.WriteLine("Build ObjectTables Zip Files Data for customs:" + DateTime.Now.ToString());
                        TenantsUpdateClass.BuildObjectTablesZipFilesData(false, true, currentTenantId);
                    }
                    Console.WriteLine("Build ObjectTables Zip Files Data:" + DateTime.Now.ToString());
                    TenantsUpdateClass.BuildObjectTablesZipFilesData(false, false, currentTenantId);
                    if (isCustomsTenant)
                    {
                        Console.WriteLine("TableLastUpdateClass.UpdateCacheTableHistory:" + DateTime.Now.ToString());
                        BL.Helpers.TableLastUpdateClass.UpdateCacheTableHistory(currentTenantId);
                        Console.WriteLine("TenantsUpdateClass.UpdateTenants:" + DateTime.Now.ToString());
                        WebFreight.Web.MetaDataUpdate.TenantsUpdateClass.UpdateTenants(currentTenantId, multiDB);
                    }


                    Console.WriteLine($"Building zip files finished for DB Connection: {dbConnection} ...");

                    processedDBConnections.Add(currentTenantId, dbConnection);
                 }

                Console.WriteLine("Updating all modules and building zip files finished successfully");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                Console.WriteLine("Error: " + e.StackTrace);
                Console.WriteLine(e.InnerException?.Message);
                Console.WriteLine("Inner Exception Stack Trace:");
                Console.WriteLine(e.InnerException?.StackTrace);
                Environment.Exit(1);
            }
        }

        private void InitializeSettings()
        {
            string dbms = ConfigurationManager.AppSettings.Get("DBMS");
            LogitudeSettings.DatabaseManagementSystem = dbms;
            Console.WriteLine("Connected to " + dbms);
            Console.WriteLine(GetConnectionString());

            SettingRepository settingRepository = new SettingRepository();
            Setting setting = settingRepository.GetSingleSetting("1");
            LogitudeSettings.Id = setting.Id;
            LogitudeSettings.ChampEnv = setting.ChampEnv;
            LogitudeSettings.ChampURL = setting.ChampURL;
            LogitudeSettings.ChampTestAPIURL = setting.ChampTestAPIURL;
            LogitudeSettings.ChampTestAPIPassword = setting.ChampTestAPIPassword;
            LogitudeSettings.ChampProdAPIURL = setting.ChampProdAPIURL;
            LogitudeSettings.ChampProdAPIPassword = setting.ChampProdAPIPassword;
            LogitudeSettings.CustomerCareIP = setting.CustomerCareIP;
            LogitudeSettings.DeploymentStage = setting.DeploymentStage;
            LogitudeSettings.IsLogEnabled = setting.IsLogEnabled;
            LogitudeSettings.LogitudeURL = setting.LogitudeURL;
            LogitudeSettings.TotangoServiceId = setting.TotangoServiceId;
            LogitudeSettings.UsingAzure = setting.UsingAzure;
            LogitudeSettings.StorageAccountKey = setting.StorageAccountKey;
            LogitudeSettings.StorageAccountName = setting.StorageAccountName;
            LogitudeSettings.StorageType = setting.StorageType;
            LogitudeSettings.LogitudeCRMTenantNumber = setting.LogitudeCRMTenantNumber;
            LogitudeSettings.AutoSignupEmail = setting.AutoSignupEmail;
            LogitudeSettings.AutoSignupPassword = setting.AutoSignupPassword;
            LogitudeSettings.WorkEnvironment = setting.WorkEnvironment; // maybe we need to init more fields ?
            LogitudeSettings.StorageServiceMode = setting.StorageServiceMode;

            //if (LogitudeSettings.IsCostomsDeploy) 
            LogitudeSettings.ABMProductId = setting.ABMProductId;
            LogitudeSettings.AzureFolderName = setting.AzureFolderName;
            if (LogitudeSettings.IsCostomsDeploy)
            {
                LogitudeSettings.GetUnfDBConnectionInfoFromTenantInject = CustomsSettingQueryService.GetUnfDBConnectionInfo;// this project no need but in FilingManager is must 
                LogitudeSettings.GetLogitudeCustomsSettingsMInject = CustomsSettingQueryService.GetLogitudeCustomsSettingsM;

            }

            Logitude.Server.Tools.ContainerAccessor.InitContainer();
            InjectionUtil.Init(null, null, null, () => (new ByteCompressorUtil()) as IByteCompressorUtil, null, null, null, null, () => (new TreeFilterQueryService()) as ITreeFilterQueryService);
            InfraRegistrationHelper.Register();
           var globalTenants = new GlobalDomainService().GetGlobalDBs().Where(x=>x.IsActive && !string.IsNullOrEmpty(x.DBConnection));
            foreach (var item in globalTenants)
            {
                _globalDBs.Add(Convert.ToInt32(item.Id), item.DBConnection);
            }

            CacheManager.CacheWrapper = new CacheWrapper(WorkerEntryPoint.Cache, _globalDBs);
        }

        private string GetConnectionString()
        {
            string dbConnectionInfo = "";
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                dbConnectionInfo = ConfigurationManager.ConnectionStrings["Oracle_Globalstr"].ConnectionString;
            }
            else
            {
                dbConnectionInfo = ConfigurationManager.ConnectionStrings["Globalstr"].ConnectionString;
            }
            return dbConnectionInfo;
        }

        private IncludedModulesClass GetIncludeModules(string connectionString)
        {
            string queryString = "SELECT * FROM [dbo].[DBMigrationSettings]";
            SqlDataReader reader = null;
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(queryString, connection);

            IncludedModulesClass includedModules = null;

            try
            {
                connection.Open();
                reader = command.ExecuteReader();

                reader.Read();

                if (reader.HasRows)
                {
                    includedModules = new IncludedModulesClass
                    {
                        Include = reader["Mode"].ToString().ToLower() == "include",
                        Modules = reader["ModulesList"].ToString().ToLower().Split(',').ToList()
                    };
                }

                reader.Close();
                connection.Close();
            }
            catch (Exception ex)
            {
                if (reader != null)
                {
                    reader.Close();
                }
                connection.Close();
            }

            return includedModules;
        }

        public string GetConnection(GlobalDB currentDb)
        {
            string dbConnectionInfo = currentDb.DBConnection;
            string dbSeconderyConnectionInfo = currentDb.SecondaryAzureDBConnection;

            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo, dbSeconderyConnectionInfo);
            return connection.ConnectionString;
        }
        public class IncludedModulesClass
        {
            public bool Include { get; set; }

            public List<string> Modules { get; set; }
        }
    }
}


