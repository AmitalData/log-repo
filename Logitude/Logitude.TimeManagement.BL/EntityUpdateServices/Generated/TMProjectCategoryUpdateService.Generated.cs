 
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
   public partial class TMProjectCategoryUpdateService:EntityUpdateService<TMProjectCategory,TMProjectCategoryPM,EntityPM>
   {
   
        TMProjectCategoryRepository entityRepository;
        public TMProjectCategoryUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            ITimeManagementContext  context = mainContext as TimeManagementContext;
            context = context ??mainContext as ITimeManagementContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new TMProjectCategoryDataMapping();
            Repository = new TMProjectCategoryRepository(context);
        }

       
        private ITimeManagementContext currentContext;
        public TMProjectCategoryUpdateService(int tenant)
        {
            currentContext = TimeManagementContext.GetContext(tenant);
        }

        public TMProjectCategoryUpdateService(ITimeManagementContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(TMProjectCategoryPM entityPM)
        {
            TMProjectCategoryKeys entityKeys = new TMProjectCategoryKeys() { Id = entityPM.Id };
            return entityKeys;
        }

		
		protected override void FillDefaultValuesOnCreate(TMProjectCategoryPM entityPM)
        {     
  
		
		    entityPM.Id = IdCounter.GetNumber("TMProjectCategory", entityPM.Tenant); 
					
	    }
        
		protected override void FillDefaultValuesOnUpdate(TMProjectCategoryPM entityPM)
        {       
           
        }
		  
		 
	 
   }
   
}
	 