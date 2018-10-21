 
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
   public partial class SharedLogisticsSettingUpdateService:EntityUpdateService<SharedLogisticsSetting,SharedLogisticsSettingPM,EntityPM>
   {
   
        SharedLogisticsSettingRepository entityRepository;
        public SharedLogisticsSettingUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            IInfrastructureContext  context = mainContext as InfrastructureContext;
            context = context ??mainContext as IInfrastructureContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new SharedLogisticsSettingDataMapping();
            Repository = new SharedLogisticsSettingRepository(context);
        }

       
        private IInfrastructureContext currentContext;
        public SharedLogisticsSettingUpdateService(int tenant)
        {
            currentContext = InfrastructureContext.GetContext(tenant);
        }

        public SharedLogisticsSettingUpdateService(IInfrastructureContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(SharedLogisticsSettingPM entityPM)
        {
            SharedLogisticsSettingKeys entityKeys = new SharedLogisticsSettingKeys() { Id = entityPM.Id };
            return entityKeys;
        }

		
		protected override void FillDefaultValuesOnCreate(SharedLogisticsSettingPM entityPM)
        {     
  
		
	    }
        
		protected override void FillDefaultValuesOnUpdate(SharedLogisticsSettingPM entityPM)
        {       
           
        }
		  
		 
	 
   }
   
}
	 