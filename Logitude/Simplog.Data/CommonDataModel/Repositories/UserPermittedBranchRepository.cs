using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class UserPermittedBranchRepository:IRepository<UserPermittedBranch>
    {
        ICommonDataContext commonDataContext;
        public UserPermittedBranchRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public UserPermittedBranchRepository()
        {
            commonDataContext = new CommonDataContext();

        }

        public UserPermittedBranchRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<UserPermittedBranch> GetUserPermittedBranches(int tenant)
        {
            return (from record in context.UserPermittedBranches.Include("User") where record.Tenant == tenant select record);
        }

        public IQueryable<UserPermittedBranch> GetUserPermittedBranchesByUserId(string userId,int tenant)
        {
            return (from record in context.UserPermittedBranches.Include("User") where record.UserId == userId && record.Tenant == tenant select record);
        }

        public UserPermittedBranch GetSingleUserPermittedBranch(string id, int tenant)
        {
            return (from record in context.UserPermittedBranches.Include("User") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

     

        public UserPermittedBranch GetUserPermittedBranchByUserAndBranch(string userId, string branchId, int tenant)
        {
            return (from record in context.UserPermittedBranches.Include("User") where record.UserId == userId && record.Tenant == tenant && record.BranchId == branchId select record).FirstOrDefault();
        }

        public void Add(UserPermittedBranch entity)
        {
            context.UserPermittedBranches.Add(entity);

        }

        public void Remove(UserPermittedBranch entity)
        {
            try
            {
                context.UserPermittedBranches.Attach(entity);
            }
            catch { }
            context.UserPermittedBranches.Remove(entity);
        }

        public void Update(UserPermittedBranch entity)
        {
            context.UserPermittedBranches.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<UserPermittedBranch> All()
        {
            return context.UserPermittedBranches.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<UserPermittedBranch> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public UserPermittedBranch GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}