using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
   public class CustomerFreelancerByProductRepository:IRepository<CustomerFreelancerByProduct>
    {
        ICommonDataContext commonDataContext;

        public CustomerFreelancerByProductRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CustomerFreelancerByProductRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public CustomerFreelancerByProductRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public int GetCustomerFreelancerByProductCount(int tenant)
        {
            return (from d in context.CustomerFreelancerByProducts where d.Tenant == tenant select d).Count();
        }

        public IQueryable<CustomerFreelancerByProduct> GetCustomerFreelancerByProduct(int tenant)
        {
            return (from d in context.CustomerFreelancerByProducts where d.Tenant == tenant select d);
        }

        public CustomerFreelancerByProduct GetSingleCustomerFreelancerByProduct(string productTypeCode, string customerId,int tenant)
        {
            return (from d in context.CustomerFreelancerByProducts where d.ProductTypeCode == productTypeCode && d.CustomerId == customerId && d.Tenant == tenant select d).FirstOrDefault();
        }

        public void Add(CustomerFreelancerByProduct entity)
        {
            context.CustomerFreelancerByProducts.Add(entity);
        }

        public void Remove(CustomerFreelancerByProduct entity)
        {
            try
            {
                context.CustomerFreelancerByProducts.Attach(entity);
            }

            catch
            {

            }

            context.CustomerFreelancerByProducts.Remove(entity);
        }

        public void Update(CustomerFreelancerByProduct entity)
        {
            try
            {
                context.CustomerFreelancerByProducts.Attach(entity);
            }

            catch
            {

            }

            context.SetAsModified(entity);
        }

        public List<CustomerFreelancerByProduct> All()
        {
            return context.CustomerFreelancerByProducts.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CustomerFreelancerByProduct> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CustomerFreelancerByProduct GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
   
}
