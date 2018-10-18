 
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
   public partial class ActivityInviteeQueryService: EntityQueryService<ActivityInvitee,ActivityInviteeKeys,ActivityInviteePM,ActivityPM,ActivityKeys>
   {
   
        ActivityInviteeRepository repository;
		ICRMContext  context;
        public ActivityInviteeQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new ActivityInviteeRepository(context);
            Repository = repository;
            mapping = new ActivityInviteeDataMapping();
        }

        public ActivityInviteeQueryService(ActivityInviteeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ActivityInviteeDataMapping();
        }

        public ActivityInviteeQueryService(ICRMContext context)
        {
            this.repository = new ActivityInviteeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ActivityInviteeDataMapping();
        }
		 
		public  ActivityInviteePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ActivityInviteeKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ActivityInvitee entityPOCO)
        {
            ActivityInviteeKeys entityKeys = new ActivityInviteeKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 