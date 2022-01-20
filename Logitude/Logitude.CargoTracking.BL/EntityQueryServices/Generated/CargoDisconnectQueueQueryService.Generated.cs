 
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
   public partial class CargoDisconnectQueueQueryService: EntityQueryService<CargoDisconnectQueue,CargoDisconnectQueueKeys,CargoDisconnectQueuePM,object,CargoDisconnectQueueKeys>
   {
   
        CargoDisconnectQueueRepository repository;
		ICargoTrackingContext  context;
        public CargoDisconnectQueueQueryService(int tenant)
        {
		    context = CargoTrackingContext.GetContext(tenant);
            MainContext = context;
            repository = new CargoDisconnectQueueRepository(context);
            Repository = repository;
            mapping = new CargoDisconnectQueueDataMapping();
        }

        public CargoDisconnectQueueQueryService(CargoDisconnectQueueRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CargoDisconnectQueueDataMapping();
        }

        public CargoDisconnectQueueQueryService(ICargoTrackingContext context)
        {
            this.repository = new CargoDisconnectQueueRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CargoDisconnectQueueDataMapping();
        }
		 
		public  CargoDisconnectQueuePM GetSingle(int id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CargoDisconnectQueueKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CargoDisconnectQueue entityPOCO)
        {
            CargoDisconnectQueueKeys entityKeys = new CargoDisconnectQueueKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 