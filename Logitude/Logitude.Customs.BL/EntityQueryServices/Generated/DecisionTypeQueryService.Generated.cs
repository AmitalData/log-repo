 
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
   public partial class DecisionTypeQueryService: EntityQueryService<DecisionType,DecisionTypeKeys,DecisionTypePM,object,DecisionTypeKeys>
   {
   
        DecisionTypeRepository repository;
		ICustomContext  context;
        public DecisionTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new DecisionTypeRepository(context);
            Repository = repository;
            mapping = new DecisionTypeDataMapping();
        }

        public DecisionTypeQueryService(DecisionTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DecisionTypeDataMapping();
        }

        public DecisionTypeQueryService(ICustomContext context)
        {
            this.repository = new DecisionTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DecisionTypeDataMapping();
        }
		 
		public  DecisionTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DecisionTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DecisionType entityPOCO)
        {
            DecisionTypeKeys entityKeys = new DecisionTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 