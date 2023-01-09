using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CustomerSizeRepository : IRepository<CustomerSize>
    {
        ICommonDataContext commonDataContext;

        public CustomerSizeRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public CustomerSizeRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CustomerSizeRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<CustomerSize> GetCustomerSizes(int tenant)
        {
            return (from record in context.CustomerSizes where record.Tenant == tenant select record);
        }

        public CustomerSize GetSingleCustomerSize(string id, int tenant)
        {
            return (from record in context.CustomerSizes where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public CustomerSize GetSingleCustomerSizeByCode(string code, int tenant)
        {
            return (from record in context.CustomerSizes where record.Code == code && record.Tenant == tenant select record).FirstOrDefault();
        }
        public void Add(CustomerSize entity)
        {
            context.CustomerSizes.Add(entity);
        }

        public void Remove(CustomerSize entity)
        {
            context.CustomerSizes.Remove(entity);
        }

        public void Update(CustomerSize entity)
        {
            context.CustomerSizes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomerSize> All()
        {
            return context.CustomerSizes.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CustomerSize> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public CustomerSize GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}