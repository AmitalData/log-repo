 
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
   public partial class QuoteOPPriceStepsRepository:IRepository<QuoteOPPriceSteps>
   {
   
        private IQuoteOPMContext currentContext;
        public QuoteOPPriceStepsRepository(int tenant)
        {
            currentContext = QuoteOPMContext.GetContext(tenant);
        }

        public QuoteOPPriceStepsRepository(IQuoteOPMContext context)
        {
            currentContext = context;
        }

		 
		
		public  QuoteOPPriceSteps GetSingle(string id, int tenant)
        {
            return (from a in context.QuoteOPPriceSteps
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<QuoteOPPriceSteps> GetAll(int tenant)
        {
            return from a in context.QuoteOPPriceSteps  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public QuoteOPPriceSteps GetSingle(EntityKeyFields entityKeys)
        {
            QuoteOPPriceStepsKeys keys = entityKeys as QuoteOPPriceStepsKeys;
            return (from a in context.QuoteOPPriceSteps
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(QuoteOPPriceSteps entity)
        {
            onAdd();
            context.QuoteOPPriceSteps.Add(entity);
        }

        public void Remove(QuoteOPPriceSteps entity)
        {
            context.QuoteOPPriceSteps.Attach(entity);
            context.QuoteOPPriceSteps.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(QuoteOPPriceSteps entity)
        {
            onUpdate();
            context.QuoteOPPriceSteps.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuoteOPPriceSteps> All()
        {
            return context.QuoteOPPriceSteps.ToList();
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
	 