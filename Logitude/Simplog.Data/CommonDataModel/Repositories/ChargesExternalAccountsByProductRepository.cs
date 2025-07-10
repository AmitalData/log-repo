using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class ChargesExternalAccountsByProductRepository : IRepository<ChargesExternalAccountsByProduct>
    {
        ICommonDataContext commonDataContext;
        public ChargesExternalAccountsByProductRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }
        public ChargesExternalAccountsByProductRepository()
        {
            commonDataContext = new CommonDataContext();

        }
        public ChargesExternalAccountsByProductRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<ChargesExternalAccountsByProduct> GetChargesExternalAccountsByProducts(int tenant)
        {
            return (from d in context.ChargesExternalAccountsByProducts where d.Tenant == tenant select d);
        }
        public IQueryable<ChargesExternalAccountsByProduct> GetChargesExternalAccountsByProductsByChargesTypeId(string myChargesTypeId, int tenant)
        {
            return (from d in context.ChargesExternalAccountsByProducts where d.Tenant == tenant && d.ChargesTypeId == myChargesTypeId select d);
        }

        public ChargesExternalAccountsByProduct GetSingleChargesExternalAccountsByProduct(string id, int tenant)
        {
            return (from d in context.ChargesExternalAccountsByProducts where d.Id == id && d.Tenant == tenant select d).FirstOrDefault();
        }

        public ChargesExternalAccountsByProduct GetSingle(string myChargesTypeId, string myProductTypeCode, int tenant)
        {
            return (from d in context.ChargesExternalAccountsByProducts where d.ChargesTypeId == myChargesTypeId && d.ProductTypeCode == myProductTypeCode && d.Tenant == tenant select d).FirstOrDefault();
        }

        public void Add(ChargesExternalAccountsByProduct entity)
        {
            context.ChargesExternalAccountsByProducts.Add(entity);
        }

        public void Remove(ChargesExternalAccountsByProduct entity)
        {
            context.ChargesExternalAccountsByProducts.Attach(entity);
            context.ChargesExternalAccountsByProducts.Remove(entity);
        }

        public void Update(ChargesExternalAccountsByProduct entity)
        {
            context.ChargesExternalAccountsByProducts.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ChargesExternalAccountsByProduct> All()
        {
            return context.ChargesExternalAccountsByProducts.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ChargesExternalAccountsByProduct> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ChargesExternalAccountsByProduct GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }

}
