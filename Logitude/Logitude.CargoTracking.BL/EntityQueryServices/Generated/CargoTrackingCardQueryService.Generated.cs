 
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
   public partial class CargoTrackingCardQueryService: EntityQueryService<CargoTrackingCard,CargoTrackingCardKeys,CargoTrackingCardPM,object,CargoTrackingCardKeys>
   {
   
        CargoTrackingCardRepository repository;
		ICargoTrackingContext  context;
        public CargoTrackingCardQueryService(int tenant)
        {
		    context = CargoTrackingContext.GetContext(tenant);
            MainContext = context;
            repository = new CargoTrackingCardRepository(context);
            Repository = repository;
            mapping = new CargoTrackingCardDataMapping();
        }

        public CargoTrackingCardQueryService(CargoTrackingCardRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CargoTrackingCardDataMapping();
        }

        public CargoTrackingCardQueryService(ICargoTrackingContext context)
        {
            this.repository = new CargoTrackingCardRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CargoTrackingCardDataMapping();
        }
		 
		public  CargoTrackingCardPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CargoTrackingCardKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CargoTrackingCard entityPOCO)
        {
            CargoTrackingCardKeys entityKeys = new CargoTrackingCardKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 