using System.Collections.Generic;
using System.Linq;

using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.QuoteModel.Repositories
{
    public class QuoteTypeRepository: IRepository<QuoteType>
    {
        IQuotesContext quotesContext;

        public QuoteTypeRepository()
        {
            quotesContext = new QuotesContext();
        }
        public QuoteTypeRepository(IQuotesContext context)
        {
            quotesContext = context;
        }
        public QuoteTypeRepository(int tenant)
        {
            quotesContext = QuotesContext.GetContext(tenant);
        }
        public QuoteType GetSingleQuoteType(string code)
        {
            return (from a in context.QuoteTypes
                    where a.Code == code
                    select a).FirstOrDefault();
        }
        public IQueryable<QuoteType> GetAll()
        {
            return context.QuoteTypes;
        }
        public IQueryable<QuoteType> GetQuoteTypes()
        {
            return context.QuoteTypes;
        }       

        public void Add(QuoteType entity)
        {
            context.QuoteTypes.Add(entity);
        }

        public void Remove(QuoteType entity)
        {
            context.QuoteTypes.Attach(entity);
            context.QuoteTypes.Remove(entity);
        }

        public void Update(QuoteType entity)
        {
            context.QuoteTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuoteType> All()
        {
            return context.QuoteTypes.ToList();
        }

        public IQuotesContext context
        {
            get { return quotesContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<QuoteType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public QuoteType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}