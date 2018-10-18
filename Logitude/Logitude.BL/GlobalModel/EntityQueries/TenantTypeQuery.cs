using Logitude.BL.GlobalModel.EntityLists;
using Logitude.BL.GlobalModel.EntityPMs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.GlobalModel.EntityQueries
{
    public class TenantTypeQuery
    {
        TenantTypeRepository repository;

        public TenantTypeQuery()
        {
            repository = new TenantTypeRepository(); 
        }

        public TenantTypeQuery(int tenant)
        {
            repository = new TenantTypeRepository(tenant);
        }

        public TenantTypeQuery(TenantTypeRepository tenantTypeRepository)
        {
            repository = tenantTypeRepository;
        }

        public TenantTypePM GetSingleTenantTypePM(string code)
        {
            return (from a in repository.context.TenantTypes
                    where a.Code == code
                    select new TenantTypePM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }

        public IQueryable<TenantTypeList> GetIQueryableEntityList(IQueryable<TenantType> iQueryable)
        {
            IQueryable<TenantTypeList> result = from entity in iQueryable
                                                select new TenantTypeList()
                                                     {
                                                         Name = entity.Name,
                                                         Code = entity.Code,
                                                         SearchFields = entity.SearchFields,
                                                     };
            return result;
        }
    }
}
