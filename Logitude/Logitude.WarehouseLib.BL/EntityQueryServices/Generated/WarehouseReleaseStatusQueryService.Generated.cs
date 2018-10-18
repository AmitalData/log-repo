 
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
   public partial class WarehouseReleaseStatusQueryService: EntityQueryService<WarehouseReleaseStatus,WarehouseReleaseStatusKeys,WarehouseReleaseStatusPM,object,WarehouseReleaseStatusKeys>
   {
   
        WarehouseReleaseStatusRepository repository;
		IWarehouseContext  context;
        public WarehouseReleaseStatusQueryService(int tenant)
        {
		    context = WarehouseContext.GetContext(tenant);
            MainContext = context;
            repository = new WarehouseReleaseStatusRepository(context);
            Repository = repository;
            mapping = new WarehouseReleaseStatusDataMapping();
        }

        public WarehouseReleaseStatusQueryService(WarehouseReleaseStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new WarehouseReleaseStatusDataMapping();
        }

        public WarehouseReleaseStatusQueryService(IWarehouseContext context)
        {
            this.repository = new WarehouseReleaseStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new WarehouseReleaseStatusDataMapping();
        }
		 
		public  WarehouseReleaseStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new WarehouseReleaseStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(WarehouseReleaseStatus entityPOCO)
        {
            WarehouseReleaseStatusKeys entityKeys = new WarehouseReleaseStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 