 
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
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data;

namespace Logitude.Customs.BL.EntityUpdateServices
{ 
   public partial class CourierMasterUpdateService:EntityUpdateService<CourierMaster,CourierMasterPM,EntityPM>
   {
   
        CourierMasterRepository entityRepository;
        public CourierMasterUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            ICustomContext  context = mainContext as CustomContext;
            context = context ??mainContext as ICustomContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new CourierMasterDataMapping();
            Repository = new CourierMasterRepository(context);
        }

       
        private ICustomContext currentContext;
        public CourierMasterUpdateService(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CourierMasterUpdateService(ICustomContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(CourierMasterPM entityPM)
        {
            CourierMasterKeys entityKeys = new CourierMasterKeys() { Id = entityPM.Id };
            return entityKeys;
        }

		
		protected override void FillDefaultValuesOnCreate(CourierMasterPM entityPM)
        {     
  
		
		    entityPM.Id = IdCounter.GetNumber("CourierMaster", entityPM.Tenant); 
					
			string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
							 

			if (loggedContact != null)
            {
		        entityPM.CreatedByUserId = loggedContact.Id;
		    }
					
	    }
        
		protected override void FillDefaultValuesOnUpdate(CourierMasterPM entityPM)
        {       
           
        }
		  
		 
	 
   }
   
}
	 