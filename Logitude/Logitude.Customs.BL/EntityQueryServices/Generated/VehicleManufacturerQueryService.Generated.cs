 
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
   public partial class VehicleManufacturerQueryService: EntityQueryService<VehicleManufacturer,VehicleManufacturerKeys,VehicleManufacturerPM,object,VehicleManufacturerKeys>
   {
   
        VehicleManufacturerRepository repository;
		ICustomContext  context;
        public VehicleManufacturerQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new VehicleManufacturerRepository(context);
            Repository = repository;
            mapping = new VehicleManufacturerDataMapping();
        }

        public VehicleManufacturerQueryService(VehicleManufacturerRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new VehicleManufacturerDataMapping();
        }

        public VehicleManufacturerQueryService(ICustomContext context)
        {
            this.repository = new VehicleManufacturerRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new VehicleManufacturerDataMapping();
        }
		 
		public  VehicleManufacturerPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new VehicleManufacturerKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(VehicleManufacturer entityPOCO)
        {
            VehicleManufacturerKeys entityKeys = new VehicleManufacturerKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 