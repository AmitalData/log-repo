
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Accounting.BL.Utils;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Server.Tools.Helpers;
using Logitude.Accounting.BL.EntityQueryServices;

using Logitude.Accounting.Def.EntityPMs;
using System.Diagnostics;
using Logitude.Accounting.BL.CoreBL;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.QueueService;
using System.Web;

namespace CommunicationWorkerRole// DUE LOADER ///.Accounting
{
    public class AccountingConversionJournalApproveWR : WorkerEntryPoint
    {
        bool _OnStartDone = false;
        int _SleepM = 100;
        private QueueClient _QueueClient;
        //private QueueClient _DeadletterQueueClient;
        DateTime _LastGC = DateTime.MinValue;
        //private bool _UseQueue = true;
        private DbQueueService _DbQueueService;
        public override void Run()
        {

            while (IsRunning)
            {

                if (General.IsUpdating())
                {
                    Thread.Sleep(60000);
                    continue;
                }
                try
                {
                    WorkOnce();
                    Thread.Sleep(
                        TimeSpan.FromSeconds(
                        //1
                        .5
                        )

                        );
                }
                catch (Exception e)
                {
                    ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "AccountingConversionJournalApproveWR : Run() Method", null);
                    Thread.Sleep(10000);
                }

            }

        }

        public override bool OnStart()
        {
            try
            {
                if (_OnStartDone) return true;
                _OnStartDone = true;




                _DbQueueService = new DbQueueService(JournalApproveService.K_AccountingConversionJournalApproveWR, 0);


            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "amital send data worker role start", null, null);
            }

            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            //DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }






        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {

            // If a configuration setting is changing
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {

                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }



        public override void WorkOnce()
        {
            try
            {
                if (DateTime.Now.Subtract(_LastGC) > TimeSpan.FromMinutes(10))//cache 20 min
                {
                    _LastGC = DateTime.Now;
                    //CacheManager.ClearCacheItems();
                    //CustomsWorkerRole.Utils.GenUtil.CollectGC();

                    ///_AllCustomsSetting.Clear();
                    //GenUtil.CollectGC();
                }

                OnStart();


                string logtext = "AccountingConversionJournalApproveWR.WorkOnce(), Point 2, _UseQueue " + _UseQueue.ToString();
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug(logtext);



                var myWorker = new JournalApproveService.JournalApproveWorker();
                myWorker.SetLastActivate = () => { this.LastActivity = DateTime.UtcNow; };
                myWorker.LogDoneItemInMemoryAction = this.LogDoneItemInMemory;
                myWorker.WorkUntilQEmptyQueueDB(timeSpan:null, selectedQueue: JournalApproveService.K_AccountingConversionJournalApproveWR);



            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole" + this.GetType().Name, " : Run() Method", null);
                Thread.Sleep(TimeSpan.FromSeconds(5));
                _OnStartDone = false;
            }

        }
        public int? SeedTenant { get; set; }// worker role per tenant /Should inject from base 





    }
}
