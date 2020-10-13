 
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
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data;

namespace Logitude.Accounting.BL.EntityUpdateServices
{ 
   public partial class CalculatedChartsOfAccountsLineUpdateService:EntityUpdateService<CalculatedChartsOfAccountsLine,CalculatedChartsOfAccountsLinePM,EntityPM>
   {
   
        CalculatedChartsOfAccountsLineRepository entityRepository;
        public CalculatedChartsOfAccountsLineUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            IAccountingContext  context = mainContext as AccountingContext;
            context = context ??mainContext as IAccountingContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new CalculatedChartsOfAccountsLineDataMapping();
            Repository = new CalculatedChartsOfAccountsLineRepository(context);
        }

       
        private IAccountingContext currentContext;
        public CalculatedChartsOfAccountsLineUpdateService(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public CalculatedChartsOfAccountsLineUpdateService(IAccountingContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(CalculatedChartsOfAccountsLinePM entityPM)
        {
            CalculatedChartsOfAccountsLineKeys entityKeys = new CalculatedChartsOfAccountsLineKeys() { Id = entityPM.Id };
            return entityKeys;
        }

		
		protected override void FillDefaultValuesOnCreate(CalculatedChartsOfAccountsLinePM entityPM)
        {     
  
		
		    entityPM.Id = IdCounter.GetNumber("CalculatedChartsOfAccountsLine", entityPM.Tenant); 
					
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
        
		protected override void FillDefaultValuesOnUpdate(CalculatedChartsOfAccountsLinePM entityPM)
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
	 