using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class UserLastSettingsQuery
    {
        UserLastSettingsRepository repository;

        public UserLastSettingsQuery()
        {
            repository = new UserLastSettingsRepository(); 
        }

        public UserLastSettingsQuery(int tenant)
        {
            repository = new UserLastSettingsRepository(tenant);
        }

        public UserLastSettingsQuery(UserLastSettingsRepository repository)
        {
            this.repository = repository;
        }

        public UserLastSettingsPM GetSinglePM(string id, int tenant)
        {
            UserLastSettingsPM entity = (from a in repository.context.UserLastSettings
                                      where a.Tenant == tenant
                                      && a.Id == id
                                      select new UserLastSettingsPM()
                                      {
                                          UserId = a.UserId,
                                          Tenant = a.Tenant,
                                          Id = a.Id,
                                          ControlNameSpace = a.ControlNameSpace,
                                          FilterName = a.FilterName,
                                          FilterValue = a.FilterValue
                                      }).FirstOrDefault();
            return entity;
        }
        public UserLastSettingsPM GetSinglePM(string userId, int tenant, string FilterName)
        {
            UserLastSettingsPM entity = (from a in repository.context.UserLastSettings
                                         where a.Tenant == tenant
                                         && a.UserId == userId && a.FilterName == FilterName
                                         select new UserLastSettingsPM()
                                         {
                                             UserId = a.UserId,
                                             Tenant = a.Tenant,
                                             Id = a.Id,
                                             ControlNameSpace = a.ControlNameSpace,
                                             FilterName = a.FilterName,
                                             FilterValue = a.FilterValue
                                         }).FirstOrDefault();
            return entity;
        }

        public List<UserLastSettingsPM> GetAllUserSettingsPM(string userId, int tenant, string regionnamespace)
        {
            List<UserLastSettingsPM> entity = (from a in repository.context.UserLastSettings
                                         where a.Tenant == tenant
                                         && a.UserId == userId && a.ControlNameSpace == regionnamespace
                                         select new UserLastSettingsPM()
                                         {
                                             UserId = a.UserId,
                                             Tenant = a.Tenant,
                                             Id = a.Id,
                                             ControlNameSpace = a.ControlNameSpace,
                                             FilterName = a.FilterName,
                                             FilterValue = a.FilterValue
                                         }).ToList();
            return entity;
        }

        public IQueryable<UserLastSettingsPM> GetUserLastSettingsPMsByTenant(int tenant)
        {
            IQueryable<UserLastSettingsPM> UserLastSettingsPMs = from a in repository.context.UserLastSettings
                                                           where a.Tenant == tenant
                                                           select new UserLastSettingsPM()
                                                           {
                                                               UserId = a.UserId,
                                                               Tenant = a.Tenant,
                                                               Id = a.Id,
                                                               ControlNameSpace = a.ControlNameSpace,
                                                               FilterName = a.FilterName,
                                                               FilterValue = a.FilterValue
                                                           };
            return UserLastSettingsPMs;
        }


        public IQueryable<UserLastSettingsPM> GetUserLastSettingsPMsByUserIds(List<string> userIds )
        {
            IQueryable<UserLastSettingsPM> UserLastSettingsPMs = from a in repository.context.UserLastSettings
                                                           where userIds.Contains(a.Id)
                                                           select new UserLastSettingsPM()
                                                           {
                                                               UserId = a.UserId,
                                                               Tenant = a.Tenant,
                                                               Id = a.Id,
                                                               ControlNameSpace = a.ControlNameSpace,
                                                               FilterName = a.FilterName,
                                                               FilterValue = a.FilterValue

                                                           };
            return UserLastSettingsPMs;
        }

        //public IQueryable<UserLastSettingsList> GetIQueryableEntityList(List<string> userIds)
        //{
        //    IQueryable<UserLastSettingsPM> UserLastSettingsPMs = from a in repository.context.UserLastSettings
        //                                                         where userIds.Contains(a.Id)
        //                                                         select new UserLastSettingsPM()
        //                                                         {
        //                                                             UserId = a.UserId,
        //                                                             Tenant = a.Tenant,
        //                                                             Id = a.Id,
        //                                                             ControlNameSpace = a.ControlNameSpace,
        //                                                             FilterName = a.FilterName,
        //                                                             FilterValue = a.FilterValue

        //                                                         };
        //    return UserLastSettingsPMs;
        //}
        
    }
}