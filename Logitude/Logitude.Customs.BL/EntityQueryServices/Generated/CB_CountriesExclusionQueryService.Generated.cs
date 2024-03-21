 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Customs.BL.EntityQueryServices
{ 
   public partial class CB_CountriesExclusionQueryService: EntityQueryService<CB_CountriesExclusion,CB_CountriesExclusionKeys,CB_CountriesExclusionPM,object,CB_CountriesExclusionKeys>
   {
   
        CB_CountriesExclusionRepository repository;
		ICustomContext  context;
        public CB_CountriesExclusionQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CB_CountriesExclusionRepository(context);
            Repository = repository;
            mapping = new CB_CountriesExclusionDataMapping();
        }

        public CB_CountriesExclusionQueryService(CB_CountriesExclusionRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CB_CountriesExclusionDataMapping();
        }

        public CB_CountriesExclusionQueryService(ICustomContext context)
        {
            this.repository = new CB_CountriesExclusionRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CB_CountriesExclusionDataMapping();
        }
		 
		public  CB_CountriesExclusionPM GetSingle(string cb_id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CB_CountriesExclusionKeys(){ CB_ID = cb_id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CB_CountriesExclusion entityPOCO)
        {
            CB_CountriesExclusionKeys entityKeys = new CB_CountriesExclusionKeys() { CB_ID = entityPOCO.CB_ID,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 