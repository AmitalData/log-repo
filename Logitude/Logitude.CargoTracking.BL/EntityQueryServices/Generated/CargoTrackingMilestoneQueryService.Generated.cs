 
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
   public partial class CargoTrackingMilestoneQueryService: EntityQueryService<CargoTrackingMilestone,CargoTrackingMilestoneKeys,CargoTrackingMilestonePM,object,CargoTrackingMilestoneKeys>
   {
   
        CargoTrackingMilestoneRepository repository;
		ICargoTrackingContext  context;
        public CargoTrackingMilestoneQueryService(int tenant)
        {
		    context = CargoTrackingContext.GetContext(tenant);
            MainContext = context;
            repository = new CargoTrackingMilestoneRepository(context);
            Repository = repository;
            mapping = new CargoTrackingMilestoneDataMapping();
        }

        public CargoTrackingMilestoneQueryService(CargoTrackingMilestoneRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CargoTrackingMilestoneDataMapping();
        }

        public CargoTrackingMilestoneQueryService(ICargoTrackingContext context)
        {
            this.repository = new CargoTrackingMilestoneRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CargoTrackingMilestoneDataMapping();
        }
		 
		public  CargoTrackingMilestonePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CargoTrackingMilestoneKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CargoTrackingMilestone entityPOCO)
        {
            CargoTrackingMilestoneKeys entityKeys = new CargoTrackingMilestoneKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 