 
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
   public partial class TariffSurchargesUpdateMethodQueryService: EntityQueryService<TariffSurchargesUpdateMethod,TariffSurchargesUpdateMethodKeys,TariffSurchargesUpdateMethodPM,object,TariffSurchargesUpdateMethodKeys>
   {
   
        TariffSurchargesUpdateMethodRepository repository;
		ITariffModuleContext  context;
        public TariffSurchargesUpdateMethodQueryService(int tenant)
        {
		    context = TariffModuleContext.GetContext(tenant);
            MainContext = context;
            repository = new TariffSurchargesUpdateMethodRepository(context);
            Repository = repository;
            mapping = new TariffSurchargesUpdateMethodDataMapping();
        }

        public TariffSurchargesUpdateMethodQueryService(TariffSurchargesUpdateMethodRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TariffSurchargesUpdateMethodDataMapping();
        }

        public TariffSurchargesUpdateMethodQueryService(ITariffModuleContext context)
        {
            this.repository = new TariffSurchargesUpdateMethodRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TariffSurchargesUpdateMethodDataMapping();
        }
		 
		public  TariffSurchargesUpdateMethodPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TariffSurchargesUpdateMethodKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TariffSurchargesUpdateMethod entityPOCO)
        {
            TariffSurchargesUpdateMethodKeys entityKeys = new TariffSurchargesUpdateMethodKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 