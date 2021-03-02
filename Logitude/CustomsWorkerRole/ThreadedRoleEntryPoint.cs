using Logitude.Customs.BL.EntityQueryServiceExt;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityQueryServicesExt;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Utils;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebFreight.Web;

namespace CustomsWorkerRole
{
    public class ThreadedRoleEntryPoint : RoleEntryPoint
    {
        private List<Thread> threads = new List<Thread>();
        List<WorkerEntryPoint> workers;
        protected EventWaitHandle EventWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
        //public static string DeploymentStage = "Dev";//Dev//Test1//Simplog

        public override void Run()
        {
            foreach (WorkerEntryPoint worker in workers)
                threads.Add(new Thread(worker.ProtectedRun) { Name = worker.ThreadName });

            foreach (Thread thread in threads)
                thread.Start();

            while (!EventWaitHandle.WaitOne(0))
            {
                // WWB: Restart Dead Threads
                for (Int32 i = 0; i < threads.Count; i++)
                {
                    if (!threads[i].IsAlive)
                    {
                        threads[i] = new Thread(workers[i].Run);
                        threads[i].Start();
                    }
                }

                EventWaitHandle.WaitOne(1000);
            }

        }

        //public bool OnStart(WorkerEntryPoint[] workers)
        //{
        //    this.Workers = workers;

        //    foreach (WorkerEntryPoint worker in workers)
        //        worker.OnStart();

        //    return base.OnStart(); 
        //}

        public override bool OnStart()
        {
            StartStatic();
            workers = //new List<WorkerEntryPoint>();
             GetAllWorkerEntryPointType();

#if false
            {
            workers.Add(new SendDataToExternalServicesWR());
            workers.Add(new UpdateClosedTablesWR());
                workers.Add(new DownloadDcaMessageSheetWR());
                workers.Add(new CustomsMessagingSheetWR());    
            }
#endif




            foreach (WorkerEntryPoint worker in workers)
                worker.OnStart();

            return base.OnStart();

        }

        public static List<Logitude.Server.Tools.WorkerEntryPoint> GetAllWorkerEntryPointType()
        //where TWorker :WorkerEntryPoint,new() 
        {
#if false



            
       

                        
            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONS" (CODE, CLASSNAME) VALUES ('SendDataToExternalServicesWR', 'SendDataToExternalServicesWR');
            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONMODS" VALUES ('SendDataToExternalServicesWR', '0', '1');


            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONS" (CODE, CLASSNAME) VALUES ('CommunicationLogWorkerRoleWinService', 'CommunicationLogWorkerRoleWinService');
            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONMODS" VALUES ('CommunicationLogWorkerRoleWinService', '0', '1');


            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONS" (CODE, CLASSNAME) VALUES ('DownloadDcaMessageSheetWR', 'DownloadDcaMessageSheetWR');
            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONMODS" VALUES ('DownloadDcaMessageSheetWR', '0', '1');

            
            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONS" (CODE, CLASSNAME) VALUES ('CustomsMessagingSheetWR', 'CustomsMessagingSheetWR');
            /
            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONMODS" VALUES ('CustomsMessagingSheetWR', '0', '1');
    
            /

            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONS" (CODE, CLASSNAME) VALUES ('CustomsCommandGetCustomRequestWR', 'CustomsCommandGetCustomRequestWR');
            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONMODS" VALUES ('CustomsCommandGetCustomRequestWR', '0', '7');

           

            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONS" (CODE, CLASSNAME) VALUES ('CustomsCommandSendDCAWR', 'CustomsCommandSendDCAWR');
            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONMODS" VALUES ('CustomsCommandSendDCAWR', '0', '7');

            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONS" (CODE, CLASSNAME) VALUES ('CustomsCommandSendDCAUploadStatusWR', 'CustomsCommandSendDCAUploadStatusWR');
            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONMODS" VALUES ('CustomsCommandSendDCAUploadStatusWR', '0', '7');
            

            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONS" (CODE, CLASSNAME) VALUES ('CustomsCommandSendWSReceiveCorrelationWR', 'CustomsCommandSendWSReceiveCorrelationWR');
            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONMODS" VALUES ('CustomsCommandSendWSReceiveCorrelationWR', '0', '7');

            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONS" (CODE, CLASSNAME) VALUES ('CustomsCommandDownloadDcaReceiveCorrelationWR', 'CustomsCommandDownloadDcaReceiveCorrelationWR');
            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONMODS" VALUES ('CustomsCommandDownloadDcaReceiveCorrelationWR', '0', '7');
            
           
            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONS" (CODE, CLASSNAME) VALUES ('CustomsCommandAnalyzeResponseWR', 'CustomsCommandAnalyzeResponseWR');
            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONMODS" VALUES ('CustomsCommandAnalyzeResponseWR', '0', '7');



#endif


            return new WorkerEntryPoint[] {
                new   SendDataToExternalServicesWR() ,
                //new   UpdateClosedTablesWR() ,
                new   CustomsMessagingSheetWR() ,
                new   DownloadDcaMessageSheetWR() ,
                new   CustomsCommandGetCustomRequestWR() ,
                ///new   CustomsCommandSignRequestWR() ,
                new   CustomsCommandSendDCAWR() ,
                new   CustomsCommandSendDCAUploadStatusWR() ,
                new   CustomsCommandSendWSReceiveCorrelationWR() ,
                new   CustomsCommandDownloadDcaReceiveCorrelationWR() ,
                new   CustomsCommandAnalyzeResponseWR()
            }.ToList();
        }



        private void AddWorker<T1>()
        {
            throw new NotImplementedException();
        }

