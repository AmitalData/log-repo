

using System.Linq;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using System.Collections.Generic;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class DWQueryFilterQuery
    {
        DWQueryFilterRepository repository;

        public DWQueryFilterQuery()
        {
            repository = new DWQueryFilterRepository();
        }

        public DWQueryFilterQuery(int tenant)
        {
            repository = new DWQueryFilterRepository(tenant);
        }

        public DWQueryFilterQuery(DWQueryFilterRepository DWQueryFilterRepository)
        {
            repository = DWQueryFilterRepository;
        }

        public DWQueryFilterPM GetSingleDWQueryFilterPM(string id, int tenant)
        {
            return (from a in repository.webFreightContext.DWQueryFilters
                    where a.Id == id && a.Tenant == tenant
                    select new DWQueryFilterPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        DWObjectFieldId = a.DWObjectFieldId,
                        DWQueryId = a.DWQueryId,
                        IndexOrder = a.IndexOrder,
                        UserId = a.UserId,
                        IsPredefined = a.IsPredefined,
                        Operator = a.Operator,
                        PredefinedValue = a.PredefinedValue,
                        PredefinedValue2 = a.PredefinedValue2,
                    }).FirstOrDefault();
        }


        public IQueryable<DWQueryFilterPM> GetDWQueryFilterPMsByTenant(int tenant)
        {
            return (from a in repository.webFreightContext.DWQueryFilters
                    where a.Tenant == tenant
                    select new DWQueryFilterPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        DWObjectFieldId = a.DWObjectFieldId,
                        DWQueryId = a.DWQueryId,
                        IndexOrder = a.IndexOrder,
                        UserId = a.UserId,
                        IsPredefined = a.IsPredefined,
                        Operator = a.Operator,
                        PredefinedValue = a.PredefinedValue,
                        PredefinedValue2 = a.PredefinedValue2,
                    }
                  );
        }

        public DWQueryFilterPM GetSinglePM(string id, int tenant)
        {
            return (from a in repository.webFreightContext.DWQueryFilters
                    where a.Id == id && a.Tenant == tenant
                    select new DWQueryFilterPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        DWObjectFieldId = a.DWObjectFieldId,
                        DWQueryId = a.DWQueryId,
                        IndexOrder = a.IndexOrder,
                        UserId = a.UserId,
                        IsPredefined = a.IsPredefined,
                        Operator = a.Operator,
                        PredefinedValue = a.PredefinedValue,
                        PredefinedValue2 = a.PredefinedValue2,
                    }).FirstOrDefault();
        }

        public IQueryable<DWQueryFilterPM> GetDWQueryFilterPMs(int tenant)
        {
            return (from a in repository.webFreightContext.DWQueryFilters
                    where a.Tenant == tenant
                    select new DWQueryFilterPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        DWObjectFieldId = a.DWObjectFieldId,
                        DWQueryId = a.DWQueryId,
                        IndexOrder = a.IndexOrder,
                        UserId = a.UserId,
                        IsPredefined = a.IsPredefined,
                        Operator = a.Operator,
                        PredefinedValue = a.PredefinedValue,
                        PredefinedValue2 = a.PredefinedValue2,
                    });
        }

        public IQueryable<DWQueryFilterList> GetIQueryableEntityList(IQueryable<DWQueryFilter> iQueryable)
        {
            IQueryable<DWQueryFilterList> result = from a in iQueryable
                                                   select new DWQueryFilterList()
                                                   {
                                                       Id = a.Id,
                                                       Tenant = a.Tenant,
                                                       DWObjectFieldId = a.DWObjectFieldId,
                                                       DWQueryId = a.DWQueryId,
                                                       IndexOrder = a.IndexOrder,
                                                       UserId = a.UserId,
                                                       IsPredefined = a.IsPredefined,
                                                       Operator = a.Operator,
                                                       PredefinedValue = a.PredefinedValue,
                                                       PredefinedValue2 = a.PredefinedValue2,
                                                   };

            return result;
        }


        public IQueryable<DWQueryFilterPM> GetDWQueryFilterPMsByTenantAndUser(int tenant, string userid)
        {
            IQueryable<DWQueryFilterPM> advancedFilters = from a in repository.webFreightContext.DWQueryFilters
                                                                where (a.Tenant == tenant && a.UserId == userid) || a.Tenant == 0
                                                                select new DWQueryFilterPM()
                                                                {
                                                                    Id = a.Id,
                                                                    Tenant = a.Tenant,
                                                                    DWObjectFieldId = a.DWObjectFieldId,
                                                                    DWQueryId = a.DWQueryId,
                                                                    IndexOrder = a.IndexOrder,
                                                                    UserId = a.UserId,
                                                                    IsPredefined = a.IsPredefined,
                                                                    Operator = a.Operator,
                                                                    PredefinedValue = a.PredefinedValue,
                                                                    PredefinedValue2 = a.PredefinedValue2,
                                                                };
            return advancedFilters;

        }



    }
}
