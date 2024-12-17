using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CustomerProductRepository : IRepository<CustomerProduct>
    {
        ICommonDataContext commonDataContext;

        public CustomerProductRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CustomerProductRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public CustomerProductRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public IQueryable<CustomerProduct> GetCustomerProducts(int tenant)
        {
            return (from record in context.CustomerProducts
                    where record.Tenant == tenant 
                    select record);
        }

        public IQueryable<CustomerProduct> GetProductsByCustomerId(string customerId,int tenant)
        {
            return (from d in context.CustomerProducts
                    where d.Tenant == tenant && d.CustomerId == customerId
                    select d);
        }

        public CustomerProduct GetSingleCustomerProduct(string customerId, string productTypeCode, int tenant)
        {
            return (from a in commonDataContext.CustomerProducts
                    where a.CustomerId == customerId && a.ProductTypeCode == productTypeCode && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public void Add(CustomerProduct entity)
        {
            context.CustomerProducts.Add(entity);
        }

        public void Remove(CustomerProduct entity)
        {
            context.CustomerProducts.Attach(entity);
            context.CustomerProducts.Remove(entity);
        }

        public void Update(CustomerProduct entity)
        {
            try
            {
                context.CustomerProducts.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<CustomerProduct> All()
        {
            return context.CustomerProducts.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
        
        public List<CustomerProduct> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CustomerProduct GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}