using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CustomerTenantAccessCardRepository : IRepository<CustomerTenantAccessCard>
    {
         ICommonDataContext commonDataContext;

        public CustomerTenantAccessCardRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CustomerTenantAccessCardRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public CustomerTenantAccessCard GetSingleCustomerTenantAccessCard(string CustomerTenantAccessId,string CustomerId , int Tenant)
        {
            return (from record in context.CustomerTenantAccessCards.Include("CreateByUser.Contact").Include("CreateByUser").Include("StatusType") where record.CustomerTenantAccessId == CustomerTenantAccessId && record.CustomerId == CustomerId && record.Tenant == Tenant select record).FirstOrDefault();
        }

        public IQueryable<CustomerTenantAccessCard> GetAllCustomerTenantAccessCardByCustomerTenantAccessId(string CustomerTenantAccessId, int Tenant)
        {
            return (from record in context.CustomerTenantAccessCards where record.CustomerTenantAccessId == CustomerTenantAccessId && record.Tenant == Tenant select record);
        }

        public IQueryable<CustomerTenantAccessCard> GetAllCustomerTenantAccessCardByTenant(int Tenant)
        {
            return (from record in context.CustomerTenantAccessCards.Include("Customer").Include("Customer.Card")
                    where record.Tenant == Tenant
                    select record);
        }

        public CustomerTenantAccessCardRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void Add(CustomerTenantAccessCard entity)
        {
            context.CustomerTenantAccessCards.Add(entity);
        }

        public void Remove(CustomerTenantAccessCard entity)
        {
            context.CustomerTenantAccessCards.Attach(entity);
            context.CustomerTenantAccessCards.Remove(entity);
        }

        public void Update(CustomerTenantAccessCard entity)
        {
            context.CustomerTenantAccessCards.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomerTenantAccessCard> All()
        {
            return context.CustomerTenantAccessCards.ToList();

        }


        public List<CustomerTenantAccessCard> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public CustomerTenantAccessCard GetByCustomerId(string customerId, int tenant)
        {
            return (from record in context.CustomerTenantAccessCards where record.CustomerId == customerId && record.StatusTypeCode.ToUpper() != "IA" && record.Tenant == tenant select record).FirstOrDefault();

 
        }
 

        public CustomerTenantAccessCard GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }


        public CustomerTenantAccessCard GetCustomerById(string customerId, int tenant)
        {
            return (from record in context.CustomerTenantAccessCards where record.CustomerId == customerId && record.Tenant == tenant select record).FirstOrDefault();


        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
    }
}
