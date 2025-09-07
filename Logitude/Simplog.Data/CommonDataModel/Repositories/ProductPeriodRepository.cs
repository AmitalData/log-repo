using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class ProductPeriodRepository : IRepository<ProductPeriod>
    {
        ICommonDataContext commonDataContext;

        public ProductPeriodRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public ProductPeriodRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

 

        public IQueryable<ProductPeriod> GetProductPeriods()
        {
            return context.ProductPeriods;
        }
        public IQueryable<ProductPeriod> GetAll()
        {
            return context.ProductPeriods;
        }

        public ProductPeriod GetSingleProductPeriod(string code)
        {
            return (from a in context.ProductPeriods
                    where a.Code == code
                    select a).FirstOrDefault();
        }

        public void Add(ProductPeriod entity)
        {
            commonDataContext.ProductPeriods.Add(entity);
        }

        public void Remove(ProductPeriod entity)
        {
            commonDataContext.ProductPeriods.Remove(entity);
        }

        public void Update(ProductPeriod entity)
        {
            commonDataContext.ProductPeriods.Attach(entity);
            commonDataContext.SetAsModified(entity);
        }

        public List<ProductPeriod> All()
        {
            return context.ProductPeriods.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            commonDataContext.SaveChanges();
        }

        public List<ProductPeriod> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ProductPeriod GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}