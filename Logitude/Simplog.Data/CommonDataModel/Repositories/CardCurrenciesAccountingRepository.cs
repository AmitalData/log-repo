using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CardCurrenciesAccountingRepository : IRepository<CardCurrenciesAccounting>
    {
        ICommonDataContext commonDataContext;

        public CardCurrenciesAccountingRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public CardCurrenciesAccountingRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public CardCurrenciesAccountingRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CardCurrenciesAccounting GetSingleCardCurrenciesAccountings(string id, int tenant)
        {
            return (from d in context.CardCurrenciesAccountings.Include("Card").Include("Currency") where d.Id == id && d.Tenant == tenant select d).FirstOrDefault();
        }

        public IQueryable<CardCurrenciesAccounting> GetCardCurrenciesAccountings(int tenant)
        {
            return (from d in context.CardCurrenciesAccountings.Include("Card").Include("Currency") where d.Tenant == tenant select d);
        }

        public IQueryable<CardCurrenciesAccounting> GetCardCurrenciesAccountingsForCard(string cardId, int tenant)
        {
            return (from d in context.CardCurrenciesAccountings.Include("Card").Include("Currency") where d.CardId == cardId && d.Tenant == tenant select d);
        }

        public void Add(CardCurrenciesAccounting entity)
        {
            context.CardCurrenciesAccountings.Add(entity);
        }

        public void Remove(CardCurrenciesAccounting entity)
        {
            context.CardCurrenciesAccountings.Attach(entity);
            context.CardCurrenciesAccountings.Remove(entity);
        }

        public void Update(CardCurrenciesAccounting entity)
        {
            context.CardCurrenciesAccountings.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CardCurrenciesAccounting> All()
        {
            return context.CardCurrenciesAccountings.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }



        public List<CardCurrenciesAccounting> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CardCurrenciesAccounting GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
