using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System.Transactions;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Repositories
{
    
    public class CardSearchRepository : IRepository<CardSearch>
    {
        ICommonDataContext commonDataContext;

        public CardSearchRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public CardSearchRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CardSearchRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<CardSearch> GetCardSearchs(int tenant)
        {
            return (from record in context.CardSearchs where record.Tenant == tenant select record);
        }

        public CardSearch GetSingleCardSearch(string id, int tenant)
        {
            return (from record in context.CardSearchs where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public void Add(CardSearch entity)
        {
            context.CardSearchs.Add(entity);
        }

        public void Remove(CardSearch entity)
        {
            context.CardSearchs.Attach(entity);
            context.CardSearchs.Remove(entity);
        }

        public void Update(CardSearch entity)
        {
            context.CardSearchs.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CardSearch> All()
        {
            return context.CardSearchs.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CardSearch> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public CardSearch GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}