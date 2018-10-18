 
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
   public partial class ClaimsRelatedEntitiesAmountQueryService: EntityQueryService<ClaimsRelatedEntitiesAmount,ClaimsRelatedEntitiesAmountKeys,ClaimsRelatedEntitiesAmountPM,ClaimsRelatedEntityPM,ClaimsRelatedEntityKeys>
   {
   
        ClaimsRelatedEntitiesAmountRepository repository;
		ICustomContext  context;
        public ClaimsRelatedEntitiesAmountQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ClaimsRelatedEntitiesAmountRepository(context);
            Repository = repository;
            mapping = new ClaimsRelatedEntitiesAmountDataMapping();
        }

        public ClaimsRelatedEntitiesAmountQueryService(ClaimsRelatedEntitiesAmountRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ClaimsRelatedEntitiesAmountDataMapping();
        }

        public ClaimsRelatedEntitiesAmountQueryService(ICustomContext context)
        {
            this.repository = new ClaimsRelatedEntitiesAmountRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ClaimsRelatedEntitiesAmountDataMapping();
        }
		 
		public  ClaimsRelatedEntitiesAmountPM GetSingle(string claimid, int counterkey, int lineno,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ClaimsRelatedEntitiesAmountKeys(){ ClaimId = claimid, CounterKey = counterkey, LineNo = lineno };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ClaimsRelatedEntitiesAmount entityPOCO)
        {
            ClaimsRelatedEntitiesAmountKeys entityKeys = new ClaimsRelatedEntitiesAmountKeys() { ClaimId = entityPOCO.ClaimId, CounterKey = entityPOCO.CounterKey, LineNo = entityPOCO.LineNo,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 