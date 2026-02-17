using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class ProductTypeRepository : IRepository<ProductType>
    {
        ICommonDataContext commonDataContext;

        public ProductTypeRepository(ICommonDataContext context)
        {
            commonDataContext = context;
             
        }

        public ProductTypeRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public ProductTypeRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public IQueryable<ProductType> GetProductTypes()
        {
            return context.ProductTypes;
        }
        public IQueryable<ProductType> GetProductTypes(int tenant)
        {
            return context.ProductTypes;
        }
        public IQueryable<ProductType> GetActiveProductTypes(int tenant)
        {
            List<string> prodctTypeCodes=(from a in context.ProductTypeModifications
                                          where a.InActive && a.Tenant==tenant
                                          select a.ProductTypeCode).ToList();

            IQueryable<ProductType> productTypes = (from a in context.ProductTypes
                                                    where !prodctTypeCodes.Contains(a.Code)
                                                    select a);

            return productTypes;
        }

        public IQueryable<ProductTypeModificationView> GetProductViewsByTenant(int tenant)
        {
            IProductDataViewContext dataViewEntities = ProductDataViewContext.GetContext(tenant);
            IQueryable<ProductTypeModificationView> views = from a in dataViewEntities.ProductDataViews
                   where a.Tenant == tenant
                   select a;
            return views;
        }

        public ProductType GetSingleProductType(string code)
        {
            return (from a in context.ProductTypes
                    where a.Code == code
                    select a).FirstOrDefault();
        }

        public ProductType GetSingleProductType(string code, int tenant)
        {
            return (from a in context.ProductTypes
                    where a.Code == code
                    select a).FirstOrDefault();
        }

        public IQueryable<ProductTypeModification> GetProductTypeModificationsForTenant(int tenant)
        {
            return (from a in context.ProductTypeModifications
                    where a.Tenant == tenant
                    select a);
        }

        public void Add(ProductType entity)
        {
            commonDataContext.ProductTypes.Add(entity);
        }

        public void Remove(ProductType entity)
        {
            commonDataContext.ProductTypes.Remove(entity);
        }

        public void Update(ProductType entity)
        {
            commonDataContext.ProductTypes.Attach(entity);
            commonDataContext.SetAsModified(entity);
        }

        public List<ProductType> All()
        {
            return context.ProductTypes.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            commonDataContext.SaveChanges();
        }

        public List<ProductType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ProductType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}