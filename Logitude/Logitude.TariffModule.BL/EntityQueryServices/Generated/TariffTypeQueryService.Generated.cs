 
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
   public partial class TariffTypeQueryService: EntityQueryService<TariffType,TariffTypeKeys,TariffTypePM,object,TariffTypeKeys>
   {
   
        TariffTypeRepository repository;
		ITariffModuleContext  context;
        public TariffTypeQueryService(int tenant)
        {
		    context = TariffModuleContext.GetContext(tenant);
            MainContext = context;
            repository = new TariffTypeRepository(context);
            Repository = repository;
            mapping = new TariffTypeDataMapping();
        }

        public TariffTypeQueryService(TariffTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TariffTypeDataMapping();
        }

        public TariffTypeQueryService(ITariffModuleContext context)
        {
            this.repository = new TariffTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TariffTypeDataMapping();
        }
		 
		public  TariffTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TariffTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TariffType entityPOCO)
        {
            TariffTypeKeys entityKeys = new TariffTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 