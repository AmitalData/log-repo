 
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
   public partial class TMOfficeHourQueryService: EntityQueryService<TMOfficeHour,TMOfficeHourKeys,TMOfficeHourPM,object,TMOfficeHourKeys>
   {
   
        TMOfficeHourRepository repository;
		ITimeManagementContext  context;
        public TMOfficeHourQueryService(int tenant)
        {
		    context = TimeManagementContext.GetContext(tenant);
            MainContext = context;
            repository = new TMOfficeHourRepository(context);
            Repository = repository;
            mapping = new TMOfficeHourDataMapping();
        }

        public TMOfficeHourQueryService(TMOfficeHourRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TMOfficeHourDataMapping();
        }

        public TMOfficeHourQueryService(ITimeManagementContext context)
        {
            this.repository = new TMOfficeHourRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TMOfficeHourDataMapping();
        }
		 
		public  TMOfficeHourPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TMOfficeHourKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TMOfficeHour entityPOCO)
        {
            TMOfficeHourKeys entityKeys = new TMOfficeHourKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 