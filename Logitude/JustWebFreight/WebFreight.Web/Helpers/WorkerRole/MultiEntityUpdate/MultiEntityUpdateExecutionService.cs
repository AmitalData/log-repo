using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.QueueService;
using Simplog.Server.Infrastructure;
using System;

namespace WebFreight.Web.Helpers.WorkerRole.MultiEntityUpdate
{
    public class MultiEntityUpdateExecutionService
    {

        private readonly DbQueueService queueService = null;
        private readonly QueueResponse queueResponse = null;
        private readonly int? tenant;
        private readonly string multiEntityUpdateId = string.Empty;
        private MultiEntityUpdateLogPM multiEntityUpdateLogPM = null;
        private MultiEntityUpdateLogExecutionService multiEntityUpdateLogService = null;
        private MultiEntityUpdateGeneralService multiEntityUpdateGeneralService = null;
        private MultiEntityUpdateProcessService multiEntityUpdateProcessService = null;
        public MultiEntityUpdateExecutionService(DbQueueService queueService, QueueResponse queueResponse)
        {
            if (queueService == null || queueResponse == null) return;
            this.queueService = queueService;
            this.queueResponse = queueResponse;
            this.multiEntityUpdateId = queueResponse.MessageValues != null && queueResponse.MessageValues.Keys.Contains("MultiEntityUpdateId") ? queueResponse.MessageValues["MultiEntityUpdateId"].ToString() : "";
            this.tenant = GetTenantValueFromQueueResponse(queueResponse);
            Initiallize();
        }

        private int? GetTenantValueFromQueueResponse(QueueResponse queueResponse)
        {
            if (queueResponse.MessageValues == null) return null;
            if (!queueResponse.MessageValues.Keys.Contains("Tenant")) return null;

            string tenantString = queueResponse.MessageValues["Tenant"].ToString();
            if (string.IsNullOrEmpty(tenantString)) return null;

            return int.Parse(tenantString);
        }

        private void Initiallize()
        {
            multiEntityUpdateLogService = new MultiEntityUpdateLogExecutionService(multiEntityUpdateId, (int)tenant);
            multiEntityUpdateGeneralService = new MultiEntityUpdateGeneralService();
            multiEntityUpdateLogPM = multiEntityUpdateLogService.Get();
            multiEntityUpdateProcessService = new MultiEntityUpdateProcessService(multiEntityUpdateLogPM, multiEntityUpdateLogService, (int)tenant);
        }

        public void ExecuteMultiEntityUpdateQueue()
        {
            try
            {
                if (queueService == null || queueResponse == null || multiEntityUpdateLogPM == null) return;
                
                if (!multiEntityUpdateGeneralService.IsCompletedQueueService(multiEntityUpdateLogPM))
                    HandleMultiEntityUpdateExecution();

                queueService.Complete();
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        private void HandleMultiEntityUpdateExecution()
        {
            MarkMultiEntityUpdateLogAsInProgress();
            multiEntityUpdateProcessService.Update();
            MarkMultiEntityUpdateLogAsDone();
        }

        private void MarkMultiEntityUpdateLogAsInProgress()
        {
            multiEntityUpdateLogService.Update(new MultiEntityUpdateLogArgs()
            {
                StatusCode = "P",
                UpdatedEntitiesNumber = 0,
                StartDate = DateTime.UtcNow
            });
        }

        private void MarkMultiEntityUpdateLogAsDone()
        {
            multiEntityUpdateLogService.Update(new MultiEntityUpdateLogArgs()
            {
                StatusCode = "D",
                UpdatedEntitiesNumber = multiEntityUpdateLogPM.UpdatedEntitiesNumber,
                DoneDate = DateTime.UtcNow,
                XMLData = LogitudeXmlSerializer.SerializeObjectToXmlString(multiEntityUpdateProcessService.multiEntityUpdateData),
                ExceptionMessage = ""
            });
        }

        private void HandleException(Exception exception)
        {
            DatabaseInitializer.RunOnSeconderyDB = false;
            MarkMultiEntityUpdateLogAsFailed(exception);
        }

        private void MarkMultiEntityUpdateLogAsFailed(Exception exception)
        {
            multiEntityUpdateLogService.Update(new MultiEntityUpdateLogArgs()
            {
                StatusCode = "F",
                UpdatedEntitiesNumber = multiEntityUpdateLogPM.UpdatedEntitiesNumber,
                DoneDate = DateTime.UtcNow,
                XMLData = LogitudeXmlSerializer.SerializeObjectToXmlString(multiEntityUpdateProcessService.modifiedMultiEntityUpdateDataEntities),
                ExceptionMessage = multiEntityUpdateGeneralService.GetFullExceptionMessageFromException(exception)
            });
        }
    }
}