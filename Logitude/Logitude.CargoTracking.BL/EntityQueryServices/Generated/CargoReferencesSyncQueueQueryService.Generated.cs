 
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
   public partial class CargoReferencesSyncQueueQueryService: EntityQueryService<CargoReferencesSyncQueue,CargoReferencesSyncQueueKeys,CargoReferencesSyncQueuePM,object,CargoReferencesSyncQueueKeys>
   {
   
        CargoReferencesSyncQueueRepository repository;
		ICargoTrackingContext  context;
        public CargoReferencesSyncQueueQueryService(int tenant)
        {
		    context = CargoTrackingContext.GetContext(tenant);
            MainContext = context;
            repository = new CargoReferencesSyncQueueRepository(context);
            Repository = repository;
            mapping = new CargoReferencesSyncQueueDataMapping();
        }

        public CargoReferencesSyncQueueQueryService(CargoReferencesSyncQueueRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CargoReferencesSyncQueueDataMapping();
        }

        public CargoReferencesSyncQueueQueryService(ICargoTrackingContext context)
        {
            this.repository = new CargoReferencesSyncQueueRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CargoReferencesSyncQueueDataMapping();
        }
		 
		public  CargoReferencesSyncQueuePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CargoReferencesSyncQueueKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CargoReferencesSyncQueue entityPOCO)
        {
            CargoReferencesSyncQueueKeys entityKeys = new CargoReferencesSyncQueueKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 