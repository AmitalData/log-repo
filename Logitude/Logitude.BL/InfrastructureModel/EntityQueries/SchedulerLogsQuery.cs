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
    public class SchedulerLogsQuery
    {
        SchedulerLogsRepository repository;
        public SchedulerLogsQuery()
        {
            repository = new SchedulerLogsRepository();
        }

        public SchedulerLogsQuery(int tenant)
        {
            repository = new SchedulerLogsRepository(tenant);
        }

        public SchedulerLogsQuery(SchedulerLogsRepository SchedulerLogsRepository)
        {
            repository = SchedulerLogsRepository;
        }


        public SchedulerLogsPM GetSingleSchedulerLogsPM(string id)
        {
            return (from a in repository.context.SchedulerLogs
                    where a.Id == id
                    select new SchedulerLogsPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        CreateDate = a.CreateDate,
                        HistoryId = a.HistoryId,
                        Log = a.Log, 

                    }).FirstOrDefault();
        }

        public SchedulerLogsPM GetSingleSchedulerLogsPMByTenant(string Id, int Tenant)
        {
            return (from a in repository.context.SchedulerLogs
                    where a.Id == Id && a.Tenant == Tenant
                    select new SchedulerLogsPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        CreateDate = a.CreateDate,
                        HistoryId = a.HistoryId,
                        Log = a.Log,
                    }).FirstOrDefault();
        }

        public SchedulerLogsPM GetSinglePM(string Id, int Tenant)
        {
            return (from a in repository.context.SchedulerLogs
                    where a.Id == Id && a.Tenant == Tenant
                    select new SchedulerLogsPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        CreateDate = a.CreateDate,
                        HistoryId = a.HistoryId,
                        Log = a.Log,

                    }).FirstOrDefault();
        }


        public List<SchedulerLogsPM> GetSchedulerLogsPMs(int Tenant)
        {
            return (from a in repository.context.SchedulerLogs
                    where a.Tenant == Tenant
                    select new SchedulerLogsPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        CreateDate = a.CreateDate,
                        HistoryId = a.HistoryId,
                        Log = a.Log,

                    }).ToList();
        }

        public SchedulerLogsPM GetSchedulerLogsByHistory(string HistoryId)
        {
            return (from a in repository.context.SchedulerLogs
                    where a.HistoryId == HistoryId
                    select new SchedulerLogsPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        CreateDate = a.CreateDate,
                        HistoryId = a.HistoryId,
                        Log = a.Log,

                    }).FirstOrDefault();
        }


        public IQueryable<SchedulerLogsList> GetIQueryableEntityList(IQueryable<SchedulerLogs> iQueryable)
        {
            IQueryable<SchedulerLogsList> result = from a in iQueryable
                                                          select new SchedulerLogsList()
                                                          {
                                                              Id = a.Id,
                                                              Tenant = a.Tenant,
                                                              CreateDate = a.CreateDate,
                                                              HistoryId = a.HistoryId,
                                                              Log = a.Log,
                                                          };
            return result;
        }

    }
}