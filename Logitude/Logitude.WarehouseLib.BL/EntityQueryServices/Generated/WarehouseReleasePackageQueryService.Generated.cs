 
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
   public partial class WarehouseReleasePackageQueryService: EntityQueryService<WarehouseReleasePackage,WarehouseReleasePackageKeys,WarehouseReleasePackagePM,WarehouseReleasePM,WarehouseReleaseKeys>
   {
   
        WarehouseReleasePackageRepository repository;
		IWarehouseContext  context;
        public WarehouseReleasePackageQueryService(int tenant)
        {
		    context = WarehouseContext.GetContext(tenant);
            MainContext = context;
            repository = new WarehouseReleasePackageRepository(context);
            Repository = repository;
            mapping = new WarehouseReleasePackageDataMapping();
        }

        public WarehouseReleasePackageQueryService(WarehouseReleasePackageRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new WarehouseReleasePackageDataMapping();
        }

        public WarehouseReleasePackageQueryService(IWarehouseContext context)
        {
            this.repository = new WarehouseReleasePackageRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new WarehouseReleasePackageDataMapping();
        }
		 
		public  WarehouseReleasePackagePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new WarehouseReleasePackageKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(WarehouseReleasePackage entityPOCO)
        {
            WarehouseReleasePackageKeys entityKeys = new WarehouseReleasePackageKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 