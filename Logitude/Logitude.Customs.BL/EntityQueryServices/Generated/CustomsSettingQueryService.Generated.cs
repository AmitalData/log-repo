 
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
   public partial class CustomsSettingQueryService: EntityQueryService<CustomsSetting,CustomsSettingKeys,CustomsSettingPM,object,CustomsSettingKeys>
   {
   
        CustomsSettingRepository repository;
		ICustomContext  context;
        public CustomsSettingQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomsSettingRepository(context);
            Repository = repository;
            mapping = new CustomsSettingDataMapping();
        }

        public CustomsSettingQueryService(CustomsSettingRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomsSettingDataMapping();
        }

        public CustomsSettingQueryService(ICustomContext context)
        {
            this.repository = new CustomsSettingRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomsSettingDataMapping();
        }
		 
		public  CustomsSettingPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomsSettingKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomsSetting entityPOCO)
        {
            CustomsSettingKeys entityKeys = new CustomsSettingKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 