 
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
   public partial class WarehouseEntryPackagesReleaseQueryService: EntityQueryService<WarehouseEntryPackagesRelease,WarehouseEntryPackagesReleaseKeys,WarehouseEntryPackagesReleasePM,object,WarehouseEntryPackagesReleaseKeys>
   {
   
        WarehouseEntryPackagesReleaseRepository repository;
		IWarehouseContext  context;
        public WarehouseEntryPackagesReleaseQueryService(int tenant)
        {
		    context = WarehouseContext.GetContext(tenant);
            MainContext = context;
            repository = new WarehouseEntryPackagesReleaseRepository(context);
            Repository = repository;
            mapping = new WarehouseEntryPackagesReleaseDataMapping();
        }

        public WarehouseEntryPackagesReleaseQueryService(WarehouseEntryPackagesReleaseRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new WarehouseEntryPackagesReleaseDataMapping();
        }

        public WarehouseEntryPackagesReleaseQueryService(IWarehouseContext context)
        {
            this.repository = new WarehouseEntryPackagesReleaseRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new WarehouseEntryPackagesReleaseDataMapping();
        }
		 
		public  WarehouseEntryPackagesReleasePM GetSingle(string entrypackageid, string releasepackageid,bool getComposition, bool getFromCache)
        {
             EntityKeys = new WarehouseEntryPackagesReleaseKeys(){ EntryPackageId = entrypackageid, ReleasePackageId = releasepackageid };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(WarehouseEntryPackagesRelease entityPOCO)
        {
            WarehouseEntryPackagesReleaseKeys entityKeys = new WarehouseEntryPackagesReleaseKeys() { EntryPackageId = entityPOCO.EntryPackageId, ReleasePackageId = entityPOCO.ReleasePackageId,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 