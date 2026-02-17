using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class SharedUserQueryQuery
    {
        SharedUserQueryRepository repository;
        
        public SharedUserQueryQuery(int tenant)
        {
            repository = new SharedUserQueryRepository(tenant);
        }

        public SharedUserQueryQuery(SharedUserQueryRepository queryRepository)
        {
            repository = queryRepository;
        }

        public SharedUserQueryPM GetSinglePM(string id, int tenant)
        {
            SharedUserQueryPM result =
            (from a in repository.context.SharedUserQueries.Include("User").Include("Query")
             where a.Id == id && (a.Tenant == tenant || a.Tenant == 0)
             select new SharedUserQueryPM()
             {                 
                 Id = a.Id,                 
                 Tenant = a.Tenant,
                 UserId = a.UserId,                 
                 QueryId = a.QueryId,
             }).FirstOrDefault();
            
            return result;
        }

        public IQueryable<SharedUserQueryPM> GetSharedUserQueriesForQuery(string queryId, int tenant)
        {
            IQueryable<SharedUserQueryPM> result = (from d in repository.context.SharedUserQueries
                                                         where d.QueryId == queryId && d.Tenant == tenant
                                                         select new SharedUserQueryPM()
                                                         {
                                                             Id = d.Id,
                                                             Tenant = d.Tenant,
                                                             UserId = d.UserId,
                                                             QueryId = d.QueryId,
                                                         });

            return result;
        }

        public IQueryable<SharedUserQueryPM> GetSharedUserQueriesByTenant(int tenant)
        {
            IQueryable<SharedUserQueryPM> result = (from d in repository.context.SharedUserQueries
                                                    where d.Tenant == tenant
                                                    select new SharedUserQueryPM()
                                                    {
                                                        Id = d.Id,
                                                        Tenant = d.Tenant,
                                                        UserId = d.UserId,
                                                        QueryId = d.QueryId,
                                                    });

            return result;
        }
    }    
}
