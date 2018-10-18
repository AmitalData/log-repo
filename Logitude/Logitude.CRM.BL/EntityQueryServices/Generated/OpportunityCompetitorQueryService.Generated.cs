 
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
   public partial class OpportunityCompetitorQueryService: EntityQueryService<OpportunityCompetitor,OpportunityCompetitorKeys,OpportunityCompetitorPM,OpportunityPM,OpportunityKeys>
   {
   
        OpportunityCompetitorRepository repository;
		ICRMContext  context;
        public OpportunityCompetitorQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new OpportunityCompetitorRepository(context);
            Repository = repository;
            mapping = new OpportunityCompetitorDataMapping();
        }

        public OpportunityCompetitorQueryService(OpportunityCompetitorRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new OpportunityCompetitorDataMapping();
        }

        public OpportunityCompetitorQueryService(ICRMContext context)
        {
            this.repository = new OpportunityCompetitorRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new OpportunityCompetitorDataMapping();
        }
		 
		public  OpportunityCompetitorPM GetSingle(string opportunityid, string competitorid,bool getComposition, bool getFromCache)
        {
             EntityKeys = new OpportunityCompetitorKeys(){ OpportunityId = opportunityid, CompetitorId = competitorid };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(OpportunityCompetitor entityPOCO)
        {
            OpportunityCompetitorKeys entityKeys = new OpportunityCompetitorKeys() { OpportunityId = entityPOCO.OpportunityId, CompetitorId = entityPOCO.CompetitorId,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 