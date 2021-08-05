 
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
   public partial class QuoteOPSettingQueryService: EntityQueryService<QuoteOPSetting,QuoteOPSettingKeys,QuoteOPSettingPM,object,QuoteOPSettingKeys>
   {
   
        QuoteOPSettingRepository repository;
		IQuoteOPMContext  context;
        public QuoteOPSettingQueryService(int tenant)
        {
		    context = QuoteOPMContext.GetContext(tenant);
            MainContext = context;
            repository = new QuoteOPSettingRepository(context);
            Repository = repository;
            mapping = new QuoteOPSettingDataMapping();
        }

        public QuoteOPSettingQueryService(QuoteOPSettingRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new QuoteOPSettingDataMapping();
        }

        public QuoteOPSettingQueryService(IQuoteOPMContext context)
        {
            this.repository = new QuoteOPSettingRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new QuoteOPSettingDataMapping();
        }
		 
		public  QuoteOPSettingPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new QuoteOPSettingKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(QuoteOPSetting entityPOCO)
        {
            QuoteOPSettingKeys entityKeys = new QuoteOPSettingKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 