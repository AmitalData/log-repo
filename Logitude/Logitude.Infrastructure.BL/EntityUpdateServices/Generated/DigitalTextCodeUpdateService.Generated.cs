 
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
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityDataMappings;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Infrastructure.Data.EntityKeys;
using Logitude.Infrastructure.Data;

namespace Logitude.Infrastructure.BL.EntityUpdateServices
{ 
   public partial class DigitalTextCodeUpdateService:EntityUpdateService<DigitalTextCode,DigitalTextCodePM,EntityPM>
   {
   
        DigitalTextCodeRepository entityRepository;
        public DigitalTextCodeUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            IInfrastructureContext  context = mainContext as InfrastructureContext;
            context = context ??mainContext as IInfrastructureContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new DigitalTextCodeDataMapping();
            Repository = new DigitalTextCodeRepository(context);
        }

       
        private IInfrastructureContext currentContext;
        public DigitalTextCodeUpdateService(int tenant)
        {
            currentContext = InfrastructureContext.GetContext(tenant);
        }

        public DigitalTextCodeUpdateService(IInfrastructureContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(DigitalTextCodePM entityPM)
        {
            DigitalTextCodeKeys entityKeys = new DigitalTextCodeKeys() { Id = entityPM.Id };
            return entityKeys;
        }

		
		protected override void FillDefaultValuesOnCreate(DigitalTextCodePM entityPM)
        {     
  
		
		    entityPM.Id = IdCounter.GetNumber("DigitalTextCode", entityPM.Tenant); 
					
			DateTime myDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
							
		    entityPM.CreateDate =  myDate;
					  

		    entityPM.UpdateDate =  myDate;
                     
	    }
        
		protected override void FillDefaultValuesOnUpdate(DigitalTextCodePM entityPM)
        {       
           
		    DateTime myDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
			entityPM.UpdateDate =  myDate;
					
        }
		  
		 
	 
   }
   
}
	 