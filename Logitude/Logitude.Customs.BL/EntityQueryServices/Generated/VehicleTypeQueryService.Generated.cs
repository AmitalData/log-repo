 
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
   public partial class VehicleTypeQueryService: EntityQueryService<VehicleType,VehicleTypeKeys,VehicleTypePM,object,VehicleTypeKeys>
   {
   
        VehicleTypeRepository repository;
		ICustomContext  context;
        public VehicleTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new VehicleTypeRepository(context);
            Repository = repository;
            mapping = new VehicleTypeDataMapping();
        }

        public VehicleTypeQueryService(VehicleTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new VehicleTypeDataMapping();
        }

        public VehicleTypeQueryService(ICustomContext context)
        {
            this.repository = new VehicleTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new VehicleTypeDataMapping();
        }
		 
		public  VehicleTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new VehicleTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(VehicleType entityPOCO)
        {
            VehicleTypeKeys entityKeys = new VehicleTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 