 
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
   public partial class QuoteOPQueryService: EntityQueryService<QuoteOP,QuoteOPKeys,QuoteOPPM,object,QuoteOPKeys>
   {
   
        QuoteOPRepository repository;
		IQuoteOPMContext  context;
        public QuoteOPQueryService(int tenant)
        {
		    context = QuoteOPMContext.GetContext(tenant);
            MainContext = context;
            repository = new QuoteOPRepository(context);
            Repository = repository;
            mapping = new QuoteOPDataMapping();
        }

        public QuoteOPQueryService(QuoteOPRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new QuoteOPDataMapping();
        }

        public QuoteOPQueryService(IQuoteOPMContext context)
        {
            this.repository = new QuoteOPRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new QuoteOPDataMapping();
        }
		 
		public  QuoteOPPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new QuoteOPKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(QuoteOP entityPOCO)
        {
            QuoteOPKeys entityKeys = new QuoteOPKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 