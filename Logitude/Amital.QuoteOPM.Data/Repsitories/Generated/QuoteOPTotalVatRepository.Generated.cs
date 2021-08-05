 
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
   public partial class QuoteOPTotalVATRepository:IRepository<QuoteOPTotalVAT>
   {
   
        private IQuoteOPMContext currentContext;
        public QuoteOPTotalVATRepository(int tenant)
        {
            currentContext = QuoteOPMContext.GetContext(tenant);
        }

        public QuoteOPTotalVATRepository(IQuoteOPMContext context)
        {
            currentContext = context;
        }

		 
		
		public  QuoteOPTotalVAT GetSingle(string id, int tenant)
        {
            return (from a in context.QuoteOPTotalVATs
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<QuoteOPTotalVAT> GetAll(int tenant)
        {
            return from a in context.QuoteOPTotalVATs  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public QuoteOPTotalVAT GetSingle(EntityKeyFields entityKeys)
        {
            QuoteOPTotalVATKeys keys = entityKeys as QuoteOPTotalVATKeys;
            return (from a in context.QuoteOPTotalVATs
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(QuoteOPTotalVAT entity)
        {
            onAdd();
            context.QuoteOPTotalVATs.Add(entity);
        }

        public void Remove(QuoteOPTotalVAT entity)
        {
            context.QuoteOPTotalVATs.Attach(entity);
            context.QuoteOPTotalVATs.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(QuoteOPTotalVAT entity)
        {
            onUpdate();
            context.QuoteOPTotalVATs.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuoteOPTotalVAT> All()
        {
            return context.QuoteOPTotalVATs.ToList();
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
	 