 
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
   public partial class ActivityPriorityQueryService: EntityQueryService<ActivityPriority,ActivityPriorityKeys,ActivityPriorityPM,object,ActivityPriorityKeys>
   {
   
        ActivityPriorityRepository repository;
		ICRMContext  context;
        public ActivityPriorityQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new ActivityPriorityRepository(context);
            Repository = repository;
            mapping = new ActivityPriorityDataMapping();
        }

        public ActivityPriorityQueryService(ActivityPriorityRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ActivityPriorityDataMapping();
        }

        public ActivityPriorityQueryService(ICRMContext context)
        {
            this.repository = new ActivityPriorityRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ActivityPriorityDataMapping();
        }
		 
		public  ActivityPriorityPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ActivityPriorityKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ActivityPriority entityPOCO)
        {
            ActivityPriorityKeys entityKeys = new ActivityPriorityKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 