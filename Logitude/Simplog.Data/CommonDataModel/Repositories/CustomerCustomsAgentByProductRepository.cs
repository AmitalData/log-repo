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
   public class CustomerCustomsAgentByProductRepository: IRepository<CustomerCustomsAgentByProduct>
    {

          ICommonDataContext commonDataContext;

        public CustomerCustomsAgentByProductRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CustomerCustomsAgentByProductRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public CustomerCustomsAgentByProductRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        //public int GetCustomerCustomsAgentByProductCount(int tenant)
        //{
        //    return (from d in context.CustomerCustomsAgentByProducts where d.Tenant == tenant select d).Count();
        //}

        public IQueryable<CustomerCustomsAgentByProduct> GetCustomerCustomsAgentByProduct(int tenant)
        {
            return (from d in context.CustomerCustomsAgentByProducts.Include("CustomsAgentCard") where d.Tenant == tenant select d);
        }

        public IQueryable<CustomerCustomsAgentByProduct> GetCustomerCustomsAgentByProductforCustomer(int tenant, string customerId)
        {
            return (from d in context.CustomerCustomsAgentByProducts.Include("CustomsAgentCard") where d.Tenant == tenant && d.CustomerId == customerId select d);
        }


        public CustomerCustomsAgentByProduct GetSingleCustomerCustomsAgentByProduct(string productTypeCode, string customerId,int tenant)
        {
            return (from d in context.CustomerCustomsAgentByProducts.Include("CustomsAgentCard") where d.ProductTypeCode == productTypeCode && d.CustomerId == customerId && d.Tenant == tenant select d).FirstOrDefault();
        }

        public void Add(CustomerCustomsAgentByProduct entity)
        {
            context.CustomerCustomsAgentByProducts.Add(entity);
        }

        public void Remove(CustomerCustomsAgentByProduct entity)
        {
            try
            {
                context.CustomerCustomsAgentByProducts.Attach(entity);
            }

            catch
            {

            }

            context.CustomerCustomsAgentByProducts.Remove(entity);
        }

        public void Update(CustomerCustomsAgentByProduct entity)
        {
            try
            {
                context.CustomerCustomsAgentByProducts.Attach(entity);
            }

            catch
            {

            }

            context.SetAsModified(entity);
        }

        public List<CustomerCustomsAgentByProduct> All()
        {
            return context.CustomerCustomsAgentByProducts.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CustomerCustomsAgentByProduct> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CustomerCustomsAgentByProduct GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

    }
}
