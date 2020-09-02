 
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
   public partial class CargoTrackingShipmentComputedQueryService: EntityQueryService<CargoTrackingShipmentComputed,CargoTrackingShipmentComputedKeys,CargoTrackingShipmentComputedPM,object,CargoTrackingShipmentComputedKeys>
   {
   
        CargoTrackingShipmentComputedRepository repository;
		ICargoTrackingContext  context;
        public CargoTrackingShipmentComputedQueryService(int tenant)
        {
		    context = CargoTrackingContext.GetContext(tenant);
            MainContext = context;
            repository = new CargoTrackingShipmentComputedRepository(context);
            Repository = repository;
            mapping = new CargoTrackingShipmentComputedDataMapping();
        }

        public CargoTrackingShipmentComputedQueryService(CargoTrackingShipmentComputedRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CargoTrackingShipmentComputedDataMapping();
        }

        public CargoTrackingShipmentComputedQueryService(ICargoTrackingContext context)
        {
            this.repository = new CargoTrackingShipmentComputedRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CargoTrackingShipmentComputedDataMapping();
        }
		 
		public  CargoTrackingShipmentComputedPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CargoTrackingShipmentComputedKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CargoTrackingShipmentComputed entityPOCO)
        {
            CargoTrackingShipmentComputedKeys entityKeys = new CargoTrackingShipmentComputedKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 