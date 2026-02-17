using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CardExternalAccountsByProductRepository : IRepository<CardExternalAccountsByProduct>
    {
        ICommonDataContext commonDataContext;
        public CardExternalAccountsByProductRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }
        public CardExternalAccountsByProductRepository()
        {
            commonDataContext = new CommonDataContext();

        }
        public CardExternalAccountsByProductRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<CardExternalAccountsByProduct> GetCardExternalAccountsByProducts(int tenant)
        {
            return (from d in context.CardExternalAccountsByProducts where d.Tenant == tenant select d);
        }
        public IQueryable<CardExternalAccountsByProduct> GetCardExternalAccountsByProductsByCardId(string cardId, int tenant)
        {
            return (from d in context.CardExternalAccountsByProducts where d.Tenant == tenant && d.CardId == cardId select d);
        }

        public CardExternalAccountsByProduct GetSingleCardExternalAccountsByProduct(string id, int tenant)
        {
            return (from d in context.CardExternalAccountsByProducts where d.Id == id && d.Tenant == tenant select d).FirstOrDefault();
        }

        public CardExternalAccountsByProduct GetSingle(string myCardId, string myProductTypeCode, int tenant)
        {
            return (from d in context.CardExternalAccountsByProducts where d.CardId == myCardId && d.ProductTypeCode == myProductTypeCode && d.Tenant == tenant select d).FirstOrDefault();
        }

        public void Add(CardExternalAccountsByProduct entity)
        {
            context.CardExternalAccountsByProducts.Add(entity);
        }

        public void Remove(CardExternalAccountsByProduct entity)
        {
            context.CardExternalAccountsByProducts.Attach(entity);
            context.CardExternalAccountsByProducts.Remove(entity);
        }

        public void Update(CardExternalAccountsByProduct entity)
        {
            context.CardExternalAccountsByProducts.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CardExternalAccountsByProduct> All()
        {
            return context.CardExternalAccountsByProducts.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
      
        public List<CardExternalAccountsByProduct> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public CardExternalAccountsByProduct GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
