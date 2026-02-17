 
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
   public partial class CollateralAnswerStatusQueryService: EntityQueryService<CollateralAnswerStatus,CollateralAnswerStatusKeys,CollateralAnswerStatusPM,object,CollateralAnswerStatusKeys>
   {
   
        CollateralAnswerStatusRepository repository;
		ICustomContext  context;
        public CollateralAnswerStatusQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CollateralAnswerStatusRepository(context);
            Repository = repository;
            mapping = new CollateralAnswerStatusDataMapping();
        }

        public CollateralAnswerStatusQueryService(CollateralAnswerStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CollateralAnswerStatusDataMapping();
        }

        public CollateralAnswerStatusQueryService(ICustomContext context)
        {
            this.repository = new CollateralAnswerStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CollateralAnswerStatusDataMapping();
        }
		 
		public  CollateralAnswerStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CollateralAnswerStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CollateralAnswerStatus entityPOCO)
        {
            CollateralAnswerStatusKeys entityKeys = new CollateralAnswerStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 