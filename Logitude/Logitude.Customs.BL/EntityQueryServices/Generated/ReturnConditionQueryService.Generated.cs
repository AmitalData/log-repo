 
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
   public partial class ReturnConditionQueryService: EntityQueryService<ReturnCondition,ReturnConditionKeys,ReturnConditionPM,object,ReturnConditionKeys>
   {
   
        ReturnConditionRepository repository;
		ICustomContext  context;
        public ReturnConditionQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ReturnConditionRepository(context);
            Repository = repository;
            mapping = new ReturnConditionDataMapping();
        }

        public ReturnConditionQueryService(ReturnConditionRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ReturnConditionDataMapping();
        }

        public ReturnConditionQueryService(ICustomContext context)
        {
            this.repository = new ReturnConditionRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ReturnConditionDataMapping();
        }
		 
		public  ReturnConditionPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ReturnConditionKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ReturnCondition entityPOCO)
        {
            ReturnConditionKeys entityKeys = new ReturnConditionKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 