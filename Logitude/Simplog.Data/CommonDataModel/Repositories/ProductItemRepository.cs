using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public  class ProductItemRepository
    {

        ICommonDataContext commonDataContext;

        public ProductItemRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public ProductItemRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }



        public IQueryable<ProductItem> GetProductItems(int tenant)
        {
            return context.ProductItems;
        }
        public IQueryable<ProductItem> GetAll()
        {
            return context.ProductItems;
        }

        public ProductItem GetSingleProductItem(string id,int tenant)
        {
            return (from a in context.ProductItems.Include("OriginCountry").Include("Shipper").Include("ProductValueCurrency")
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public void Add(ProductItem entity)
        {
            commonDataContext.ProductItems.Add(entity);
        }

        public void Remove(ProductItem entity)
        {
            commonDataContext.ProductItems.Remove(entity);
        }

        public void Update(ProductItem entity)
        {
            commonDataContext.ProductItems.Attach(entity);
            commonDataContext.SetAsModified(entity);
        }

        public List<ProductItem> All()
        {
            return context.ProductItems.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            commonDataContext.SaveChanges();
        }

        public List<ProductItem> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ProductItem GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
