 
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
   public partial class TMEmployeeTimeUpdateService:EntityUpdateService<TMEmployeeTime,TMEmployeeTimePM,EntityPM>
   {
   
        TMEmployeeTimeRepository entityRepository;
        public TMEmployeeTimeUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            ITimeManagementContext  context = mainContext as TimeManagementContext;
            context = context ??mainContext as ITimeManagementContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new TMEmployeeTimeDataMapping();
            Repository = new TMEmployeeTimeRepository(context);
        }

       
        private ITimeManagementContext currentContext;
        public TMEmployeeTimeUpdateService(int tenant)
        {
            currentContext = TimeManagementContext.GetContext(tenant);
        }

        public TMEmployeeTimeUpdateService(ITimeManagementContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(TMEmployeeTimePM entityPM)
        {
            TMEmployeeTimeKeys entityKeys = new TMEmployeeTimeKeys() { Id = entityPM.Id };
            return entityKeys;
        }

		
		protected override void FillDefaultValuesOnCreate(TMEmployeeTimePM entityPM)
        {     
  
		
		    entityPM.Id = IdCounter.GetNumber("TMEmployeeTime", entityPM.Tenant); 
					
			DateTime myDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
							
		    entityPM.CreateDate =  myDate;
					  

		    entityPM.UpdateDate =  myDate;
                     
	    }
        
		protected override void FillDefaultValuesOnUpdate(TMEmployeeTimePM entityPM)
        {       
           
		    DateTime myDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
			entityPM.UpdateDate =  myDate;
					
        }
		  
		 
	 
   }
   
}
	 