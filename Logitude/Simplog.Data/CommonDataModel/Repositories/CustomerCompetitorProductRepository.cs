using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CustomerCompetitorProductRepository : IRepository<CustomerCompetitorProduct>
    {
        ICommonDataContext commonDataContext;

        public CustomerCompetitorProductRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CustomerCompetitorProductRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public CustomerCompetitorProductRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public IQueryable<CustomerCompetitorProduct> GetAllCustomerCompetitorProducts(int tenant)
        {
            return (from record in context.CustomerCompetitorProducts
                    where record.Tenant == tenant
                    select record);
        }

        public IQueryable<CustomerCompetitorProduct> GetCustomerCompetitorProducts(string customerId, string competitorId, int tenant)
        {
            return (from a in context.CustomerCompetitorProducts
                    where a.Tenant == tenant && a.CompetitorId == competitorId && a.CustomerId == customerId
                    select a);
        }

        public CustomerCompetitorProduct GetSingleCustomerCompetitorProduct(string customerId, string competitorId, string code, int tenant)
        {
            return (from a in commonDataContext.CustomerCompetitorProducts
                    where a.CustomerId == customerId && a.CompetitorId == competitorId && a.ProductTypeCode == code && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public void Add(CustomerCompetitorProduct entity)
        {
            context.CustomerCompetitorProducts.Add(entity);
        }

        public void Remove(CustomerCompetitorProduct entity)
        {
            context.CustomerCompetitorProducts.Attach(entity);
            context.CustomerCompetitorProducts.Remove(entity);
        }

        public void Update(CustomerCompetitorProduct entity)
        {
            try
            {
                context.CustomerCompetitorProducts.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<CustomerCompetitorProduct> All()
        {
            return context.CustomerCompetitorProducts.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CustomerCompetitorProduct> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CustomerCompetitorProduct GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}