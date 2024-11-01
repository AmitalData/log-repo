using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using AmitalCloud.Infrastructure.Data.Repositories;
using System.Linq;

namespace AmitalCloud.Infrastructure.Data.Queries
{
    public class UserLicenseQuery
    {
        IRepository<UserLicense> repository;
        IAmitalCloudContext context ;
        public UserLicenseQuery() : this(0)
        {
        }
        public UserLicenseQuery(int tenant)
        {
            context =  AmitalCloudContext.GetContext(tenant);
            repository = new Repository<UserLicense>(context);
        }
        public UserLicenseQuery(IRepository<UserLicense> myRepository)
        {
            repository = myRepository;
        }
        public IQueryable<UserLicensePM> GetUserLicensesByTenant(int tenant)
        {
            IQueryable<UserLicensePM> myResult = (from a in context.UserLicenses
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
