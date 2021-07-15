 
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
   public partial class QuoteOPSaleChargeRepository:IRepository<QuoteOPSaleCharge>
   {
   
        private IQuoteOPMContext currentContext;
        public QuoteOPSaleChargeRepository(int tenant)
        {
            currentContext = QuoteOPMContext.GetContext(tenant);
        }

        public QuoteOPSaleChargeRepository(IQuoteOPMContext context)
        {
            currentContext = context;
        }

		 
		
		public  QuoteOPSaleCharge GetSingle(, int tenant)
        {
            return (from a in context.NONE
                    where  && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<QuoteOPSaleCharge> GetAll(int tenant)
        {
            return from a in context.NONE  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public QuoteOPSaleCharge GetSingle(EntityKeyFields entityKeys)
        {
            QuoteOPSaleChargeKeys keys = entityKeys as QuoteOPSaleChargeKeys;
            return (from a in context.NONE
                    where 
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(QuoteOPSaleCharge entity)
        {
            onAdd();
            context.NONE.Add(entity);
        }

        public void Remove(QuoteOPSaleCharge entity)
        {
            context.NONE.Attach(entity);
            context.NONE.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(QuoteOPSaleCharge entity)
        {
            onUpdate();
            context.NONE.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuoteOPSaleCharge> All()
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
	 