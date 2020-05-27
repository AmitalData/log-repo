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
    public class QuoteTemplateSectionTypeRepository : IRepository<QuoteTemplateSectionType>
    {

        public IQuotesContext quotesContext;
        public QuoteTemplateSectionTypeRepository(IQuotesContext context)
        {
            quotesContext = context;
        }
        public QuoteTemplateSectionTypeRepository()
        {
            quotesContext = new QuotesContext();
        }
        

        public QuoteTemplateSectionType GetSingleQuoteTemplateSectionType(string code, bool getFromCache)
        {
            if (!string.IsNullOrEmpty(code))
            {
                QuoteTemplateSectionType entity;
                if (getFromCache)
                {
                    string entityName = "QuoteTemplateSectionType" + code ;
                  
                        if (CacheManager.CacheWrapper.Get(entityName) == null)
                        {

                            var quoteTemplateSectionType = (from a in quotesContext.QuoteTemplateSectionTypes
                                                        
                                                            select a);

                            foreach (var c in quoteTemplateSectionType)
                            {
                                string cname = "QuoteTemplateSectionType" + c.Code;

                                if (CacheManager.CacheWrapper.Get(cname) == null)
                                {
                                    CacheManager.CacheWrapper.Insert(cname, c, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                                }
                            }
                            entity = (QuoteTemplateSectionType)CacheManager.CacheWrapper.Get(entityName);

                        }
                        else
                        {
                            entity = (QuoteTemplateSectionType)CacheManager.CacheWrapper.Get(entityName);
                            // HttpContext.Current.Cache.Insert(EntityNameValue, Entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                    
                
                }
                else
                {
                    entity = (from record in quotesContext.QuoteTemplateSectionTypes where record.Code == code   select record).FirstOrDefault();
                }
                return entity;
            }
            return null;

            //return (from Record in context.Departments where Record.Id == id && Record.Tenant == tenant select Record).FirstOrDefault();
        }




        public void Add(QuoteTemplateSectionType entity)
        {
            quotesContext.QuoteTemplateSectionTypes.Add(entity);
        }

        public void Remove(QuoteTemplateSectionType entity)
        {
            quotesContext.QuoteTemplateSectionTypes.Attach(entity);
            quotesContext.QuoteTemplateSectionTypes.Remove(entity);
        }


        public void Update(QuoteTemplateSectionType entity)
        {
            quotesContext.QuoteTemplateSectionTypes.Attach(entity);
            quotesContext.SetAsModified(entity);
        }

        public List<QuoteTemplateSectionType> All()
        {
            return quotesContext.QuoteTemplateSectionTypes.ToList();
        }

        public void SubmitChanges()
        {
            quotesContext.SaveChanges();
        }

        public IQueryable<QuoteTemplateSectionType> GetQuoteTemplateSectionTypes()
        {

            return quotesContext.QuoteTemplateSectionTypes;
             
        }

        public List<QuoteTemplateSectionType> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public QuoteTemplateSectionType GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
