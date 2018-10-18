 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.BL.EntityPMs;
using Logitude.TimeManagement.BL.EntityDataMappings;
using Logitude.TimeManagement.Data.Repositories;
using Logitude.TimeManagement.Data.EntityKeys;
using Logitude.TimeManagement.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.TimeManagement.BL.EntityQueryServices
{ 
   public partial class TMProjectCategoryQueryService: EntityQueryService<TMProjectCategory,TMProjectCategoryKeys,TMProjectCategoryPM,object,TMProjectCategoryKeys>
   {
   
        TMProjectCategoryRepository repository;
		ITimeManagementContext  context;
        public TMProjectCategoryQueryService(int tenant)
        {
		    context = TimeManagementContext.GetContext(tenant);
            MainContext = context;
            repository = new TMProjectCategoryRepository(context);
            Repository = repository;
            mapping = new TMProjectCategoryDataMapping();
        }

        public TMProjectCategoryQueryService(TMProjectCategoryRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TMProjectCategoryDataMapping();
        }

        public TMProjectCategoryQueryService(ITimeManagementContext context)
        {
            this.repository = new TMProjectCategoryRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TMProjectCategoryDataMapping();
        }
		 
		public  TMProjectCategoryPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TMProjectCategoryKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TMProjectCategory entityPOCO)
        {
            TMProjectCategoryKeys entityKeys = new TMProjectCategoryKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 