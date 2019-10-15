using AmitalCustomsWindowsService.BL;
using AmitalCustomsWindowsService.Utils;
using CustomsWorkerRole;
using CustomsWorkerRole.Utils;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Utils;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using CommunicationWorkerRole;
using Logitude.BL.GlobalModel.EntityQueries;
using Simplog.Server.Infrastructure.Helpers;

namespace AmitalCustomsWindowsService
{
    public partial class MyWinService : ServiceBase, IServiceStartMe
    {
        // array of worker threads
        List<Thread> _Threads;
        List<IWorkerBaseWorkOnce> _Workers;
        //private List<IWorkerBaseWorkOnce> _WorkersWorkOnce;
        int _workerId=0;
        private System.Timers.Timer _myTimer;
        
        

        public MyWinService()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            //throw new Exception("3333"); 
  
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var err = e.ExceptionObject.ToString();
            Logger.LogMe("CurrentDomain_UnhandledException!!!" + e.IsTerminating.ToString() + err, true);
            Logger.LogMe("CurrentDomain_UnhandledException!!!" + e.ToString(), true);
            //System.Diagnostics.Debugger.Launch();
        }

        protected override void OnStart(string[] args)
        {
            
             Task.Factory.StartNew( () => {
                SMTP.SendItDefault(Environment.CommandLine.ToString() + " " , "AmitalCustomsWindowsService:OnStart()"); 
             });

            Task.Factory.StartNew(() => {
                StartMe();
            });
            
            
        }
        public override EventLog EventLog
        {
            get
            {
                return base.EventLog;
            }
        }
        protected override object GetService(Type service)
        {
            return base.GetService(service);
        }
        protected override void OnShutdown()
        {
            
            base.OnShutdown();
        }
        public override ISite Site
        {
            get
            {
                return base.Site;
            }
            set
            {
                base.Site = value;
            }
        }
        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            base.OnSessionChange(changeDescription);
        }
        protected override void OnStop()
        {
            

            Logger.LogMe("OnStop()", false);
            if (false)
            {
                SMTP.SendItDefault(Environment.CommandLine.ToString() + " ", "AmitalCustomsWindowsService:OnStop()");
                Logger.LogMe("OnStop()!!!", false);

            }
            for (int i = 0; i < _Workers.Count; i++)
            {

                _Workers[i].ServiceStarted = false;
                //_Threads[i].Join(TimeSpan.FromSeconds(15));
            }

            for (int i = 0; i < _Workers.Count; i++)
            {

                if (_Workers[i].MyType == "DownloadDcaMessageSheetWR")
                {
                    _Threads[i].Join(TimeSpan.FromSeconds(0.5));
                }

            }
        }

        public void StartMe()
        {

            //SMTP.SendItDefault(Environment.CommandLine.ToString() + " " , "AmitalCustomsWindowsService:OnStart()"); 
            Program.ThreadStartStaticIsMustB4UsingTheDB();
            if (_Workers == null)
            {
                _Workers = new List<IWorkerBaseWorkOnce>();
                var listOfWorkerEntryPoint = CustomsWorkerRole.ThreadedRoleEntryPoint.GetAllWorkerEntryPointType();

                bool suppresDoOnlyCheck = true;
                if (false)//no 10x - change to interactive in TesterForm
                {
                    //AddWorkerFromAppSetting<CommunicationWorkerRole.SignUpWorkerRoleWinService>();    
                }
                
                var addWorkerFromAppSettingMethodInfo = typeof(MyWinService).GetMethod("AddWorkerFromAppSetting");
                if (addWorkerFromAppSettingMethodInfo == null)
                {
                    throw new Exception("how change code where is method >public AddWorkerFromAppSetting");  
                }

                AddWorkerFromAppSetting<CommunicationWorkerRole.FTPCommunicationWorkerRoleWinService>();
                AddWorkerFromAppSetting<SendWEBAPIMessage2MamanWR>();
                AddWorkerFromAppSetting<FTPToAnalyzeQueueWR>();
                
                bool courierFeaturePackageExist = true;
                if (courierFeaturePackageExist)
                {
                    AddWorkerFromAppSetting<SendWEBAPIMessage2MamanWR>();
                    AddWorkerFromAppSetting<FTPToAnalyzeQueueWR>();
                    AddWorkerFromAppSetting<CustomsAnalyzeQueueWR>();
                    


                }
                if (!AmitalProxy.Have_UnfConnectionString())
                {
                    /// itzik+ihab  - AddWorkerFromAppSetting<CommunicationWorkerRole.CommunicationLogWorkerRoleWinService>();// Email change Pass ?!?!?    
                }
                // Email change Pass ?!?!?
                this._FromDB = true;
                if (this._FromDB)
                {
                    LoadWorkerFromDB();
                    ///return;
                }
                else
                {
                    foreach (var worker in listOfWorkerEntryPoint)
                    {
                        suppresDoOnlyCheck = false;

                        var AddWorkerFromAppSettingGenericMethod = addWorkerFromAppSettingMethodInfo.MakeGenericMethod(new Type[] { worker.GetType() });
                        AddWorkerFromAppSettingGenericMethod.Invoke(this, new object[] { (object)suppresDoOnlyCheck });

                    }
                }
                
                listOfWorkerEntryPoint.Clear();
                listOfWorkerEntryPoint = null;

                //AddAllWR();
                if (_Workers.Count < 1)
                {
                    Logger.LogMe("unexpected setting - No Worker Loaded !!!!!!!!!!!", true);
                    return;
                }

                _Threads = new List<Thread>();
                for (int i = 0; i < _Workers.Count; i++)
                {
                    ThreadStart st = new ThreadStart(_Workers[i].ExecuteTask);
                    var t = new Thread(st);
                    _Workers[i].ManagedThreadId = t.ManagedThreadId;
                    _Threads.Add(t);
                }


                // start the threads
                for (int i = 0; i < _Workers.Count; i++)
                {
                    _Threads[i].Start();
                }
            }




            
            if (_myTimer == null)
            {
                this._myTimer = new System.Timers.Timer();
                ServiceState.ClearSandBoxDir();
                TimeSpan t = new TimeSpan(0, 0, 30);
                _myTimer.Interval = (int)t.TotalMilliseconds;
                _myTimer.Enabled = true;
                _myTimer.Elapsed += new System.Timers.ElapsedEventHandler(_myTimer_Elapsed);
            }

        }

        private void LoadWorkerFromDB()
        {

            var suppresDoOnlyCheck = false;
            var addWorkerFromAppSettingMethodInfoDB = typeof(MyWinService).GetMethod("AddWorkerFromAppSettingDB");
            if (addWorkerFromAppSettingMethodInfoDB == null)
            {
                throw new Exception("how change code where is method >public AddWorkerFromAppSettingDB");
            }
            var listOfWorkerEntryPoint = CustomsWorkerRole.ThreadedRoleEntryPoint.GetAllWorkerEntryPointType();
            ///itzik +  ihab  listOfWorkerEntryPoint.Add(new CommunicationWorkerRole.CommunicationLogWorkerRoleWinService());
            listOfWorkerEntryPoint.Add(new CommunicationWorkerRole.FTPCommunicationWorkerRoleWinService());
            listOfWorkerEntryPoint.Add(new SendWEBAPIMessage2MamanWR());
            listOfWorkerEntryPoint.Add(new FTPToAnalyzeQueueWR());
            listOfWorkerEntryPoint.Add(new CustomsAnalyzeQueueWR());
            

            BatchServicesDefinitionRepository BatchServicesRepository = new BatchServicesDefinitionRepository();
            BatchServicesDefinitionQuery BatchServicesQuery = new BatchServicesDefinitionQuery(BatchServicesRepository);
            List<BatchServicesDefinitionPM> BatchServicesDefinitions = BatchServicesQuery.GetAllActiveBatchServicesDefinitions().ToList();//.Where(b => b.Code == "EmailOut-EmailQueue")
            foreach (var batchServicesDefinitionPM in BatchServicesDefinitions)
            {
                var worker =listOfWorkerEntryPoint.FirstOrDefault(r => r.NameOf() == batchServicesDefinitionPM.Code);
                if (worker != null)
                {
                    for (int i = 0; i < batchServicesDefinitionPM.NumberOfThreads; i++)
                    {
                        var AddWorkerFromAppSettingGenericMethod = addWorkerFromAppSettingMethodInfoDB.MakeGenericMethod(new Type[] { worker.GetType() });
                        AddWorkerFromAppSettingGenericMethod.Invoke(this, new object[] { (object)suppresDoOnlyCheck });
                    }
                }
                
            }
        }
        
        private void AddAllWR()
        {
            //_Workers = new List<WorkerBase>();
            
            //workers.Add(new SendDataToAmitalWR());
            //workers.Add(new DownloadDcaMessagesWR());
            //workers.Add(new AnalyzeQueueMessagesWR());

            //_Workers.Add(new AnalyzeQueueMessagesWorker(1, 2) { ServiceStarted = true });
            //AddWorkerFromAppSetting<AnalyzeQueueMessagesWR>();

            //AddWorkerFromAppSetting<DownloadDcaMessagesWR>();

            AddWorkerFromAppSetting<SendDataToExternalServicesWR>();

            //AddWorkerFromAppSetting<UpdateClosedTablesWR>();
            //AddWorkerFromAppSetting<CustomsMessagingOutWR>();

            AddWorkerFromAppSetting<CustomsMessagingSheetWR>();
            AddWorkerFromAppSetting<DownloadDcaMessageSheetWR>();

            AddWorkerFromAppSetting<CustomsCommandGetCustomRequestWR>();
            AddWorkerFromAppSetting<CustomsCommandSignRequestWR>();
            AddWorkerFromAppSetting<CustomsCommandSendDCAWR>();
            AddWorkerFromAppSetting<CustomsCommandSendDCAUploadStatusWR>();
            AddWorkerFromAppSetting<CustomsCommandSendWSReceiveCorrelationWR>();
            AddWorkerFromAppSetting<CustomsCommandDownloadDcaReceiveCorrelationWR>();
            AddWorkerFromAppSetting<CustomsCommandAnalyzeResponseWR>();
        }

        private void _myTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                _myTimer.Stop();
                //Logger.LogMe("timer1_Tick", false, "timer1");
                StartMe();

                if (!_IsCheckOnce)
                {
                    _IsCheckOnce = true;
                    ///CheckOnce();

                }
                _Workers.ForEach(w => {
                    w.InvokeStatistics();
                });


                SaveState();
                //Logger.LogMe("timer1_Tick", false, "timer2");
            }
            catch (Exception ex)
            {
                Logger.LogMe("timer1_Tick:" + ex.ToString(), true);
            }
            finally
            {
                _myTimer.Start();
            }

        }
        DateTime DbLogAt = DateTime.MinValue;
        DateTime GCAt = DateTime.MinValue;
        private bool _FromDB;
        

        private void SaveState()
        {
            
            try
            {

                if (Logger.WorkingDir == "") return;
                Logger.DeleteAllLogState();
                if (!ServiceState.CurrentDate.Equals(DateTime.Now.Date))
                {
                    ServiceState.RaiseAnotherDay();

                }
                var state=ServiceState.GetState();
                if (DateTime.Now.Subtract(GCAt) > TimeSpan.FromMinutes(10))
                {
                    GCAt = DateTime.Now;
                    CacheManager.ClearCacheItems();
                    CustomsWorkerRole.Utils.GenUtil.CollectGC();
                }
                

                if (DateTime.Now.Subtract(DbLogAt) > TimeSpan.FromMinutes(60))
                {

                    

                    DbLogAt = DateTime.Now;
                    CustomsWorkerRole.Utils.LogUtil.LogMe("AmitalCustomsWindowsServiceState",
                    "M", DateTime.Now, "AmitalCustomsWindowsService", state, 0, Environment.UserName, Environment.MachineName, "");
                }
                Logger.LogState(state, "");
            }
            catch (Exception ex)
            {
                Logger.LogMe("SaveState:" + ex.ToString(), true);
            }
        }
        
        public void AddWorkerFromAppSetting<TWorker>(bool suppresDoOnlyCheck=false)
            where TWorker : Logitude.Server.Tools.WorkerEntryPointDoneLog, new()
        {
            if (suppresDoOnlyCheck)
            {
                return;
            }
            //WorkerEntryPoint TWorkerEntryPoint = myWorkerEntryPointType;
            //for (int i = 0; i < 1; i++)//todo more then   1 
            //{
            //    _Workers.Add(new WorkerOnce<AnalyzeQueueMessagesWR>(10, 1) { ServiceStarted = true });
            //}
            var type = typeof(TWorker);
            var typeName = type.Name;
            string workerCount = ConfigurationManager.AppSettings.Get(typeName);
            int iWorkerCount = 0;
            int.TryParse(workerCount, out  iWorkerCount);
            Logger.LogMe("AddWorkerFromAppSetting " + typeName + " Value:" + iWorkerCount.ToString(), false);
            if (iWorkerCount<1)
            {
                Logger.LogMe("AddWorkerFromAppSetting " + typeName + " Value < 0", false);
                return;
            }
            if (iWorkerCount>10)
            {
                Logger.LogMe("unexpected setting (Mean while 3 worker allowed !!) AddWorkerFromAppSetting " + typeName + " Value > 3", true);
                iWorkerCount = 10;
            }
            
            
            for (int i = 0; i < iWorkerCount; i++)//todo more then   1 
            {
                _Workers.Add(new WorkerOnce<TWorker>(1, _Workers.Count) { ServiceStarted = true });
            }
        }


        public void AddWorkerFromAppSettingDB<TWorker>(bool suppresDoOnlyCheck = false)
          where TWorker : Logitude.Server.Tools.WorkerEntryPointDoneLog, new()
        {
            if (suppresDoOnlyCheck)
            {
                return;
            }

            
            //WorkerEntryPoint TWorkerEntryPoint = myWorkerEntryPointType;
            //for (int i = 0; i < 1; i++)//todo more then   1 
            //{
            //    _Workers.Add(new WorkerOnce<AnalyzeQueueMessagesWR>(10, 1) { ServiceStarted = true });
            //}
            var type = typeof(TWorker);
            var typeName = type.Name;

            var workerOnce = new WorkerOnce<TWorker>(1, _Workers.Count) { ServiceStarted = true };
            
            _Workers.Add(workerOnce);

        }


        public bool _IsCheckOnce { get; set; }
    }
}
