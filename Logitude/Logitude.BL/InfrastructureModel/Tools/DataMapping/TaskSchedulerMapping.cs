using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class TasksSchedulerMapping
    {
        public static void MapEntity(TasksSchedulerPM TaskSchedulerPM, TasksScheduler TaskScheduler, bool isNewState)
        {
            if (isNewState)
            {
                TaskScheduler.Id = TaskSchedulerPM.Id;
                TaskScheduler.Tenant = TaskSchedulerPM.Tenant;
                
            }
  
            TaskScheduler.CreateDateTime = TaskSchedulerPM.CreateDateTime;
            TaskScheduler.CreatedBy = TaskSchedulerPM.CreatedBy;
            TaskScheduler.Description = TaskSchedulerPM.Description;
            TaskScheduler.Friday = TaskSchedulerPM.Friday;
            TaskScheduler.InActive = TaskSchedulerPM.InActive;
            TaskScheduler.IsLastRunError = TaskSchedulerPM.IsLastRunError;
            TaskScheduler.LastRunResult = TaskSchedulerPM.LastRunResult;
            TaskScheduler.LastRunTime = TaskSchedulerPM.LastRunTime;
            TaskScheduler.Monday = TaskSchedulerPM.Monday;
            TaskScheduler.Name = TaskSchedulerPM.Name;
            TaskScheduler.NextRunTime = TaskSchedulerPM.NextRunTime;
            TaskScheduler.RepeatInMinutes = TaskSchedulerPM.RepeatInMinutes;
            TaskScheduler.Satarday = TaskSchedulerPM.Satarday;
            TaskScheduler.ServiceClassName = TaskSchedulerPM.ServiceClassName;
            TaskScheduler.StartDateTime = TaskSchedulerPM.StartDateTime;
            TaskScheduler.Sunday = TaskSchedulerPM.Sunday;
            TaskScheduler.Thursday = TaskSchedulerPM.Thursday;
            TaskScheduler.TriggerType = TaskSchedulerPM.TriggerType;
            TaskScheduler.Tuesday = TaskSchedulerPM.Tuesday;
            TaskScheduler.UpdateDateTime = TaskSchedulerPM.UpdateDateTime;
            TaskScheduler.UpdatedBy = TaskSchedulerPM.UpdatedBy;
            TaskScheduler.Wednesday = TaskSchedulerPM.Wednesday;
            TaskScheduler.Type = TaskSchedulerPM.Type;
            TaskScheduler.SchedulerDetailsXML = TaskSchedulerPM.SchedulerDetailsXML;

            TaskScheduler.StartDateTimeUTC = TaskSchedulerPM.StartDateTimeUTC;
            TaskScheduler.LastRunTimeUTC = TaskSchedulerPM.LastRunTimeUTC;
            TaskScheduler.NextRunTimeUTC = TaskSchedulerPM.NextRunTimeUTC;

            TaskScheduler.Version = TaskSchedulerPM.Version;
            TaskScheduler.Status = TaskSchedulerPM.Status;
            TaskScheduler.Retries = TaskSchedulerPM.Retries;

        }
    }
}