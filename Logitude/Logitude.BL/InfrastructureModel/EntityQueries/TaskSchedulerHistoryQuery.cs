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
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                var r = (from a in repository.context.TaskSchedulerHistories
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
                             LogType = a.LogType,
                             LogDocumentId = a.LogDocumentId,

                         }).FirstOrDefault()
                        ;
                r.Duration = GetDurationDiffSeconds(r.EndDateTime,r.StartDateTime);
                return r;
            }
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
                        LogType = a.LogType,
                        Duration = DbFunctions.DiffSeconds(a.EndDateTime, a.StartDateTime),
                        LogDocumentId = a.LogDocumentId,

                    }).FirstOrDefault();
        }

        private int GetDurationDiffSeconds(DateTime? endDateTime, DateTime? startDateTime)
        {
            if (endDateTime.HasValue && startDateTime.HasValue)
            {
                return (int)endDateTime.Value.Subtract(startDateTime.Value).TotalSeconds;

            }
            return 0;
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
                        LogType = a.LogType,
                        LogDocumentId = a.LogDocumentId,

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
                        LogType = a.LogType,
                        LogDocumentId = a.LogDocumentId,
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
                        LogType = a.LogType,
                        LogDocumentId = a.LogDocumentId,
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
                                                              LogType = a.LogType,
                                                              LogDocumentId = a.LogDocumentId,
                                                          };
            return result;
        }

        public double GetTaskAvarageDuration(string taskId)
        {
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                

                var Latest10Histories1 = (from a in repository.context.TaskSchedulerHistories
                                         where a.TaskId == taskId
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
                                             LogType = a.LogType,
                                             LogDocumentId = a.LogDocumentId,
                                             //Duration = DbFunctions.DiffSeconds(a.EndDateTime, a.StartDateTime),

                                         }).OrderByDescending(x => x.StartDateTime).Take(10)
                                         .ToList();
                Latest10Histories1.ForEach(r => r.Duration = GetDurationDiffSeconds(r.EndDateTime, r.StartDateTime));
                var Duration1 = Latest10Histories1.Average(a => a.Duration);
                return (double)Duration1;
            }


            var Latest10Histories = (from a in repository.context.TaskSchedulerHistories
                                     where a.TaskId == taskId
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
                                         LogType = a.LogType,
                                         Duration = DbFunctions.DiffSeconds(a.EndDateTime, a.StartDateTime),
                                         LogDocumentId = a.LogDocumentId,

                                     }).OrderByDescending(x => x.StartDateTime).Take(10);

            var Duration = Latest10Histories.Average(a => a.Duration);
            return (double)Duration;

            //return (from a in repository.context.TaskSchedulerHistories
            //                         where a.TaskId == taskId
            //                         select new TaskSchedulerHistoryPM()
            //                         {
            //                             Id = a.Id,
            //                             Tenant = a.Tenant,
            //                             EndDateTime = a.EndDateTime,
            //                             IsError = a.IsError,
            //                             RunResult = a.RunResult,
            //                             StartDateTime = a.StartDateTime,
            //                             TaskId = a.TaskId,
            //                             StartDateTimeUTC = a.StartDateTimeUTC,
            //                             EndDateTimeUTC = a.EndDateTimeUTC,
            //                             LogFirstLine = a.LogFirstLine,
            //                             LogType = a.LogType,
            //                             Duration = DbFunctions.DiffSeconds(a.EndDateTime, a.StartDateTime),
            //                         }).Average(x => x.Duration);
        }

        public TaskSchedulerHistoryPM GetLastTaskSchedulerHistoryPM(string TaskId)
        {
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                var my = (from a in repository.context.TaskSchedulerHistories
                          where a.TaskId == TaskId
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
                              LogType = a.LogType,
                              LogDocumentId = a.LogDocumentId,
                              //Duration = DbFunctions.DiffSeconds(a.EndDateTime, a.StartDateTime),

                          }).OrderByDescending(a => a.StartDateTime).FirstOrDefault();
                my.Duration = GetDurationDiffSeconds(my.EndDateTime, my.StartDateTime);
                return my;
            }
                return (from a in repository.context.TaskSchedulerHistories
                    where a.TaskId == TaskId
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
                        LogType = a.LogType,
                        Duration = DbFunctions.DiffSeconds(a.EndDateTime, a.StartDateTime),
                        LogDocumentId = a.LogDocumentId,


                    }).OrderByDescending(a => a.StartDateTime).FirstOrDefault();
        }

    }
}