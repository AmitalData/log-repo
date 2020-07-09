 
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
using Logitude.CargoTracking.Data.EntityPOCOs;
using Logitude.CargoTracking.Def.EntityPMs;
using Logitude.CargoTracking.BL.EntityDataMappings;
using Logitude.CargoTracking.Data.Repositories;
using Logitude.CargoTracking.Data.EntityKeys;
using Logitude.CargoTracking.Data;

namespace Logitude.CargoTracking.BL.EntityUpdateServices
{ 
   public partial class CargoTrackingPortUpdateService:EntityUpdateService<CargoTrackingPort,CargoTrackingPortPM,EntityPM>
   {
   
        CargoTrackingPortRepository entityRepository;
        public CargoTrackingPortUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            ICargoTrackingContext  context = mainContext as CargoTrackingContext;
            context = context ??mainContext as ICargoTrackingContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new CargoTrackingPortDataMapping();
            Repository = new CargoTrackingPortRepository(context);
        }

       
        private ICargoTrackingContext currentContext;
        public CargoTrackingPortUpdateService(int tenant)
        {
            currentContext = CargoTrackingContext.GetContext(tenant);
        }

        public CargoTrackingPortUpdateService(ICargoTrackingContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(CargoTrackingPortPM entityPM)
        {
            CargoTrackingPortKeys entityKeys = new CargoTrackingPortKeys() { Id = entityPM.Id };
            return entityKeys;
        }

		
		protected override void FillDefaultValuesOnCreate(CargoTrackingPortPM entityPM)
        {     
  
		
		    //entityPM.Id = IdCounter.GetNumber("CargoTrackingPort", entityPM.Tenant); 
					
	    }
        
		protected override void FillDefaultValuesOnUpdate(CargoTrackingPortPM entityPM)
        {       
           
        }
		  
		 
	 
   }
   
}
	 