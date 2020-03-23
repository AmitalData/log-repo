 
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
   public partial class TariffProductQueryService: EntityQueryService<TariffProduct,TariffProductKeys,TariffProductPM,object,TariffProductKeys>
   {
   
        TariffProductRepository repository;
		ITariffModuleContext  context;
        public TariffProductQueryService(int tenant)
        {
		    context = TariffModuleContext.GetContext(tenant);
            MainContext = context;
            repository = new TariffProductRepository(context);
            Repository = repository;
            mapping = new TariffProductDataMapping();
        }

        public TariffProductQueryService(TariffProductRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TariffProductDataMapping();
        }

        public TariffProductQueryService(ITariffModuleContext context)
        {
            this.repository = new TariffProductRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TariffProductDataMapping();
        }
		 
		public  TariffProductPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TariffProductKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TariffProduct entityPOCO)
        {
            TariffProductKeys entityKeys = new TariffProductKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 