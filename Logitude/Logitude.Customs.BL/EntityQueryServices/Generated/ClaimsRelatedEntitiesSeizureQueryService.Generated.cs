 
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
   public partial class ClaimsRelatedEntitiesSeizureQueryService: EntityQueryService<ClaimsRelatedEntitiesSeizure,ClaimsRelatedEntitiesSeizureKeys,ClaimsRelatedEntitiesSeizurePM,ClaimsRelatedEntityPM,ClaimsRelatedEntityKeys>
   {
   
        ClaimsRelatedEntitiesSeizureRepository repository;
		ICustomContext  context;
        public ClaimsRelatedEntitiesSeizureQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ClaimsRelatedEntitiesSeizureRepository(context);
            Repository = repository;
            mapping = new ClaimsRelatedEntitiesSeizureDataMapping();
        }

        public ClaimsRelatedEntitiesSeizureQueryService(ClaimsRelatedEntitiesSeizureRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ClaimsRelatedEntitiesSeizureDataMapping();
        }

        public ClaimsRelatedEntitiesSeizureQueryService(ICustomContext context)
        {
            this.repository = new ClaimsRelatedEntitiesSeizureRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ClaimsRelatedEntitiesSeizureDataMapping();
        }
		 
		public  ClaimsRelatedEntitiesSeizurePM GetSingle(string claimid, int counterkey, int seizurelinono,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ClaimsRelatedEntitiesSeizureKeys(){ ClaimId = claimid, CounterKey = counterkey, SeizureLinoNo = seizurelinono };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ClaimsRelatedEntitiesSeizure entityPOCO)
        {
            ClaimsRelatedEntitiesSeizureKeys entityKeys = new ClaimsRelatedEntitiesSeizureKeys() { ClaimId = entityPOCO.ClaimId, CounterKey = entityPOCO.CounterKey, SeizureLinoNo = entityPOCO.SeizureLinoNo,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 