 
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
using Logitude.ShipmentOrderModule.Data.EntityPOCOs;
using Logitude.ShipmentOrderModule.Def.EntityPMs;
using Logitude.ShipmentOrderModule.BL.EntityDataMappings;
using Logitude.ShipmentOrderModule.Data.Repositories;
using Logitude.ShipmentOrderModule.Data.EntityKeys;
using Logitude.ShipmentOrderModule.Data;

namespace Logitude.ShipmentOrderModule.BL.EntityUpdateServices
{ 
   public partial class ShipmentOrderUpdateService:EntityUpdateService<ShipmentOrder,ShipmentOrderPM,EntityPM>
   {
   
        ShipmentOrderRepository entityRepository;
        public ShipmentOrderUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            IShipmentOrderContext  context = mainContext as ShipmentOrderContext;
            context = context ??mainContext as IShipmentOrderContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new ShipmentOrderDataMapping();
            Repository = new ShipmentOrderRepository(context);
        }

       
        private IShipmentOrderContext currentContext;
        public ShipmentOrderUpdateService(int tenant)
        {
            currentContext = ShipmentOrderContext.GetContext(tenant);
        }

        public ShipmentOrderUpdateService(IShipmentOrderContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(ShipmentOrderPM entityPM)
        {
            ShipmentOrderKeys entityKeys = new ShipmentOrderKeys() { Id = entityPM.Id };
            return entityKeys;
        }

		
		protected override void FillDefaultValuesOnCreate(ShipmentOrderPM entityPM)
        {     
  
		
		    entityPM.Id = IdCounter.GetNumber("ShipmentOrder", entityPM.Tenant); 
					
			string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
							 

			if (loggedContact != null)
            {
		        entityPM.CreatedByUserId = loggedContact.Id;
		    }
					
			DateTime myDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
							 

		    entityPM.UpdateDate =  myDate;
                      

			if (loggedContact != null)
            {
		        entityPM.UpdatedByUserId = loggedContact.Id;
		    }
                        
					
	    }
        
		protected override void FillDefaultValuesOnUpdate(ShipmentOrderPM entityPM)
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
	 