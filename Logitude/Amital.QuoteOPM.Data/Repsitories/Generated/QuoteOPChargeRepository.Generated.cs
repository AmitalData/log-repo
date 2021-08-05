 
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
   public partial class QuoteOPChargeRepository:IRepository<QuoteOPCharge>
   {
   
        private IQuoteOPMContext currentContext;
        public QuoteOPChargeRepository(int tenant)
        {
            currentContext = QuoteOPMContext.GetContext(tenant);
        }

        public QuoteOPChargeRepository(IQuoteOPMContext context)
        {
            currentContext = context;
        }

		 
		
		public  QuoteOPCharge GetSingle(string id, int tenant)
        {
            return (from a in context.QuoteOPCharges
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<QuoteOPCharge> GetAll(int tenant)
        {
            return from a in context.QuoteOPCharges  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public QuoteOPCharge GetSingle(EntityKeyFields entityKeys)
        {
            QuoteOPChargeKeys keys = entityKeys as QuoteOPChargeKeys;
            return (from a in context.QuoteOPCharges
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(QuoteOPCharge entity)
        {
            onAdd();
            context.QuoteOPCharges.Add(entity);
        }

        public void Remove(QuoteOPCharge entity)
        {
            context.QuoteOPCharges.Attach(entity);
            context.QuoteOPCharges.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(QuoteOPCharge entity)
        {
            onUpdate();
            context.QuoteOPCharges.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuoteOPCharge> All()
        {
            return context.QuoteOPCharges.ToList();
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
	 