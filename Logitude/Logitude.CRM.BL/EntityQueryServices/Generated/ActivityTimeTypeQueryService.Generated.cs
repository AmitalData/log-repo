 
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
   public partial class ActivityTimeTypeQueryService: EntityQueryService<ActivityTimeType,ActivityTimeTypeKeys,ActivityTimeTypePM,object,ActivityTimeTypeKeys>
   {
   
        ActivityTimeTypeRepository repository;
		ICRMContext  context;
        public ActivityTimeTypeQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new ActivityTimeTypeRepository(context);
            Repository = repository;
            mapping = new ActivityTimeTypeDataMapping();
        }

        public ActivityTimeTypeQueryService(ActivityTimeTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ActivityTimeTypeDataMapping();
        }

        public ActivityTimeTypeQueryService(ICRMContext context)
        {
            this.repository = new ActivityTimeTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ActivityTimeTypeDataMapping();
        }
		 
		public  ActivityTimeTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ActivityTimeTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ActivityTimeType entityPOCO)
        {
            ActivityTimeTypeKeys entityKeys = new ActivityTimeTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 