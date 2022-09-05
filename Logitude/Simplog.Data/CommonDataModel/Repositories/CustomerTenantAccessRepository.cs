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
    public class CustomerTenantAccessRepository : IRepository<CustomerTenantAccess>
    {
         ICommonDataContext commonDataContext;

        public CustomerTenantAccessRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CustomerTenantAccessRepository()
        {
            commonDataContext=new CommonDataContext();
        }

        public CustomerTenantAccessRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<CustomerTenantAccess> GetCustomerTenantAccesses(int tenant)
        {
            return from a in context.CustomerTenantAccesses where a.Tenant == tenant select a;
        }
        public IQueryable<CustomerTenantAccess> GetCustomerTenantAccessesByCustomerTenant(int customerTenant)
        {
            return from a in context.CustomerTenantAccesses where a.CustomerTenant == customerTenant select a;
        }

        public IQueryable<CustomerTenantAccess> GetCustomerTenantAccessesByTenant(int tenant)
        {
            return from a in context.CustomerTenantAccesses where a.Tenant == tenant select a;
        }

        public CustomerTenantAccess GetSingleCustomerTenantAccess(string id,int tenant)
        {
            return (from a in context.CustomerTenantAccesses.Include("StatusCode").Include("UpdatedByUser.Contact").Include("UpdatedByUser") where a.Id == id && a.Tenant == tenant select a).FirstOrDefault();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public List<CustomerTenantAccess> GetCustomerTenantAccessFromIdList(List<string> ids, int tenant)
        {

            List<CustomerTenantAccess> customerTenantAccess = (from a in context.CustomerTenantAccesses
                                                where a.Tenant == tenant && ids.Contains(a.Id)
                                                select a).ToList();


            return customerTenantAccess;
        }

        public IQueryable<CustomerTenantAccess> GetAllCustomerTenantAccessFromIdList(List<string> ids, int tenant)
        {

            IQueryable<CustomerTenantAccess> customerTenantAccess = (from a in context.CustomerTenantAccesses
                                                               where a.Tenant == tenant && ids.Contains(a.Id)
                                                               select a);


            return customerTenantAccess;
        }

        public IQueryable<CustomerTenantAccess> GetlasCustomerTenantAccessList(int tenant)
        {

            IQueryable<CustomerTenantAccess> customerTenantAccess = (from a in context.CustomerTenantAccesses
                                                                     where a.Tenant == tenant
                                                                     orderby a.RequestDateTime descending
                                                                     select a).Take(10);


            return customerTenantAccess;
        }

       

        public void Add(CustomerTenantAccess entity)
        {
            context.CustomerTenantAccesses.Add(entity);
        }

        public void Remove(CustomerTenantAccess entity)
        {
            context.CustomerTenantAccesses.Attach(entity);
            context.CustomerTenantAccesses.Remove(entity);
        }

        public void Update(CustomerTenantAccess entity)
        {
            context.CustomerTenantAccesses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomerTenantAccess> All()
        {
            return context.CustomerTenantAccesses.ToList();

        }

        public List<CustomerTenantAccess> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public CustomerTenantAccess GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
    }
}
