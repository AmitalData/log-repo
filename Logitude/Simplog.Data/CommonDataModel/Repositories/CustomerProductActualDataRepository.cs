using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CustomerProductActualDataRepository: IRepository<CustomerProductActualData>
    {
        ICommonDataContext commonDataContext;

        public CustomerProductActualDataRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CustomerProductActualDataRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public CustomerProductActualDataRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public IQueryable<CustomerProductActualData> GetCustomerProductActualDatas(int tenant)
        {
            return (from d in context.CustomerProductActualDatas.Include("Customer").Include("Customer.SalesmanUser").Include("Customer.Card")
                    where d.Tenant == tenant select d);
        }

        public CustomerProductActualData GetSingleCustomerProductActualData(string customerId, string productTypeCode, int month, int year, int tenant)
        {
            return (from d in commonDataContext.CustomerProductActualDatas.Include("Customer").Include("Customer.SalesmanUser").Include("Customer.Card")
                    where d.CustomerId == customerId
                    && d.ProductTypeCode == productTypeCode 
                    && d.Month == month
                    && d.Year == year
                    && d.Tenant == tenant
                    select d).FirstOrDefault();
        }

        public void Add(CustomerProductActualData entity)
        {
            context.CustomerProductActualDatas.Add(entity);
        }

        public void Remove(CustomerProductActualData entity)
        {
            context.CustomerProductActualDatas.Attach(entity);
            context.CustomerProductActualDatas.Remove(entity);
        }

        public void Update(CustomerProductActualData entity)
        {
            try
            {
                context.CustomerProductActualDatas.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<CustomerProductActualData> All()
        {
            return context.CustomerProductActualDatas.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CustomerProductActualData> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CustomerProductActualData GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

    }
}