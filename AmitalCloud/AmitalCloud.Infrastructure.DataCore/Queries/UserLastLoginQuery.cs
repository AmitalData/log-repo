using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace AmitalCloud.Infrastructure.Data.Queries
{
    public class UserLastLoginQuery
    {
        UserLastLoginRepository repository;


        public UserLastLoginQuery(int tenant) : this(AmitalCloudContext.GetContext(tenant))
        {
        }
        public UserLastLoginQuery(IAmitalCloudContext context) : this(new UserLastLoginRepository(context))
        {
        }
        public UserLastLoginQuery(UserLastLoginRepository repository)
        {
            this.repository = repository;
        }

        public UserLastLoginPM GetSinglePM(string id, int tenant)
        {
            UserLastLoginPM entity = (from a in repository.context.UserLastLogins.Include("User")
                                      where a.Tenant == tenant
                                      && a.Id == id
                                      select new UserLastLoginPM()
                                      {
                                          LoginDateTime = a.LoginDateTime,
                                          Tenant = a.Tenant,
                                          Id = a.Id,
                                          ComputerId = a.ComputerId,
                                          WorkEnvironment = a.WorkEnvironment,
                                          IP = a.IP,
                                      }).FirstOrDefault();
            return entity;
        }

        public IQueryable<UserLastLoginPM> GetUserLastLoginPMsByTenant(int tenant)
        {
            IQueryable<UserLastLoginPM> userLastLoginPMs = from a in repository.context.UserLastLogins.Include("User")
                                                           where a.Tenant == tenant
                                                           select new UserLastLoginPM()
                                                           {
                                                               LoginDateTime = a.LoginDateTime,
                                                               Tenant = a.Tenant,
                                                               Id = a.Id,
                                                               ComputerId = a.ComputerId,
                                                               WorkEnvironment = a.WorkEnvironment,
                                                               IP = a.IP,
                                                           };
            return userLastLoginPMs;
        }


        public IQueryable<UserLastLoginPM> GetUserLastLoginPMsByUserIds(List<string> userIds)
        {
            IQueryable<UserLastLoginPM> userLastLoginPMs = from a in repository.context.UserLastLogins
                                                           where userIds.Contains(a.Id)
                                                           select new UserLastLoginPM()
                                                           {
                                                               LoginDateTime = a.LoginDateTime,
                                                               Tenant = a.Tenant,
                                                               Id = a.Id,
                                                               ComputerId = a.ComputerId,
                                                               WorkEnvironment = a.WorkEnvironment,
                                                               IP = a.IP,
                                                           };
            return userLastLoginPMs;
        }

    }
}