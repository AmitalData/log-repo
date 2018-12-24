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
    }    
}
