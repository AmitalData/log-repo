 
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
   public partial class CargoTrackingPortQueryService: EntityQueryService<CargoTrackingPort,CargoTrackingPortKeys,CargoTrackingPortPM,object,CargoTrackingPortKeys>
   {
   
        CargoTrackingPortRepository repository;
		ICargoTrackingContext  context;
        public CargoTrackingPortQueryService(int tenant)
        {
		    context = CargoTrackingContext.GetContext(tenant);
            MainContext = context;
            repository = new CargoTrackingPortRepository(context);
            Repository = repository;
            mapping = new CargoTrackingPortDataMapping();
        }

        public CargoTrackingPortQueryService(CargoTrackingPortRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CargoTrackingPortDataMapping();
        }

        public CargoTrackingPortQueryService(ICargoTrackingContext context)
        {
            this.repository = new CargoTrackingPortRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CargoTrackingPortDataMapping();
        }
		 
		public  CargoTrackingPortPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CargoTrackingPortKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CargoTrackingPort entityPOCO)
        {
            CargoTrackingPortKeys entityKeys = new CargoTrackingPortKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 