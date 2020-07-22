 
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
   public partial class CargoTrackingTransportModeQueryService: EntityQueryService<CargoTrackingTransportMode,CargoTrackingTransportModeKeys,CargoTrackingTransportModePM,object,CargoTrackingTransportModeKeys>
   {
   
        CargoTrackingTransportModeRepository repository;
		ICargoTrackingContext  context;
        public CargoTrackingTransportModeQueryService(int tenant)
        {
		    context = CargoTrackingContext.GetContext(tenant);
            MainContext = context;
            repository = new CargoTrackingTransportModeRepository(context);
            Repository = repository;
            mapping = new CargoTrackingTransportModeDataMapping();
        }

        public CargoTrackingTransportModeQueryService(CargoTrackingTransportModeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CargoTrackingTransportModeDataMapping();
        }

        public CargoTrackingTransportModeQueryService(ICargoTrackingContext context)
        {
            this.repository = new CargoTrackingTransportModeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CargoTrackingTransportModeDataMapping();
        }
		 
		public  CargoTrackingTransportModePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CargoTrackingTransportModeKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CargoTrackingTransportMode entityPOCO)
        {
            CargoTrackingTransportModeKeys entityKeys = new CargoTrackingTransportModeKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 