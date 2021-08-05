 
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
   public partial class QuoteOPSaleChargeQueryService: EntityQueryService<QuoteOPSaleCharge,QuoteOPSaleChargeKeys,QuoteOPSaleChargePM,object,QuoteOPSaleChargeKeys>
   {
   
        QuoteOPSaleChargeRepository repository;
		IQuoteOPMContext  context;
        public QuoteOPSaleChargeQueryService(int tenant)
        {
		    context = QuoteOPMContext.GetContext(tenant);
            MainContext = context;
            repository = new QuoteOPSaleChargeRepository(context);
            Repository = repository;
            mapping = new QuoteOPSaleChargeDataMapping();
        }

        public QuoteOPSaleChargeQueryService(QuoteOPSaleChargeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new QuoteOPSaleChargeDataMapping();
        }

        public QuoteOPSaleChargeQueryService(IQuoteOPMContext context)
        {
            this.repository = new QuoteOPSaleChargeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new QuoteOPSaleChargeDataMapping();
        }
		 
		public  QuoteOPSaleChargePM GetSingle(,bool getComposition, bool getFromCache)
        {
             EntityKeys = new QuoteOPSaleChargeKeys(){  };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(QuoteOPSaleCharge entityPOCO)
        {
            QuoteOPSaleChargeKeys entityKeys = new QuoteOPSaleChargeKeys() {  };
            return entityKeys;
        }
     
	 
   }
   
}
	 