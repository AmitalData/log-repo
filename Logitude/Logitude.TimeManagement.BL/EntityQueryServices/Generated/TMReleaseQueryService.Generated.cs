 
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
   public partial class TMReleaseQueryService: EntityQueryService<TMRelease,TMReleaseKeys,TMReleasePM,object,TMReleaseKeys>
   {
   
        TMReleaseRepository repository;
		ITimeManagementContext  context;
        public TMReleaseQueryService(int tenant)
        {
		    context = TimeManagementContext.GetContext(tenant);
            MainContext = context;
            repository = new TMReleaseRepository(context);
            Repository = repository;
            mapping = new TMReleaseDataMapping();
        }

        public TMReleaseQueryService(TMReleaseRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TMReleaseDataMapping();
        }

        public TMReleaseQueryService(ITimeManagementContext context)
        {
            this.repository = new TMReleaseRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TMReleaseDataMapping();
        }
		 
		public  TMReleasePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TMReleaseKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TMRelease entityPOCO)
        {
            TMReleaseKeys entityKeys = new TMReleaseKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 