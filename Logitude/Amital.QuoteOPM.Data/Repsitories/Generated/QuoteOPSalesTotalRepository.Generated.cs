 
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
   public partial class QuoteOPSalesTotalRepository:IRepository<QuoteOPSalesTotal>
   {
   
        private IQuoteOPMContext currentContext;
        public QuoteOPSalesTotalRepository(int tenant)
        {
            currentContext = QuoteOPMContext.GetContext(tenant);
        }

        public QuoteOPSalesTotalRepository(IQuoteOPMContext context)
        {
            currentContext = context;
        }

		 
		
		public  QuoteOPSalesTotal GetSingle()
        {
            return (from a in context.NONE
                    where  
                    select a).FirstOrDefault();
        }

        public IQueryable<QuoteOPSalesTotal> GetAll()
        {
            return from a in context.NONE  
                   select a;
        }
				 
        public QuoteOPSalesTotal GetSingle(EntityKeyFields entityKeys)
        {
            QuoteOPSalesTotalKeys keys = entityKeys as QuoteOPSalesTotalKeys;
            return (from a in context.NONE
                    where 
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(QuoteOPSalesTotal entity)
        {
            onAdd();
            context.NONE.Add(entity);
        }

        public void Remove(QuoteOPSalesTotal entity)
        {
            context.NONE.Attach(entity);
            context.NONE.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(QuoteOPSalesTotal entity)
        {
            onUpdate();
            context.NONE.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuoteOPSalesTotal> All()
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
	 