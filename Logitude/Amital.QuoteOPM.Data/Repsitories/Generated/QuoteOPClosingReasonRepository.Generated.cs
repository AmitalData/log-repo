 
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
   public partial class QuoteOPClosingReasonRepository:IRepository<QuoteOPClosingReason>
   {
   
        private IQuoteOPMContext currentContext;
        public QuoteOPClosingReasonRepository(int tenant)
        {
            currentContext = QuoteOPMContext.GetContext(tenant);
        }

        public QuoteOPClosingReasonRepository(IQuoteOPMContext context)
        {
            currentContext = context;
        }

		 
		
		public  QuoteOPClosingReason GetSingle(string id, int tenant)
        {
            return (from a in context.QuoteOPClosingReasons
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<QuoteOPClosingReason> GetAll(int tenant)
        {
            return from a in context.QuoteOPClosingReasons  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public QuoteOPClosingReason GetSingle(EntityKeyFields entityKeys)
        {
            QuoteOPClosingReasonKeys keys = entityKeys as QuoteOPClosingReasonKeys;
            return (from a in context.QuoteOPClosingReasons
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(QuoteOPClosingReason entity)
        {
            onAdd();
            context.QuoteOPClosingReasons.Add(entity);
        }

        public void Remove(QuoteOPClosingReason entity)
        {
            context.QuoteOPClosingReasons.Attach(entity);
            context.QuoteOPClosingReasons.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(QuoteOPClosingReason entity)
        {
            onUpdate();
            context.QuoteOPClosingReasons.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuoteOPClosingReason> All()
        {
            return context.QuoteOPClosingReasons.ToList();
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
	 