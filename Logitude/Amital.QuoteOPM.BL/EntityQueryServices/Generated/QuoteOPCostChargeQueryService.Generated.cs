 
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
   public partial class QuoteOPCostChargeQueryService: EntityQueryService<QuoteOPCostCharge,QuoteOPCostChargeKeys,QuoteOPCostChargePM,object,QuoteOPCostChargeKeys>
   {
   
        QuoteOPCostChargeRepository repository;
		IQuoteOPMContext  context;
        public QuoteOPCostChargeQueryService(int tenant)
        {
		    context = QuoteOPMContext.GetContext(tenant);
            MainContext = context;
            repository = new QuoteOPCostChargeRepository(context);
            Repository = repository;
            mapping = new QuoteOPCostChargeDataMapping();
        }

        public QuoteOPCostChargeQueryService(QuoteOPCostChargeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new QuoteOPCostChargeDataMapping();
        }

        public QuoteOPCostChargeQueryService(IQuoteOPMContext context)
        {
            this.repository = new QuoteOPCostChargeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new QuoteOPCostChargeDataMapping();
        }
		 
		public  QuoteOPCostChargePM GetSingle(,bool getComposition, bool getFromCache)
        {
             EntityKeys = new QuoteOPCostChargeKeys(){  };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(QuoteOPCostCharge entityPOCO)
        {
            QuoteOPCostChargeKeys entityKeys = new QuoteOPCostChargeKeys() {  };
            return entityKeys;
        }
     
	 
   }
   
}
	 