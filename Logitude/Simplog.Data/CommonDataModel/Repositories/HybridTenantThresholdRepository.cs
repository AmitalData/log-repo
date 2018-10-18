using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class HybridTenantThresholdRepository : IRepository<HybridTenantThreshold>
    {
        ICommonDataContext commonDataContext;

        public HybridTenantThresholdRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public HybridTenantThresholdRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public HybridTenantThresholdRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<HybridTenantThreshold> GetHybridTenantThresholds(int tenant)
        {
            return context.HybridTenantThresholds.Where(a=>a.Tenant == tenant);
        }

        public HybridTenantThreshold GetSingleHybridTenantThresholdByTenant(int tenant)
        {
            return (from record in context.HybridTenantThresholds where record.Tenant == tenant select record).FirstOrDefault();
        }


              public HybridTenantThreshold GetSingleHybridTenantThreshold(int tenant,int tenant2)
        {
            return (from record in context.HybridTenantThresholds where record.Tenant == tenant select record).FirstOrDefault();
        }

        public HybridTenantThreshold GetSingleHybridTenantThreshold(int tenant)
        {
            return (from record in context.HybridTenantThresholds where record.Tenant == tenant select record).FirstOrDefault();
        }
        public void Add(HybridTenantThreshold entity)
        {
            context.HybridTenantThresholds.Add(entity);
        }

        public void Remove(HybridTenantThreshold entity)
        {
            context.HybridTenantThresholds.Attach(entity);
            context.HybridTenantThresholds.Remove(entity);
        }

        public void Update(HybridTenantThreshold entity)
        {
            context.HybridTenantThresholds.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<HybridTenantThreshold> All()
        {
            return context.HybridTenantThresholds.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<HybridTenantThreshold> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public HybridTenantThreshold GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
