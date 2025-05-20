 
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
   public partial class CB_RuleClassificationQueryService: EntityQueryService<CB_RuleClassification,CB_RuleClassificationKeys,CB_RuleClassificationPM,CB_RuleClassificationPM,CB_RuleClassificationKeys>
   {
   
        CB_RuleClassificationRepository repository;
		ICustomContext  context;
        public CB_RuleClassificationQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CB_RuleClassificationRepository(context);
            Repository = repository;
            mapping = new CB_RuleClassificationDataMapping();
        }

        public CB_RuleClassificationQueryService(CB_RuleClassificationRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CB_RuleClassificationDataMapping();
        }

        public CB_RuleClassificationQueryService(ICustomContext context)
        {
            this.repository = new CB_RuleClassificationRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CB_RuleClassificationDataMapping();
        }
		 
		public  CB_RuleClassificationPM GetSingle(string cb_id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CB_RuleClassificationKeys(){ CB_ID = cb_id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CB_RuleClassification entityPOCO)
        {
            CB_RuleClassificationKeys entityKeys = new CB_RuleClassificationKeys() { CB_ID = entityPOCO.CB_ID,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 