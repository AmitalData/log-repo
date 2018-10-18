 
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
   public partial class ActivityTypeQueryService: EntityQueryService<ActivityType,ActivityTypeKeys,ActivityTypePM,object,ActivityTypeKeys>
   {
   
        ActivityTypeRepository repository;
		ICRMContext  context;
        public ActivityTypeQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new ActivityTypeRepository(context);
            Repository = repository;
            mapping = new ActivityTypeDataMapping();
        }

        public ActivityTypeQueryService(ActivityTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ActivityTypeDataMapping();
        }

        public ActivityTypeQueryService(ICRMContext context)
        {
            this.repository = new ActivityTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ActivityTypeDataMapping();
        }
		 
		public  ActivityTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ActivityTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ActivityType entityPOCO)
        {
            ActivityTypeKeys entityKeys = new ActivityTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 