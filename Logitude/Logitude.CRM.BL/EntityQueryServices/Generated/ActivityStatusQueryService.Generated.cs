 
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
   public partial class ActivityStatusQueryService: EntityQueryService<ActivityStatus,ActivityStatusKeys,ActivityStatusPM,object,ActivityStatusKeys>
   {
   
        ActivityStatusRepository repository;
		ICRMContext  context;
        public ActivityStatusQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new ActivityStatusRepository(context);
            Repository = repository;
            mapping = new ActivityStatusDataMapping();
        }

        public ActivityStatusQueryService(ActivityStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ActivityStatusDataMapping();
        }

        public ActivityStatusQueryService(ICRMContext context)
        {
            this.repository = new ActivityStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ActivityStatusDataMapping();
        }
		 
		public  ActivityStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ActivityStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ActivityStatus entityPOCO)
        {
            ActivityStatusKeys entityKeys = new ActivityStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 