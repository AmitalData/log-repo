 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Amital.QuoteOPM.Data.Repsitories
{
   public partial class QuoteOPTemplateSettingRepository:IRepository<QuoteOPTemplateSetting>
   {
   
        private IQuoteOPMContext currentContext;
        public QuoteOPTemplateSettingRepository(int tenant)
        {
            currentContext = QuoteOPMContext.GetContext(tenant);
        }

        public QuoteOPTemplateSettingRepository(IQuoteOPMContext context)
        {
            currentContext = context;
        }

		 
		
		public  QuoteOPTemplateSetting GetSingle(string id, int tenant)
        {
            return (from a in context.QuoteOPTemplateSettings
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<QuoteOPTemplateSetting> GetAll(int tenant)
        {
            return from a in context.QuoteOPTemplateSettings  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public QuoteOPTemplateSetting GetSingle(EntityKeyFields entityKeys)
        {
            QuoteOPTemplateSettingKeys keys = entityKeys as QuoteOPTemplateSettingKeys;
            return (from a in context.QuoteOPTemplateSettings
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(QuoteOPTemplateSetting entity)
        {
            onAdd();
            context.QuoteOPTemplateSettings.Add(entity);
        }

        public void Remove(QuoteOPTemplateSetting entity)
        {
            context.QuoteOPTemplateSettings.Attach(entity);
            context.QuoteOPTemplateSettings.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(QuoteOPTemplateSetting entity)
        {
            onUpdate();
            context.QuoteOPTemplateSettings.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuoteOPTemplateSetting> All()
        {
            return context.QuoteOPTemplateSettings.ToList();
        }

        private IQuoteOPMContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 