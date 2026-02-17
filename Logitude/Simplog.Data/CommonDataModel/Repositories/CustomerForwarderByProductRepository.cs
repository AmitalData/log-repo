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
   public class CustomerForwarderByProductRepository: IRepository<CustomerForwarderByProduct>
    {

          ICommonDataContext commonDataContext;

        public CustomerForwarderByProductRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CustomerForwarderByProductRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public CustomerForwarderByProductRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        //public int GetCustomerForwarderByProductCount(int tenant)
        //{
        //    return (from d in context.CustomerForwarderByProducts where d.Tenant == tenant select d).Count();
        //}

        public IQueryable<CustomerForwarderByProduct> GetCustomerForwarderByProduct(int tenant)
        {
            return (from d in context.CustomerForwarderByProducts.Include("ForwarderCard") where d.Tenant == tenant select d);
        }

        public IQueryable<CustomerForwarderByProduct> GetCustomerForwarderByProductforCustomer(int tenant, string customerId)
        {
            return (from d in context.CustomerForwarderByProducts.Include("ForwarderCard") where d.Tenant == tenant && d.CustomerId == customerId select d);
        }


        public CustomerForwarderByProduct GetSingleCustomerForwarderByProduct(string productTypeCode, string customerId,int tenant)
        {
            return (from d in context.CustomerForwarderByProducts.Include("ForwarderCard") where d.ProductTypeCode == productTypeCode && d.CustomerId == customerId && d.Tenant == tenant select d).FirstOrDefault();
        }

        public void Add(CustomerForwarderByProduct entity)
        {
            context.CustomerForwarderByProducts.Add(entity);
        }

        public void Remove(CustomerForwarderByProduct entity)
        {
            try
            {
                context.CustomerForwarderByProducts.Attach(entity);
            }

            catch
            {

            }

            context.CustomerForwarderByProducts.Remove(entity);
        }

        public void Update(CustomerForwarderByProduct entity)
        {
            try
            {
                context.CustomerForwarderByProducts.Attach(entity);
            }

            catch
            {

            }

            context.SetAsModified(entity);
        }

        public List<CustomerForwarderByProduct> All()
        {
            return context.CustomerForwarderByProducts.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CustomerForwarderByProduct> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CustomerForwarderByProduct GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
   }
}
