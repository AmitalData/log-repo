 
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
   public partial class QuoteOPTemplateQueryService: EntityQueryService<QuoteOPTemplate,QuoteOPTemplateKeys,QuoteOPTemplatePM,object,QuoteOPTemplateKeys>
   {
   
        QuoteOPTemplateRepository repository;
		IQuoteOPMContext  context;
        public QuoteOPTemplateQueryService(int tenant)
        {
		    context = QuoteOPMContext.GetContext(tenant);
            MainContext = context;
            repository = new QuoteOPTemplateRepository(context);
            Repository = repository;
            mapping = new QuoteOPTemplateDataMapping();
        }

        public QuoteOPTemplateQueryService(QuoteOPTemplateRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new QuoteOPTemplateDataMapping();
        }

        public QuoteOPTemplateQueryService(IQuoteOPMContext context)
        {
            this.repository = new QuoteOPTemplateRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new QuoteOPTemplateDataMapping();
        }
		 
		public  QuoteOPTemplatePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new QuoteOPTemplateKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(QuoteOPTemplate entityPOCO)
        {
            QuoteOPTemplateKeys entityKeys = new QuoteOPTemplateKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 