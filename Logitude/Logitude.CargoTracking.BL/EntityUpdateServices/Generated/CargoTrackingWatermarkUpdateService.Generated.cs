 
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
   public partial class CargoTrackingWatermarkUpdateService:EntityUpdateService<CargoTrackingWatermark,CargoTrackingWatermarkPM,EntityPM>
   {
   
        CargoTrackingWatermarkRepository entityRepository;
        public CargoTrackingWatermarkUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            ICargoTrackingContext  context = mainContext as CargoTrackingContext;
            context = context ??mainContext as ICargoTrackingContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new CargoTrackingWatermarkDataMapping();
            Repository = new CargoTrackingWatermarkRepository(context);
        }

       
        private ICargoTrackingContext currentContext;
        public CargoTrackingWatermarkUpdateService(int tenant)
        {
            currentContext = CargoTrackingContext.GetContext(tenant);
        }

        public CargoTrackingWatermarkUpdateService(ICargoTrackingContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(CargoTrackingWatermarkPM entityPM)
        {
            CargoTrackingWatermarkKeys entityKeys = new CargoTrackingWatermarkKeys() { TableName = entityPM.TableName };
            return entityKeys;
        }

		
	    protected override void FillDefaultValuesOnCreate(CargoTrackingWatermarkPM entityPM)
        {
 
		}
		protected override void FillDefaultValuesOnUpdate(CargoTrackingWatermarkPM entityPM)
		{
 
		}
		
		 
	 
   }
   
}
	 