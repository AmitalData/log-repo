 
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
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.BL.EntityPMs;
using Logitude.TariffModule.BL.EntityDataMappings;
using Logitude.TariffModule.Data.Repositories;
using Logitude.TariffModule.Data.EntityKeys;
using Logitude.TariffModule.Data;

namespace Logitude.TariffModule.BL.EntityUpdateServices
{ 
   public partial class TariffUpdateService:EntityUpdateService<Tariff,TariffPM,EntityPM>
   {
   
        TariffRepository entityRepository;
        public TariffUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            ITariffModuleContext  context = mainContext as TariffModuleContext;
            context = context ??mainContext as ITariffModuleContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new TariffDataMapping();
            Repository = new TariffRepository(context);
        }

       
        private ITariffModuleContext currentContext;
        public TariffUpdateService(int tenant)
        {
            currentContext = TariffModuleContext.GetContext(tenant);
        }

        public TariffUpdateService(ITariffModuleContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(TariffPM entityPM)
        {
            TariffKeys entityKeys = new TariffKeys() { Id = entityPM.Id };
            return entityKeys;
        }

		
		protected override void FillDefaultValuesOnCreate(TariffPM entityPM)
        {     
  
		
		    entityPM.Id = IdCounter.GetNumber("Tariff", entityPM.Tenant); 
					
			string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
							 

			if (loggedContact != null)
            {
		        entityPM.CreatedByUserId = loggedContact.Id;
		    }
					 

			if (loggedContact != null)
            {
		        entityPM.UpdatedByUserId = loggedContact.Id;
		    }
                        
					
	    }
        
		protected override void FillDefaultValuesOnUpdate(TariffPM entityPM)
        {       
             
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
	 