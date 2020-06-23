 
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
   public partial class CargoTrackingPort2QueryService: EntityQueryService<CargoTrackingPort2,CargoTrackingPort2Keys,CargoTrackingPort2PM,object,CargoTrackingPort2Keys>
   {
   
        CargoTrackingPort2Repository repository;
		ICargoTrackingContext  context;
        public CargoTrackingPort2QueryService(int tenant)
        {
		    context = CargoTrackingContext.GetContext(tenant);
            MainContext = context;
            repository = new CargoTrackingPort2Repository(context);
            Repository = repository;
            mapping = new CargoTrackingPort2DataMapping();
        }

        public CargoTrackingPort2QueryService(CargoTrackingPort2Repository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CargoTrackingPort2DataMapping();
        }

        public CargoTrackingPort2QueryService(ICargoTrackingContext context)
        {
            this.repository = new CargoTrackingPort2Repository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CargoTrackingPort2DataMapping();
        }
		 
		public  CargoTrackingPort2PM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CargoTrackingPort2Keys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CargoTrackingPort2 entityPOCO)
        {
            CargoTrackingPort2Keys entityKeys = new CargoTrackingPort2Keys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 