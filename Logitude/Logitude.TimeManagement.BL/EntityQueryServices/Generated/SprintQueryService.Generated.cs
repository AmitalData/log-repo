 
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
   public partial class SprintQueryService: EntityQueryService<Sprint,SprintKeys,SprintPM,object,SprintKeys>
   {
   
        SprintRepository repository;
		ITimeManagementContext  context;
        public SprintQueryService(int tenant)
        {
		    context = TimeManagementContext.GetContext(tenant);
            MainContext = context;
            repository = new SprintRepository(context);
            Repository = repository;
            mapping = new SprintDataMapping();
        }

        public SprintQueryService(SprintRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new SprintDataMapping();
        }

        public SprintQueryService(ITimeManagementContext context)
        {
            this.repository = new SprintRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new SprintDataMapping();
        }
		 
		public  SprintPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new SprintKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Sprint entityPOCO)
        {
            SprintKeys entityKeys = new SprintKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 