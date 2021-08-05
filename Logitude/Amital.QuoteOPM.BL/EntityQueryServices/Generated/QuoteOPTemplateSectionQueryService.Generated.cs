 
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
   public partial class QuoteOPTemplateSectionQueryService: EntityQueryService<QuoteOPTemplateSection,QuoteOPTemplateSectionKeys,QuoteOPTemplateSectionPM,object,QuoteOPTemplateSectionKeys>
   {
   
        QuoteOPTemplateSectionRepository repository;
		IQuoteOPMContext  context;
        public QuoteOPTemplateSectionQueryService(int tenant)
        {
		    context = QuoteOPMContext.GetContext(tenant);
            MainContext = context;
            repository = new QuoteOPTemplateSectionRepository(context);
            Repository = repository;
            mapping = new QuoteOPTemplateSectionDataMapping();
        }

        public QuoteOPTemplateSectionQueryService(QuoteOPTemplateSectionRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new QuoteOPTemplateSectionDataMapping();
        }

        public QuoteOPTemplateSectionQueryService(IQuoteOPMContext context)
        {
            this.repository = new QuoteOPTemplateSectionRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new QuoteOPTemplateSectionDataMapping();
        }
		 
		public  QuoteOPTemplateSectionPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new QuoteOPTemplateSectionKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(QuoteOPTemplateSection entityPOCO)
        {
            QuoteOPTemplateSectionKeys entityKeys = new QuoteOPTemplateSectionKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 