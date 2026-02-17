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
   public class QuoteTemplateTextDesignRepository : IRepository<QuoteTemplateTextDesign>
    {

         public IQuotesContext quotesContext;
        public QuoteTemplateTextDesignRepository(IQuotesContext context)
        {
            quotesContext = context;
        }
          public QuoteTemplateTextDesignRepository()
        {
            quotesContext = new QuotesContext();
        }
          public QuoteTemplateTextDesignRepository(int tenant)
        {
            quotesContext = QuotesContext.GetContext(tenant);
        }

          public QuoteTemplateTextDesign GetSingleQuoteTemplateTextDesign(string id, int tenant, bool getFromCache)
          {
              if (!string.IsNullOrEmpty(id))
              {
                  QuoteTemplateTextDesign entity;
                  if (getFromCache)
                  {
                      string entityName = "QuoteTemplateTextDesign" + id + tenant;
                      if (HttpContext.Current != null)
                      {
                          if (CacheManager.CacheWrapper.Get(entityName) == null)
                          {

                              var quoteTemplateTextDesign = (from a in quotesContext.QuoteTemplateTextDesigns
                                                   where a.Tenant == tenant
                                                   select a);

                              foreach (var c in quoteTemplateTextDesign)
                              {
                                  string cname = "QuoteTemplateTextDesign" + c.Id + c.Tenant;

                                  if (CacheManager.CacheWrapper.Get(cname) == null)
                                  {
                                      CacheManager.CacheWrapper.Insert(cname, c, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                                  }
                              }
                              entity = (QuoteTemplateTextDesign)CacheManager.CacheWrapper.Get(entityName);

                          }
                          else
                          {
                              entity = (QuoteTemplateTextDesign)CacheManager.CacheWrapper.Get(entityName);
                              // HttpContext.Current.Cache.Insert(EntityNameValue, Entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                          }
                      }
                      else
                      {
                          entity = (from record in quotesContext.QuoteTemplateTextDesigns where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
                      }
                  }
                  else
                  {
                      entity = (from record in quotesContext.QuoteTemplateTextDesigns where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
                  }
                  return entity;
              }
              return null;

              //return (from Record in context.Departments where Record.Id == id && Record.Tenant == tenant select Record).FirstOrDefault();
          }

          public void Add(QuoteTemplateTextDesign entity)
          {
              quotesContext.QuoteTemplateTextDesigns.Add(entity);
          }

          public void Remove(QuoteTemplateTextDesign entity)
          {
              quotesContext.QuoteTemplateTextDesigns.Attach(entity);
              quotesContext.QuoteTemplateTextDesigns.Remove(entity);
          }

          public IQueryable<QuoteTemplateTextDesign> GetQuoteTemplateTextDesigns(int tenant)
          {
              return (from record in quotesContext.QuoteTemplateTextDesigns where record.Tenant == tenant select record);
          }

          public IQueryable<QuoteTemplateTextDesign> GetQuoteTemplateTextDesignByTenant(int tenant, string Id)
          {
              IQueryable<QuoteTemplateTextDesign> quoteTemplateTextDesigns = null;

              if (!string.IsNullOrEmpty(Id))
              {
                  quoteTemplateTextDesigns = from a in quotesContext.QuoteTemplateTextDesigns
                                   where a.Tenant == tenant && (a.Id == Id)
                                   select a;
              }
              else
              {
                  quoteTemplateTextDesigns = from a in quotesContext.QuoteTemplateTextDesigns
                                   where a.Tenant == tenant
                                   select a;
              }

              return quoteTemplateTextDesigns;
          }
          public void Update(QuoteTemplateTextDesign entity)
          {
              quotesContext.QuoteTemplateTextDesigns.Attach(entity);
              quotesContext.SetAsModified(entity);
          }

          public List<QuoteTemplateTextDesign> All()
          {
              return quotesContext.QuoteTemplateTextDesigns.ToList();
          }

          public void SubmitChanges()
          {
              quotesContext.SaveChanges();
          }

          public List<QuoteTemplateTextDesign> GetMulti(EntityKeyFields entityKeys)
          {
              throw new NotImplementedException();
          }

          public QuoteTemplateTextDesign GetSingle(EntityKeyFields entityKeys)
          {
              throw new NotImplementedException();
          }

    }
}
