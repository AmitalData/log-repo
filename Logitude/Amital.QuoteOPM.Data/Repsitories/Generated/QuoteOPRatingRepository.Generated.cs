 
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
   public partial class QuoteOPRatingRepository:IRepository<QuoteOPRating>
   {
   
        private IQuoteOPMContext currentContext;
        public QuoteOPRatingRepository(int tenant)
        {
            currentContext = QuoteOPMContext.GetContext(tenant);
        }

        public QuoteOPRatingRepository(IQuoteOPMContext context)
        {
            currentContext = context;
        }

		 
		
		public  QuoteOPRating GetSingle(string code)
        {
            return (from a in context.QuoteOPRatings
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<QuoteOPRating> GetAll()
        {
            return from a in context.QuoteOPRatings  
                   select a;
        }
				 
        public QuoteOPRating GetSingle(EntityKeyFields entityKeys)
        {
            QuoteOPRatingKeys keys = entityKeys as QuoteOPRatingKeys;
            return (from a in context.QuoteOPRatings
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(QuoteOPRating entity)
        {
            onAdd();
            context.QuoteOPRatings.Add(entity);
        }

        public void Remove(QuoteOPRating entity)
        {
            context.QuoteOPRatings.Attach(entity);
            context.QuoteOPRatings.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(QuoteOPRating entity)
        {
            onUpdate();
            context.QuoteOPRatings.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuoteOPRating> All()
        {
            return context.QuoteOPRatings.ToList();
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
	 