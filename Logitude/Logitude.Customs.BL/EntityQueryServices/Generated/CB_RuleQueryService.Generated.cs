 
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
   public partial class CB_RuleQueryService: EntityQueryService<CB_Rule,CB_RuleKeys,CB_RulePM,CB_CustomsItemPM,CB_CustomsItemKeys>
   {
   
        CB_RuleRepository repository;
		ICustomContext  context;
        public CB_RuleQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CB_RuleRepository(context);
            Repository = repository;
            mapping = new CB_RuleDataMapping();
        }

        public CB_RuleQueryService(CB_RuleRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CB_RuleDataMapping();
        }

        public CB_RuleQueryService(ICustomContext context)
        {
            this.repository = new CB_RuleRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CB_RuleDataMapping();
        }
		 
		public  CB_RulePM GetSingle(int id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CB_RuleKeys(){ ID = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CB_Rule entityPOCO)
        {
            CB_RuleKeys entityKeys = new CB_RuleKeys() { ID = entityPOCO.ID,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 