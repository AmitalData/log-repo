 
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
   public partial class OccasionInviteeQueryService: EntityQueryService<OccasionInvitee,OccasionInviteeKeys,OccasionInviteePM,OccasionPM,OccasionKeys>
   {
   
        OccasionInviteeRepository repository;
		ICRMContext  context;
        public OccasionInviteeQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new OccasionInviteeRepository(context);
            Repository = repository;
            mapping = new OccasionInviteeDataMapping();
        }

        public OccasionInviteeQueryService(OccasionInviteeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new OccasionInviteeDataMapping();
        }

        public OccasionInviteeQueryService(ICRMContext context)
        {
            this.repository = new OccasionInviteeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new OccasionInviteeDataMapping();
        }
		 
		public  OccasionInviteePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new OccasionInviteeKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(OccasionInvitee entityPOCO)
        {
            OccasionInviteeKeys entityKeys = new OccasionInviteeKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 