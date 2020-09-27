using AmitalCustomsWindowsService.BL;
using Logitude.Server.Tools.Utils;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace AmitalCustomsWindowsService
{
    partial class AmitalCustomTolerantWindowsService : ServiceBase , IServiceStartMe
    {

        private System.Timers.Timer _AmitalCustomWindowsServiceTimer;
        DateTime DbLogAt = DateTime.MinValue;
        DateTime GCAt = DateTime.MinValue;
        DBWorkerService _DBWorkerService;
        public AmitalCustomTolerantWindowsService()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            _DBWorkerService = new DBWorkerService();
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var err = e.ExceptionObject.ToString();
            Logger.LogMe("CurrentDomain_UnhandledException!!!" + e.IsTerminating.ToString() + err, true);
            Logger.LogMe("CurrentDomain_UnhandledException!!!" + e.ToString(), true);
            //System.Diagnostics.Debugger.Launch();
            var featureCheckMaxPoolSizeWasReachedThenRetart = ConfigurationManager.AppSettings["20180219.CheckMaxPoolSizeWasReachedThenRetart"] == "1";
            if (featureCheckMaxPoolSizeWasReachedThenRetart) { }
            Environment.Exit(-1);
        }

        public void StartMe()
        {
            try
            {
                if (_AmitalCustomWindowsServiceTimer == null)
                {
                    this._AmitalCustomWindowsServiceTimer = new System.Timers.Timer();
                    
                    TimeSpan t = new TimeSpan(0, 0, 30);
                    _AmitalCustomWindowsServiceTimer.Interval = (int)t.TotalMilliseconds;
                    _AmitalCustomWindowsServiceTimer.Stop();
                    _AmitalCustomWindowsServiceTimer.Elapsed += new System.Timers.ElapsedEventHandler(amitalCustomWindowsServiceTimer_Elapsed);
                }
                _DBWorkerService.EnshureThreadWorking(false);
            }
            catch (Exception e)
            {

                Logger.LogMe("startMe:" + e.ToString(), true);
            }
            finally
            {
                
                _AmitalCustomWindowsServiceTimer.Start();
                ///ServiceState.ClearSandBoxDir();
            }
        }
        DateTime _LastRestartAt = DateTime.Now;
        private void amitalCustomWindowsServiceTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            
            bool restart = false;
            int? restartEveryInMin = null;
            try
            {
                _AmitalCustomWindowsServiceTimer.Stop();
                //Logger.LogMe("timer1_Tick", false, "timer1");
                Program.ThreadStartStaticIsMustB4UsingTheDB();

                //_DBWorkerService.CheckOldThreads();
                var featureCheckMaxPoolSizeWasReachedThenRetart = ConfigurationManager.AppSettings["20180219.CheckMaxPoolSizeWasReachedThenRetart"] == "1";
                var threadsMaxPoolSizeWasReachedWhileSave = false;
                if (featureCheckMaxPoolSizeWasReachedThenRetart)
                {
                    threadsMaxPoolSizeWasReachedWhileSave = DbContextBaseUtil.MaxPoolSizeWasReachedWhileSave.HasValue;
                    if (!threadsMaxPoolSizeWasReachedWhileSave)
                    {
                        threadsMaxPoolSizeWasReachedWhileSave = TesterRestartMe();
                    }
                }

                if (threadsMaxPoolSizeWasReachedWhileSave)
                {
                    Logger.LogMe("DbContextBaseUtil.MaxPoolSizeWasReachedWhileSave=" + DbContextBaseUtil.MaxPoolSizeWasReachedWhileSave.GetValueOrDefault().ToString() , false, "ThreadsMaxPoolRestart");
                    _DBWorkerService.StopThreads();
                    Thread.Sleep(TimeSpan.FromMinutes(1));
                    DbContextBaseUtil.MaxPoolSizeWasReachedWhileSave = null;

                    Environment.Exit(-1);
                    return;
                }
                if (!threadsMaxPoolSizeWasReachedWhileSave && _DBWorkerService.HaveDB())
                {

                    restartEveryInMin = -1;//suppress feature!!!  ///GetRestartThreadsEveryInMin();
                    if (restartEveryInMin.HasValue && restartEveryInMin.GetValueOrDefault() > 10)
                    {
                        if (DateTime.Now.Subtract(_LastRestartAt) > TimeSpan.FromMinutes(restartEveryInMin.Value))
                        {
                            restart = true;
                            Logger.LogMe("restart now ", false, "restartEveryInMin");
                        }
                    }
                    _DBWorkerService.EnshureThreadWorking(restart);
                    _DBWorkerService.InvokeStatistics();
                }
                else
                {
                    
                    _DBWorkerService.StopThreads();
                    Thread.Sleep(TimeSpan.FromMinutes(1));
                    DbContextBaseUtil.MaxPoolSizeWasReachedWhileSave = null;
                }

                SaveState();
                //Logger.LogMe("timer1_Tick", false, "timer2");
            }
            catch (Exception ex)
            {
                Logger.LogMe("timer1_Tick:" + ex.ToString(), true);
            }
            finally
            {
                _AmitalCustomWindowsServiceTimer.Start();
                if (restart)
                {
                    _LastRestartAt = DateTime.Now;
                }
            }
        }

        private static bool TesterRestartMe()
        {
            var retartMeFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "RestartMe.txt");
            try
            {
                if (File.Exists(retartMeFile))
                {
                    File.Delete(retartMeFile);
                    return true;
                }
            }
            catch (Exception)
            {

            }
            

            return false;
        }

        static int? _RestartThreadsEveryInMin = null;
        private int? GetRestartThreadsEveryInMin()
        {
            if (_RestartThreadsEveryInMin.HasValue) return _RestartThreadsEveryInMin;
            try
            {
                

                if (String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["RestartThreadsEveryInMin"]))
                {
                    _RestartThreadsEveryInMin = -1;

                }
                else
                {
                    var s = ConfigurationManager.AppSettings["RestartThreadsEveryInMin"].ToString();
                    _RestartThreadsEveryInMin = int.Parse(s);
                }
            }
            catch (Exception)
            {
                _RestartThreadsEveryInMin = -1;


            }
            return _RestartThreadsEveryInMin;
        }

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
                var state = ServiceState.GetState();
                if (DateTime.Now.Subtract(GCAt) > TimeSpan.FromMinutes(10))
                {
                    GCAt = DateTime.Now;
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

        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.
            Task.Factory.StartNew(() => {
                SMTP.SendItDefault(Environment.CommandLine.ToString() + " ", "AmitalCustomsWindowsService:OnStart()");
            });
            Task.Factory.StartNew(() => {
                StartMe();
            });
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
            Logger.LogMe("OnStop()", false);

            _DBWorkerService.StopThreads();
        }
    }
}
