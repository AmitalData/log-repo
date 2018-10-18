 
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
   public partial class TMEmployeeTimeQueryService: EntityQueryService<TMEmployeeTime,TMEmployeeTimeKeys,TMEmployeeTimePM,object,TMEmployeeTimeKeys>
   {
   
        TMEmployeeTimeRepository repository;
		ITimeManagementContext  context;
        public TMEmployeeTimeQueryService(int tenant)
        {
		    context = TimeManagementContext.GetContext(tenant);
            MainContext = context;
            repository = new TMEmployeeTimeRepository(context);
            Repository = repository;
            mapping = new TMEmployeeTimeDataMapping();
        }

        public TMEmployeeTimeQueryService(TMEmployeeTimeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TMEmployeeTimeDataMapping();
        }

        public TMEmployeeTimeQueryService(ITimeManagementContext context)
        {
            this.repository = new TMEmployeeTimeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TMEmployeeTimeDataMapping();
        }
		 
		public  TMEmployeeTimePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TMEmployeeTimeKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TMEmployeeTime entityPOCO)
        {
            TMEmployeeTimeKeys entityKeys = new TMEmployeeTimeKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 