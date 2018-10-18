 
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
   public partial class ClaimsRelatedEntsReasonsExpQueryService: EntityQueryService<ClaimsRelatedEntsReasonsExp,ClaimsRelatedEntsReasonsExpKeys,ClaimsRelatedEntsReasonsExpPM,ClaimsRelatedEntitiesReasonPM,ClaimsRelatedEntitiesReasonKeys>
   {
   
        ClaimsRelatedEntsReasonsExpRepository repository;
		ICustomContext  context;
        public ClaimsRelatedEntsReasonsExpQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ClaimsRelatedEntsReasonsExpRepository(context);
            Repository = repository;
            mapping = new ClaimsRelatedEntsReasonsExpDataMapping();
        }

        public ClaimsRelatedEntsReasonsExpQueryService(ClaimsRelatedEntsReasonsExpRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ClaimsRelatedEntsReasonsExpDataMapping();
        }

        public ClaimsRelatedEntsReasonsExpQueryService(ICustomContext context)
        {
            this.repository = new ClaimsRelatedEntsReasonsExpRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ClaimsRelatedEntsReasonsExpDataMapping();
        }
		 
		public  ClaimsRelatedEntsReasonsExpPM GetSingle(string claimid, int counterkey, int reasonlineno, int lineno,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ClaimsRelatedEntsReasonsExpKeys(){ ClaimId = claimid, CounterKey = counterkey, ReasonLineNo = reasonlineno, LineNo = lineno };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ClaimsRelatedEntsReasonsExp entityPOCO)
        {
            ClaimsRelatedEntsReasonsExpKeys entityKeys = new ClaimsRelatedEntsReasonsExpKeys() { ClaimId = entityPOCO.ClaimId, CounterKey = entityPOCO.CounterKey, ReasonLineNo = entityPOCO.ReasonLineNo, LineNo = entityPOCO.LineNo,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 