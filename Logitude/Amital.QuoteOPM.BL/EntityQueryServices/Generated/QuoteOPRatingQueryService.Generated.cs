 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Def.EntityPMs;
using Amital.QuoteOPM.BL.EntityDataMappings;
using Amital.QuoteOPM.Data.Repsitories;
using Amital.QuoteOPM.Data.EntityKeys;
using Amital.QuoteOPM.Data;
using Simplog.Server.Infrastructure;
namespace Amital.QuoteOPM.BL.EntityQueryServices
{ 
   public partial class QuoteOPRatingQueryService: EntityQueryService<QuoteOPRating,QuoteOPRatingKeys,QuoteOPRatingPM,object,QuoteOPRatingKeys>
   {
   
        QuoteOPRatingRepository repository;
		IQuoteOPMContext  context;
        public QuoteOPRatingQueryService(int tenant)
        {
		    context = QuoteOPMContext.GetContext(tenant);
            MainContext = context;
            repository = new QuoteOPRatingRepository(context);
            Repository = repository;
            mapping = new QuoteOPRatingDataMapping();
        }

        public QuoteOPRatingQueryService(QuoteOPRatingRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new QuoteOPRatingDataMapping();
        }

        public QuoteOPRatingQueryService(IQuoteOPMContext context)
        {
            this.repository = new QuoteOPRatingRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new QuoteOPRatingDataMapping();
        }
		 
		public  QuoteOPRatingPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new QuoteOPRatingKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(QuoteOPRating entityPOCO)
        {
            QuoteOPRatingKeys entityKeys = new QuoteOPRatingKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 