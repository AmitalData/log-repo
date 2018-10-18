 
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
   public partial class ActivityEmailRecipientQueryService: EntityQueryService<ActivityEmailRecipient,ActivityEmailRecipientKeys,ActivityEmailRecipientPM,ActivityPM,ActivityKeys>
   {
   
        ActivityEmailRecipientRepository repository;
		ICRMContext  context;
        public ActivityEmailRecipientQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new ActivityEmailRecipientRepository(context);
            Repository = repository;
            mapping = new ActivityEmailRecipientDataMapping();
        }

        public ActivityEmailRecipientQueryService(ActivityEmailRecipientRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ActivityEmailRecipientDataMapping();
        }

        public ActivityEmailRecipientQueryService(ICRMContext context)
        {
            this.repository = new ActivityEmailRecipientRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ActivityEmailRecipientDataMapping();
        }
		 
		public  ActivityEmailRecipientPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ActivityEmailRecipientKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ActivityEmailRecipient entityPOCO)
        {
            ActivityEmailRecipientKeys entityKeys = new ActivityEmailRecipientKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 