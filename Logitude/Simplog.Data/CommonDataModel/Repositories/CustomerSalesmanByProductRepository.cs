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
    public class CustomerSalesmanByProductRepository : IRepository<CustomerSalesmanByProduct>
    {

          ICommonDataContext commonDataContext;

        public CustomerSalesmanByProductRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CustomerSalesmanByProductRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public CustomerSalesmanByProductRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        //public int GetCustomerSalesmanByProductCount(int tenant)
        //{
        //    return (from d in context.CustomerSalesmanByProducts where d.Tenant == tenant select d).Count();
        //}

        public IQueryable<CustomerSalesmanByProduct> GetCustomerSalesmanByProduct(int tenant)
        {
            return (from d in context.CustomerSalesmanByProducts.Include("SalesmanUser.Contact") where d.Tenant == tenant select d);
        }

        public IQueryable<CustomerSalesmanByProduct> GetCustomerSalesmanByProductforCustomer(int tenant, string customerId)
        {
            return (from d in context.CustomerSalesmanByProducts.Include("SalesmanUser.Contact") where d.Tenant == tenant && d.CustomerId == customerId select d);
        }


        public CustomerSalesmanByProduct GetSingleCustomerSalesmanByProduct(string productTypeCode, string customerId,int tenant)
        {
            return (from d in context.CustomerSalesmanByProducts.Include("SalesmanUser.Contact") where d.ProductTypeCode == productTypeCode && d.CustomerId == customerId && d.Tenant == tenant select d).FirstOrDefault();
        }

        public void Add(CustomerSalesmanByProduct entity)
        {
            context.CustomerSalesmanByProducts.Add(entity);
        }

        public void Remove(CustomerSalesmanByProduct entity)
        {
            try
            {
                context.CustomerSalesmanByProducts.Attach(entity);
            }

            catch
            {

            }

            context.CustomerSalesmanByProducts.Remove(entity);
        }

        public void Update(CustomerSalesmanByProduct entity)
        {
            try
            {
                context.CustomerSalesmanByProducts.Attach(entity);
            }

            catch
            {

            }

            context.SetAsModified(entity);
        }

        public List<CustomerSalesmanByProduct> All()
        {
            return context.CustomerSalesmanByProducts.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CustomerSalesmanByProduct> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CustomerSalesmanByProduct GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

    }
}
