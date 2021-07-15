 
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
   public partial class QuoteOPTemplateSectionRepository:IRepository<QuoteOPTemplateSection>
   {
   
        private IQuoteOPMContext currentContext;
        public QuoteOPTemplateSectionRepository(int tenant)
        {
            currentContext = QuoteOPMContext.GetContext(tenant);
        }

        public QuoteOPTemplateSectionRepository(IQuoteOPMContext context)
        {
            currentContext = context;
        }

		 
		
		public  QuoteOPTemplateSection GetSingle(string id, int tenant)
        {
            return (from a in context.QuoteOPTemplateSections
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<QuoteOPTemplateSection> GetAll(int tenant)
        {
            return from a in context.QuoteOPTemplateSections  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public QuoteOPTemplateSection GetSingle(EntityKeyFields entityKeys)
        {
            QuoteOPTemplateSectionKeys keys = entityKeys as QuoteOPTemplateSectionKeys;
            return (from a in context.QuoteOPTemplateSections
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(QuoteOPTemplateSection entity)
        {
            onAdd();
            context.QuoteOPTemplateSections.Add(entity);
        }

        public void Remove(QuoteOPTemplateSection entity)
        {
            context.QuoteOPTemplateSections.Attach(entity);
            context.QuoteOPTemplateSections.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(QuoteOPTemplateSection entity)
        {
            onUpdate();
            context.QuoteOPTemplateSections.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuoteOPTemplateSection> All()
        {
            return context.QuoteOPTemplateSections.ToList();
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
	 