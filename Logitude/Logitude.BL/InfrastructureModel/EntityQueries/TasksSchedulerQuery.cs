using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using System.Data.Entity;
using Simplog.Server.Infrastructure;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class TasksSchedulerQuery
    {
        TasksSchedulerRepository repository;
        public TasksSchedulerQuery()
        {
            repository = new TasksSchedulerRepository();
        }

        public TasksSchedulerQuery(int tenant)
        {
            repository = new TasksSchedulerRepository(tenant);
        }

        public string GetSchedulerDetailsXmalById(string schedulerId, int tenant)
        {

            string result = (from a in repository.context.TasksSchedulers
                             where a.Tenant == tenant && a.Id == schedulerId
                             select a.SchedulerDetailsXML).FirstOrDefault();

            return result;

        }

        public TasksSchedulerQuery(TasksSchedulerRepository TasksSchedulerRepository)
        {
            repository = TasksSchedulerRepository;
        }


        public TasksSchedulerPM GetSingleTasksSchedulerPM(string id)
        {
            return (from a in repository.context.TasksSchedulers
                    where a.Id == id
                    select new TasksSchedulerPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        CreateDateTime = a.CreateDateTime,
                        CreatedBy = a.CreatedBy,
                        Description = a.Description,
                        Friday = a.Friday,
                        InActive = a.InActive,
                        IsLastRunError = a.IsLastRunError,
                        LastRunResult = a.LastRunResult,
                        LastRunStartTime = a.LastRunStartTime,
                        Monday = a.Monday,
                        Name = a.Name,
                        NextRunTime = a.NextRunTime,
                        RepeatInMinutes = a.RepeatInMinutes,
                        Satarday = a.Satarday,
                        ProcedureCode = a.ProcedureCode,
                        StartDateTime = a.StartDateTime,
                        Sunday = a.Sunday,
                        Thursday = a.Thursday,
                        TriggerType = a.TriggerType,
                        Tuesday = a.Tuesday,
                        UpdateDateTime = a.UpdateDateTime,
                        UpdatedBy = a.UpdatedBy,
                        Wednesday = a.Wednesday,
                        Type = a.Type,
                        SchedulerDetailsXML = a.SchedulerDetailsXML,
                        NextRunTimeUTC = a.NextRunTimeUTC,
                        StartDateTimeUTC = a.StartDateTimeUTC,
                        LastRunStartTimeUTC = a.LastRunStartTimeUTC,
                        Version = a.Version,
                        Status = a.Status,
                        Retries = a.Retries,
                        LastRunEndTime = a.LastRunEndTime,
                        LastRunEndTimeUTC = a.LastRunEndTimeUTC,
                        AverageRunTime = a.AverageRunTime, 
                        Duration = a.AverageRunTime,
                        EntityId = a.EntityId

                    }).FirstOrDefault();
        }

        public TasksSchedulerPM GetSingleTasksSchedulerPMByTenant(string Id, int Tenant)
        {
            return (from a in repository.context.TasksSchedulers
                    where a.Id == Id && a.Tenant == Tenant
                    select new TasksSchedulerPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        CreateDateTime = a.CreateDateTime,
                        CreatedBy = a.CreatedBy,
                        Description = a.Description,
                        Friday = a.Friday,
                        InActive = a.InActive,
                        IsLastRunError = a.IsLastRunError,
                        LastRunResult = a.LastRunResult,
                        LastRunStartTime = a.LastRunStartTime,
                        Monday = a.Monday,
                        Name = a.Name,
                        NextRunTime = a.NextRunTime,
                        RepeatInMinutes = a.RepeatInMinutes,
                        Satarday = a.Satarday,
                        ProcedureCode = a.ProcedureCode,
                        StartDateTime = a.StartDateTime,
                        Sunday = a.Sunday,
                        Thursday = a.Thursday,
                        TriggerType = a.TriggerType,
                        Tuesday = a.Tuesday,
                        UpdateDateTime = a.UpdateDateTime,
                        UpdatedBy = a.UpdatedBy,
                        Wednesday = a.Wednesday,
                        Type = a.Type,
                        SchedulerDetailsXML = a.SchedulerDetailsXML,
                        NextRunTimeUTC = a.NextRunTimeUTC,
                        StartDateTimeUTC = a.StartDateTimeUTC,
                        LastRunStartTimeUTC = a.LastRunStartTimeUTC,
                        Version = a.Version,
                        Status = a.Status,
                        Retries = a.Retries,
                        LastRunEndTime = a.LastRunEndTime,
                        LastRunEndTimeUTC = a.LastRunEndTimeUTC,
                        AverageRunTime = a.AverageRunTime,
                        Duration = a.AverageRunTime,
                        EntityId = a.EntityId
                    }).FirstOrDefault();
        }


        public List<TasksSchedulerPM> GetTasksSchedulerPMs(int Tenant)
        {
            return (from a in repository.context.TasksSchedulers
                    where a.Tenant == Tenant
                    select new TasksSchedulerPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        CreateDateTime = a.CreateDateTime,
                        CreatedBy = a.CreatedBy,
                        Description = a.Description,
                        Friday = a.Friday,
                        InActive = a.InActive,
                        IsLastRunError = a.IsLastRunError,
                        LastRunResult = a.LastRunResult,
                        LastRunStartTime = a.LastRunStartTime,
                        Monday = a.Monday,
                        Name = a.Name,
                        NextRunTime = a.NextRunTime,
                        RepeatInMinutes = a.RepeatInMinutes,
                        Satarday = a.Satarday,
                        ProcedureCode = a.ProcedureCode,
                        StartDateTime = a.StartDateTime,
                        Sunday = a.Sunday,
                        Thursday = a.Thursday,
                        TriggerType = a.TriggerType,
                        Tuesday = a.Tuesday,
                        UpdateDateTime = a.UpdateDateTime,
                        UpdatedBy = a.UpdatedBy,
                        Wednesday = a.Wednesday,
                        Type = a.Type,
                        NextRunTimeUTC = a.NextRunTimeUTC,
                        StartDateTimeUTC = a.StartDateTimeUTC,
                        LastRunStartTimeUTC = a.LastRunStartTimeUTC,
                        Version = a.Version,
                        Status = a.Status,
                        Retries = a.Retries,
                        LastRunEndTime = a.LastRunEndTime,
                        LastRunEndTimeUTC = a.LastRunEndTimeUTC,
                        AverageRunTime = a.AverageRunTime,
                        Duration = a.AverageRunTime,
                        SchedulerDetailsXML = a.SchedulerDetailsXML,
                        EntityId = a.EntityId
                    }).ToList();
        }

        public List<TasksSchedulerPM> GetTasksSchedulerPMsBByType(string type, int Tenant)
        {
            var Tasks = (from a in repository.context.TasksSchedulers
                         where a.Tenant == Tenant && a.Type == type
                         select new TasksSchedulerPM()
                         {
                             Id = a.Id,
                             Tenant = a.Tenant,
                             CreateDateTime = a.CreateDateTime,
                             CreatedBy = a.CreatedBy,
                             Description = a.Description,
                             Friday = a.Friday,
                             InActive = a.InActive,
                             IsLastRunError = a.IsLastRunError,
                             LastRunResult = a.LastRunResult,
                             LastRunStartTime = a.LastRunStartTime,
                             Monday = a.Monday,
                             Name = a.Name,
                             NextRunTime = a.NextRunTime,
                             RepeatInMinutes = a.RepeatInMinutes,
                             Satarday = a.Satarday,
                             ProcedureCode = a.ProcedureCode,
                             StartDateTime = a.StartDateTime,
                             Sunday = a.Sunday,
                             Thursday = a.Thursday,
                             TriggerType = a.TriggerType,
                             Tuesday = a.Tuesday,
                             UpdateDateTime = a.UpdateDateTime,
                             UpdatedBy = a.UpdatedBy,
                             Wednesday = a.Wednesday,
                             Type = a.Type,
                             NextRunTimeUTC = a.NextRunTimeUTC,
                             StartDateTimeUTC = a.StartDateTimeUTC,
                             LastRunStartTimeUTC = a.LastRunStartTimeUTC,
                             Version = a.Version,
                             Status = a.Status,
                             Retries = a.Retries,
                             LastRunEndTime = a.LastRunEndTime,
                             LastRunEndTimeUTC = a.LastRunEndTimeUTC,
                             AverageRunTime = a.AverageRunTime,
                             Duration = a.AverageRunTime,
                             SchedulerDetailsXML = a.SchedulerDetailsXML,
                             EntityId = a.EntityId
                         }).ToList().OrderByDescending(x => x.CreateDateTime);

            //foreach (var Task in Tasks)
            //{
            //    Task.Duration = GetTaskAvarageDuration(Task.Id);
            //}

            return Tasks.ToList();
        }

        private double GetTaskAvarageDuration(string taskId)
        {
            double? Latest10HistoriesQuery = null;
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                var q = (from a in repository.context.TaskSchedulerHistories
                         where a.TaskId == taskId
                         where a.StartDateTime != null
                         where a.EndDateTime != null
                         orderby a.EndDateTimeUTC descending

                         select new //TaskSchedulerHistoryPM()
                         {
                             StartDateTime = a.StartDateTime.Value,
                             EndDateTime = a.EndDateTime.Value,

                         }).Take(10)
                         ;
                var l10 = q.ToList();
                if (l10.Count() > 0)
                {
                    Latest10HistoriesQuery = l10.Average(r => r.EndDateTime.Subtract(r.StartDateTime).TotalSeconds);
                }
            }
            else
            {
                Latest10HistoriesQuery = (from a in repository.context.TaskSchedulerHistories
                 where a.TaskId == taskId
                 select new TaskSchedulerHistoryPM()
                 {
                     StartDateTime = a.StartDateTime,
                     EndDateTime = a.EndDateTime,
                     Duration = DbFunctions.DiffSeconds(a.EndDateTime, a.StartDateTime),
                 }).Where(x => x.StartDateTime != null && x.EndDateTime != null).Average(a => a.Duration);//.ToList();.OrderByDescending(x => x.StartDateTime).Take(10)
                                                                                                          //var Latest10Histories = Latest10HistoriesQuery.ToList();
            }


            double? Duration = 0.0;
            if (Latest10HistoriesQuery != null)
            {
                Duration = Latest10HistoriesQuery;// Latest10Histories.Average(a => a.Duration);
            }


            return (double)Duration;
            //return (from a in repository.context.TaskSchedulerHistories
            //        where a.TaskId == taskId
            //        select new TaskSchedulerHistoryPM()
            //        {
            //            Id = a.Id,
            //            Tenant = a.Tenant,
            //            EndDateTime = a.EndDateTime,
            //            IsError = a.IsError,
            //            RunResult = a.RunResult,
            //            StartDateTime = a.StartDateTime,
            //            TaskId = a.TaskId,
            //            StartDateTimeUTC = a.StartDateTimeUTC,
            //            EndDateTimeUTC = a.EndDateTimeUTC,
            //            LogFirstLine = a.LogFirstLine,
            //            LogType = a.LogType,
            //            Duration = (a.EndDateTime.Value - a.StartDateTime.Value).TotalSeconds
            //        }).Average(x => x.Duration);
        }

        public IQueryable<TasksSchedulerList> GetIQueryableEntityList(IQueryable<TasksScheduler> iQueryable)
        {
            IQueryable<TasksSchedulerList> result = null;
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                result = from a in iQueryable
                                                        select new TasksSchedulerList()
                                                        {
                                                            Id = a.Id,
                                                            Tenant = a.Tenant,
                                                            CreateDateTime = a.CreateDateTime,
                                                            CreatedBy = a.CreatedBy,
                                                            Description = a.Description,
                                                            Friday = a.Friday,
                                                            InActive = a.InActive,
                                                            IsLastRunError = a.IsLastRunError,
                                                            LastRunResult = a.LastRunResult,
                                                            LastRunStartTime = a.LastRunStartTime,
                                                            Monday = a.Monday,
                                                            Name = a.Name,
                                                            NextRunTime = a.NextRunTime,
                                                            RepeatInMinutes = a.RepeatInMinutes,
                                                            Satarday = a.Satarday,
                                                            ProcedureCode = a.ProcedureCode,
                                                            StartDateTime = a.StartDateTime,
                                                            Sunday = a.Sunday,
                                                            Thursday = a.Thursday,
                                                            TriggerType = a.TriggerType,
                                                            Tuesday = a.Tuesday,
                                                            UpdateDateTime = a.UpdateDateTime,
                                                            UpdatedBy = a.UpdatedBy,
                                                            Wednesday = a.Wednesday,
                                                            Type = a.Type,
                                                            NextRunTimeUTC = a.NextRunTimeUTC,
                                                            StartDateTimeUTC = a.StartDateTimeUTC,
                                                            LastRunStartTimeUTC = a.LastRunStartTimeUTC,
                                                            Version = a.Version,
                                                            Status = a.Status,
                                                            Retries = a.Retries,
                                                            LastRunEndTime = a.LastRunEndTime,
                                                            LastRunEndTimeUTC = a.LastRunEndTimeUTC,
                                                            AverageRunTime = a.AverageRunTime,
                                                            EntityId = a.EntityId,
                                                            Recepients ="" 
                                                        };
            }
            else
            {
                result = from a in iQueryable
                                                        select new TasksSchedulerList()
                                                        {
                                                            Id = a.Id,
                                                            Tenant = a.Tenant,
                                                            CreateDateTime = a.CreateDateTime,
                                                            CreatedBy = a.CreatedBy,
                                                            Description = a.Description,
                                                            Friday = a.Friday,
                                                            InActive = a.InActive,
                                                            IsLastRunError = a.IsLastRunError,
                                                            LastRunResult = a.LastRunResult,
                                                            LastRunStartTime = a.LastRunStartTime,
                                                            Monday = a.Monday,
                                                            Name = a.Name,
                                                            NextRunTime = a.NextRunTime,
                                                            RepeatInMinutes = a.RepeatInMinutes,
                                                            Satarday = a.Satarday,
                                                            ProcedureCode = a.ProcedureCode,
                                                            StartDateTime = a.StartDateTime,
                                                            Sunday = a.Sunday,
                                                            Thursday = a.Thursday,
                                                            TriggerType = a.TriggerType,
                                                            Tuesday = a.Tuesday,
                                                            UpdateDateTime = a.UpdateDateTime,
                                                            UpdatedBy = a.UpdatedBy,
                                                            Wednesday = a.Wednesday,
                                                            Type = a.Type,
                                                            NextRunTimeUTC = a.NextRunTimeUTC,
                                                            StartDateTimeUTC = a.StartDateTimeUTC,
                                                            LastRunStartTimeUTC = a.LastRunStartTimeUTC,
                                                            Version = a.Version,
                                                            Status = a.Status,
                                                            Retries = a.Retries,
                                                            LastRunEndTime = a.LastRunEndTime,
                                                            LastRunEndTimeUTC = a.LastRunEndTimeUTC,
                                                            AverageRunTime = a.AverageRunTime,
                                                            EntityId = a.EntityId,
                                                            Recepients =
                    (a.SchedulerDetailsXML.IndexOf("<To>") > -1 ? a.SchedulerDetailsXML.Substring(a.SchedulerDetailsXML.IndexOf("<To>") + 4, a.SchedulerDetailsXML.IndexOf("</To>") - 4 - a.SchedulerDetailsXML.IndexOf("<To>")) : null) + (a.SchedulerDetailsXML.IndexOf("<Cc>") > -1 ? ";" + a.SchedulerDetailsXML.Substring(a.SchedulerDetailsXML.IndexOf("<Cc>") + 4, a.SchedulerDetailsXML.IndexOf("</Cc>") - 4 - a.SchedulerDetailsXML.IndexOf("<Cc>")) : null) + (a.SchedulerDetailsXML.IndexOf("<Bcc>") > -1 ? ";" + a.SchedulerDetailsXML.Substring(a.SchedulerDetailsXML.IndexOf("<Bcc>") + 5, a.SchedulerDetailsXML.IndexOf("</Bcc>") - 5 - a.SchedulerDetailsXML.IndexOf("<Bcc>")) : null)
                                                        };

            }
            return result;
        }

        public TasksSchedulerPM GetSinglePM(string Id, int Tenant)
        {
            return (from a in repository.context.TasksSchedulers
                    where a.Id == Id && a.Tenant == Tenant
                    select new TasksSchedulerPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        CreateDateTime = a.CreateDateTime,
                        CreatedBy = a.CreatedBy,
                        Description = a.Description,
                        Friday = a.Friday,
                        InActive = a.InActive,
                        IsLastRunError = a.IsLastRunError,
                        LastRunResult = a.LastRunResult,
                        LastRunStartTime = a.LastRunStartTime,
                        Monday = a.Monday,
                        Name = a.Name,
                        NextRunTime = a.NextRunTime,
                        RepeatInMinutes = a.RepeatInMinutes,
                        Satarday = a.Satarday,
                        ProcedureCode = a.ProcedureCode,
                        StartDateTime = a.StartDateTime,
                        Sunday = a.Sunday,
                        Thursday = a.Thursday,
                        TriggerType = a.TriggerType,
                        Tuesday = a.Tuesday,
                        UpdateDateTime = a.UpdateDateTime,
                        UpdatedBy = a.UpdatedBy,
                        Wednesday = a.Wednesday,
                        Type = a.Type,
                        SchedulerDetailsXML = a.SchedulerDetailsXML,
                        NextRunTimeUTC = a.NextRunTimeUTC,
                        StartDateTimeUTC = a.StartDateTimeUTC,
                        LastRunStartTimeUTC = a.LastRunStartTimeUTC,
                        Version = a.Version,
                        Status = a.Status,
                        Retries = a.Retries,
                        LastRunEndTime = a.LastRunEndTime,
                        LastRunEndTimeUTC = a.LastRunEndTimeUTC,
                        AverageRunTime = a.AverageRunTime,
                        Duration = a.AverageRunTime,
                        EntityId = a.EntityId
                    }).FirstOrDefault();
        }

        public List<TasksSchedulerPM> GetAllInprogressTasksSchedulerPMs()
        {
            return (from a in repository.context.TasksSchedulers
                    where a.Status == "In progress" && a.InActive == false
                    select new TasksSchedulerPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        CreateDateTime = a.CreateDateTime,
                        CreatedBy = a.CreatedBy,
                        Description = a.Description,
                        Friday = a.Friday,
                        InActive = a.InActive,
                        IsLastRunError = a.IsLastRunError,
                        LastRunResult = a.LastRunResult,
                        LastRunStartTime = a.LastRunStartTime,
                        Monday = a.Monday,
                        Name = a.Name,
                        NextRunTime = a.NextRunTime,
                        RepeatInMinutes = a.RepeatInMinutes,
                        Satarday = a.Satarday,
                        ProcedureCode = a.ProcedureCode,
                        StartDateTime = a.StartDateTime,
                        Sunday = a.Sunday,
                        Thursday = a.Thursday,
                        TriggerType = a.TriggerType,
                        Tuesday = a.Tuesday,
                        UpdateDateTime = a.UpdateDateTime,
                        UpdatedBy = a.UpdatedBy,
                        Wednesday = a.Wednesday,
                        Type = a.Type,
                        NextRunTimeUTC = a.NextRunTimeUTC,
                        StartDateTimeUTC = a.StartDateTimeUTC,
                        LastRunStartTimeUTC = a.LastRunStartTimeUTC,
                        Version = a.Version,
                        Status = a.Status,
                        Retries = a.Retries,
                        LastRunEndTime = a.LastRunEndTime,
                        LastRunEndTimeUTC = a.LastRunEndTimeUTC,
                        AverageRunTime = a.AverageRunTime,
                        Duration = a.AverageRunTime,
                        SchedulerDetailsXML = a.SchedulerDetailsXML,
                        EntityId = a.EntityId

                    }).ToList();
        }

    }

    public class CustomSchedulerHistory
    {
        public int HistoryDuration { get; set; }
    }
}