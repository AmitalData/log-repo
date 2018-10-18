 
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
   public partial class OpportunityProductLocationQueryService: EntityQueryService<OpportunityProductLocation,OpportunityProductLocationKeys,OpportunityProductLocationPM,OpportunityProductPM,OpportunityProductKeys>
   {
   
        OpportunityProductLocationRepository repository;
		ICRMContext  context;
        public OpportunityProductLocationQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new OpportunityProductLocationRepository(context);
            Repository = repository;
            mapping = new OpportunityProductLocationDataMapping();
        }

        public OpportunityProductLocationQueryService(OpportunityProductLocationRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new OpportunityProductLocationDataMapping();
        }

        public OpportunityProductLocationQueryService(ICRMContext context)
        {
            this.repository = new OpportunityProductLocationRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new OpportunityProductLocationDataMapping();
        }
		 
		public  OpportunityProductLocationPM GetSingle(string opportunityid, string opportunityproducttypecode, int linenumber,bool getComposition, bool getFromCache)
        {
             EntityKeys = new OpportunityProductLocationKeys(){ OpportunityId = opportunityid, OpportunityProductTypeCode = opportunityproducttypecode, LineNumber = linenumber };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(OpportunityProductLocation entityPOCO)
        {
            OpportunityProductLocationKeys entityKeys = new OpportunityProductLocationKeys() { OpportunityId = entityPOCO.OpportunityId, OpportunityProductTypeCode = entityPOCO.OpportunityProductTypeCode, LineNumber = entityPOCO.LineNumber,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 