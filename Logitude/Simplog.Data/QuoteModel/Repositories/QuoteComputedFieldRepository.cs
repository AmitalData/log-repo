using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simplog.Data.QuoteModel.Repositories
{
    public class QuoteComputedFieldRepository : IRepository<QuoteComputedField>
    {
        IQuotesContext quotesContext;
        public QuoteComputedFieldRepository(IQuotesContext context)
        {
            quotesContext = context;
        }

        public QuoteComputedFieldRepository()
        {
            quotesContext = new QuotesContext();
        }
        public QuoteComputedFieldRepository(int tenant)
        {
            quotesContext =  QuotesContext.GetContext(tenant);
        }

        public IQueryable<QuoteComputedField> GetQuoteComputedField(int tenant)
        {
            return context.QuoteComputedField.Where(s => s.Tenant == tenant);
        }
        public IQueryable<QuoteComputedField> GetQuoteComputedField()
        {
            return context.QuoteComputedField;
        }
        public QuoteComputedField GetSingleQuoteComputedField(string Id, int tenant)
        {
            if (!string.IsNullOrEmpty(Id))
            {

                QuoteComputedField quoteComputedFieldEntity = (from quoteComputedField in context.QuoteComputedField
                                             where quoteComputedField.Id == Id && quoteComputedField.Tenant == tenant
                                             select quoteComputedField).FirstOrDefault();
                return quoteComputedFieldEntity;
            }
            return null;
        }
        public void Add(QuoteComputedField entity)
        {
            context.QuoteComputedField.Add(entity);
        }

        public List<QuoteComputedField> All()
        {
            return context.QuoteComputedField.ToList();
        }

        public List<QuoteComputedField> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public QuoteComputedField GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public QuoteComputedField GetSingleQuoteComputedField(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                QuoteComputedField quoteComputedFieldEntity = (from quoteComputedField in context.QuoteComputedField
                                                               where quoteComputedField.Id == id 
                                                               select quoteComputedField).FirstOrDefault();
                return quoteComputedFieldEntity;
            }
            return null;
        }
        public void Remove(QuoteComputedField entity)
        {
            context.QuoteComputedField.Attach(entity);
            context.QuoteComputedField.Remove(entity);
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public void Update(QuoteComputedField entity)
        {
            context.QuoteComputedField.Attach(entity);
            context.SetAsModified(entity);
        }

        public IQuotesContext context
        {
            get { return quotesContext; }
        }

        public IQueryable<QuoteComputedField> GetShipmentComputedFieldsByIds(List<string> ids, int tenant)
        {
            return context.QuoteComputedField.Where(s => s.Tenant == tenant && ids.Contains(s.Id));
        }
    }
}
