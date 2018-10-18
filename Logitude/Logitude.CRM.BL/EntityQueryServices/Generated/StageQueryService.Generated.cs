 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityDataMappings;
using Logitude.CRM.Data.Repsitories;
using Logitude.CRM.Data.EntityKeys;
using Logitude.CRM.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.CRM.BL.EntityQueryServices
{ 
   public partial class StageQueryService: EntityQueryService<Stage,StageKeys,StagePM,object,StageKeys>
   {
   
        StageRepository repository;
		ICRMContext  context;
        public StageQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new StageRepository(context);
            Repository = repository;
            mapping = new StageDataMapping();
        }

        public StageQueryService(StageRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new StageDataMapping();
        }

        public StageQueryService(ICRMContext context)
        {
            this.repository = new StageRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new StageDataMapping();
        }
		 
		public  StagePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new StageKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Stage entityPOCO)
        {
            StageKeys entityKeys = new StageKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 