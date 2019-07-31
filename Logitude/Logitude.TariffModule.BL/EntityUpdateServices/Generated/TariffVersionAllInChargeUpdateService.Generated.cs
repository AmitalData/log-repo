 
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
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.BL.EntityPMs;
using Logitude.TariffModule.BL.EntityDataMappings;
using Logitude.TariffModule.Data.Repositories;
using Logitude.TariffModule.Data.EntityKeys;
using Logitude.TariffModule.Data;

namespace Logitude.TariffModule.BL.EntityUpdateServices
{ 
   public partial class TariffVersionAllInChargeUpdateService:EntityUpdateService<TariffVersionAllInCharge,TariffVersionAllInChargePM,TariffVersionPM>
   {
   
        TariffVersionAllInChargeRepository entityRepository;
        public TariffVersionAllInChargeUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            ITariffModuleContext  context = mainContext as TariffModuleContext;
            context = context ??mainContext as ITariffModuleContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new TariffVersionAllInChargeDataMapping();
            Repository = new TariffVersionAllInChargeRepository(context);
        }

       
        private ITariffModuleContext currentContext;
        public TariffVersionAllInChargeUpdateService(int tenant)
        {
            currentContext = TariffModuleContext.GetContext(tenant);
        }

        public TariffVersionAllInChargeUpdateService(ITariffModuleContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(TariffVersionAllInChargePM entityPM)
        {
            TariffVersionAllInChargeKeys entityKeys = new TariffVersionAllInChargeKeys() { Id = entityPM.Id };
            return entityKeys;
        }

		
		protected override void FillDefaultValuesOnCreate(TariffVersionAllInChargePM entityPM)
        {     
  
		
		    entityPM.Id = IdCounter.GetNumber("TariffVersionAllInCharge", entityPM.Tenant); 
					
	    }
        
		protected override void FillDefaultValuesOnUpdate(TariffVersionAllInChargePM entityPM)
        {       
           
        }
		  
		 
	 
   }
   
}
	 