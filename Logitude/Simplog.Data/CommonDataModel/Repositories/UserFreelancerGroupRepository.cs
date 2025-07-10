using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class UserFreelancerGroupRepository : IRepository<UserFreelancerGroup>
    {
        ICommonDataContext commonDataContext;

        public UserFreelancerGroupRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public UserFreelancerGroupRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public UserFreelancerGroupRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public List<UserFreelancerGroup> GetUserFreeLancerGroupByUserId(string UserId, int tenant)
        {
            return (from d in context.UserFreelancerGroups where d.UserId == UserId && d.Tenant == tenant select d).ToList();
        }
        public List<UserFreelancerGroup> GetUserFreelancerGroupByUserId(string UserId, int tenant)
        {
            return context.UserFreelancerGroups
                          .Where(d => d.UserId == UserId && d.Tenant == tenant)
                          .ToList();
        }



        public void Add(UserFreelancerGroup entity)
        {
            context.UserFreelancerGroups.Add(entity);
        }

        public void Remove(UserFreelancerGroup entity)
        {
            context.UserFreelancerGroups.Attach(entity);
            context.UserFreelancerGroups.Remove(entity);
        }

        public void Update(UserFreelancerGroup entity)
        {
            context.UserFreelancerGroups.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<UserFreelancerGroup> All()
        {
            return context.UserFreelancerGroups.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        List<UserFreelancerGroup> IRepository<UserFreelancerGroup>.GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        UserFreelancerGroup IRepository<UserFreelancerGroup>.GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
