 
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
   public partial class QuoteOPTemplateTextCodeRepository:IRepository<QuoteOPTemplateTextCode>
   {
   
        private IQuoteOPMContext currentContext;
        public QuoteOPTemplateTextCodeRepository(int tenant)
        {
            currentContext = QuoteOPMContext.GetContext(tenant);
        }

        public QuoteOPTemplateTextCodeRepository(IQuoteOPMContext context)
        {
            currentContext = context;
        }

		 
		
		public  QuoteOPTemplateTextCode GetSingle(string id, int tenant)
        {
            return (from a in context.QuoteOPTemplateTextCodes
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<QuoteOPTemplateTextCode> GetAll(int tenant)
        {
            return from a in context.QuoteOPTemplateTextCodes  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public QuoteOPTemplateTextCode GetSingle(EntityKeyFields entityKeys)
        {
            QuoteOPTemplateTextCodeKeys keys = entityKeys as QuoteOPTemplateTextCodeKeys;
            return (from a in context.QuoteOPTemplateTextCodes
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(QuoteOPTemplateTextCode entity)
        {
            onAdd();
            context.QuoteOPTemplateTextCodes.Add(entity);
        }

        public void Remove(QuoteOPTemplateTextCode entity)
        {
            context.QuoteOPTemplateTextCodes.Attach(entity);
            context.QuoteOPTemplateTextCodes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(QuoteOPTemplateTextCode entity)
        {
            onUpdate();
            context.QuoteOPTemplateTextCodes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuoteOPTemplateTextCode> All()
        {
            return context.QuoteOPTemplateTextCodes.ToList();
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
	 