 
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
   public partial class CargoTrackingCountryQueryService: EntityQueryService<CargoTrackingCountry,CargoTrackingCountryKeys,CargoTrackingCountryPM,object,CargoTrackingCountryKeys>
   {
   
        CargoTrackingCountryRepository repository;
		ICargoTrackingContext  context;
        public CargoTrackingCountryQueryService(int tenant)
        {
		    context = CargoTrackingContext.GetContext(tenant);
            MainContext = context;
            repository = new CargoTrackingCountryRepository(context);
            Repository = repository;
            mapping = new CargoTrackingCountryDataMapping();
        }

        public CargoTrackingCountryQueryService(CargoTrackingCountryRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CargoTrackingCountryDataMapping();
        }

        public CargoTrackingCountryQueryService(ICargoTrackingContext context)
        {
            this.repository = new CargoTrackingCountryRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CargoTrackingCountryDataMapping();
        }
		 
		public  CargoTrackingCountryPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CargoTrackingCountryKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CargoTrackingCountry entityPOCO)
        {
            CargoTrackingCountryKeys entityKeys = new CargoTrackingCountryKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 