 
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
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.DashboardModule.BL.EntityDataMappings;
using Logitude.DashboardModule.Data.Repositories;
using Logitude.DashboardModule.Data.EntityKeys;
using Logitude.DashboardModule.Data;

namespace Logitude.DashboardModule.BL.EntityUpdateServices
{ 
   public partial class DashboardGlobalFilterUpdateService:EntityUpdateService<DashboardGlobalFilter,DashboardGlobalFilterPM,DashboardPM>
   {
   
        DashboardGlobalFilterRepository entityRepository;
        public DashboardGlobalFilterUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            IDashboardContext  context = mainContext as DashboardContext;
            context = context ??mainContext as IDashboardContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new DashboardGlobalFilterDataMapping();
            Repository = new DashboardGlobalFilterRepository(context);
        }

       
        private IDashboardContext currentContext;
        public DashboardGlobalFilterUpdateService(int tenant)
        {
            currentContext = DashboardContext.GetContext(tenant);
        }

        public DashboardGlobalFilterUpdateService(IDashboardContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(DashboardGlobalFilterPM entityPM)
        {
            DashboardGlobalFilterKeys entityKeys = new DashboardGlobalFilterKeys() { Id = entityPM.Id };
            return entityKeys;
        }

		
		protected override void FillDefaultValuesOnCreate(DashboardGlobalFilterPM entityPM)
        {     
  
		
		    entityPM.Id = IdCounter.GetNumber("DashboardGlobalFilter", entityPM.Tenant); 
					
	    }
        
		protected override void FillDefaultValuesOnUpdate(DashboardGlobalFilterPM entityPM)
        {       
           
        }
		  
		 
	 
   }
   
}
	 