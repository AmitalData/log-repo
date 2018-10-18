using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.QuoteModel.Repositories
{
    public class QuoteRatingRepository : IRepository<QuoteRating>
    {
        IQuotesContext quotesContext;

        public QuoteRatingRepository()
        {
            quotesContext = new QuotesContext();
        }

        public QuoteRatingRepository(IQuotesContext context)
        {
            quotesContext = context;
        }

        public QuoteRatingRepository(int tenant)
        {
            quotesContext = QuotesContext.GetContext(tenant);
        }

        public QuoteRating GetSingleQuoteRating(string code)
        {
            return (from a in context.QuoteRatings where a.Code == code select a).FirstOrDefault();
        }
        public IQueryable<QuoteRating> GetAll()
        {
            return context.QuoteRatings;
        }
        public IQueryable<QuoteRating> GetQuoteRatings()
        {
            return context.QuoteRatings;
        }

        public void Add(QuoteRating entity)
        {
            context.QuoteRatings.Add(entity);
        }

        public void Remove(QuoteRating entity)
        {
            context.QuoteRatings.Attach(entity);
            context.QuoteRatings.Remove(entity);
        }

        public void Update(QuoteRating entity)
        {
            context.QuoteRatings.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuoteRating> All()
        {
            return context.QuoteRatings.ToList();
        }

        public IQuotesContext context
        {
            get { return quotesContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<QuoteRating> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public QuoteRating GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

    }
}
