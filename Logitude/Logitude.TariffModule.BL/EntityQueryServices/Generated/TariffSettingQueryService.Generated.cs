 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.BL.EntityPMs;
using Logitude.TariffModule.BL.EntityDataMappings;
using Logitude.TariffModule.Data.Repositories;
using Logitude.TariffModule.Data.EntityKeys;
using Logitude.TariffModule.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.TariffModule.BL.EntityQueryServices
{ 
   public partial class TariffSettingQueryService: EntityQueryService<TariffSetting,TariffSettingKeys,TariffSettingPM,object,TariffSettingKeys>
   {
   
        TariffSettingRepository repository;
		ITariffModuleContext  context;
        public TariffSettingQueryService(int tenant)
        {
		    context = TariffModuleContext.GetContext(tenant);
            MainContext = context;
            repository = new TariffSettingRepository(context);
            Repository = repository;
            mapping = new TariffSettingDataMapping();
        }

        public TariffSettingQueryService(TariffSettingRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TariffSettingDataMapping();
        }

        public TariffSettingQueryService(ITariffModuleContext context)
        {
            this.repository = new TariffSettingRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TariffSettingDataMapping();
        }
		 
		public  TariffSettingPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TariffSettingKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TariffSetting entityPOCO)
        {
            TariffSettingKeys entityKeys = new TariffSettingKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 