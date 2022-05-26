using CommunicationWorkerRole.Services.ContainerTraking;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CommunicationWorkerRole
{
    public class UpdateContainerStatusWorkerRole : WorkerEntryPoint
    {
        private DbQueueService queueService;
        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "UpdateContainerStatusWorkerRole";
            DoneItemsInRange = new Dictionary<DateTime, int>();
            ConnectClient();
            return base.OnStart();
        }

        public override void Run()
        {
            while (IsRunning) StartWork();
        }

        private void StartWork()
        {
            if (General.IsUpdating())
            {
                Thread.Sleep(60000);
                return;
            }

            try
            {
                InitializeQueueService();
                ExecuteQueue();
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception, DateTime.Now, 0, null, "General Update Container Status start fail", null, null);
                Thread.Sleep(10000);
            }
        }

        private void ExecuteQueue()
        {
            var queueResponse = queueService.Receive();
            if (queueResponse == null || queueResponse.MessageId == null)
            {
                return;
            }
            try
            {
                new ContainerTrackingWRService(queueService, queueResponse).ExecuteQueue();
                queueService.Complete();
            }
            catch (Exception)
            {
                queueService.CompleteAsFailed();
                throw;
            }
        }

        private void ConnectClient()
        {
            try
            {
                InitializeQueueService();
                queueService.Complete();

            }
            catch (Exception ex)
            {
                queueService.CompleteAsFailed();

                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "General Update Container Status start fail", null, null);
            }
        }

        private void InitializeQueueService()
        {
            queueService = new DbQueueService();
            queueService.InitializeQueue("GeneralUpdateContainerStatus", 0);
        }
    }
}
