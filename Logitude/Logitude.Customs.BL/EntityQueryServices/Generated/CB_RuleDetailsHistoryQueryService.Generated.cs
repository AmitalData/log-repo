 
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
   public partial class CB_RuleDetailsHistoryQueryService: EntityQueryService<CB_RuleDetailsHistory,CB_RuleDetailsHistoryKeys,CB_RuleDetailsHistoryPM,CB_RulePM,CB_RuleKeys>
   {
   
        CB_RuleDetailsHistoryRepository repository;
		ICustomContext  context;
        public CB_RuleDetailsHistoryQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CB_RuleDetailsHistoryRepository(context);
            Repository = repository;
            mapping = new CB_RuleDetailsHistoryDataMapping();
        }

        public CB_RuleDetailsHistoryQueryService(CB_RuleDetailsHistoryRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CB_RuleDetailsHistoryDataMapping();
        }

        public CB_RuleDetailsHistoryQueryService(ICustomContext context)
        {
            this.repository = new CB_RuleDetailsHistoryRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CB_RuleDetailsHistoryDataMapping();
        }
		 
		public  CB_RuleDetailsHistoryPM GetSingle(int id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CB_RuleDetailsHistoryKeys(){ ID = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CB_RuleDetailsHistory entityPOCO)
        {
            CB_RuleDetailsHistoryKeys entityKeys = new CB_RuleDetailsHistoryKeys() { ID = entityPOCO.ID,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 