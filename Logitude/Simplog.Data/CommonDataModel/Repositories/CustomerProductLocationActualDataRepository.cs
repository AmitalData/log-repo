using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CustomerProductLocationActualDataRepository: IRepository<CustomerProductLocationActualData>
    {
        ICommonDataContext commonDataContext;

        public CustomerProductLocationActualDataRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CustomerProductLocationActualDataRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public CustomerProductLocationActualDataRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public IQueryable<CustomerProductLocationActualData> GetCustomerProductLocationActualDatas(int tenant)
        {
            return (from record in context.CustomerProductLocationActualDatas
                    where record.Tenant == tenant 
                    select record);
        }

        public CustomerProductLocationActualData GetSingleCustomerProductLocationActualData(string customerId, string productTypeCode, int month, int year, int tenant)
        {
            return (from a in commonDataContext.CustomerProductLocationActualDatas
                    where a.CustomerId == customerId && a.ProductTypeCode == productTypeCode && a.Month == month && a.Year == year && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public void Add(CustomerProductLocationActualData entity)
        {
            context.CustomerProductLocationActualDatas.Add(entity);
        }

        public void Remove(CustomerProductLocationActualData entity)
        {
            context.CustomerProductLocationActualDatas.Attach(entity);
            context.CustomerProductLocationActualDatas.Remove(entity);
        }

        public void Update(CustomerProductLocationActualData entity)
        {
            try
            {
                context.CustomerProductLocationActualDatas.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<CustomerProductLocationActualData> All()
        {
            return context.CustomerProductLocationActualDatas.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CustomerProductLocationActualData> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CustomerProductLocationActualData GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}