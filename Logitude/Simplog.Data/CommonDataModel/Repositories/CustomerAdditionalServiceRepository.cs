using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CustomerAdditionalServiceRepository : IRepository<CustomerAdditionalService>
    {
        ICommonDataContext commonDataContext;



        public CustomerAdditionalServiceRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public CustomerAdditionalServiceRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }
         

        public CustomerAdditionalService GetSingleCustomerAdditionalService(string customerId, string serviceId, int tenant)
        {
            return (from d in context.CustomerAdditionalServices where d.Tenant == tenant && d.CustomerId == customerId && d.AdditionalServiceId == serviceId select d).FirstOrDefault();
        }
        public CustomerAdditionalService GetSingleAdditionalService(string customerId, string serviceId, int tenant)
        {
            return (from d in context.CustomerAdditionalServices where d.Tenant == tenant && d.CustomerId == customerId && d.AdditionalServiceId == serviceId select d).FirstOrDefault();
        }

        public IQueryable<CustomerAdditionalService> GetAdditionalServicesByCustomerId(string customerId, int tenant)
        {
            return (from d in context.CustomerAdditionalServices where d.Tenant == tenant && d.CustomerId == customerId select d);
        }

        public IQueryable<CustomerAdditionalService> GetCustomerAdditionalServices(int tenant)
        {
            return (from d in context.CustomerAdditionalServices where d.Tenant == tenant select d);
        }

        

        public void Add(CustomerAdditionalService entity)
        {
            context.CustomerAdditionalServices.Add(entity);
        }

        public void Remove(CustomerAdditionalService entity)
        {
            context.CustomerAdditionalServices.Attach(entity);
            context.CustomerAdditionalServices.Remove(entity);
        }

        public void Update(CustomerAdditionalService entity)
        {
            try
            {
                context.CustomerAdditionalServices.Attach(entity);
            }
            catch
            {
            }

            context.SetAsModified(entity);
        }

        public List<CustomerAdditionalService> All()
        {
            return context.CustomerAdditionalServices.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CustomerAdditionalService> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CustomerAdditionalService GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public IQueryable<CustomerAdditionalService> GetAdditionalServicesByTenant(int tenant)
        {
            return (from d in context.CustomerAdditionalServices where d.Tenant == tenant select d);
        }
    }
}
