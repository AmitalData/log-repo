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
    public class CustomerTenantAccessRequestRepository : IRepository<CustomerTenantAccessRequest>
    {
        ICommonDataContext commonDataContext;

        public CustomerTenantAccessRequestRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CustomerTenantAccessRequestRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public CustomerTenantAccessRequestRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<CustomerTenantAccessRequest> GetCustomerTenantAccessRequestsByTenant(int tenant)
        {
            return from a in context.CustomerTenantAccessRequests where a.Tenant == tenant select a;
        }

        public CustomerTenantAccessRequest GetSingleCustomerTenantAccessRequest(string id, int tenant)
        {
            return (from a in context.CustomerTenantAccessRequests.Include("RequestStatusCode").Include("HybridPartnerId") where a.Id == id && a.Tenant == tenant select a).FirstOrDefault();

        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void Add(CustomerTenantAccessRequest entity)
        {
            context.CustomerTenantAccessRequests.Add(entity);
        }

        public void Remove(CustomerTenantAccessRequest entity)
        {
            context.CustomerTenantAccessRequests.Attach(entity);
            context.CustomerTenantAccessRequests.Remove(entity);
        }

        public void Update(CustomerTenantAccessRequest entity)
        {
            context.CustomerTenantAccessRequests.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomerTenantAccessRequest> All()
        {
            return context.CustomerTenantAccessRequests.ToList();

        }

        public List<CustomerTenantAccessRequest> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public CustomerTenantAccessRequest GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
    }
}
