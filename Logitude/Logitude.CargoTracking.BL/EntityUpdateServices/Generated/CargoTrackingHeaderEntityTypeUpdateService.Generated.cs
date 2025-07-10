 
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
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System.Web;
using Logitude.CargoTracking.Data.EntityPOCOs;
using Logitude.CargoTracking.Def.EntityPMs;
using Logitude.CargoTracking.BL.EntityDataMappings;
using Logitude.CargoTracking.Data.Repositories;
using Logitude.CargoTracking.Data.EntityKeys;
using Logitude.CargoTracking.Data;

namespace Logitude.CargoTracking.BL.EntityUpdateServices
{ 
   public partial class CargoTrackingHeaderEntityTypeUpdateService:EntityUpdateService<CargoTrackingHeaderEntityType,CargoTrackingHeaderEntityTypePM,EntityPM>
   {
   
        CargoTrackingHeaderEntityTypeRepository entityRepository;
        public CargoTrackingHeaderEntityTypeUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            ICargoTrackingContext  context = mainContext as CargoTrackingContext;
            context = context ??mainContext as ICargoTrackingContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new CargoTrackingHeaderEntityTypeDataMapping();
            Repository = new CargoTrackingHeaderEntityTypeRepository(context);
        }

       
        private ICargoTrackingContext currentContext;
        public CargoTrackingHeaderEntityTypeUpdateService(int tenant)
        {
            currentContext = CargoTrackingContext.GetContext(tenant);
        }

        public CargoTrackingHeaderEntityTypeUpdateService(ICargoTrackingContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(CargoTrackingHeaderEntityTypePM entityPM)
        {
            CargoTrackingHeaderEntityTypeKeys entityKeys = new CargoTrackingHeaderEntityTypeKeys() { Code = entityPM.Code };
            return entityKeys;
        }

		
	    protected override void FillDefaultValuesOnCreate(CargoTrackingHeaderEntityTypePM entityPM)
        {
 
		}
		protected override void FillDefaultValuesOnUpdate(CargoTrackingHeaderEntityTypePM entityPM)
		{
 
		}
		
		 
	 
   }
   
}
	 