 
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
using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.WarehouseLib.BL.EntityPMs;
using Logitude.WarehouseLib.BL.EntityDataMappings;
using Logitude.WarehouseLib.Data.Repositories;
using Logitude.WarehouseLib.Data.EntityKeys;
using Logitude.WarehouseLib.Data;

namespace Logitude.WarehouseLib.BL.EntityUpdateServices
{ 
   public partial class WarehouseEntryUpdateService:EntityUpdateService<WarehouseEntry,WarehouseEntryPM,EntityPM>
   {
   
        WarehouseEntryRepository entityRepository;
        public WarehouseEntryUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            IWarehouseContext  context = mainContext as WarehouseContext;
            context = context ??mainContext as IWarehouseContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new WarehouseEntryDataMapping();
            Repository = new WarehouseEntryRepository(context);
        }

       
        private IWarehouseContext currentContext;
        public WarehouseEntryUpdateService(int tenant)
        {
            currentContext = WarehouseContext.GetContext(tenant);
        }

        public WarehouseEntryUpdateService(IWarehouseContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(WarehouseEntryPM entityPM)
        {
            WarehouseEntryKeys entityKeys = new WarehouseEntryKeys() { Id = entityPM.Id };
            return entityKeys;
        }

		
		protected override void FillDefaultValuesOnCreate(WarehouseEntryPM entityPM)
        {     
  
		
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
        
		protected override void FillDefaultValuesOnUpdate(WarehouseEntryPM entityPM)
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
	 