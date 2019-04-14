 
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
   public partial class TariffVersionUpdateService:EntityUpdateService<TariffVersion,TariffVersionPM,EntityPM>
   {
   
        TariffVersionRepository entityRepository;
        public TariffVersionUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            ITariffModuleContext  context = mainContext as TariffModuleContext;
            context = context ??mainContext as ITariffModuleContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new TariffVersionDataMapping();
            Repository = new TariffVersionRepository(context);
        }

       
        private ITariffModuleContext currentContext;
        public TariffVersionUpdateService(int tenant)
        {
            currentContext = TariffModuleContext.GetContext(tenant);
        }

        public TariffVersionUpdateService(ITariffModuleContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(TariffVersionPM entityPM)
        {
            TariffVersionKeys entityKeys = new TariffVersionKeys() { TariffId = entityPM.TariffId, Version = entityPM.Version };
            return entityKeys;
        }

		
		protected override void FillDefaultValuesOnCreate(TariffVersionPM entityPM)
        {     
  
		
			string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
							 

			if (loggedContact != null)
            {
		        entityPM.CreatedByUserId = loggedContact.Id;
		    }
					
	    }
        
		protected override void FillDefaultValuesOnUpdate(TariffVersionPM entityPM)
        {       
           
        }
		  
		 
	 
   }
   
}
	 