 
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
   public partial class WarehouseEntryPackageQueryService: EntityQueryService<WarehouseEntryPackage,WarehouseEntryPackageKeys,WarehouseEntryPackagePM,WarehouseEntryPM,WarehouseEntryKeys>
   {
   
        WarehouseEntryPackageRepository repository;
		IWarehouseContext  context;
        public WarehouseEntryPackageQueryService(int tenant)
        {
		    context = WarehouseContext.GetContext(tenant);
            MainContext = context;
            repository = new WarehouseEntryPackageRepository(context);
            Repository = repository;
            mapping = new WarehouseEntryPackageDataMapping();
        }

        public WarehouseEntryPackageQueryService(WarehouseEntryPackageRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new WarehouseEntryPackageDataMapping();
        }

        public WarehouseEntryPackageQueryService(IWarehouseContext context)
        {
            this.repository = new WarehouseEntryPackageRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new WarehouseEntryPackageDataMapping();
        }
		 
		public  WarehouseEntryPackagePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new WarehouseEntryPackageKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(WarehouseEntryPackage entityPOCO)
        {
            WarehouseEntryPackageKeys entityKeys = new WarehouseEntryPackageKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 