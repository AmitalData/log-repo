 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.WarehouseLib.BL.EntityPMs;
using Logitude.WarehouseLib.BL.EntityDataMappings;
using Logitude.WarehouseLib.Data.Repositories;
using Logitude.WarehouseLib.Data.EntityKeys;
using Logitude.WarehouseLib.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.WarehouseLib.BL.EntityQueryServices
{ 
   public partial class WarehouseReleaseQueryService: EntityQueryService<WarehouseRelease,WarehouseReleaseKeys,WarehouseReleasePM,object,WarehouseReleaseKeys>
   {
   
        WarehouseReleaseRepository repository;
		IWarehouseContext  context;
        public WarehouseReleaseQueryService(int tenant)
        {
		    context = WarehouseContext.GetContext(tenant);
            MainContext = context;
            repository = new WarehouseReleaseRepository(context);
            Repository = repository;
            mapping = new WarehouseReleaseDataMapping();
        }

        public WarehouseReleaseQueryService(WarehouseReleaseRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new WarehouseReleaseDataMapping();
        }

        public WarehouseReleaseQueryService(IWarehouseContext context)
        {
            this.repository = new WarehouseReleaseRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new WarehouseReleaseDataMapping();
        }
		 
		public  WarehouseReleasePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new WarehouseReleaseKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(WarehouseRelease entityPOCO)
        {
            WarehouseReleaseKeys entityKeys = new WarehouseReleaseKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 