using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class TenantManagmentLicenseQuery
    {
        TenantManagmentLicenseRepository repository;

        public TenantManagmentLicenseQuery()
        {
            repository = new TenantManagmentLicenseRepository(); 
        }

        public TenantManagmentLicenseQuery(int tenant)
        {
            repository = new TenantManagmentLicenseRepository(tenant);
        }

        public TenantManagmentLicenseQuery(TenantManagmentLicenseRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<TenantManagmentLicensePM> GetTenantManagmentLicensePMs(int tenant)
        {
            return (from a in repository.context.TenantManagmentLicenses
                    where a.Tenant == tenant
                    select new TenantManagmentLicensePM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        PackageCode = a.PackageCode,
                        NumberOfUsers = a.NumberOfUsers,
                    });
        }

        public IQueryable<TenantManagmentLicensePM> GetTenantManagmentLicenseByPackageCode(string packageCode, int tenant)
        {
            return (from a in repository.context.TenantManagmentLicenses
                    where a.Tenant == tenant && a.PackageCode == packageCode
                    select new TenantManagmentLicensePM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        PackageCode = a.PackageCode,
                        NumberOfUsers = a.NumberOfUsers,
                    });
        }

        public TenantManagmentLicensePM GetSinglePM(string id)
        {
            var package = (from a in repository.context.TenantManagmentLicenses
                           where a.Id == id
                           select new TenantManagmentLicensePM()
                           {
                               Id = a.Id,
                               Tenant = a.Tenant,
                               PackageCode = a.PackageCode,
                               NumberOfUsers = a.NumberOfUsers,
                           }).FirstOrDefault();

            return package;
        }

        public IQueryable<TenantManagmentLicenseList> GetIQueryableEntityList(IQueryable<TenantManagmentLicense> iQueryable)
        {
            var result = from a in iQueryable
                         select new TenantManagmentLicenseList()
                         {
                             Id = a.Id,
                             Tenant = a.Tenant,
                             PackageCode = a.PackageCode,
                             NumberOfUsers = a.NumberOfUsers,
                         };

            return result;
        }
    }
}
