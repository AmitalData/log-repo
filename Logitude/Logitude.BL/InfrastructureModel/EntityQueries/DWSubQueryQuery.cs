

using System.Linq;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using System.Collections.Generic;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class DWSubQueryQuery
    {
        DWSubQueryRepository repository;

        public DWSubQueryQuery()
        {
            repository = new DWSubQueryRepository();
        }

        public DWSubQueryQuery(int tenant)
        {
            repository = new DWSubQueryRepository(tenant);
        }

        public DWSubQueryQuery(DWSubQueryRepository DWSubQueryRepository)
        {
            repository = DWSubQueryRepository;
        }

        public DWSubQueryPM GetSingleDWSubQueryPM(string id, int tenant)
        {
            return (from a in repository.webFreightContext.DWSubQueries
                    where a.Id == id && a.Tenant == tenant
                    select new DWSubQueryPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        SQLString = a.SQLString,
                        ColumnsXML = a.ColumnsXML,
                        FiltersXML = a.FiltersXML,
                        DWQueryId = a.DWQueryId,
                        DWFactTableCode = a.DWFactTableCode,
                        
                    }).FirstOrDefault();
        }


        public IQueryable<DWSubQueryPM> GetDWSubQueryPMsByTenant(int tenant)
        {
            return (from a in repository.webFreightContext.DWSubQueries
                    where a.Tenant == tenant
                    select new DWSubQueryPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        SQLString = a.SQLString,
                        ColumnsXML = a.ColumnsXML,
                        FiltersXML = a.FiltersXML,
                        DWQueryId = a.DWQueryId,
                        DWFactTableCode = a.DWFactTableCode,
                    }
                  );
        }

        public DWSubQueryPM GetSinglePM(string id, int tenant)
        {
            return (from a in repository.webFreightContext.DWSubQueries
                    where a.Id == id && a.Tenant == tenant
                    select new DWSubQueryPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        SQLString = a.SQLString,
                        ColumnsXML = a.ColumnsXML,
                        FiltersXML = a.FiltersXML,
                        DWQueryId = a.DWQueryId,
                        DWFactTableCode = a.DWFactTableCode,
                    }).FirstOrDefault();
        }

        public DWSubQueryPM GetSinglePMByQueryid(string Queryid, int tenant)
        {
            return (from a in repository.webFreightContext.DWSubQueries
                    where a.DWQueryId == Queryid && a.Tenant == tenant
                    select new DWSubQueryPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        SQLString = a.SQLString,
                        ColumnsXML = a.ColumnsXML,
                        FiltersXML = a.FiltersXML,
                        DWQueryId = a.DWQueryId,
                        DWFactTableCode = a.DWFactTableCode,
                    }).FirstOrDefault();
        }

        public IQueryable<DWSubQueryPM> GetDWSubQueryPMs(int tenant)
        {
            return (from a in repository.webFreightContext.DWSubQueries
                    where a.Tenant == tenant
                    select new DWSubQueryPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        SQLString = a.SQLString,
                        ColumnsXML = a.ColumnsXML,
                        FiltersXML = a.FiltersXML,
                        DWQueryId = a.DWQueryId,
                        DWFactTableCode = a.DWFactTableCode,
                    });
        }

        public IQueryable<DWSubQueryList> GetIQueryableEntityList(IQueryable<DWSubQuery> iQueryable)
        {
            IQueryable<DWSubQueryList> result = from a in iQueryable
                                                   select new DWSubQueryList()
                                                   {
                                                       Id = a.Id,
                                                       Tenant = a.Tenant,
                                                       SQLString = a.SQLString,
                                                       //DWObjectTableCode = a.DWObjectTableCode,
                                                       ColumnsXML = a.ColumnsXML,
                                                       FiltersXML = a.FiltersXML,
                                                       DWQueryId = a.DWQueryId,
                                                       DWFactTableCode = a.DWFactTableCode,
                                                   };

            return result;
        }


    }
}
