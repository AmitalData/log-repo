 
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
   public partial class WarehouseEntryStatusQueryService: EntityQueryService<WarehouseEntryStatus,WarehouseEntryStatusKeys,WarehouseEntryStatusPM,object,WarehouseEntryStatusKeys>
   {
   
        WarehouseEntryStatusRepository repository;
		IWarehouseContext  context;
        public WarehouseEntryStatusQueryService(int tenant)
        {
		    context = WarehouseContext.GetContext(tenant);
            MainContext = context;
            repository = new WarehouseEntryStatusRepository(context);
            Repository = repository;
            mapping = new WarehouseEntryStatusDataMapping();
        }

        public WarehouseEntryStatusQueryService(WarehouseEntryStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new WarehouseEntryStatusDataMapping();
        }

        public WarehouseEntryStatusQueryService(IWarehouseContext context)
        {
            this.repository = new WarehouseEntryStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new WarehouseEntryStatusDataMapping();
        }
		 
		public  WarehouseEntryStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new WarehouseEntryStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(WarehouseEntryStatus entityPOCO)
        {
            WarehouseEntryStatusKeys entityKeys = new WarehouseEntryStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 