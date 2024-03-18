 
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
   public partial class CB_LevyExclusionQueryService: EntityQueryService<CB_LevyExclusion,CB_LevyExclusionKeys,CB_LevyExclusionPM,object,CB_LevyExclusionKeys>
   {
   
        CB_LevyExclusionRepository repository;
		ICustomContext  context;
        public CB_LevyExclusionQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CB_LevyExclusionRepository(context);
            Repository = repository;
            mapping = new CB_LevyExclusionDataMapping();
        }

        public CB_LevyExclusionQueryService(CB_LevyExclusionRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CB_LevyExclusionDataMapping();
        }

        public CB_LevyExclusionQueryService(ICustomContext context)
        {
            this.repository = new CB_LevyExclusionRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CB_LevyExclusionDataMapping();
        }
		 
		public  CB_LevyExclusionPM GetSingle(int id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CB_LevyExclusionKeys(){ ID = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CB_LevyExclusion entityPOCO)
        {
            CB_LevyExclusionKeys entityKeys = new CB_LevyExclusionKeys() { ID = entityPOCO.ID,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 