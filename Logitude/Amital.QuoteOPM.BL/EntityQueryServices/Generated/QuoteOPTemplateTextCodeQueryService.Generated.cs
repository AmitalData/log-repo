 
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
   public partial class QuoteOPTemplateTextCodeQueryService: EntityQueryService<QuoteOPTemplateTextCode,QuoteOPTemplateTextCodeKeys,QuoteOPTemplateTextCodePM,object,QuoteOPTemplateTextCodeKeys>
   {
   
        QuoteOPTemplateTextCodeRepository repository;
		IQuoteOPMContext  context;
        public QuoteOPTemplateTextCodeQueryService(int tenant)
        {
		    context = QuoteOPMContext.GetContext(tenant);
            MainContext = context;
            repository = new QuoteOPTemplateTextCodeRepository(context);
            Repository = repository;
            mapping = new QuoteOPTemplateTextCodeDataMapping();
        }

        public QuoteOPTemplateTextCodeQueryService(QuoteOPTemplateTextCodeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new QuoteOPTemplateTextCodeDataMapping();
        }

        public QuoteOPTemplateTextCodeQueryService(IQuoteOPMContext context)
        {
            this.repository = new QuoteOPTemplateTextCodeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new QuoteOPTemplateTextCodeDataMapping();
        }
		 
		public  QuoteOPTemplateTextCodePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new QuoteOPTemplateTextCodeKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(QuoteOPTemplateTextCode entityPOCO)
        {
            QuoteOPTemplateTextCodeKeys entityKeys = new QuoteOPTemplateTextCodeKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 