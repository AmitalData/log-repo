 
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
   public partial class CargoTrackingHeaderEntityTypeQueryService: EntityQueryService<CargoTrackingHeaderEntityType,CargoTrackingHeaderEntityTypeKeys,CargoTrackingHeaderEntityTypePM,object,CargoTrackingHeaderEntityTypeKeys>
   {
   
        CargoTrackingHeaderEntityTypeRepository repository;
		ICargoTrackingContext  context;
        public CargoTrackingHeaderEntityTypeQueryService(int tenant)
        {
		    context = CargoTrackingContext.GetContext(tenant);
            MainContext = context;
            repository = new CargoTrackingHeaderEntityTypeRepository(context);
            Repository = repository;
            mapping = new CargoTrackingHeaderEntityTypeDataMapping();
        }

        public CargoTrackingHeaderEntityTypeQueryService(CargoTrackingHeaderEntityTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CargoTrackingHeaderEntityTypeDataMapping();
        }

        public CargoTrackingHeaderEntityTypeQueryService(ICargoTrackingContext context)
        {
            this.repository = new CargoTrackingHeaderEntityTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CargoTrackingHeaderEntityTypeDataMapping();
        }
		 
		public  CargoTrackingHeaderEntityTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CargoTrackingHeaderEntityTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CargoTrackingHeaderEntityType entityPOCO)
        {
            CargoTrackingHeaderEntityTypeKeys entityKeys = new CargoTrackingHeaderEntityTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 