using CustomsWorkerRole;
using Logitude.BL.Helpers;
using Logitude.BL.Resolvers;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Server.Tools;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unifreight.BL.EntityQueryServices;
using Unifreight.BL.Models;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.EntityPOCOs;
using Unifreight.Data.AmitalModel.Repsitories;
using WebFreight.Web.WcfApi;

namespace AmitalTestConsoleApp
{
    internal class TestSyncRecord
    {
        public static void Run()
        {
            Console.WriteLine("************ start TestSyncRecord ************");

            LoadLogitudeSettings();

            int tenant = 102;
            string fileNo = "0";

            string tableName = "Customs.CustomsCountries";
            string itemUpdate =  tableName;
            bool isCloseTable = true;

            //string tableName = "GGGQ";
            //string itemUpdate =  fileNo;
            //bool isCloseTable = false;

            //CreateNewRecord(tenant, fileNo, tableName, isCloseTable);
            WRSendTaskToQueueMessage();
            //Envelope task = GetTaskFromQueue(tenant);
            //List<EntityRecord> dataSync = APIGetSyncData(tenant, itemUpdate);
            //APIMarkSyncFinished(tenant, itemUpdate, dataSync);
            //EnqueueTask(task.CommunicationLogId, tenant);

            Console.WriteLine("************ finish TestSyncRecord ************");
        }

        private static void CreateNewRecord(int tenant, string fileNo, string tableName, bool isCloseTable)
        {
            SyncRecordQuery syncRecordQuery = new SyncRecordQuery(tenant);

            syncRecordQuery.Add(new List<SyncRecord>() { new SyncRecord()
            {
                Id = Guid.NewGuid().ToString(),
                Tenant = 0,
                CreateDate = DateTime.Now,
                IsSync = 0,
                Entname = tableName,
                FileNo = fileNo,
                KeyVal = isCloseTable ? "ALL" : "QUE_ID='202407221103296136791706705218'",
                TrigAction = isCloseTable ? "U" : "I",
            } });
        }

        private static void WRSendTaskToQueueMessage()
        {
            Thread.Sleep(500);
            Console.WriteLine("Send Task To QueueMessage");
            new SyncRecordsCCUTableWR().SendSyncRecoredToUnifreightQueue();
        }

        private static Envelope GetTaskFromQueue(int tenant)
        {
            Thread.Sleep(500);
            string taskXml = new ExternalTasksQueueWcfService().GetTaskFromQueue(tenant, 4);
            if(string.IsNullOrEmpty(taskXml))
                throw new Exception("No task in queue");

            Envelope task = LogitudeXmlSerializer.DeserializeObject<Envelope>(taskXml);
            QueueTask queueTask = task.Tasks.FirstOrDefault();
            Console.WriteLine($"Get Task From Queue - action: {queueTask?.Action}, parameter: {queueTask?.Parameters?.FirstOrDefault()?.Value}, CommunicationLogId: {task.CommunicationLogId}" );
            return task;
        }

        private static List<EntityRecord> APIGetSyncData(int tenant, string itemUpdate)
        {
            Thread.Sleep(500);
            List<EntityRecord> res = new SyncRecordQuery(tenant).GetUnsyncRecordsAndMarkAsInProcess(tenant, itemUpdate);
            Console.WriteLine("Get sync Data");
            res.ForEach(x => Console.WriteLine($"record: table: {x.Entname}, data: {x.RecordAsJson}"));
            return res;
        }

        private static void APIMarkSyncFinished(int tenant, string itemUpdate, List<EntityRecord> dataSync = null , DateTime? syncDT = null)
        {
            Thread.Sleep(500);

            if ((dataSync == null || dataSync.Count == 0) && syncDT == null)
            {
                Console.WriteLine("No dataSync or syncDT");
                return;
            }

            if(syncDT == null)
                syncDT = dataSync.First()?.UpdateDate.Value;

            new SyncRecordQuery(tenant).UpdateSyncData(tenant, itemUpdate, syncDT.Value);

            Thread.Sleep(500);

            DateTime yesterday = DateTime.Now.AddDays(-1);            
            DateTime maxSyncDT = syncDT.Value.AddSeconds(1);
            DateTime minSyncDT = syncDT.Value.AddSeconds(-1);
            bool isUpdate = AmitalContext.GetContext(tenant).SyncRecord.Where(syncRecord =>
                syncRecord.Tenant == tenant &&
                (syncRecord.FileNo == itemUpdate || syncRecord.Entname == itemUpdate) &&
                syncRecord.IsSync == SyncRecordStatus.SyncedAndUpdated &&
                syncRecord.SyncDT <= maxSyncDT &&
                syncRecord.SyncDT >= minSyncDT &&
                syncRecord.CreateDate > yesterday
            ).ToList().Count() > 0;

            Console.WriteLine($"update is {(isUpdate ? "" : "not")} success");
        }

        private static void EnqueueTask(string communicationLogId, int tenant)
        {
            Thread.Sleep(500);

            Console.WriteLine("Enqueue Task, communicationLogId: " + communicationLogId);
            new ExternalTasksQueueWcfService().MarkTaskAsDone(communicationLogId, tenant, 4);
        }

        public static void LoadLogitudeSettings()
        {
            LogitudeSettings.GetLogitudeCustomsSettingsMInject = CustomsSettingQueryService.GetLogitudeCustomsSettingsM;
            LogitudeSettings.WorkerRoleName = "production";
            LoggedContactResolver.RegisterLoggedContactUtil();
            ContainerAccessor.InitContainer();
            
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
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
            //if (LogitudeSettings.IsCostomsDeploy)
            {
                //LogitudeSettings.GetUnfDBConnectionInfoFromTenantInject = CustomsSettingQueryService.GetUnfDBConnectionInfo;
                LogitudeSettings.GetLogitudeCustomsSettingsMInject = CustomsSettingQueryService.GetLogitudeCustomsSettingsM;
            }
            string storageServiceMode = "fs";
            string queueServiceMode = "azure";
            Logitude.Server.Tools.ContainerAccessor.InitContainer();
            //InjectionUtil.Init(null, null, null, () => (new ByteCompressorUtil()) as IByteCompressorUtil, null, null, null, null, () => (new TreeFilterQueryService()) as ITreeFilterQueryService);
            InfraRegistrationHelper.Register();
            CacheManager.CacheWrapper = new MockCacheWrapper();
        }
    }
}