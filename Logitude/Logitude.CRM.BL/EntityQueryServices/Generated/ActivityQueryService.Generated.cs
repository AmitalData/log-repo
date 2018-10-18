 
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
   public partial class ActivityQueryService: EntityQueryService<Activity,ActivityKeys,ActivityPM,object,ActivityKeys>
   {
   
        ActivityRepository repository;
		ICRMContext  context;
        public ActivityQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new ActivityRepository(context);
            Repository = repository;
            mapping = new ActivityDataMapping();
        }

        public ActivityQueryService(ActivityRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ActivityDataMapping();
        }

        public ActivityQueryService(ICRMContext context)
        {
            this.repository = new ActivityRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ActivityDataMapping();
        }
		 
		public  ActivityPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ActivityKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Activity entityPOCO)
        {
            ActivityKeys entityKeys = new ActivityKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 