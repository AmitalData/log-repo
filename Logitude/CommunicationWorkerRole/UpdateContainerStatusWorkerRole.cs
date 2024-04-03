using CommunicationWorkerRole.Services.ContainerTraking;
using Logitude.BL.ShipmentsModel.CloseTables;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading;
using WebFreight.Web.ContainerTracking;

namespace CommunicationWorkerRole
{
    public class UpdateContainerStatusWorkerRole : WorkerEntryPoint
    {
        private DbQueueService queueService;
        private AnalyzeQueueRepository analyzeQueueRepository;
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
            if (General.IsUpdating() || LogitudeSettings.WorkerRoleName.ToLower() == "staging")
            {
                Thread.Sleep(60000);
                return;
            }

            try
            {
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
            analyzeQueueRepository = new AnalyzeQueueRepository();
            AnalyzeQueue analyzeQueue = analyzeQueueRepository.GetOpenAnalyzeQueue("GeneralContainerTrackingReceiver");
            LastActivity = DateTime.UtcNow;

            if (analyzeQueue != null)
            {
                SetAnalyzeQueueStatusToInProgress(analyzeQueue);
                ContainerTrackingGeneralAnalyzer analyzer = new ContainerTrackingGeneralAnalyzer(ContainerStatusSourceValues.Vizion, analyzeQueue, analyzeQueueRepository);
                analyzer.Run();
                LogDoneItemInMemory();
            }
            else
            {
                Thread.Sleep(3000);
            }
        }

        private void SetAnalyzeQueueStatusToInProgress(AnalyzeQueue analyzeQueue)
        {
            string inProgressStatusCode = "I";
            analyzeQueue.Status = inProgressStatusCode;
            analyzeQueueRepository.Update(analyzeQueue);
            analyzeQueueRepository.SubmitChanges();
        }
    }
}
