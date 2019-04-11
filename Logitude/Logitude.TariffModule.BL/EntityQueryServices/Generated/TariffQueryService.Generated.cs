 
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
   public partial class TariffQueryService: EntityQueryService<Tariff,TariffKeys,TariffPM,object,TariffKeys>
   {
   
        TariffRepository repository;
		ITariffModuleContext  context;
        public TariffQueryService(int tenant)
        {
		    context = TariffModuleContext.GetContext(tenant);
            MainContext = context;
            repository = new TariffRepository(context);
            Repository = repository;
            mapping = new TariffDataMapping();
        }

        public TariffQueryService(TariffRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TariffDataMapping();
        }

        public TariffQueryService(ITariffModuleContext context)
        {
            this.repository = new TariffRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TariffDataMapping();
        }
		 
		public  TariffPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TariffKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Tariff entityPOCO)
        {
            TariffKeys entityKeys = new TariffKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 