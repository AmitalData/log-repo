 
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
   public partial class QuoteOPTemplateTableDesignRepository:IRepository<QuoteOPTemplateTableDesign>
   {
   
        private IQuoteOPMContext currentContext;
        public QuoteOPTemplateTableDesignRepository(int tenant)
        {
            currentContext = QuoteOPMContext.GetContext(tenant);
        }

        public QuoteOPTemplateTableDesignRepository(IQuoteOPMContext context)
        {
            currentContext = context;
        }

		 
		
		public  QuoteOPTemplateTableDesign GetSingle(string id, int tenant)
        {
            return (from a in context.QuoteOPTemplateTableDesigns
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<QuoteOPTemplateTableDesign> GetAll(int tenant)
        {
            return from a in context.QuoteOPTemplateTableDesigns  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public QuoteOPTemplateTableDesign GetSingle(EntityKeyFields entityKeys)
        {
            QuoteOPTemplateTableDesignKeys keys = entityKeys as QuoteOPTemplateTableDesignKeys;
            return (from a in context.QuoteOPTemplateTableDesigns
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(QuoteOPTemplateTableDesign entity)
        {
            onAdd();
            context.QuoteOPTemplateTableDesigns.Add(entity);
        }

        public void Remove(QuoteOPTemplateTableDesign entity)
        {
            context.QuoteOPTemplateTableDesigns.Attach(entity);
            context.QuoteOPTemplateTableDesigns.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(QuoteOPTemplateTableDesign entity)
        {
            onUpdate();
            context.QuoteOPTemplateTableDesigns.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuoteOPTemplateTableDesign> All()
        {
            return context.QuoteOPTemplateTableDesigns.ToList();
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
	 