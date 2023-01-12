 
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
   public partial class DashboardGlobalPresetFilterUpdateService:EntityUpdateService<DashboardGlobalPresetFilter,DashboardGlobalPresetFilterPM,EntityPM>
   {
   
        DashboardGlobalPresetFilterRepository entityRepository;
        public DashboardGlobalPresetFilterUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            IDashboardContext  context = mainContext as DashboardContext;
            context = context ??mainContext as IDashboardContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new DashboardGlobalPresetFilterDataMapping();
            Repository = new DashboardGlobalPresetFilterRepository(context);
        }

       
        private IDashboardContext currentContext;
        public DashboardGlobalPresetFilterUpdateService(int tenant)
        {
            currentContext = DashboardContext.GetContext(tenant);
        }

        public DashboardGlobalPresetFilterUpdateService(IDashboardContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(DashboardGlobalPresetFilterPM entityPM)
        {
            DashboardGlobalPresetFilterKeys entityKeys = new DashboardGlobalPresetFilterKeys() { Code = entityPM.Code };
            return entityKeys;
        }

		
		protected override void FillDefaultValuesOnCreate(DashboardGlobalPresetFilterPM entityPM)
        {     
  
		
	    }
        
		protected override void FillDefaultValuesOnUpdate(DashboardGlobalPresetFilterPM entityPM)
        {       
           
        }
		  
		 
	 
   }
   
}
	 