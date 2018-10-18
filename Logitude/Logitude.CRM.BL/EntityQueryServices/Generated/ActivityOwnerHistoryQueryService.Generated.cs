 
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
   public partial class ActivityOwnerHistoryQueryService: EntityQueryService<ActivityOwnerHistory,ActivityOwnerHistoryKeys,ActivityOwnerHistoryPM,object,ActivityOwnerHistoryKeys>
   {
   
        ActivityOwnerHistoryRepository repository;
		ICRMContext  context;
        public ActivityOwnerHistoryQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new ActivityOwnerHistoryRepository(context);
            Repository = repository;
            mapping = new ActivityOwnerHistoryDataMapping();
        }

        public ActivityOwnerHistoryQueryService(ActivityOwnerHistoryRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ActivityOwnerHistoryDataMapping();
        }

        public ActivityOwnerHistoryQueryService(ICRMContext context)
        {
            this.repository = new ActivityOwnerHistoryRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ActivityOwnerHistoryDataMapping();
        }
		 
		public  ActivityOwnerHistoryPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ActivityOwnerHistoryKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ActivityOwnerHistory entityPOCO)
        {
            ActivityOwnerHistoryKeys entityKeys = new ActivityOwnerHistoryKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 