using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using System;
using System.Collections.Generic;
using System.Threading;
using WebFreight.Web.Helpers.WorkerRole.DocsOut;

namespace CommunicationWorkerRole
{
    class DocumentsExecutionV2WorkerRole : WorkerEntryPoint
    {
        private DbQueueService queueService;
        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "DocumentExecutionV2WorkerRole";
            DoneItemsInRange = new Dictionary<DateTime, int>();
            ConnectClient();
            return base.OnStart();
        }

        public override void Run()
        {
            ExceptionHandler.HandleException(new Exception("Document execution version 2 Worker role thread start running"), DateTime.Now, 0, null, "Document execution version 2 WorkerRole Monitor" + "|" + ThreadedRoleEntryPoint.getWorkerRoleName(), null, System.Environment.MachineName);
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                       ExecuteQueue();
                    }
                    catch (Exception exception)
                    {
                        ExceptionHandler.HandleException(exception, DateTime.Now, 0, null, "Document execution version 2 WorkerRole Monitor|Main thread", null, System.Environment.MachineName);                 
                        Thread.Sleep(new TimeSpan(0, 0, 0, 0, 250));
                    }
                }
                else Thread.Sleep(new TimeSpan(0, 0, 0, 0, 250));
            }
        }

        private void ExecuteQueue()
        {
            queueService = new DbQueueService("DocumentsExecutionV2Queue", 0);
            var queueResponse = queueService.Receive(new TimeSpan(0, 0, 0, 0, 250));
            if (queueResponse == null || queueResponse.MessageId == null) return;
            try
            {
                new DocumentsExecutionService(queueService, queueResponse).ExecuteDocumentsV2ExecutionQueue();
                LogDoneItemInMemory();
                queueService.Complete();
            }
            catch (Exception exception)
            {
                HandleExceptionRetries(queueResponse, exception);
            }
        }

        private void HandleExceptionRetries(QueueResponse response, Exception insideException)
        {
            bool isBuildDocumentFailed = !string.IsNullOrEmpty(insideException.Message) && insideException.Message.Contains("Document build failed after 3 retries or it reaches the time out");
            if (response.RetryNumber <= 1 && !isBuildDocumentFailed)
            {
                queueService.Delay(new TimeSpan(0, 0, 0, 5));
            }
            if (response.RetryNumber >= 2 || isBuildDocumentFailed)
            {
                queueService.CompleteAsFailed();
            }
            ExceptionHandler.HandleException(insideException, DateTime.Now, 0, null, "Document Execution V2 WorkerRole Monitor|" + "Catch ExecuteDocumentsExecutionQueue", null, System.Environment.MachineName);
            Thread.Sleep(new TimeSpan(0, 0, 0, 0, 250));
        }

        private void ConnectClient()
        {
            try
            {
                queueService = new DbQueueService();
                queueService.InitializeQueue("DocumentsExecutionV2Queue", 0);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Document execution version 2 worker role start", null, null);
            }
        }
    }
}
