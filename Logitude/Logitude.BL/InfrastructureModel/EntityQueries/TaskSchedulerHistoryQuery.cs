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
    public class TaskSchedulerHistoryQuery
    {
        TaskSchedulerHistoryRepository repository;
        public TaskSchedulerHistoryQuery()
        {
            repository = new TaskSchedulerHistoryRepository();
        }

        public TaskSchedulerHistoryQuery(int tenant)
        {
            repository = new TaskSchedulerHistoryRepository(tenant);
        }

        public TaskSchedulerHistoryQuery(TaskSchedulerHistoryRepository TaskSchedulerHistoryRepository)
        {
            repository = TaskSchedulerHistoryRepository;
        }


        public TaskSchedulerHistoryPM GetSingleTaskSchedulerHistoryPM(string id)
        {
            return (from a in repository.context.TaskSchedulerHistories
                    where a.Id == id
                    select new TaskSchedulerHistoryPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        EndDateTime = a.EndDateTime,
                        IsError = a.IsError,
                        RunResult = a.RunResult,
                        StartDateTime = a.StartDateTime,
                        TaskId = a.TaskId,
                        StartDateTimeUTC = a.StartDateTimeUTC,
                        EndDateTimeUTC = a.EndDateTimeUTC,
                        LogFirstLine = a.LogFirstLine,
                        LogType = a.LogType

                    }).FirstOrDefault();
        }

        public TaskSchedulerHistoryPM GetSingleTaskSchedulerHistoryPMByTenant(string Id, int Tenant)
        {
            return (from a in repository.context.TaskSchedulerHistories
                    where a.Id == Id && a.Tenant == Tenant
                    select new TaskSchedulerHistoryPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        EndDateTime = a.EndDateTime,
                        IsError = a.IsError,
                        RunResult = a.RunResult,
                        StartDateTime = a.StartDateTime,
                        TaskId = a.TaskId,
                        StartDateTimeUTC = a.StartDateTimeUTC,
                        EndDateTimeUTC = a.EndDateTimeUTC,
                        LogFirstLine = a.LogFirstLine,
                        LogType = a.LogType
                    }).FirstOrDefault();
        }

        public TaskSchedulerHistoryPM GetSinglePM(string Id, int Tenant)
        {
            return (from a in repository.context.TaskSchedulerHistories
                    where a.Id == Id && a.Tenant == Tenant
                    select new TaskSchedulerHistoryPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        EndDateTime = a.EndDateTime,
                        IsError = a.IsError,
                        RunResult = a.RunResult,
                        StartDateTime = a.StartDateTime,
                        TaskId = a.TaskId,
                        StartDateTimeUTC = a.StartDateTimeUTC,
                        EndDateTimeUTC = a.EndDateTimeUTC,
                        LogFirstLine = a.LogFirstLine,
                        LogType = a.LogType
                    }).FirstOrDefault();
        }


        public List<TaskSchedulerHistoryPM> GetTaskSchedulerHistoryPMs(int Tenant)
        {
            return (from a in repository.context.TaskSchedulerHistories
                    where a.Tenant == Tenant
                    select new TaskSchedulerHistoryPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        EndDateTime = a.EndDateTime,
                        IsError = a.IsError,
                        RunResult = a.RunResult,
                        StartDateTime = a.StartDateTime,
                        TaskId = a.TaskId,
                        StartDateTimeUTC = a.StartDateTimeUTC,
                        EndDateTimeUTC = a.EndDateTimeUTC,
                        LogFirstLine = a.LogFirstLine,
                        LogType = a.LogType
                    }).ToList();
        }


        public IQueryable<TaskSchedulerHistoryList> GetIQueryableEntityList(IQueryable<TaskSchedulerHistory> iQueryable)
        {
            IQueryable<TaskSchedulerHistoryList> result = from a in iQueryable
                                                          select new TaskSchedulerHistoryList()
                                                          {
                                                              Id = a.Id,
                                                              Tenant = a.Tenant,
                                                              EndDateTime = a.EndDateTime,
                                                              IsError = a.IsError,
                                                              RunResult = a.RunResult,
                                                              StartDateTime = a.StartDateTime,
                                                              TaskId = a.TaskId,
                                                              StartDateTimeUTC = a.StartDateTimeUTC,
                                                              EndDateTimeUTC = a.EndDateTimeUTC,
                                                              LogFirstLine = a.LogFirstLine,
                                                              LogType = a.LogType
                                                          };
            return result;
        }

    }
}