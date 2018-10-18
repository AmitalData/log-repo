 
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
   public partial class OpportunityTypeQueryService: EntityQueryService<OpportunityType,OpportunityTypeKeys,OpportunityTypePM,object,OpportunityTypeKeys>
   {
   
        OpportunityTypeRepository repository;
		ICRMContext  context;
        public OpportunityTypeQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new OpportunityTypeRepository(context);
            Repository = repository;
            mapping = new OpportunityTypeDataMapping();
        }

        public OpportunityTypeQueryService(OpportunityTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new OpportunityTypeDataMapping();
        }

        public OpportunityTypeQueryService(ICRMContext context)
        {
            this.repository = new OpportunityTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new OpportunityTypeDataMapping();
        }
		 
		public  OpportunityTypePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new OpportunityTypeKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(OpportunityType entityPOCO)
        {
            OpportunityTypeKeys entityKeys = new OpportunityTypeKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 