 
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
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.BL.EntityPMs;
using Logitude.Workflow.BL.EntityDataMappings;
using Logitude.Workflow.Data.Repositories;
using Logitude.Workflow.Data.EntityKeys;
using Logitude.Workflow.Data;

namespace Logitude.Workflow.BL.EntityUpdateServices
{ 
   public partial class TaskTypeUpdateService:EntityUpdateService<TaskType,TaskTypePM,EntityPM>
   {
   
        TaskTypeRepository entityRepository;
        public TaskTypeUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            IWorkflowContext  context = mainContext as WorkflowContext;
            context = context ??mainContext as IWorkflowContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new TaskTypeDataMapping();
            Repository = new TaskTypeRepository(context);
        }

       
        private IWorkflowContext currentContext;
        public TaskTypeUpdateService(int tenant)
        {
            currentContext = WorkflowContext.GetContext(tenant);
        }

        public TaskTypeUpdateService(IWorkflowContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(TaskTypePM entityPM)
        {
            TaskTypeKeys entityKeys = new TaskTypeKeys() { Id = entityPM.Id };
            return entityKeys;
        }

		
		protected override void FillDefaultValuesOnCreate(TaskTypePM entityPM)
        {     
  
		
		    entityPM.Id = IdCounter.GetNumber("TaskType", entityPM.Tenant); 
					
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
        
		protected override void FillDefaultValuesOnUpdate(TaskTypePM entityPM)
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
	 