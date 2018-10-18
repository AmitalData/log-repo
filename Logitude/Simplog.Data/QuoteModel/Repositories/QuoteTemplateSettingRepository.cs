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
    public class QuoteTemplateSettingRepository : IRepository<QuoteTemplateSetting>
    {
      
        public IQuotesContext quotesContext;
        public QuoteTemplateSettingRepository(IQuotesContext context)
        {
            quotesContext = context;
        }
        public QuoteTemplateSettingRepository()
        {
            quotesContext = new QuotesContext();
        }
        public QuoteTemplateSettingRepository(int tenant)
        {
            quotesContext = QuotesContext.GetContext(tenant);
        }
   
        public QuoteTemplateSetting GetSingleQuoteTemplateSetting(string id, int tenant)
        {

            QuoteTemplateSetting entity = this.quotesContext.QuoteTemplateSettings.Where(d => d.Id == id  && d.Tenant == tenant ).FirstOrDefault();
  

            return entity;
            //if (!string.IsNullOrEmpty(id))
            //{
            //    QuoteTemplateSetting entity;
            //    if (getFromCache)
            //    {
            //        string entityName = "QuoteTemplateSetting" + id + tenant;
            //        if (HttpContext.Current != null)
            //        {
            //            if (CacheManager.CacheWrapper.Get(entityName) == null)
            //            {

            //                var quoteTemplateSetting = (from a in quotesContext.QuoteTemplateSettings
            //                                   where a.Tenant == tenant
            //                                   select a);

            //                foreach (var c in quoteTemplateSetting)
            //                {
            //                    string cname = "QuoteTemplateSetting" + c.Id + c.Tenant;

            //                    if (CacheManager.CacheWrapper.Get(cname) == null)
            //                    {
            //                        CacheManager.CacheWrapper.Insert(cname, c, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
            //                    }
            //                }
            //                entity = (QuoteTemplateSetting)CacheManager.CacheWrapper.Get(entityName);

            //            }
            //            else
            //            {
            //                entity = (QuoteTemplateSetting)CacheManager.CacheWrapper.Get(entityName);
            //                // HttpContext.Current.Cache.Insert(EntityNameValue, Entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
            //            }
            //        }
            //        else
            //        {
            //            entity = (from record in quotesContext.QuoteTemplateSettings where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
            //        }
            //    }
            //    else
            //    {
            //        entity = (from record in quotesContext.QuoteTemplateSettings where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
            //    }
            //    return entity;
            //}
            //return null;

            //return (from Record in context.Departments where Record.Id == id && Record.Tenant == tenant select Record).FirstOrDefault();
           // return entity;
        }


        public IQueryable<QuoteTemplateSetting> GetQuoteTemplateSettings(int tenant)
        {
            return (from record in quotesContext.QuoteTemplateSettings where record.Tenant == tenant select record);
        }


        public void Add(QuoteTemplateSetting entity)
        {
            quotesContext.QuoteTemplateSettings.Add(entity);
        }

        public void Remove(QuoteTemplateSetting entity)
        {
            quotesContext.QuoteTemplateSettings.Attach(entity);
            quotesContext.QuoteTemplateSettings.Remove(entity);
        }


        public void Update(QuoteTemplateSetting entity)
        {
            quotesContext.QuoteTemplateSettings.Attach(entity);
            quotesContext.SetAsModified(entity);
        }

        public List<QuoteTemplateSetting> All()
        {
            return quotesContext.QuoteTemplateSettings.ToList();
        }

        public void SubmitChanges()
        {
            quotesContext.SaveChanges();
        }

        public List<QuoteTemplateSetting> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public QuoteTemplateSetting GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}