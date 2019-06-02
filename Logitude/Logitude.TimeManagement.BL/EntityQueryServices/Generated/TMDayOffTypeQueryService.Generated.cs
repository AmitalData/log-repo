 
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
   public partial class TMDayOffTypeQueryService: EntityQueryService<TMDayOffType,TMDayOffTypeKeys,TMDayOffTypePM,object,TMDayOffTypeKeys>
   {
   
        TMDayOffTypeRepository repository;
		ITimeManagementContext  context;
        public TMDayOffTypeQueryService(int tenant)
        {
		    context = TimeManagementContext.GetContext(tenant);
            MainContext = context;
            repository = new TMDayOffTypeRepository(context);
            Repository = repository;
            mapping = new TMDayOffTypeDataMapping();
        }

        public TMDayOffTypeQueryService(TMDayOffTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TMDayOffTypeDataMapping();
        }

        public TMDayOffTypeQueryService(ITimeManagementContext context)
        {
            this.repository = new TMDayOffTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TMDayOffTypeDataMapping();
        }
		 
		public  TMDayOffTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TMDayOffTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TMDayOffType entityPOCO)
        {
            TMDayOffTypeKeys entityKeys = new TMDayOffTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 