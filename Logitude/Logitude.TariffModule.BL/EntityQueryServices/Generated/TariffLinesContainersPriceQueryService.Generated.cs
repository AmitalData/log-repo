 
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
   public partial class TariffLinesContainersPriceQueryService: EntityQueryService<TariffLinesContainersPrice,TariffLinesContainersPriceKeys,TariffLinesContainersPricePM,TariffLinePM,TariffLineKeys>
   {
   
        TariffLinesContainersPriceRepository repository;
		ITariffModuleContext  context;
        public TariffLinesContainersPriceQueryService(int tenant)
        {
		    context = TariffModuleContext.GetContext(tenant);
            MainContext = context;
            repository = new TariffLinesContainersPriceRepository(context);
            Repository = repository;
            mapping = new TariffLinesContainersPriceDataMapping();
        }

        public TariffLinesContainersPriceQueryService(TariffLinesContainersPriceRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TariffLinesContainersPriceDataMapping();
        }

        public TariffLinesContainersPriceQueryService(ITariffModuleContext context)
        {
            this.repository = new TariffLinesContainersPriceRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TariffLinesContainersPriceDataMapping();
        }
		 
		public  TariffLinesContainersPricePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TariffLinesContainersPriceKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TariffLinesContainersPrice entityPOCO)
        {
            TariffLinesContainersPriceKeys entityKeys = new TariffLinesContainersPriceKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 