 
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
   public partial class OpportunityCompetitorProductQueryService: EntityQueryService<OpportunityCompetitorProduct,OpportunityCompetitorProductKeys,OpportunityCompetitorProductPM,OpportunityCompetitorPM,OpportunityCompetitorKeys>
   {
   
        OpportunityCompetitorProductRepository repository;
		ICRMContext  context;
        public OpportunityCompetitorProductQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new OpportunityCompetitorProductRepository(context);
            Repository = repository;
            mapping = new OpportunityCompetitorProductDataMapping();
        }

        public OpportunityCompetitorProductQueryService(OpportunityCompetitorProductRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new OpportunityCompetitorProductDataMapping();
        }

        public OpportunityCompetitorProductQueryService(ICRMContext context)
        {
            this.repository = new OpportunityCompetitorProductRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new OpportunityCompetitorProductDataMapping();
        }
		 
		public  OpportunityCompetitorProductPM GetSingle(string opportunityid, string competitorid, string producttypecode,bool getComposition, bool getFromCache)
        {
             EntityKeys = new OpportunityCompetitorProductKeys(){ OpportunityId = opportunityid, CompetitorId = competitorid, ProductTypeCode = producttypecode };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(OpportunityCompetitorProduct entityPOCO)
        {
            OpportunityCompetitorProductKeys entityKeys = new OpportunityCompetitorProductKeys() { OpportunityId = entityPOCO.OpportunityId, CompetitorId = entityPOCO.CompetitorId, ProductTypeCode = entityPOCO.ProductTypeCode,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 