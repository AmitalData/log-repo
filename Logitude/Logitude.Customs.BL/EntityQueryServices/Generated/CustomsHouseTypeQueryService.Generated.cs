 
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
   public partial class CustomsHouseTypeQueryService: EntityQueryService<CustomsHouseType,CustomsHouseTypeKeys,CustomsHouseTypePM,object,CustomsHouseTypeKeys>
   {
   
        CustomsHouseTypeRepository repository;
		ICustomContext  context;
        public CustomsHouseTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomsHouseTypeRepository(context);
            Repository = repository;
            mapping = new CustomsHouseTypeDataMapping();
        }

        public CustomsHouseTypeQueryService(CustomsHouseTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomsHouseTypeDataMapping();
        }

        public CustomsHouseTypeQueryService(ICustomContext context)
        {
            this.repository = new CustomsHouseTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomsHouseTypeDataMapping();
        }
		 
		public  CustomsHouseTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomsHouseTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomsHouseType entityPOCO)
        {
            CustomsHouseTypeKeys entityKeys = new CustomsHouseTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 