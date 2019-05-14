using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;

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
                        LastRunTime = a.LastRunTime,
                        Monday = a.Monday,
                        Name = a.Name,
                        NextRunTime = a.NextRunTime,
                        RepeatInMinutes = a.RepeatInMinutes,
                        Satarday = a.Satarday,
                        ServiceClassName = a.ServiceClassName,
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
                        LastRunTimeUTC = a.LastRunTimeUTC,
                        Version = a.Version,
                        Status = a.Status


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
                        LastRunTime = a.LastRunTime,
                        Monday = a.Monday,
                        Name = a.Name,
                        NextRunTime = a.NextRunTime,
                        RepeatInMinutes = a.RepeatInMinutes,
                        Satarday = a.Satarday,
                        ServiceClassName = a.ServiceClassName,
                        StartDateTime = a.StartDateTime,
                        Sunday = a.Sunday,
                        Thursday = a.Thursday,
                        TriggerType = a.TriggerType,
                        Tuesday = a.Tuesday,
                        UpdateDateTime = a.UpdateDateTime,
                        UpdatedBy = a.UpdatedBy,
                        Wednesday = a.Wednesday ,
                        Type = a.Type,
                        SchedulerDetailsXML = a.SchedulerDetailsXML,
                        NextRunTimeUTC = a.NextRunTimeUTC,
                        StartDateTimeUTC = a.StartDateTimeUTC,
                        LastRunTimeUTC = a.LastRunTimeUTC,
                        Version = a.Version,
                        Status = a.Status
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
                        LastRunTime = a.LastRunTime,
                        Monday = a.Monday,
                        Name = a.Name,
                        NextRunTime = a.NextRunTime,
                        RepeatInMinutes = a.RepeatInMinutes,
                        Satarday = a.Satarday,
                        ServiceClassName = a.ServiceClassName,
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
                        LastRunTimeUTC = a.LastRunTimeUTC,
                        Version = a.Version,
                        Status = a.Status
                    }).ToList();
        }

        public List<TasksSchedulerPM> GetTasksSchedulerPMsBByType(string type,int Tenant)
        {
            return (from a in repository.context.TasksSchedulers
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
                        LastRunTime = a.LastRunTime,
                        Monday = a.Monday,
                        Name = a.Name,
                        NextRunTime = a.NextRunTime,
                        RepeatInMinutes = a.RepeatInMinutes,
                        Satarday = a.Satarday,
                        ServiceClassName = a.ServiceClassName,
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
                        LastRunTimeUTC = a.LastRunTimeUTC,
                        Version = a.Version,
                        Status = a.Status
                    }).ToList();
        }


        public IQueryable<TasksSchedulerList> GetIQueryableEntityList(IQueryable<TasksScheduler> iQueryable)
        {
            IQueryable<TasksSchedulerList> result = from a in iQueryable
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
                                                               LastRunTime = a.LastRunTime,
                                                               Monday = a.Monday,
                                                               Name = a.Name,
                                                               NextRunTime = a.NextRunTime,
                                                               RepeatInMinutes = a.RepeatInMinutes,
                                                               Satarday = a.Satarday,
                                                               ServiceClassName = a.ServiceClassName,
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
                                                               LastRunTimeUTC = a.LastRunTimeUTC,
                                                               Version = a.Version,
                                                               Status = a.Status
                                                           };
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
                        LastRunTime = a.LastRunTime,
                        Monday = a.Monday,
                        Name = a.Name,
                        NextRunTime = a.NextRunTime,
                        RepeatInMinutes = a.RepeatInMinutes,
                        Satarday = a.Satarday,
                        ServiceClassName = a.ServiceClassName,
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
                        LastRunTimeUTC = a.LastRunTimeUTC,
                        Version = a.Version,
                        Status = a.Status
                    }).FirstOrDefault();
        }
    }
}