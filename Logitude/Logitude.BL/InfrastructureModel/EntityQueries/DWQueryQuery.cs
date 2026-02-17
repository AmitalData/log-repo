

using System.Linq;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using System.Collections.Generic;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class DWQueryQuery
    {
        DWQueryRepository repository;

        public DWQueryQuery()
        {
            repository = new DWQueryRepository();
        }

        public DWQueryQuery(int tenant)
        {
            repository = new DWQueryRepository(tenant);
        }

        public DWQueryQuery(DWQueryRepository DWQueryRepository)
        {
            repository = DWQueryRepository;
        }

        public DWQueryPM GetSingleDWQueryPM(string id, int tenant)
        {
            return (from a in repository.webFreightContext.DWQueries
                    where a.Id == id && a.Tenant == tenant
                    select new DWQueryPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        SQLString = a.SQLString,
                        //DWObjectTableCode = a.DWObjectTableCode,
                        CreatedByUserId  =a.CreatedByUserId,
                        UpdateByUserId = a.UpdateByUserId,
                        UpdatedDate = a.UpdatedDate,
                        CreatedDate = a.CreatedDate,
                    }).FirstOrDefault();
        }


        public IQueryable<DWQueryPM> GetDWQueryPMsByTenant(int tenant)
        {
            return (from a in repository.webFreightContext.DWQueries
                    where a.Tenant == tenant
                    select new DWQueryPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        SQLString = a.SQLString,
                        //DWObjectTableCode = a.DWObjectTableCode,
                        CreatedByUserId = a.CreatedByUserId,
                        UpdateByUserId = a.UpdateByUserId,
                        UpdatedDate = a.UpdatedDate,
                        CreatedDate = a.CreatedDate,
                    }
                  );
        }

        public DWQueryPM GetSinglePM(string id, int tenant)
        {
            return (from a in repository.webFreightContext.DWQueries
                    where a.Id == id && a.Tenant == tenant
                    select new DWQueryPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        SQLString = a.SQLString,
                        //DWObjectTableCode = a.DWObjectTableCode,
                        CreatedByUserId = a.CreatedByUserId,
                        UpdateByUserId = a.UpdateByUserId,
                        UpdatedDate = a.UpdatedDate,
                        CreatedDate = a.CreatedDate,
                    }).FirstOrDefault();
        }

        public IQueryable<DWQueryPM> GetDWQueryPMs(int tenant)
        {
            return (from a in repository.webFreightContext.DWQueries
                    where a.Tenant == tenant
                    select new DWQueryPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        SQLString = a.SQLString,
                        //DWObjectTableCode = a.DWObjectTableCode,
                        CreatedByUserId = a.CreatedByUserId,
                        UpdateByUserId = a.UpdateByUserId,
                        UpdatedDate = a.UpdatedDate,
                        CreatedDate = a.CreatedDate,
                    });
        }

        public IQueryable<DWQueryList> GetIQueryableEntityList(IQueryable<DWQuery> iQueryable)
        {
            IQueryable<DWQueryList> result = from a in iQueryable
                                                   select new DWQueryList()
                                                   {
                                                       Id = a.Id,
                                                       Tenant = a.Tenant,
                                                       SQLString = a.SQLString,
                                                       //DWObjectTableCode = a.DWObjectTableCode,
                                                       CreatedByUserId = a.CreatedByUserId,
                                                       UpdateByUserId = a.UpdateByUserId,
                                                       UpdatedDate = a.UpdatedDate,
                                                       CreatedDate = a.CreatedDate,
                                                   };

            return result;
        }


    }
}
