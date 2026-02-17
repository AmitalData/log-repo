using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class VATTypesGroupRepository : IRepository<VATTypesGroup>
    {
        ICommonDataContext commonDataContext;

        public VATTypesGroupRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public VATTypesGroupRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }           

        public VATTypesGroup GetSingleVATTypesGroup(string groupVATTypeId, string singleVATTypeId, int tenant)
        {
            return (from d in context.VATTypesGroups
                    where d.GroupVATTypeId == groupVATTypeId
                    && d.SingleVATTypeId == singleVATTypeId
                    && d.Tenant == tenant
                    select d).FirstOrDefault();
        }

        public IQueryable<VATTypesGroup> GetVATTypesGroup(int tenant)
        {
            return (from d in context.VATTypesGroups.Include("GroupVATType").Include("SingleVATType")
                    where d.Tenant == tenant
                    select d);
        }

        public IQueryable<VATTypesGroup> GetVATTypesGroup(string groupVATTypeId, int tenant)
        {
            return (from d in context.VATTypesGroups.Include("GroupVATType").Include("SingleVATType")
                    where d.GroupVATTypeId == groupVATTypeId
                    && d.Tenant == tenant
                    select d);
        }

        public void Add(VATTypesGroup entity)
        {
            context.VATTypesGroups.Add(entity);
        }

        public void Remove(VATTypesGroup entity)
        {
            context.VATTypesGroups.Attach(entity);
            context.VATTypesGroups.Remove(entity);
        }

        public void Update(VATTypesGroup entity)
        {
            context.VATTypesGroups.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<VATTypesGroup> All()
        {
            return context.VATTypesGroups.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<VATTypesGroup> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public VATTypesGroup GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
