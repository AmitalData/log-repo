 
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
   public partial class CB_TariffDetailsHistoryQueryService: EntityQueryService<CB_TariffDetailsHistory,CB_TariffDetailsHistoryKeys,CB_TariffDetailsHistoryPM,CB_TariffPM,CB_TariffKeys>
   {
   
        CB_TariffDetailsHistoryRepository repository;
		ICustomContext  context;
        public CB_TariffDetailsHistoryQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CB_TariffDetailsHistoryRepository(context);
            Repository = repository;
            mapping = new CB_TariffDetailsHistoryDataMapping();
        }

        public CB_TariffDetailsHistoryQueryService(CB_TariffDetailsHistoryRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CB_TariffDetailsHistoryDataMapping();
        }

        public CB_TariffDetailsHistoryQueryService(ICustomContext context)
        {
            this.repository = new CB_TariffDetailsHistoryRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CB_TariffDetailsHistoryDataMapping();
        }
		 
		public  CB_TariffDetailsHistoryPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CB_TariffDetailsHistoryKeys(){ ID = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CB_TariffDetailsHistory entityPOCO)
        {
            CB_TariffDetailsHistoryKeys entityKeys = new CB_TariffDetailsHistoryKeys() { ID = entityPOCO.ID,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 