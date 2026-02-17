 
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
   public partial class VehicleQueryService: EntityQueryService<Vehicle,VehicleKeys,VehiclePM,object,VehicleKeys>
   {
   
        VehicleRepository repository;
		ICustomContext  context;
        public VehicleQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new VehicleRepository(context);
            Repository = repository;
            mapping = new VehicleDataMapping();
        }

        public VehicleQueryService(VehicleRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new VehicleDataMapping();
        }

        public VehicleQueryService(ICustomContext context)
        {
            this.repository = new VehicleRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new VehicleDataMapping();
        }
		 
		public  VehiclePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new VehicleKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Vehicle entityPOCO)
        {
            VehicleKeys entityKeys = new VehicleKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 