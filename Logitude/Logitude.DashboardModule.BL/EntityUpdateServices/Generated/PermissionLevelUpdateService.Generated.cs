 
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
   public partial class PermissionLevelUpdateService:EntityUpdateService<PermissionLevel,PermissionLevelPM,EntityPM>
   {
   
        PermissionLevelRepository entityRepository;
        public PermissionLevelUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            IDashboardContext  context = mainContext as DashboardContext;
            context = context ??mainContext as IDashboardContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new PermissionLevelDataMapping();
            Repository = new PermissionLevelRepository(context);
        }

       
        private IDashboardContext currentContext;
        public PermissionLevelUpdateService(int tenant)
        {
            currentContext = DashboardContext.GetContext(tenant);
        }

        public PermissionLevelUpdateService(IDashboardContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(PermissionLevelPM entityPM)
        {
            PermissionLevelKeys entityKeys = new PermissionLevelKeys() { Code = entityPM.Code };
            return entityKeys;
        }

		
	    protected override void FillDefaultValuesOnCreate(PermissionLevelPM entityPM)
        {
 
		}
		protected override void FillDefaultValuesOnUpdate(PermissionLevelPM entityPM)
		{
 
		}
		
		 
	 
   }
   
}
	 