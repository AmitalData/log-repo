using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.QuoteModel.Repositories
{
    public class QuoteTemplateDetailsFieldRepository : IRepository<QuoteTemplateDetailsField>
     {




         public IQuotesContext quotesContext;

         public QuoteTemplateDetailsFieldRepository(IQuotesContext context)
         {
             quotesContext = context;
         }
         public QuoteTemplateDetailsFieldRepository()
         {
             quotesContext = new QuotesContext();
         }
         public QuoteTemplateDetailsFieldRepository(int tenant)
         {
             quotesContext = QuotesContext.GetContext(tenant);
         }




         public QuoteTemplateDetailsField GetSingleQuoteTemplateDetailsField(string id, int tenant)
         {

             QuoteTemplateDetailsField entity = this.quotesContext.QuoteTemplateDetailsFields.Where(d => d.Id == id && d.Tenant == tenant).FirstOrDefault();


             return entity;
         }
         public IQueryable<QuoteTemplateDetailsField> GetQuoteTemplateDetailsFields(int tenant)
         {
             return (from quoteTemplateDetails in quotesContext.QuoteTemplateDetailsFields where quoteTemplateDetails.Tenant == tenant select quoteTemplateDetails);

         }


         public void Add(QuoteTemplateDetailsField entity)
         {
             quotesContext.QuoteTemplateDetailsFields.Add(entity);
         }

         public void Remove(QuoteTemplateDetailsField entity)
         {
             quotesContext.QuoteTemplateDetailsFields.Attach(entity);
             quotesContext.QuoteTemplateDetailsFields.Remove(entity);
         }


         public void Update(QuoteTemplateDetailsField entity)
         {
             quotesContext.QuoteTemplateDetailsFields.Attach(entity);
             quotesContext.SetAsModified(entity);
         }

         public List<QuoteTemplateDetailsField> All()
         {
             return quotesContext.QuoteTemplateDetailsFields.ToList();
         }

         public void SubmitChanges()
         {
             quotesContext.SaveChanges();
         }

         public List<QuoteTemplateDetailsField> GetMulti(EntityKeyFields entityKeys)
         {
             throw new NotImplementedException();
         }

         public QuoteTemplateDetailsField GetSingle(EntityKeyFields entityKeys)
         {
             throw new NotImplementedException();
         }









         public List<QuoteTemplateDetailsField> GetQuoteTemplateDetailsFieldsByQuoteTemplateId(string quotetemplateid, int tenant)
         {
             List<QuoteTemplateDetailsField> quoteTemplateDetailsFieldLists = this.quotesContext.QuoteTemplateDetailsFields.Where(d => d.QuoteTemplateId == quotetemplateid && d.Tenant == tenant).ToList();


             return quoteTemplateDetailsFieldLists;
         }



   
     }
}
