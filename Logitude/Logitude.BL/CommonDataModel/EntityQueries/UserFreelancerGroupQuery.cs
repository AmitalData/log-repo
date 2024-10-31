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
    public class UserFreelancerGroupQuery
    {
        UserFreelancerGroupRepository repository;

        public UserFreelancerGroupQuery()
        {
            repository = new UserFreelancerGroupRepository(); 
        }

        public UserFreelancerGroupQuery(int tenant)
        {
            repository = new UserFreelancerGroupRepository(tenant);
        }

        public UserFreelancerGroupQuery(UserFreelancerGroupRepository UserFreelancerGroupRepository)
        {
            repository = UserFreelancerGroupRepository;
        }

        public UserFreelancerGroupPM GetSinglePM(string id, int tenant)
        {
            UserFreelancerGroupPM UserFreelancerGroup = (from a in repository.context.UserFreelancerGroups
                                                         where a.Id == id && a.Tenant == tenant
                                                         select new UserFreelancerGroupPM()
                                                         {
                                                             Id = a.Id,
                                                             Tenant = a.Tenant,
                                                             GroupID = a.GroupID,
                                                             UserId = a.UserId,
                                                         }).FirstOrDefault();
            return UserFreelancerGroup;
        }

        public IQueryable<UserFreelancerGroupPM> GetUserFreelancerGroupPMsByTenantAndUserId(string id, int tenant)
        {
            IQueryable<UserFreelancerGroupPM> UserFreelancerGroup = (from a in repository.context.UserFreelancerGroups
                                                         where a.Id == id && a.Tenant == tenant
                                                         select new UserFreelancerGroupPM()
                                                         {
                                                             Id = a.Id,
                                                             Tenant = a.Tenant,
                                                             GroupID = a.GroupID,
                                                             UserId = a.UserId,
                                                         });
            return UserFreelancerGroup;
        }

        public IQueryable<UserFreelancerGroupPM> GetUserFreelancerGroupPMsByTenant(int tenant)
        {
            IQueryable<UserFreelancerGroupPM> UserFreelancerGroup = from a in repository.context.UserFreelancerGroups
                                                                       where a.Tenant == tenant
                                                     select new UserFreelancerGroupPM()
                                                     {
                                                         Id = a.Id,
                                                         Tenant = a.Tenant,
                                                         GroupID = a.GroupID,
                                                         UserId = a.UserId,
                                                     };
            return UserFreelancerGroup;
        }
        public List<string> GetUserFreelancerGroupsByUserId(string userId, int tenant)
        {
            var UserFreelancerGroup = (from a in repository.context.UserFreelancerGroups
                                        join b in repository.context.FreelancerGroupTypes on a.GroupID equals b.Id
                                                                    where a.UserId == userId && a.Tenant == tenant
                                                                    select b.Code).ToList();
            return UserFreelancerGroup;
        }

    }
}