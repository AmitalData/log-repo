 
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
   public partial class QuoteOPClosingReasonQueryService: EntityQueryService<QuoteOPClosingReason,QuoteOPClosingReasonKeys,QuoteOPClosingReasonPM,object,QuoteOPClosingReasonKeys>
   {
   
        QuoteOPClosingReasonRepository repository;
		IQuoteOPMContext  context;
        public QuoteOPClosingReasonQueryService(int tenant)
        {
		    context = QuoteOPMContext.GetContext(tenant);
            MainContext = context;
            repository = new QuoteOPClosingReasonRepository(context);
            Repository = repository;
            mapping = new QuoteOPClosingReasonDataMapping();
        }

        public QuoteOPClosingReasonQueryService(QuoteOPClosingReasonRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new QuoteOPClosingReasonDataMapping();
        }

        public QuoteOPClosingReasonQueryService(IQuoteOPMContext context)
        {
            this.repository = new QuoteOPClosingReasonRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new QuoteOPClosingReasonDataMapping();
        }
		 
		public  QuoteOPClosingReasonPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new QuoteOPClosingReasonKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(QuoteOPClosingReason entityPOCO)
        {
            QuoteOPClosingReasonKeys entityKeys = new QuoteOPClosingReasonKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 