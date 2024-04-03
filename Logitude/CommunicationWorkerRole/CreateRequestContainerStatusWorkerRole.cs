using CommunicationWorkerRole.Services.ContainerTraking;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CommunicationWorkerRole
{
    public class CreateRequestContainerStatusWorkerRole : WorkerEntryPoint
    {
        private DbQueueService queueService;
        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "CreateRequestContainerStatusWR";
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
                queueService.Complete();
            }
            catch (Exception exception)
            {
                queueService.CompleteAsFailed();
                ExceptionHandler.HandleException(exception, DateTime.Now, 0, null, "General Request Update Container Status start fail", null, null);
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

            new RequestContainerStatusService(queueService, queueResponse).ExecuteQueue();

        }

        private void ConnectClient()
        {
            try
            {
                InitializeQueueService();

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "General Request Update Container Status start fail", null, null);
            }
        }

        private void InitializeQueueService()
        {
            queueService = new DbQueueService();
            queueService.InitializeQueue("GeneralRequestUpdateContainerStatus", 0);
        }
    }
}
