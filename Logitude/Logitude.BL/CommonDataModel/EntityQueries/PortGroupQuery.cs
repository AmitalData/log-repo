using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class PortGroupQuery
    {
        PortGroupRepository repository;
        public PortGroupQuery()
        {
            repository = new PortGroupRepository();
        }
        public PortGroupQuery(int tenant)
        {
            repository = new PortGroupRepository(tenant);
        }
        public PortGroupQuery(PortGroupRepository portGroupRepository)
        {
            repository = portGroupRepository;
        }

        public PortGroupPM GetSinglePM(string id, int tenant)
        {
            return (from a in repository.context.PortGroups
                   where a.Tenant == tenant && a.Id == id
                   select new PortGroupPM()
                   {
                       Id = a.Id,
                       Tenant = a.Tenant,
                       Code = a.Code,
                       Name = a.Name,                       
                       Inactive = a.Inactive,
                       SearchFields = a.SearchFields,                       
                   }).FirstOrDefault();
             }
        public PortGroupPM GetSinglePMByCode(string code, int tenant)
        {
            return (from a in repository.context.PortGroups
                    where a.Tenant == tenant && a.Code == code
                    select new PortGroupPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        Code = a.Code,
                        Name = a.Name,
                        Inactive = a.Inactive,
                        SearchFields = a.SearchFields,
                    }).FirstOrDefault();
        }
        public IQueryable<PortGroupList> GetIQueryableEntityList(IQueryable<PortGroup> iQueryable)
        {
            IQueryable<PortGroupList> result = from a in iQueryable
                                            select new PortGroupList()
                                            {
                                                Id = a.Id,
                                                Tenant = a.Tenant,
                                                Code = a.Code,
                                                Name = a.Name,
                                                Inactive = a.Inactive,
                                                SearchFields = a.SearchFields,
                                            };
            return result;
        }
    }
}
