using AmitalCustomsWindowsService.BL;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.Server.Tools.Utils;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel.Repsitories;
using Logitude.Server.Tools;
using Simplog.Global.Data.GlobalModel;
using System.Configuration;
using Devart.Data.Oracle;
using CustomsWorkerRole;
using Logitude.Customs.BL.PatchDistribution;
using System.Diagnostics;
using CommunicationWorkerRole;
using System.IO;
using Logitude.CustomsMessaging.Dca;
using System.Reflection;

namespace AmitalCustomsWindowsService
{
    class DBWorkerService
    {

        List<Thread> _Threads;

        List<IWorkerBaseWorkOnce> _Workers;
        private DateTime? _ThreadsStartAt;
        private bool _AllWorkerLoaded;

        public bool HaveDB()
        {
            try
            {


                var myDB = GlobalContext.GetContext((int)TimeSpan.FromMinutes(2).TotalSeconds, true) as DbContextBase;
                var myDualRepository = new DualRepository(myDB);
                var dt = DateTime.Now;
                return true;
            }
            catch (System.Exception e)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(e);
                return false;

            }

        }
        public void InvokeStatistics()
        {
            _Workers.ForEach(w =>
            {
                w.InvokeStatistics();
            });
        }


        public void EnshureThreadWorking(bool forceStartAgain)
        {

            Program.ThreadStartStaticIsMustB4UsingTheDB();
            if (!HaveDB()  )
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError("no connection to db, stop all threads");
                StopThreads();
                return;
            }

            LoadWorkerFromDB();
            if (forceStartAgain)
            {
                StopThreads();
            }

