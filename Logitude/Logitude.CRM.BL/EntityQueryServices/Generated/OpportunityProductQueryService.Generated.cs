 
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
   public partial class OpportunityProductQueryService: EntityQueryService<OpportunityProduct,OpportunityProductKeys,OpportunityProductPM,OpportunityPM,OpportunityKeys>
   {
   
        OpportunityProductRepository repository;
		ICRMContext  context;
        public OpportunityProductQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new OpportunityProductRepository(context);
            Repository = repository;
            mapping = new OpportunityProductDataMapping();
        }

        public OpportunityProductQueryService(OpportunityProductRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new OpportunityProductDataMapping();
        }

        public OpportunityProductQueryService(ICRMContext context)
        {
            this.repository = new OpportunityProductRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new OpportunityProductDataMapping();
        }
		 
		public  OpportunityProductPM GetSingle(string opportunityid, string opportunityproducttypecode,bool getComposition, bool getFromCache)
        {
             EntityKeys = new OpportunityProductKeys(){ OpportunityId = opportunityid, OpportunityProductTypeCode = opportunityproducttypecode };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(OpportunityProduct entityPOCO)
        {
            OpportunityProductKeys entityKeys = new OpportunityProductKeys() { OpportunityId = entityPOCO.OpportunityId, OpportunityProductTypeCode = entityPOCO.OpportunityProductTypeCode,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 