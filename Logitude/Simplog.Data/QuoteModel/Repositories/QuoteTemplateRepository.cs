using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Simplog.Data.QuoteModel.Repositories
{
   public class QuoteTemplateRepository: IRepository<QuoteTemplate>
    {
       public IQuotesContext quotesContext;
        public QuoteTemplateRepository(IQuotesContext context)
        {
            quotesContext = context;
        }
          public QuoteTemplateRepository()
        {
            quotesContext = new QuotesContext();
        }
          public QuoteTemplateRepository(int tenant)
        {
            quotesContext = QuotesContext.GetContext(tenant);
        }

          public QuoteTemplate GetSingleQuoteTemplate(string id, int tenant)
          {
              if (!string.IsNullOrEmpty(id))
              {
                  QuoteTemplate entity = (from record in quotesContext.QuoteTemplates.Include("QuoteType").Include("QuoteTemplateSetting") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
                  return entity;
              }
              else
              {
                  return null;
              }

              //return (from Record in context.Departments where Record.Id == id && Record.Tenant == tenant select Record).FirstOrDefault();
          }



          public void Add(QuoteTemplate entity)
          {
              quotesContext.QuoteTemplates.Add(entity);
          }

          public void Remove(QuoteTemplate entity)
          {
              quotesContext.QuoteTemplates.Attach(entity);
              quotesContext.QuoteTemplates.Remove(entity);
          }

          public IQueryable<QuoteTemplate> GetQuoteTemplates(int tenant)
          {
              return (from record in quotesContext.QuoteTemplates where record.Tenant == tenant select record);
          }


          public IQueryable<QuoteTemplate> GetAllQuoteTemplates()
          {
              return (from record in quotesContext.QuoteTemplates select record);
          }



          public IQueryable<QuoteTemplate> GetQuoteTemplatesByType(string quotetemplatetype,int tenant)
          {
              return (from record in quotesContext.QuoteTemplates where record.Tenant == tenant && record.TemplateTypeCode == quotetemplatetype && record.InActive == false select record);
          }

          public IQueryable<QuoteTemplate> GetQuoteTemplateByTenant(int tenant, string Id)
          {
              IQueryable<QuoteTemplate> quoteTemplates = null;

              if (!string.IsNullOrEmpty(Id))
              {
                  quoteTemplates = from a in quotesContext.QuoteTemplates.Include("QuoteType").Include("QuoteTemplateSetting")
                           where a.Tenant == tenant && (a.Id == Id)
                           select a;
              }
              else
              {
                  quoteTemplates = from a in quotesContext.QuoteTemplates.Include("QuoteType").Include("QuoteTemplateSetting")
                           where a.Tenant == tenant
                           select a;
              }

              return quoteTemplates;
          }
          public void Update(QuoteTemplate entity)
          {
              quotesContext.QuoteTemplates.Attach(entity);
              quotesContext.SetAsModified(entity);
          }

          public List<QuoteTemplate> All()
          {
              return quotesContext.QuoteTemplates.ToList();
          }

          public void SubmitChanges()
          {
              quotesContext.SaveChanges();
          }

          public List<QuoteTemplate> GetMulti(EntityKeyFields entityKeys)
          {
              throw new NotImplementedException();
          }

          public QuoteTemplate GetSingle(EntityKeyFields entityKeys)
          {
              throw new NotImplementedException();
          }
    }
}
