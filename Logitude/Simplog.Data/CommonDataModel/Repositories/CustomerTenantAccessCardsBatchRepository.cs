using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CustomerTenantAccessCardsBatchRepository : IRepository<CustomerTenantAccessCardsBatch>
    {
          ICommonDataContext commonDataContext;

        public CustomerTenantAccessCardsBatchRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CustomerTenantAccessCardsBatchRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public IQueryable<CustomerTenantAccessCardsBatch> GetCustomerTenantAccessCradsBatchByTenant(int tenant)
        {
            return from a in context.CustomerTenantAccessCardsBatches where a.Tenant == tenant select a;
        } 

        public CustomerTenantAccessCardsBatch GetSingleCustomerTenantAccessCardsBatch(string CustomerTenantAccessId, string CustomerId, string BatchNumber,int Tenant)
        {
            return (from record in context.CustomerTenantAccessCardsBatches where record.CustomerTenantAccessId == CustomerTenantAccessId && record.CustomerId == CustomerId && record.BatchNumber == BatchNumber && record.Tenant == Tenant select record).FirstOrDefault();
        }

        public IQueryable<CustomerTenantAccessCardsBatch> GetCustomerTenantAccessCardsBatches(int tenant)
        {
            return from a in context.CustomerTenantAccessCardsBatches where a.Tenant == tenant select a;
        }

        


        public CustomerTenantAccessCardsBatch GetSingleCustomerTenantAccessCard1sBatch( string BatchNumber, int Tenant)
        {
            return (from record in context.CustomerTenantAccessCardsBatches where record.BatchNumber == BatchNumber && record.Tenant == Tenant select record).FirstOrDefault();
        }
        public CustomerTenantAccessCardsBatchRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void Add(CustomerTenantAccessCardsBatch entity)
        {
            context.CustomerTenantAccessCardsBatches.Add(entity);
        }

        public void Remove(CustomerTenantAccessCardsBatch entity)
        {
            context.CustomerTenantAccessCardsBatches.Attach(entity);
            context.CustomerTenantAccessCardsBatches.Remove(entity);
        }

        public void Update(CustomerTenantAccessCardsBatch entity)
        {
            context.CustomerTenantAccessCardsBatches.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomerTenantAccessCardsBatch> All()
        {
            return context.CustomerTenantAccessCardsBatches.ToList();

        }

        public List<CustomerTenantAccessCardsBatch> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public CustomerTenantAccessCardsBatch GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
    }
}
