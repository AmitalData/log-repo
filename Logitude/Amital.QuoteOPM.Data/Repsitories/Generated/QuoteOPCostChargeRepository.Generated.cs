 
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
   public partial class QuoteOPCostChargeRepository:IRepository<QuoteOPCostCharge>
   {
   
        private IQuoteOPMContext currentContext;
        public QuoteOPCostChargeRepository(int tenant)
        {
            currentContext = QuoteOPMContext.GetContext(tenant);
        }

        public QuoteOPCostChargeRepository(IQuoteOPMContext context)
        {
            currentContext = context;
        }

		 
		
		public  QuoteOPCostCharge GetSingle(, int tenant)
        {
            return (from a in context.NONE
                    where  && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<QuoteOPCostCharge> GetAll(int tenant)
        {
            return from a in context.NONE  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public QuoteOPCostCharge GetSingle(EntityKeyFields entityKeys)
        {
            QuoteOPCostChargeKeys keys = entityKeys as QuoteOPCostChargeKeys;
            return (from a in context.NONE
                    where 
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(QuoteOPCostCharge entity)
        {
            onAdd();
            context.NONE.Add(entity);
        }

        public void Remove(QuoteOPCostCharge entity)
        {
            context.NONE.Attach(entity);
            context.NONE.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(QuoteOPCostCharge entity)
        {
            onUpdate();
            context.NONE.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuoteOPCostCharge> All()
        {
            return context.NONE.ToList();
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
	 