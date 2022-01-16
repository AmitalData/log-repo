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
                Thread.Sleep(new TimeSpan(0, 0, 0, 0, 250));
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
                Thread.Sleep(new TimeSpan(0, 0, 0, 0, 250));
            }
        }

        private void ExecuteQueue()
        {
            var queueResponse = queueService.Receive(new TimeSpan(0, 0, 0, 0, 10));
            if (queueResponse == null || queueResponse.MessageId == null)
            {
                return;
            }

            ThreadStart executeDocumentsThreadStart = () => new ImporterShipmentOrdersService(queueService, queueResponse).ExecuteQueue();
            executeDocumentsThreadStart += () => { LogDoneItemInMemory(); };
            new Thread(executeDocumentsThreadStart) { IsBackground = true }.Start();
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
