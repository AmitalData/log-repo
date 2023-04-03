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
            ExceptionHandler.HandleException(new Exception("Document Execution V2 Worker role started"), DateTime.Now, 0, null, "Doc WorkerRole Monitor" + "|" + ThreadedRoleEntryPoint.getWorkerRoleName(), null, System.Environment.MachineName);
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
                GC.Collect();
                queueService.Complete();
            }
            catch (Exception exception)
            {
                HandleExceptionRetries(queueResponse, exception);
            }
        }

        private void HandleExceptionRetries(QueueResponse response, Exception exception)
        {
            if (IsExceededTimeOut(exception))
            {
                queueService.CompleteAsFailed();
            }
            else if (response.RetryNumber <= 1)
            {
                queueService.Delay(new TimeSpan(0, 0, 0, 5));
            }
            else if (response.RetryNumber >= 2)
            {
                queueService.CompleteAsFailed();
            }
            ExceptionHandler.HandleException(exception, DateTime.Now, 0, null, "Document Execution V2 WorkerRole Monitor|" + "Catch ExecuteDocumentsExecutionQueue", null, System.Environment.MachineName);
            Thread.Sleep(new TimeSpan(0, 0, 0, 0, 250));
        }

        private bool IsExceededTimeOut(Exception exception)
        {
            return !string.IsNullOrEmpty(exception.Message) && exception.Message.Contains("Document build failed since it reached the time out.");
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
