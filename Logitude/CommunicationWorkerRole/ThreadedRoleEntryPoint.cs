using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Threading;

using Simplog.Server.Infrastructure;


using Microsoft.WindowsAzure.Storage;
using System.IO;
using System.Diagnostics;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Net;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Customs.BL.EntityQueryServices;
using System.Globalization;
using Logitude.SystemLogs;
using System.Linq;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.GlobalModel.EntityPMs;
using System.Web.Caching;
using System.Web;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools.Utils;
using Logitude.BL.Security;
using System.Timers;
using WebFreight.Web.AccountingModel;
using Logitude.BL.Interfaces;
using WebFreight.Web.Validators;
using Logitude.BL.Helpers;
using Microsoft.Practices.Unity;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.BL.EntityQueryServices;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools.Resolvers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Stimulsoft.Base;
using System.Xml;
using System.Xml.Linq;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.Customs.BL.Messaging.Amital;
using Simplog.Server.Infrastructure.Interfaces;

namespace CommunicationWorkerRole
{
    public class ThreadedRoleEntryPoint : RoleEntryPoint
    {
        private List<Thread> threads = new List<Thread>();
        System.Timers.Timer batchServiceLogTimer = new System.Timers.Timer();

        //private WorkerEntryPoint[] Workers;
        List<WorkerEntryPoint> workers;
        protected EventWaitHandle EventWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
        string BatchServicesParam = "";
        string IgnoredBatchServicesParam = "";
        bool IgnoreServices = false;
        bool IsManagedProcess = false;

        //public static string DeploymentStage = "Dev";//Dev//Test1//Simplog//logitudetest3//amital//logitudetest2
        //public static string ChampEnv = "TEST";//PROD//TEST
        //

        public ThreadedRoleEntryPoint()
        {

        }

        public ThreadedRoleEntryPoint(string BatchServices, string IgnoredBatchServices)
        {
            IsManagedProcess = true;
            BatchServicesParam = BatchServices;
            IgnoredBatchServicesParam = IgnoredBatchServices;
            if (!string.IsNullOrEmpty(IgnoredBatchServices))
            {
                IgnoreServices = true;
            }


        }

        public override void Run()
        {
            try
            {
                foreach (WorkerEntryPoint worker in workers)
                {
                    Thread myThread = new Thread(worker.ProtectedRun) { Name = worker.ThreadName };
                    worker.CurrentThread = myThread;
                    threads.Add(myThread);
                }


                foreach (Thread thread in threads)
                    thread.Start();

                batchServiceLogTimer.Elapsed += batchServiceLogTimer_Elapsed;
                batchServiceLogTimer.Interval = 30000;
                batchServiceLogTimer.Start();
                while (!EventWaitHandle.WaitOne(0))
                {
                    // WWB: Restart Dead Threads
                    for (Int32 i = 0; i < threads.Count; i++)
                    {
                        if (!threads[i].IsAlive)
                        {
                            threads[i] = new Thread(workers[i].Run) { Name = threads[i].Name };
                            threads[i].Start();
                        }
                    }

                    EventWaitHandle.WaitOne(1000);
                }
            }
            catch (SystemException e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "ThreadedRoleEntryPoint :  Run() Method", null);
                Thread.Sleep(10000);
            }
        }

