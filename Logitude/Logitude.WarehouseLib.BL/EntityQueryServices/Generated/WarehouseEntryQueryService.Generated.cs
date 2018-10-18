 
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
   public partial class WarehouseEntryQueryService: EntityQueryService<WarehouseEntry,WarehouseEntryKeys,WarehouseEntryPM,object,WarehouseEntryKeys>
   {
   
        WarehouseEntryRepository repository;
		IWarehouseContext  context;
        public WarehouseEntryQueryService(int tenant)
        {
		    context = WarehouseContext.GetContext(tenant);
            MainContext = context;
            repository = new WarehouseEntryRepository(context);
            Repository = repository;
            mapping = new WarehouseEntryDataMapping();
        }

        public WarehouseEntryQueryService(WarehouseEntryRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new WarehouseEntryDataMapping();
        }

        public WarehouseEntryQueryService(IWarehouseContext context)
        {
            this.repository = new WarehouseEntryRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new WarehouseEntryDataMapping();
        }
		 
		public  WarehouseEntryPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new WarehouseEntryKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(WarehouseEntry entityPOCO)
        {
            WarehouseEntryKeys entityKeys = new WarehouseEntryKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 