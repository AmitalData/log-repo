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
using Simplog.Data.CommonDataModel.Repositories;
using System.Runtime.Remoting.Contexts;
using Logitude.BL.InfrastructureModel.Tools.EntityService;

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
                        EntityId = a.EntityId,
                        ResultType = a.ResultType,
                        Format = a.Format,
                        AdvancedFormat = a.AdvancedFormat,
                        ExecutedByServerName = a.ExecutedByServerName,

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
                        EntityId = a.EntityId,
                        ResultType = a.ResultType,
                        Format = a.Format,
                        AdvancedFormat = a.AdvancedFormat,
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
                        EntityId = a.EntityId,
                        ResultType = a.ResultType,
                        Format = a.Format,
                        AdvancedFormat = a.AdvancedFormat,
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
                             EntityId = a.EntityId,
                             ResultType = a.ResultType,
                             Format = a.Format,
                             AdvancedFormat = a.AdvancedFormat,
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
                             Recepients = ""
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
                             ResultType = a.ResultType,
                             Format = a.Format,
                             AdvancedFormat = a.AdvancedFormat,
                             Recepients = (a.SchedulerDetailsXML.IndexOf("<To>") > -1 ? a.SchedulerDetailsXML.Substring(a.SchedulerDetailsXML.IndexOf("<To>") + 4, a.SchedulerDetailsXML.IndexOf("</To>") - 4 - a.SchedulerDetailsXML.IndexOf("<To>")) : null) + (a.SchedulerDetailsXML.IndexOf("<Cc>") > -1 ? ";" + a.SchedulerDetailsXML.Substring(a.SchedulerDetailsXML.IndexOf("<Cc>") + 4, a.SchedulerDetailsXML.IndexOf("</Cc>") - 4 - a.SchedulerDetailsXML.IndexOf("<Cc>")) : null) + (a.SchedulerDetailsXML.IndexOf("<Bcc>") > -1 ? ";" + a.SchedulerDetailsXML.Substring(a.SchedulerDetailsXML.IndexOf("<Bcc>") + 5, a.SchedulerDetailsXML.IndexOf("</Bcc>") - 5 - a.SchedulerDetailsXML.IndexOf("<Bcc>")) : null)
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
                        EntityId = a.EntityId,
                        ResultType = a.ResultType,
                        Format = a.Format,
                        AdvancedFormat = a.AdvancedFormat,
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
                        EntityId = a.EntityId,
                        ResultType = a.ResultType,
                        Format = a.Format,
                        AdvancedFormat = a.AdvancedFormat,
                        ExecutedByServerName = a.ExecutedByServerName,

                    }).ToList();
        }

        public bool GetIsEntityHasScheduler(string entityId, int tenant)
        {

            bool result = (from a in repository.context.TasksSchedulers
                           where a.Tenant == tenant && a.EntityId == entityId && !a.InActive
                           select a).Any();

            return result;

        }



        public void CopyFromTenant0(int tenant, int tenatToCopy)
        {

            TasksSchedulerService service = new TasksSchedulerService(repository.context, tenatToCopy);

            List<TasksScheduler> pocos = this.repository.context.TasksSchedulers.Where(r => r.InActive == false && r.Tenant == tenant && r.Name == "ExchangeRateUpdateTask").ToList();


            foreach (var item in pocos)
            {
                DateTime date = DateTime.UtcNow;
                TimeSpan ts02 = new TimeSpan(02, 00, 0);
                TimeSpan ts00 = new TimeSpan(00, 00, 0);
               
                TasksSchedulerPM tasksScheduler = new TasksSchedulerPM()
                {
                    Tenant = tenatToCopy,
                    CreateDateTime = DateTime.UtcNow,
                    UpdateDateTime = DateTime.UtcNow,
                    Name = item.Name,
                    Description = item.Description,
                    NextRunTime = date.AddDays(1).Date + ts02,
                    InActive = item.InActive,
                    TriggerType = item.TriggerType,
                    Satarday = item.Satarday,
                    Sunday = item.Sunday,
                    Monday = item.Monday,
                    Tuesday = item.Tuesday,
                    Wednesday = item.Wednesday,
                    Thursday = item.Thursday,
                    Friday = item.Friday,
                    StartDateTime = date.AddDays(1) .Date+ ts02,
                    RepeatInMinutes = item.RepeatInMinutes,
                    IsLastRunError = item.IsLastRunError,
                    MonthlyDay = item.MonthlyDay,
                    SchedulerDetailsXML = item.SchedulerDetailsXML,
                    Type = item.Type,
                    NextRunTimeUTC = date.AddDays(1).Date + ts00,
                    StartDateTimeUTC = date.AddDays(1) .Date+ ts00,
                    Version = item.Version,
                    Status = item.Status,
                    Retries = item.Retries,
                    ProcedureCode = item.ProcedureCode,
                    AverageRunTime = item.AverageRunTime,
                    EntityId = item.EntityId,
                    ResultType = item.ResultType,
                    Format = item.Format,
                    AdvancedFormat = item.AdvancedFormat,
                    ExecutedByServerName = item.ExecutedByServerName


                };

                service.Create(tasksScheduler);

            }
            this.repository.context.SaveChanges();

        }
    }

    public class CustomSchedulerHistory
    {
        public int HistoryDuration { get; set; }
    }
}