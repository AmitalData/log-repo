 
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
   public partial class QuoteOPVATsTotalRepository:IRepository<QuoteOPVATsTotal>
   {
   
        private IQuoteOPMContext currentContext;
        public QuoteOPVATsTotalRepository(int tenant)
        {
            currentContext = QuoteOPMContext.GetContext(tenant);
        }

        public QuoteOPVATsTotalRepository(IQuoteOPMContext context)
        {
            currentContext = context;
        }

		 
		
		public  QuoteOPVATsTotal GetSingle()
        {
            return (from a in context.NONE
                    where  
                    select a).FirstOrDefault();
        }

        public IQueryable<QuoteOPVATsTotal> GetAll()
        {
            return from a in context.NONE  
                   select a;
        }
				 
        public QuoteOPVATsTotal GetSingle(EntityKeyFields entityKeys)
        {
            QuoteOPVATsTotalKeys keys = entityKeys as QuoteOPVATsTotalKeys;
            return (from a in context.NONE
                    where 
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(QuoteOPVATsTotal entity)
        {
            onAdd();
            context.NONE.Add(entity);
        }

        public void Remove(QuoteOPVATsTotal entity)
        {
            context.NONE.Attach(entity);
            context.NONE.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(QuoteOPVATsTotal entity)
        {
            onUpdate();
            context.NONE.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuoteOPVATsTotal> All()
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
	 