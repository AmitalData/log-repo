 
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
   public partial class TMDayOffTypeUpdateService:EntityUpdateService<TMDayOffType,TMDayOffTypePM,EntityPM>
   {
   
        TMDayOffTypeRepository entityRepository;
        public TMDayOffTypeUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            ITimeManagementContext  context = mainContext as TimeManagementContext;
            context = context ??mainContext as ITimeManagementContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new TMDayOffTypeDataMapping();
            Repository = new TMDayOffTypeRepository(context);
        }

       
        private ITimeManagementContext currentContext;
        public TMDayOffTypeUpdateService(int tenant)
        {
            currentContext = TimeManagementContext.GetContext(tenant);
        }

        public TMDayOffTypeUpdateService(ITimeManagementContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(TMDayOffTypePM entityPM)
        {
            TMDayOffTypeKeys entityKeys = new TMDayOffTypeKeys() { Code = entityPM.Code };
            return entityKeys;
        }

		
	    protected override void FillDefaultValuesOnCreate(TMDayOffTypePM entityPM)
        {
 
		}
		protected override void FillDefaultValuesOnUpdate(TMDayOffTypePM entityPM)
		{
 
		}
		
		 
	 
   }
   
}
	 