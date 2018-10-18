 
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
   public partial class OpportunityClosingReasonQueryService: EntityQueryService<OpportunityClosingReason,OpportunityClosingReasonKeys,OpportunityClosingReasonPM,object,OpportunityClosingReasonKeys>
   {
   
        OpportunityClosingReasonRepository repository;
		ICRMContext  context;
        public OpportunityClosingReasonQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new OpportunityClosingReasonRepository(context);
            Repository = repository;
            mapping = new OpportunityClosingReasonDataMapping();
        }

        public OpportunityClosingReasonQueryService(OpportunityClosingReasonRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new OpportunityClosingReasonDataMapping();
        }

        public OpportunityClosingReasonQueryService(ICRMContext context)
        {
            this.repository = new OpportunityClosingReasonRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new OpportunityClosingReasonDataMapping();
        }
		 
		public  OpportunityClosingReasonPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new OpportunityClosingReasonKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(OpportunityClosingReason entityPOCO)
        {
            OpportunityClosingReasonKeys entityKeys = new OpportunityClosingReasonKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 