using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class TenantAddOnRepository : IRepository<TenantAddOn>
    {
        ICommonDataContext commonDataContext;

        public TenantAddOnRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public TenantAddOnRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public TenantAddOnRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<TenantAddOn> GetTenantAddOns(int tenant)
        {
            return (from d in context.TenantAddOns where d.Tenant == tenant select d);
        }

        public TenantAddOn GetSingleTenantAddOn(string id)
        {
            return (from d in context.TenantAddOns where d.Id == id select d).FirstOrDefault();

        }

        public void Add(TenantAddOn entity)
        {
            this.context.TenantAddOns.Add(entity);
        }

        public void Remove(TenantAddOn entity)
        {
            this.context.TenantAddOns.Attach(entity);
            this.context.TenantAddOns.Remove(entity);
        }

        public void Update(TenantAddOn entity)
        {
            try
            {
                this.context.TenantAddOns.Attach(entity);
            }

            catch
            {

            }

            this.context.SetAsModified(entity);
        }

        public List<TenantAddOn> All()
        {
            return this.context.TenantAddOns.ToList<TenantAddOn>();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }

        public List<TenantAddOn> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public TenantAddOn GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
