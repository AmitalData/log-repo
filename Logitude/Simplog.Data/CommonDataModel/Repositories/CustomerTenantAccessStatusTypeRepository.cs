using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CustomerTenantAccessStatusTypeRepository : IRepository<CustomerTenantAccessStatusType>
    {
         ICommonDataContext commonDataContext;

        public CustomerTenantAccessStatusTypeRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CustomerTenantAccessStatusTypeRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public CustomerTenantAccessStatusTypeRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<CustomerTenantAccessStatusType> GetCustomerTenantAccessStatusTypes()
        {
            return context.CustomerTenantAccessStatusTypes;
        }

        public CustomerTenantAccessStatusType GetSingleCustomerTenantAccessStatusType(string code)
        {
            return (from a in context.CustomerTenantAccessStatusTypes where a.Code == code select a).FirstOrDefault();
        }

        public void Add(CustomerTenantAccessStatusType entity)
        {
            context.CustomerTenantAccessStatusTypes.Add(entity);
        }

        public void Remove(CustomerTenantAccessStatusType entity)
        {
            context.CustomerTenantAccessStatusTypes.Attach(entity);
            context.CustomerTenantAccessStatusTypes.Remove(entity);
        }

        public void Update(CustomerTenantAccessStatusType entity)
        {
            context.CustomerTenantAccessStatusTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomerTenantAccessStatusType> All()
        {
            return context.CustomerTenantAccessStatusTypes.ToList();

        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<CustomerTenantAccessStatusType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public CustomerTenantAccessStatusType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
