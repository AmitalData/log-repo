 
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
   public partial class CB_TariffQueryService: EntityQueryService<CB_Tariff,CB_TariffKeys,CB_TariffPM,object,CB_TariffKeys>
   {
   
        CB_TariffRepository repository;
		ICustomContext  context;
        public CB_TariffQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CB_TariffRepository(context);
            Repository = repository;
            mapping = new CB_TariffDataMapping();
        }

        public CB_TariffQueryService(CB_TariffRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CB_TariffDataMapping();
        }

        public CB_TariffQueryService(ICustomContext context)
        {
            this.repository = new CB_TariffRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CB_TariffDataMapping();
        }
		 
		public  CB_TariffPM GetSingle(string cb_id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CB_TariffKeys(){ CB_ID = cb_id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CB_Tariff entityPOCO)
        {
            CB_TariffKeys entityKeys = new CB_TariffKeys() { CB_ID = entityPOCO.CB_ID,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 