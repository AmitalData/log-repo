 
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
   public partial class WidgetMeasureUpdateService:EntityUpdateService<WidgetMeasure,WidgetMeasurePM,WidgetPM>
   {
   
        WidgetMeasureRepository entityRepository;
        public WidgetMeasureUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            IInfrastructureContext  context = mainContext as InfrastructureContext;
            context = context ??mainContext as IInfrastructureContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new WidgetMeasureDataMapping();
            Repository = new WidgetMeasureRepository(context);
        }

       
        private IInfrastructureContext currentContext;
        public WidgetMeasureUpdateService(int tenant)
        {
            currentContext = InfrastructureContext.GetContext(tenant);
        }

        public WidgetMeasureUpdateService(IInfrastructureContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(WidgetMeasurePM entityPM)
        {
            WidgetMeasureKeys entityKeys = new WidgetMeasureKeys() { Id = entityPM.Id };
            return entityKeys;
        }

		
		protected override void FillDefaultValuesOnCreate(WidgetMeasurePM entityPM)
        {     
  
		
		    entityPM.Id = IdCounter.GetNumber("WidgetMeasure", entityPM.Tenant); 
					
	    }
        
		protected override void FillDefaultValuesOnUpdate(WidgetMeasurePM entityPM)
        {       
           
        }
		  
		 
	 
   }
   
}
	 