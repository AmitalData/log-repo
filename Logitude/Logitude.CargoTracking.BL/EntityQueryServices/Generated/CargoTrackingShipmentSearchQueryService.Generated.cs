 
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
   public partial class CargoTrackingShipmentSearchQueryService: EntityQueryService<CargoTrackingShipmentSearch,CargoTrackingShipmentSearchKeys,CargoTrackingShipmentSearchPM,object,CargoTrackingShipmentSearchKeys>
   {
   
        CargoTrackingShipmentSearchRepository repository;
		ICargoTrackingContext  context;
        public CargoTrackingShipmentSearchQueryService(int tenant)
        {
		    context = CargoTrackingContext.GetContext(tenant);
            MainContext = context;
            repository = new CargoTrackingShipmentSearchRepository(context);
            Repository = repository;
            mapping = new CargoTrackingShipmentSearchDataMapping();
        }

        public CargoTrackingShipmentSearchQueryService(CargoTrackingShipmentSearchRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CargoTrackingShipmentSearchDataMapping();
        }

        public CargoTrackingShipmentSearchQueryService(ICargoTrackingContext context)
        {
            this.repository = new CargoTrackingShipmentSearchRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CargoTrackingShipmentSearchDataMapping();
        }
		 
		public  CargoTrackingShipmentSearchPM GetSingle(int id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CargoTrackingShipmentSearchKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CargoTrackingShipmentSearch entityPOCO)
        {
            CargoTrackingShipmentSearchKeys entityKeys = new CargoTrackingShipmentSearchKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 