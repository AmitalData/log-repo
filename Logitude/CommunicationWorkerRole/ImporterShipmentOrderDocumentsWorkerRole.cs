using CommunicationWorkerRole.Services.ShipmentOrderDocuments;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CommunicationWorkerRole
{
    public class ImporterShipmentOrderDocumentsWorkerRole : WorkerEntryPoint
    {
        private DbQueueService queueService;
        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "ImporterShipmentOrderDocumentsWorkerRole";
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
                ExceptionHandler.HandleException(exception, DateTime.Now, 0, null, "Importer Shipment Order Document Queue worker role start", null, null);
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

            new ShipmentOrderDocumentsService(queueService, queueResponse).ExecuteQueue();
        }

        private void ConnectClient()
        {
            try
            {
                InitializeQueueService();

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Importer Shipment Order Document execution worker role start", null, null);
            }
        }

        private void InitializeQueueService()
        {
            queueService = new DbQueueService();
            queueService.InitializeQueue("ImporterShipmentOrderDocumentsQueue", 0);
        }
    }
}
