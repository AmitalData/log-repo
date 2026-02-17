
using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class TenantLoginPolicyRepository : IRepository<TenantLoginPolicy>
    {

        ICommonDataContext commonDataContext;

        public TenantLoginPolicyRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public TenantLoginPolicyRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public TenantLoginPolicyRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<TenantLoginPolicy> GetTenantLoginPolicies(int tenant)
        {
            return (from record in context.TenantLoginPolicies.Include("LoginPolicy") where record.Tenant == tenant select record);
        }

        public TenantLoginPolicy GetSingleTenantLoginPolicy(int tenant)
        {
            return (from record in context.TenantLoginPolicies.Include("LoginPolicy") where  record.Tenant == tenant select record).FirstOrDefault();
        }


        public TenantLoginPolicy GetSingleTenantLoginPolicy(int id , int tenant)
        {
            return (from record in context.TenantLoginPolicies.Include("LoginPolicy") where record.Tenant == tenant select record).FirstOrDefault();
        }


    

        public void Add(TenantLoginPolicy entity)
        {
            context.TenantLoginPolicies.Add(entity);
        }

        public void Remove(TenantLoginPolicy entity)
        {
            try
            {
                context.TenantLoginPolicies.Attach(entity);
            }
            catch { };
            context.TenantLoginPolicies.Remove(entity);
        }

        public void Update(TenantLoginPolicy entity)
        {
            try
            {
                context.TenantLoginPolicies.Attach(entity);
            }
            catch { };
            context.SetAsModified(entity);
        }

        public List<TenantLoginPolicy> All()
        {
            return context.TenantLoginPolicies.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<TenantLoginPolicy> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public TenantLoginPolicy GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

    }
}