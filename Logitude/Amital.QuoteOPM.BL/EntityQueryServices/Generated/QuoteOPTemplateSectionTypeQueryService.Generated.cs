 
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
   public partial class QuoteOPTemplateSectionTypeQueryService: EntityQueryService<QuoteOPTemplateSectionType,QuoteOPTemplateSectionTypeKeys,QuoteOPTemplateSectionTypePM,object,QuoteOPTemplateSectionTypeKeys>
   {
   
        QuoteOPTemplateSectionTypeRepository repository;
		IQuoteOPMContext  context;
        public QuoteOPTemplateSectionTypeQueryService(int tenant)
        {
		    context = QuoteOPMContext.GetContext(tenant);
            MainContext = context;
            repository = new QuoteOPTemplateSectionTypeRepository(context);
            Repository = repository;
            mapping = new QuoteOPTemplateSectionTypeDataMapping();
        }

        public QuoteOPTemplateSectionTypeQueryService(QuoteOPTemplateSectionTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new QuoteOPTemplateSectionTypeDataMapping();
        }

        public QuoteOPTemplateSectionTypeQueryService(IQuoteOPMContext context)
        {
            this.repository = new QuoteOPTemplateSectionTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new QuoteOPTemplateSectionTypeDataMapping();
        }
		 
		public  QuoteOPTemplateSectionTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new QuoteOPTemplateSectionTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(QuoteOPTemplateSectionType entityPOCO)
        {
            QuoteOPTemplateSectionTypeKeys entityKeys = new QuoteOPTemplateSectionTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 