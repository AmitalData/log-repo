using Logitude.BL.GlobalModel.EntityPMs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.GlobalModel.EntityQueries
{
    public class TenantAddOnQuery
    {
        TenantAddOnRepository repository;

        public TenantAddOnQuery()
        {
            repository = new TenantAddOnRepository();
        }

        public TenantAddOnQuery(int tenant)
        {
            repository = new TenantAddOnRepository(tenant);
        }

        public TenantAddOnQuery(TenantAddOnRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<TenantAddOnPM> GetTenantAddOnPMs(int tenant)
        {
            return (from a in repository.context.TenantAddOns
                    where a.Tenant == tenant
                    select new TenantAddOnPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        PackageCode = a.PackageCode,
                    });
        }

        public TenantAddOnPM GetSinglePM(string id)
        {
            TenantAddOnPM entityPM = (from a in repository.context.TenantAddOns
                                      where a.Id == id
                                      select new TenantAddOnPM()
                                      {
                                          Id = a.Id,
                                          Tenant = a.Tenant,
                                          PackageCode = a.PackageCode,
                                      }).FirstOrDefault();

            return entityPM;
        }


        public IQueryable<TenantAddOnPM> GetTenantAddOnPMByListTenantids(List<int> tenantManagementids)
        {
            IQueryable<TenantAddOnPM>tenantAddOnsPMlist = (from a in repository.context.TenantAddOns
                                                                                   where tenantManagementids.Contains(a.Tenant)
                                                                                   select new TenantAddOnPM()
                                                                                   {
                                                                                       Id = a.Id,
                                                                                       Tenant = a.Tenant,
                                                                                       PackageCode = a.PackageCode,
                                                                                       
                                                                                   });
            return tenantAddOnsPMlist;
        }

    }
}
