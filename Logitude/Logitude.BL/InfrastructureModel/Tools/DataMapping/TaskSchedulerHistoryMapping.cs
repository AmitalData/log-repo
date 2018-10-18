using Logitude.BL.InfrastructureModel.EntityPMs;
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
                TaskSchedulerHistory.StartDateTime = DateTime.Now;
            }
            else
            {
                TaskSchedulerHistory.StartDateTime = TaskSchedulerHistoryPM.StartDateTime;
            }
            TaskSchedulerHistory.EndDateTime = TaskSchedulerHistoryPM.EndDateTime;
            TaskSchedulerHistory.IsError = TaskSchedulerHistoryPM.IsError;
            TaskSchedulerHistory.RunResult = TaskSchedulerHistoryPM.RunResult;
            TaskSchedulerHistory.TaskId = TaskSchedulerHistoryPM.TaskId; 
        }
    }
}