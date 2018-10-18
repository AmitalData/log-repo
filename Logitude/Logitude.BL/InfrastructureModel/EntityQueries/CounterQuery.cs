using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InfrastructureModel.Repositories;

using Logitude.BL.InfrastructureModel.EntityPMs;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class CounterQuery
    {
        CounterRepository repository;
        public CounterQuery()
        {
            repository = new CounterRepository(); 
        }

        public CounterQuery(int tenant)
        {
            repository = new CounterRepository(tenant);
        }

        public CounterQuery(CounterRepository counterRepository)
        {
            repository = counterRepository;
        }

        public CounterPM GetSingleCounterPM(string myCounterId, int tenant)
        {
            CounterPM myResult = (from a in repository.context.Counters
                                  where a.Tenant == tenant && a.Id == myCounterId
                                  select new CounterPM()
                                  {
                                      Id = a.Id,
                                      Code = a.Code,
                                      Name = a.Name,
                                      ObjectTableId = a.ObjectTableId,
                                      Tenant = a.Tenant,
                                      ChangedByUserId = a.ChangedByUserId,
                                      ChangedDate = a.ChangedDate,                                      
                                  }).FirstOrDefault();


            return myResult;
        }

        public IQueryable<CounterPM> GetObjectTableCounters(string objectTableId, int tenant)
        {
            IQueryable<CounterPM> result = (from a in repository.context.Counters
                                            where a.Tenant == tenant && a.ObjectTableId == objectTableId
                                            select new CounterPM()
                                            {
                                                Id = a.Id,
                                                Code = a.Code,
                                                Name = a.Name,
                                                ObjectTableId = a.ObjectTableId,
                                                Tenant = a.Tenant,
                                                ChangedByUserId = a.ChangedByUserId,
                                                ChangedDate = a.ChangedDate,
                                            });

            return result;
        }
        public IQueryable<CounterPM> GetCountersByTenant(int tenant)
        {
            IQueryable<CounterPM> result = (from a in repository.context.Counters
                                            where a.Tenant == tenant
                                            select new CounterPM()
                                            {
                                                Id = a.Id,
                                                Code = a.Code,
                                                Name = a.Name,
                                                ObjectTableId = a.ObjectTableId,

                                                Tenant = a.Tenant,
                                                ChangedByUserId = a.ChangedByUserId,
                                                ChangedDate = a.ChangedDate,
                                            });


            return result;
        }
    }
}