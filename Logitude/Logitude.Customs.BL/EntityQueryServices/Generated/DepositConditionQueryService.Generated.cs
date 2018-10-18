 
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
   public partial class DepositConditionQueryService: EntityQueryService<DepositCondition,DepositConditionKeys,DepositConditionPM,DepositPM,DepositKeys>
   {
   
        DepositConditionRepository repository;
		ICustomContext  context;
        public DepositConditionQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new DepositConditionRepository(context);
            Repository = repository;
            mapping = new DepositConditionDataMapping();
        }

        public DepositConditionQueryService(DepositConditionRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DepositConditionDataMapping();
        }

        public DepositConditionQueryService(ICustomContext context)
        {
            this.repository = new DepositConditionRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DepositConditionDataMapping();
        }
		 
		public  DepositConditionPM GetSingle(string depositid, string depositconditioncode,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DepositConditionKeys(){ DepositId = depositid, DepositConditionCode = depositconditioncode };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DepositCondition entityPOCO)
        {
            DepositConditionKeys entityKeys = new DepositConditionKeys() { DepositId = entityPOCO.DepositId, DepositConditionCode = entityPOCO.DepositConditionCode,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 