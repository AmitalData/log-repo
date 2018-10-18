 
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
   public partial class VehicleReductionTypeQueryService: EntityQueryService<VehicleReductionType,VehicleReductionTypeKeys,VehicleReductionTypePM,object,VehicleReductionTypeKeys>
   {
   
        VehicleReductionTypeRepository repository;
		ICustomContext  context;
        public VehicleReductionTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new VehicleReductionTypeRepository(context);
            Repository = repository;
            mapping = new VehicleReductionTypeDataMapping();
        }

        public VehicleReductionTypeQueryService(VehicleReductionTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new VehicleReductionTypeDataMapping();
        }

        public VehicleReductionTypeQueryService(ICustomContext context)
        {
            this.repository = new VehicleReductionTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new VehicleReductionTypeDataMapping();
        }
		 
		public  VehicleReductionTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new VehicleReductionTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(VehicleReductionType entityPOCO)
        {
            VehicleReductionTypeKeys entityKeys = new VehicleReductionTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 