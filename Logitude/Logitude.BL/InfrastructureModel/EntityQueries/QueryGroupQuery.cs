using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InfrastructureModel.Repositories;

using Logitude.BL.InfrastructureModel.EntityPMs;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class QueryGroupQuery
    {
        QueryGroupRepository repository;
        public QueryGroupQuery()
        {
            repository = new QueryGroupRepository(); 
        }

        public QueryGroupQuery(int tenant)
        {
            repository = new QueryGroupRepository(tenant);
        }

        public QueryGroupQuery(QueryGroupRepository queryGroupRepository)
        {
            repository = queryGroupRepository;
        }

        public QueryGroupPM GetSingleQueryGroupPM(string code)
        {
            return (from a in repository.context.QueryGroups
                    where a.Code == code
                    select new QueryGroupPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        IndexOrder = a.IndexOrder,
                    }).FirstOrDefault();
        }


        public IQueryable<QueryGroupPM> GetQueryGroupPMs()
        {
            return from a in repository.context.QueryGroups
                   select new QueryGroupPM() { Code = a.Code, Name = a.Name };
        }



    }
}