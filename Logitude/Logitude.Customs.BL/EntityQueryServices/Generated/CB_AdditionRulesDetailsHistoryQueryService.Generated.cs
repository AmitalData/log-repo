 
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
   public partial class CB_AdditionRulesDetailsHistoryQueryService: EntityQueryService<CB_AdditionRulesDetailsHistory,CB_AdditionRulesDetailsHistoryKeys,CB_AdditionRulesDetailsHistoryPM,object,CB_AdditionRulesDetailsHistoryKeys>
   {
   
        CB_AdditionRulesDetailsHistoryRepository repository;
		ICustomContext  context;
        public CB_AdditionRulesDetailsHistoryQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CB_AdditionRulesDetailsHistoryRepository(context);
            Repository = repository;
            mapping = new CB_AdditionRulesDetailsHistoryDataMapping();
        }

        public CB_AdditionRulesDetailsHistoryQueryService(CB_AdditionRulesDetailsHistoryRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CB_AdditionRulesDetailsHistoryDataMapping();
        }

        public CB_AdditionRulesDetailsHistoryQueryService(ICustomContext context)
        {
            this.repository = new CB_AdditionRulesDetailsHistoryRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CB_AdditionRulesDetailsHistoryDataMapping();
        }
		 
		public  CB_AdditionRulesDetailsHistoryPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CB_AdditionRulesDetailsHistoryKeys(){ ID = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CB_AdditionRulesDetailsHistory entityPOCO)
        {
            CB_AdditionRulesDetailsHistoryKeys entityKeys = new CB_AdditionRulesDetailsHistoryKeys() { ID = entityPOCO.ID,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 