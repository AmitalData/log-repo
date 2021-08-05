 
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
   public partial class QuoteOPStageQueryService: EntityQueryService<QuoteOPStage,QuoteOPStageKeys,QuoteOPStagePM,object,QuoteOPStageKeys>
   {
   
        QuoteOPStageRepository repository;
		IQuoteOPMContext  context;
        public QuoteOPStageQueryService(int tenant)
        {
		    context = QuoteOPMContext.GetContext(tenant);
            MainContext = context;
            repository = new QuoteOPStageRepository(context);
            Repository = repository;
            mapping = new QuoteOPStageDataMapping();
        }

        public QuoteOPStageQueryService(QuoteOPStageRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new QuoteOPStageDataMapping();
        }

        public QuoteOPStageQueryService(IQuoteOPMContext context)
        {
            this.repository = new QuoteOPStageRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new QuoteOPStageDataMapping();
        }
		 
		public  QuoteOPStagePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new QuoteOPStageKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(QuoteOPStage entityPOCO)
        {
            QuoteOPStageKeys entityKeys = new QuoteOPStageKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 