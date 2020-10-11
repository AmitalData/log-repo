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
                

                var myDB= GlobalContext.GetContext((int)TimeSpan.FromMinutes(2).TotalSeconds,true) as DbContextBase;
                var myDualRepository = new DualRepository(myDB);
                var dt = myDualRepository.GetServerDateTime(true);
                return true;
            }
            catch (System.Exception e)
            {
                Logger.LogMe(e.ToString(), true, "DbError");
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
            if (!HaveDB() || IsOldDB())
            {
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
                
                Debug.WriteLine(_PatchDistributionMatchModel.Message);

                if (assemblyVersion == "1.0.0.0" || _PatchDistributionMatchModel.LastClosed_DBMigration == null)
                {
                    return false;
                }
                Debug.WriteLine($"assemblyVersion ={assemblyVersion}");
                Debug.WriteLine($"DB MajorVersion={_PatchDistributionMatchModel.LastClosed_DBMigration.MajorVersion}");
                Debug.WriteLine($"DB MinorVersion Last Closed !!!={_PatchDistributionMatchModel.LastClosed_DBMigration.MinorVersion}");
                //Debug.WriteLine($"DB MinorLine={_PatchDistributionMatchModel.Last_DBMigrationLine.CounterKey}");
                if (_PatchDistributionMatchModel.MajorVersionMatch == PatchDistributionMatch.MajorVersionMatchEnum.OldDB)
                {
                    Logger.LogMe($"shuttttdown !!!_PatchDistributionMatchModel.MajorVersionMatch == PatchDistributionMatch.MajorVersionMatchEnum.OldDB", true);
                    return true;

                }
                if (
                    _PatchDistributionMatchModel.MyAssemblyDBMigrationModel.MinorVersion 
                    > 
                    _PatchDistributionMatchModel.LastClosed_DBMigration.MinorVersion)
                {
                    Logger.LogMe($"shuttttdown !!!OldDB !!! MyAssemblyDBMigrationModel.MinorVersion {_PatchDistributionMatchModel.MyAssemblyDBMigrationModel.MinorVersion }> _PatchDistributionMatchModel.LastClosed_DBMigration.MinorVersion {_PatchDistributionMatchModel.LastClosed_DBMigration.MinorVersion}", true);
                    return true;
                }
                return false;
            }
            catch (Exception ee )
            {
                Logger.LogMe("IsOldDB -- " + ee.ToString(), true);
                return true;
            }
        }

        private void AllThreadsAreAlive()
        {

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
                        Logger.LogMe(GetThreadName(iWorker), false, "NotIsAliveBut_WhileServiceStarted_IsOut");
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
            Logger.LogMe(GetThreadName(iWorker), false, "StartThread");
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
                LoadWorkerFromDB(BatchServicesDefinitions);

                
                for (int iWorker = 0; iWorker < _Workers.Count; iWorker++)
                {
                    StartThread(iWorker);
                }
                _AllWorkerLoaded = true;
            }
        }




        public void AddWorkerFromAppSettingDB<TWorker>(bool suppresDoOnlyCheck = false)
       where TWorker : Logitude.Server.Tools.WorkerEntryPointDoneLog, new()
        {
            if (suppresDoOnlyCheck)
            {
                return;
            }
            var type = typeof(TWorker);
            var typeName = type.Name;

            var workerOnce = new WorkerOnce<TWorker>(1, _Workers.Count) { ServiceStarted = true };

            _Workers.Add(workerOnce);
            _Threads.Add(null);


        }
        private void LoadWorkerFromDB(List<BatchServicesDefinitionPM> BatchServicesDefinitions)
        {

            var suppresDoOnlyCheck = false;
            var addWorkerFromAppSettingMethodInfoDB = typeof(DBWorkerService).GetMethod("AddWorkerFromAppSettingDB");
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
            


            foreach (var batchServicesDefinitionPM in BatchServicesDefinitions)
            {
                var worker = listOfWorkerEntryPoint.FirstOrDefault(r => r.NameOf() == batchServicesDefinitionPM.Code);
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


                for (int i = 0; i < _Workers.Count; i++)
                {
                    StopThread(i);

                }
                Thread.Sleep(1000);

            }
            catch (Exception e)
            {

                Logger.LogMe(e.ToString(), true, "StopThreads");
            }

        }

        private void StopThread(int iWorker)
        {
            _Workers[iWorker].ServiceStarted = false;//== dispose !!!
            Logger.LogMe(GetThreadName(iWorker), false, "StopThread");
        }






    }
}
