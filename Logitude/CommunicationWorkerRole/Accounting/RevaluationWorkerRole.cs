
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
//using CustomsWorkerRole.Queue;
using UnifreightIIG.Common.Utils;
using Logitude.Server.Tools;
//using Logitude.CustomsMessaging.MessagingServices;
using Microsoft.Practices.Unity;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Customs.BL.Messaging.Customs;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
//using CustomsWorkerRole.Utils;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Accounting.BL.Utils;
using Logitude.Server.Tools.QueueService;

namespace CommunicationWorkerRole// DUE LOADER ///.Accounting
{
    public class RevaluationWorkerRole : WorkerEntryPoint
    {
        bool _OnStartDone = false;
        //QueueDescription _QueueDescription;
        //QueueClient _QueueClient;
        //private QueueClient _DeadletterQueueClient;


        DateTime _LastGC = DateTime.MinValue;
        private DbQueueService _DbQueueService;

        public const string K_RevaluationWorkerRole = "RevaluationWorkerRole";
        public const string QP_Tenant = "Tenant";
        public const string QP_JournalId = "JournalId";


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
                    ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "AccountingJournalApproveWR : Run() Method", null);
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


               

                _DbQueueService = new DbQueueService(RevaluationWorkerRole.K_RevaluationWorkerRole, 0);


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
                var myWorker = new RevaluationBatch.RevaluationWorkerRole();
                myWorker.WorkUntilQEmptyQueueDB();


            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole" + this.GetType().Name, " : Run() Method", null);
                Thread.Sleep(TimeSpan.FromSeconds(5));
                _OnStartDone = false;
            }

        }

    

      

    }
}
