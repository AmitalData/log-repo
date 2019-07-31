 
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
   public partial class TariffLineQueryService: EntityQueryService<TariffLine,TariffLineKeys,TariffLinePM,TariffVersionPM,TariffVersionKeys>
   {
   
        TariffLineRepository repository;
		ITariffModuleContext  context;
        public TariffLineQueryService(int tenant)
        {
		    context = TariffModuleContext.GetContext(tenant);
            MainContext = context;
            repository = new TariffLineRepository(context);
            Repository = repository;
            mapping = new TariffLineDataMapping();
        }

        public TariffLineQueryService(TariffLineRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TariffLineDataMapping();
        }

        public TariffLineQueryService(ITariffModuleContext context)
        {
            this.repository = new TariffLineRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TariffLineDataMapping();
        }
		 
		public  TariffLinePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TariffLineKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TariffLine entityPOCO)
        {
            TariffLineKeys entityKeys = new TariffLineKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 