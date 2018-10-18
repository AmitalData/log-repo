 
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
   public partial class VehicleSafetyAccessoryTypeQueryService: EntityQueryService<VehicleSafetyAccessoryType,VehicleSafetyAccessoryTypeKeys,VehicleSafetyAccessoryTypePM,object,VehicleSafetyAccessoryTypeKeys>
   {
   
        VehicleSafetyAccessoryTypeRepository repository;
		ICustomContext  context;
        public VehicleSafetyAccessoryTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new VehicleSafetyAccessoryTypeRepository(context);
            Repository = repository;
            mapping = new VehicleSafetyAccessoryTypeDataMapping();
        }

        public VehicleSafetyAccessoryTypeQueryService(VehicleSafetyAccessoryTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new VehicleSafetyAccessoryTypeDataMapping();
        }

        public VehicleSafetyAccessoryTypeQueryService(ICustomContext context)
        {
            this.repository = new VehicleSafetyAccessoryTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new VehicleSafetyAccessoryTypeDataMapping();
        }
		 
		public  VehicleSafetyAccessoryTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new VehicleSafetyAccessoryTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(VehicleSafetyAccessoryType entityPOCO)
        {
            VehicleSafetyAccessoryTypeKeys entityKeys = new VehicleSafetyAccessoryTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 