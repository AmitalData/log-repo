 
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
   public partial class CargoTrackingIncrementalStatQueryService: EntityQueryService<CargoTrackingIncrementalStat,CargoTrackingIncrementalStatKeys,CargoTrackingIncrementalStatPM,object,CargoTrackingIncrementalStatKeys>
   {
   
        CargoTrackingIncrementalStatRepository repository;
		ICargoTrackingContext  context;
        public CargoTrackingIncrementalStatQueryService(int tenant)
        {
		    context = CargoTrackingContext.GetContext(tenant);
            MainContext = context;
            repository = new CargoTrackingIncrementalStatRepository(context);
            Repository = repository;
            mapping = new CargoTrackingIncrementalStatDataMapping();
        }

        public CargoTrackingIncrementalStatQueryService(CargoTrackingIncrementalStatRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CargoTrackingIncrementalStatDataMapping();
        }

        public CargoTrackingIncrementalStatQueryService(ICargoTrackingContext context)
        {
            this.repository = new CargoTrackingIncrementalStatRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CargoTrackingIncrementalStatDataMapping();
        }
		 
		public  CargoTrackingIncrementalStatPM GetSingle(int id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CargoTrackingIncrementalStatKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CargoTrackingIncrementalStat entityPOCO)
        {
            CargoTrackingIncrementalStatKeys entityKeys = new CargoTrackingIncrementalStatKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 