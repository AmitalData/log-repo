using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFreight.Web.MetaDataUpdate;

namespace Logitude.MetadataUpdate
{
    public class MetadataUpdateService
    {
        public void RunModulesUpdate(string moduleName)
        {
            try
            {
                Console.WriteLine("Initializing Settings ...");
                InitializeSettings();
                TenantsUpdateClass.UpdateDataForTenant(0, moduleName);
                Console.WriteLine("Updating all modules finished successfully");
                Console.WriteLine("Building Object table zip files ...");
                bool buildCustomsZipFiles = false;
                TenantsUpdateClass.BuildObjectTablesZipFilesData(false, buildCustomsZipFiles);
                Console.WriteLine("Building zip files finished successfully ...");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                Console.WriteLine(e.InnerException?.Message);
                Environment.Exit(1);
            }
        }

        private void InitializeSettings()
        {
            string dbms = ConfigurationManager.AppSettings.Get("DBMS");
            LogitudeSettings.DatabaseManagementSystem = dbms;
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
            InjectionUtil.Init(null, null, null, () => (new ByteCompressorUtil()) as IByteCompressorUtil, null, null,null);

            CacheManager.CacheWrapper = new CacheWrapper(WorkerEntryPoint.Cache);
        }
    }
}
