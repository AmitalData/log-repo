using CommunicationWorkerRole.Services.ImporterShipmentOrders;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CommunicationWorkerRole
{
    public class ImporterShipmentOrdersWorkerRole : WorkerEntryPoint
    {
        private DbQueueService queueService;
        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "ImporterShipmentOrdersWorkerRole";
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
                ExceptionHandler.HandleException(exception, DateTime.Now, 0, null, "Importer Shipment Order Queue worker role start", null, null);
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

            new ImporterShipmentOrdersService(queueService, queueResponse).ExecuteQueue();
        }

        private void ConnectClient()
        {
            try
            {
                InitializeQueueService();

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Importer Shipment Order execution worker role start", null, null);
            }
        }

        private void InitializeQueueService()
        {
            queueService = new DbQueueService();
            queueService.InitializeQueue("ImporterShipmentOrderQueue", 0);
        }
    }
}
