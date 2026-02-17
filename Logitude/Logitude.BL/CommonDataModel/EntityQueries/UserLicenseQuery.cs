using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class UserLicenseQuery
    {
        UserLicenseRepository repository;
        public UserLicenseQuery()
        {
            repository = new UserLicenseRepository();
        }

        public UserLicenseQuery(int tenant)
        {
            repository = new UserLicenseRepository(tenant);
        }

        public UserLicenseQuery(UserLicenseRepository myRepository)
        {
            repository = myRepository;
        }

        public IQueryable<UserLicensePM> GetUserLicensesByTenant(int tenant)
        {
            IQueryable<UserLicensePM> myResult = (from a in repository.context.UserLicenses
                                                  where a.Tenant == tenant
                                                  select new UserLicensePM()
                                                  {
                                                      Id = a.Id,
                                                      Tenant = a.Tenant,
                                                      UserId = a.UserId,
                                                      PackageCode = a.PackageCode,
                                                  });
            return myResult;
        }
    }
}
