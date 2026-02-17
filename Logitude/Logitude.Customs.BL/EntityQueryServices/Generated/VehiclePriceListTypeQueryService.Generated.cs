 
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
   public partial class VehiclePriceListTypeQueryService: EntityQueryService<VehiclePriceListType,VehiclePriceListTypeKeys,VehiclePriceListTypePM,object,VehiclePriceListTypeKeys>
   {
   
        VehiclePriceListTypeRepository repository;
		ICustomContext  context;
        public VehiclePriceListTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new VehiclePriceListTypeRepository(context);
            Repository = repository;
            mapping = new VehiclePriceListTypeDataMapping();
        }

        public VehiclePriceListTypeQueryService(VehiclePriceListTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new VehiclePriceListTypeDataMapping();
        }

        public VehiclePriceListTypeQueryService(ICustomContext context)
        {
            this.repository = new VehiclePriceListTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new VehiclePriceListTypeDataMapping();
        }
		 
		public  VehiclePriceListTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new VehiclePriceListTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(VehiclePriceListType entityPOCO)
        {
            VehiclePriceListTypeKeys entityKeys = new VehiclePriceListTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 