 
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
   public partial class QuoteOPPropertiesQueryService: EntityQueryService<QuoteOPProperties,QuoteOPPropertiesKeys,QuoteOPPropertiesPM,QuoteOPPM,QuoteOPKeys>
   {
   
        QuoteOPPropertiesRepository repository;
		IQuoteOPMContext  context;
        public QuoteOPPropertiesQueryService(int tenant)
        {
		    context = QuoteOPMContext.GetContext(tenant);
            MainContext = context;
            repository = new QuoteOPPropertiesRepository(context);
            Repository = repository;
            mapping = new QuoteOPPropertiesDataMapping();
        }

        public QuoteOPPropertiesQueryService(QuoteOPPropertiesRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new QuoteOPPropertiesDataMapping();
        }

        public QuoteOPPropertiesQueryService(IQuoteOPMContext context)
        {
            this.repository = new QuoteOPPropertiesRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new QuoteOPPropertiesDataMapping();
        }
		 
		public  QuoteOPPropertiesPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new QuoteOPPropertiesKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(QuoteOPProperties entityPOCO)
        {
            QuoteOPPropertiesKeys entityKeys = new QuoteOPPropertiesKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 