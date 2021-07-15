 
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
   public partial class QuoteOPComputedFieldRepository:IRepository<QuoteOPComputedField>
   {
   
        private IQuoteOPMContext currentContext;
        public QuoteOPComputedFieldRepository(int tenant)
        {
            currentContext = QuoteOPMContext.GetContext(tenant);
        }

        public QuoteOPComputedFieldRepository(IQuoteOPMContext context)
        {
            currentContext = context;
        }

		 
		
		public  QuoteOPComputedField GetSingle(string id, int tenant)
        {
            return (from a in context.QuoteOPComputedFields
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<QuoteOPComputedField> GetAll(int tenant)
        {
            return from a in context.QuoteOPComputedFields  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public QuoteOPComputedField GetSingle(EntityKeyFields entityKeys)
        {
            QuoteOPComputedFieldKeys keys = entityKeys as QuoteOPComputedFieldKeys;
            return (from a in context.QuoteOPComputedFields
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(QuoteOPComputedField entity)
        {
            onAdd();
            context.QuoteOPComputedFields.Add(entity);
        }

        public void Remove(QuoteOPComputedField entity)
        {
            context.QuoteOPComputedFields.Attach(entity);
            context.QuoteOPComputedFields.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(QuoteOPComputedField entity)
        {
            onUpdate();
            context.QuoteOPComputedFields.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuoteOPComputedField> All()
        {
            return context.QuoteOPComputedFields.ToList();
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
	 