 
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
   public partial class CB_RequirementComputedDataQueryService: EntityQueryService<CB_RequirementComputedData,CB_RequirementComputedDataKeys,CB_RequirementComputedDataPM,object,CB_RequirementComputedDataKeys>
   {
   
        CB_RequirementComputedDataRepository repository;
		ICustomContext  context;
        public CB_RequirementComputedDataQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CB_RequirementComputedDataRepository(context);
            Repository = repository;
            mapping = new CB_RequirementComputedDataDataMapping();
        }

        public CB_RequirementComputedDataQueryService(CB_RequirementComputedDataRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CB_RequirementComputedDataDataMapping();
        }

        public CB_RequirementComputedDataQueryService(ICustomContext context)
        {
            this.repository = new CB_RequirementComputedDataRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CB_RequirementComputedDataDataMapping();
        }
		 
		public  CB_RequirementComputedDataPM GetSingle(string cb_id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CB_RequirementComputedDataKeys(){ CB_ID = cb_id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CB_RequirementComputedData entityPOCO)
        {
            CB_RequirementComputedDataKeys entityKeys = new CB_RequirementComputedDataKeys() { CB_ID = entityPOCO.CB_ID,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 