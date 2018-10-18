 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityDataMappings;
using Logitude.CRM.Data.Repsitories;
using Logitude.CRM.Data.EntityKeys;
using Logitude.CRM.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.CRM.BL.EntityQueryServices
{ 
   public partial class OpportunityStageQueryService: EntityQueryService<OpportunityStage,OpportunityStageKeys,OpportunityStagePM,object,OpportunityStageKeys>
   {
   
        OpportunityStageRepository repository;
		ICRMContext  context;
        public OpportunityStageQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new OpportunityStageRepository(context);
            Repository = repository;
            mapping = new OpportunityStageDataMapping();
        }

        public OpportunityStageQueryService(OpportunityStageRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new OpportunityStageDataMapping();
        }

        public OpportunityStageQueryService(ICRMContext context)
        {
            this.repository = new OpportunityStageRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new OpportunityStageDataMapping();
        }
		 
		public  OpportunityStagePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new OpportunityStageKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(OpportunityStage entityPOCO)
        {
            OpportunityStageKeys entityKeys = new OpportunityStageKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 