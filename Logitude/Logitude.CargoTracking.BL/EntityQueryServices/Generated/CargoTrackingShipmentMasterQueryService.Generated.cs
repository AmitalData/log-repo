 
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
   public partial class CargoTrackingShipmentMasterQueryService: EntityQueryService<CargoTrackingShipmentMaster,CargoTrackingShipmentMasterKeys,CargoTrackingShipmentMasterPM,object,CargoTrackingShipmentMasterKeys>
   {
   
        CargoTrackingShipmentMasterRepository repository;
		ICargoTrackingContext  context;
        public CargoTrackingShipmentMasterQueryService(int tenant)
        {
		    context = CargoTrackingContext.GetContext(tenant);
            MainContext = context;
            repository = new CargoTrackingShipmentMasterRepository(context);
            Repository = repository;
            mapping = new CargoTrackingShipmentMasterDataMapping();
        }

        public CargoTrackingShipmentMasterQueryService(CargoTrackingShipmentMasterRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CargoTrackingShipmentMasterDataMapping();
        }

        public CargoTrackingShipmentMasterQueryService(ICargoTrackingContext context)
        {
            this.repository = new CargoTrackingShipmentMasterRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CargoTrackingShipmentMasterDataMapping();
        }
		 
		public  CargoTrackingShipmentMasterPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CargoTrackingShipmentMasterKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CargoTrackingShipmentMaster entityPOCO)
        {
            CargoTrackingShipmentMasterKeys entityKeys = new CargoTrackingShipmentMasterKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 