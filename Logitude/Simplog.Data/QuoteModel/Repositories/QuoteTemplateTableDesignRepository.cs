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
   public class QuoteTemplateTableDesignRepository : IRepository<QuoteTemplateTableDesign>
    {

         public IQuotesContext quotesContext;
        public QuoteTemplateTableDesignRepository(IQuotesContext context)
        {
            quotesContext = context;
        }
          public QuoteTemplateTableDesignRepository()
        {
            quotesContext = new QuotesContext();
        }
          public QuoteTemplateTableDesignRepository(int tenant)
        {
            quotesContext = QuotesContext.GetContext(tenant);
        }



          public QuoteTemplateTableDesign GetSingleQuoteTemplateTableDesign(string id, int tenant, bool getFromCache)
          {
              if (!string.IsNullOrEmpty(id))
              {
                  QuoteTemplateTableDesign entity;
                  if (getFromCache)
                  {
                      string entityName = "QuoteTemplateTableDesign" + id + tenant;
                      if (HttpContext.Current != null)
                      {
                          if (CacheManager.CacheWrapper.Get(entityName) == null)
                          {

                              var quoteTemplateTableDesign = (from a in quotesContext.QuoteTemplateTableDesigns
                                                   where a.Tenant == tenant
                                                   select a);

                              foreach (var c in quoteTemplateTableDesign)
                              {
                                  string cname = "QuoteTemplateTableDesign" + c.Id + c.Tenant;

                                  if (CacheManager.CacheWrapper.Get(cname) == null)
                                  {
                                      CacheManager.CacheWrapper.Insert(cname, c, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                                  }
                              }
                              entity = (QuoteTemplateTableDesign)CacheManager.CacheWrapper.Get(entityName);

                          }
                          else
                          {
                              entity = (QuoteTemplateTableDesign)CacheManager.CacheWrapper.Get(entityName);
                              // HttpConTable.Current.Cache.Insert(EntityNameValue, Entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                          }
                      }
                      else
                      {
                          entity = (from record in quotesContext.QuoteTemplateTableDesigns where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
                      }
                  }
                  else
                  {
                      entity = (from record in quotesContext.QuoteTemplateTableDesigns where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
                  }
                  return entity;
              }
              return null;

              //return (from Record in conTable.Departments where Record.Id == id && Record.Tenant == tenant select Record).FirstOrDefault();
          }

          public void Add(QuoteTemplateTableDesign entity)
          {
              quotesContext.QuoteTemplateTableDesigns.Add(entity);
          }

          public void Remove(QuoteTemplateTableDesign entity)
          {
              quotesContext.QuoteTemplateTableDesigns.Attach(entity);
              quotesContext.QuoteTemplateTableDesigns.Remove(entity);
          }

          public IQueryable<QuoteTemplateTableDesign> GetQuoteTemplateTableDesigns(int tenant)
          {
              return (from record in quotesContext.QuoteTemplateTableDesigns where record.Tenant == tenant select record);
          }

          public IQueryable<QuoteTemplateTableDesign> GetQuoteTemplateTableDesignByTenant(int tenant, string Id)
          {
              IQueryable<QuoteTemplateTableDesign> quoteTemplateTableDesigns = null;

              if (!string.IsNullOrEmpty(Id))
              {
                  quoteTemplateTableDesigns = from a in quotesContext.QuoteTemplateTableDesigns
                                   where a.Tenant == tenant && (a.Id == Id)
                                   select a;
              }
              else
              {
                  quoteTemplateTableDesigns = from a in quotesContext.QuoteTemplateTableDesigns
                                   where a.Tenant == tenant
                                   select a;
              }

              return quoteTemplateTableDesigns;
          }
          public void Update(QuoteTemplateTableDesign entity)
          {
              quotesContext.QuoteTemplateTableDesigns.Attach(entity);
              quotesContext.SetAsModified(entity);
          }

          public List<QuoteTemplateTableDesign> All()
          {
              return quotesContext.QuoteTemplateTableDesigns.ToList();
          }

          public void SubmitChanges()
          {
              quotesContext.SaveChanges();
          }

          public List<QuoteTemplateTableDesign> GetMulti(EntityKeyFields entityKeys)
          {
              throw new NotImplementedException();
          }

          public QuoteTemplateTableDesign GetSingle(EntityKeyFields entityKeys)
          {
              throw new NotImplementedException();
          }

    }
}
