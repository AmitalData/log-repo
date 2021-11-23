using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
   public class ProductTypeModificationRepository  :IRepository<ProductTypeModification>
    {
        ICommonDataContext commonDataContext;
        public ProductTypeModificationRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public ProductTypeModificationRepository()
        {
            commonDataContext=new CommonDataContext();
        }

        public ProductTypeModificationRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public void Add(ProductTypeModification entity)
        {
            context.ProductTypeModifications.Add(entity);
        }

        public void Remove(ProductTypeModification entity)
        {
            try
            {
                context.ProductTypeModifications.Attach(entity);
            }
            catch { };
            context.ProductTypeModifications.Remove(entity);
        }

        public void Update(ProductTypeModification entity)
        {
            try
            {
                context.ProductTypeModifications.Attach(entity);
            }
            catch { };
            context.SetAsModified(entity);
        }

        public List<ProductTypeModification> All()
        {
            return context.ProductTypeModifications.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public ProductTypeModification GetSingleProductTypeModification(string code,int tenant)
        {
            return (from a in context.ProductTypeModifications
                    where a.ProductTypeCode == code && a.Tenant==tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ProductTypeModification> GetProductTypeModificationsForTenant(int tenant)
        {
            return (from a in context.ProductTypeModifications
                    where a.Tenant == tenant
                    select a);
        }

        public List<ProductTypeModification> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ProductTypeModification GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
        public ProductTypeModification GetSingle(string productTypeCode)
        {
            return (from a in context.ProductTypeModifications
                    where a.ProductTypeCode == productTypeCode
                    select a).FirstOrDefault() ;
        }
    }
}


