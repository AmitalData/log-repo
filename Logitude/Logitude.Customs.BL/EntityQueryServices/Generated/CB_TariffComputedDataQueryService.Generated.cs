 
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
   public partial class CB_TariffComputedDataQueryService: EntityQueryService<CB_TariffComputedData,CB_TariffComputedDataKeys,CB_TariffComputedDataPM,object,CB_TariffComputedDataKeys>
   {
   
        CB_TariffComputedDataRepository repository;
		ICustomContext  context;
        public CB_TariffComputedDataQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CB_TariffComputedDataRepository(context);
            Repository = repository;
            mapping = new CB_TariffComputedDataDataMapping();
        }

        public CB_TariffComputedDataQueryService(CB_TariffComputedDataRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CB_TariffComputedDataDataMapping();
        }

        public CB_TariffComputedDataQueryService(ICustomContext context)
        {
            this.repository = new CB_TariffComputedDataRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CB_TariffComputedDataDataMapping();
        }
		 
		public  CB_TariffComputedDataPM GetSingle(string cb_id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CB_TariffComputedDataKeys(){ CB_ID = cb_id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CB_TariffComputedData entityPOCO)
        {
            CB_TariffComputedDataKeys entityKeys = new CB_TariffComputedDataKeys() { CB_ID = entityPOCO.CB_ID,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 