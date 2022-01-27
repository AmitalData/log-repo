 
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
   public partial class CustomsEnvironmentSettingQueryService: EntityQueryService<CustomsEnvironmentSetting,CustomsEnvironmentSettingKeys,CustomsEnvironmentSettingPM,object,CustomsEnvironmentSettingKeys>
   {
   
        CustomsEnvironmentSettingRepository repository;
		ICustomContext  context;
        public CustomsEnvironmentSettingQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomsEnvironmentSettingRepository(context);
            Repository = repository;
            mapping = new CustomsEnvironmentSettingDataMapping();
        }

        public CustomsEnvironmentSettingQueryService(CustomsEnvironmentSettingRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomsEnvironmentSettingDataMapping();
        }

        public CustomsEnvironmentSettingQueryService(ICustomContext context)
        {
            this.repository = new CustomsEnvironmentSettingRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomsEnvironmentSettingDataMapping();
        }
		 
		public  CustomsEnvironmentSettingPM GetSingle(string id, string environmentcode,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomsEnvironmentSettingKeys(){ Id = id, EnvironmentCode = environmentcode };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomsEnvironmentSetting entityPOCO)
        {
            CustomsEnvironmentSettingKeys entityKeys = new CustomsEnvironmentSettingKeys() { Id = entityPOCO.Id, EnvironmentCode = entityPOCO.EnvironmentCode,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 