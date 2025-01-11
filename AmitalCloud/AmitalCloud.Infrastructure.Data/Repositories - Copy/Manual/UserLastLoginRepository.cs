using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class UserLastLoginRepository : IRepository<UserLastLogin,string>
    {
        IAmitalCloudContext currentContext;

        public UserLastLoginRepository()
        {
            currentContext = new AmitalCloudContext();
        }

        public UserLastLoginRepository(IAmitalCloudContext context)
        {
            currentContext = context;
        }

        public UserLastLoginRepository(int tenant)
        {
            currentContext = AmitalCloudContext.GetContext(tenant);
        }

        public IQueryable<UserLastLogin> GetUserLastLogins(int tenant)
        {
            return (from record in context.UserLastLogins.Include("User") where record.Tenant == tenant select record);
        }

        public UserLastLogin GetSingleUserLastLogin(string id, int tenant, bool getFromCache)
        {
            return (from record in context.UserLastLogins.Include("User") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public IQueryable<UserLastLogin> GetUsersWorkspaceLastLogins(int tenant)
        {
            return (from d in context.UserLastLogins.Include("User").Include("User.Contact").Include("User.BusinessUnit")
                    where d.Tenant == tenant 
                    select d);
        }

        public void Add(UserLastLogin entity)
        {
            context.UserLastLogins.Add(entity);
        }

        public void Remove(UserLastLogin entity)
        {
            try
            {
                context.UserLastLogins.Attach(entity);
            }
            catch { };
            context.UserLastLogins.Remove(entity);
        }

        public void Update(UserLastLogin entity)
        {
            try
            {
                context.UserLastLogins.Attach(entity);
            }
            catch { };
            context.SetAsModified(entity);
        }

        public List<UserLastLogin> All()
        {
            return context.UserLastLogins.ToList();
        }

        public IAmitalCloudContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<UserLastLogin> GetMulti(IEntityKeyFields<UserLastLogin,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public UserLastLogin GetSingle(IEntityKeyFields<UserLastLogin,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }

    }
}