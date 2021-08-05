 
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
   public partial class QuoteOPTypeRepository:IRepository<QuoteOPType>
   {
   
        private IQuoteOPMContext currentContext;
        public QuoteOPTypeRepository(int tenant)
        {
            currentContext = QuoteOPMContext.GetContext(tenant);
        }

        public QuoteOPTypeRepository(IQuoteOPMContext context)
        {
            currentContext = context;
        }

		 
		
		public  QuoteOPType GetSingle(string code)
        {
            return (from a in context.QuoteOPTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<QuoteOPType> GetAll()
        {
            return from a in context.QuoteOPTypes  
                   select a;
        }
				 
        public QuoteOPType GetSingle(EntityKeyFields entityKeys)
        {
            QuoteOPTypeKeys keys = entityKeys as QuoteOPTypeKeys;
            return (from a in context.QuoteOPTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(QuoteOPType entity)
        {
            onAdd();
            context.QuoteOPTypes.Add(entity);
        }

        public void Remove(QuoteOPType entity)
        {
            context.QuoteOPTypes.Attach(entity);
            context.QuoteOPTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(QuoteOPType entity)
        {
            onUpdate();
            context.QuoteOPTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuoteOPType> All()
        {
            return context.QuoteOPTypes.ToList();
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
	 