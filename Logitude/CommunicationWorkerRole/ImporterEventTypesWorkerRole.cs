using CommunicationWorkerRole.Services.Logbox;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace CommunicationWorkerRole
{
    class ImporterEventTypesWorkerRole : WorkerEntryPoint
    {
        private DbQueueService queueService;
        private ImporterEventTypesService importerEventTypesService;
        private const string queueCode = "ImporterEventTypesQueue";
        private const int tenSecondsInMilliFormat = 10000;
        public ImporterEventTypesWorkerRole()
        {
        }

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "ImporterEventTypesWorkerRole";
            DoneItemsInRange = new Dictionary<DateTime, int>();
            return base.OnStart();
        }

        public override void Run()
        {
            while (IsRunning)
            {
                if(General.IsUpdating()) Thread.Sleep(tenSecondsInMilliFormat * 6);
                else QueueMessageRecieverListener();
            }
        }

        private void QueueMessageRecieverListener()
        {
            queueService = new DbQueueService();
            queueService.InitializeQueue(queueCode, 0);
            var response = queueService.Receive(new TimeSpan(0, 0, 10));
            LastActivity = DateTime.UtcNow;
            if (response != null && response.MessageId != null)
            {
                HandleQueueMessageResponse(response);
            }
            else
            {
                Thread.Sleep(tenSecondsInMilliFormat);
            }
        }

        private void HandleQueueMessageResponse(QueueResponse response)
        {
            try
            {
                ExecuteQueue(response);
                queueService.Complete();
            }
            catch (Exception exception)
            {
                HandleQueueMessageResponseException(response, exception);
            }
        }

        private void ExecuteQueue(QueueResponse response)
        {
            int tenant = int.Parse(response.MessageValues["Tenant"].ToString());
            string portId = response.MessageValues["EventTypeId"].ToString();
            importerEventTypesService = new ImporterEventTypesService(response);
            importerEventTypesService.Run(portId, tenant);
        }

        private void HandleQueueMessageResponseException(QueueResponse response, Exception exception)
        {
            ExceptionHandler.HandleException(exception, DateTime.Now, 0, "", "ImporterEventTypesWorkerRole", "", null);
            if (response.RetryNumber <= 1)
            {
                queueService.Delay(new TimeSpan(0, 0, 0, 5));
            }

            if (response.RetryNumber > 1 && response.RetryNumber <= 2)
            {
                queueService.Delay(new TimeSpan(0, 0, 0, 10));
            }

            if (response.RetryNumber >= 3)
            {
                queueService.CompleteAsFailed();
            }
        }
    }
}
