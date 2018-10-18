 
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
   public partial class CustomsHouseTypeAdditionalQueryService: EntityQueryService<CustomsHouseTypeAdditional,CustomsHouseTypeAdditionalKeys,CustomsHouseTypeAdditionalPM,object,CustomsHouseTypeAdditionalKeys>
   {
   
        CustomsHouseTypeAdditionalRepository repository;
		ICustomContext  context;
        public CustomsHouseTypeAdditionalQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomsHouseTypeAdditionalRepository(context);
            Repository = repository;
            mapping = new CustomsHouseTypeAdditionalDataMapping();
        }

        public CustomsHouseTypeAdditionalQueryService(CustomsHouseTypeAdditionalRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomsHouseTypeAdditionalDataMapping();
        }

        public CustomsHouseTypeAdditionalQueryService(ICustomContext context)
        {
            this.repository = new CustomsHouseTypeAdditionalRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomsHouseTypeAdditionalDataMapping();
        }
		 
		public  CustomsHouseTypeAdditionalPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomsHouseTypeAdditionalKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomsHouseTypeAdditional entityPOCO)
        {
            CustomsHouseTypeAdditionalKeys entityKeys = new CustomsHouseTypeAdditionalKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 