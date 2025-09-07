using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CustomerProductLocationRepository: IRepository<CustomerProductLocation>
    {
        ICommonDataContext commonDataContext;

        public CustomerProductLocationRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CustomerProductLocationRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }



        public IQueryable<CustomerProductLocation> GetProductLocationsByCustomerId(string customerId, int tenant)
        {
            return (from d in context.CustomerProductLocations
                    where d.Tenant == tenant
                    && d.CustomerId == customerId
                    select d);
        }

        public IQueryable<CustomerProductLocation> GetCustomerProductLocations( int tenant)
        {
            return (from d in context.CustomerProductLocations
                    where d.Tenant == tenant
                   
                    select d);
        }

        public IQueryable<CustomerProductLocation> GetCustomerProductLocations(string customerId, string productTypeCode, int tenant)
        {
            return (from d in context.CustomerProductLocations
                    where d.Tenant == tenant
                    && d.CustomerId == customerId
                    && d.ProductTypeCode == productTypeCode
                    select d);
        }

        public CustomerProductLocation GetSingleCustomerProductLocation(string customerId, string productTypeCode, string countryId, int tenant)
        {
            return (from a in commonDataContext.CustomerProductLocations
                    where a.CustomerId == customerId && a.ProductTypeCode == productTypeCode && a.CountryId == countryId && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public void Add(CustomerProductLocation entity)
        {
            context.CustomerProductLocations.Add(entity);
        }

        public void Remove(CustomerProductLocation entity)
        {
            context.CustomerProductLocations.Attach(entity);
            context.CustomerProductLocations.Remove(entity);
        }

        public void Update(CustomerProductLocation entity)
        {
            try
            {
                context.CustomerProductLocations.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<CustomerProductLocation> All()
        {
            return context.CustomerProductLocations.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CustomerProductLocation> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CustomerProductLocation GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}