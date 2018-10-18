 
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
   public partial class OpportunityAdditionalServiceQueryService: EntityQueryService<OpportunityAdditionalService,OpportunityAdditionalServiceKeys,OpportunityAdditionalServicePM,OpportunityPM,OpportunityKeys>
   {
   
        OpportunityAdditionalServiceRepository repository;
		ICRMContext  context;
        public OpportunityAdditionalServiceQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new OpportunityAdditionalServiceRepository(context);
            Repository = repository;
            mapping = new OpportunityAdditionalServiceDataMapping();
        }

        public OpportunityAdditionalServiceQueryService(OpportunityAdditionalServiceRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new OpportunityAdditionalServiceDataMapping();
        }

        public OpportunityAdditionalServiceQueryService(ICRMContext context)
        {
            this.repository = new OpportunityAdditionalServiceRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new OpportunityAdditionalServiceDataMapping();
        }
		 
		public  OpportunityAdditionalServicePM GetSingle(string opportunityid, string additionalserviceid,bool getComposition, bool getFromCache)
        {
             EntityKeys = new OpportunityAdditionalServiceKeys(){ OpportunityId = opportunityid, AdditionalServiceId = additionalserviceid };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(OpportunityAdditionalService entityPOCO)
        {
            OpportunityAdditionalServiceKeys entityKeys = new OpportunityAdditionalServiceKeys() { OpportunityId = entityPOCO.OpportunityId, AdditionalServiceId = entityPOCO.AdditionalServiceId,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 