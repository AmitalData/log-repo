using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CustomerGroupRepository : IRepository<CustomerGroup>
    {
        ICommonDataContext commonDataContext;

        public CustomerGroupRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CustomerGroupRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public CustomerGroupRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public int GetCustomerGroupsCount(int tenant)
        {
            return (from record in context.CustomerGroups where record.Tenant == tenant select record).Count();
        }

        public IQueryable<CustomerGroup> GetCustomerGroups(int tenant)
        {
            return (from record in context.CustomerGroups where record.Tenant == tenant select record);
        }

        public CustomerGroup GetSingleCustomerGroup(string id, int tenant)
        {
            return (from record in context.CustomerGroups where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public void Add(CustomerGroup entity)
        {
            context.CustomerGroups.Add(entity);
        }

        public void Remove(CustomerGroup entity)
        {
            try
            {
                context.CustomerGroups.Attach(entity);
            }
            catch { }
            context.CustomerGroups.Remove(entity);
        }

        public void Update(CustomerGroup entity)
        {
            try
            {
                context.CustomerGroups.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<CustomerGroup> All()
        {
            return context.CustomerGroups.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CustomerGroup> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public CustomerGroup GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
