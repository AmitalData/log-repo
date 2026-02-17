using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CardContactProductRepository : IRepository<CardContactProduct>
    {
        ICommonDataContext commonDataContext;

        public CardContactProductRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CardContactProductRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public CardContactProductRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public IQueryable<CardContactProduct> GetCardContactProducts(int tenant)
        {
            return (from record in context.CardContactProducts
                    where record.Tenant == tenant
                    select record);
        }

        public IQueryable<CardContactProduct> GetProductsByCardContactIdd(string cardContactId, int tenant)
        {
            return (from d in context.CardContactProducts.Include("ProductType")
                    where d.Tenant == tenant && d.CardContactId == cardContactId
                    select d);
        }

        public CardContactProduct GetSingleCardContactProduct(string id, int tenant)
        {
            return (from a in commonDataContext.CardContactProducts
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public void Add(CardContactProduct entity)
        {
            context.CardContactProducts.Add(entity);
        }

        public void Remove(CardContactProduct entity)
        {
            context.CardContactProducts.Attach(entity);
            context.CardContactProducts.Remove(entity);
        }

        public void Update(CardContactProduct entity)
        {
            try
            {
                context.CardContactProducts.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<CardContactProduct> All()
        {
            return context.CardContactProducts.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CardContactProduct> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CardContactProduct GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}