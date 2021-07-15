 
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
   public partial class QuoteOPPriceStepsQueryService: EntityQueryService<QuoteOPPriceSteps,QuoteOPPriceStepsKeys,QuoteOPPriceStepsPM,QuoteOPChargePM,QuoteOPChargeKeys>
   {
   
        QuoteOPPriceStepsRepository repository;
		IQuoteOPMContext  context;
        public QuoteOPPriceStepsQueryService(int tenant)
        {
		    context = QuoteOPMContext.GetContext(tenant);
            MainContext = context;
            repository = new QuoteOPPriceStepsRepository(context);
            Repository = repository;
            mapping = new QuoteOPPriceStepsDataMapping();
        }

        public QuoteOPPriceStepsQueryService(QuoteOPPriceStepsRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new QuoteOPPriceStepsDataMapping();
        }

        public QuoteOPPriceStepsQueryService(IQuoteOPMContext context)
        {
            this.repository = new QuoteOPPriceStepsRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new QuoteOPPriceStepsDataMapping();
        }
		 
		public  QuoteOPPriceStepsPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new QuoteOPPriceStepsKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(QuoteOPPriceSteps entityPOCO)
        {
            QuoteOPPriceStepsKeys entityKeys = new QuoteOPPriceStepsKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 