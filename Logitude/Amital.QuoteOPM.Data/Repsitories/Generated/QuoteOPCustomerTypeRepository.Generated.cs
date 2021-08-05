 
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
   public partial class QuoteOPCustomerTypeRepository:IRepository<QuoteOPCustomerType>
   {
   
        private IQuoteOPMContext currentContext;
        public QuoteOPCustomerTypeRepository(int tenant)
        {
            currentContext = QuoteOPMContext.GetContext(tenant);
        }

        public QuoteOPCustomerTypeRepository(IQuoteOPMContext context)
        {
            currentContext = context;
        }

		 
		
		public  QuoteOPCustomerType GetSingle(string code)
        {
            return (from a in context.QuoteOPCustomerTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<QuoteOPCustomerType> GetAll()
        {
            return from a in context.QuoteOPCustomerTypes  
                   select a;
        }
				 
        public QuoteOPCustomerType GetSingle(EntityKeyFields entityKeys)
        {
            QuoteOPCustomerTypeKeys keys = entityKeys as QuoteOPCustomerTypeKeys;
            return (from a in context.QuoteOPCustomerTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(QuoteOPCustomerType entity)
        {
            onAdd();
            context.QuoteOPCustomerTypes.Add(entity);
        }

        public void Remove(QuoteOPCustomerType entity)
        {
            context.QuoteOPCustomerTypes.Attach(entity);
            context.QuoteOPCustomerTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(QuoteOPCustomerType entity)
        {
            onUpdate();
            context.QuoteOPCustomerTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuoteOPCustomerType> All()
        {
            return context.QuoteOPCustomerTypes.ToList();
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
	 