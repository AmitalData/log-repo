using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class UserPermittedBranchRepository:IRepository<UserPermittedBranch,string>
    {
        IAmitalCloudContext currentContext;
        public UserPermittedBranchRepository(IAmitalCloudContext context)
        {
            currentContext = context;
        }

        public UserPermittedBranchRepository()
        {
            currentContext = new AmitalCloudContext();

        }

        public UserPermittedBranchRepository(int tenant)
        {
            currentContext = AmitalCloudContext.GetContext(tenant);
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

        public IAmitalCloudContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<UserPermittedBranch> GetMulti(IEntityKeyFields<UserPermittedBranch,string> entityKeys)
        {
            throw new NotImplementedException();
        }

        public UserPermittedBranch GetSingle(IEntityKeyFields<UserPermittedBranch,string> entityKeys)
        {
            throw new NotImplementedException();
        }

        public List<string> GetUserPermittedBranchesIdsByUserId(string userId, int tenant)
        {
            return (from record in context.UserPermittedBranches where record.UserId == userId && record.Tenant == tenant select record.BranchId).ToList();
        }
    }
}