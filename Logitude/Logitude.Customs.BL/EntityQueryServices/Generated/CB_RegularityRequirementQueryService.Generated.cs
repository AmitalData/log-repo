 
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
   public partial class CB_RegularityRequirementQueryService: EntityQueryService<CB_RegularityRequirement,CB_RegularityRequirementKeys,CB_RegularityRequirementPM,object,CB_RegularityRequirementKeys>
   {
   
        CB_RegularityRequirementRepository repository;
		ICustomContext  context;
        public CB_RegularityRequirementQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CB_RegularityRequirementRepository(context);
            Repository = repository;
            mapping = new CB_RegularityRequirementDataMapping();
        }

        public CB_RegularityRequirementQueryService(CB_RegularityRequirementRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CB_RegularityRequirementDataMapping();
        }

        public CB_RegularityRequirementQueryService(ICustomContext context)
        {
            this.repository = new CB_RegularityRequirementRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CB_RegularityRequirementDataMapping();
        }
		 
		public  CB_RegularityRequirementPM GetSingle(string cb_id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CB_RegularityRequirementKeys(){ CB_ID = cb_id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CB_RegularityRequirement entityPOCO)
        {
            CB_RegularityRequirementKeys entityKeys = new CB_RegularityRequirementKeys() { CB_ID = entityPOCO.CB_ID,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 