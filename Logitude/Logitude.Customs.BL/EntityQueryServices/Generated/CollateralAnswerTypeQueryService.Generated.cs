 
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
   public partial class CollateralAnswerTypeQueryService: EntityQueryService<CollateralAnswerType,CollateralAnswerTypeKeys,CollateralAnswerTypePM,object,CollateralAnswerTypeKeys>
   {
   
        CollateralAnswerTypeRepository repository;
		ICustomContext  context;
        public CollateralAnswerTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CollateralAnswerTypeRepository(context);
            Repository = repository;
            mapping = new CollateralAnswerTypeDataMapping();
        }

        public CollateralAnswerTypeQueryService(CollateralAnswerTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CollateralAnswerTypeDataMapping();
        }

        public CollateralAnswerTypeQueryService(ICustomContext context)
        {
            this.repository = new CollateralAnswerTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CollateralAnswerTypeDataMapping();
        }
		 
		public  CollateralAnswerTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CollateralAnswerTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CollateralAnswerType entityPOCO)
        {
            CollateralAnswerTypeKeys entityKeys = new CollateralAnswerTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 