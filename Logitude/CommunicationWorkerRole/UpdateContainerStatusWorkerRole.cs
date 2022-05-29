using CommunicationWorkerRole.Services.ContainerTraking;
using Logitude.BL.ShipmentsModel.CloseTables;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using WebFreight.Web.ContainerTracking;

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
        //private void StartWork()
        //{
        //    if (General.IsUpdating())
        //    {
        //        Thread.Sleep(60000);
        //        return;
        //    }

        //    try
        //    {
        //        ExecuteQueue();
        //    }
        //    catch (Exception exception)
        //    {
        //        ExceptionHandler.HandleException(exception, DateTime.Now, 0, null, "General Update Container Status start fail", null, null);
        //        Thread.Sleep(10000);
        //    }
        //}

        //private void ExecuteQueue()
        //{
        //    AnalyzeQueueRepository analyzeQueueRepository = new AnalyzeQueueRepository();
        //    AnalyzeQueue analyzeQueue = analyzeQueueRepository.GetOpenAnalyzeQueue("GeneralContainerTrackingReceiver");
        //    LastActivity = DateTime.UtcNow;

        //    if (analyzeQueue != null)
        //    {
        //        ContainerTrackingGeneralAnalyzer analyzer = new ContainerTrackingGeneralAnalyzer(ContainerStatusSourceValues.Vizion, analyzeQueue, analyzeQueueRepository);
        //        analyzer.Run();
        //        LogDoneItemInMemory();
        //    }
        //    else
        //    {
        //        Thread.Sleep(3000);
        //    }
        //}

    }
}
