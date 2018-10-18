using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CustomerStatusRepository:IRepository<CustomerStatus>
    {
        ICommonDataContext commonDataContext;

        public CustomerStatusRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public CustomerStatusRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CustomerStatusRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<CustomerStatus> GetCustomerStatus()
        {
            return context.CustomerStatus;
        }

        public IQueryable<CustomerStatus> GetAll()
        {
            return context.CustomerStatus;
        }

        public CustomerStatus GetSingleCustomerStatus(string code)
        {
            return (from a in context.CustomerStatus
                    where a.Code == code
                    select a).FirstOrDefault();
        }

        public void Add(CustomerStatus entity)
        {
            commonDataContext.CustomerStatus.Add(entity);
        }

        public void Remove(CustomerStatus entity)
        {
            commonDataContext.CustomerStatus.Remove(entity);
        }

        public void Update(CustomerStatus entity)
        {
            commonDataContext.CustomerStatus.Attach(entity);
            commonDataContext.SetAsModified(entity);
        }

        public List<CustomerStatus> All()
        {
            return context.CustomerStatus.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            commonDataContext.SaveChanges();
        }

        public List<CustomerStatus> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public CustomerStatus GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}