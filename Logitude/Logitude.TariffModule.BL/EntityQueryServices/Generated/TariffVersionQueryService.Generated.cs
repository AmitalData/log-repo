 
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
   public partial class TariffVersionQueryService: EntityQueryService<TariffVersion,TariffVersionKeys,TariffVersionPM,TariffPM,TariffKeys>
   {
   
        TariffVersionRepository repository;
		ITariffModuleContext  context;
        public TariffVersionQueryService(int tenant)
        {
		    context = TariffModuleContext.GetContext(tenant);
            MainContext = context;
            repository = new TariffVersionRepository(context);
            Repository = repository;
            mapping = new TariffVersionDataMapping();
        }

        public TariffVersionQueryService(TariffVersionRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TariffVersionDataMapping();
        }

        public TariffVersionQueryService(ITariffModuleContext context)
        {
            this.repository = new TariffVersionRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TariffVersionDataMapping();
        }
		 
		public  TariffVersionPM GetSingle(string tariffid, int version,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TariffVersionKeys(){ TariffId = tariffid, Version = version };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TariffVersion entityPOCO)
        {
            TariffVersionKeys entityKeys = new TariffVersionKeys() { TariffId = entityPOCO.TariffId, Version = entityPOCO.Version,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 