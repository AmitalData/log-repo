 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Customs.BL.EntityQueryServices
{ 
   public partial class VehicleSafetyAccessoryQueryService: EntityQueryService<VehicleSafetyAccessory,VehicleSafetyAccessoryKeys,VehicleSafetyAccessoryPM,VehiclePM,VehicleKeys>
   {
   
        VehicleSafetyAccessoryRepository repository;
		ICustomContext  context;
        public VehicleSafetyAccessoryQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new VehicleSafetyAccessoryRepository(context);
            Repository = repository;
            mapping = new VehicleSafetyAccessoryDataMapping();
        }

        public VehicleSafetyAccessoryQueryService(VehicleSafetyAccessoryRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new VehicleSafetyAccessoryDataMapping();
        }

        public VehicleSafetyAccessoryQueryService(ICustomContext context)
        {
            this.repository = new VehicleSafetyAccessoryRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new VehicleSafetyAccessoryDataMapping();
        }
		 
		public  VehicleSafetyAccessoryPM GetSingle(string vehicleid, int linenumber,bool getComposition, bool getFromCache)
        {
             EntityKeys = new VehicleSafetyAccessoryKeys(){ VehicleId = vehicleid, LineNumber = linenumber };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(VehicleSafetyAccessory entityPOCO)
        {
            VehicleSafetyAccessoryKeys entityKeys = new VehicleSafetyAccessoryKeys() { VehicleId = entityPOCO.VehicleId, LineNumber = entityPOCO.LineNumber,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 