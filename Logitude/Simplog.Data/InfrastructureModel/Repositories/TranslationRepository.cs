using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using System.Web;
using System;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class TranslationRepository:IRepository<Translation>
    {

        IWebFreightContext webFreightContext;
        public TranslationRepository(IWebFreightContext context)
        {
            webFreightContext = context;

        }
        public TranslationRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public TranslationRepository()
        {
               webFreightContext=new WebFreightContext(); 
        }

        public IQueryable<Translation> GetTranslations()
        {
            return context.Translations.Include("TextCode");
        }

        public Translation GetSingleTranslation(int tenant, string code, string language)
        {
            Translation result = context.Translations.Where(d => d.Tenant == tenant && d.TextCode.Code == code && (d.TranslationHeader.Description == language || d.TranslationHeaderCode == language)).Include("TextCode").FirstOrDefault();
          
            return result;
        }

        public List<Translation> GetTranslationsByTenant(int tenant)
        {
            List<Translation> translations = (from a in context.Translations.Include("TextCode")
                                                   where a.Tenant == tenant                                                   
                                                   select a).ToList();
            return translations;
        }
        //Islam: this is only for silverlight version to fix the timeout login issue.
        public List<Translation> GetTranslationsWithoutESByTenant(int tenant)
        {
            List<Translation> translations = (from a in context.Translations.Include("TextCode")
                                              where a.Tenant == tenant && a.TranslationHeaderCode != "ES"
                                              select a).ToList();
            return translations;
        }

        public List<Translation> GetTranslationsByLanguageCode(string headerCode,int tenant)
        {
            List<Translation> translations = (from a in context.Translations.Include("TextCode")
                                              where a.Tenant == 0 && a.TranslationHeaderCode == headerCode
                                              select a).ToList();
            return translations;
        }


        public Dictionary<string, Translation> GetTranslationsByTenantDictionary(int tenant)
        {
            Dictionary<string, Translation> translations = (from a in context.Translations.Include("TextCode")
                                              where a.Tenant == tenant
                                              select a).ToDictionary(d=>d.TextCode.Code,a=>a);
            return translations;
        }

		public Translation GetLastTranslationsByTenant(int tenant)
		{
			string entityName = "LastTranslationsByTenant" + tenant;
			Translation lastTranslation = null;

			
				if (CacheManager.CacheWrapper.Get(entityName) != null)
				{
					lastTranslation = (Translation)CacheManager.CacheWrapper.Get(entityName);

				}
				else
				{

					lastTranslation = (from a in context.Translations
									   where a.Tenant == tenant && a.UpdateDateGMT != null
									   select a).OrderByDescending(a => a.UpdateDateGMT).FirstOrDefault();

					if (CacheManager.CacheWrapper.Get(entityName) == null)
					{
						if (lastTranslation != null)
						{
							CacheManager.CacheWrapper.Insert(entityName, lastTranslation, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
						}
						else   // cache a default value
							CacheManager.CacheWrapper.Insert(entityName, new Translation() { UpdateDateGMT = new DateTime(2015, 1, 1) }, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
					}
				}
			
	

			return lastTranslation;

		}
       

        public void Add(Translation entity)
		{
			
			context.Translations.Add(entity);
			InvalidateLastTranslationCache(entity);

		}

		private static void InvalidateLastTranslationCache(Translation entity)
		{
			
				string entityName = "LastTranslationsByTenant" + entity.Tenant;
				if (CacheManager.CacheWrapper.Get(entityName) != null)
				{
					CacheManager.CacheWrapper.Invalidate(entityName);
				}
			
		}

		public void Remove(Translation entity)
        {
			
			context.Translations.Attach(entity);
            context.Translations.Remove(entity);
			InvalidateLastTranslationCache(entity);
		}

        public void Update(Translation entity)
        {
			
			context.Translations.Attach(entity);
            context.SetAsModified(entity);
			InvalidateLastTranslationCache(entity);

		}

		public List<Translation> All()
        {
            return context.Translations.ToList();
        }

        public IWebFreightContext context
        {
            get {return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<Translation> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public Translation GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}