using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class UserLastLoginQuery
    {
        UserLastLoginRepository repository;

        public UserLastLoginQuery()
        {
            repository = new UserLastLoginRepository(); 
        }

        public UserLastLoginQuery(int tenant)
        {
            repository = new UserLastLoginRepository(tenant);
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
                                                           };
            return userLastLoginPMs;
        }


        public IQueryable<UserLastLoginPM> GetUserLastLoginPMsByUserIds(List<string> userIds )
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
                                                           };
            return userLastLoginPMs;
        }

    }
}