 
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
   public partial class ClaimsRelatedEntitiesRefundQueryService: EntityQueryService<ClaimsRelatedEntitiesRefund,ClaimsRelatedEntitiesRefundKeys,ClaimsRelatedEntitiesRefundPM,ClaimsRelatedEntityPM,ClaimsRelatedEntityKeys>
   {
   
        ClaimsRelatedEntitiesRefundRepository repository;
		ICustomContext  context;
        public ClaimsRelatedEntitiesRefundQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ClaimsRelatedEntitiesRefundRepository(context);
            Repository = repository;
            mapping = new ClaimsRelatedEntitiesRefundDataMapping();
        }

        public ClaimsRelatedEntitiesRefundQueryService(ClaimsRelatedEntitiesRefundRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ClaimsRelatedEntitiesRefundDataMapping();
        }

        public ClaimsRelatedEntitiesRefundQueryService(ICustomContext context)
        {
            this.repository = new ClaimsRelatedEntitiesRefundRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ClaimsRelatedEntitiesRefundDataMapping();
        }
		 
		public  ClaimsRelatedEntitiesRefundPM GetSingle(string claimid, int counterkey, int refundquntitylineno,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ClaimsRelatedEntitiesRefundKeys(){ ClaimId = claimid, CounterKey = counterkey, RefundQuntityLineNo = refundquntitylineno };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ClaimsRelatedEntitiesRefund entityPOCO)
        {
            ClaimsRelatedEntitiesRefundKeys entityKeys = new ClaimsRelatedEntitiesRefundKeys() { ClaimId = entityPOCO.ClaimId, CounterKey = entityPOCO.CounterKey, RefundQuntityLineNo = entityPOCO.RefundQuntityLineNo,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 