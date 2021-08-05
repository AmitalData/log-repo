 
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
   public partial class QuoteOPTemplateSettingQueryService: EntityQueryService<QuoteOPTemplateSetting,QuoteOPTemplateSettingKeys,QuoteOPTemplateSettingPM,object,QuoteOPTemplateSettingKeys>
   {
   
        QuoteOPTemplateSettingRepository repository;
		IQuoteOPMContext  context;
        public QuoteOPTemplateSettingQueryService(int tenant)
        {
		    context = QuoteOPMContext.GetContext(tenant);
            MainContext = context;
            repository = new QuoteOPTemplateSettingRepository(context);
            Repository = repository;
            mapping = new QuoteOPTemplateSettingDataMapping();
        }

        public QuoteOPTemplateSettingQueryService(QuoteOPTemplateSettingRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new QuoteOPTemplateSettingDataMapping();
        }

        public QuoteOPTemplateSettingQueryService(IQuoteOPMContext context)
        {
            this.repository = new QuoteOPTemplateSettingRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new QuoteOPTemplateSettingDataMapping();
        }
		 
		public  QuoteOPTemplateSettingPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new QuoteOPTemplateSettingKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(QuoteOPTemplateSetting entityPOCO)
        {
            QuoteOPTemplateSettingKeys entityKeys = new QuoteOPTemplateSettingKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 