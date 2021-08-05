 
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
   public partial class QuoteOPComputedFieldQueryService: EntityQueryService<QuoteOPComputedField,QuoteOPComputedFieldKeys,QuoteOPComputedFieldPM,object,QuoteOPComputedFieldKeys>
   {
   
        QuoteOPComputedFieldRepository repository;
		IQuoteOPMContext  context;
        public QuoteOPComputedFieldQueryService(int tenant)
        {
		    context = QuoteOPMContext.GetContext(tenant);
            MainContext = context;
            repository = new QuoteOPComputedFieldRepository(context);
            Repository = repository;
            mapping = new QuoteOPComputedFieldDataMapping();
        }

        public QuoteOPComputedFieldQueryService(QuoteOPComputedFieldRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new QuoteOPComputedFieldDataMapping();
        }

        public QuoteOPComputedFieldQueryService(IQuoteOPMContext context)
        {
            this.repository = new QuoteOPComputedFieldRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new QuoteOPComputedFieldDataMapping();
        }
		 
		public  QuoteOPComputedFieldPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new QuoteOPComputedFieldKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(QuoteOPComputedField entityPOCO)
        {
            QuoteOPComputedFieldKeys entityKeys = new QuoteOPComputedFieldKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 