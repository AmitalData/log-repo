 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Web;
using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.BL.EntityPMs;
using Logitude.TimeManagement.BL.EntityDataMappings;
using Logitude.TimeManagement.Data.Repositories;
using Logitude.TimeManagement.Data.EntityKeys;
using Logitude.TimeManagement.Data;

namespace Logitude.TimeManagement.BL.EntityUpdateServices
{ 
   public partial class TMOfficeHourUpdateService:EntityUpdateService<TMOfficeHour,TMOfficeHourPM,EntityPM>
   {
   
        TMOfficeHourRepository entityRepository;
        public TMOfficeHourUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            ITimeManagementContext  context = mainContext as TimeManagementContext;
            context = context ??mainContext as ITimeManagementContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new TMOfficeHourDataMapping();
            Repository = new TMOfficeHourRepository(context);
        }

       
        private ITimeManagementContext currentContext;
        public TMOfficeHourUpdateService(int tenant)
        {
            currentContext = TimeManagementContext.GetContext(tenant);
        }

        public TMOfficeHourUpdateService(ITimeManagementContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(TMOfficeHourPM entityPM)
        {
            TMOfficeHourKeys entityKeys = new TMOfficeHourKeys() { Id = entityPM.Id };
            return entityKeys;
        }

		
		protected override void FillDefaultValuesOnCreate(TMOfficeHourPM entityPM)
        {     
  
		
		    entityPM.Id = IdCounter.GetNumber("TMOfficeHour", entityPM.Tenant); 
					
			DateTime myDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
							
		    entityPM.CreateDate =  myDate;
					 
			string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
							 

			if (loggedContact != null)
            {
		        entityPM.CreatedByUserId = loggedContact.Id;
		    }
					 

		    entityPM.UpdateDate =  myDate;
                      

			if (loggedContact != null)
            {
		        entityPM.UpdatedByUserId = loggedContact.Id;
		    }
                        
					
	    }
        
		protected override void FillDefaultValuesOnUpdate(TMOfficeHourPM entityPM)
        {       
           
		    DateTime myDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
			entityPM.UpdateDate =  myDate;
					  
            string email = "system@tenant" + entityPM.Tenant + ".com";
            if (HttpContext.Current != null)
            {
                email = HttpContext.Current.User.Identity.Name; 
            }
             
		    //string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
			if (loggedContact != null)
            {
		        entityPM.UpdatedByUserId = loggedContact.Id;
		    }
                        
					
        }
		  
		 
	 
   }
   
}
	 