            AllThreadsAreAlive();


        }

        public static bool IsOldDB()
        {
            try
            {

                //var myP19R03_0000_PatchDist = new P19R03_0001_PatchDist();
                //myP19R03_0000_PatchDist.Enshure_SeedDbMigrateTable();

                var _PatchDistributionManager = new PatchDistributionManager();
                _PatchDistributionManager.Check_PatchDistributionListAreValid();



                var assemblyUtil = new Logitude.Server.Tools.Helpers.AssemblyUtil();
                var prodInfo = assemblyUtil.GetProductInfo(typeof(JustWebFreight.WebFreight.Web.MetaDataUpdate.GeneratedUpdate.EntityUpdateClasses.MyEntityUpdateClass).Assembly);
                var assemblyVersion = assemblyUtil.GetVersion(prodInfo);



                var patchDistributionMatch = new PatchDistributionMatch();
                var _PatchDistributionMatchModel = patchDistributionMatch.GetPatchDistributionMatchModel(assemblyVersion);

               NetCommonHelper.Logger.DevLog.Instance.WriteDebug(_PatchDistributionMatchModel.Message);

                if (assemblyVersion == "1.0.0.0" || _PatchDistributionMatchModel.LastClosed_DBMigration == null)
                {
                    return false;
                }
               NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"assemblyVersion ={assemblyVersion}");
               NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"DB MajorVersion={_PatchDistributionMatchModel.LastClosed_DBMigration.MajorVersion}");
               NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"DB MinorVersion Last Closed !!!={_PatchDistributionMatchModel.LastClosed_DBMigration.MinorVersion}");
                //Debug.WriteLine($"DB MinorLine={_PatchDistributionMatchModel.Last_DBMigrationLine.CounterKey}");
                if (_PatchDistributionMatchModel.MajorVersionMatch == PatchDistributionMatch.MajorVersionMatchEnum.OldDB)
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteError($"shuttttdown !!!_PatchDistributionMatchModel.MajorVersionMatch == PatchDistributionMatch.MajorVersionMatchEnum.OldDB");
                    return true;

                }
                if (
                    _PatchDistributionMatchModel.MyAssemblyDBMigrationModel.MinorVersion
                    >
                    _PatchDistributionMatchModel.LastClosed_DBMigration.MinorVersion)
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteError($"shuttttdown !!!OldDB !!! MyAssemblyDBMigrationModel.MinorVersion {_PatchDistributionMatchModel.MyAssemblyDBMigrationModel.MinorVersion}> _PatchDistributionMatchModel.LastClosed_DBMigration.MinorVersion {_PatchDistributionMatchModel.LastClosed_DBMigration.MinorVersion}");
                    return true;
                }
                return false;
            }
            catch (Exception ee)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ee,"IsOldDB -- " );
                return true;
            }
        }

        private void AllThreadsAreAlive()
        {
            if (WorkerRoleServiceLocator.PleaseShutDown)
            {
                WorkerRoleServiceLocator.PleaseShutDown = false;
                NetCommonHelper.Logger.DevLog.Instance.WriteWarning("WorkerRoleServiceLocator.PleaseShutDown = false;");

            }
            for (Int32 iWorker = 0; iWorker < _Workers.Count; iWorker++)
            {
                _Workers[iWorker].ServiceStarted = true;//startIt
                if (!_Threads[iWorker].IsAlive)
                {
                    if (_Workers[iWorker].WhileServiceStarted_IsOut)
                    {
                        StartThread(iWorker);
                    }
                    else
                    {
                        NetCommonHelper.Logger.DevLog.Instance.WriteInfo(GetThreadName(iWorker)+":"+ "NotIsAliveBut_WhileServiceStarted_IsOut");
                    }
                    //_Threads[iWorker] = new Thread(_Workers[iWorker].Run);
                    //_Threads[iWorker].Start();

                }
            }

        }

        private void StartThread(int iWorker)
        {
            _Workers[iWorker].ServiceStarted = true;//startIt
            ThreadStart st = new ThreadStart(_Workers[iWorker].ExecuteTask);
            var currThread = new Thread(st);
            _Workers[iWorker].ManagedThreadId = currThread.ManagedThreadId;
            currThread.Name = GetThreadName(iWorker);
            //_Threads.Add(t);
            _Threads[iWorker] = currThread;
            _Threads[iWorker].Start();
            NetCommonHelper.Logger.DevLog.Instance.WriteInfo(GetThreadName(iWorker));
        }

        private string GetThreadName(int iWorker)
        {
            return _Workers[iWorker].MyType + ":" + iWorker.ToString();
        }

        
        private void LoadWorkerFromDB()
        {
            if (!_AllWorkerLoaded)
            {
                _Workers = new List<IWorkerBaseWorkOnce>();
                _Threads = new List<Thread>(_Workers.Count);
                List<BatchServicesDefinitionPM> BatchServicesDefinitions = GetBatchServicesDefinitions();

                List<BatchServicesDefinitionPM> batchServicesDefinitionsGroups = BatchServicesDefinitions?.Where(b =>
                          !string.IsNullOrEmpty(b.QueueBase) && !string.IsNullOrEmpty(b.Code)
                          && b.Code.IndexOf(b.QueueBase, StringComparison.OrdinalIgnoreCase) >= 0
                          && b.Code.Length > b.QueueBase.Length)?.ToList();



                 LoadWorkerFromDB(BatchServicesDefinitions, batchServicesDefinitionsGroups);


                for (int iWorker = 0; iWorker < _Workers.Count; iWorker++)
                {
                    StartThread(iWorker);
                }
                _AllWorkerLoaded = true;
            }
        }




        public void AddWorkerFromAppSettingDB<TWorker>(bool suppresDoOnlyCheck = false,String queueDefinitionCode=null, WorkerQueueType workerQueueType= WorkerQueueType.DB)
       where TWorker : Logitude.Server.Tools.WorkerEntryPointDoneLog, new()
        {
            if (suppresDoOnlyCheck)
            {
                return;
            }
            var type = typeof(TWorker);
            var typeName = type.Name;

            var workerOnce = new WorkerOnce<TWorker>(1, _Workers.Count) { ServiceStarted = true , QueueDefinitionCode = queueDefinitionCode , WorkerQueueType  =workerQueueType };

            _Workers.Add(workerOnce);
            _Threads.Add(null);


        }
        private void LoadWorkerFromDB(List<BatchServicesDefinitionPM> BatchServicesDefinitions, List<BatchServicesDefinitionPM> BatchServicesDefinitionsGroups)
        {

            var suppresDoOnlyCheck = false;
            string queueDefinitionCode = null;    
            WorkerQueueType workerQueueType = WorkerQueueType.DB;

            var addWorkerFromAppSettingMethodInfoDB = typeof(DBWorkerService).GetMethod("AddWorkerFromAppSettingDB");
            if (addWorkerFromAppSettingMethodInfoDB == null)
            {
                throw new Exception("how change code where is method >public AddWorkerFromAppSettingDB");
            }
            var listOfWorkerEntryPoint = CustomsWorkerRole.AllWorkerEntryPointTypeService.GetAllWorkerEntryPointType();
            listOfWorkerEntryPoint.Add(new CommunicationWorkerRole.FTPCommunicationWorkerRoleWinService());



            listOfWorkerEntryPoint.Add(new SendWEBAPIMessage2MamanWR());
            listOfWorkerEntryPoint.Add(new CustomsAnalyzeQueueWR());
            listOfWorkerEntryPoint.Add(new RabbitMQReceiveWR());
            listOfWorkerEntryPoint.Add(new CustomsHSMSignWR());
            listOfWorkerEntryPoint.Add(new ReportExecutionLogWR());
            listOfWorkerEntryPoint.Add(new SyncRecordsCCUTableWR());
			listOfWorkerEntryPoint.Add(new DocumentAzureQueueWR());
			listOfWorkerEntryPoint.Add(new DocumentSFTPAnalyzeWR());

			bool testCustomsSchedularWR = false;
            if (testCustomsSchedularWR)
            {
                listOfWorkerEntryPoint = new List<Logitude.Server.Tools.WorkerEntryPoint>();
            }
            listOfWorkerEntryPoint.Add(new CustomsSchedularWR());



            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("Discard.DownloadDcaMessageSheetWR")))
            {
                BatchServicesDefinitions = BatchServicesDefinitions.Where(r => r.ClassName != "DownloadDcaMessageSheetWR").ToList();
            }

            var dedicatedCourierDCAService = new DedicatedCourierDCAService();
            var modelDedicatedCourierDCA = dedicatedCourierDCAService.CreateDedicatedCourierDCA();
            if (modelDedicatedCourierDCA != null)//Task 147744: AMITALCUSTOMSSERVER העברת הטיפול בכספת בבלדרות לתהליך
            {
                listOfWorkerEntryPoint = new List<Logitude.Server.Tools.WorkerEntryPoint>();
                listOfWorkerEntryPoint.Add(new DownloadDcaMessageSheetWR());
                var AddWorkerFromAppSettingGenericMethod = addWorkerFromAppSettingMethodInfoDB.MakeGenericMethod(new Type[] { (new DownloadDcaMessageSheetWR()).GetType() });
                AddWorkerFromAppSettingGenericMethod.Invoke(this, new object[] { (object)suppresDoOnlyCheck, (object)queueDefinitionCode, (object)workerQueueType }); 
                return;
            }

            bool test = false;
            if (test)
            {
                BatchServicesDefinitions = BatchServicesDefinitions.Where(r => r.ClassName == "DownloadDcaMessageSheetWR").ToList();
                //BatchServicesDefinitions = BatchServicesDefinitions.Where(r => r.ClassName == "CustomsHSMSignWR").ToList();
            }


            if (BatchServicesDefinitionsGroups?.Any() == true)
            {
                foreach (var g in BatchServicesDefinitionsGroups.Where(x => !string.IsNullOrEmpty(x.ClassName)))
                {
                    var type = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(a =>
                        {
                            try
                            {
                                return a.GetTypes();
                            }
                            catch (ReflectionTypeLoadException ex)
                            {
                                return ex.Types.Where(t => t != null);
                            }
                            catch
                            {
                                return Enumerable.Empty<Type>();
                            }
                        })
                        .FirstOrDefault(t =>
                            t.Name == g.ClassName &&
                            typeof(Logitude.Server.Tools.WorkerEntryPoint).IsAssignableFrom(t));

                    if (type == null)
                        continue;

                    try
                    {
                        var instance = (Logitude.Server.Tools.WorkerEntryPoint)
                            Activator.CreateInstance(type);

                        listOfWorkerEntryPoint.Add(instance);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }


            foreach (var batchServicesDefinitionPM in BatchServicesDefinitions)
            {
                var worker = listOfWorkerEntryPoint.FirstOrDefault(r => r.NameOf() == batchServicesDefinitionPM.Code);
                if (worker != null)
                {
                    for (int i = 0; i < batchServicesDefinitionPM.NumberOfThreads; i++)
                    {
                        var AddWorkerFromAppSettingGenericMethod = addWorkerFromAppSettingMethodInfoDB.MakeGenericMethod(new Type[] { worker.GetType() });
                        AddWorkerFromAppSettingGenericMethod.Invoke(this, new object[] { (object)suppresDoOnlyCheck, (object)queueDefinitionCode, (object)workerQueueType });
                    
                    }
                }
            }
            if(BatchServicesDefinitionsGroups != null)
            {
                foreach (var batchServicesDefinitionPM in BatchServicesDefinitionsGroups)
                {
                    var worker = listOfWorkerEntryPoint.FirstOrDefault(r => r.NameOf() == batchServicesDefinitionPM.QueueBase);
                    if (worker != null)
                    {
                        for (int i = 0; i < batchServicesDefinitionPM.NumberOfThreads; i++)
                        {
                            var AddWorkerFromAppSettingGenericMethod = addWorkerFromAppSettingMethodInfoDB.MakeGenericMethod(new Type[] { worker.GetType() });
                            AddWorkerFromAppSettingGenericMethod.Invoke(this, new object[] { (object)suppresDoOnlyCheck, batchServicesDefinitionPM.Code, (object)workerQueueType });
                    
                        }
                    }
                }
            }
            ////FROM CONFIG !!! 
            SingletonFTPCommunicationLogQueue(listOfWorkerEntryPoint);
        }

        private void SingletonFTPCommunicationLogQueue(List<Logitude.Server.Tools.WorkerEntryPoint> listOfWorkerEntryPoint)
        {
            var suppresDoOnlyCheck = false;
            string queueDefinitionCode = null;
            WorkerQueueType workerQueueType = WorkerQueueType.DB;

            var addWorkerFromAppSettingMethodInfoDB = typeof(DBWorkerService).GetMethod("AddWorkerFromAppSettingDB");
            //INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONS"(CODE, CLASSNAME) VALUES('SingletonFTPCommunicationWorkerRoleWinService', 'SingletonFTPCommunicationWorkerRoleWinService');
            //INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONMODS" VALUES('SingletonFTPCommunicationWorkerRoleWinService', '0', '1');

            ///< add key = "SingletonFTPCommunicationWorkerRoleWinService" value = "1" />
            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("SingletonFTPWorker")))/*"SingletonFTPCommunicationLogQueue"*/
            {
                listOfWorkerEntryPoint.Add(new CommunicationWorkerRole.SingletonFTPCommunicationWorkerRoleWinService());
                var AddWorkerFromAppSettingGenericMethod = addWorkerFromAppSettingMethodInfoDB.MakeGenericMethod(new Type[] { (new SingletonFTPCommunicationWorkerRoleWinService()).GetType() });
                AddWorkerFromAppSettingGenericMethod.Invoke(this, new object[] { (object)suppresDoOnlyCheck, (object)queueDefinitionCode, (object)workerQueueType });

                ///listOfWorkerEntryPoint.Add(new FTPToAnalyzeQueueWR());

                listOfWorkerEntryPoint.Add(new FTPToAnalyzeQueueWR());
                var AddWorkerFromAppSettingGenericMethodDown = addWorkerFromAppSettingMethodInfoDB.MakeGenericMethod(new Type[] { (new FTPToAnalyzeQueueWR()).GetType() });
                AddWorkerFromAppSettingGenericMethodDown.Invoke(this, new object[] { (object)suppresDoOnlyCheck, (object)queueDefinitionCode, (object)workerQueueType });

            }

        }


        private List<BatchServicesDefinitionPM> GetBatchServicesDefinitions()
        {
            BatchServicesDefinitionRepository BatchServicesRepository = new BatchServicesDefinitionRepository();
            BatchServicesDefinitionQuery BatchServicesQuery = new BatchServicesDefinitionQuery(BatchServicesRepository);
            List<BatchServicesDefinitionPM> BatchServicesDefinitions = BatchServicesQuery.GetAllActiveBatchServicesDefinitions().ToList();//.Where(b => b.Code == "EmailOut-EmailQueue")
            return BatchServicesDefinitions;
        }

        public void StopThreads()
        {
            if (_Workers == null)
            {
                return;
            }
            try
            {
                ///WorkerRoleServiceLocator.PleaseShutDown = true;

                for (int i = 0; i < _Workers.Count; i++)
                {
                    StopThread(i);

                }
                //if (WorkerRoleServiceLocator.HaveCourierTenant)
                //{
                //    Logger.LogMe("Is Courier Wait 60Sec", false);
                //    var sw = Stopwatch.StartNew();
                //    while (sw.Elapsed < TimeSpan.FromSeconds(60))
                //    {
                //        if (!_Threads.Any(r => !r.IsAlive))
                //        {
                //            break;
                //        }
                //        Thread.Sleep(300);
                //    }
                //    bool b = _Threads.Any(r => !r.IsAlive);
                //    Logger.LogMe($"All thread down == _Threads.Any(r => !r.IsAlive)? ={b} ", false);
                //    Thread.Sleep(100);
                //}
                //else
                {
                    Thread.Sleep(1000);
                }




            }
            catch (Exception e)
            {

                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(e);
            }

        }

        private void StopThread(int iWorker)
        {

            _Workers[iWorker].ServiceStarted = false;//== dispose !!!
            NetCommonHelper.Logger.DevLog.Instance.WriteInfo(GetThreadName(iWorker));
        }






    }
}
