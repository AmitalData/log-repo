 
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
   public partial class TariffSurchargesUpdateQueryService: EntityQueryService<TariffSurchargesUpdate,TariffSurchargesUpdateKeys,TariffSurchargesUpdatePM,object,TariffSurchargesUpdateKeys>
   {
   
        TariffSurchargesUpdateRepository repository;
		ITariffModuleContext  context;
        public TariffSurchargesUpdateQueryService(int tenant)
        {
		    context = TariffModuleContext.GetContext(tenant);
            MainContext = context;
            repository = new TariffSurchargesUpdateRepository(context);
            Repository = repository;
            mapping = new TariffSurchargesUpdateDataMapping();
        }

        public TariffSurchargesUpdateQueryService(TariffSurchargesUpdateRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TariffSurchargesUpdateDataMapping();
        }

        public TariffSurchargesUpdateQueryService(ITariffModuleContext context)
        {
            this.repository = new TariffSurchargesUpdateRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TariffSurchargesUpdateDataMapping();
        }
		 
		public  TariffSurchargesUpdatePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TariffSurchargesUpdateKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TariffSurchargesUpdate entityPOCO)
        {
            TariffSurchargesUpdateKeys entityKeys = new TariffSurchargesUpdateKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 