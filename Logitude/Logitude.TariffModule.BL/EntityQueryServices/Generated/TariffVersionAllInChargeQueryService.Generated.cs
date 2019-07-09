 
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
   public partial class TariffVersionAllInChargeQueryService: EntityQueryService<TariffVersionAllInCharge,TariffVersionAllInChargeKeys,TariffVersionAllInChargePM,TariffVersionPM,TariffVersionKeys>
   {
   
        TariffVersionAllInChargeRepository repository;
		ITariffModuleContext  context;
        public TariffVersionAllInChargeQueryService(int tenant)
        {
		    context = TariffModuleContext.GetContext(tenant);
            MainContext = context;
            repository = new TariffVersionAllInChargeRepository(context);
            Repository = repository;
            mapping = new TariffVersionAllInChargeDataMapping();
        }

        public TariffVersionAllInChargeQueryService(TariffVersionAllInChargeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TariffVersionAllInChargeDataMapping();
        }

        public TariffVersionAllInChargeQueryService(ITariffModuleContext context)
        {
            this.repository = new TariffVersionAllInChargeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TariffVersionAllInChargeDataMapping();
        }
		 
		public  TariffVersionAllInChargePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TariffVersionAllInChargeKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TariffVersionAllInCharge entityPOCO)
        {
            TariffVersionAllInChargeKeys entityKeys = new TariffVersionAllInChargeKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 