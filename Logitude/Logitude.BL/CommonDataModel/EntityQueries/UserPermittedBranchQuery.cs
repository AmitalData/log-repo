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
    public class UserPermittedBranchQuery
    {
         UserPermittedBranchRepository repository;

        public UserPermittedBranchQuery(int tenant)
        {
            repository = new UserPermittedBranchRepository(tenant);
        }

        public UserPermittedBranchQuery(UserPermittedBranchRepository UserPermittedBranchRepository)
        {
            repository = UserPermittedBranchRepository;
        }

        public UserPermittedBranchPM GetSinglePM(string id, int tenant)
        {
            UserPermittedBranchPM UserPermittedBranch = (from a in repository.context.UserPermittedBranches
                                                         where a.Id == id && a.Tenant == tenant
                                                         select new UserPermittedBranchPM()
                                                         {
                                                             Id = a.Id,
                                                             Tenant = a.Tenant,
                                                             BranchId = a.BranchId,
                                                             UserId = a.UserId,
                                                         }).FirstOrDefault();
            return UserPermittedBranch;
        }

        public IQueryable<UserPermittedBranchPM> GetUserPermittedBranchPMsByTenant(int tenant)
        {
            IQueryable<UserPermittedBranchPM> UserPermittedBranchs = from a in repository.context.UserPermittedBranches
                                                     where a.Tenant == tenant
                                                     select new UserPermittedBranchPM()
                                                     {
                                                         Id = a.Id,
                                                         Tenant = a.Tenant,
                                                         BranchId = a.BranchId,
                                                         UserId = a.UserId,
                                                     };
            return UserPermittedBranchs;
        }

        public IQueryable<UserPermittedBranchPM> GetContactFromUserPermittedBranchPMsByUserId(string id, int tenant)
        {
            NetCommonHelper.Logger.DevLog.Instance.WriteInfo(" before GetContactFromUserPermittedBranchPMsByUserId  repository.context.GetConnection().Database" + repository.context.GetConnection()?.Database);
            NetCommonHelper.Logger.DevLog.Instance.WriteInfo(" GetContactFromUserPermittedBranchPMsByUserId  id:" + id+",tenant:"+tenant);
            IQueryable<UserPermittedBranchPM> UserPermittedBranchs = (from a in repository.context.UserPermittedBranches.Include("Contact")
                                      where a.UserId == id
                                      select new UserPermittedBranchPM()
                                      {
                                          Id = a.Id,
                                          Tenant = a.Tenant,
                                          BranchId = a.BranchId,
                                          UserId = a.UserId,
                                      });
            NetCommonHelper.Logger.DevLog.Instance.WriteInfo(" after GetContactFromUserPermittedBranchPMsByUserId  repository.context.GetConnection().Database" + repository.context.GetConnection()?.Database);

            return UserPermittedBranchs;
        }
    }
}