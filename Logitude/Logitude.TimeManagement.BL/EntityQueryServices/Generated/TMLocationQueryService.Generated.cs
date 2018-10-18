 
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
   public partial class TMLocationQueryService: EntityQueryService<TMLocation,TMLocationKeys,TMLocationPM,object,TMLocationKeys>
   {
   
        TMLocationRepository repository;
		ITimeManagementContext  context;
        public TMLocationQueryService(int tenant)
        {
		    context = TimeManagementContext.GetContext(tenant);
            MainContext = context;
            repository = new TMLocationRepository(context);
            Repository = repository;
            mapping = new TMLocationDataMapping();
        }

        public TMLocationQueryService(TMLocationRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TMLocationDataMapping();
        }

        public TMLocationQueryService(ITimeManagementContext context)
        {
            this.repository = new TMLocationRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TMLocationDataMapping();
        }
		 
		public  TMLocationPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TMLocationKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TMLocation entityPOCO)
        {
            TMLocationKeys entityKeys = new TMLocationKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 