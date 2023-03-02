 
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
   public partial class DashboardsUserSettingUpdateService:EntityUpdateService<DashboardsUserSetting,DashboardsUserSettingPM,EntityPM>
   {
   
        DashboardsUserSettingRepository entityRepository;
        public DashboardsUserSettingUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            IDashboardContext  context = mainContext as DashboardContext;
            context = context ??mainContext as IDashboardContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new DashboardsUserSettingDataMapping();
            Repository = new DashboardsUserSettingRepository(context);
        }

       
        private IDashboardContext currentContext;
        public DashboardsUserSettingUpdateService(int tenant)
        {
            currentContext = DashboardContext.GetContext(tenant);
        }

        public DashboardsUserSettingUpdateService(IDashboardContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(DashboardsUserSettingPM entityPM)
        {
            DashboardsUserSettingKeys entityKeys = new DashboardsUserSettingKeys() { Id = entityPM.Id };
            return entityKeys;
        }

		
		protected override void FillDefaultValuesOnCreate(DashboardsUserSettingPM entityPM)
        {     
  
		
		    entityPM.Id = IdCounter.GetNumber("DashboardsUserSetting", entityPM.Tenant); 
					
	    }
        
		protected override void FillDefaultValuesOnUpdate(DashboardsUserSettingPM entityPM)
        {       
           
        }
		  
		 
	 
   }
   
}
	 