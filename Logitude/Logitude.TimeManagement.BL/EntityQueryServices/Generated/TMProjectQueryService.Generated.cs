 
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
   public partial class TMProjectQueryService: EntityQueryService<TMProject,TMProjectKeys,TMProjectPM,object,TMProjectKeys>
   {
   
        TMProjectRepository repository;
		ITimeManagementContext  context;
        public TMProjectQueryService(int tenant)
        {
		    context = TimeManagementContext.GetContext(tenant);
            MainContext = context;
            repository = new TMProjectRepository(context);
            Repository = repository;
            mapping = new TMProjectDataMapping();
        }

        public TMProjectQueryService(TMProjectRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TMProjectDataMapping();
        }

        public TMProjectQueryService(ITimeManagementContext context)
        {
            this.repository = new TMProjectRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TMProjectDataMapping();
        }
		 
		public  TMProjectPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TMProjectKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TMProject entityPOCO)
        {
            TMProjectKeys entityKeys = new TMProjectKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 