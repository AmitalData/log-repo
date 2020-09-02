 
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
   public partial class CargoTrackingWatermarkQueryService: EntityQueryService<CargoTrackingWatermark,CargoTrackingWatermarkKeys,CargoTrackingWatermarkPM,object,CargoTrackingWatermarkKeys>
   {
   
        CargoTrackingWatermarkRepository repository;
		ICargoTrackingContext  context;
        public CargoTrackingWatermarkQueryService(int tenant)
        {
		    context = CargoTrackingContext.GetContext(tenant);
            MainContext = context;
            repository = new CargoTrackingWatermarkRepository(context);
            Repository = repository;
            mapping = new CargoTrackingWatermarkDataMapping();
        }

        public CargoTrackingWatermarkQueryService(CargoTrackingWatermarkRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CargoTrackingWatermarkDataMapping();
        }

        public CargoTrackingWatermarkQueryService(ICargoTrackingContext context)
        {
            this.repository = new CargoTrackingWatermarkRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CargoTrackingWatermarkDataMapping();
        }
		 
		public  CargoTrackingWatermarkPM GetSingle(string tablename,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CargoTrackingWatermarkKeys(){ TableName = tablename };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CargoTrackingWatermark entityPOCO)
        {
            CargoTrackingWatermarkKeys entityKeys = new CargoTrackingWatermarkKeys() { TableName = entityPOCO.TableName,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 