        public static void StartStatic(Action<bool, bool> BuildObjectTablesZipFilesDataAction=null,string ProductInfo=null)
        {
            if (string.IsNullOrEmpty(LogitudeSettings.DeploymentStage))
            {
                string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
                LogitudeSettings.DatabaseManagementSystem = dbms;
                LogitudeSettings.DebugKey = System.Configuration.ConfigurationManager.AppSettings.Get("DebugKey");
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
                LogitudeSettings.ForceHttps = setting.ForceHttps;
                LogitudeSettings.CheckConnectionURL = setting.CheckConnectionURL;
                LogitudeSettings.IOSSharedAppMinimumVersion = setting.IOSSharedAppMinimumVersion;
                LogitudeSettings.WorkEnvironment = setting.WorkEnvironment;
                LogitudeSettings.LogoCode = setting.LogoCode;
                LogitudeSettings.EnableHybridQueue = setting.EnableHybridQueue;
                
                //LogitudeSettings.IsCostomsDeploy = Logitude.Customs.BL.Utils.CustomsSettingUtil.ForceDownloadXapFromIIS();
                //LogitudeSettings.GetUnfDBConnectionInfoFromTenantInject = CustomsSettingQueryService.GetUnfDBConnectionInfo;
                LogitudeSettings.GetLogitudeCustomsSettingsMInject = CustomsSettingQueryService.GetLogitudeCustomsSettingsM;

                LogitudeSettings.HandleDbExceptionInject = ExceptionHandler.HandleDbException;
                LogitudeSettings.HandleBuildObjectTablesZipFilesData_Inject = BuildObjectTablesZipFilesDataAction;
                LogitudeSettings.GetUserNameInject = AuthenticationUtil.ResolveUserIdentityName;

                LogitudeSettings.StorageServiceMode = setting.StorageServiceMode;
                LogitudeSettings.QueueServiceMode = setting.QueueServiceMode;
                LogitudeSettings.ABMProductId = setting.ABMProductId;
                LogitudeSettings.AzureFolderName = setting.AzureFolderName;
                LogitudeSettings.CPUIntensiveWebServicesURL = setting.CPUIntensiveWebServicesURL;

                
            }
             //CommunicationWorkerRole.ThreadedRoleEntryPoint.SetWorkerRoleName();
            if (LogitudeSettings.IsCostomsDeploy)
            {
                LogitudeSettings.ProductInfo = ProductInfo;

                var he = new CultureInfo("he-IL");// '("en-US") '    "he-IL")
                he.DateTimeFormat.DateSeparator = ".";
                he.DateTimeFormat.ShortDatePattern = "dd-MM-yy";// ' "yyyy/MM/dd" '  ' "DD/MM/YYYY"
                System.Threading.Thread.CurrentThread.CurrentCulture = he;
                
                LogitudeSettings.HandleLogMe = new Action<string, bool, string, DateTime>((mess, err, suffix, stopLogAt) =>
                {
                    if (DateTime.Now > stopLogAt) return;
                    Logger.LogMe(mess, err, suffix);
                });

                LogitudeSettings.RunWorkerRoleAutomaticBreakPoint = false;

                LogitudeSettings.WorkerRoleName = LogitudeSettings.WorkerRoleName ?? "production";
            }
           // string storageServiceMode = System.Configuration.ConfigurationManager.AppSettings.Get("StorageServiceMode");
            //string queueServiceMode = System.Configuration.ConfigurationManager.AppSettings.Get("QueueServiceMode");
            ContainerAccessor.InitContainer();
           


        }

        public override void OnStop()
        {
            EventWaitHandle.Set();

            foreach (Thread thread in threads)
                while (thread.IsAlive)
                    thread.Abort();

            // WWB: Check To Make Sure The Threads Are
            // Not Running Before Continuing
            foreach (Thread thread in threads)
                while (thread.IsAlive)
                    Thread.Sleep(10);

            // WWB: Tell The Workers To Stop Looping
            foreach (WorkerEntryPoint worker in workers)
                worker.OnStop();

            base.OnStop();
        }

        /// <summary>
        /// Return the name of queue depend on enviroment(Dev,Prodcution or Test)
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public static string GetQueueByEnviroment(string queueName)
        {

            return Simplog.Server.Infrastructure.WebFreightEntryPoint.GetQueueByEnviroment(queueName);
#if false
               var save_queueName = queueName;
            if (LogitudeSettings.DeploymentStage == "Dev")
            {
                queueName = Environment.MachineName + "_" + queueName;
            }
            else if (LogitudeSettings.DeploymentStage == "customs")
            {
                queueName = "customs" + "_" + queueName;
            }
            else if (LogitudeSettings.DeploymentStage == "Simplog")
            {
                queueName = "Production" + "_" + queueName;
            }
            else
            {
                queueName = "Test" + "_" + queueName;
            }
            var customsDeploymentStage = SettingUtil.GetCustomsDeploymentStage();
            switch (customsDeploymentStage)
            {
                case SettingUtil.CustomsDeploymentStage.Test:
                case SettingUtil.CustomsDeploymentStage.Pilot:

                    var uri = new Uri(LogitudeSettings.LogitudeURL);
                    var branch = uri.LocalPath.Trim(@"\"[0]).Trim(@"/"[0]);
                    queueName = LogitudeSettings.StorageAccountName + "_Customs" + customsDeploymentStage.ToString() + branch + "_" + save_queueName;
                    break;
                case SettingUtil.CustomsDeploymentStage.Production:
                    //queueName = "Customs" + customsDeploymentStage.ToString() + "_" + save_queueName;
                    queueName = LogitudeSettings.StorageAccountName + "_Customs" + customsDeploymentStage.ToString() + "_" + save_queueName;
                    break;
             
                default:
                    break;
            }

 
            return queueName;
#endif

        }
    }


}