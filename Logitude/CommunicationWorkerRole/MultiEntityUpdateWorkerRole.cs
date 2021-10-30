using CommunicationWorkerRole.Services;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;

namespace CommunicationWorkerRole
{
    public class MultiEntityUpdateWorkerRole : WorkerEntryPoint
    {
        DbQueueService queueService;
        public override void Run()
        {
            while (IsRunning) StartWork();
        }

        public void StartWork()
        {
            if (General.IsUpdating()) Thread.Sleep(60000);
            try
            {
                InitializeQueueService();
                QueueResponse queueResponse = queueService.Receive(new TimeSpan(0, 0, 0, 10));

                if (queueResponse.MessageId != null) GetStartedInQueueMessage(queueResponse);
            }
            catch (Exception ex)
            {
                ConnectClient();
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "MultiEntityUpdate worker role start", null, null);
                Thread.Sleep(10000);
            }
        }

        private void InitializeQueueService()
        {
            queueService = new DbQueueService();
            queueService.InitializeQueue("MultiEntityUpdateQueue", 0);
        }

        private void GetStartedInQueueMessage(QueueResponse queueResponse)
        {
            ThreadStart multiEntityUpdateServiceThreadStart = (() => new MultiEntityUpdateService(queueService, queueResponse).ExecuteMultiEntityUpdateQueue());
            multiEntityUpdateServiceThreadStart += () => { LogDoneItemInMemory(); };
            new Thread(multiEntityUpdateServiceThreadStart) { IsBackground = true }.Start();
            queueService.Complete();
        }

        public void ConnectClient()
        {
            try
            {
                InitializeQueueService();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Connect client method", null, null);
            }
        }

        public override bool OnStart()
        {
            if (!string.IsNullOrEmpty(ThreadId)) 
            {
                return true; 
            }
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "MultiEntityUpdateWorkerRole";
            DoneItemsInRange = new Dictionary<DateTime, int>();
            try
            {
                InitializeQueueService();
            }
            catch (Exception ex)
            {
                HandleExceptionOnStart(ex);
            }
            return base.OnStart();
        }

        private static void HandleExceptionOnStart(Exception ex)
        {
            string ip = "";
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                ip = HttpContext.Current.Request.UserHostAddress;
            }
            ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "MultiEntityUpdateWorkerRole Role", null, ip);
        }
    }

    public class MultiEntityUpdateWorkerRoleWinService : Logitude.Server.Tools.WorkerEntryPointDoneLog
    {
        MultiEntityUpdateWorkerRole MultiEntityUpdateWorkerRole;
        public MultiEntityUpdateWorkerRoleWinService()
        {
            MultiEntityUpdateWorkerRole = new MultiEntityUpdateWorkerRole();
        }

        public override void StartMe()
        {
            throw new NotImplementedException();
        }

        public override void WorkOnce()
        {
            MultiEntityUpdateWorkerRole.OnStart();
            MultiEntityUpdateWorkerRole.StartWork();
        }
    }
}