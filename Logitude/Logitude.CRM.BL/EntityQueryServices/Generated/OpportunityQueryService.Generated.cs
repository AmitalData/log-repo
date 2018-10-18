 
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
   public partial class OpportunityQueryService: EntityQueryService<Opportunity,OpportunityKeys,OpportunityPM,object,OpportunityKeys>
   {
   
        OpportunityRepository repository;
		ICRMContext  context;
        public OpportunityQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new OpportunityRepository(context);
            Repository = repository;
            mapping = new OpportunityDataMapping();
        }

        public OpportunityQueryService(OpportunityRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new OpportunityDataMapping();
        }

        public OpportunityQueryService(ICRMContext context)
        {
            this.repository = new OpportunityRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new OpportunityDataMapping();
        }
		 
		public  OpportunityPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new OpportunityKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Opportunity entityPOCO)
        {
            OpportunityKeys entityKeys = new OpportunityKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 