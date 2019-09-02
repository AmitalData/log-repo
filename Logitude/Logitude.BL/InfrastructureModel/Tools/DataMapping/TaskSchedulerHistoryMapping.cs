using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class TaskSchedulerHistoryMapping
    {
        public static void MapEntity(TaskSchedulerHistoryPM TaskSchedulerHistoryPM, TaskSchedulerHistory TaskSchedulerHistory, bool isNewState)
        {
            if (isNewState)
            {
                TaskSchedulerHistory.Id = TaskSchedulerHistoryPM.Id; 
                TaskSchedulerHistory.Tenant = TaskSchedulerHistoryPM.Tenant;
                TaskSchedulerHistory.StartDateTime = TenantServerConfigration.GetCurrentDateTime(TaskSchedulerHistory.Tenant); 
                TaskSchedulerHistory.StartDateTimeUTC = DateTime.UtcNow;

            }
            else
            {
                TaskSchedulerHistory.StartDateTime = TaskSchedulerHistoryPM.StartDateTime;
                TaskSchedulerHistory.StartDateTimeUTC = TaskSchedulerHistoryPM.StartDateTimeUTC;
            }
            TaskSchedulerHistory.EndDateTime = TaskSchedulerHistoryPM.EndDateTime;
            TaskSchedulerHistory.EndDateTimeUTC = TaskSchedulerHistoryPM.EndDateTimeUTC;
            TaskSchedulerHistory.IsError = TaskSchedulerHistoryPM.IsError;
            TaskSchedulerHistory.RunResult = TaskSchedulerHistoryPM.RunResult;
            TaskSchedulerHistory.TaskId = TaskSchedulerHistoryPM.TaskId;
            TaskSchedulerHistory.LogFirstLine = TaskSchedulerHistoryPM.LogFirstLine;
            TaskSchedulerHistory.LogType = TaskSchedulerHistoryPM.LogType;

            TaskSchedulerHistory.LogDocumentId = TaskSchedulerHistoryPM.LogDocumentId;
        }
    }
}