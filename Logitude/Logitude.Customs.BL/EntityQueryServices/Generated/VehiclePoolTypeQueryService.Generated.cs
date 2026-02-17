 
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
   public partial class VehiclePoolTypeQueryService: EntityQueryService<VehiclePoolType,VehiclePoolTypeKeys,VehiclePoolTypePM,object,VehiclePoolTypeKeys>
   {
   
        VehiclePoolTypeRepository repository;
		ICustomContext  context;
        public VehiclePoolTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new VehiclePoolTypeRepository(context);
            Repository = repository;
            mapping = new VehiclePoolTypeDataMapping();
        }

        public VehiclePoolTypeQueryService(VehiclePoolTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new VehiclePoolTypeDataMapping();
        }

        public VehiclePoolTypeQueryService(ICustomContext context)
        {
            this.repository = new VehiclePoolTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new VehiclePoolTypeDataMapping();
        }
		 
		public  VehiclePoolTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new VehiclePoolTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(VehiclePoolType entityPOCO)
        {
            VehiclePoolTypeKeys entityKeys = new VehiclePoolTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 