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
    public class QuoteTemplateSectionRepository: IRepository<QuoteTemplateSection>
    {
            public IQuotesContext quotesContext;
        public QuoteTemplateSectionRepository(IQuotesContext context)
        {
            quotesContext = context;
        }
        public QuoteTemplateSectionRepository()
        {
            quotesContext = new QuotesContext();
        }
        public QuoteTemplateSectionRepository(int tenant)
        {
            quotesContext = QuotesContext.GetContext(tenant);
        }
   
        public QuoteTemplateSection GetSingleQuoteTemplateSection(string id, int tenant, bool getFromCache)
        {
            if (!string.IsNullOrEmpty(id))
            {
                QuoteTemplateSection entity;
                if (getFromCache)
                {
                    string entityName = "QuoteTemplateSection" + id + tenant;
                
                        if (CacheManager.CacheWrapper.Get(entityName) == null)
                        {

                            var quoteTemplateSection = (from a in quotesContext.QuoteTemplateSections
                                               where a.Tenant == tenant
                                               select a);

                            foreach (var c in quoteTemplateSection)
                            {
                                string cname = "QuoteTemplateSection" + c.Id + c.Tenant;

                                if (CacheManager.CacheWrapper.Get(cname) == null)
                                {
                                    CacheManager.CacheWrapper.Insert(cname, c, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                                }
                            }
                            entity = (QuoteTemplateSection)CacheManager.CacheWrapper.Get(entityName);

                        }
                        else
                        {
                            entity = (QuoteTemplateSection)CacheManager.CacheWrapper.Get(entityName);
                            // HttpContext.Current.Cache.Insert(EntityNameValue, Entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                    
                 
                }
                else
                {
                    entity = (from record in quotesContext.QuoteTemplateSections where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
                }
                return entity;
            }
            return null;

            //return (from Record in context.Departments where Record.Id == id && Record.Tenant == tenant select Record).FirstOrDefault();
        }


        public IQueryable<QuoteTemplateSection> GetQuoteTemplateSections(int tenant)
        {
            return (from record in quotesContext.QuoteTemplateSections where record.Tenant == tenant select record);
        }

        public QuoteTemplateSectionModification GetQuoteTemplateSectionModification(string quoteTemplateSectionnId, string quoteId, int tenant)
        {
            return (from record in quotesContext.QuoteTemplateSectionModifications
                    where record.Tenant == tenant && record.QuoteTemplateSectionId == quoteTemplateSectionnId
                    && record.QuoteId == quoteId
                    select record).FirstOrDefault();
        }

        public IQueryable<QuoteTemplateSectionModification> GetAllQuoteTemplateSectionModifications(string quoteId, int tenant)
        {
            return (from record in quotesContext.QuoteTemplateSectionModifications
                    where record.Tenant == tenant
                    && record.QuoteId == quoteId
                    
                    select record);
        }


        public void Add(QuoteTemplateSection entity)
        {
            quotesContext.QuoteTemplateSections.Add(entity);
        }

        public void Remove(QuoteTemplateSection entity)
        {
            quotesContext.QuoteTemplateSections.Attach(entity);
            quotesContext.QuoteTemplateSections.Remove(entity);
        }


        public void Update(QuoteTemplateSection entity)
        {
            quotesContext.QuoteTemplateSections.Attach(entity);
            quotesContext.SetAsModified(entity);
        }

        public List<QuoteTemplateSection> All()
        {
            return quotesContext.QuoteTemplateSections.ToList();
        }

        public void SubmitChanges()
        {
            quotesContext.SaveChanges();
        }

        public List<QuoteTemplateSection> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public QuoteTemplateSection GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public IQueryable<QuoteTemplateSection> GetQuoteTemplateSectionByQuoteTemplateId(string quoteTemplateid, int tenant)
        {
            return (from record in quotesContext.QuoteTemplateSections
                    where record.Tenant == tenant
                    && record.QuoteTemplateId == quoteTemplateid

                    select record);
        }
    }
}
