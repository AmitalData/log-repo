 
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
   public partial class TMBudgetQueryService: EntityQueryService<TMBudget,TMBudgetKeys,TMBudgetPM,object,TMBudgetKeys>
   {
   
        TMBudgetRepository repository;
		ITimeManagementContext  context;
        public TMBudgetQueryService(int tenant)
        {
		    context = TimeManagementContext.GetContext(tenant);
            MainContext = context;
            repository = new TMBudgetRepository(context);
            Repository = repository;
            mapping = new TMBudgetDataMapping();
        }

        public TMBudgetQueryService(TMBudgetRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TMBudgetDataMapping();
        }

        public TMBudgetQueryService(ITimeManagementContext context)
        {
            this.repository = new TMBudgetRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TMBudgetDataMapping();
        }
		 
		public  TMBudgetPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TMBudgetKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TMBudget entityPOCO)
        {
            TMBudgetKeys entityKeys = new TMBudgetKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 