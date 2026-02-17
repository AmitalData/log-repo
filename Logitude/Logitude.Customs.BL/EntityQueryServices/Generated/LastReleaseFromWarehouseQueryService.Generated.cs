 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Customs.BL.EntityQueryServices
{ 
   public partial class LastReleaseFromWarehouseQueryService: EntityQueryService<LastReleaseFromWarehouse,LastReleaseFromWarehouseKeys,LastReleaseFromWarehousePM,object,LastReleaseFromWarehouseKeys>
   {
   
        LastReleaseFromWarehouseRepository repository;
		ICustomContext  context;
        public LastReleaseFromWarehouseQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new LastReleaseFromWarehouseRepository(context);
            Repository = repository;
            mapping = new LastReleaseFromWarehouseDataMapping();
        }

        public LastReleaseFromWarehouseQueryService(LastReleaseFromWarehouseRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new LastReleaseFromWarehouseDataMapping();
        }

        public LastReleaseFromWarehouseQueryService(ICustomContext context)
        {
            this.repository = new LastReleaseFromWarehouseRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new LastReleaseFromWarehouseDataMapping();
        }
		 
		public  LastReleaseFromWarehousePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new LastReleaseFromWarehouseKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(LastReleaseFromWarehouse entityPOCO)
        {
            LastReleaseFromWarehouseKeys entityKeys = new LastReleaseFromWarehouseKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 