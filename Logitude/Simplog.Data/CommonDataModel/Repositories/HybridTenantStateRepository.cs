using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class HybridTenantStateRepository : IRepository<HybridTenantState>
    {
        ICommonDataContext commonDataContext;



        public HybridTenantStateRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public HybridTenantStateRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<HybridTenantState> GetHybridTenantStates()
        {
            return context.HybridTenantStates;
        }

        public HybridTenantState GetSingleHybridTenantState(int tenant)
        {
            return (from record in context.HybridTenantStates where  record.Tenant == tenant select  record).FirstOrDefault();
        }

    


        public void Add(HybridTenantState entity)
        {
            context.HybridTenantStates.Add(entity);
        }

        public void Remove(HybridTenantState entity)
        {
            context.HybridTenantStates.Attach(entity);
            context.HybridTenantStates.Remove(entity);
        }

        public void Update(HybridTenantState entity)
        {
            context.HybridTenantStates.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<HybridTenantState> All()
        {
            return context.HybridTenantStates.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<HybridTenantState> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public HybridTenantState GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
