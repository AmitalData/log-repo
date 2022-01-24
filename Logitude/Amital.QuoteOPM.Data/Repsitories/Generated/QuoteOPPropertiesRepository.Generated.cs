 
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
   public partial class QuoteOPPropertiesRepository:IRepository<QuoteOPProperties>
   {
   
        private IQuoteOPMContext currentContext;
        public QuoteOPPropertiesRepository(int tenant)
        {
            currentContext = QuoteOPMContext.GetContext(tenant);
        }

        public QuoteOPPropertiesRepository(IQuoteOPMContext context)
        {
            currentContext = context;
        }

		 
		
		public  QuoteOPProperties GetSingle(string id, int tenant)
        {
            return (from a in context.QuoteOPPropertiess
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<QuoteOPProperties> GetAll(int tenant)
        {
            return from a in context.QuoteOPPropertiess  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public QuoteOPProperties GetSingle(EntityKeyFields entityKeys)
        {
            QuoteOPPropertiesKeys keys = entityKeys as QuoteOPPropertiesKeys;
            return (from a in context.QuoteOPPropertiess
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(QuoteOPProperties entity)
        {
            onAdd();
            context.QuoteOPPropertiess.Add(entity);
        }

        public void Remove(QuoteOPProperties entity)
        {
            context.QuoteOPPropertiess.Attach(entity);
            context.QuoteOPPropertiess.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(QuoteOPProperties entity)
        {
            onUpdate();
            context.QuoteOPPropertiess.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuoteOPProperties> All()
        {
            return context.QuoteOPPropertiess.ToList();
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
	 