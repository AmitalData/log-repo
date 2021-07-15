 
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
   public partial class QuoteOPTemplateTextDesignRepository:IRepository<QuoteOPTemplateTextDesign>
   {
   
        private IQuoteOPMContext currentContext;
        public QuoteOPTemplateTextDesignRepository(int tenant)
        {
            currentContext = QuoteOPMContext.GetContext(tenant);
        }

        public QuoteOPTemplateTextDesignRepository(IQuoteOPMContext context)
        {
            currentContext = context;
        }

		 
		
		public  QuoteOPTemplateTextDesign GetSingle(string id, int tenant)
        {
            return (from a in context.QuoteOPTemplateTextDesigns
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<QuoteOPTemplateTextDesign> GetAll(int tenant)
        {
            return from a in context.QuoteOPTemplateTextDesigns  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public QuoteOPTemplateTextDesign GetSingle(EntityKeyFields entityKeys)
        {
            QuoteOPTemplateTextDesignKeys keys = entityKeys as QuoteOPTemplateTextDesignKeys;
            return (from a in context.QuoteOPTemplateTextDesigns
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(QuoteOPTemplateTextDesign entity)
        {
            onAdd();
            context.QuoteOPTemplateTextDesigns.Add(entity);
        }

        public void Remove(QuoteOPTemplateTextDesign entity)
        {
            context.QuoteOPTemplateTextDesigns.Attach(entity);
            context.QuoteOPTemplateTextDesigns.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(QuoteOPTemplateTextDesign entity)
        {
            onUpdate();
            context.QuoteOPTemplateTextDesigns.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuoteOPTemplateTextDesign> All()
        {
            return context.QuoteOPTemplateTextDesigns.ToList();
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
	 