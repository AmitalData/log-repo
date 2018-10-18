using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class UserLastLoginRepository : IRepository<UserLastLogin>
    {
        ICommonDataContext commonDataContext;

        public UserLastLoginRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public UserLastLoginRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public UserLastLoginRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
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

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<UserLastLogin> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public UserLastLogin GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

    }
}