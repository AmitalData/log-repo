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
   public class CustomerAccountManagerByProductRepository: IRepository<CustomerAccountManagerByProduct>
    {

          ICommonDataContext commonDataContext;

        public CustomerAccountManagerByProductRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CustomerAccountManagerByProductRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public CustomerAccountManagerByProductRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        //public int GetCustomerAccountManagerByProductCount(int tenant)
        //{
        //    return (from d in context.CustomerAccountManagerByProducts where d.Tenant == tenant select d).Count();
        //}

        public IQueryable<CustomerAccountManagerByProduct> GetCustomerAccountManagerByProduct(int tenant)
        {
            return (from d in context.CustomerAccountManagerByProducts.Include("AccountManagerUser.Contact") where d.Tenant == tenant select d);
        }

        public IQueryable<CustomerAccountManagerByProduct> GetCustomerAccountManagerByProductforCustomer(int tenant, string customerId)
        {
            return (from d in context.CustomerAccountManagerByProducts.Include("AccountManagerUser.Contact") where d.Tenant == tenant && d.CustomerId == customerId select d);
        }


        public CustomerAccountManagerByProduct GetSingleCustomerAccountManagerByProduct(string productTypeCode, string customerId,int tenant)
        {
            return (from d in context.CustomerAccountManagerByProducts.Include("AccountManagerUser.Contact") where d.ProductTypeCode == productTypeCode && d.CustomerId == customerId && d.Tenant == tenant select d).FirstOrDefault();
        }

        public void Add(CustomerAccountManagerByProduct entity)
        {
            context.CustomerAccountManagerByProducts.Add(entity);
        }

        public void Remove(CustomerAccountManagerByProduct entity)
        {
            try
            {
                context.CustomerAccountManagerByProducts.Attach(entity);
            }

            catch
            {

            }

            context.CustomerAccountManagerByProducts.Remove(entity);
        }

        public void Update(CustomerAccountManagerByProduct entity)
        {
            try
            {
                context.CustomerAccountManagerByProducts.Attach(entity);
            }

            catch
            {

            }

            context.SetAsModified(entity);
        }

        public List<CustomerAccountManagerByProduct> All()
        {
            return context.CustomerAccountManagerByProducts.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CustomerAccountManagerByProduct> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CustomerAccountManagerByProduct GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

    }
}
