using AmitalCustomsWindowsService.BL;
using CustomsWorkerRole;
using Logitude.Server.Tools.Utils;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AmitalCustomsWindowsService.Tester.LoadTest
{
    public class LoadTestWorkerService
    {
        List<Thread> _Threads;
        List<IWorkerBaseWorkOnce> _Workers;
        

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

        public void EnshureThreadWorking(bool forceStartAgain)
        {

            Program.ThreadStartStaticIsMustB4UsingTheDB();

            LoadTestWR.Async = !String.IsNullOrEmpty(ConfigurationManager.AppSettings["LoadTestWService.Async"]);



            _Workers = new List<IWorkerBaseWorkOnce>();
            _Threads = new List<Thread>(_Workers.Count);
            string sThreadCount = ConfigurationManager.AppSettings["LoadTestWService.ThreadCount"];
            int iThreadCount = int.Parse(sThreadCount);


            string sTotalRetrieve = ConfigurationManager.AppSettings["LoadTestWService.TotalRetrieve"];
            int iTotalRetrieve = int.Parse(sTotalRetrieve);


            var s = new CommunicationLogStepRepository();
            var q = s.Get104921();
            if (iTotalRetrieve<50*1000)
            {
                q = q.Take(iTotalRetrieve + 1000);
            }
            var t = q.ToListAsync();
            t.Wait();
            LoadTestWR.CommunicationLogList104921 = t.Result;

            
            var sw = Stopwatch.StartNew();
            int totalEachThread = iTotalRetrieve / iThreadCount;
            for (int i = 0; i < iThreadCount; i++)
            {
                //var type = typeof(TWorker);
                //var typeName = type.Name;
                var debugObject = new LoadTestParam()
                {
                    currentThread = i,
                    iTotalRetrieve = iTotalRetrieve,
                    totalEachThread= totalEachThread

                };
                var workerOnce = new WorkerOnce<LoadTestWR>(1, _Workers.Count, debugMode: true, DebugObject: debugObject) { ServiceStarted = true };

                _Workers.Add(workerOnce);
                _Threads.Add(null);

            }


            for (int iWorker = 0; iWorker < _Workers.Count; iWorker++)
            {
                StartThread(iWorker);
            }

            for (int iWorker = 0; iWorker < _Workers.Count; iWorker++)
            {
                _Threads[iWorker].Join();
            }
            Logger.LogMe($"Async:{LoadTestWR.Async} TotalRetrieve:{sTotalRetrieve} ThreadCount{sThreadCount} took:{sw.Elapsed} ", false, "TOT." + (LoadTestWR.Async ? "Async" : "Sync"));

        }
        private void StopThread(int iWorker)
        {
            _Workers[iWorker].ServiceStarted = false;//== dispose !!!
            Logger.LogMe(GetThreadName(iWorker), false, "StopThread");
        }
        private string GetThreadName(int iWorker)
        {
            return _Workers[iWorker].MyType + ":" + iWorker.ToString();
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
    }
    public class LoadTestParam
    {
        internal int currentThread;
        internal int iTotalRetrieve;
        internal int totalEachThread;
    }

    public class LoadTestWR
    : CustomsWorkerEntryPoint
    {
        public const int _ThreadHandeleCount = 1000;

        public static bool Async { get; internal set; }
        public static List<string> CommunicationLogList104921 { get; internal set; }
        static  Random _Random = new Random(Guid.NewGuid().GetHashCode());
        public LoadTestWR()
        {
        }
        public override void Run()
        {
            base.Run();
        }
        public override void OnStop()
        {
            base.OnStop();
        }
        public override void WorkOnce()
        {
            //throw new NotImplementedException();
            Debug.WriteLine("LoadTestWR");

            var myLoadTestParam =this.DebugObject as LoadTestParam;

            //int myStart = _Random.Next(0, CommunicationLogList104921.Count());
            //if (myStart + _ThreadHandeleCount > CommunicationLogList104921.Count())
            //{
            //    myStart = myStart - _ThreadHandeleCount;
            //}
            
            var myHandleList = CommunicationLogList104921.Skip(myLoadTestParam.currentThread * myLoadTestParam.totalEachThread ).Take(myLoadTestParam.totalEachThread);
            var sw = Stopwatch.StartNew();
            var s = new CommunicationLogStepRepository();
            foreach (string CommunicationLogId in myHandleList)
            {

                var q = s.GetQMultiCommunicationLog(CommunicationLogId, 1);
                if (Async)
                {
                    var t = q.ToListAsync();
                    t.Wait();
                    var CommunicationLog = t.Result;

                }
                else
                {
                    var res = q.ToList();
                }
            }
            Logger.LogMe($"Async:{Async} _ThreadHandeleCount:{myLoadTestParam.currentThread} countDone{myLoadTestParam.totalEachThread} took:{sw.Elapsed}  UserInteractive:{Environment.UserInteractive} ", false);

        }
        public override void StartMe()
        {
            base.StartMe();
        }
        public override bool OnStart()
        {
            return base.OnStart();
        }

    }
}
