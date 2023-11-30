 
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
   public partial class ContainerSettingUpdateService:EntityUpdateService<ContainerSetting,ContainerSettingPM,EntityPM>
   {
   
        ContainerSettingRepository entityRepository;
        public ContainerSettingUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            IInfrastructureContext  context = mainContext as InfrastructureContext;
            context = context ??mainContext as IInfrastructureContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new ContainerSettingDataMapping();
            Repository = new ContainerSettingRepository(context);
        }

       
        private IInfrastructureContext currentContext;
        public ContainerSettingUpdateService(int tenant)
        {
            currentContext = InfrastructureContext.GetContext(tenant);
        }

        public ContainerSettingUpdateService(IInfrastructureContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(ContainerSettingPM entityPM)
        {
            ContainerSettingKeys entityKeys = new ContainerSettingKeys() { Id = entityPM.Id };
            return entityKeys;
        }

		
		protected override void FillDefaultValuesOnCreate(ContainerSettingPM entityPM)
        {     
  
		
		    entityPM.Id = IdCounter.GetNumber("ContainerSetting", entityPM.Tenant); 
					
	    }
        
		protected override void FillDefaultValuesOnUpdate(ContainerSettingPM entityPM)
        {       
           
        }
		  
		 
	 
   }
   
}
	 