        void batchServiceLogTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            StartLogging();
        }
        System.Timers.Timer aTimer = new System.Timers.Timer();
        public override bool OnStart()
        {


            if (string.IsNullOrEmpty(LogitudeSettings.DeploymentStage))
            {

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
                LogitudeSettings.ForceHttps = setting.ForceHttps;
                LogitudeSettings.WorkEnvironment = setting.WorkEnvironment;
                LogitudeSettings.LogoCode = setting.LogoCode;
                LogitudeSettings.EnableHybridQueue = setting.EnableHybridQueue;
                LogitudeSettings.GLSHKEnv = setting.GLSHKEnv;
                LogitudeSettings.GLSHKURL = setting.GLSHKURL;
                //LogitudeSettings.IsCostomsDeploy = Logitude.Customs.BL.Utils.CustomsSettingUtil.ForceDownloadXapFromIIS();
                if (LogitudeSettings.IsCostomsDeploy)
                {
                    //LogitudeSettings.GetUnfDBConnectionInfoFromTenantInject = CustomsSettingQueryService.GetUnfDBConnectionInfo;
                    LogitudeSettings.GetLogitudeCustomsSettingsMInject = CustomsSettingQueryService.GetLogitudeCustomsSettingsM;


                }
                LogitudeSettings.HandleLogMe = new Action<string, bool, string, DateTime>((mess, err, suffix, stopLogAt) =>
                {
                    if (DateTime.Now > stopLogAt) return;
                    Logger.LogMe(mess, err, suffix);
                });
                LogitudeSettings.HandleDbExceptionInject = ExceptionHandler.HandleDbException;
                LogitudeSettings.HandleBuildObjectTablesZipFilesData_Inject = WebFreight.Web.MetaDataUpdate.TenantsUpdateClass.BuildObjectTablesZipFilesData;
                LogitudeSettings.GetUserNameInject = AuthenticationUtil.ResolveUserIdentityName;
                LogitudeSettings.DomainName = setting.DomainName;
                LogitudeSettings.ProductName = setting.ProductName;
                LogitudeSettings.EmailAlertSignature = setting.EmailAlertSignature;
                LogitudeSettings.QueueServiceMode = setting.QueueServiceMode;
                LogitudeSettings.StorageServiceMode = setting.StorageServiceMode;
                LogitudeSettings.NotificationHubName = setting.NotificationHubName;
                LogitudeSettings.NotificationHubConnectionString = setting.NotificationHubConnectionString;
                LogitudeSettings.AndroidAppLink = setting.AndroidAppLink;
                LogitudeSettings.IOSAppLink = setting.IOSAppLink;
                LogitudeSettings.AzureFolderName = setting.AzureFolderName;
                LogitudeSettings.SMSServiceUserId = setting.SMSServiceUserId;
                LogitudeSettings.SMSServiceAuthToken = setting.SMSServiceAuthToken;
                LogitudeSettings.SMSServicePhoneNumber = setting.SMSServicePhoneNumber;
                LogitudeSettings.EmailSendingQuota = setting.EmailSendingQuota;
                LogitudeSettings.CPUIntensiveWebServicesURL = setting.CPUIntensiveWebServicesURL;

                //LogitudeSettings.ABMProductId = setting.ABMProductId;

            }

            StiLicense.Key = "6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHk5LQfMb0Dr1Ze4z6YRXSb7imTiay6/HzKYGUzkd/h3FMt5R7" +
"uunoM5lX8Vs2voVkSeT6Wv6WI6Jcy4xOeAjjPkTBhC+ivrrxidMQjLaebItqFcnJWqKXBUgoJa0WfmH3soi0IbfEmI" +
"fQ3ZmMq5BHsjsKoHSdnbzDUPWMXieYRTJZL6tsBC6QRy2ALPnYwg88ZJDGAWgAqMhZ+M0BVM17B3YJN9mu1MfAblN7" +
"rG1eWrSrR5B53af4aeWs0RmqVNatfenGL8sufvTgOiyEuQmC9J7sHOT6VoQpWOlZthrc7JOl4zbw+qduZHZrpLuK+1" +
"O3AB8EeDCQ6EgM8TcUesQBZZrUA4ZUFpxsCdvL0n4DQiB1tIof1TGHXCtZ62S1kAfU4XJzEGM/g3MYbKridAK5ckyc" +
"0xwsK2y46rm9W3EV0m49Na0pcJe+2ZScc6BP1o3tDS9ddHbfkt7hFZpUNTqOxn9BOP0YVoQul+dPckYle4PS4mzXVp" +
"tMrKV4En69rnW/z658axW0kQ2GxorKwW0IAR";
            if (BatchServicesParam.Contains("SATInterface"))
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11;

            }
            else
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            }

            StartStatic();
            ContainerAccessor.InitContainer();
            ContainerAccessor.RegisterTypeFactory<IRulesValidator, RulesValidator>("RulesValidator", new RulesValidator());
            ContainerAccessor.RegisterTypeFactory<IQuoteTemplateReportHelper, QuoteTemplateReportHelper>("QuoteTemplateReportHelper", new QuoteTemplateReportHelper());

            LoggedContactResolver.RegisterLoggedContactUtil();
            DateTimeUtilResolver.RegisterDateTimeUtil();
            TranslateTextsClassUtilResolver.RegisterTranslateTextsClassUtil();
            IdCounterUtilResolver.RegisterIdCounterUtil();


            AccountingRegistrations.Register();
            InfraRegistrationHelper.Register();

            Func<IAmitalRestrictOwnerService> createAmitalRestrictOwnerModelService = null;

            if (LogitudeSettings.IsCostomsDeploy)
            {
                createAmitalRestrictOwnerModelService = () =>
                {
                    var amitalRestrictOwnerService = new AmitalRestrictOwnerService();
                    return amitalRestrictOwnerService;
                };
            }
            Func<int> getTenantFromToken = () =>
            {

                return 0;

            };


            InjectionUtil.Init(createAmitalRestrictOwnerModelService, getTenantFromToken, SecurityUtility.CheckContactFeature,
               () => (new ByteCompressorUtil()) as IByteCompressorUtil,
               new IISManager(),
               () => (new HtmlEditorHelper()) as IHtmlEditorHelper,
               () => (new EntityUpdateReflectorService()) as IEntityUpdateReflectorService
               );

           



            if (LogitudeSettings.IsCostomsDeploy)
            {
                var he = new CultureInfo("he-IL");// '("en-US") '    "he-IL")
                he.DateTimeFormat.DateSeparator = ".";
                he.DateTimeFormat.ShortDatePattern = "dd-MM-yy";// ' "yyyy/MM/dd" '  ' "DD/MM/YYYY"
                System.Threading.Thread.CurrentThread.CurrentCulture = he;
            }
            bool toTest = false;
            if (toTest)
            {
                TestBatch();
                workers = new List<WorkerEntryPoint>();
                return base.OnStart();
            }

            UpdateRunningWR();

            aTimer.Elapsed += new ElapsedEventHandler(OnSettingsCheckTimedEvent);
            aTimer.Interval = 30000;
            aTimer.Enabled = true;

            //try
            //{

            SetWorkerRoleName();


            //HttpContext.Current.Items.Add("workerrolename", xmlnode.v);

            //} 
            //catch(Exception ex)
            //{
            //    throw new ex
            //}


            return base.OnStart();

            //throw (new InvalidOperationException());
        }
        private static string[] ActiveWorkers = { };
        public static void SetWorkerRoleName()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            string filePath = Path.Combine(directoryInfo.FullName, "WorkerRoleName.xml");
            XElement workerRoleNameElement = XElement.Load(filePath);
            var nameElement = workerRoleNameElement.Element("WorkerName");
            LogitudeSettings.WorkerRoleName = nameElement.Value;

            var automaticBreakPointElement = workerRoleNameElement.Element("AutomaticBreakPoint");
            bool automaticBreakPoint;
            if (automaticBreakPointElement != null)
                bool.TryParse(automaticBreakPointElement.Value, out automaticBreakPoint);
            else
            {
                string automaticBreakPointStr = System.Configuration.ConfigurationManager.AppSettings.Get("AutomaticBreakPoint");
                bool.TryParse(automaticBreakPointStr, out automaticBreakPoint);
            }

            var activeWorkersElement = workerRoleNameElement.Element("ActiveWorkers");
            if(activeWorkersElement != null)
            {
                string activeWorkersStr = activeWorkersElement.Value;
                if (!string.IsNullOrEmpty(activeWorkersStr))
                {
                    ActiveWorkers = activeWorkersStr.Split(',');
                }
            }

            LogitudeSettings.RunWorkerRoleAutomaticBreakPoint = automaticBreakPoint;

            if (Debugger.IsAttached && LogitudeSettings.WorkerRoleName == "production")
            {
                throw new Exception("Production worker role should not be run in Debug mode! To debug the worker role in production please use a custom worker name");// 
            }
        }

        private void TestBatch()
        {

            try
            {
                List<string> Last_journalBufferKeys = null;
                //var myWorker = new Logitude.Accounting.BL.CoreBL.JournalApproveService.JournalApproveWorker();
                //myWorker.WorkUntilQEmptyQueueDB();

                //Logitude.Accounting.BL.CoreBL.JournalApproveService.WorkWithoutQueue(1051, null, ref Last_journalBufferKeys);

                var batchTaskExecutionWR = new BatchTaskExecutionWR();
                var dic = new Dictionary<string, string>();
                //{"BatchTaskExecutionId":"1-7167","Tenant":"1071"}-QueueDefinitionCode ='batchtaskexecutionqueue'
                dic.Add("BatchTaskExecutionId", "1-3620");
                dic.Add("Tenant", "18");
                batchTaskExecutionWR.SupressStartThread = true;
                batchTaskExecutionWR.ExecuteQueue(new Logitude.Server.Tools.QueueService.QueueResponse() { MessageValues = dic });
                //var myEmailsWorkerRole = new EmailsWorkerRole("EmailQueue","itzik");
                //var context = CommonDataContext.GetContext(989);
                //var communicationLogRep = new CommunicationLogRepository(context);
                //var cl = communicationLogRep.GetSingleCommunicationLog(id: "1-1075543", tenant: 989);

                //myEmailsWorkerRole.SendWaitingCommunicationLog(cl);
                /////BatchAccountingLoadTestTask();
                ///            }
            }
            catch (Exception)
            {


            }

        }

        private static void BatchAccountingLoadTestTask()
        {
            string s =
                            @"<?xml version=""1.0"" encoding=""utf-16""?><BatchAccountingLoadArg xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""><Tenant>1051</Tenant>";
            //s+="<ActionType>CreateCustomers</ActionType>";
            s += "<ActionType>CreateJournalEvery</ActionType>";
            s += @"<Amount>10</Amount>
  <SleepEveryMinute>1</SleepEveryMinute>
  <JournalYYYY>2018</JournalYYYY>
</BatchAccountingLoadArg>";

            int tenant = 1051;
            var MyContext = InfrastructureContext.GetContext(tenant);
            var qsUpdateService = new BatchTaskExecutionQueryService(tenant);
            var pm = qsUpdateService.GetSingle("1-2838", true, false);


            var myBatchAccountingLoadTestTask = new Logitude.Accounting.BL.CoreBL.Batch.BatchAccountingLoadTestTask(pm);
            myBatchAccountingLoadTestTask.Execute();
            //myBatchAccountingLoadTestTask.RunCode();
        }

        private void OnSettingsCheckTimedEvent(object source, ElapsedEventArgs e)
        {
            UpdateRunningWR();
        }
        List<BatchServicesDefinitionPM> BatchServicesDefinitions;
        private void UpdateRunningWR()
        {


            //BatchServicesDefinitionRepository BatchServicesRepository = new BatchServicesDefinitionRepository();
            //BatchServicesDefinitionQuery BatchServicesQuery = new BatchServicesDefinitionQuery(BatchServicesRepository);
            List<BatchServicesDefinitionPM> BatchServicesDefinitionsTemp; //BatchServicesQuery.GetAllActiveBatchServicesDefinitions().ToList();//.Where(b => b.Code == "EmailOut-EmailQueue")
            if (IsManagedProcess)
            {
                BatchServicesDefinitionsTemp = GetManagedProcessActiveBatchServiceDef();
            }
            else
            {
                BatchServicesDefinitionsTemp = GetActiveBatchServiceDef();
            }
            //if (Environment.MachineName == "LogitudeWR2")
            //{
            //    BatchServicesDefinitions = BatchServicesDefinitionsTemp;
            //    //var temp = SpecialBatchCode.Split(',');
            //    //if (temp.Length > 0)
            //    //{
            //    //    var BatchCode = temp[0].ToLower();
            //    //    var IsActivate = temp[1].ToLower();
            //    //    if (IsActivate == "true")
            //    //    {
            //    //        BatchServicesDefinitionsTemp = BatchServicesDefinitionsTemp.Where(a => a.Code.ToLower() == BatchCode).ToList();
            //    //    }
            //    //    else
            //    //    {
            //    //        BatchServicesDefinitionsTemp = BatchServicesDefinitionsTemp.Where(a => a.Code.ToLower() != BatchCode).ToList();
            //    //    }
            //    //}
            //}
            RemoveSchedular(BatchServicesDefinitionsTemp);
            

            if (BatchServicesDefinitions == null)
            {
                BatchServicesDefinitions = BatchServicesDefinitionsTemp;
                StartWorkerRoles(BatchServicesDefinitions);
            }
            else
            {
                if (!ISSameList(BatchServicesDefinitions, BatchServicesDefinitionsTemp))
                {
                    foreach (Thread thread in threads)
                    {
                        thread.Abort();
                    }
                    //while (thread.IsAlive)


                    // WWB: Check To Make Sure The Threads Are
                    // Not Running Before Continuing
                    //foreach (Thread thread in threads)
                    //{
                    //    while (thread.IsAlive)
                    //        Thread.Sleep(10);
                    //}


                    // WWB: Tell The Workers To Stop Looping
                    foreach (WorkerEntryPoint worker in workers)
                    {
                        //worker.MaxWorkingTimeInMinutes = 0;
                        worker.OnStop();
                    }

                    BatchServicesDefinitions = BatchServicesDefinitionsTemp;
                    StartWorkerRoles(BatchServicesDefinitions);
                    threads = new List<Thread>();
                    foreach (WorkerEntryPoint worker in workers)
                        threads.Add(new Thread(worker.ProtectedRun) { Name = worker.ThreadName });

                    foreach (Thread thread in threads)
                        thread.Start();

                }
            }
        }

        private void StartWorkerRoles(List<BatchServicesDefinitionPM> BatchServicesDefinitions)
        {
            workers = new List<WorkerEntryPoint>();
            var tst = false;
            if (tst)
            {
                BatchServicesDefinitions = BatchServicesDefinitions.Where(r => r.ClassName == "LogMessagesWorkerRole" || r.ClassName == "CustomsSchedularWR").ToList();
            }
            var reportsTest = true;

            if (reportsTest)
            {
                
                   BatchServicesDefinitions = BatchServicesDefinitions.Where(r => r.ClassName == "BatchTaskExecutionWR").ToList();
            }
            if(ActiveWorkers != null && ActiveWorkers.Length > 0)
            {
                BatchServicesDefinitions = BatchServicesDefinitions.Where(r => ActiveWorkers.Contains(r.Code)).ToList();
            }

            foreach (var Service in BatchServicesDefinitions)
            {
                try
                {
                    for (int i = 0; i < Service.NumberOfThreads; i++)
                    {
                        List<object> args = new List<object>();
                        if (!string.IsNullOrEmpty(Service.Parameter1))
                        {
                            args.Add(Service.Parameter1);
                        }
                        if (!string.IsNullOrEmpty(Service.Parameter2))
                        {
                            args.Add(Service.Parameter2 == "null" ? null : Service.Parameter2);
                        }
                        object[] ArrArgs = args.ToArray();
                        var classname = (Service.ClassName == "CustomsSchedularWR" ? "SchedularWorkerRole" : Service.ClassName);
                        var Item = System.Activator.CreateInstance(Type.GetType("CommunicationWorkerRole." + classname), ArrArgs) as WorkerEntryPoint;
                        Item.MaxWorkingTimeInMinutes = Service.MaxWorkingTimeInMinutes;
                        workers.Add(Item);
                    }
                }
                catch(Exception ex)
                {
                    EventLog eventLog = new EventLog();
                    eventLog.WriteEntry("LogitudeBatchServices Exception : " + ex.ToString(), EventLogEntryType.Error);
                    ExceptionHandler.HandleException(ex, DateTime.UtcNow, 0, "", "WorkerRole", "ThreadedRoleEntryPoint :  Creating Instance for service: "+ Service?.ClassName, null);
                }
            }
            int into = 0;
            foreach (WorkerEntryPoint worker in workers)
            {
                into++;
                worker.OnStart();
            }
        }

        private static void RemoveSchedular(List<BatchServicesDefinitionPM> BatchServicesDefinitions)
        {
            if (Debugger.IsAttached && LogitudeSettings.WorkerRoleName.ToLower() != "development")
            {
                var schedularWorkerRole = BatchServicesDefinitions.FirstOrDefault(b => b.ClassName == "SchedularWorkerRole");
                if (schedularWorkerRole != null)
                    BatchServicesDefinitions.Remove(schedularWorkerRole);
            }
        }

        public bool ISSameList(List<BatchServicesDefinitionPM> aListA, List<BatchServicesDefinitionPM> aListB)
        {
            if (aListA.Count != aListB.Count)
            {
                return false;
            }

            foreach (var item in aListB)
            {
                if (aListA.Where(a => a.ClassName == item.ClassName && a.InActive == item.InActive && a.NumberOfThreads == item.NumberOfThreads).Count() == 0)
                {
                    return false;
                }
            }

            return true;
        }
        private List<BatchServicesDefinitionPM> GetManagedProcessActiveBatchServiceDef()
        {
            var Services = new List<ServiceDefinition>();
            if (IgnoreServices)
            {
                var TempServicesList = IgnoredBatchServicesParam.Split(',').ToList();
                foreach (var item in TempServicesList)
                {
                    ServiceDefinition ServiceDef = new ServiceDefinition();
                    ServiceDef.SarviceName = item;
                    Services.Add(ServiceDef);
                }
            }
            else
            {
                var temp = BatchServicesParam.Split(';');
                foreach (var item in temp)
                {
                    var ServiceParams = item.Split('-');
                    ServiceDefinition ServiceDef = new ServiceDefinition();
                    ServiceDef.SarviceName = ServiceParams[0];
                    if (ServiceParams.Length > 1)
                    {
                        var MaxWorkingTimeInMinutesParam = ServiceParams[1];
                        if (temp != null)
                        {
                            ServiceDef.MaxWorkingTimeInMinutes = int.Parse(MaxWorkingTimeInMinutesParam.Split('~')[1]);
                        }

                    }

                    Services.Add(ServiceDef);
                }
            }
            //string SpecialBatchCode = null;

            //var iAppSettings = System.Configuration.ConfigurationManager.AppSettings;
            //if (iAppSettings != null)
            //{
            //    if (iAppSettings["BatchCode"] != null)
            //    {
            //        SpecialBatchCode = iAppSettings["BatchCode"].ToString();
            //    }
            //}

            BatchServicesDefinitionRepository BatchServicesRepository = new BatchServicesDefinitionRepository();
            BatchServicesDefinitionQuery BatchServicesQuery = new BatchServicesDefinitionQuery(BatchServicesRepository);
            List<BatchServicesDefinitionPM> BatchServicesDefinitionsTemp = BatchServicesQuery.GetAllActiveBatchServicesDefinitions().ToList();
            //.Where(b => b.Code == "EmailOut-EmailQueue")
            //var temp = SpecialBatchCode.Split(',');
            //if (temp.Length > 0)
            //{
            //    var BatchCode = temp[0].ToLower();
            //    var IsActivate = temp[1].ToLower();
            if (IgnoreServices)
            {
                BatchServicesDefinitionsTemp = BatchServicesDefinitionsTemp.Where(a => !Services.Select(s => s.SarviceName).Contains(a.Code)).ToList();
            }
            else
            {
                BatchServicesDefinitionsTemp = BatchServicesDefinitionsTemp.Where(a => Services.Select(s => s.SarviceName).Contains(a.Code)).ToList();
            }
            RemoveSchedular(BatchServicesDefinitionsTemp);
            //}
            return BatchServicesDefinitionsTemp;
        }
        private List<BatchServicesDefinitionPM> GetActiveBatchServiceDef()
        {
            //string SpecialBatchCode = null;

            //var iAppSettings = System.Configuration.ConfigurationManager.AppSettings;
            //if (iAppSettings != null)
            //{
            //    if (iAppSettings["BatchCode"] != null)
            //    {
            //        SpecialBatchCode = iAppSettings["BatchCode"].ToString();
            //    }
            //}

            BatchServicesDefinitionRepository BatchServicesRepository = new BatchServicesDefinitionRepository();
            BatchServicesDefinitionQuery BatchServicesQuery = new BatchServicesDefinitionQuery(BatchServicesRepository);
            List<BatchServicesDefinitionPM> BatchServicesDefinitionsTemp = BatchServicesQuery.GetAllActiveBatchServicesDefinitions().ToList();//.Where(b => b.Code == "EmailOut-EmailQueue")
                                                                                                                                              //var temp = SpecialBatchCode.Split(',');
                                                                                                                                              //if (temp.Length > 1)
                                                                                                                                              //{
                                                                                                                                              //    var BatchCode = temp[0].ToLower();
                                                                                                                                              //    var IsActivate = temp[1].ToLower();
                                                                                                                                              //    if (IsActivate == "true")
                                                                                                                                              //    {
                                                                                                                                              //        BatchServicesDefinitionsTemp = BatchServicesDefinitionsTemp.Where(a => a.Code.ToLower() == BatchCode).ToList();
                                                                                                                                              //    }
                                                                                                                                              //    else
                                                                                                                                              //    {
                                                                                                                                              //        BatchServicesDefinitionsTemp = BatchServicesDefinitionsTemp.Where(a => a.Code.ToLower() != BatchCode).ToList();
                                                                                                                                              //    }
                                                                                                                                              //}


            return BatchServicesDefinitionsTemp;
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

            batchServiceLogTimer.Stop();
            batchServiceLogTimer.Elapsed -= batchServiceLogTimer_Elapsed;
            batchServiceLogTimer = null;

            base.OnStop();
        }

        /// <summary>
        /// Return the name of queue depend on enviroment(Dev,Prodcution or Test)
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public static string GetQueueByEnviroment(string queueName)
        {
            if (LogitudeSettings.DeploymentStage == "Dev")
            {
                queueName = Environment.MachineName + "_" + queueName;
            }
            else if (LogitudeSettings.DeploymentStage == "customs")
            {
                queueName = "customs" + "_" + queueName;
            }
            else if (LogitudeSettings.DeploymentStage == "Simplog" || LogitudeSettings.DeploymentStage == "amitalstorage")
            {
                queueName = "Production" + "_" + queueName;
            }
            else
            {
                queueName = "Test" + "_" + queueName;
            }

            return queueName;
        }

        public static void StartStatic()
        {
            ApplicationAppInfo.WorkerRoleCall = true;

            ContainerAccessor.InitContainer();

            if (string.IsNullOrEmpty(LogitudeSettings.DeploymentStage))
            {

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
                LogitudeSettings.WorkEnvironment = setting.WorkEnvironment;
                LogitudeSettings.LogoCode = setting.LogoCode;
                LogitudeSettings.EnableHybridQueue = setting.EnableHybridQueue;
                LogitudeSettings.GLSHKEnv = setting.GLSHKEnv;
                LogitudeSettings.GLSHKURL = setting.GLSHKURL;
                LogitudeSettings.ABMProductId = setting.ABMProductId;
                LogitudeSettings.AzureFolderName = setting.AzureFolderName;
                LogitudeSettings.CPUIntensiveWebServicesURL = setting.CPUIntensiveWebServicesURL;




                //LogitudeSettings.IsCostomsDeploy = Logitude.Customs.BL.Utils.CustomsSettingUtil.ForceDownloadXapFromIIS();
                if (LogitudeSettings.IsCostomsDeploy)
                {
                    //LogitudeSettings.GetUnfDBConnectionInfoFromTenantInject = CustomsSettingQueryService.GetUnfDBConnectionInfo;
                    LogitudeSettings.GetLogitudeCustomsSettingsMInject = CustomsSettingQueryService.GetLogitudeCustomsSettingsM;
                }
                LogitudeSettings.HandleDbExceptionInject = ExceptionHandler.HandleDbException;
                LogitudeSettings.HandleBuildObjectTablesZipFilesData_Inject = WebFreight.Web.MetaDataUpdate.TenantsUpdateClass.BuildObjectTablesZipFilesData;
                LogitudeSettings.GetUserNameInject = AuthenticationUtil.ResolveUserIdentityName;

                string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
                LogitudeSettings.DatabaseManagementSystem = dbms;



            }

            SetWorkerRoleName();

            if (LogitudeSettings.IsCostomsDeploy)
            {
                var he = new CultureInfo("he-IL");// '("en-US") '    "he-IL")
                he.DateTimeFormat.DateSeparator = ".";
                he.DateTimeFormat.ShortDatePattern = "dd-MM-yy";// ' "yyyy/MM/dd" '  ' "DD/MM/YYYY"
                System.Threading.Thread.CurrentThread.CurrentCulture = he;

                LogitudeSettings.RunWorkerRoleAutomaticBreakPoint = false;

                LogitudeSettings.WorkerRoleName = LogitudeSettings.WorkerRoleName?? "production";
            }


            CacheManager.CacheWrapper = CacheManager.CacheWrapper ?? new CacheWrapper(Cache);//Where is the cache (Why as usuall i neeed to do averything ?!?)
        }

        public void StartLogging()
        {
            try
            {
                batchServiceLogTimer.Stop();
                foreach (WorkerEntryPoint worker in workers)
                {
                    worker.LogStatisticInDB();
                    //if (!string.IsNullOrEmpty(worker.ThreadId))
                    //{
                    //    BatchServiceLogParams logParams = new BatchServiceLogParams()
                    //    {
                    //        BatchServiceCode = worker.BatchServiceCode,
                    //        CreateDate = DateTime.UtcNow,
                    //        Id = worker.ThreadId,
                    //        LastActivity = worker.LastActivity,
                    //        NumberOfDoneItems = worker.NumberOfDoneItems,
                    //        CPU = worker.CPU
                    //    };

                    //    if (worker.DoneItemsInRange != null)
                    //    {
                    //        DateTime currentDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, 0);

                    //        int doneItemsInOneMinute = worker.DoneItemsInRange.Where(d => d.Key <= currentDate && d.Key > currentDate.AddMinutes(-1)).Sum(d => d.Value);
                    //        int doneItemsInFiveMinutes = worker.DoneItemsInRange.Where(d => d.Key <= currentDate && d.Key > currentDate.AddMinutes(-5)).Sum(d => d.Value);
                    //        int doneItemsInOneHour = worker.DoneItemsInRange.Where(d => d.Key <= currentDate && d.Key > currentDate.AddMinutes(-60)).Sum(d => d.Value);

                    //        logParams.DoneItemsInFiveMinutes = doneItemsInFiveMinutes;
                    //        logParams.DoneItemsInOneHour = doneItemsInOneHour;
                    //        logParams.DoneItemsInOneMinute = doneItemsInOneMinute;
                    //    }

                    //    BatchServicesLogger.Log(logParams);
                    //}
                }

                batchServiceLogTimer.Start();
            }

            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.UtcNow, 0, "", "WorkerRole", "ThreadedRoleEntryPoint :  StartLoggingMethod log is in utc", null);
                batchServiceLogTimer.Start();
            }
        }
        private static void EnsureHttpRuntime()
        {
            try
            {
                if (null == _httpRuntime)
                {
                    try
                    {
                        //Monitor.Enter(typeof(State));
                        if (null == _httpRuntime)
                        {
                            // Create an Http Content to give us access to the cache.
                            _httpRuntime = new HttpRuntime();

                        }
                    }
                    finally
                    {
                        //Monitor.Exit(typeof(State));
                    }

                }
            }
            catch (Exception e)
            {
                //Logger.LogMe(e.ToString(), true);
            }
        }
        public static HttpRuntime _httpRuntime { get; set; }
        public static Cache Cache
        {
            get
            {
                try
                {
                    EnsureHttpRuntime();
                    return HttpRuntime.Cache;
                }
                catch (Exception e)
                {
                    //Logger.LogMe(e.ToString(), true);
                }
                return null;

            }



        }
    }

    public class ServiceDefinition
    {
        public string SarviceName { get; set; }
        public int MaxWorkingTimeInMinutes { get; set; }
    }
}
