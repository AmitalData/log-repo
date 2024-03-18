 
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
   public partial class CB_QuotaDetailsHistoryQueryService: EntityQueryService<CB_QuotaDetailsHistory,CB_QuotaDetailsHistoryKeys,CB_QuotaDetailsHistoryPM,object,CB_QuotaDetailsHistoryKeys>
   {
   
        CB_QuotaDetailsHistoryRepository repository;
		ICustomContext  context;
        public CB_QuotaDetailsHistoryQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CB_QuotaDetailsHistoryRepository(context);
            Repository = repository;
            mapping = new CB_QuotaDetailsHistoryDataMapping();
        }

        public CB_QuotaDetailsHistoryQueryService(CB_QuotaDetailsHistoryRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CB_QuotaDetailsHistoryDataMapping();
        }

        public CB_QuotaDetailsHistoryQueryService(ICustomContext context)
        {
            this.repository = new CB_QuotaDetailsHistoryRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CB_QuotaDetailsHistoryDataMapping();
        }
		 
		public  CB_QuotaDetailsHistoryPM GetSingle(int id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CB_QuotaDetailsHistoryKeys(){ ID = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CB_QuotaDetailsHistory entityPOCO)
        {
            CB_QuotaDetailsHistoryKeys entityKeys = new CB_QuotaDetailsHistoryKeys() { ID = entityPOCO.ID,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 