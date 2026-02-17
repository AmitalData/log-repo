using Logitude.BL.GlobalModel.EntityPMs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.GlobalModel.EntityQueries
{
    public class TenantManagementLicenseQuery
    {
        TenantManagementLicenseRepository repository;

        public TenantManagementLicenseQuery()
        {
            repository = new TenantManagementLicenseRepository();
        }

        public TenantManagementLicenseQuery(int tenant)
        {
            repository = new TenantManagementLicenseRepository(tenant);
        }

        public TenantManagementLicenseQuery(TenantManagementLicenseRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<TenantManagementLicensePM> GetTenantManagementLicensePMs(int tenant)
        {
            return (from a in repository.context.TenantManagementLicenses
                    where a.Tenant == tenant
                    select new TenantManagementLicensePM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        PackageCode = a.PackageCode,                        
                        NumberOfUsers = a.NumberOfUsers,
                    });
        }


        public IQueryable<TenantManagementLicensePM> GetTenantManagementLicenseByListids(List<int> tenantManagementids)
        {
            IQueryable<TenantManagementLicensePM> TenantManagementLicensePMlist = (from a in repository.context.TenantManagementLicenses
                                               where tenantManagementids.Contains(a.Tenant)
                                               select new TenantManagementLicensePM()
                                                      {
                                                        Id = a.Id,
                                                        Tenant = a.Tenant,
                                                        PackageCode = a.PackageCode,
                                                        NumberOfUsers = a.NumberOfUsers,
                                                      });
            return TenantManagementLicensePMlist;
        }





        public TenantManagementLicensePM GetSinglePM(string id)
        {
            TenantManagementLicensePM entityPM = (from a in repository.context.TenantManagementLicenses
                                                  where a.Id == id
                                                  select new TenantManagementLicensePM()
                                                  {
                                                      Id = a.Id,
                                                      Tenant = a.Tenant,
                                                      PackageCode = a.PackageCode,
                                                      NumberOfUsers = a.NumberOfUsers,
                                                  }).FirstOrDefault();

            return entityPM;
        }
    }
}
