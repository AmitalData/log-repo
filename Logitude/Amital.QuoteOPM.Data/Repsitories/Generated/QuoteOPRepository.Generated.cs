 
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
   public partial class QuoteOPRepository:IRepository<QuoteOP>
   {
   
        private IQuoteOPMContext currentContext;
        public QuoteOPRepository(int tenant)
        {
            currentContext = QuoteOPMContext.GetContext(tenant);
        }

        public QuoteOPRepository(IQuoteOPMContext context)
        {
            currentContext = context;
        }

		 
		
		public  QuoteOP GetSingle(string id, int tenant)
        {
            return (from a in context.QuoteOPs
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<QuoteOP> GetAll(int tenant)
        {
            return from a in context.QuoteOPs  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public QuoteOP GetSingle(EntityKeyFields entityKeys)
        {
            QuoteOPKeys keys = entityKeys as QuoteOPKeys;
            return (from a in context.QuoteOPs
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(QuoteOP entity)
        {
            onAdd();
            context.QuoteOPs.Add(entity);
        }

        public void Remove(QuoteOP entity)
        {
            context.QuoteOPs.Attach(entity);
            context.QuoteOPs.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(QuoteOP entity)
        {
            onUpdate();
            context.QuoteOPs.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuoteOP> All()
        {
            return context.QuoteOPs.ToList();
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
	 