 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.CargoTracking.Data.EntityPOCOs;
using Logitude.CargoTracking.Def.EntityPMs;
using Logitude.CargoTracking.BL.EntityDataMappings;
using Logitude.CargoTracking.Data.Repositories;
using Logitude.CargoTracking.Data.EntityKeys;
using Logitude.CargoTracking.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.CargoTracking.BL.EntityQueryServices
{ 
   public partial class CargoTrackingShipmentQueryService: EntityQueryService<CargoTrackingShipment,CargoTrackingShipmentKeys,CargoTrackingShipmentPM,object,CargoTrackingShipmentKeys>
   {
   
        CargoTrackingShipmentRepository repository;
		ICargoTrackingContext  context;
        public CargoTrackingShipmentQueryService(int tenant)
        {
		    context = CargoTrackingContext.GetContext(tenant);
            MainContext = context;
            repository = new CargoTrackingShipmentRepository(context);
            Repository = repository;
            mapping = new CargoTrackingShipmentDataMapping();
        }

        public CargoTrackingShipmentQueryService(CargoTrackingShipmentRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CargoTrackingShipmentDataMapping();
        }

        public CargoTrackingShipmentQueryService(ICargoTrackingContext context)
        {
            this.repository = new CargoTrackingShipmentRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CargoTrackingShipmentDataMapping();
        }

        protected override EntityKeyFields GetKeys(CargoTrackingShipment entityPOCO)
        {
            CargoTrackingShipmentKeys entityKeys = new CargoTrackingShipmentKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 