 
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
   public partial class SatisfactionSurveyUpdateService:EntityUpdateService<SatisfactionSurvey,SatisfactionSurveyPM,EntityPM>
   {
   
        SatisfactionSurveyRepository entityRepository;
        public SatisfactionSurveyUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            IInfrastructureContext  context = mainContext as InfrastructureContext;
            context = context ??mainContext as IInfrastructureContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new SatisfactionSurveyDataMapping();
            Repository = new SatisfactionSurveyRepository(context);
        }

       
        private IInfrastructureContext currentContext;
        public SatisfactionSurveyUpdateService(int tenant)
        {
            currentContext = InfrastructureContext.GetContext(tenant);
        }

        public SatisfactionSurveyUpdateService(IInfrastructureContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(SatisfactionSurveyPM entityPM)
        {
            SatisfactionSurveyKeys entityKeys = new SatisfactionSurveyKeys() { Id = entityPM.Id };
            return entityKeys;
        }

		
		protected override void FillDefaultValuesOnCreate(SatisfactionSurveyPM entityPM)
        {     
  
		
		    entityPM.Id = IdCounter.GetNumber("SatisfactionSurvey", entityPM.Tenant); 
					
			DateTime myDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
							
		    entityPM.CreateDate =  myDate;
					  

		    entityPM.UpdateDate =  myDate;
                     
	    }
        
		protected override void FillDefaultValuesOnUpdate(SatisfactionSurveyPM entityPM)
        {       
           
		    DateTime myDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
			entityPM.UpdateDate =  myDate;
					
        }
		  
		 
	 
   }
   
}
	 