using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class INTTRABranchRegisteredCarrierRepository : IRepository<INTTRABranchRegisteredCarrier>
    {
        ICommonDataContext context;
        public ICommonDataContext Context
        {
            get { return this.context; }
        }
        public INTTRABranchRegisteredCarrierRepository(int tenant)
        {
            context = CommonDataContext.GetContext(tenant);
        }
        public INTTRABranchRegisteredCarrierRepository(ICommonDataContext context)
        {
            this.context = context;
        }

        public IQueryable<INTTRABranchRegisteredCarrier> GetAllByTenant(int tenant)
        {
            IQueryable<INTTRABranchRegisteredCarrier> myResult = from a in Context.INTTRABranchRegisteredCarriers
                                                                 where a.Tenant == tenant
                                                                 select a;
            return myResult;
        }

        public void Add(INTTRABranchRegisteredCarrier entity)
        {
            context.INTTRABranchRegisteredCarriers.Add(entity);
        }
        public void Remove(INTTRABranchRegisteredCarrier entity)
        {
            context.INTTRABranchRegisteredCarriers.Attach(entity);
            context.INTTRABranchRegisteredCarriers.Remove(entity);
        }
        public void Update(INTTRABranchRegisteredCarrier entity)
        {
            context.INTTRABranchRegisteredCarriers.Attach(entity);
            context.SetAsModified(entity);
        }
        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<INTTRABranchRegisteredCarrier> All()
        {
            return context.INTTRABranchRegisteredCarriers.ToList();
        }
        public INTTRABranchRegisteredCarrier GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
        public List<INTTRABranchRegisteredCarrier> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
