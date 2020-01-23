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
    public class QuoteTemplateTextCodeRepository : IRepository<QuoteTemplateTextCode>
    {

         public IQuotesContext quotesContext;
        public QuoteTemplateTextCodeRepository(IQuotesContext context)
        {
            quotesContext = context;
        }
          public QuoteTemplateTextCodeRepository()
        {
            quotesContext = new QuotesContext();
        }
          public QuoteTemplateTextCodeRepository(int tenant)
        {
            quotesContext = QuotesContext.GetContext(tenant);
        }


          public QuoteTemplateTextCode GetSingleQuoteTemplateTextCode(string id, int tenant, bool getFromCache)
          {
              if (!string.IsNullOrEmpty(id))
              {
                  QuoteTemplateTextCode entity;
                  if (getFromCache)
                  {
                      string entityName = "QuoteTemplateTextCode" + id + tenant;
                     
                          if (CacheManager.CacheWrapper.Get(entityName) == null)
                          {

                              var quotetemplatetextcode = (from a in quotesContext.QuoteTemplateTextCodes
                                                           where a.Tenant == tenant
                                                           select a);

                              foreach (var c in quotetemplatetextcode)
                              {
                                  string cname = "QuoteTemplateTextCode" + c.Id + c.Tenant;

                                  if (CacheManager.CacheWrapper.Get(cname) == null)
                                  {
                                      CacheManager.CacheWrapper.Insert(cname, c, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                                  }
                              }
                              entity = (QuoteTemplateTextCode)CacheManager.CacheWrapper.Get(entityName);

                          }
                          else
                          {
                              entity = (QuoteTemplateTextCode)CacheManager.CacheWrapper.Get(entityName);
                              // HttpContext.Current.Cache.Insert(EntityNameValue, Entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                          }
                      
                    
                  }
                  else
                  {
                      entity = (from record in quotesContext.QuoteTemplateTextCodes where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
                  }
                  return entity;
              }
              return null;

              //return (from Record in context.Departments where Record.Id == id && Record.Tenant == tenant select Record).FirstOrDefault();
          }

          public void Add(QuoteTemplateTextCode entity)
          {
              quotesContext.QuoteTemplateTextCodes.Add(entity);
          }

          public void Remove(QuoteTemplateTextCode entity)
          {
              quotesContext.QuoteTemplateTextCodes.Attach(entity);
              quotesContext.QuoteTemplateTextCodes.Remove(entity);
          }

          public IQueryable<QuoteTemplateTextCode> GetQuoteTemplateTextCodes(int tenant)
          {
              return (from record in quotesContext.QuoteTemplateTextCodes where record.Tenant == tenant select record);
          }

          public IQueryable<QuoteTemplateTextCode> GetQuoteTemplateTextCodeByTenant(int tenant, string Id)
          {
              IQueryable<QuoteTemplateTextCode> quotetemplatetextcodes = null;

              if (!string.IsNullOrEmpty(Id))
              {
                  quotetemplatetextcodes = from a in quotesContext.QuoteTemplateTextCodes
                                   where a.Tenant == tenant && (a.Id == Id)
                                   select a;
              }
              else
              {
                  quotetemplatetextcodes = from a in quotesContext.QuoteTemplateTextCodes
                                   where a.Tenant == tenant
                                   select a;
              }

              return quotetemplatetextcodes;
          }
          public void Update(QuoteTemplateTextCode entity)
          {
              quotesContext.QuoteTemplateTextCodes.Attach(entity);
              quotesContext.SetAsModified(entity);
          }

          public List<QuoteTemplateTextCode> All()
          {
              return quotesContext.QuoteTemplateTextCodes.ToList();
          }

          public void SubmitChanges()
          {
              quotesContext.SaveChanges();
          }

          public List<QuoteTemplateTextCode> GetMulti(EntityKeyFields entityKeys)
          {
              throw new NotImplementedException();
          }

          public QuoteTemplateTextCode GetSingle(EntityKeyFields entityKeys)
          {
              throw new NotImplementedException();
          }


          public IQueryable<QuoteTemplateTextCode> GetQuoteTemplateTextCodeByQuoteTemplateId(string quotetemplateid, int tenant)
          {
              IQueryable<QuoteTemplateTextCode> QuoteTemplateTextCodeLists = this.quotesContext.QuoteTemplateTextCodes.Where(d => d.QuoteTemplateId == quotetemplateid && d.Tenant == tenant);


              return QuoteTemplateTextCodeLists;
          }
    }
}
