using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.QuoteModel.Repositories
{
    public class QuoteTemplateHeaderFieldRepository : IRepository<QuoteTemplateHeaderField>
    {

        public IQuotesContext quotesContext;

        public QuoteTemplateHeaderFieldRepository(IQuotesContext context)
        {
            quotesContext = context;
        }
        public QuoteTemplateHeaderFieldRepository()
        {
            quotesContext = new QuotesContext();
        }
        public QuoteTemplateHeaderFieldRepository(int tenant)
        {
            quotesContext = QuotesContext.GetContext(tenant);
        }




        public QuoteTemplateHeaderField GetSingleQuoteTemplateHeaderField(string id, int tenant)
        {

            QuoteTemplateHeaderField entity = this.quotesContext.QuoteTemplateHeaderFields.Where(d => d.Id == id && d.Tenant == tenant).FirstOrDefault();


            return entity;
        }
        public IQueryable<QuoteTemplateHeaderField> GetQuoteTemplateHeaderFields(int tenant)
        {
            return (from quoteTemplateHeader in quotesContext.QuoteTemplateHeaderFields where quoteTemplateHeader.Tenant == tenant select quoteTemplateHeader);

        }


        public void Add(QuoteTemplateHeaderField entity)
        {
            quotesContext.QuoteTemplateHeaderFields.Add(entity);
        }

        public void Remove(QuoteTemplateHeaderField entity)
        {
            quotesContext.QuoteTemplateHeaderFields.Attach(entity);
            quotesContext.QuoteTemplateHeaderFields.Remove(entity);
        }


        public void Update(QuoteTemplateHeaderField entity)
        {
            quotesContext.QuoteTemplateHeaderFields.Attach(entity);
            quotesContext.SetAsModified(entity);
        }

        public List<QuoteTemplateHeaderField> All()
        {
            return quotesContext.QuoteTemplateHeaderFields.ToList();
        }

        public void SubmitChanges()
        {
            quotesContext.SaveChanges();
        }

        public List<QuoteTemplateHeaderField> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public QuoteTemplateHeaderField GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }


       // public List<QuoteTemplateHeaderField> GetQuoteTemplateHeaderFieldsByQuoteTemplateId(string quotetemplateId, int tenant)
      //  {
      
            //return (from quoteTemplateHeader in quotesContext.QuoteTemplateHeaderFields where quoteTemplateHeader.Tenant == tenant && quoteTemplateHeader.QuoteTemplateId == quotetemplateId select quoteTemplateHeader);
      //  }


        public List<QuoteTemplateHeaderField> GetQuoteTemplateHeaderFieldsByQuoteTemplateId(string quotetemplateid, int tenant)
        {
            List<QuoteTemplateHeaderField> quoteTemplateHeaderFieldLists = this.quotesContext.QuoteTemplateHeaderFields.Where(d => d.QuoteTemplateId == quotetemplateid && d.Tenant == tenant).ToList();


            return quoteTemplateHeaderFieldLists;
        }



    }
}