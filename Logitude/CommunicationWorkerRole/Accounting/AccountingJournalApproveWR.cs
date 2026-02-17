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
    public class AccountingJournalApproveWR : WorkerEntryPoint
    {
        bool _OnStartDone = false;
        int _SleepM = 100;
        private QueueClient _QueueClient;
        //private QueueClient _DeadletterQueueClient;
        DateTime _LastGC = DateTime.MinValue;
        private bool _UseQueue = true;
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

                
                if (!_UseQueue)
                {
                    return true;
                }

                _DbQueueService = new DbQueueService(JournalApproveService.K_AccountingJournalApproveWR, 0);


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
                if (_UseQueue)
                {
                    var myWorker = new JournalApproveService.JournalApproveWorker();
                    myWorker.SetLastActivate = () => { this.LastActivity = DateTime.UtcNow; };
                    myWorker.LogDoneItemInMemoryAction = this.LogDoneItemInMemory;
                    myWorker.WorkUntilQEmptyQueueDB();    
                }
                else
                {
                    WorkNonStopWithoutQueue();
                }


            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole" + this.GetType().Name, " : Run() Method", null);
                Thread.Sleep(TimeSpan.FromSeconds(5));
                _OnStartDone = false;
            }

        }
        public int? SeedTenant { get; set; }// worker role per tenant /Should inject from base 
        
        private void WorkNonStopWithoutQueue()
        {
            var sw = new Stopwatch();
            List<string> Last_journalBufferKeys = null;
            while (IsRunning)
            {
                if (General.IsUpdating())
                {
                    Thread.Sleep(1000);
                    continue;
                }
                JournalApproveService.WorkWithoutQueue(this.SeedTenant.Value,null, ref Last_journalBufferKeys);


            }//while (IsRunning)

        }
#if false
        void WorkUntilQEmptyQueue()
        {
            BrokeredMessage receivedMessage = null;
            List<long> deferredSequenceNumbers = new List<long>();
            bool proccesDone = false;
            for (int filtterPriority = 2; filtterPriority < 3; filtterPriority++)
            {
                while (IsRunning)
                {
                    //throw new Exception("BrokeredMessage receivedMessage = _QueueClient.Receive(TimeSpan.FromSeconds(5));");
                    try
                    {
                        receivedMessage = _QueueClient.Receive(TimeSpan.FromSeconds(5));
                    }
                    catch (Exception)
                    {

                        throw;
                    }


                    if (receivedMessage == null)
                    {
                        break;
                    }




                    proccesDone = true;
                    ProcessMessage(receivedMessage);
                }
            }




        }

        private void ProcessMessage(BrokeredMessage message)
        {

            try
            {



                var tenant = message.GetProperty<int>(QueueExt.QueuePropertyNames.Tenant, -1);
                if (tenant == -1)
                {
                    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "AccountingJournalApproveWR: ProcessMessage() Method :tenant==-1", null);
                    return;
                }
                var correlationId = message.CorrelationId;
                LogMessagingUtil.Instance.AppendLine("receivedMessage.DeliveryCount =" + message.DeliveryCount.ToString());
                JournalApproveService.MyActions actions =
                JournalApproveService.MyActions.BuildLedgerTransaction | JournalApproveService.MyActions.BuildGLAccountTotalByMonths;
                var myJournalApproveService = new JournalApproveService(tenant, correlationId);
                var res = myJournalApproveService.SubmitApprove(actions);
                if (res.Success)
                {

                    message.SafeComplete();
                }
                else
                {
                    var ex1 = new Exception("AccountingJournalApproveWR.JournalApproveService(" + tenant.ToString() + "," + correlationId.ToString() + ").SubmitApprove():FailDue=" + res.FailDue);
                    ExceptionHandler.HandleException(ex1, DateTime.Now, 0, "", "WorkerRole", "AccountingJournalApproveWR: ProcessMessage() Method", null);
                }

            }
            catch (CustomsRequestsSheetDomainModelServiceException customsRequestsSheetServiceException)
            {

                if (customsRequestsSheetServiceException.What2Do == CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueue)
                {
                    message.SafeComplete();
                }
                else
                {
                    message.SetProperty<DateTime>(QueueExt.QueuePropertyNames.LastExecAt, DateTime.UtcNow);
                    message.SafeAbandon();
                }

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", "AccountingJournalApproveWR: ProcessMessage() Method", null);
                message.SafeComplete();
                //throw;
            }
        }

        
#endif


    }
}
