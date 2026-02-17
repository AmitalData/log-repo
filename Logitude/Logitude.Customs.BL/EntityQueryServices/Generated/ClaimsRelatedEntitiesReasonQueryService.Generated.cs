 
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
   public partial class ClaimsRelatedEntitiesReasonQueryService: EntityQueryService<ClaimsRelatedEntitiesReason,ClaimsRelatedEntitiesReasonKeys,ClaimsRelatedEntitiesReasonPM,ClaimsRelatedEntityPM,ClaimsRelatedEntityKeys>
   {
   
        ClaimsRelatedEntitiesReasonRepository repository;
		ICustomContext  context;
        public ClaimsRelatedEntitiesReasonQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ClaimsRelatedEntitiesReasonRepository(context);
            Repository = repository;
            mapping = new ClaimsRelatedEntitiesReasonDataMapping();
        }

        public ClaimsRelatedEntitiesReasonQueryService(ClaimsRelatedEntitiesReasonRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ClaimsRelatedEntitiesReasonDataMapping();
        }

        public ClaimsRelatedEntitiesReasonQueryService(ICustomContext context)
        {
            this.repository = new ClaimsRelatedEntitiesReasonRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ClaimsRelatedEntitiesReasonDataMapping();
        }
		 
		public  ClaimsRelatedEntitiesReasonPM GetSingle(string claimid, int counterkey, int lineno,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ClaimsRelatedEntitiesReasonKeys(){ ClaimId = claimid, CounterKey = counterkey, LineNo = lineno };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ClaimsRelatedEntitiesReason entityPOCO)
        {
            ClaimsRelatedEntitiesReasonKeys entityKeys = new ClaimsRelatedEntitiesReasonKeys() { ClaimId = entityPOCO.ClaimId, CounterKey = entityPOCO.CounterKey, LineNo = entityPOCO.LineNo,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 