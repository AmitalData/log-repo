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
   public class CustomerMediatorByProductRepository: IRepository<CustomerMediatorByProduct>
    {

          ICommonDataContext commonDataContext;

        public CustomerMediatorByProductRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CustomerMediatorByProductRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public CustomerMediatorByProductRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        //public int GetCustomerMediatorByProductCount(int tenant)
        //{
        //    return (from d in context.CustomerMediatorByProducts where d.Tenant == tenant select d).Count();
        //}

        public IQueryable<CustomerMediatorByProduct> GetCustomerMediatorByProduct(int tenant)
        {
            return (from d in context.CustomerMediatorByProducts.Include("MediatorCard") where d.Tenant == tenant select d);
        }

        public IQueryable<CustomerMediatorByProduct> GetCustomerMediatorByProductforCustomer(int tenant, string customerId)
        {
            return (from d in context.CustomerMediatorByProducts.Include("MediatorCard") where d.Tenant == tenant && d.CustomerId == customerId select d);
        }


        public CustomerMediatorByProduct GetSingleCustomerMediatorByProduct(string productTypeCode, string customerId,int tenant)
        {
            return (from d in context.CustomerMediatorByProducts.Include("MediatorCard") where d.ProductTypeCode == productTypeCode && d.CustomerId == customerId && d.Tenant == tenant select d).FirstOrDefault();
        }

        public void Add(CustomerMediatorByProduct entity)
        {
            context.CustomerMediatorByProducts.Add(entity);
        }

        public void Remove(CustomerMediatorByProduct entity)
        {
            try
            {
                context.CustomerMediatorByProducts.Attach(entity);
            }

            catch
            {

            }

            context.CustomerMediatorByProducts.Remove(entity);
        }

        public void Update(CustomerMediatorByProduct entity)
        {
            try
            {
                context.CustomerMediatorByProducts.Attach(entity);
            }

            catch
            {

            }

            context.SetAsModified(entity);
        }

        public List<CustomerMediatorByProduct> All()
        {
            return context.CustomerMediatorByProducts.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CustomerMediatorByProduct> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CustomerMediatorByProduct GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

    }
}
